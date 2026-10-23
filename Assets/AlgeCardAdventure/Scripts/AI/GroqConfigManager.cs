using System;
using UnityEngine;

namespace AlgeCardAdventure.AI
{
    [Serializable]
    public class GroqConfig
    {
        public string api_key = "";
        public string model = "llama-3.3-70b-versatile";
        public string fallback_model = "llama-3.1-8b-instant";
        public float temperature = 0.6f;
        public int max_tokens = 512;
    }

    /// <summary>
    /// Pengelola konfigurasi aman API Key dan parameter inference Groq.
    /// Memuat data dari Resources/groq_config.json (terlindungi oleh .gitignore - Pilar 3).
    /// </summary>
    public static class GroqConfigManager
    {
        private static GroqConfig _cachedConfig = null;
        private const string ResourcePath = "groq_config";

        public static GroqConfig GetConfig()
        {
            if (_cachedConfig != null && !string.IsNullOrEmpty(_cachedConfig.api_key))
            {
                return _cachedConfig;
            }

            string jsonText = null;

            // 1. Coba baca dari file lokal disk (aman di Editor & Test Runner)
            try
            {
                string[] diskCandidates = new[]
                {
                    System.IO.Path.Combine("Assets", "Resources", "groq_config.json"),
                    System.IO.Path.Combine("..", "..", "Assets", "Resources", "groq_config.json")
                };

                foreach (var candidate in diskCandidates)
                {
                    if (System.IO.File.Exists(candidate))
                    {
                        jsonText = System.IO.File.ReadAllText(candidate);
                        break;
                    }
                }
            }
            catch { }

            // 2. Coba baca dari Unity Resources jika di runtime build Android
            if (string.IsNullOrEmpty(jsonText))
            {
                jsonText = TryLoadFromUnityResources(ResourcePath);
            }

            if (!string.IsNullOrEmpty(jsonText))
            {
                _cachedConfig = ParseConfigJson(jsonText);
            }

            // 3. Coba baca dari file api gitignored jika config belum memuat key
            if (_cachedConfig == null || string.IsNullOrEmpty(_cachedConfig.api_key))
            {
                string keyFromFile = TryLoadApiKeyFromFile();
                if (!string.IsNullOrEmpty(keyFromFile))
                {
                    if (_cachedConfig == null)
                    {
                        _cachedConfig = new GroqConfig();
                    }
                    _cachedConfig.api_key = keyFromFile;
                }
            }

            // 4. Coba environment variable GROQ_API_KEY
            if (_cachedConfig == null || string.IsNullOrEmpty(_cachedConfig.api_key))
            {
                string envKey = Environment.GetEnvironmentVariable("GROQ_API_KEY");
                if (!string.IsNullOrEmpty(envKey))
                {
                    if (_cachedConfig == null)
                    {
                        _cachedConfig = new GroqConfig();
                    }
                    _cachedConfig.api_key = envKey;
                }
            }

            if (_cachedConfig == null)
            {
                _cachedConfig = new GroqConfig();
            }

            return _cachedConfig;
        }

        private static string TryLoadApiKeyFromFile()
        {
            try
            {
                string[] paths = new[]
                {
                    System.IO.Path.Combine("Docs", "Ui Template", "chatbot", "api"),
                    System.IO.Path.Combine("..", "..", "Docs", "Ui Template", "chatbot", "api")
                };

                foreach (var path in paths)
                {
                    if (System.IO.File.Exists(path))
                    {
                        string raw = System.IO.File.ReadAllText(path).Trim();
                        string[] lines = raw.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        if (lines.Length > 0 && !string.IsNullOrWhiteSpace(lines[0]))
                        {
                            return lines[0].Trim();
                        }
                    }
                }
            }
            catch { }

            return null;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        private static string TryLoadFromUnityResources(string path)
        {
            try
            {
                TextAsset asset = Resources.Load<TextAsset>(path);
                return asset != null ? asset.text : null;
            }
            catch
            {
                return null;
            }
        }

        public static GroqConfig ParseConfigJson(string json)
        {
            var cfg = new GroqConfig
            {
                api_key = "",
                model = "llama-3.3-70b-versatile",
                fallback_model = "llama-3.1-8b-instant",
                temperature = 0.6f,
                max_tokens = 512
            };

            if (string.IsNullOrEmpty(json)) return cfg;

            string key = ExtractJsonString(json, "api_key");
            if (!string.IsNullOrEmpty(key)) cfg.api_key = key;

            string model = ExtractJsonString(json, "model");
            if (!string.IsNullOrEmpty(model)) cfg.model = model;

            string fallbackModel = ExtractJsonString(json, "fallback_model");
            if (!string.IsNullOrEmpty(fallbackModel)) cfg.fallback_model = fallbackModel;

            return cfg;
        }

        private static string ExtractJsonString(string json, string field)
        {
            string pattern = $"\"{field}\":";
            int idx = json.IndexOf(pattern);
            if (idx == -1) return null;

            int startQuote = json.IndexOf("\"", idx + pattern.Length);
            if (startQuote == -1) return null;

            int endQuote = json.IndexOf("\"", startQuote + 1);
            if (endQuote == -1) return null;

            return json.Substring(startQuote + 1, endQuote - startQuote - 1).Trim();
        }

        public static void SetConfig(GroqConfig config)
        {
            _cachedConfig = config;
        }

        public static bool HasValidApiKey()
        {
            var cfg = GetConfig();
            return cfg != null && !string.IsNullOrWhiteSpace(cfg.api_key) && cfg.api_key.StartsWith("gsk_");
        }
    }
}
