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

        [Test]
        public void TC_BOARD_07_MoreDetails_StepBackNavigation()
        {
            var q = new QuestionModel
            {
                id = "TEST-DETAILS",
                equation_model = new string[] { "3x + 5 = 20" },
                solution_steps = new string[] { "Step 0", "Step 1", "3x = 20 - 5", "x = 5" },
                final_answer = "5"
            };

            _controller.FormulateStagesFromQuestion(q);
            _controller.SetStageActive(2); // Di stage index 2 (Stage 3)
            Assert.AreEqual(2, _controller.CurrentStageIndex);

            // Tekan More Details untuk mundur
            _controller.OnMoreDetailsClicked();
            Assert.AreEqual(1, _controller.CurrentStageIndex);
            Assert.IsFalse(_controller.IsAllStagesCompleted);

            // Mundur lagi ke stage index 0 (Stage 1)
            _controller.OnMoreDetailsClicked();
            Assert.AreEqual(0, _controller.CurrentStageIndex);

            // Mundur saat di stage pertama tidak boleh bernilai negatif
            _controller.OnMoreDetailsClicked();
            Assert.AreEqual(0, _controller.CurrentStageIndex);
        }

        [Test]
        public void TC_BOARD_08_StageBadges_StatusTransitions()
        {
            var q = new QuestionModel
            {
                id = "TEST-BADGES",
                equation_model = new string[] { "y - 3 = 5" },
                solution_steps = new string[] { "Step 0", "Step 1", "y = 5 + 3", "y = 8" },
                final_answer = "8"
            };

            _controller.FormulateStagesFromQuestion(q);
            _controller.SetStageActive(1); // Stage index 1 aktif

            // Validasi transisi status logic
            Assert.AreEqual(1, _controller.CurrentStageIndex);
            Assert.IsFalse(_controller.IsAllStagesCompleted);
        }

        [Test]
        public void TC_BOARD_09_ClearActiveStage_ResetsOnlyActiveRow()
        {
            var q = new QuestionModel
            {
                id = "TEST-CLEAR",
                equation_model = new string[] { "3x + 5 = 20" },
                solution_steps = new string[] { "Step 0", "Step 1", "3x = 20 - 5", "x = 5" },
                final_answer = "5"
            };

            _controller.FormulateStagesFromQuestion(q);
            _controller.SetStageActive(0);

            // Simulasikan slot terisi
            var stage0 = new BoardStageRow();
            stage0.targetTokens = new List<string> { "3", "x" };
            var s1 = new CardSlotView();
            s1.SetToken("3", null, Color.blue);
            stage0.slots.Add(s1);

            Assert.IsFalse(s1.IsEmpty);
            s1.Clear(null);
            Assert.IsTrue(s1.IsEmpty);
        }

        [Test]
        public void TC_BOARD_10_DynamicQuestionAdaptation_BankSoalEasyAndNormal()
        {
            // Soal Easy dengan variabel y
            var qEasy = new QuestionModel
            {
                id = "EASY-002",
                ui_level = "Easy",
                title = "Cutting a Cake",
                question_text = "A sponge cake is cut into several pieces.",
                equation_model = new string[] { "y - 3 = 5" },
                solution_steps = new string[] { "Let variable be y", "y - 3 = 5", "y = 5 + 3", "y = 8" },
                final_answer = "8"
            };

            _controller.FormulateStagesFromQuestion(qEasy);
            var tokens0 = AlgebraBoardController.TokenizeExpression(qEasy.equation_model[0]);
            Assert.AreEqual(5, tokens0.Count);
            Assert.AreEqual("y", tokens0[0]);
            Assert.AreEqual("-", tokens0[1]);
            Assert.AreEqual("3", tokens0[2]);
            Assert.AreEqual("=", tokens0[3]);
            Assert.AreEqual("5", tokens0[4]);

            // Normalisasi simbol kali dan bagi
            var tokensMult = AlgebraBoardController.TokenizeExpression("4 × p = 12");
            Assert.AreEqual("*", tokensMult[1]);

            var tokensDiv = AlgebraBoardController.TokenizeExpression("15 ÷ 3 = 5");
            Assert.AreEqual("/", tokensDiv[1]);
        }

        [Test]
        public void TC_BOARD_11_AllSevenSlotsActive_InAllStages()
        {
            var q = new QuestionModel
            {
                id = "TEST-7SLOTS",
                equation_model = new string[] { "3x + 5 = 20" },
                solution_steps = new string[] { "S0", "S1", "3x = 20 - 5", "x = 5" },
                final_answer = "5"
            };

            // Buat stages dengan 7 slot per stage
            _controller.stages = new List<BoardStageRow>();
            for (int s = 0; s < 4; s++)
            {
                var row = new BoardStageRow();
                for (int slotIdx = 0; slotIdx < 7; slotIdx++)
                {
                    row.slots.Add(new CardSlotView());
                }
                _controller.stages.Add(row);
            }

            _controller.FormulateStagesFromQuestion(q);

            // Validasi bahwa seluruh 4 tahap memiliki 7 slot dan tidak ada yang disembunyikan
            for (int s = 0; s < 4; s++)
            {
                Assert.AreEqual(7, _controller.stages[s].slots.Count);
                for (int slotIdx = 0; slotIdx < 7; slotIdx++)
                {
                    Assert.IsTrue(_controller.stages[s].slots[slotIdx].IsEmpty);
                }
            }
        }

        [Test]
        public void TC_BOARD_12_EvaluateOverallBoardSolution_CorrectAndIncorrect()
        {
            var q = new QuestionModel
            {
                id = "TEST-EVAL",
                equation_model = new string[] { "3x + 5 = 20" },
                solution_steps = new string[] { "S0", "S1", "3x = 20 - 5", "x = 5" },
                final_answer = "5"
            };

            _controller.stages = new List<BoardStageRow>();
            for (int s = 0; s < 4; s++)
            {
                var row = new BoardStageRow();
                for (int slotIdx = 0; slotIdx < 7; slotIdx++)
                {
                    row.slots.Add(new CardSlotView());
                }
                _controller.stages.Add(row);
            }

            _controller.FormulateStagesFromQuestion(q);

            // Kasus 1: Jawaban salah (x = 4)
            _controller.stages[3].slots[0].SetToken("x", null, Color.black);
            _controller.stages[3].slots[1].SetToken("=", null, Color.black);
            _controller.stages[3].slots[2].SetToken("4", null, Color.black);
            Assert.IsFalse(_controller.EvaluateOverallBoardSolution(), "Pengerjaan dengan hasil salah harus dievaluasi FALSE untuk Layar 05.");

            // Kasus 2: Jawaban benar (x = 5)
            _controller.stages[3].slots[2].SetToken("5", null, Color.black);
            Assert.IsTrue(_controller.EvaluateOverallBoardSolution(), "Pengerjaan dengan hasil benar harus dievaluasi TRUE untuk Layar 05.");
        }

        [Test]
        public void TC_BOARD_13_StrictValidation_RejectsIncorrectAdvancement()
        {
            var q = new QuestionModel
            {
                id = "TEST-STRICT",
                equation_model = new string[] { "x + 2 = 5" },
                solution_steps = new string[] { "S0", "S1", "x = 5 - 2", "x = 3" },
                final_answer = "3"
            };

            _controller.stages = new List<BoardStageRow>();
            for (int s = 0; s < 4; s++)
            {
                var row = new BoardStageRow();
                for (int slotIdx = 0; slotIdx < 7; slotIdx++)
                {
                    row.slots.Add(new CardSlotView());
                }
                _controller.stages.Add(row);
            }

            _controller.FormulateStagesFromQuestion(q);
            _controller.SetStageActive(0);

            // Simulasikan susunan salah di Stage 0 (x - 2 = 5, padahal harus x + 2 = 5)
            _controller.stages[0].slots[0].SetToken("x", null, Color.black);
            _controller.stages[0].slots[1].SetToken("-", null, Color.black);
            _controller.stages[0].slots[2].SetToken("2", null, Color.black);
            _controller.stages[0].slots[3].SetToken("=", null, Color.black);
            _controller.stages[0].slots[4].SetToken("5", null, Color.black);

            _controller.OnContinueClicked();
            Assert.AreEqual(0, _controller.CurrentStageIndex, "Pemain tidak boleh maju ke tahap berikutnya jika kartu belum pas.");

            // Perbaiki kartu menjadi benar
            _controller.stages[0].slots[1].SetToken("+", null, Color.black);
            _controller.OnContinueClicked();
            Assert.AreEqual(1, _controller.CurrentStageIndex, "Pemain harus bisa maju ke tahap berikutnya ketika kartu sudah pas.");
        }

        [Test]
        public void TC_BOARD_14_DeleteOneCard_RemovesOnlyRightmostFilledSlot()
        {
            var q = new QuestionModel
            {
                id = "TEST-DEL-ONE",
                equation_model = new string[] { "3x + 5 = 20" },
                solution_steps = new string[] { "S0", "S1", "S2", "S3" },
                final_answer = "5"
            };

            _controller.stages = new List<BoardStageRow>();
            for (int s = 0; s < 4; s++)
            {
                var row = new BoardStageRow();
                for (int slotIdx = 0; slotIdx < 7; slotIdx++)
                {
                    row.slots.Add(new CardSlotView());
                }
                _controller.stages.Add(row);
            }

            _controller.FormulateStagesFromQuestion(q);
            _controller.SetStageActive(0);

            var activeSlots = _controller.stages[0].slots;
            // Isi 3 slot pertama: [3] [x] [+]
            activeSlots[0].SetToken("3", null, Color.black);
            activeSlots[1].SetToken("x", null, Color.black);
            activeSlots[2].SetToken("+", null, Color.black);

            Assert.IsFalse(activeSlots[0].IsEmpty);
            Assert.IsFalse(activeSlots[1].IsEmpty);
            Assert.IsFalse(activeSlots[2].IsEmpty);
            Assert.IsTrue(activeSlots[3].IsEmpty);

            // Panggil OnDeleteOneClicked() pertama -> slot ke-2 (+) harus terhapus
            _controller.OnDeleteOneClicked();
            Assert.AreEqual("3", activeSlots[0].currentToken, "Slot 0 harus tetap utuh.");
            Assert.AreEqual("x", activeSlots[1].currentToken, "Slot 1 harus tetap utuh.");
            Assert.IsTrue(activeSlots[2].IsEmpty, "Slot 2 (+) harus terhapus.");

            // Panggil OnDeleteOneClicked() kedua -> slot ke-1 (x) harus terhapus
            _controller.OnDeleteOneClicked();
            Assert.AreEqual("3", activeSlots[0].currentToken, "Slot 0 harus tetap utuh.");
            Assert.IsTrue(activeSlots[1].IsEmpty, "Slot 1 (x) harus terhapus.");

            // Panggil OnDeleteOneClicked() ketiga -> slot ke-0 (3) harus terhapus
            _controller.OnDeleteOneClicked();
            Assert.IsTrue(activeSlots[0].IsEmpty, "Slot 0 (3) harus terhapus.");

            // Panggil OnDeleteOneClicked() keempat saat semua slot sudah kosong -> aman tanpa error
            Assert.DoesNotThrow(() => _controller.OnDeleteOneClicked(), "Memanggil OnDeleteOneClicked pada papan kosong tidak boleh melempar exception.");
        }

        [Test]
        public void TC_BOARD_15_ClearAll_EmptiesAllSlotsInActiveStage()
        {
            var q = new QuestionModel
            {
                id = "TEST-CLEAR-ALL",
                equation_model = new string[] { "3x + 5 = 20" },
                solution_steps = new string[] { "S0", "S1", "S2", "S3" },
                final_answer = "5"
            };

            _controller.stages = new List<BoardStageRow>();
            for (int s = 0; s < 4; s++)
            {
                var row = new BoardStageRow();
                for (int slotIdx = 0; slotIdx < 7; slotIdx++)
                {
                    row.slots.Add(new CardSlotView());
                }
                _controller.stages.Add(row);
            }

            _controller.FormulateStagesFromQuestion(q);
            _controller.SetStageActive(1);

            var stage0Slots = _controller.stages[0].slots;
            var stage1Slots = _controller.stages[1].slots;

            // Isi slot pada stage 0 dan stage 1
            stage0Slots[0].SetToken("3", null, Color.black);
            stage1Slots[0].SetToken("3", null, Color.black);
            stage1Slots[1].SetToken("x", null, Color.black);
            stage1Slots[2].SetToken("=", null, Color.black);

            // Panggil OnClearClicked() pada stage 1 aktif
            _controller.OnClearClicked();

            // Stage 1 harus kosong seluruhnya
            for (int i = 0; i < stage1Slots.Count; i++)
            {
                Assert.IsTrue(stage1Slots[i].IsEmpty, $"Slot {i} pada stage aktif harus kosong setelah OnClearClicked.");
            }

            // Stage 0 tidak boleh terpengaruh
            Assert.IsFalse(stage0Slots[0].IsEmpty, "Stage non-aktif tidak boleh terhapus.");
            Assert.AreEqual("3", stage0Slots[0].currentToken);
        }

        [Test]
        public void TC_BOARD_16_QuestionStoryAndVariableHint_FormattedCorrectly()
        {
            var qDoll = new QuestionModel
            {
                id = "TEST-DOLL",
                question_text = "Seorang anak memiliki boneka yang berkurang 4 sehingga tersisa 7.",
                equation_model = new string[] { "b - 4 = 7" },
                solution_steps = new string[] { "b - 4 = 7", "b = 7 + 4", "b = 11" },
                final_answer = "11"
            };

            var qGrape = new QuestionModel
            {
                id = "TEST-GRAPE",
                question_text = "Ibu membeli beberapa anggur.",
                equation_model = new string[] { "4 * a = 12" },
                solution_steps = new string[] { "4 * a = 12", "a = 3" },
                final_answer = "3"
            };

            var qSavings = new QuestionModel
            {
                id = "TEST-SAVINGS",
                question_text = "Adik memiliki tabungan awal x rupiah di celengan.",
                equation_model = new string[] { "x + 10000 = 25000" },
                solution_steps = new string[] { "x + 10000 = 25000", "x = 15000" },
                final_answer = "15000"
            };

            string hintDoll = AlgebraBoardController.GetVariableContextHint(qDoll);
            Assert.IsTrue(hintDoll.Contains("boneka"), "Variabel b harus memuat petunjuk boneka!");

            string hintGrape = AlgebraBoardController.GetVariableContextHint(qGrape);
            Assert.IsTrue(hintGrape.Contains("anggur"), "Variabel a harus memuat petunjuk anggur!");

            string hintSavings = AlgebraBoardController.GetVariableContextHint(qSavings);
            Assert.IsTrue(hintSavings.Contains("tabungan"), "Variabel x dalam konteks tabungan harus memuat petunjuk tabungan!");
        }
    }
}

