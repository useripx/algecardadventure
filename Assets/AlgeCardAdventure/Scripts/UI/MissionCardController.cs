using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlgeCardAdventure.Session;
using AlgeCardAdventure.Data;
using AlgeCardAdventure.Audio;
using AlgeCardAdventure.Core;

namespace AlgeCardAdventure.UI
{
    /// <summary>
    /// Mengontrol interaksi pada Screen 03 — Mission Cards.
    /// Mengelola penarikan batch 5 kartu acak dari banksoal.json sesuai level aktif,
    /// animasi kocok kartu (riffle shuffle 3 detik), formasi kipas kartu (fanned arc),
    /// modal detail soal misi dengan tombol 'Continue >' dan '✕ Back to Cards',
    /// serta navigasi ke Screen 04.
    /// </summary>
    public class MissionCardController : MonoBehaviour
    {
        [Header("Navigasi & Header")]
        [SerializeField] private Button backButton;
        [SerializeField] private Text levelTitleText;
        [SerializeField] private Text levelSubtitleText;

        [Header("Deretan Kartu Misi (Kipas)")]
        [SerializeField] private Button[] cardButtons;
        [SerializeField] private GameObject[] cardObjects;
        [SerializeField] private RectTransform[] cardTransforms;

        [Header("Tumpukan Kartu Meja & Tombol Shuffle")]
        [SerializeField] private Button bottomDeckButton;
        [SerializeField] private Text shuffleButtonText;
        [SerializeField] private RectTransform bottomDeckTransform;
        [SerializeField] private GameObject shuffleStatusPill;
        [SerializeField] private Text shuffleStatusText;

        [Header("Modal Detail Misi")]
        [SerializeField] private GameObject missionDetailModal;
        [SerializeField] private RectTransform modalCardTransform;
        [SerializeField] private Text missionNumberText;
        [SerializeField] private Text missionTitleText;
        [SerializeField] private Text missionInstructionText;
        [SerializeField] private Text equationPreviewText;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button backToCardsButton;

        [Header("Bintang & Poin Kesulitan")]
        [SerializeField] private Image star1Image;
        [SerializeField] private Image star2Image;
        [SerializeField] private Image star3Image;
        [SerializeField] private Sprite starGoldSprite;
        [SerializeField] private Sprite starGreySprite;
        [SerializeField] private Text missionPointsText;

        [Header("Elemen Efek Visual Tambahan")]
        [SerializeField] private Button deckPileButton;
        [SerializeField] private RectTransform shuffleIconTransform;
        [SerializeField] private GameObject shuffleSparkles;
        [SerializeField] private RectTransform riffleCardLeft;
        [SerializeField] private RectTransform riffleCardRight;
        [SerializeField] private GameObject stationaryTopCard;

        [Header("Modal Tambahan: Materi Pembelajaran & Petunjuk Permainan (DEC-44)")]
        [SerializeField] private Button booksButton;
        [SerializeField] private Button compassButton;
        [SerializeField] private GameObject learningMaterialModal;
        [SerializeField] private GameObject gameplayGuideModal;
        [SerializeField] private Button closeMaterialButton;
        [SerializeField] private Button closeGuideButton;

        private List<QuestionModel> _currentBatch = new List<QuestionModel>();
        private QuestionModel _selectedQuestion;
        private QuestionRepository _repository;
        private bool _isShuffling = false;
        private bool _hasDealt = false;
        private Coroutine _shuffleCoroutine;

        // Formasi melengkung (fanned arc) untuk 5 kartu di canvas 1080x1920 (proporsional 215x365 px)
        private readonly Vector2[] _fannedPositions = new Vector2[]
        {
            new Vector2(-360f, -42f),
            new Vector2(-180f, -9f),
            new Vector2(0f, 14f),
            new Vector2(180f, -9f),
            new Vector2(360f, -42f)
        };

        private readonly float[] _fannedRotations = new float[]
        {
            14f,
            7f,
            0f,
            -7f,
            -14f
        };

        public IReadOnlyList<QuestionModel> CurrentBatch => _currentBatch;
        public QuestionModel SelectedQuestion => _selectedQuestion;
        public bool IsShuffling => _isShuffling;
        public bool HasDealt => _hasDealt;
        public Image Star1Image => star1Image;
        public Image Star2Image => star2Image;
        public Image Star3Image => star3Image;
        public Sprite StarGoldSprite => starGoldSprite;
        public Sprite StarGreySprite => starGreySprite;
        public Text MissionPointsText => missionPointsText;
        public GameObject LearningMaterialModal => learningMaterialModal;
        public GameObject GameplayGuideModal => gameplayGuideModal;
        public Button BooksButton => booksButton;
        public Button CompassButton => compassButton;
        public Button CloseMaterialButton => closeMaterialButton;
        public Button CloseGuideButton => closeGuideButton;

        private void Start()
        {
            // Pastikan BGM petualangan default berputar
            SafePlayDefaultBGM();

            // Pasang listener navigasi
            if (backButton != null)
            {
                backButton.onClick.AddListener(OnBackClicked);
            }

            if (continueButton != null)
            {
                continueButton.onClick.AddListener(OnContinueClicked);
            }

            if (backToCardsButton != null)
            {
                backToCardsButton.onClick.AddListener(OnBackToCardsClicked);
            }

            if (bottomDeckButton != null)
            {
                bottomDeckButton.onClick.AddListener(OnShuffleClicked);
            }

            if (deckPileButton != null)
            {
                deckPileButton.onClick.AddListener(OnShuffleClicked);
            }

            // Pasang listener modal materi pembelajaran & petunjuk permainan (DEC-44)
            if (booksButton != null)
            {
                booksButton.onClick.AddListener(OnBooksClicked);
            }

            if (compassButton != null)
            {
                compassButton.onClick.AddListener(OnCompassClicked);
            }

            if (closeMaterialButton != null)
            {
                closeMaterialButton.onClick.AddListener(OnCloseMaterialClicked);
            }

            if (closeGuideButton != null)
            {
                closeGuideButton.onClick.AddListener(OnCloseGuideClicked);
            }

            // Sembunyikan modal & efek sparkle di awal
            if (missionDetailModal != null)
            {
                missionDetailModal.SetActive(false);
            }

            if (learningMaterialModal != null)
            {
                learningMaterialModal.SetActive(false);
            }

            if (gameplayGuideModal != null)
            {
                gameplayGuideModal.SetActive(false);
            }

            if (shuffleStatusPill != null)
            {
                shuffleStatusPill.SetActive(false);
            }

            if (shuffleSparkles != null)
            {
                shuffleSparkles.SetActive(false);
            }

            if (riffleCardLeft != null)
            {
                riffleCardLeft.gameObject.SetActive(false);
            }

            if (riffleCardRight != null)
            {
                riffleCardRight.gameObject.SetActive(false);
            }

            if (shuffleButtonText != null)
            {
                shuffleButtonText.text = "SHUFFLE HERE";
            }

            // Atur posisi awal kartu tersembunyi / bertumpuk di meja
            ApplyInitialCardLayout();

            // Muat data soal misi dari repository
            LoadMissionDeck();

            // Deal otomatis kartu pertama kali agar pemain langsung dapat memilih
            DealCardsImmediately();
        }

        /// <summary>
        /// Menginisialisasi dan menarik batch soal acak dari bank soal sesuai level aktif.
        /// </summary>
        public void LoadMissionDeck()
        {
            string currentLevel = HasActiveSession ? GameSession.Instance.CurrentLevel : "Easy";

            if (levelTitleText != null)
            {
                levelTitleText.text = "Mission Card";
            }

            if (levelSubtitleText != null)
            {
                levelSubtitleText.text = $"{currentLevel} - Variables & Algebra";
            }

            if (_repository == null)
            {
                _repository = new QuestionRepository();
                _repository.LoadFromStreamingAssets("banksoal.json");
            }

            List<string> completedIds = HasActiveSession ? GameSession.Instance.CompletedQuestionIds : null;
            _currentBatch = _repository.GetRandomMissionBatch(currentLevel, count: 5, excludeIds: completedIds);

            SafeLog($"[MissionCardController] Memuat {_currentBatch.Count} kartu misi untuk level {currentLevel}.");

            int cardCount = cardButtons != null ? cardButtons.Length : 0;
            for (int i = 0; i < cardCount; i++)
            {
                int cardIndex = i;
                if (cardIndex < _currentBatch.Count)
                {
                    if (cardButtons[cardIndex] != null)
                    {
                        cardButtons[cardIndex].onClick.RemoveAllListeners();
                        cardButtons[cardIndex].onClick.AddListener(() => OnCardSelected(cardIndex));
                    }
                }
            }
        }

        /// <summary>
        /// Mengatur posisi awal 5 kartu agar tersusun melengkung (kipas).
        /// </summary>
        private void ApplyInitialCardLayout()
        {
            if (cardTransforms == null) return;

            for (int i = 0; i < cardTransforms.Length; i++)
            {
                if (cardTransforms[i] != null && i < _fannedPositions.Length)
                {
                    cardTransforms[i].anchoredPosition = _fannedPositions[i];
                    cardTransforms[i].localEulerAngles = new Vector3(0f, 0f, _fannedRotations[i]);
                    cardTransforms[i].localScale = Vector3.one;
                }
            }
        }

        /// <summary>
        /// Menampilkan kartu secara langsung ke formasi kipas tanpa animasi.
        /// </summary>
        public void DealCardsImmediately()
        {
            int cardCount = cardObjects != null ? cardObjects.Length : 0;
            for (int i = 0; i < cardCount; i++)
            {
                if (cardObjects[i] != null)
                {
                    cardObjects[i].SetActive(i < _currentBatch.Count);
                }
            }

            ApplyInitialCardLayout();
            _hasDealt = true;

            if (shuffleButtonText != null)
            {
                shuffleButtonText.text = "SHUFFLE AGAIN";
            }
        }

        /// <summary>
        /// Dipanggil saat tombol SHUFFLE di meja bawah ditekan.
        /// </summary>
        public void OnShuffleClicked()
        {
            if (_isShuffling) return;

            SafeLog("[MissionCardController] OnShuffleClicked terpicu — memulai urutan kocok kartu...");

            if (_shuffleCoroutine != null)
            {
                StopCoroutine(_shuffleCoroutine);
            }

            _shuffleCoroutine = StartCoroutine(ShuffleSequenceRoutine());
        }

        /// <summary>
        /// Coroutine urutan kocok kartu 4-fase sesuai spesifikasi prototipe code.html:
        /// 1. Retract: Kartu meluncur turun ke dek meja.
        /// 2. Riffle & Sparkles (3s): Tumpukan meja bergetar, ikon putar berputar, kilau emas, hitung mundur status pill.
        /// 3. Dealing Upward: Kartu baru meluncur naik satu per satu secara berurutan (staggered delay).
        /// 4. Settle & Idle: Tombol aktif kembali sebagai 'SHUFFLE AGAIN'.
        /// </summary>
        private IEnumerator ShuffleSequenceRoutine()
        {
            _isShuffling = true;

            // Kunci interaktivitas tombol selama proses kocok
            if (bottomDeckButton != null) bottomDeckButton.interactable = false;
            if (deckPileButton != null) deckPileButton.interactable = false;
            if (shuffleButtonText != null) shuffleButtonText.text = "SHUFFLING...";

            Vector2 tableDeckOrigin = bottomDeckTransform != null ? bottomDeckTransform.anchoredPosition : new Vector2(0f, -600f);

            // ==========================================
            // FASE 1: RETRACT ANIMATION (0.35s)
            // Kartu ditarik turun meluncur ke dek meja
            // ==========================================
            if (_hasDealt && cardTransforms != null && cardObjects != null)
            {
                float retractDuration = 0.35f;
                float retractElapsed = 0f;

                Vector2[] startPositions = new Vector2[cardTransforms.Length];
                for (int i = 0; i < cardTransforms.Length; i++)
                {
                    if (cardTransforms[i] != null) startPositions[i] = cardTransforms[i].anchoredPosition;
                }

                SafePlayCardFlip();

                while (retractElapsed < retractDuration)
                {
                    retractElapsed += Time.deltaTime;
                    float t = Mathf.SmoothStep(0f, 1f, retractElapsed / retractDuration);

                    for (int i = 0; i < cardTransforms.Length; i++)
                    {
                        if (cardTransforms[i] != null && cardObjects[i] != null && cardObjects[i].activeSelf)
                        {
                            cardTransforms[i].anchoredPosition = Vector2.Lerp(startPositions[i], new Vector2(0f, -600f), t);
                            cardTransforms[i].localScale = Vector3.Lerp(Vector3.one, new Vector3(0.4f, 0.4f, 1f), t);
                        }
                    }

                    yield return null;
                }

                foreach (var card in cardObjects)
                {
                    if (card != null) card.SetActive(false);
                }
            }

            // ==========================================
            // FASE 2: RIFFLE, ROTATING ICON & SPARKLES (3.0s)
            // ==========================================
            if (shuffleStatusPill != null) shuffleStatusPill.SetActive(true);
            if (shuffleStatusText != null) shuffleStatusText.text = "Shuffling deck (3s)...";
            if (shuffleSparkles != null) shuffleSparkles.SetActive(true);

            // Sembunyikan kartu diam tengah, aktifkan dua belahan sayap kartu kocok
            if (stationaryTopCard != null) stationaryTopCard.SetActive(false);
            if (riffleCardLeft != null) riffleCardLeft.gameObject.SetActive(true);
            if (riffleCardRight != null) riffleCardRight.gameObject.SetActive(true);

            SafePlayCardFlip();

            Vector2 deckOriginalPos = bottomDeckTransform != null ? bottomDeckTransform.anchoredPosition : Vector2.zero;
            float shuffleElapsed = 0f;
            float nextSfxTime = 0.55f;

            while (shuffleElapsed < 3.0f)
            {
                shuffleElapsed += Time.deltaTime;

                // Putar ikon panah shuffle
                if (shuffleIconTransform != null)
                {
                    shuffleIconTransform.Rotate(0f, 0f, -540f * Time.deltaTime);
                }

                // Envelope kurva pembelahan: membelah halus di awal (0-0.4s) dan menyatu merapat di akhir (2.6-3.0s)
                float envelope = 1f;
                if (shuffleElapsed < 0.4f)
                {
                    envelope = Mathf.SmoothStep(0f, 1f, shuffleElapsed / 0.4f);
                }
                else if (shuffleElapsed > 2.6f)
                {
                    envelope = Mathf.SmoothStep(1f, 0f, (shuffleElapsed - 2.6f) / 0.4f);
                }

                // Kecepatan gelombang osilasi kocokan (ritme harmonis natural ala dealer kartu remi)
                float waveSpeed = 16f;
                float waveLeft = Mathf.Sin(shuffleElapsed * waveSpeed);
                float waveRight = Mathf.Sin(shuffleElapsed * waveSpeed + Mathf.PI * 0.5f); // beda fase 90 derajat

                if (riffleCardLeft != null)
                {
                    float posX = -55f * envelope + waveLeft * 14f * envelope;
                    float posY = 40f + Mathf.Abs(waveLeft) * 18f * envelope;
                    float rotZ = 16f * envelope + waveLeft * 5f * envelope;
                    riffleCardLeft.anchoredPosition = new Vector2(posX, posY);
                    riffleCardLeft.localEulerAngles = new Vector3(0f, 0f, rotZ);
                    riffleCardLeft.localScale = Vector3.one * (1f + 0.05f * Mathf.Abs(waveLeft));
                }

                if (riffleCardRight != null)
                {
                    float posX = 55f * envelope + waveRight * 14f * envelope;
                    float posY = 40f + Mathf.Abs(waveRight) * 18f * envelope;
                    float rotZ = -16f * envelope - waveRight * 5f * envelope;
                    riffleCardRight.anchoredPosition = new Vector2(posX, posY);
                    riffleCardRight.localEulerAngles = new Vector3(0f, 0f, rotZ);
                    riffleCardRight.localScale = Vector3.one * (1f + 0.05f * Mathf.Abs(waveRight));
                }

                // Ayunan berirama lembut pada meja tumpukan
                if (bottomDeckTransform != null)
                {
                    float deckSwayX = Mathf.Sin(shuffleElapsed * 8f) * 4f * envelope;
                    float deckSwayRot = Mathf.Cos(shuffleElapsed * 8f) * 2f * envelope;
                    bottomDeckTransform.anchoredPosition = deckOriginalPos + new Vector2(deckSwayX, 0f);
                    bottomDeckTransform.localEulerAngles = new Vector3(0f, 0f, deckSwayRot);
                }

                // Update hitung mundur status pill
                if (shuffleElapsed >= 2.0f && shuffleStatusText != null)
                {
                    shuffleStatusText.text = "Shuffling deck (1s)...";
                }
                else if (shuffleElapsed >= 1.0f && shuffleStatusText != null)
                {
                    shuffleStatusText.text = "Shuffling deck (2s)...";
                }

                // Suara desir kartu berulang selama kocok
                if (shuffleElapsed >= nextSfxTime)
                {
                    SafePlayCardFlip();
                    nextSfxTime += 0.55f;
                }

                yield return null;
            }

            // Kembalikan posisi awal dan nonaktifkan sayap kartu kocok
            if (riffleCardLeft != null) riffleCardLeft.gameObject.SetActive(false);
            if (riffleCardRight != null) riffleCardRight.gameObject.SetActive(false);
            if (stationaryTopCard != null) stationaryTopCard.SetActive(true);

            if (bottomDeckTransform != null)
            {
                bottomDeckTransform.anchoredPosition = deckOriginalPos;
                bottomDeckTransform.localEulerAngles = Vector3.zero;
            }
            if (shuffleIconTransform != null)
            {
                shuffleIconTransform.localEulerAngles = Vector3.zero;
            }
            if (shuffleSparkles != null)
            {
                shuffleSparkles.SetActive(false);
            }

            // ==========================================
            // FASE 3: DEALING CARDS UPWARD (STAGGERED)
            // ==========================================
            if (shuffleStatusText != null)
            {
                shuffleStatusText.text = "Dealing 5 mission cards!";
            }

            // Muat batch soal baru secara acak dari repository
            LoadMissionDeck();

            // Animasikan pembagian kartu naik satu per satu
            int totalCards = cardObjects != null ? Mathf.Min(cardObjects.Length, _currentBatch.Count) : 0;
            float dealDuration = 0.35f;

            for (int i = 0; i < totalCards; i++)
            {
                int cardIndex = i;
                if (cardObjects[cardIndex] != null && cardTransforms[cardIndex] != null)
                {
                    cardObjects[cardIndex].SetActive(true);
                    cardTransforms[cardIndex].anchoredPosition = new Vector2(0f, -600f);
                    cardTransforms[cardIndex].localScale = new Vector3(0.4f, 0.4f, 1f);
                    cardTransforms[cardIndex].localEulerAngles = Vector3.zero;

                    StartCoroutine(AnimateSingleCardDeal(cardTransforms[cardIndex], _fannedPositions[cardIndex], _fannedRotations[cardIndex], dealDuration));
                    SafePlayCardFlip();
                }

                yield return new WaitForSeconds(0.08f);
            }

            // Tunggu hingga kartu terakhir selesai mencapai formasi kipas
            yield return new WaitForSeconds(dealDuration + 0.1f);
            _hasDealt = true;

            // ==========================================
            // FASE 4: SETTLE & IDLE STATE
            // ==========================================
            yield return new WaitForSeconds(0.7f);

            if (shuffleStatusPill != null) shuffleStatusPill.SetActive(false);
            if (bottomDeckButton != null) bottomDeckButton.interactable = true;
            if (deckPileButton != null) deckPileButton.interactable = true;
            if (shuffleButtonText != null) shuffleButtonText.text = "SHUFFLE AGAIN";

            _isShuffling = false;
            _shuffleCoroutine = null;
        }

        /// <summary>
        /// Coroutine pembantu untuk menganimasikan kartu tunggal meluncur naik dari dek ke posisi kipas.
        /// </summary>
        private IEnumerator AnimateSingleCardDeal(RectTransform cardTr, Vector2 targetPos, float targetRotZ, float duration)
        {
            if (cardTr == null) yield break;

            Vector2 startPos = cardTr.anchoredPosition;
            Vector3 startScale = cardTr.localScale;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);

                cardTr.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
                cardTr.localScale = Vector3.Lerp(startScale, Vector3.one, t);
                cardTr.localEulerAngles = new Vector3(0f, 0f, Mathf.Lerp(0f, targetRotZ, t));

                yield return null;
            }

            cardTr.anchoredPosition = targetPos;
            cardTr.localScale = Vector3.one;
            cardTr.localEulerAngles = new Vector3(0f, 0f, targetRotZ);
        }

        /// <summary>
        /// Dipanggil saat pemain mengetuk salah satu kartu misteri tertutup.
        /// </summary>
        public void OnCardSelected(int index)
        {
            if (_isShuffling) return;

            if (_currentBatch == null || index < 0 || index >= _currentBatch.Count)
            {
                SafeLogWarning($"[MissionCardController] Indeks kartu {index} tidak valid.");
                return;
            }

            _selectedQuestion = _currentBatch[index];
            SafeLog($"[MissionCardController] Kartu #{index + 1} dipilih: {_selectedQuestion.id} - {_selectedQuestion.title}");

            // Putar suara desir buka kartu
            SafePlayCardFlip();

            // Isi data ke modal kartu misi
            int currentNumber = (HasActiveSession ? GameSession.Instance.TotalAnswered : 0) + 1;

            if (missionNumberText != null)
            {
                missionNumberText.text = $"Mission {index + 1}";
            }

            if (missionTitleText != null)
            {
                missionTitleText.text = _selectedQuestion.title ?? "Variables & Simplification";
            }

            if (missionInstructionText != null)
            {
                try
                {
                    missionInstructionText.resizeTextForBestFit = true;
                    missionInstructionText.resizeTextMinSize = 13;
                    missionInstructionText.resizeTextMaxSize = 22;
                }
                catch { }

                missionInstructionText.text = _selectedQuestion.question_text ?? "Simplify the following algebraic expressions.";
            }

            if (equationPreviewText != null)
            {
                if (_selectedQuestion.equation_model != null && _selectedQuestion.equation_model.Length > 0)
                {
                    equationPreviewText.text = _selectedQuestion.equation_model[0];
                }
                else
                {
                    equationPreviewText.text = string.Empty;
                }
            }

            // Atur bintang kesulitan & poin misi dinamis sesuai ui_level soal (Easy=1, Normal/Medium=2, Hard=3)
            int activeStars = 1;
            int points = 100;
            string level = _selectedQuestion.ui_level ?? (HasActiveSession ? GameSession.Instance.CurrentLevel : "Easy");
            if (string.Equals(level, "Normal", System.StringComparison.OrdinalIgnoreCase) ||
                string.Equals(level, "Medium", System.StringComparison.OrdinalIgnoreCase))
            {
                activeStars = 2;
                points = 200;
            }
            else if (string.Equals(level, "Hard", System.StringComparison.OrdinalIgnoreCase))
            {
                activeStars = 3;
                points = 300;
            }

            if (star1Image != null) star1Image.sprite = activeStars >= 1 ? starGoldSprite : starGreySprite;
            if (star2Image != null) star2Image.sprite = activeStars >= 2 ? starGoldSprite : starGreySprite;
            if (star3Image != null) star3Image.sprite = activeStars >= 3 ? starGoldSprite : starGreySprite;
            if (missionPointsText != null) missionPointsText.text = points.ToString();

            if (missionDetailModal != null)
            {
                missionDetailModal.SetActive(true);
                StartCoroutine(AnimateCardFlipModalRoutine());
            }
        }

        /// <summary>
        /// Menganimasikan efek buka balik kartu (card flip) pada sumbu Y dari 90 derajat ke 0 derajat.
        /// </summary>
        private IEnumerator AnimateCardFlipModalRoutine()
        {
            if (modalCardTransform == null) yield break;

            modalCardTransform.localEulerAngles = new Vector3(0f, 90f, 0f);
            modalCardTransform.localScale = new Vector3(0.85f, 0.85f, 1f);

            float duration = 0.28f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);

                float rotY = Mathf.Lerp(90f, 0f, t);
                float scale = Mathf.Lerp(0.85f, 1f, t);

                modalCardTransform.localEulerAngles = new Vector3(0f, rotY, 0f);
                modalCardTransform.localScale = new Vector3(scale, scale, 1f);

                yield return null;
            }

            modalCardTransform.localEulerAngles = Vector3.zero;
            modalCardTransform.localScale = Vector3.one;
        }

        /// <summary>
        /// Menutup modal detail misi dan kembali ke deretan kartu kipas.
        /// </summary>
        public void OnBackToCardsClicked()
        {
            SafeLog("[MissionCardController] Menutup modal detail misi, kembali ke kartu.");
            SafePlayButtonClick();

            if (missionDetailModal != null)
            {
                missionDetailModal.SetActive(false);
            }
        }

        /// <summary>
        /// Melanjutkan ke Screen 04 (Algebra Card Board) dengan soal yang terpilih.
        /// </summary>
        public void OnContinueClicked()
        {
            if (_selectedQuestion == null)
            {
                SafeLogWarning("[MissionCardController] Belum ada soal yang dipilih.");
                return;
            }

            SafeLog($"[MissionCardController] Melanjutkan ke papan aljabar dengan soal {_selectedQuestion.id}");
            SafePlayButtonClick();

            // Simpan soal aktif ke sesi permainan dan tandai ID selesai (DEC-10)
            if (HasActiveSession)
            {
                GameSession.Instance.SetCurrentQuestion(_selectedQuestion);
                GameSession.Instance.MarkQuestionCompleted(_selectedQuestion.id);
            }

            // Navigasi ke Screen 04
            SceneNavigator.GoToAlgebraCardBoard();
        }

        /// <summary>
        /// Kembali ke Screen 02 (Level Selection).
        /// </summary>
        public void OnBackClicked()
        {
            SafeLog("[MissionCardController] Kembali ke Level Selection.");
            SafePlayButtonClick();
            SceneNavigator.GoToLevelSelection();
        }

        /// <summary>
        /// Dipanggil saat pemain mengetuk tumpukan buku di bawah kiri (Revisi 2 / DEC-44).
        /// Membuka modal materi pembelajaran aljabar.
        /// </summary>
        public void OnBooksClicked()
        {
            SafeLog("[MissionCardController] Membuka modal materi pembelajaran aljabar.");
            SafePlayButtonClick();

            if (gameplayGuideModal != null) gameplayGuideModal.SetActive(false);
            if (missionDetailModal != null) missionDetailModal.SetActive(false);

            if (learningMaterialModal != null)
            {
                learningMaterialModal.SetActive(true);
            }
        }

        /// <summary>
        /// Menutup modal materi pembelajaran aljabar.
        /// </summary>
        public void OnCloseMaterialClicked()
        {
            SafeLog("[MissionCardController] Menutup modal materi pembelajaran aljabar.");
            SafePlayButtonClick();

            if (learningMaterialModal != null)
            {
                learningMaterialModal.SetActive(false);
            }
        }

        /// <summary>
        /// Dipanggil saat pemain mengetuk kompas emas di bawah kanan (Revisi 3 / DEC-44).
        /// Membuka modal petunjuk permainan AlgeCard.
        /// </summary>
        public void OnCompassClicked()
        {
            SafeLog("[MissionCardController] Membuka modal petunjuk permainan AlgeCard.");
            SafePlayButtonClick();

            if (learningMaterialModal != null) learningMaterialModal.SetActive(false);
            if (missionDetailModal != null) missionDetailModal.SetActive(false);

            if (gameplayGuideModal != null)
            {
                gameplayGuideModal.SetActive(true);
            }
        }

        /// <summary>
        /// Menutup modal petunjuk permainan AlgeCard.
        /// </summary>
        public void OnCloseGuideClicked()
        {
            SafeLog("[MissionCardController] Menutup modal petunjuk permainan.");
            SafePlayButtonClick();

            if (gameplayGuideModal != null)
            {
                gameplayGuideModal.SetActive(false);
            }
        }

        private void SafeLog(string message)
        {
            try { Debug.Log(message); } catch { }
        }

        private void SafeLogWarning(string message)
        {
            try { Debug.LogWarning(message); } catch { }
        }

        private void SafePlayButtonClick()
        {
            try { AudioManager.Instance?.PlayButtonClick(); } catch { }
        }

        private void SafePlayCardFlip()
        {
            try { AudioManager.Instance?.PlayCardFlip(); } catch { }
        }

        private void SafePlayDefaultBGM()
        {
            try { AudioManager.Instance?.PlayDefaultBGM(); } catch { }
        }

        private static bool HasActiveSession => !ReferenceEquals(GameSession.Instance, null);
    }
}
