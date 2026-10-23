using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AlgeCardAdventure.AI;
using AlgeCardAdventure.UI;

namespace AlgeCardAdventure.Tests
{
    [TestFixture]
    public class ChatbotTests
    {
        [Test]
        public void TC_CHAT_01_LongPressTrigger_TimerReaches3Seconds_FiresEvent()
        {
            var go = new GameObject("TestLogo");
            var trigger = go.AddComponent<LogoLongPressTrigger>();

            bool eventFired = false;
            LogoLongPressTrigger.OnUniversalChatbotTriggered += () => { eventFired = true; };

            // Simulasikan sentuhan awal
            trigger.OnPointerDown(null);
            Assert.IsTrue(trigger.IsHolding);
            Assert.AreEqual(0f, trigger.CurrentHoldTime);

            // Simulasikan berjalannya waktu 3 detik
            trigger.SimulateTick(3.05f);

            Assert.IsTrue(eventFired, "Tahanan 3 detik wajib memicu event pembukaan Chatbot!");
            Assert.IsFalse(trigger.IsHolding, "Status tahanan wajib ter-reset setelah terpicu.");

            Object.DestroyImmediate(go);
        }

        [Test]
        public void TC_CHAT_02_LongPressTrigger_EarlyRelease_CancelsTrigger()
        {
            var go = new GameObject("TestLogo");
            var trigger = go.AddComponent<LogoLongPressTrigger>();

            bool eventFired = false;
            LogoLongPressTrigger.OnUniversalChatbotTriggered += () => { eventFired = true; };

            // Sentuhan selama 1.5 detik lalu dilepas
            trigger.OnPointerDown(null);
            trigger.SimulateTick(1.5f);
            trigger.OnPointerUp(null);

            Assert.IsFalse(trigger.IsHolding);
            Assert.AreEqual(0f, trigger.CurrentHoldTime);
            Assert.IsFalse(eventFired, "Jari dilepas sebelum 3 detik TIDAK BOLEH memicu Chatbot!");

            Object.DestroyImmediate(go);
        }

        [Test]
        public void TC_CHAT_03_GroqRequestPayload_FormatsCorrectJson()
        {
            var request = new GroqChatRequest
            {
                model = "llama-3.3-70b-versatile",
                temperature = 0.6f,
                max_tokens = 512,
                messages = new List<ChatMessage>
                {
                    new ChatMessage("system", "Test System Prompt"),
                    new ChatMessage("user", "Hello AlgeCard")
                }
            };

            string json = GroqApiClient.SerializeRequest(request);

            Assert.IsNotNull(json);
            Assert.IsTrue(json.Contains("\"model\":\"llama-3.3-70b-versatile\""));
            Assert.IsTrue(json.Contains("\"temperature\":0.6"));
            Assert.IsTrue(json.Contains("\"role\":\"system\""));
            Assert.IsTrue(json.Contains("\"role\":\"user\""));
            Assert.IsTrue(json.Contains("\"content\":\"Hello AlgeCard\""));
        }

        [Test]
        public void TC_CHAT_04_ChatSession_RollingWindow_CapsAtMaxMessages()
        {
            var session = new ChatSessionManager();

            // Tambahkan 10 pesan berturut-turut
            for (int i = 1; i <= 10; i++)
            {
                session.AddUserMessage($"User Question {i}");
                session.AddAssistantMessage($"Assistant Answer {i}");
            }

            // Kapasitas rolling window adalah MaxHistoryMessages (6) + System Prompt (1) = maksimal 7 pesan
            Assert.LessOrEqual(session.Messages.Count, 7);
            Assert.AreEqual("system", session.Messages[0].role, "System Prompt wajib selalu berada di posisi pertama!");
        }

        [Test]
        public void TC_CHAT_05_OfflineFallback_ReturnsPreauthoredTips()
        {
            var session = new ChatSessionManager();

            string replyIsolate = session.GetOfflineFallbackResponse("How do I isolate x in 3x + 5 = 20?");
            Assert.IsTrue(replyIsolate.Contains("operasi lawan"), "Fallback isolate wajib memuat tips operasi kedua ruas!");

            string replyVariable = session.GetOfflineFallbackResponse("What is a variable and coefficient?");
            Assert.IsTrue(replyVariable.Contains("kotak misteri"), "Fallback variable wajib memuat analogi ramah anak!");

            string replyGeneral = session.GetOfflineFallbackResponse("Random unrelated message");
            Assert.IsTrue(replyGeneral.Contains("offline di hutan fantasi"), "Fallback umum wajib mengabarkan status offline ramah!");
        }

        [Test]
        public void TC_CHAT_06_GroqConfigManager_LoadsValidApiKey()
        {
            var config = GroqConfigManager.GetConfig();

            Assert.IsNotNull(config);
            Assert.IsFalse(string.IsNullOrWhiteSpace(config.api_key), "Kunci API Groq tidak boleh kosong!");
            Assert.IsTrue(config.api_key.StartsWith("gsk_"), "Kunci API Groq resmi wajib diawali dengan prefix 'gsk_'!");
            Assert.AreEqual("llama-3.3-70b-versatile", config.model);
        }

        [Test]
        public void TC_CHAT_07_Bootstrap_DetectsLogoAndAttachesTrigger()
        {
            var logoGo = new GameObject("LogoPlaque_Test");
            var img = logoGo.AddComponent<UnityEngine.UI.Image>();
            img.raycastTarget = false; // Awalnya nonaktif

            Assert.IsNull(logoGo.GetComponent<LogoLongPressTrigger>());

            int attached = ChatbotRuntimeBootstrap.InjectLogoTriggerInActiveScene();

            Assert.GreaterOrEqual(attached, 1, "Bootstrap harus mendeteksi dan memasang trigger pada objek Logo!");
            Assert.IsNotNull(logoGo.GetComponent<LogoLongPressTrigger>(), "Komponen LogoLongPressTrigger wajib terpasang!");
            Assert.IsTrue(img.raycastTarget, "raycastTarget wajib diaktifkan agar klik/tahan terdeteksi!");

            Object.DestroyImmediate(logoGo);
        }

        [Test]
        public void TC_CHAT_08_Bootstrap_EnsureProceduralUI_CreatesCompleteModal()
        {
            var rootGo = new GameObject("ChatbotSystem_Test");
            var controller = rootGo.AddComponent<ChatbotUIController>();

            controller.EnsureProceduralUI();

            Assert.IsNotNull(controller, "Controller tidak boleh null");
            // Verifikasi bahwa modal terbuka dan tertutup dengan aman
            controller.OpenChatbot();
            Assert.IsTrue(controller.IsChatOpen, "Modal harus terbuka setelah OpenChatbot()");

            controller.CloseChatbot();
            Assert.IsFalse(controller.IsChatOpen, "Modal harus tertutup setelah CloseChatbot()");

            Object.DestroyImmediate(rootGo);
        }
    }
}
