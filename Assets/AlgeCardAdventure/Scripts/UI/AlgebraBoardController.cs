using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlgeCardAdventure.Core;
using AlgeCardAdventure.Data;
using AlgeCardAdventure.Session;
using AlgeCardAdventure.Audio;

namespace AlgeCardAdventure.UI
{
    /// <summary>
    /// Representasi status satu slot kartu di papan aljabar.
    /// </summary>
    [Serializable]
    public class CardSlotView
    {
        public Button slotButton;
        public Image slotImage;
        public Text slotText;
        public string currentToken = "";

        public bool IsEmpty => string.IsNullOrEmpty(currentToken);

        public void SetToken(string token, Sprite filledSprite, Color textColor)
        {
            currentToken = token;
            if (slotText != null)
            {
                slotText.text = token;
                slotText.color = textColor;
            }
            if (slotImage != null && filledSprite != null)
            {
                slotImage.sprite = filledSprite;
                slotImage.color = Color.white;
            }
        }

        public void Clear(Sprite emptySprite)
        {
            currentToken = "";
            if (slotText != null)
            {
                slotText.text = "";
            }
            if (slotImage != null && emptySprite != null)
            {
                slotImage.sprite = emptySprite;
                slotImage.color = new Color(1f, 1f, 1f, 0.7f);
            }
        }
    }

    /// <summary>
    /// Baris tahap papan aljabar (Stage Row).
    /// </summary>
    [Serializable]
    public class BoardStageRow
    {
        public string stageName;
        public GameObject rowContainer;
        public Text stageTitleText;
        public Text stageInstructionText;
        public Image rowBackground;
        public List<CardSlotView> slots = new List<CardSlotView>();
        public List<string> targetTokens = new List<string>();
        public bool isCompleted = false;
        public bool isUnlocked = false;
    }

    /// <summary>
    /// Controller utama untuk Layar 04 (Algebra Card Board).
    /// Mengelola pemuatan dinamis soal, pembentukan 4 tahap langkah aljabar,
    /// penempatan kartu dek (Tap-to-Slot), penghapusan slot (Tap-to-Clear),
    /// tombol reset baris, validasi ekspresi per langkah, serta transisi ke layar berikutnya.
    /// </summary>
    public class AlgebraBoardController : MonoBehaviour
    {
        [Header("Header & Info Soal")]
        [SerializeField] private Button backButton;
        [SerializeField] private Text headerLevelText;
        [SerializeField] private Text questionBodyText;

        [Header("Papan Aljabar (4 Tahap)")]
        [SerializeField] private List<BoardStageRow> stages = new List<BoardStageRow>();

        [Header("Palet Dek Kartu (Deck of Cards)")]
        [SerializeField] private Transform numberDeckContainer;
        [SerializeField] private Transform variableDeckContainer;
        [SerializeField] private Transform operatorDeckContainer;
        [SerializeField] private List<Button> numberButtons = new List<Button>();
        [SerializeField] private List<Button> variableButtons = new List<Button>();
        [SerializeField] private List<Button> operatorButtons = new List<Button>();

        [Header("Tombol Aksi Bawah")]
        [SerializeField] private Button continueButton;
        [SerializeField] private Text continueButtonText;
        [SerializeField] private Button resetStepButton;
        [SerializeField] private Text feedbackText;

        [Header("Aset Visual Sprite")]
        [SerializeField] private Sprite emptySlotSprite;
        [SerializeField] private Sprite whiteTileSprite;
        [SerializeField] private Sprite orangeTileSprite;
        [SerializeField] private Sprite greenTileSprite;
        [SerializeField] private Sprite panelNormalSprite;
        [SerializeField] private Sprite panelCompleteSprite;

        // State Runtime
        public int CurrentStageIndex { get; private set; } = 0;
        public QuestionModel ActiveQuestion { get; private set; }
        public bool IsAllStagesCompleted { get; private set; } = false;

        private void Start()
        {
            InitializeBoard();
            SetupButtonListeners();
        }

        /// <summary>
        /// Inisialisasi papan aljabar dengan data soal dari GameSession atau fallback default.
        /// </summary>
        public void InitializeBoard()
        {
            ActiveQuestion = GameSession.Instance.CurrentQuestion;
            if (ActiveQuestion == null)
            {
                ActiveQuestion = CreateFallbackQuestion();
            }

            // Tampilkan info header
            if (headerLevelText != null)
            {
                string lvl = !string.IsNullOrEmpty(ActiveQuestion.ui_level) ? ActiveQuestion.ui_level : "Easy";
                headerLevelText.text = $"{lvl} - Variables & Algebra";
            }

            if (questionBodyText != null)
            {
                questionBodyText.text = ActiveQuestion.question_text;
            }

            // Bentuk target token 4 tahap dari soal aktif
            FormulateStagesFromQuestion(ActiveQuestion);

            // Buka tahap pertama (Stage 0) dan kunci tahap lainnya
            SetStageActive(0);

            UpdateDeckInteractivity();
            UpdateContinueButtonVisual();

            if (feedbackText != null)
            {
                feedbackText.text = "Arrange the cards according to the mission!";
                feedbackText.color = new Color(0.2f, 0.2f, 0.2f, 1f);
            }
        }

        private void SetupButtonListeners()
        {
            if (backButton != null)
            {
                backButton.onClick.RemoveAllListeners();
                backButton.onClick.AddListener(OnBackClicked);
            }

            if (continueButton != null)
            {
                continueButton.onClick.RemoveAllListeners();
                continueButton.onClick.AddListener(OnContinueClicked);
            }

            if (resetStepButton != null)
            {
                resetStepButton.onClick.RemoveAllListeners();
                resetStepButton.onClick.AddListener(OnResetStepClicked);
            }

            // Pasang listener Tap-to-Slot untuk tombol kartu dek angka
            for (int i = 0; i < numberButtons.Count; i++)
            {
                Button btn = numberButtons[i];
                if (btn != null)
                {
                    Text t = btn.GetComponentInChildren<Text>();
                    string val = (t != null) ? t.text : i.ToString();
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(() => OnDeckCardClicked(val, "number"));
                }
            }

            // Pasang listener Tap-to-Slot untuk tombol variabel
            foreach (var btn in variableButtons)
            {
                if (btn != null)
                {
                    Text t = btn.GetComponentInChildren<Text>();
                    string val = (t != null) ? t.text : "x";
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(() => OnDeckCardClicked(val, "variable"));
                }
            }

            // Pasang listener Tap-to-Slot untuk tombol operator
            foreach (var btn in operatorButtons)
            {
                if (btn != null)
                {
                    Text t = btn.GetComponentInChildren<Text>();
                    string val = (t != null) ? t.text : "+";
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(() => OnDeckCardClicked(val, "operator"));
                }
            }

            // Pasang listener Tap-to-Clear untuk setiap slot di setiap tahap
            for (int s = 0; s < stages.Count; s++)
            {
                int stageIdx = s;
                var stage = stages[s];
                for (int slotIdx = 0; slotIdx < stage.slots.Count; slotIdx++)
                {
                    int capturedSlotIdx = slotIdx;
                    var slot = stage.slots[slotIdx];
                    if (slot.slotButton != null)
                    {
                        slot.slotButton.onClick.RemoveAllListeners();
                        slot.slotButton.onClick.AddListener(() => OnSlotClicked(stageIdx, capturedSlotIdx));
                    }
                }
            }
        }

        /// <summary>
        /// Mengekstrak target token untuk 4 tahap aljabar dari data soal.
        /// Mengadaptasi equation_model, solution_steps, dan final_answer.
        /// </summary>
        public void FormulateStagesFromQuestion(QuestionModel question)
        {
            if (question == null) return;

            if (stages == null) stages = new List<BoardStageRow>();
            while (stages.Count < 4)
            {
                stages.Add(new BoardStageRow());
            }

            // Stage 0: Persamaan Awal (Card Board: Arrange the Equations)
            stages[0].stageName = "Arrange the Equations";
            if (stages[0].stageTitleText != null) stages[0].stageTitleText.text = "Card Board: Arrange the Equations";
            if (stages[0].stageInstructionText != null) stages[0].stageInstructionText.text = "Arrange the number and variable cards to form an equation.";
            
            string eq0 = (question.equation_model != null && question.equation_model.Length > 0)
                ? question.equation_model[0]
                : "3x + 5 = 20";
            stages[0].targetTokens = TokenizeExpression(eq0);

            // Stage 1: Langkah Solusi 1
            stages[1].stageName = "Step 1";
            if (stages[1].stageTitleText != null) stages[1].stageTitleText.text = "Card Board: Step 1";
            string step1Desc = "Move the cards to subtract from both sides.";
            string step1Eq = "";

            if (question.solution_steps != null && question.solution_steps.Length > 2)
            {
                // Biasanya solution_steps[2] adalah operasi aljabar pertama misal "x = 25000 - 10000"
                step1Eq = question.solution_steps[2];
                step1Desc = question.solution_steps[1];
            }
            else
            {
                step1Eq = "3x = 20 - 5";
            }
            if (stages[1].stageInstructionText != null) stages[1].stageInstructionText.text = step1Desc;
            stages[1].targetTokens = TokenizeExpression(step1Eq);

            // Stage 2: Langkah Solusi 2 (Penyederhanaan atau pembagian)
            stages[2].stageName = "Step 2";
            if (stages[2].stageTitleText != null) stages[2].stageTitleText.text = "Card Board: Step 2";
            string step2Desc = "Simplify the expression.";
            string step2Eq = "";

            if (question.solution_steps != null && question.solution_steps.Length > 3)
            {
                step2Eq = question.solution_steps[3];
                step2Desc = "Simplify to resolve the variable.";
            }
            else
            {
                step2Eq = "3x = 15 / 3";
            }
            if (stages[2].stageInstructionText != null) stages[2].stageInstructionText.text = step2Desc;
            stages[2].targetTokens = TokenizeExpression(step2Eq);

            // Stage 3: Hasil Akhir (Final Variable Result)
            stages[3].stageName = "Card Arrangement Results";
            if (stages[3].stageTitleText != null) stages[3].stageTitleText.text = "Card Arrangement Results";
            if (stages[3].stageInstructionText != null) stages[3].stageInstructionText.text = "The final result after the variables are resolved.";

            string finalAnswer = !string.IsNullOrEmpty(question.final_answer) ? question.final_answer : "5";
            // Normalisasi final answer ke format variable = answer
            string varName = ExtractVariableFromQuestion(question);
            string cleanAnswerDigits = System.Text.RegularExpressions.Regex.Replace(finalAnswer, @"[^\d]", "");
            if (string.IsNullOrEmpty(cleanAnswerDigits)) cleanAnswerDigits = finalAnswer.Trim();
            
            string finalExpr = $"{varName} = {cleanAnswerDigits}";
            stages[3].targetTokens = TokenizeExpression(finalExpr);

            // Sesuaikan ketersediaan slot dengan jumlah target tokens
            for (int s = 0; s < stages.Count; s++)
            {
                var stage = stages[s];
                if (stage.slots == null) stage.slots = new List<CardSlotView>();
                for (int i = 0; i < stage.slots.Count; i++)
                {
                    bool isActiveSlot = (i < stage.targetTokens.Count);
                    if (stage.slots[i] != null)
                    {
                        if (stage.slots[i].slotButton != null)
                        {
                            stage.slots[i].slotButton.gameObject.SetActive(isActiveSlot);
                        }
                        stage.slots[i].Clear(emptySlotSprite);
                    }
                }
            }
        }

        /// <summary>
        /// Memecah string ekspresi aljabar menjadi list token terpisah.
        /// Contoh: "3x + 5 = 20" -> ["3", "x", "+", "5", "=", "20"]
        /// "y - 3 = 5" -> ["y", "-", "3", "=", "5"]
        /// </summary>
        public static List<string> TokenizeExpression(string expr)
        {
            var tokens = new List<string>();
            if (string.IsNullOrWhiteSpace(expr)) return tokens;

            // Bersihkan teks ekstra jika ada awalan kata
            int eqPos = expr.IndexOf('=');
            if (eqPos >= 0)
            {
                // Ambil ekspresi seputar tanda sama dengan
                int start = 0;
                for (int i = eqPos - 1; i >= 0; i--)
                {
                    if (char.IsLetterOrDigit(expr[i]) || expr[i] == '+' || expr[i] == '-' || expr[i] == '*' || expr[i] == '/' || expr[i] == ' ')
                    {
                        start = i;
                    }
                    else break;
                }
                expr = expr.Substring(start).Trim();
            }

            int idx = 0;
            while (idx < expr.Length)
            {
                char c = expr[idx];
                if (char.IsWhiteSpace(c))
                {
                    idx++;
                    continue;
                }

                if (c == '+' || c == '-' || c == '=' || c == '/' || c == '*')
                {
                    tokens.Add(c.ToString());
                    idx++;
                    continue;
                }

                if (char.IsDigit(c))
                {
                    int startDigit = idx;
                    while (idx < expr.Length && char.IsDigit(expr[idx]))
                    {
                        idx++;
                    }
                    tokens.Add(expr.Substring(startDigit, idx - startDigit));
                    continue;
                }

                if (char.IsLetter(c))
                {
                    tokens.Add(c.ToString().ToLower());
                    idx++;
                    continue;
                }

                idx++;
            }

            return tokens;
        }

        private string ExtractVariableFromQuestion(QuestionModel q)
        {
            if (q.equation_model != null && q.equation_model.Length > 0)
            {
                foreach (char c in q.equation_model[0])
                {
                    if (char.IsLetter(c)) return c.ToString().ToLower();
                }
            }
            return "x";
        }

        private QuestionModel CreateFallbackQuestion()
        {
            return new QuestionModel
            {
                id = "DEFAULT-04",
                ui_level = "Easy",
                title = "Card Board Mission",
                question_text = "If 3x + 5 = 20, find the value of x. Use number and variable cards to create equations and solve them.",
                equation_model = new string[] { "3x + 5 = 20" },
                solution_steps = new string[] { "Let variable be x", "Subtract 5 from both sides", "3x = 20 - 5", "3x = 15 / 3" },
                final_answer = "5"
            };
        }

        /// <summary>
        /// Mengatur tahap aktif (0, 1, 2, atau 3).
        /// Tahap sebelum stageIndex dikunci dengan status selesai (hijau/emas).
        /// Tahap setelah stageIndex dikunci (transparan/terkunci).
        /// </summary>
        public void SetStageActive(int stageIndex)
        {
            if (stages == null) stages = new List<BoardStageRow>();
            CurrentStageIndex = stageIndex;
            IsAllStagesCompleted = (CurrentStageIndex >= stages.Count);

            for (int i = 0; i < stages.Count; i++)
            {
                var stage = stages[i];
                if (stage == null) continue;

                if (i < stageIndex)
                {
                    stage.isCompleted = true;
                    stage.isUnlocked = true;
                    SetRowInteractable(stage, false, new Color(0.95f, 1f, 0.95f, 1f));
                }
                else if (i == stageIndex)
                {
                    stage.isCompleted = false;
                    stage.isUnlocked = true;
                    SetRowInteractable(stage, true, Color.white);
                }
                else
                {
                    stage.isCompleted = false;
                    stage.isUnlocked = false;
                    SetRowInteractable(stage, false, new Color(0.7f, 0.7f, 0.7f, 0.5f));
                }
            }

            UpdateContinueButtonVisual();
        }

        private void SetRowInteractable(BoardStageRow stage, bool interactable, Color tint)
        {
            if (stage == null) return;

            if (stage.rowBackground != null)
            {
                stage.rowBackground.color = tint;
            }

            if (stage.slots != null)
            {
                foreach (var slot in stage.slots)
                {
                    if (slot != null && slot.slotButton != null)
                    {
                        slot.slotButton.interactable = interactable;
                    }
                }
            }
        }

        /// <summary>
        /// Handler saat kartu pada dek diklik (Tap-to-Slot).
        /// Mengisi slot kosong pertama pada tahap aktif.
        /// </summary>
        public void OnDeckCardClicked(string token, string cardType)
        {
            if (IsAllStagesCompleted || CurrentStageIndex >= stages.Count) return;

            var currentStage = stages[CurrentStageIndex];
            int maxSlots = currentStage.targetTokens.Count;

            // Cari slot kosong pertama
            CardSlotView emptySlot = null;
            for (int i = 0; i < maxSlots && i < currentStage.slots.Count; i++)
            {
                if (currentStage.slots[i].IsEmpty)
                {
                    emptySlot = currentStage.slots[i];
                    break;
                }
            }

            if (emptySlot != null)
            {
                Sprite tileSprite = whiteTileSprite;
                Color textColor = new Color(0.12f, 0.23f, 0.54f, 1f); // Dark blue

                if (cardType == "variable")
                {
                    tileSprite = greenTileSprite != null ? greenTileSprite : whiteTileSprite;
                    textColor = new Color(0.02f, 0.35f, 0.15f, 1f); // Dark green
                }
                else if (cardType == "operator")
                {
                    tileSprite = orangeTileSprite != null ? orangeTileSprite : whiteTileSprite;
                    textColor = new Color(0.55f, 0.25f, 0.02f, 1f); // Dark amber
                }

                emptySlot.SetToken(token, tileSprite, textColor);
                AudioManager.Instance.PlayCardFlip();

                // Cek jika seluruh slot telah terisi
                CheckIfRowIsComplete(currentStage);
            }
        }

        /// <summary>
        /// Handler saat slot kartu pada papan diklik (Tap-to-Clear).
        /// Sesuai instruksi Yogi Ario: fitur menghapus angka/kartu dan mengembalikan slot menjadi kosong.
        /// </summary>
        public void OnSlotClicked(int stageIndex, int slotIndex)
        {
            // Hanya izinkan mengosongkan slot pada tahap yang sedang aktif
            if (stageIndex != CurrentStageIndex || IsAllStagesCompleted) return;

            var stage = stages[stageIndex];
            if (slotIndex >= 0 && slotIndex < stage.slots.Count)
            {
                var slot = stage.slots[slotIndex];
                if (!slot.IsEmpty)
                {
                    slot.Clear(emptySlotSprite);
                    AudioManager.Instance.PlayButtonClick();

                    if (feedbackText != null)
                    {
                        feedbackText.text = "Card removed. Choose another card!";
                        feedbackText.color = new Color(0.3f, 0.3f, 0.3f, 1f);
                    }
                }
            }
        }

        /// <summary>
        /// Handler tombol Reset Step / Clear Baris Aktif.
        /// Mengosongkan seluruh slot pada tahap aktif.
        /// </summary>
        public void OnResetStepClicked()
        {
            if (IsAllStagesCompleted || CurrentStageIndex >= stages.Count) return;

            var currentStage = stages[CurrentStageIndex];
            foreach (var slot in currentStage.slots)
            {
                slot.Clear(emptySlotSprite);
            }

            AudioManager.Instance.PlayButtonClick();

            if (feedbackText != null)
            {
                feedbackText.text = "Step cleared. Arrange your cards!";
                feedbackText.color = new Color(0.3f, 0.3f, 0.3f, 1f);
            }
        }

        private void CheckIfRowIsComplete(BoardStageRow stage)
        {
            int filledCount = 0;
            int totalNeeded = stage.targetTokens.Count;

            for (int i = 0; i < totalNeeded && i < stage.slots.Count; i++)
            {
                if (!stage.slots[i].IsEmpty) filledCount++;
            }

            if (filledCount == totalNeeded)
            {
                if (feedbackText != null)
                {
                    feedbackText.text = "Row complete! Press 'Continue' to verify.";
                    feedbackText.color = new Color(0.05f, 0.45f, 0.15f, 1f);
                }
            }
        }

        /// <summary>
        /// Handler tombol Continue > (Verifikasi jawaban langkah aktif, atau lanjut ke layar berikutnya).
        /// </summary>
        public void OnContinueClicked()
        {
            AudioManager.Instance.PlayButtonClick();

            if (IsAllStagesCompleted)
            {
                // Navigasi ke Layar 05 (True or False) atau Layar 06 (Score Results)
                if (ActiveQuestion != null && ActiveQuestion.HasTrueFalseReady)
                {
                    SceneNavigator.LoadScene(SceneNavigator.SceneTrueOrFalse);
                }
                else
                {
                    SceneNavigator.LoadScene(SceneNavigator.SceneScoreResults);
                }
                return;
            }

            var currentStage = stages[CurrentStageIndex];
            bool isCorrect = ValidateStage(currentStage);

            if (isCorrect)
            {
                AudioManager.Instance.PlayCorrect();

                if (feedbackText != null)
                {
                    feedbackText.text = "Great job! Step verified!";
                    feedbackText.color = new Color(0.05f, 0.55f, 0.15f, 1f);
                }

                // Kunci baris saat ini dengan sukses
                currentStage.isCompleted = true;
                if (panelCompleteSprite != null && currentStage.rowBackground != null)
                {
                    currentStage.rowBackground.sprite = panelCompleteSprite;
                }

                // Maju ke tahap berikutnya
                int nextStage = CurrentStageIndex + 1;
                SetStageActive(nextStage);

                if (nextStage >= stages.Count)
                {
                    IsAllStagesCompleted = true;
                    if (feedbackText != null)
                    {
                        feedbackText.text = "All steps solved! Tap 'Continue' to finish!";
                        feedbackText.color = new Color(0.75f, 0.45f, 0.05f, 1f);
                    }
                    UpdateContinueButtonVisual();
                }
            }
            else
            {
                AudioManager.Instance.PlayWrong();

                if (feedbackText != null)
                {
                    feedbackText.text = "Cards don't match the target equation. Tap card to clear and try again!";
                    feedbackText.color = new Color(0.8f, 0.15f, 0.1f, 1f);
                }
            }
        }

        /// <summary>
        /// Memvalidasi susunan kartu di tahap tertentu dengan target tokens.
        /// </summary>
        public bool ValidateStage(BoardStageRow stage)
        {
            if (stage == null || stage.targetTokens == null) return false;

            int targetCount = stage.targetTokens.Count;
            for (int i = 0; i < targetCount; i++)
            {
                if (i >= stage.slots.Count) return false;
                string playerToken = stage.slots[i].currentToken;
                string targetToken = stage.targetTokens[i];

                if (!string.Equals(playerToken.Trim(), targetToken.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        public void OnBackClicked()
        {
            AudioManager.Instance.PlayButtonClick();
            SceneNavigator.GoToMissionCard();
        }

        private void UpdateContinueButtonVisual()
        {
            if (continueButtonText != null)
            {
                if (IsAllStagesCompleted)
                {
                    continueButtonText.text = "Next Stage  >";
                }
                else
                {
                    continueButtonText.text = "Continue  >";
                }
            }
        }

        private void UpdateDeckInteractivity()
        {
            // Pastikan semua tombol dek aktif untuk interaksi
            foreach (var b in numberButtons) if (b != null) b.interactable = true;
            foreach (var b in variableButtons) if (b != null) b.interactable = true;
            foreach (var b in operatorButtons) if (b != null) b.interactable = true;
        }
    }
}
