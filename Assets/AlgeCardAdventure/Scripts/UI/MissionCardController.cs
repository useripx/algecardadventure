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
        [SerializeField] private Text missionNumberText;
        [SerializeField] private Text missionTitleText;
        [SerializeField] private Text missionInstructionText;
        [SerializeField] private Text equationPreviewText;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button backToCardsButton;

        private List<QuestionModel> _currentBatch = new List<QuestionModel>();
        private QuestionModel _selectedQuestion;
        private QuestionRepository _repository;
        private bool _isShuffling = false;
        private bool _hasDealt = false;
        private Coroutine _shuffleCoroutine;

        // Formasi melengkung (fanned arc) untuk 5 kartu di canvas 1080x1920
        private readonly Vector2[] _fannedPositions = new Vector2[]
        {
            new Vector2(-280f, -35f),
            new Vector2(-140f, -5f),
            new Vector2(0f, 10f),
            new Vector2(140f, -5f),
            new Vector2(280f, -35f)
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

        private void Start()
        {
            // Pastikan BGM petualangan default berputar
            AudioManager.Instance?.PlayDefaultBGM();

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

            // Sembunyikan modal di awal
            if (missionDetailModal != null)
            {
                missionDetailModal.SetActive(false);
            }

            if (shuffleStatusPill != null)
            {
                shuffleStatusPill.SetActive(false);
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
            string currentLevel = GameSession.Instance != null ? GameSession.Instance.CurrentLevel : "Easy";

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

            List<string> completedIds = GameSession.Instance != null ? GameSession.Instance.CompletedQuestionIds : null;
            _currentBatch = _repository.GetRandomMissionBatch(currentLevel, count: 5, excludeIds: completedIds);

            Debug.Log($"[MissionCardController] Memuat {_currentBatch.Count} kartu misi untuk level {currentLevel}.");

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
                }
            }
        }

        /// <summary>
        /// Menampilkan kartu secara langsung ke formasi kipas.
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

            if (_shuffleCoroutine != null)
            {
                StopCoroutine(_shuffleCoroutine);
            }

            _shuffleCoroutine = StartCoroutine(ShuffleSequenceRoutine());
        }

        /// <summary>
        /// Coroutine urutan kocok kartu 3 detik sesuai spesifikasi HTML template.
        /// </summary>
        private IEnumerator ShuffleSequenceRoutine()
        {
            _isShuffling = true;

            // Sembunyikan atau tarik kartu ke bawah saat dikocok
            if (cardObjects != null)
            {
                foreach (var card in cardObjects)
                {
                    if (card != null) card.SetActive(false);
                }
            }

            // Update status tombol
            if (bottomDeckButton != null) bottomDeckButton.interactable = false;
            if (shuffleButtonText != null) shuffleButtonText.text = "SHUFFLING...";

            // Tampilkan status pill
            if (shuffleStatusPill != null) shuffleStatusPill.SetActive(true);
            if (shuffleStatusText != null) shuffleStatusText.text = "Shuffling deck (3s)...";

            AudioManager.Instance?.PlayCardFlip();

            // Efek getar tumpukan kartu meja
            Vector2 origPos = bottomDeckTransform != null ? bottomDeckTransform.anchoredPosition : Vector2.zero;
            float elapsed = 0f;

            while (elapsed < 3.0f)
            {
                elapsed += Time.deltaTime;

                if (bottomDeckTransform != null)
                {
                    float jitterX = Random.Range(-4f, 4f);
                    float jitterY = Random.Range(-3f, 3f);
                    bottomDeckTransform.anchoredPosition = origPos + new Vector2(jitterX, jitterY);
                }

                if (elapsed >= 2.0f && shuffleStatusText != null)
                {
                    shuffleStatusText.text = "Shuffling deck (1s)...";
                }
                else if (elapsed >= 1.0f && shuffleStatusText != null)
                {
                    shuffleStatusText.text = "Shuffling deck (2s)...";
                }

                yield return null;
            }

            // Kembalikan posisi tumpukan kartu meja
            if (bottomDeckTransform != null)
            {
                bottomDeckTransform.anchoredPosition = origPos;
            }

            // Muat batch soal baru secara acak
            LoadMissionDeck();

            // Bagikan kartu
            if (shuffleStatusText != null)
            {
                shuffleStatusText.text = "Dealing 5 mission cards!";
            }
            AudioManager.Instance?.PlayCardFlip();

            DealCardsImmediately();

            // Tunggu 0.8 detik sebelum menyembunyikan status pill
            yield return new WaitForSeconds(0.8f);

            if (shuffleStatusPill != null) shuffleStatusPill.SetActive(false);
            if (bottomDeckButton != null) bottomDeckButton.interactable = true;
            if (shuffleButtonText != null) shuffleButtonText.text = "SHUFFLE AGAIN";

            _isShuffling = false;
            _shuffleCoroutine = null;
        }

        /// <summary>
        /// Dipanggil saat pemain mengetuk salah satu kartu misteri tertutup.
        /// </summary>
        public void OnCardSelected(int index)
        {
            if (_isShuffling) return;

            if (_currentBatch == null || index < 0 || index >= _currentBatch.Count)
            {
                Debug.LogWarning($"[MissionCardController] Indeks kartu {index} tidak valid.");
                return;
            }

            _selectedQuestion = _currentBatch[index];
            Debug.Log($"[MissionCardController] Kartu #{index + 1} dipilih: {_selectedQuestion.id} - {_selectedQuestion.title}");

            // Putar suara desir buka kartu
            AudioManager.Instance?.PlayCardFlip();

            // Isi data ke modal kartu misi
            int currentNumber = (GameSession.Instance != null ? GameSession.Instance.TotalAnswered : 0) + 1;

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

            if (missionDetailModal != null)
            {
                missionDetailModal.SetActive(true);
            }
        }

        /// <summary>
        /// Menutup modal detail misi dan kembali ke deretan kartu kipas.
        /// </summary>
        public void OnBackToCardsClicked()
        {
            Debug.Log("[MissionCardController] Menutup modal detail misi, kembali ke kartu.");
            AudioManager.Instance?.PlayButtonClick();

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
                Debug.LogWarning("[MissionCardController] Belum ada soal yang dipilih.");
                return;
            }

            Debug.Log($"[MissionCardController] Melanjutkan ke papan aljabar dengan soal {_selectedQuestion.id}");

            AudioManager.Instance?.PlayButtonClick();

            // Simpan soal aktif ke sesi permainan dan tandai ID selesai (DEC-10)
            if (GameSession.Instance != null)
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
            Debug.Log("[MissionCardController] Kembali ke Level Selection.");
            AudioManager.Instance?.PlayButtonClick();
            SceneNavigator.GoToLevelSelection();
        }
    }
}
