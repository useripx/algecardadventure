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
    /// animasi buka kartu (flip), modal detail soal misi, dan navigasi ke Screen 04.
    /// </summary>
    public class MissionCardController : MonoBehaviour
    {
        [Header("Navigasi & Header")]
        [SerializeField] private Button backButton;
        [SerializeField] private Text levelTitleText;
        [SerializeField] private Text levelSubtitleText;

        [Header("Deretan Kartu Misi")]
        [SerializeField] private Button[] cardButtons;
        [SerializeField] private GameObject[] cardObjects;

        [Header("Modal Detail Misi")]
        [SerializeField] private GameObject missionDetailModal;
        [SerializeField] private Text missionNumberText;
        [SerializeField] private Text missionTitleText;
        [SerializeField] private Text missionInstructionText;
        [SerializeField] private Text equationPreviewText;
        [SerializeField] private Button continueButton;

        private List<QuestionModel> _currentBatch = new List<QuestionModel>();
        private QuestionModel _selectedQuestion;
        private QuestionRepository _repository;

        public IReadOnlyList<QuestionModel> CurrentBatch => _currentBatch;
        public QuestionModel SelectedQuestion => _selectedQuestion;

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

            // Sembunyikan modal di awal
            if (missionDetailModal != null)
            {
                missionDetailModal.SetActive(false);
            }

            // Muat dan tampilkan batch kartu misi
            LoadMissionDeck();
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
                    if (cardObjects != null && cardIndex < cardObjects.Length && cardObjects[cardIndex] != null)
                    {
                        cardObjects[cardIndex].SetActive(true);
                    }

                    if (cardButtons[cardIndex] != null)
                    {
                        cardButtons[cardIndex].onClick.RemoveAllListeners();
                        cardButtons[cardIndex].onClick.AddListener(() => OnCardSelected(cardIndex));
                    }
                }
                else
                {
                    if (cardObjects != null && cardIndex < cardObjects.Length && cardObjects[cardIndex] != null)
                    {
                        cardObjects[cardIndex].SetActive(false);
                    }
                }
            }
        }

        /// <summary>
        /// Dipanggil saat pemain mengetuk salah satu kartu misteri tertutup.
        /// </summary>
        public void OnCardSelected(int index)
        {
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
                missionNumberText.text = $"Question {currentNumber} of {GameSession.MaxQuestionsPerSession}";
            }

            if (missionTitleText != null)
            {
                missionTitleText.text = _selectedQuestion.title ?? "Algebra Mission";
            }

            if (missionInstructionText != null)
            {
                missionInstructionText.text = _selectedQuestion.question_text ?? string.Empty;
            }

            if (equationPreviewText != null)
            {
                if (_selectedQuestion.equation_model != null && _selectedQuestion.equation_model.Count > 0)
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
