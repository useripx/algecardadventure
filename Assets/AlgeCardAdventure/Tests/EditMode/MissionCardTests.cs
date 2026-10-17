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
    }
}
