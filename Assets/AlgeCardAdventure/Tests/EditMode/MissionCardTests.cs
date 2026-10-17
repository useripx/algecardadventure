using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AlgeCardAdventure.Data;
using AlgeCardAdventure.Session;
using AlgeCardAdventure.Audio;
using AlgeCardAdventure.Core;
using AlgeCardAdventure.UI;

namespace AlgeCardAdventure.Tests
{
    [TestFixture]
    public class MissionCardTests
    {
        private QuestionRepository _repo;

        [SetUp]
        public void Setup()
        {
            _repo = new QuestionRepository();
            _repo.LoadFromStreamingAssets("banksoal.json");
        }

        [Test]
        public void TC_MISSION_01_BatchDrawing_ReturnsUpTo5EligibleQuestions()
        {
            var batch = _repo.GetRandomMissionBatch("Easy", count: 5);
            Assert.IsNotNull(batch);
            Assert.AreEqual(5, batch.Count);

            foreach (var q in batch)
            {
                Assert.AreEqual("Easy", q.ui_level);
                Assert.IsTrue(q.IsEligibleForPlay);
            }
        }

        [Test]
        public void TC_MISSION_02_AntiDuplication_ExcludesCompletedQuestionIds()
        {
            var excluded = new List<string> { "EASY-001", "EASY-002", "EASY-003" };
            var batch = _repo.GetRandomMissionBatch("Easy", count: 5, excludeIds: excluded);

            Assert.IsNotNull(batch);
            foreach (var q in batch)
            {
                Assert.IsFalse(excluded.Contains(q.id), $"Soal {q.id} harusnya dieksklusi dari batch.");
            }
        }

        [Test]
        public void TC_MISSION_03_GameSession_CompletedQuestionIds_Lifecycle()
        {
            var go = new GameObject("SessionTest");
            var session = go.AddComponent<GameSession>();

            try
            {
                session.StartNewSession("Normal");
                Assert.AreEqual(0, session.CompletedQuestionIds.Count);

                session.MarkQuestionCompleted("NORMAL-001");
                session.MarkQuestionCompleted("NORMAL-002");
                Assert.AreEqual(2, session.CompletedQuestionIds.Count);
                Assert.IsTrue(session.CompletedQuestionIds.Contains("NORMAL-001"));
                Assert.IsTrue(session.CompletedQuestionIds.Contains("NORMAL-002"));

                // Penandaan ulang ID yang sama tidak menggandakan list
                session.MarkQuestionCompleted("NORMAL-001");
                Assert.AreEqual(2, session.CompletedQuestionIds.Count);

                // Reset sesi membersihkan daftar completed IDs
                session.ResetSession();
                Assert.AreEqual(0, session.CompletedQuestionIds.Count);
            }
            finally
            {
                Object.DestroyImmediate(go);
            }
        }

        [Test]
        public void TC_MISSION_04_ProceduralAudio_CozyFantasyBGMClip_IsValid()
        {
            var bgmClip = ProceduralAudio.CreateCozyFantasyBGMClip();
            Assert.IsNotNull(bgmClip);
            Assert.AreEqual("Procedural_CozyFantasyBGM", bgmClip.name);
            Assert.AreEqual(8.0f, bgmClip.length, 0.05f);
            Assert.AreEqual(22050, bgmClip.frequency);
            Assert.AreEqual(1, bgmClip.channels);
        }

        [Test]
        public void TC_MISSION_05_MissionCardController_SelectCard_FillsSelectedQuestion()
        {
            var go = new GameObject("TestMissionController");
            var controller = go.AddComponent<MissionCardController>();

            try
            {
                controller.LoadMissionDeck();
                Assert.Greater(controller.CurrentBatch.Count, 0);

                controller.OnCardSelected(0);
                Assert.IsNotNull(controller.SelectedQuestion);
                Assert.AreEqual(controller.CurrentBatch[0].id, controller.SelectedQuestion.id);
            }
            finally
            {
                Object.DestroyImmediate(go);
            }
        }

        [Test]
        public void TC_MISSION_06_MissionCardController_Continue_SetsSessionAndNavigates()
        {
            var sessionGo = new GameObject("TestSession");
            var session = sessionGo.AddComponent<GameSession>();
            session.StartNewSession("Hard");

            var controllerGo = new GameObject("TestMissionController");
            var controller = controllerGo.AddComponent<MissionCardController>();

            string navigatedScene = null;
            void Handler(string s) => navigatedScene = s;
            SceneNavigator.OnBeforeSceneLoad += Handler;

            try
            {
                controller.LoadMissionDeck();
                controller.OnCardSelected(0);
                string chosenId = controller.SelectedQuestion.id;

                controller.OnContinueClicked();

                Assert.IsNotNull(session.CurrentQuestion);
                Assert.AreEqual(chosenId, session.CurrentQuestion.id);
                Assert.IsTrue(session.CompletedQuestionIds.Contains(chosenId));
                Assert.AreEqual("04_AlgebraCardBoard", navigatedScene);
            }
            finally
            {
                SceneNavigator.OnBeforeSceneLoad -= Handler;
                Object.DestroyImmediate(controllerGo);
                Object.DestroyImmediate(sessionGo);
            }
        }

        [Test]
        public void TC_MISSION_07_MissionCardController_BackToCards_HidesModal()
        {
            var controllerGo = new GameObject("TestMissionController");
            var controller = controllerGo.AddComponent<MissionCardController>();

            var modalGo = new GameObject("MissionDetailModal");
            modalGo.SetActive(true);

            // Set modal via reflection
            var field = typeof(MissionCardController).GetField("missionDetailModal", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field.SetValue(controller, modalGo);

            try
            {
                Assert.IsTrue(modalGo.activeSelf);
                controller.OnBackToCardsClicked();
                Assert.IsFalse(modalGo.activeSelf, "Modal misi harus tertutup setelah tombol OnBackToCardsClicked ditekan.");
            }
            finally
            {
                Object.DestroyImmediate(modalGo);
                Object.DestroyImmediate(controllerGo);
            }
        }

        [Test]
        public void TC_MISSION_08_MissionCardController_DealCardsImmediately_SetsHasDealt()
        {
            var controllerGo = new GameObject("TestMissionController");
            var controller = controllerGo.AddComponent<MissionCardController>();

            try
            {
                controller.LoadMissionDeck();
                controller.DealCardsImmediately();
                Assert.IsTrue(controller.HasDealt);
                Assert.IsFalse(controller.IsShuffling);
            }
            finally
            {
                Object.DestroyImmediate(controllerGo);
            }
        }

        [Test]
        public void TC_MISSION_09_MissionCardController_ShuffleRoutine_SetsIsShufflingTrue()
        {
            var controllerGo = new GameObject("TestMissionController");
            var controller = controllerGo.AddComponent<MissionCardController>();

            try
            {
                controller.LoadMissionDeck();
                Assert.IsFalse(controller.IsShuffling);

                controller.OnShuffleClicked();
                Assert.IsTrue(controller.IsShuffling, "IsShuffling harus bernilai true saat OnShuffleClicked dipanggil.");
            }
            finally
            {
                Object.DestroyImmediate(controllerGo);
            }
        }

        [Test]
        public void TC_MISSION_10_MissionCardController_DynamicStarsAndPoints_ReflectsLevel()
        {
            var controllerGo = new GameObject("TestMissionController");
            var controller = controllerGo.AddComponent<MissionCardController>();

            // Setup mock UI elements
            var s1 = new GameObject("Star1").AddComponent<UnityEngine.UI.Image>();
            var s2 = new GameObject("Star2").AddComponent<UnityEngine.UI.Image>();
            var s3 = new GameObject("Star3").AddComponent<UnityEngine.UI.Image>();
            var pts = new GameObject("Pts").AddComponent<UnityEngine.UI.Text>();

            var goldSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 4, 4), Vector2.zero);
            var greySprite = Sprite.Create(Texture2D.blackTexture, new Rect(0, 0, 4, 4), Vector2.zero);

            var fields = typeof(MissionCardController).GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            foreach (var f in fields)
            {
                if (f.Name == "star1Image") f.SetValue(controller, s1);
                if (f.Name == "star2Image") f.SetValue(controller, s2);
                if (f.Name == "star3Image") f.SetValue(controller, s3);
                if (f.Name == "starGoldSprite") f.SetValue(controller, goldSprite);
                if (f.Name == "starGreySprite") f.SetValue(controller, greySprite);
                if (f.Name == "missionPointsText") f.SetValue(controller, pts);
            }

            try
            {
                // Test Easy
                controller.LoadMissionDeck();
                controller.OnCardSelected(0);
                Assert.AreEqual(goldSprite, s1.sprite, "Star 1 harus gold pada Easy");
                Assert.AreEqual(greySprite, s2.sprite, "Star 2 harus grey pada Easy");
                Assert.AreEqual(greySprite, s3.sprite, "Star 3 harus grey pada Easy");
                Assert.AreEqual("100", pts.text, "Poin harus 100 pada Easy");
            }
            finally
            {
                Object.DestroyImmediate(s1.gameObject);
                Object.DestroyImmediate(s2.gameObject);
                Object.DestroyImmediate(s3.gameObject);
                Object.DestroyImmediate(pts.gameObject);
                Object.DestroyImmediate(controllerGo);
            }
        }
    }
}
