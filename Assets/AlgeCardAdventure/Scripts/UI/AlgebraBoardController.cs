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
    /// Mendukung kartu statis (bagian persamaan awal) dan slot interaktif (yang diisi pemain).
    /// </summary>
    [Serializable]
    public class CardSlotView
    {
        public Button slotButton;
        public Image slotImage;
        public Text slotText;
        public string currentToken = "";
        public bool isTargetSlot = true; // true jika slot kosong interaktif; false jika kartu statis

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
    /// Memuat judul, ikon, instruksi, status badge (DONE, ACTIVE, LOCKED),
    /// daftar slot kartu, target token, dan panah penghubung ke tahap berikutnya.
    /// </summary>
    [Serializable]
    public class BoardStageRow
    {
        public string stageName;
        public GameObject rowContainer;
        public Text stageTitleText;
        public Text stageIconText;
        public Text stageInstructionText;
        public Text badgeStatusText;
        public Image badgeStatusImage;
        public Image rowBackground;
        public GameObject stepArrowDown;
        public List<CardSlotView> slots = new List<CardSlotView>();
        public List<string> targetTokens = new List<string>();
        public bool isCompleted = false;
        public bool isUnlocked = false;
    }

    /// <summary>
    /// Controller utama untuk Layar 04 (Algebra Card Board).
    /// Sepenuhnya selaras dengan acuan prototype Docs/Ui Template/04_algebra_card_board/code.html:
    /// - Top Header Bar (Back button, Logo AlgeCard Adventure, Mission Plaque).
    /// - Question Banner (Bingkai kayu, ornamen daun, badge Prof. Bug, highlight formula).
    /// - 4 Solving Stages (Ikon, Judul, Status Badge dinamis, Panah emas, Slot placeholder interaktif).
    /// - Deck of Cards Sidebar (Number Cards 0-9 biru, Variable Cards hijau, Operators kuning, 3D CLEAR button, D-Pad Nav Pad).
    /// - Kontrol Aksi Bawah (Tombol emas Continue/Complete Quest, Tombol cyan More Details / Review, Strip dekorasi petualangan).
    /// - Adaptasi dinamis dari banksoal.json.
    /// </summary>
    public class AlgebraBoardController : MonoBehaviour
    {
        [Header("Top Header Bar")]
        [SerializeField] private Button backButton;
        [SerializeField] private Image logoPlaqueImage;
        [SerializeField] private Text missionPlaqueLevelText;

        [Header("Question Plaque Banner")]
        [SerializeField] private Text questionBodyText;
        [SerializeField] private Text questionHighlightedFormulaText;

        [Header("Papan Aljabar (4 Tahap)")]
        [SerializeField] private ScrollRect mainScrollRect;
        [SerializeField] private RectTransform scrollContent;
        [SerializeField] public List<BoardStageRow> stages = new List<BoardStageRow>();

        [Header("Palet Dek Kartu (Deck of Cards)")]
        [SerializeField] private List<Button> numberButtons = new List<Button>();
        [SerializeField] private List<Button> variableButtons = new List<Button>();
        [SerializeField] private List<Button> operatorButtons = new List<Button>();
        [SerializeField] private Button customVariableButton;
        [SerializeField] private Text customVariableText;
        [SerializeField] private Button deleteOneButton;
        [SerializeField] private Button clearButton;
        [SerializeField] private Button clearAllButton;

        [Header("Adventure Compass D-Pad / NAV PAD")]
        [SerializeField] private Button dpadUp;
        [SerializeField] private Button dpadDown;
        [SerializeField] private Button dpadLeft;
        [SerializeField] private Button dpadRight;
        [SerializeField] private Button dpadCenter;

        [Header("Tombol Aksi Bawah (Footer)")]
        [SerializeField] private Button continueButton;
        [SerializeField] private Text continueLabelText;
        [SerializeField] private Text continueSubText;
        [SerializeField] private Button moreDetailsButton;
        [SerializeField] private Text moreDetailsSubText;
        [SerializeField] private Button resetStepButton; // legacy fallback support

        [Header("Aksesori & Feedback")]
        [SerializeField] private GameObject feedbackBar;
        [SerializeField] private Text feedbackText;
        [SerializeField] private Text levelBadgeText;
        [SerializeField] private Text stageBadgeIndicatorText;

        [Header("Aset Visual Sprite")]
        [SerializeField] private Sprite emptySlotSprite;
        [SerializeField] private Sprite whiteTileSprite;
        [SerializeField] private Sprite orangeTileSprite;
        [SerializeField] private Sprite greenTileSprite;
        [SerializeField] private Sprite badgeDoneSprite;
        [SerializeField] private Sprite badgeActiveSprite;
        [SerializeField] private Sprite badgeLockedSprite;
        [SerializeField] private Sprite panelNormalSprite;
        [SerializeField] private Sprite panelCompleteSprite;

        [Header("Level Hard Mode SPLDV (Mockup Alignment)")]
        [SerializeField] private GameObject modeRibbonContainer;
        [SerializeField] private Text modeRibbonText;
        [SerializeField] private GameObject eliminationOpPanel;
        [SerializeField] private Button addEquationButton;
        [SerializeField] private Button subtractEquationButton;
        [SerializeField] private Text addEquationText;
        [SerializeField] private Text subtractEquationText;

        public enum HardAlgebraMode
        {
            None,
            Elimination,
            Substitution
        }

        public HardAlgebraMode ActiveHardMode { get; private set; } = HardAlgebraMode.None;
        public string SelectedEliminationOp { get; private set; } = "";

        // Button Accessor Properties for Testing & Scripting
        public GameObject ModeRibbonContainer
        {
            get => modeRibbonContainer;
            set => modeRibbonContainer = value;
        }

        public Text ModeRibbonText
        {
            get => modeRibbonText;
            set => modeRibbonText = value;
        }

        public GameObject EliminationOpPanel
        {
            get => eliminationOpPanel;
            set => eliminationOpPanel = value;
        }

        public Button AddEquationButton
        {
            get => addEquationButton;
            set => addEquationButton = value;
        }

        public Button SubtractEquationButton
        {
            get => subtractEquationButton;
            set => subtractEquationButton = value;
        }

        public Text AddEquationText
        {
            get => addEquationText;
            set => addEquationText = value;
        }

        public Text SubtractEquationText
        {
            get => subtractEquationText;
            set => subtractEquationText = value;
        }

        public Button CustomVariableButton
        {
            get => customVariableButton;
            set => customVariableButton = value;
        }

        public Text CustomVariableText
        {
            get => customVariableText;
            set => customVariableText = value;
        }

        public string CurrentCustomVariable { get; private set; } = "x";

        public Button DeleteOneButton
        {
            get => deleteOneButton;
            set => deleteOneButton = value;
        }

        public Button ClearAllButton
        {
            get => clearAllButton != null ? clearAllButton : clearButton;
            set => clearAllButton = value;
        }

        public Button ClearButton
        {
            get => ClearAllButton;
            set => clearButton = value;
        }

        // State Runtime
        public int CurrentStageIndex { get; private set; } = 0;
        public QuestionModel ActiveQuestion { get; private set; }
        public bool IsAllStagesCompleted { get; private set; } = false;

        private Coroutine _noticeCoroutine;
        private Coroutine _pulseCoroutine;

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
            if (missionPlaqueLevelText != null)
            {
                string lvl = !string.IsNullOrEmpty(ActiveQuestion.ui_level) ? ActiveQuestion.ui_level : "Easy";
                missionPlaqueLevelText.text = $"{lvl} · Variables";
            }

            if (levelBadgeText != null)
            {
                string lvl = !string.IsNullOrEmpty(ActiveQuestion.ui_level) ? ActiveQuestion.ui_level : "Level 1";
                levelBadgeText.text = $"{lvl}";
            }

            // Tampilkan narasi soal dan formula
            string formula = (ActiveQuestion.equation_model != null && ActiveQuestion.equation_model.Length > 0)
                ? ActiveQuestion.equation_model[0]
                : "3x + 5 = 20";

            string targetVar = ExtractVariableFromQuestion(ActiveQuestion);
            string varHint = GetVariableContextHint(ActiveQuestion);

            // Pastikan FormulaBox tersembunyi agar tidak bertabrakan dengan teks soal cerita (DEC-52)
            if (questionHighlightedFormulaText != null)
            {
                questionHighlightedFormulaText.text = formula;
                try
                {
                    if (questionHighlightedFormulaText.transform.parent != null &&
                        questionHighlightedFormulaText.transform.parent.name.Contains("FormulaBox"))
                    {
                        questionHighlightedFormulaText.transform.parent.gameObject.SetActive(false);
                    }
                }
                catch { }
            }

            // Deteksi Mode Aljabar Khusus Level Hard (Eliminasi vs Substitusi)
            bool isHard = string.Equals(ActiveQuestion.ui_level, "Hard", StringComparison.OrdinalIgnoreCase);
            if (isHard)
            {
                string method = ActiveQuestion.method != null ? ActiveQuestion.method.ToLowerInvariant() : "";
                if (method == "elimination" || (ActiveQuestion.equation_model != null && ActiveQuestion.equation_model.Length > 1 && string.IsNullOrEmpty(method)))
                {
                    ActiveHardMode = HardAlgebraMode.Elimination;
                    if (modeRibbonContainer != null) modeRibbonContainer.SetActive(true);
                    if (modeRibbonText != null) modeRibbonText.text = "⚔️ ELIMINATION MODE";
                }
                else if (method == "substitution")
                {
                    ActiveHardMode = HardAlgebraMode.Substitution;
                    if (modeRibbonContainer != null) modeRibbonContainer.SetActive(true);
                    if (modeRibbonText != null) modeRibbonText.text = "🔄 SUBSTITUTION MODE";
                }
                else
                {
                    ActiveHardMode = HardAlgebraMode.None;
                    if (modeRibbonContainer != null) modeRibbonContainer.SetActive(false);
                }
            }
            else
            {
                ActiveHardMode = HardAlgebraMode.None;
                if (modeRibbonContainer != null) modeRibbonContainer.SetActive(false);
            }

            if (questionBodyText != null)
            {
                // Proteksi runtime anti-meluber frame (Catatan Revisi 1 Butir 4)
                try
                {
                    questionBodyText.horizontalOverflow = HorizontalWrapMode.Wrap;
                    questionBodyText.verticalOverflow = VerticalWrapMode.Truncate;
                    questionBodyText.resizeTextForBestFit = true;
                    questionBodyText.resizeTextMinSize = 13;
                    questionBodyText.resizeTextMaxSize = 18;
                }
                catch { }

                if (isHard && ActiveQuestion.equation_model != null && ActiveQuestion.equation_model.Length > 1)
                {
                    string eq1 = ActiveQuestion.equation_model[0];
                    string eq2 = ActiveQuestion.equation_model[1];
                    string modeLabel = ActiveHardMode == HardAlgebraMode.Elimination ? "Metode Eliminasi" : "Metode Substitusi";
                    if (!string.IsNullOrWhiteSpace(ActiveQuestion.question_text))
                    {
                        questionBodyText.text = $"{ActiveQuestion.question_text}\n💡 <b>Mode:</b> {modeLabel} · Cari nilai x dan y";
                    }
                    else
                    {
                        questionBodyText.text = $"Selesaikan sistem persamaan linear dua variabel:\n<b>(1) {eq1}</b>\n<b>(2) {eq2}</b>\n💡 <b>Mode:</b> {modeLabel}";
                    }
                }
                else
                {
                    // Tampilkan soal cerita dan petunjuk variabel (Catatan Revisi 1 Butir 5 & 6)
                    if (!string.IsNullOrWhiteSpace(ActiveQuestion.question_text))
                    {
                        questionBodyText.text = $"{ActiveQuestion.question_text}\n💡 <b>Petunjuk:</b> {varHint}";
                    }
                    else
                    {
                        questionBodyText.text = $"Selesaikan persamaan aljabar berikut:\n<color=#003882><b>{formula}</b></color> (Tentukan nilai {targetVar}!)\n💡 <b>Petunjuk:</b> {varHint}";
                    }
                }
            }

            // Setel kartu variabel kustom default sesuai variabel soal aktif (DEC-51)
            if (ActiveQuestion != null)
            {
                string detectedVar = ExtractVariableFromQuestion(ActiveQuestion);
                SetCustomVariable(detectedVar);
            }

            // Bentuk target token dan struktur 4 tahap dari soal aktif
            FormulateStagesFromQuestion(ActiveQuestion);

            // Buka tahap pertama (Stage 0) dan kunci tahap lainnya
            SetStageActive(0);

            UpdateDeckInteractivity();
            UpdateActionButtonsVisual();

            ShowNotice("Arrange cards according to the mission!", true);
        }

        /// <summary>
        /// Menyetel huruf variabel pada kartu kustom (DEC-51).
        /// Default cerdas otomatis mengikuti variabel soal aktif.
        /// </summary>
        public void SetCustomVariable(string letter)
        {
            if (string.IsNullOrWhiteSpace(letter)) return;
            CurrentCustomVariable = letter.Trim().Substring(0, 1).ToLower();
            if (customVariableText != null)
            {
                customVariableText.text = CurrentCustomVariable;
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

            if (moreDetailsButton != null)
            {
                moreDetailsButton.onClick.RemoveAllListeners();
                moreDetailsButton.onClick.AddListener(OnMoreDetailsClicked);
            }

            if (deleteOneButton != null)
            {
                deleteOneButton.onClick.RemoveAllListeners();
                deleteOneButton.onClick.AddListener(OnDeleteOneClicked);
            }

            if (clearAllButton != null)
            {
                clearAllButton.onClick.RemoveAllListeners();
                clearAllButton.onClick.AddListener(OnClearClicked);
            }

            if (clearButton != null && clearButton != clearAllButton)
            {
                clearButton.onClick.RemoveAllListeners();
                clearButton.onClick.AddListener(OnClearClicked);
            }

            if (resetStepButton != null)
            {
                resetStepButton.onClick.RemoveAllListeners();
                resetStepButton.onClick.AddListener(OnClearClicked);
            }

            // D-Pad Compass Navigation listeners
            if (dpadUp != null) dpadUp.onClick.AddListener(() => PanScrollView(new Vector2(0f, 120f)));
            if (dpadDown != null) dpadDown.onClick.AddListener(() => PanScrollView(new Vector2(0f, -120f)));
            if (dpadLeft != null) dpadLeft.onClick.AddListener(() => PanScrollView(new Vector2(-120f, 0f)));
            if (dpadRight != null) dpadRight.onClick.AddListener(() => PanScrollView(new Vector2(120f, 0f)));
            if (dpadCenter != null) dpadCenter.onClick.AddListener(OnDpadCenterClicked);

            // Pasang listener Tap-to-Slot untuk tombol kartu dek angka (0..9)
            for (int i = 0; i < numberButtons.Count; i++)
            {
                Button btn = numberButtons[i];
                if (btn != null)
                {
                    Text t = btn.GetComponentInChildren<Text>();
                    string val = (t != null && !string.IsNullOrEmpty(t.text)) ? t.text : i.ToString();
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(() => OnDeckCardClicked(val, "number"));
                }
            }

            // Pasang listener Tap-to-Slot untuk tombol variabel (x, y, a, b)
            foreach (var btn in variableButtons)
            {
                if (btn != null)
                {
                    Text t = btn.GetComponentInChildren<Text>();
                    string val = (t != null && !string.IsNullOrEmpty(t.text)) ? t.text : "x";
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(() => OnDeckCardClicked(val, "variable"));
                }
            }

            // Pasang listener untuk tombol variabel kustom / dinamis (DEC-51)
            if (customVariableButton != null)
            {
                customVariableButton.onClick.RemoveAllListeners();
                customVariableButton.onClick.AddListener(() =>
                {
                    string targetVar = !string.IsNullOrEmpty(CurrentCustomVariable) ? CurrentCustomVariable : "x";
                    OnDeckCardClicked(targetVar, "variable");
                });
            }

            // Pasang listener Tap-to-Slot untuk tombol operator (+, -, ×, ÷)
            foreach (var btn in operatorButtons)
            {
                if (btn != null)
                {
                    Text t = btn.GetComponentInChildren<Text>();
                    string val = (t != null && !string.IsNullOrEmpty(t.text)) ? t.text : "+";
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

            if (addEquationButton != null)
            {
                addEquationButton.onClick.RemoveAllListeners();
                addEquationButton.onClick.AddListener(() => OnEliminationOperationClicked("+"));
            }

            if (subtractEquationButton != null)
            {
                subtractEquationButton.onClick.RemoveAllListeners();
                subtractEquationButton.onClick.AddListener(() => OnEliminationOperationClicked("-"));
            }
        }

        /// <summary>
        /// Handler saat tombol operasi eliminasi dipilih (+ atau -) pada Stage 2 Mode Eliminasi Level Hard.
        /// Memvalidasi keselarasan operasi dengan ActiveQuestion.elimination_op.
        /// </summary>
        public void OnEliminationOperationClicked(string op)
        {
            if (ActiveHardMode != HardAlgebraMode.Elimination) return;

            string expectedOp = !string.IsNullOrEmpty(ActiveQuestion?.elimination_op) ? ActiveQuestion.elimination_op.Trim() : "+";

            if (op == expectedOp)
            {
                SelectedEliminationOp = op;
                SafePlayCorrect();

                if (stages != null && stages.Count > 1)
                {
                    stages[1].isCompleted = true;
                    if (stages[1].badgeStatusText != null) stages[1].badgeStatusText.text = "DONE ✓";
                    if (stages[1].badgeStatusImage != null && badgeDoneSprite != null)
                    {
                        stages[1].badgeStatusImage.sprite = badgeDoneSprite;
                    }
                    if (panelCompleteSprite != null && stages[1].rowBackground != null)
                    {
                        stages[1].rowBackground.sprite = panelCompleteSprite;
                    }

                    if (stages[1].slots != null && stages[1].slots.Count > 0 && stages[1].slots[0] != null)
                    {
                        stages[1].slots[0].SetToken(op, orangeTileSprite != null ? orangeTileSprite : whiteTileSprite, new Color(0.55f, 0.25f, 0.02f, 1f));
                    }
                }

                ShowNotice($"Operasi [{op}] Tepat! Variabel tereliminasi. Melangkah ke Tahap 3!", true);
                SetStageActive(2);
            }
            else
            {
                SafePlayWrong();
                string hint = expectedOp == "+" ? "menjumlahkan (+)" : "mengurangkan (-)";
                ShowNotice($"Operasi belum tepat. Cobalah {hint} kedua persamaan untuk mengeliminasi variabel!", false);
            }
        }

        /// <summary>
        /// Mengekstrak target token untuk 4 tahap aljabar dari data soal.
        /// Mengadaptasi equation_model, solution_steps, dan final_answer.
        /// Sesuai prototype code.html:
        /// - Stage 1 (Arrange the Equations): Persamaan awal dengan kartu statis + slot kosong target.
        /// - Stage 2 (Step 1): Operasi pertama (pengurangan/pemindahan konstanta).
        /// - Stage 3 (Step 2): Operasi kedua (penyederhanaan/pembagian).
        /// - Stage 4 (Card Arrangement Results): Hasil akhir variabel x = 5.
        /// </summary>
        public void FormulateStagesFromQuestion(QuestionModel question)
        {
            if (question == null) return;

            if (stages == null) stages = new List<BoardStageRow>();
            while (stages.Count < 4)
            {
                stages.Add(new BoardStageRow());
            }

            bool isHard = string.Equals(question.ui_level, "Hard", StringComparison.OrdinalIgnoreCase);
            string method = question.method != null ? question.method.ToLowerInvariant() : "";

            if (isHard && method == "elimination")
            {
                FormulateEliminationStages(question);
            }
            else if (isHard && method == "substitution")
            {
                FormulateSubstitutionStages(question);
            }
            else
            {
                FormulateStandardStages(question);
            }

            // Pastikan jumlah target tokens per baris tidak melebihi kapasitas fisik 8 slot (DEC-54)
            for (int s = 0; s < stages.Count; s++)
            {
                if (stages[s].targetTokens != null && stages[s].targetTokens.Count > 8)
                {
                    stages[s].targetTokens = stages[s].targetTokens.GetRange(0, 8);
                }
            }

            // Pastikan seluruh 8 slot selalu aktif dan terbuka di semua tahap (Pilar 1 Zero Code Approved Plan)
            for (int s = 0; s < stages.Count; s++)
            {
                var stage = stages[s];
                if (stage.slots == null) stage.slots = new List<CardSlotView>();
                for (int i = 0; i < stage.slots.Count; i++)
                {
                    if (stage.slots[i] != null)
                    {
                        if (stage.slots[i].slotButton != null)
                        {
                            stage.slots[i].slotButton.gameObject.SetActive(true);
                        }
                        stage.slots[i].Clear(emptySlotSprite);
                    }
                }
            }
        }

        private void FormulateEliminationStages(QuestionModel question)
        {
            // Stage 0: Initial Equations
            stages[0].stageName = "Initial Equations";
            if (stages[0].stageTitleText != null) stages[0].stageTitleText.text = "Card Board: Initial Equations";
            if (stages[0].stageIconText != null) stages[0].stageIconText.text = "📖";
            string eq0 = (question.equation_model != null && question.equation_model.Length > 0)
                ? question.equation_model[0]
                : "2x + y = 10";
            if (stages[0].stageInstructionText != null) stages[0].stageInstructionText.text = $"Susun persamaan utama: {eq0}";
            stages[0].targetTokens = TokenizeExpression(eq0);

            // Stage 1: Choose Operation (+ or -)
            stages[1].stageName = "Choose Operation";
            if (stages[1].stageTitleText != null) stages[1].stageTitleText.text = "Card Board: Choose Operation";
            if (stages[1].stageIconText != null) stages[1].stageIconText.text = "⚔️";
            string op = !string.IsNullOrEmpty(question.elimination_op) ? question.elimination_op : "+";
            if (stages[1].stageInstructionText != null) stages[1].stageInstructionText.text = "Pilih operasi [ + ] Tambah atau [ - ] Kurang untuk mengeliminasi variabel!";
            stages[1].targetTokens = new List<string> { op };

            // Stage 2: Result after Elimination
            stages[2].stageName = "Result after Elimination";
            if (stages[2].stageTitleText != null) stages[2].stageTitleText.text = "Card Board: Result after Elimination";
            if (stages[2].stageIconText != null) stages[2].stageIconText.text = "💡";
            string step2Eq = (question.solution_steps != null && question.solution_steps.Length > 2)
                ? ExtractPureEquation(question.solution_steps[2])
                : "3x = 12";
            if (stages[2].stageInstructionText != null) stages[2].stageInstructionText.text = "Sederhanakan persamaan gabungan setelah eliminasi.";
            stages[2].targetTokens = TokenizeExpression(step2Eq);

            // Stage 3: Solve for Variable
            stages[3].stageName = "Card Arrangement Results";
            if (stages[3].stageTitleText != null) stages[3].stageTitleText.text = "Card Board: Solved Variable";
            if (stages[3].stageIconText != null) stages[3].stageIconText.text = "⭐";
            string step3Eq = (question.solution_steps != null && question.solution_steps.Length > 3)
                ? ExtractPureEquation(question.solution_steps[3])
                : "x = 4";
            if (stages[3].stageInstructionText != null) stages[3].stageInstructionText.text = $"Hasil nilai variabel yang diselesaikan: {step3Eq}";
            stages[3].targetTokens = TokenizeExpression(step3Eq);
        }

        private void FormulateSubstitutionStages(QuestionModel question)
        {
            // Stage 0: Isolate a Variable
            stages[0].stageName = "Isolate a Variable";
            if (stages[0].stageTitleText != null) stages[0].stageTitleText.text = "Card Board: Isolate Variable";
            if (stages[0].stageIconText != null) stages[0].stageIconText.text = "📖";
            string step0Eq = (question.solution_steps != null && question.solution_steps.Length > 0)
                ? ExtractPureEquation(question.solution_steps[0])
                : "x = y + 2";
            if (stages[0].stageInstructionText != null) stages[0].stageInstructionText.text = $"Isolasi variabel: {step0Eq}";
            stages[0].targetTokens = TokenizeExpression(step0Eq);

            // Stage 1: Substitute into Other Equation
            stages[1].stageName = "Substitute Expression";
            if (stages[1].stageTitleText != null) stages[1].stageTitleText.text = "Card Board: Substitute Expression";
            if (stages[1].stageIconText != null) stages[1].stageIconText.text = "🔄";
            string step1Eq = (question.solution_steps != null && question.solution_steps.Length > 1)
                ? ExtractPureEquation(question.solution_steps[1])
                : "2y + 4 + y = 10";
            if (stages[1].stageInstructionText != null) stages[1].stageInstructionText.text = "Substitusikan ekspresi ke persamaan lain.";
            stages[1].targetTokens = TokenizeExpression(step1Eq);

            // Stage 2: Simplify the Expression
            stages[2].stageName = "Simplify Expression";
            if (stages[2].stageTitleText != null) stages[2].stageTitleText.text = "Card Board: Simplify Expression";
            if (stages[2].stageIconText != null) stages[2].stageIconText.text = "💡";
            string step2Eq = (question.solution_steps != null && question.solution_steps.Length > 2)
                ? ExtractPureEquation(question.solution_steps[2])
                : "3y = 6";
            if (stages[2].stageInstructionText != null) stages[2].stageInstructionText.text = "Sederhanakan persamaan untuk mencari variabel pertama.";
            stages[2].targetTokens = TokenizeExpression(step2Eq);

            // Stage 3: Final Solution
            stages[3].stageName = "Card Arrangement Results";
            if (stages[3].stageTitleText != null) stages[3].stageTitleText.text = "Card Board: Final Solution";
            if (stages[3].stageIconText != null) stages[3].stageIconText.text = "⭐";
            string step3Eq = (question.solution_steps != null && question.solution_steps.Length > 3)
                ? ExtractPureEquation(question.solution_steps[3])
                : "x = 4";
            if (stages[3].stageInstructionText != null) stages[3].stageInstructionText.text = $"Nilai variabel: {step3Eq}";
            stages[3].targetTokens = TokenizeExpression(step3Eq);
        }

        private void FormulateStandardStages(QuestionModel question)
        {
            string varName = ExtractVariableFromQuestion(question);

            // ==========================================
            // STAGE 0 (Stage 1): Card Board: Arrange the Equations
            // ==========================================
            stages[0].stageName = "Arrange the Equations";
            if (stages[0].stageTitleText != null) stages[0].stageTitleText.text = "Card Board: Arrange the Equations";
            if (stages[0].stageIconText != null) stages[0].stageIconText.text = "📖";
            if (stages[0].stageInstructionText != null) stages[0].stageInstructionText.text = "Arrange cards to form the initial equation.";
            
            string eq0 = (question.equation_model != null && question.equation_model.Length > 0)
                ? question.equation_model[0]
                : "3x + 5 = 20";
            stages[0].targetTokens = TokenizeExpression(eq0);

            // ==========================================
            // STAGE 1 (Stage 2): Card Board: Step 1
            // ==========================================
            stages[1].stageName = "Step 1";
            if (stages[1].stageTitleText != null) stages[1].stageTitleText.text = "Card Board: Step 1";
            if (stages[1].stageIconText != null) stages[1].stageIconText.text = "💡";

            string step1Desc = "Move the cards to subtract from both sides.";
            string step1Eq = "";

            if (question.solution_steps != null && question.solution_steps.Length > 2)
            {
                step1Eq = ExtractPureEquation(question.solution_steps[2]);
                if (question.solution_steps.Length > 1 && !string.IsNullOrWhiteSpace(question.solution_steps[1]))
                {
                    step1Desc = question.solution_steps[1];
                }
            }
            else
            {
                step1Eq = $"3{varName} = 20 - 5";
                step1Desc = "Move the cards to subtract 5 from both sides.";
            }
            if (stages[1].stageInstructionText != null) stages[1].stageInstructionText.text = step1Desc;
            stages[1].targetTokens = TokenizeExpression(step1Eq);

            // ==========================================
            // STAGE 2 (Stage 3): Card Board: Step 2
            // ==========================================
            stages[2].stageName = "Step 2";
            if (stages[2].stageTitleText != null) stages[2].stageTitleText.text = "Card Board: Step 2";
            if (stages[2].stageIconText != null) stages[2].stageIconText.text = "🔍";

            string step2Desc = "Simplify the expression to divide both sides.";
            string step2Eq = "";

            if (question.solution_steps != null && question.solution_steps.Length > 3)
            {
                step2Eq = ExtractPureEquation(question.solution_steps[3]);
                step2Desc = "Simplify the expression.";
            }
            else
            {
                step2Eq = $"3{varName} = 15 / 3";
                step2Desc = "Move the cards to divide both sides by 3.";
            }
            if (stages[2].stageInstructionText != null) stages[2].stageInstructionText.text = step2Desc;
            stages[2].targetTokens = TokenizeExpression(step2Eq);

            // ==========================================
            // STAGE 3 (Stage 4): Card Arrangement Results
            // ==========================================
            stages[3].stageName = "Card Arrangement Results";
            if (stages[3].stageTitleText != null) stages[3].stageTitleText.text = "Card Arrangement Results";
            if (stages[3].stageIconText != null) stages[3].stageIconText.text = "⭐";
            if (stages[3].stageInstructionText != null) stages[3].stageInstructionText.text = "The final result after the variables are resolved.";

            string finalAnswer = !string.IsNullOrEmpty(question.final_answer) ? question.final_answer : "5";
            string cleanDigits = System.Text.RegularExpressions.Regex.Replace(finalAnswer, @"[^\d]", "");
            if (string.IsNullOrEmpty(cleanDigits)) cleanDigits = finalAnswer.Trim();

            string finalExpr = $"{varName} = {cleanDigits}";
            stages[3].targetTokens = TokenizeExpression(finalExpr);

            // Pastikan jumlah target tokens per baris tidak melebihi kapasitas fisik 8 slot (DEC-54)
            for (int s = 0; s < stages.Count; s++)
            {
                if (stages[s].targetTokens != null && stages[s].targetTokens.Count > 8)
                {
                    stages[s].targetTokens = stages[s].targetTokens.GetRange(0, 8);
                }
            }

            // Pastikan seluruh 8 slot selalu aktif dan terbuka di semua tahap (Pilar 1 Zero Code Approved Plan)
            for (int s = 0; s < stages.Count; s++)
            {
                var stage = stages[s];
                if (stage.slots == null) stage.slots = new List<CardSlotView>();
                for (int i = 0; i < stage.slots.Count; i++)
                {
                    if (stage.slots[i] != null)
                    {
                        if (stage.slots[i].slotButton != null)
                        {
                            stage.slots[i].slotButton.gameObject.SetActive(true);
                        }
                        stage.slots[i].Clear(emptySlotSprite);
                    }
                }
            }
        }

        /// <summary>
        /// Mengekstrak persamaan matematika murni dari kalimat narasi langkah solusi.
        /// Mengambil bagian formula setelah pemisah naratif seperti ": ", ", giving ", atau "so ".
        /// </summary>
        public static string ExtractPureEquation(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "";
            string s = raw.Trim();
            string[] separators = new[] { ": ", ", giving ", " giving ", "so " };
            foreach (var sep in separators)
            {
                if (s.Contains(sep))
                {
                    string[] parts = s.Split(new[] { sep }, StringSplitOptions.RemoveEmptyEntries);
                    s = parts[parts.Length - 1].Trim();
                }
            }
            if (s.Contains(","))
            {
                s = s.Split(',')[0].Trim();
            }
            return s.Trim();
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

            int eqPos = expr.IndexOf('=');
            if (eqPos >= 0)
            {
                int start = 0;
                for (int i = eqPos - 1; i >= 0; i--)
                {
                    if (char.IsLetterOrDigit(expr[i]) || expr[i] == '+' || expr[i] == '-' || expr[i] == '*' || expr[i] == '/' || expr[i] == '×' || expr[i] == '÷' || expr[i] == '(' || expr[i] == ')' || expr[i] == ' ')
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

                if (c == '+' || c == '-' || c == '=' || c == '/' || c == '*' || c == '×' || c == '÷')
                {
                    string op = c.ToString();
                    if (c == '×') op = "*";
                    if (c == '÷') op = "/";
                    tokens.Add(op);
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

        public static string ExtractVariableFromQuestion(QuestionModel q)
        {
            if (q != null && q.equation_model != null && q.equation_model.Length > 0)
            {
                foreach (char c in q.equation_model[0])
                {
                    if (char.IsLetter(c)) return c.ToString().ToLower();
                }
            }
            return "x";
        }

        /// <summary>
        /// Menghasilkan petunjuk variabel konkret dan ramah anak sesuai variabel dan konteks soal.
        /// Memenuhi Catatan Revisi 1 Butir 5 & 6 (b = boneka, a = anggur, p = pensil, dll.).
        /// </summary>
        public static string GetVariableContextHint(QuestionModel question)
        {
            string varName = ExtractVariableFromQuestion(question).ToLower();
            string qText = (question != null && !string.IsNullOrEmpty(question.question_text))
                ? question.question_text.ToLower()
                : "";

            switch (varName)
            {
                case "b":
                    return "b = banyak boneka 🧸";
                case "a":
                    return "a = buah anggur 🍇";
                case "p":
                    return "p = batang pensil ✏️";
                case "y":
                    return "y = kartu misteri 🃏";
                case "x":
                    if (qText.Contains("saving") || qText.Contains("money") || qText.Contains("tabungan") || qText.Contains("uang") || qText.Contains("rp"))
                    {
                        return "x = jumlah uang tabungan 💰";
                    }
                    if (qText.Contains("age") || qText.Contains("umur") || qText.Contains("tahun"))
                    {
                        return "x = usia / umur 🎂";
                    }
                    if (qText.Contains("book") || qText.Contains("buku"))
                    {
                        return "x = jumlah buku 📚";
                    }
                    return "x = kotak misteri yang dicari 📦";
                default:
                    return $"{varName} = nilai misteri yang dicari 🔍";
            }
        }

        private QuestionModel CreateFallbackQuestion()
        {
            return new QuestionModel
            {
                id = "DEFAULT-04",
                ui_level = "Easy",
                title = "Card Board Mission",
                question_text = "If 3x + 5 = 20, find the value of x. Tap cards from the deck to fill the empty slot.",
                equation_model = new string[] { "3x + 5 = 20" },
                solution_steps = new string[] { "Let variable be x", "Subtract 5 from both sides", "3x = 20 - 5", "3x = 15 / 3" },
                final_answer = "5"
            };
        }

        /// <summary>
        /// Mengatur tahap aktif (0, 1, 2, atau 3).
        /// Memperbarui visual badge (DONE ✓, ACTIVE, LOCKED), status interaktivitas baris,
        /// panah penghubung, label tombol continue, serta memusatkan tampilan ke tahap aktif.
        /// </summary>
        public void SetStageActive(int stageIndex)
        {
            if (stages == null) stages = new List<BoardStageRow>();
            CurrentStageIndex = Mathf.Clamp(stageIndex, 0, stages.Count);
            IsAllStagesCompleted = (CurrentStageIndex >= stages.Count);

            for (int i = 0; i < stages.Count; i++)
            {
                var stage = stages[i];
                if (stage == null) continue;

                if (i < CurrentStageIndex)
                {
                    // Tahap Selesai (Completed)
                    stage.isCompleted = true;
                    stage.isUnlocked = true;
                    SetRowInteractable(stage, false, new Color(0.92f, 1f, 0.92f, 1f));

                    if (stage.badgeStatusText != null) stage.badgeStatusText.text = "DONE ✓";
                    if (stage.badgeStatusImage != null && badgeDoneSprite != null)
                    {
                        stage.badgeStatusImage.sprite = badgeDoneSprite;
                        stage.badgeStatusImage.color = Color.white;
                    }
                    if (stage.stepArrowDown != null) stage.stepArrowDown.SetActive(true);
                }
                else if (i == CurrentStageIndex && !IsAllStagesCompleted)
                {
                    // Tahap Aktif (Active)
                    stage.isCompleted = false;
                    stage.isUnlocked = true;
                    SetRowInteractable(stage, true, Color.white);

                    if (stage.badgeStatusText != null) stage.badgeStatusText.text = "ACTIVE";
                    if (stage.badgeStatusImage != null && badgeActiveSprite != null)
                    {
                        stage.badgeStatusImage.sprite = badgeActiveSprite;
                        stage.badgeStatusImage.color = Color.white;
                    }
                    if (stage.stepArrowDown != null) stage.stepArrowDown.SetActive(true);
                }
                else
                {
                    // Tahap Terkunci (Locked)
                    stage.isCompleted = false;
                    stage.isUnlocked = false;
                    SetRowInteractable(stage, false, new Color(0.65f, 0.65f, 0.65f, 0.65f));

                    if (stage.badgeStatusText != null) stage.badgeStatusText.text = "LOCKED";
                    if (stage.badgeStatusImage != null && badgeLockedSprite != null)
                    {
                        stage.badgeStatusImage.sprite = badgeLockedSprite;
                        stage.badgeStatusImage.color = new Color(1f, 1f, 1f, 0.8f);
                    }
                    if (stage.stepArrowDown != null) stage.stepArrowDown.SetActive(false);
                }
            }

            // Perbarui indikator tahap footer
            if (stageBadgeIndicatorText != null)
            {
                int displayStage = Mathf.Min(CurrentStageIndex + 1, stages.Count);
                stageBadgeIndicatorText.text = $"Stage {displayStage} of {stages.Count}";
            }

            if (eliminationOpPanel != null)
            {
                eliminationOpPanel.SetActive(ActiveHardMode == HardAlgebraMode.Elimination && CurrentStageIndex == 1 && !IsAllStagesCompleted);
            }

            UpdateActionButtonsVisual();
            AutoScrollToActiveStage(CurrentStageIndex);
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
        /// Mengisi slot kosong pertama pada tahap aktif sesuai jumlah target token.
        /// </summary>
        public void OnDeckCardClicked(string token, string cardType)
        {
            if (IsAllStagesCompleted || CurrentStageIndex >= stages.Count) return;

            // Jika sedang di Stage 1 Mode Eliminasi dan pemain mengklik kartu operator '+' atau '-',
            // arahkan langsung ke OnEliminationOperationClicked untuk responsivitas maksimal
            if (CurrentStageIndex == 1 && ActiveHardMode == HardAlgebraMode.Elimination && (token == "+" || token == "-"))
            {
                OnEliminationOperationClicked(token);
                return;
            }

            var currentStage = stages[CurrentStageIndex];
            int maxSlots = currentStage.slots.Count;

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
                SafePlayCardFlip();

                ShowNotice($"Placed card [{token}]!", true);
                CheckIfRowIsComplete(currentStage);
            }
            else
            {
                ShowNotice("Current slots are already filled. Press CLEAR or tap a card to edit.", false);
            }
        }

        /// <summary>
        /// Handler saat slot kartu pada papan diklik (Tap-to-Clear).
        /// Mengosongkan slot yang bersangkutan kembali ke status empty slot.
        /// </summary>
        public void OnSlotClicked(int stageIndex, int slotIndex)
        {
            if (stageIndex != CurrentStageIndex || IsAllStagesCompleted) return;

            var stage = stages[stageIndex];
            if (slotIndex >= 0 && slotIndex < stage.slots.Count)
            {
                var slot = stage.slots[slotIndex];
                if (!slot.IsEmpty)
                {
                    slot.Clear(emptySlotSprite);
                    SafePlayButtonClick();
                    ShowNotice("Card removed. Tap a card from the deck!", true);
                }
            }
        }

        /// <summary>
        /// Handler tombol Hapus 1 Kartu (⌫).
        /// Menghapus satu kartu terisi paling kanan (slot indeks tertinggi) pada tahap aktif.
        /// </summary>
        public void OnDeleteOneClicked()
        {
            if (IsAllStagesCompleted || CurrentStageIndex >= stages.Count) return;

            var currentStage = stages[CurrentStageIndex];
            if (currentStage == null || currentStage.slots == null) return;

            // Cari kartu terisi dari slot paling kanan (indeks tertinggi) ke kiri
            for (int i = currentStage.slots.Count - 1; i >= 0; i--)
            {
                var slot = currentStage.slots[i];
                if (slot != null && !slot.IsEmpty)
                {
                    string removedToken = slot.currentToken;
                    slot.Clear(emptySlotSprite);
                    SafePlayButtonClick();
                    ShowNotice($"Kartu [{removedToken}] dihapus.", true);
                    return;
                }
            }

            ShowNotice("Papan aljabar sudah kosong.", false);
        }

        /// <summary>
        /// Handler tombol 3D CLEAR / HAPUS SEMUA 🗑️.
        /// Mengosongkan seluruh slot pada tahap aktif secara instan.
        /// </summary>
        public void OnClearClicked()
        {
            if (IsAllStagesCompleted || CurrentStageIndex >= stages.Count) return;

            var currentStage = stages[CurrentStageIndex];
            if (currentStage != null && currentStage.slots != null)
            {
                foreach (var slot in currentStage.slots)
                {
                    if (slot != null)
                    {
                        slot.Clear(emptySlotSprite);
                    }
                }
            }

            SafePlayButtonClick();
            ShowNotice("Seluruh kartu pada tahap aktif dihapus.", true);
        }

        private void CheckIfRowIsComplete(BoardStageRow stage)
        {
            if (stage == null || stage.slots == null) return;
            int filledCount = 0;
            for (int i = 0; i < stage.slots.Count; i++)
            {
                if (!stage.slots[i].IsEmpty) filledCount++;
            }

            int totalNeeded = stage.targetTokens != null ? Mathf.Min(stage.targetTokens.Count, stage.slots.Count) : stage.slots.Count;

            if (filledCount >= totalNeeded && totalNeeded > 0)
            {
                ShowNotice("Equation ready! Tap 'Continue' to verify.", true);
            }
        }

        /// <summary>
        /// Handler tombol Continue > (Verifikasi jawaban langkah aktif, atau lanjut ke layar berikutnya).
        /// Pemain harus menyusun kartu dengan benar sesuai rumus di setiap tahap agar dapat melangkah maju ("harus pas").
        /// </summary>
        public void OnContinueClicked()
        {
            SafePlayButtonClick();

            if (IsAllStagesCompleted)
            {
                if (GameSession.Instance != null)
                {
                    GameSession.Instance.LastBoardSolutionCorrect = true;
                }
                SceneNavigator.LoadScene(SceneNavigator.SceneTrueOrFalse);
                return;
            }

            var currentStage = stages[CurrentStageIndex];

            // Proteksi Stage 1 Mode Eliminasi: wajib memilih operasi
            if (CurrentStageIndex == 1 && ActiveHardMode == HardAlgebraMode.Elimination)
            {
                if (string.IsNullOrEmpty(SelectedEliminationOp) && (currentStage.slots == null || currentStage.slots.Count == 0 || currentStage.slots[0].IsEmpty))
                {
                    SafePlayWrong();
                    ShowNotice("Pilih tombol operasi [ + ] atau [ - ] pada papan untuk mengeliminasi variabel!", false);
                    return;
                }
            }

            bool isCorrect = ValidateStage(currentStage);

            if (isCorrect)
            {
                SafePlayCorrect();

                currentStage.isCompleted = true;
                if (panelCompleteSprite != null && currentStage.rowBackground != null)
                {
                    currentStage.rowBackground.sprite = panelCompleteSprite;
                }

                int nextStage = CurrentStageIndex + 1;

                if (nextStage >= stages.Count)
                {
                    IsAllStagesCompleted = true;
                    SetStageActive(nextStage);
                    ShowNotice("🎉 Quest Complete! You solved the equation with Professor Bug!", true);
                }
                else
                {
                    ShowNotice($"Step completed! Moving to Stage {nextStage + 1}", true);
                    SetStageActive(nextStage);
                }
            }
            else
            {
                SafePlayWrong();
                ShowNotice("Cards don't match the equation. Tap a card to clear and try again!", false);
            }
        }

        /// <summary>
        /// Handler tombol More Details 🔄 (Review / Step Back).
        /// Mengizinkan pemain kembali ke tahap sebelumnya tanpa menghilangkan susunan kartu valid.
        /// </summary>
        public void OnMoreDetailsClicked()
        {
            SafePlayButtonClick();

            if (CurrentStageIndex > 0)
            {
                IsAllStagesCompleted = false;
                int prevStage = CurrentStageIndex - 1;
                SetStageActive(prevStage);
                ShowNotice($"Returned to Stage {prevStage + 1}", true);
            }
            else
            {
                ShowNotice("Already at the first stage!", false);
            }
        }

        /// <summary>
        /// Mengevaluasi kebenaran seluruh pengerjaan aljabar di papan.
        /// Memeriksa ketepatan hasil akhir di Stage 3 dan kesesuaian formula aljabar.
        /// Hasil disimpan di GameSession.Instance.LastBoardSolutionCorrect untuk ditampilkan di Layar 05 (True or False).
        /// </summary>
        public bool EvaluateOverallBoardSolution()
        {
            if (stages == null || stages.Count == 0) return true;

            // 1. Evaluasi Stage 3 (Card Arrangement Results)
            if (stages.Count > 3 && stages[3] != null)
            {
                var stage3 = stages[3];
                var tokens3 = GetFilledTokens(stage3);

                if (tokens3.Count == 0) return false;

                var target3 = stage3.targetTokens ?? new List<string>();
                string expectedAnswer = "";
                if (target3.Count > 0)
                {
                    expectedAnswer = NormalizeTokenStatic(target3[target3.Count - 1]);
                }

                bool matchesExact = TokensMatch(tokens3, target3);
                bool matchesLastDigit = false;
                for (int i = 0; i < tokens3.Count; i++)
                {
                    if (NormalizeTokenStatic(tokens3[i]) == expectedAnswer)
                    {
                        matchesLastDigit = true;
                        break;
                    }
                }

                if (!matchesExact && !matchesLastDigit)
                {
                    return false;
                }
            }

            // 2. Evaluasi Stage 0 (Arrange the Equations): jika diisi, tidak boleh bertentangan
            if (stages.Count > 0 && stages[0] != null)
            {
                var stage0 = stages[0];
                var tokens0 = GetFilledTokens(stage0);
                if (tokens0.Count > 0 && stage0.targetTokens != null && stage0.targetTokens.Count > 0)
                {
                    int compareCount = Mathf.Min(tokens0.Count, stage0.targetTokens.Count);
                    for (int i = 0; i < compareCount; i++)
                    {
                        if (!string.Equals(NormalizeTokenStatic(tokens0[i]), NormalizeTokenStatic(stage0.targetTokens[i]), StringComparison.OrdinalIgnoreCase))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public static List<string> GetFilledTokens(BoardStageRow stage)
        {
            var list = new List<string>();
            if (stage == null || stage.slots == null) return list;
            foreach (var slot in stage.slots)
            {
                if (slot != null && !string.IsNullOrWhiteSpace(slot.currentToken))
                {
                    list.Add(slot.currentToken.Trim());
                }
            }
            return list;
        }

        public static bool TokensMatch(List<string> a, List<string> b)
        {
            if (a == null || b == null) return false;
            if (a.Count != b.Count) return false;
            for (int i = 0; i < a.Count; i++)
            {
                string normA = NormalizeTokenStatic(a[i]);
                string normB = NormalizeTokenStatic(b[i]);
                if (!string.Equals(normA, normB, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            return true;
        }

        public static string NormalizeTokenStatic(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return "";
            string t = token.Trim().ToLower();
            if (t == "×") return "*";
            if (t == "÷") return "/";
            return t;
        }

        /// <summary>
        /// Memvalidasi susunan kartu di tahap tertentu dengan target tokens.
        /// Mendukung normalisasi simbol matematika (* dengan ×, / dengan ÷),
        /// pencocokan posisi langsung, serta fallback pencocokan string gabungan (concatenation)
        /// untuk mendukung input angka bertingkat/multi-digit.
        /// </summary>
        public bool ValidateStage(BoardStageRow stage)
        {
            if (stage == null || stage.targetTokens == null) return false;

            // Penanganan khusus Stage 1 Mode Eliminasi (pemilihan operasi + atau -)
            if (ActiveHardMode == HardAlgebraMode.Elimination && stages != null && stages.Count > 1 && stage == stages[1])
            {
                string expectedOp = !string.IsNullOrEmpty(ActiveQuestion?.elimination_op) ? ActiveQuestion.elimination_op.Trim() : "+";
                if (SelectedEliminationOp == expectedOp) return true;
                if (stage.slots != null && stage.slots.Count > 0 && stage.slots[0].currentToken == expectedOp) return true;
                return false;
            }

            int targetCount = Mathf.Min(stage.targetTokens.Count, stage.slots.Count);
            if (targetCount == 0) return true;

            // 1. Direct slot-by-slot matching
            bool directMatch = true;
            for (int i = 0; i < targetCount; i++)
            {
                if (i >= stage.slots.Count)
                {
                    directMatch = false;
                    break;
                }
                string playerToken = NormalizeToken(stage.slots[i].currentToken);
                string targetToken = NormalizeToken(stage.targetTokens[i]);

                if (!string.Equals(playerToken, targetToken, StringComparison.OrdinalIgnoreCase))
                {
                    directMatch = false;
                    break;
                }
            }

            // Pastikan jika direct match, slot sisanya tidak ada isi yang tidak sengaja terisi
            if (directMatch)
            {
                for (int i = targetCount; i < stage.slots.Count; i++)
                {
                    if (!stage.slots[i].IsEmpty)
                    {
                        directMatch = false;
                        break;
                    }
                }
            }

            if (directMatch) return true;

            // 2. Concatenated token matching (misal kartu digit terpisah atau pengelompokan token fleksibel)
            var playerTokens = new List<string>();
            for (int i = 0; i < stage.slots.Count; i++)
            {
                if (!stage.slots[i].IsEmpty)
                {
                    playerTokens.Add(NormalizeToken(stage.slots[i].currentToken));
                }
            }

            if (playerTokens.Count == 0 && stage.targetTokens.Count == 0) return true;

            string playerConcat = string.Concat(playerTokens).Replace(" ", "").ToLowerInvariant();
            var targetNormalized = new List<string>();
            foreach (var t in stage.targetTokens)
            {
                targetNormalized.Add(NormalizeToken(t));
            }
            string targetConcat = string.Concat(targetNormalized).Replace(" ", "").ToLowerInvariant();

            return !string.IsNullOrEmpty(playerConcat) && string.Equals(playerConcat, targetConcat, StringComparison.OrdinalIgnoreCase);
        }

        private string NormalizeToken(string token)
        {
            return NormalizeTokenStatic(token);
        }

        public void OnBackClicked()
        {
            SafePlayButtonClick();
            SceneNavigator.GoToMissionCard();
        }

        private void UpdateActionButtonsVisual()
        {
            if (continueLabelText != null)
            {
                continueLabelText.text = IsAllStagesCompleted ? "Complete Quest" : "Continue";
            }

            if (continueSubText != null)
            {
                continueSubText.text = IsAllStagesCompleted ? "Verify final equation" : "Verify answers";
            }

            if (moreDetailsButton != null)
            {
                moreDetailsButton.interactable = (CurrentStageIndex > 0);
            }
        }

        private void UpdateDeckInteractivity()
        {
            if (numberButtons != null) { foreach (var b in numberButtons) if (b != null) b.interactable = true; }
            if (variableButtons != null) { foreach (var b in variableButtons) if (b != null) b.interactable = true; }
            if (operatorButtons != null) { foreach (var b in operatorButtons) if (b != null) b.interactable = true; }
        }

        /// <summary>
        /// Menampilkan feedback notice bar melayang selama 2.5 detik sesuai code.html.
        /// </summary>
        public void ShowNotice(string message, bool isSuccess = true)
        {
            if (feedbackText != null)
            {
                feedbackText.text = message;
                feedbackText.color = isSuccess
                    ? new Color(0.04f, 0.35f, 0.15f, 1f) // Dark emerald
                    : new Color(0.45f, 0.20f, 0.02f, 1f); // Dark amber
            }

            if (feedbackBar != null)
            {
                Image bg = feedbackBar.GetComponent<Image>();
                if (bg != null)
                {
                    bg.color = isSuccess
                        ? new Color(0.2f, 0.85f, 0.55f, 0.95f)
                        : new Color(0.98f, 0.75f, 0.25f, 0.95f);
                }

                feedbackBar.SetActive(true);

                if (_noticeCoroutine != null) StopCoroutine(_noticeCoroutine);
                _noticeCoroutine = StartCoroutine(HideNoticeRoutine(2.5f));
            }
        }

        private IEnumerator HideNoticeRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (feedbackBar != null)
            {
                feedbackBar.SetActive(false);
            }
        }

        private void PanScrollView(Vector2 delta)
        {
            if (mainScrollRect != null && scrollContent != null)
            {
                Vector2 pos = scrollContent.anchoredPosition;
                pos += delta;
                scrollContent.anchoredPosition = pos;
                SafePlayButtonClick();
            }
        }

        private void OnDpadCenterClicked()
        {
            SafePlayButtonClick();
            AutoScrollToActiveStage(CurrentStageIndex);
            ShowNotice("Centered on active stage.", true);
        }

        private void AutoScrollToActiveStage(int stageIdx)
        {
            if (scrollContent != null)
            {
                // Tiap stage berjarak vertikal sekitar 260 px
                float targetY = Mathf.Clamp(stageIdx * 240f, 0f, 600f);
                StartCoroutine(SmoothScrollRoutine(targetY));
            }
        }

        private IEnumerator SmoothScrollRoutine(float targetY)
        {
            if (scrollContent == null) yield break;

            Vector2 startPos = scrollContent.anchoredPosition;
            Vector2 endPos = new Vector2(startPos.x, targetY);
            float elapsed = 0f;
            float duration = 0.25f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
                scrollContent.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
                yield return null;
            }

            scrollContent.anchoredPosition = endPos;
        }

        private void SafePlayButtonClick()
        {
            try { AudioManager.Instance?.PlayButtonClick(); } catch { }
        }

        private void SafePlayCardFlip()
        {
            try { AudioManager.Instance?.PlayCardFlip(); } catch { }
        }

        private void SafePlayCorrect()
        {
            try { AudioManager.Instance?.PlayCorrect(); } catch { }
        }

        private void SafePlayWrong()
        {
            try { AudioManager.Instance?.PlayWrong(); } catch { }
        }
    }
}
