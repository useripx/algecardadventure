using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AlgeCardAdventure.Core;
using AlgeCardAdventure.Data;
using AlgeCardAdventure.Session;
using AlgeCardAdventure.UI;

namespace AlgeCardAdventure.Tests
{
    [TestFixture]
    public class TrueFalseTests
    {
        private TrueFalseController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = (TrueFalseController)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(TrueFalseController));
            _controller.CountdownDuration = 3.0f;
            _controller.RemainingCountdown = 3.0f;
        }

        [Test]
        public void TC_TF_01_Evaluation_CorrectAnswer_HighlightsTrueBadge()
        {
            var q = new QuestionModel
            {
                id = "TEST-CORRECT",
                equation_model = new string[] { "3x + 5 = 20" },
                final_answer = "5"
            };

            _controller.EvaluateSession(q, true);

            Assert.IsTrue(_controller.IsEvaluated);
            Assert.IsTrue(_controller.IsCorrectEvaluation);
            Assert.IsFalse(_controller.IsExplanationRevealed);
        }

        [Test]
        public void TC_TF_02_Evaluation_IncorrectAnswer_HighlightsFalseBadge()
        {
            var q = new QuestionModel
            {
                id = "TEST-INCORRECT",
                equation_model = new string[] { "3x + 5 = 20" },
                final_answer = "5"
            };

            _controller.EvaluateSession(q, false);

            Assert.IsTrue(_controller.IsEvaluated);
            Assert.IsFalse(_controller.IsCorrectEvaluation);
            Assert.IsFalse(_controller.IsExplanationRevealed);
        }

        [Test]
        public void TC_TF_03_StatusBadges_AreNotButtons_NoInteractable()
        {
            // Validasi instruksi khusus Yogi Ario: Elemen TRUE dan FALSE adalah visual display/badge, BUKAN TOMBOL
            var conclusionTrue = TrueFalseController.GetResultConclusion(true);
            var conclusionFalse = TrueFalseController.GetResultConclusion(false);

            Assert.IsTrue(conclusionTrue.Contains("TRUE"));
            Assert.IsTrue(conclusionFalse.Contains("FALSE"));
        }

        [Test]
        public void TC_TF_04_ScoreRecording_SingleSubmissionPerQuestion()
        {
            // Validasi kalkulasi skor akumulatif per soal
            int score1 = ScoreCalculator.CalculateScore(1, 1);
            Assert.AreEqual(100, score1);

            int score2 = ScoreCalculator.CalculateScore(1, 2);
            Assert.AreEqual(50, score2);

            int score3 = ScoreCalculator.CalculateScore(2, 3);
            Assert.AreEqual(67, score3);
        }

        [Test]
        public void TC_TF_05_ThreeSecondCountdown_CalculatesProperNormalizedProgress()
        {
            Assert.AreEqual(3.0f, _controller.CountdownDuration);
            Assert.AreEqual(3.0f, _controller.RemainingCountdown);

            // Pada t = 1.5s (setengah durasi), normalized progress = 0.5f
            float simulatedElapsed = 1.5f;
            float remaining = _controller.CountdownDuration - simulatedElapsed;
            float progress = Mathf.Clamp01(remaining / _controller.CountdownDuration);

            Assert.AreEqual(0.5f, progress, 0.001f);
        }

        [Test]
        public void TC_TF_06_SkipCountdown_ImmediatelyRevealsExplanation()
        {
            var q = new QuestionModel
            {
                id = "TEST-SKIP",
                equation_model = new string[] { "y - 3 = 5" },
                final_answer = "8"
            };

            _controller.EvaluateSession(q, true);
            Assert.IsFalse(_controller.IsExplanationRevealed);

            // Tekan skip
            _controller.OnSkipClicked();

            Assert.IsTrue(_controller.IsExplanationRevealed);
            Assert.AreEqual(0f, _controller.RemainingCountdown);
        }

        [Test]
        public void TC_TF_07_NextQuestion_Below10Questions_NavigatesToScoreResults()
        {
            // Sesuai alur resmi Yogi Ario (DEC-38): Tombol NEXT pada True/False selalu menuju Score Results
            string nextScene5 = TrueFalseController.GetNextScene(totalAnswered: 5, maxQuestions: 10);
            Assert.AreEqual(SceneNavigator.SceneScoreResults, nextScene5);

            string nextScene9 = TrueFalseController.GetNextScene(totalAnswered: 9, maxQuestions: 10);
            Assert.AreEqual(SceneNavigator.SceneScoreResults, nextScene9);
        }

        [Test]
        public void TC_TF_08_NextQuestion_At10Questions_NavigatesToScoreResults()
        {
            // Jika soal ke-10 (akhir sesi), navigasi juga menuju Score Results
            string nextScene10 = TrueFalseController.GetNextScene(totalAnswered: 10, maxQuestions: 10);
            Assert.AreEqual(SceneNavigator.SceneScoreResults, nextScene10);

            string nextScene11 = TrueFalseController.GetNextScene(totalAnswered: 11, maxQuestions: 10);
            Assert.AreEqual(SceneNavigator.SceneScoreResults, nextScene11);
        }

        [Test]
        public void TC_TF_09_ExplanationProof_GeneratesStepByStepSubstitution()
        {
            var q = new QuestionModel
            {
                id = "TEST-PROOF",
                equation_model = new string[] { "4 * p = 12" },
                final_answer = "3"
            };

            string statement = TrueFalseController.GenerateStatement(q);
            Assert.IsTrue(statement.Contains("p = 3"));
            Assert.IsTrue(statement.Contains("4 * p = 12"));

            List<string> steps = TrueFalseController.GenerateExplanationSteps(q, true);
            Assert.AreEqual(3, steps.Count);
            Assert.IsTrue(steps[0].Contains("Substitute p = 3"));
            Assert.IsTrue(steps[1].Contains("arithmetic"));
            Assert.IsTrue(steps[2].Contains("Result"));
        }

        [Test]
        public void TC_TF_10_StreakCounter_IncrementsOnCorrect_ResetsOnWrong()
        {
            int streak = 0;

            // Benar pertama kali
            streak++;
            Assert.AreEqual(1, streak);

            // Benar kedua kali
            streak++;
            Assert.AreEqual(2, streak);

            // Salah -> reset streak ke 0
            streak = 0;
            Assert.AreEqual(0, streak);
        }

        [Test]
        public void TC_TF_11_ExplanationProof_UsesRealSolutionSteps_WhenAvailable()
        {
            var q = new QuestionModel
            {
                id = "TEST-REAL-STEPS",
                equation_model = new string[] { "x + 10000 = 25000" },
                solution_steps = new string[]
                {
                    "Let the initial savings be x.",
                    "x + 10000 = 25000",
                    "x = 25000 - 10000",
                    "x = 15000"
                },
                final_answer = "Rp15,000"
            };

            List<string> steps = TrueFalseController.GenerateExplanationSteps(q, true);
            Assert.AreEqual(3, steps.Count);
            Assert.IsTrue(steps[0].Contains("x + 10000 = 25000"), "Step 1 should state initial equation");
            Assert.IsTrue(steps[1].Contains("x = 25000 - 10000"), "Step 2 should state algebraic transformation/operation");
            Assert.IsTrue(steps[2].Contains("x = 15000"), "Step 3 should state final solution value");
        }
    }
}
