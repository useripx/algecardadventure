using System;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace AlgeCardAdventure.UI
{
    /// <summary>
    /// Runtime Bootstrap otomatis untuk sistem Easter Egg AI Math Companion Chatbot.
    /// Berjalan otomatis setelah setiap scene dimuat di Unity Play Mode (RuntimeInitializeOnLoadMethod).
    /// Memastikan Logo di layar aktif dapat ditahan 3 detik untuk membuka obrolan Prof. Hopper & Pip.
    /// </summary>
    public static class ChatbotRuntimeBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnSceneLoaded()
        {
            AttachToActiveScene();
        }

        /// <summary>
        /// Menginisialisasi sistem Chatbot dan menautkan trigger ke objek logo di scene yang sedang aktif.
        /// Dapat dipanggil secara manual atau melalui automated test.
        /// </summary>
        public static void AttachToActiveScene()
        {
            try
            {
                // 1. Pastikan ChatbotUIController telah terbentuk di DontDestroyOnLoad
                var chatbotController = ChatbotUIController.Instance;
                if (chatbotController == null)
                {
                    var chatbotGo = new GameObject("[Chatbot_Companion_System]");
                    try
                    {
                        Object.DontDestroyOnLoad(chatbotGo);
                    }
                    catch
                    {
                        // Safe fallback untuk environment pengujian non-engine
                    }

                    chatbotController = chatbotGo.AddComponent<ChatbotUIController>();
                }
                else
                {
                    chatbotController.EnsureProceduralUI();
                }

                // 2. Pasang LogoLongPressTrigger pada seluruh objek Logo di scene aktif
                InjectLogoTriggerInActiveScene();
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[ChatbotRuntimeBootstrap] Catatan inisialisasi: {ex.Message}");
            }
        }

        /// <summary>
        /// <summary>
        /// Validasi apakah nama sebuah GameObject merepresentasikan elemen Logo permainan.
        /// </summary>
        public static bool IsLogoObjectName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            string lower = name.ToLowerInvariant();
            return lower.Contains("logo");
        }

        /// <summary>
        /// Mencari seluruh objek di scene yang bertindak sebagai Logo (nama memuat 'logo', 'logoplaque', dll.)
        /// dan memasangkan komponen LogoLongPressTrigger secara dinamis.
        /// </summary>
        public static int InjectLogoTriggerInActiveScene()
        {
            int injectedCount = 0;

            try
            {
                var allTransforms = Object.FindObjectsByType<Transform>(FindObjectsInactive.Include);
                foreach (var t in allTransforms)
                {
                    if (t == null) continue;
                    if (IsLogoObjectName(t.gameObject.name))
                    {
                        try
                        {
                            // 1. Pastikan GameObject aktif agar raycast dapat diterima
                            if (!t.gameObject.activeSelf)
                            {
                                t.gameObject.SetActive(true);
                            }

                            // 2. Pastikan memiliki komponen Graphic (Image) dan raycastTarget aktif
                            var graphic = t.GetComponent<Graphic>();
                            if (graphic != null)
                            {
                                graphic.raycastTarget = true;
                                var imgComp = graphic as Image;
                                if (imgComp != null && imgComp.sprite == null)
                                {
                                    imgComp.color = Color.clear;
                                }
                            }
                            else
                            {
                                var img = t.gameObject.AddComponent<Image>();
                                img.color = Color.clear;
                                img.raycastTarget = true;
                            }

                            // 3. Pasang LogoLongPressTrigger jika belum ada
                            var trigger = t.GetComponent<LogoLongPressTrigger>();
                            if (trigger == null)
                            {
                                trigger = t.gameObject.AddComponent<LogoLongPressTrigger>();
                                injectedCount++;
                            }
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[ChatbotRuntimeBootstrap] Pindai logo: {ex.Message}");
            }

            return injectedCount;
        }
    }
}
