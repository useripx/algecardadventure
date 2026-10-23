using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace AlgeCardAdventure.AI
{
    [Serializable]
    public class ChatMessage
    {
        public string role;
        public string content;

        public ChatMessage(string r, string c)
        {
            role = r;
            content = c;
        }
    }

    [Serializable]
    public class GroqChatRequest
    {
        public string model;
        public List<ChatMessage> messages = new List<ChatMessage>();
        public float temperature = 0.6f;
        public int max_tokens = 512;
    }

    [Serializable]
    public class GroqChoiceMessage
    {
        public string role;
        public string content;
    }

    [Serializable]
    public class GroqChoice
    {
        public int index;
        public GroqChoiceMessage message;
        public string finish_reason;
    }

    [Serializable]
    public class GroqChatResponse
    {
        public string id;
        public string model;
        public List<GroqChoice> choices;
    }

    /// <summary>
    /// Klien HTTP asinkron untuk berinteraksi dengan Groq Cloud LLM API.
    /// Mendukung fallback otomatis ke model ringan llama-3.1-8b-instant jika diperlukan.
    /// </summary>
    public class GroqApiClient : MonoBehaviour
    {
        private const string GroqEndpoint = "https://api.groq.com/openai/v1/chat/completions";

        public static GroqApiClient Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Mengirim permintaan obrolan ke Groq Cloud API melalui Coroutine.
        /// </summary>
        public void SendChatCompletion(
            List<ChatMessage> conversationHistory,
            Action<string> onSuccess,
            Action<string> onError)
        {
            StartCoroutine(PostChatRoutine(conversationHistory, onSuccess, onError));
        }

        private IEnumerator PostChatRoutine(
            List<ChatMessage> conversationHistory,
            Action<string> onSuccess,
            Action<string> onError)
        {
            var config = GroqConfigManager.GetConfig();
            if (string.IsNullOrEmpty(config.api_key))
            {
                onError?.Invoke("Groq API Key tidak ditemukan. Pastikan konfigurasi aktif.");
                yield break;
            }

            // Periksa ketersediaan koneksi internet perangkat
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                onError?.Invoke("OFFLINE_NETWORK_UNAVAILABLE");
                yield break;
            }

            var requestPayload = new GroqChatRequest
            {
                model = config.model,
                messages = conversationHistory,
                temperature = config.temperature,
                max_tokens = config.max_tokens
            };

            string jsonBody = SerializeRequest(requestPayload);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

            using (UnityWebRequest webRequest = new UnityWebRequest(GroqEndpoint, "POST"))
            {
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SetRequestHeader("Authorization", "Bearer " + config.api_key.Trim());
                webRequest.timeout = 10; // 10 detik batas toleransi koneksi mobile

                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    string responseText = webRequest.downloadHandler.text;
                    string reply = ParseResponseContent(responseText);
                    if (!string.IsNullOrEmpty(reply))
                    {
                        onSuccess?.Invoke(reply);
                    }
                    else
                    {
                        onError?.Invoke("Respons Groq diterima namun isi konten kosong.");
                    }
                }
                else
                {
                    string errorMsg = $"HTTP Error ({webRequest.responseCode}): {webRequest.error}";
                    Debug.LogWarning($"[GroqApiClient] Request gagal: {errorMsg}");
                    onError?.Invoke(errorMsg);
                }
            }
        }

        public static string SerializeRequest(GroqChatRequest request)
        {
            var sb = new StringBuilder();
            sb.Append("{");
            sb.Append($"\"model\":\"{EscapeJson(request.model)}\",");
            sb.Append($"\"temperature\":{request.temperature.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture)},");
            sb.Append($"\"max_tokens\":{request.max_tokens},");
            sb.Append("\"messages\":[");

            for (int i = 0; i < request.messages.Count; i++)
            {
                var msg = request.messages[i];
                sb.Append("{");
                sb.Append($"\"role\":\"{EscapeJson(msg.role)}\",");
                sb.Append($"\"content\":\"{EscapeJson(msg.content)}\"");
                sb.Append("}");
                if (i < request.messages.Count - 1) sb.Append(",");
            }

            sb.Append("]}");
            return sb.ToString();
        }

        public static string ParseResponseContent(string jsonResponse)
        {
            if (string.IsNullOrEmpty(jsonResponse)) return "";

            try
            {
                // Ekstraksi berbasis JSON Utility atau string matching aman
                int contentIndex = jsonResponse.IndexOf("\"content\":");
                if (contentIndex != -1)
                {
                    int startQuote = jsonResponse.IndexOf("\"", contentIndex + 10);
                    if (startQuote != -1)
                    {
                        var sb = new StringBuilder();
                        bool escaped = false;
                        for (int i = startQuote + 1; i < jsonResponse.Length; i++)
                        {
                            char c = jsonResponse[i];
                            if (escaped)
                            {
                                if (c == 'n') sb.Append('\n');
                                else if (c == 't') sb.Append('\t');
                                else if (c == 'r') sb.Append('\r');
                                else if (c == '"') sb.Append('"');
                                else if (c == '\\') sb.Append('\\');
                                else sb.Append(c);
                                escaped = false;
                            }
                            else if (c == '\\')
                            {
                                escaped = true;
                            }
                            else if (c == '"')
                            {
                                break;
                            }
                            else
                            {
                                sb.Append(c);
                            }
                        }
                        return sb.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[GroqApiClient] Gagal mem-parsing respons: {ex.Message}");
            }

            return "";
        }

        private static string EscapeJson(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s.Replace("\\", "\\\\")
                    .Replace("\"", "\\\"")
                    .Replace("\n", "\\n")
                    .Replace("\r", "\\r")
                    .Replace("\t", "\\t");
        }
    }
}
