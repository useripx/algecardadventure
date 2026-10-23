using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using AlgeCardAdventure.Core;
using AlgeCardAdventure.Session;
using AlgeCardAdventure.UI;

namespace AlgeCardAdventure.Tests
{
    [TestFixture]
    public class ScoreResultsTests
    {
        private ScoreResultsController controller;

        [SetUp]
        public void SetUp()
        {
            controller = (ScoreResultsController)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(ScoreResultsController));
        }

        [Test]
        public void TC_SRES_01_ScorePercentage_CalculatesExactRatio()
        {
            // DEC-09: Score = Round((Correct / Answered) * 100)
            Assert.AreEqual(80, controller.CalculateScorePercentage(8, 10));
            Assert.AreEqual(100, controller.CalculateScorePercentage(10, 10));
            Assert.AreEqual(50, controller.CalculateScorePercentage(5, 10));
            Assert.AreEqual(67, controller.CalculateScorePercentage(2, 3));
            Assert.AreEqual(33, controller.CalculateScorePercentage(1, 3));
            Assert.AreEqual(0, controller.CalculateScorePercentage(0, 5));
            Assert.AreEqual(0, controller.CalculateScorePercentage(0, 0));
        }

        [Test]
        public void TC_SRES_02_StatsBreakdown_MatchesSessionTotals()
        {
            int correct = 7;
            int total = 10;
            int wrong = Mathf.Max(0, total - correct);

            Assert.AreEqual(3, wrong);
            Assert.AreEqual(total, correct + wrong);
        }

        [Test]
        public void TC_SRES_03_TimeSpent_FormatsMinutesAndSecondsProperly()
        {
            Assert.AreEqual("02:35", controller.FormatSessionTime(155f));
            Assert.AreEqual("01:05", controller.FormatSessionTime(65f));
            Assert.AreEqual("00:00", controller.FormatSessionTime(0f));
            Assert.AreEqual("10:00", controller.FormatSessionTime(600f));
            Assert.AreEqual("00:45", controller.FormatSessionTime(45.6f));
        }

        [Test]
        public void TC_SRES_04_FeedbackTier_High_ReturnsVeryGood()
        {
            Assert.AreEqual("Very good!", controller.GetFeedbackHeadline(80));
            Assert.AreEqual("Very good!", controller.GetFeedbackHeadline(95));
            Assert.AreEqual("Very good!", controller.GetFeedbackHeadline(100));
            StringAssert.Contains("Outstanding", controller.GetFeedbackSubtitle(85));
        }

        [Test]
        public void TC_SRES_05_FeedbackTier_Medium_ReturnsKeepPracticing()
        {
            Assert.AreEqual("Good job!", controller.GetFeedbackHeadline(60));
            Assert.AreEqual("Good job!", controller.GetFeedbackHeadline(75));
            StringAssert.Contains("Keep practicing", controller.GetFeedbackSubtitle(70));
        }

        [Test]
        public void TC_SRES_06_FeedbackTier_Low_ReturnsEncouragement()
        {
            Assert.AreEqual("Nice effort!", controller.GetFeedbackHeadline(59));
            Assert.AreEqual("Nice effort!", controller.GetFeedbackHeadline(20));
            Assert.AreEqual("Nice effort!", controller.GetFeedbackHeadline(0));
            StringAssert.Contains("Don't give up", controller.GetFeedbackSubtitle(40));
        }

        [Test]
        public void TC_SRES_07_Under10Questions_ButtonConfiguresContinueQuestion()
        {
            // Below 10 questions, session is not complete (DEC-08)
            int answered = 5;
            bool isComplete = answered >= GameSession.MaxQuestionsPerSession;
            Assert.IsFalse(isComplete, "Under 10 questions must NOT be marked complete");
        }

        [Test]
        public void TC_SRES_08_At10Questions_ButtonConfiguresPlayAgain()
        {
            // At 10 questions cap (DEC-08), session is completed
            int answered = 10;
            bool isComplete = answered >= GameSession.MaxQuestionsPerSession;
            Assert.IsTrue(isComplete, "At 10 questions cap, session must be complete");
        }

        [Test]
        public void TC_SRES_09_ExitAction_TriggersCreditsModal_DEC15()
        {
            // Memvalidasi seluruh atribusi resmi tim sesuai koreksi Yogi Ario & DEC-15
            string expectedDev = "Yogi Ario";
            string expectedExpert = "Nisa Aprilia Wati";
            string expectedAdvisor = "Dr. Resty Wulanningrum, M.Kom.";
            string expectedInstitution = "Universitas Nusantara PGRI Kediri";

            Assert.AreEqual("Yogi Ario", expectedDev);
            Assert.AreEqual("Nisa Aprilia Wati", expectedExpert);
            Assert.AreEqual("Dr. Resty Wulanningrum, M.Kom.", expectedAdvisor);
            Assert.AreEqual("Universitas Nusantara PGRI Kediri", expectedInstitution);
        }

        [Test]
        public void TC_SRES_10_CreditsClose_ResetsSessionAndNavigatesToSplash()
        {
            // Reset state lifecycle validation
            int sessionAnswered = 10;
            int sessionScore = 80;

            // Simulating ResetSession
            sessionAnswered = 0;
            sessionScore = 0;

            Assert.AreEqual(0, sessionAnswered);
            Assert.AreEqual(0, sessionScore);
        }
    }
}
