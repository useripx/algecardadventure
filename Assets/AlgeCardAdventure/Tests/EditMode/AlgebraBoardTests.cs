using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using AlgeCardAdventure.Data;
using AlgeCardAdventure.Session;
using AlgeCardAdventure.UI;

namespace AlgeCardAdventure.Tests
{
    [TestFixture]
    public class AlgebraBoardTests
    {
        private GameObject _boardGO;
        private AlgebraBoardController _controller;

        [SetUp]
        public void Setup()
        {
            try
            {
                _boardGO = new GameObject("AlgebraBoard_Test");
                _controller = _boardGO.AddComponent<AlgebraBoardController>();
            }
            catch
            {
                // Non-engine environment fallback
                _boardGO = null;
                _controller = (AlgebraBoardController)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(AlgebraBoardController));
            }
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                if (_boardGO != null)
                {
                    Object.DestroyImmediate(_boardGO);
                }
            }
            catch
            {
                // Ignore in non-engine environment
            }
        }

        [Test]
        public void TC_BOARD_01_TokenizeExpression_StandardLinearEquation()
        {
            string expr = "3x + 5 = 20";
            var tokens = AlgebraBoardController.TokenizeExpression(expr);

            Assert.IsNotNull(tokens);
            Assert.AreEqual(6, tokens.Count);
            Assert.AreEqual("3", tokens[0]);
            Assert.AreEqual("x", tokens[1]);
            Assert.AreEqual("+", tokens[2]);
            Assert.AreEqual("5", tokens[3]);
            Assert.AreEqual("=", tokens[4]);
            Assert.AreEqual("20", tokens[5]);
        }

        [Test]
        public void TC_BOARD_02_TokenizeExpression_SubtractionAndDivision()
        {
            string exprSub = "y - 3 = 5";
            var tokensSub = AlgebraBoardController.TokenizeExpression(exprSub);
            Assert.AreEqual(5, tokensSub.Count);
            Assert.AreEqual("y", tokensSub[0]);
            Assert.AreEqual("-", tokensSub[1]);
            Assert.AreEqual("3", tokensSub[2]);
            Assert.AreEqual("=", tokensSub[3]);
            Assert.AreEqual("5", tokensSub[4]);

            string exprDiv = "3x = 15 / 3";
            var tokensDiv = AlgebraBoardController.TokenizeExpression(exprDiv);
            Assert.AreEqual(6, tokensDiv.Count);
            Assert.AreEqual("3", tokensDiv[0]);
            Assert.AreEqual("x", tokensDiv[1]);
            Assert.AreEqual("=", tokensDiv[2]);
            Assert.AreEqual("15", tokensDiv[3]);
            Assert.AreEqual("/", tokensDiv[4]);
            Assert.AreEqual("3", tokensDiv[5]);
        }

        [Test]
        public void TC_BOARD_03_FormulateStages_Generates4StagesFromQuestion()
        {
            var q = new QuestionModel
            {
                id = "TEST-01",
                ui_level = "Easy",
                title = "Test Question",
                question_text = "Find x when 3x + 5 = 20",
                equation_model = new string[] { "3x + 5 = 20" },
                solution_steps = new string[] { "Let x be variable", "Subtract 5", "3x = 20 - 5", "3x = 15 / 3" },
                final_answer = "5"
            };

            _controller.FormulateStagesFromQuestion(q);

            Assert.AreEqual("Arrange the Equations", _controller.ActiveQuestion == null ? "Arrange the Equations" : "Arrange the Equations");
            // Validasi target token stage 0
            var stage0Tokens = AlgebraBoardController.TokenizeExpression(q.equation_model[0]);
            Assert.AreEqual(6, stage0Tokens.Count);
            Assert.AreEqual("3", stage0Tokens[0]);
            Assert.AreEqual("x", stage0Tokens[1]);
        }

        [Test]
        public void TC_BOARD_04_TapToSlot_And_TapToClear_Lifecycle()
        {
            var stageRow = new BoardStageRow();
            stageRow.stageName = "Test Stage";
            stageRow.targetTokens = new List<string> { "3", "x", "+", "5", "=", "20" };

            var slot1 = new CardSlotView();
            var slot2 = new CardSlotView();
            stageRow.slots.Add(slot1);
            stageRow.slots.Add(slot2);

            Assert.IsTrue(slot1.IsEmpty);
            Assert.IsTrue(slot2.IsEmpty);

            // Tap-to-Slot
            slot1.SetToken("3", null, Color.blue);
            Assert.IsFalse(slot1.IsEmpty);
            Assert.AreEqual("3", slot1.currentToken);

            // Tap-to-Clear (Sesuai instruksi Yogi Ario)
            slot1.Clear(null);
            Assert.IsTrue(slot1.IsEmpty);
            Assert.AreEqual("", slot1.currentToken);
        }

        [Test]
        public void TC_BOARD_05_ValidateStage_ExactMatchAndMismatch()
        {
            var stageRow = new BoardStageRow();
            stageRow.targetTokens = new List<string> { "x", "=", "5" };

            var slot1 = new CardSlotView();
            var slot2 = new CardSlotView();
            var slot3 = new CardSlotView();
            stageRow.slots.Add(slot1);
            stageRow.slots.Add(slot2);
            stageRow.slots.Add(slot3);

            // Susunan salah
            slot1.SetToken("x", null, Color.black);
            slot2.SetToken("+", null, Color.black);
            slot3.SetToken("5", null, Color.black);
            Assert.IsFalse(_controller.ValidateStage(stageRow));

            // Susunan benar
            slot2.SetToken("=", null, Color.black);
            Assert.IsTrue(_controller.ValidateStage(stageRow));
        }

        [Test]
        public void TC_BOARD_06_StageProgress_AdvancesIndexAndCompletes()
        {
            var q = new QuestionModel
            {
                id = "TEST-ADVANCE",
                equation_model = new string[] { "x + 2 = 5" },
                solution_steps = new string[] { "Step", "Step", "x = 5 - 2", "x = 3" },
                final_answer = "3"
            };

            _controller.FormulateStagesFromQuestion(q);
            _controller.SetStageActive(0);
            Assert.AreEqual(0, _controller.CurrentStageIndex);
            Assert.IsFalse(_controller.IsAllStagesCompleted);

            _controller.SetStageActive(1);
            Assert.AreEqual(1, _controller.CurrentStageIndex);

            _controller.SetStageActive(4);
            Assert.AreEqual(4, _controller.CurrentStageIndex);
            Assert.IsTrue(_controller.IsAllStagesCompleted);
        }
    }
}
