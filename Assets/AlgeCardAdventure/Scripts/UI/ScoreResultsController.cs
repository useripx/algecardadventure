using System;
using UnityEngine;
using UnityEngine.UI;
using AlgeCardAdventure.Core;
using AlgeCardAdventure.Session;
using AlgeCardAdventure.Audio;

namespace AlgeCardAdventure.UI
{
    /// <summary>
    /// Controller untuk Layar 06: Score Results, Session Loop & Credits Scene.
    /// Mengimplementasikan:
    /// 1. Tampilan skor akurasi dinamis berbasis DEC-09: Score = Round((Correct / Answered) * 100).
    /// 2. Matriks 3 kartu statistik: Correct, Wrong, dan Durasi Waktu sesi (MM:SS).
    /// 3. Umpan balik motivasi adaptif (Very good, Keep practicing, dll.).
    /// 4. Tombol aksi cerdas: "CONTINUE QUESTION" (< 10 soal) atau "PLAY AGAIN" (>= 10 soal) sesuai DEC-08.
    /// 5. Integrasi Credits Scene formal (DEC-15) dengan atribusi resmi:
    ///    - Software Developer: Yogi Ario
    ///    - Game Planner and Math Expert: Nisa Aprilia Wati
    ///    - Academic Advisor: Dr. Resty Wulanningrum, M.Kom.
    ///    - Academic Institution: Universitas Nusantara PGRI Kediri
    /// </summary>
    public class ScoreResultsController : MonoBehaviour
    {
        [Header("Header & Main Score")]
        [SerializeField] private Text scoreBigText;
        [SerializeField] private Text maxScoreText;
        [SerializeField] private Image starLaurelImage;
        [SerializeField] private Text headlineFeedbackText;
        [SerializeField] private Text subtitleFeedbackText;

        [Header("Performance Stats (3 Cards)")]
        [SerializeField] private Text statCorrectCountText;
        [SerializeField] private Text statCorrectTotalText;
        [SerializeField] private Text statWrongCountText;
        [SerializeField] private Text statWrongTotalText;
        [SerializeField] private Text statTimeDisplayText;

        [Header("Action Buttons")]
        [SerializeField] private Button continueOrReplayButton;
        [SerializeField] private Text continueOrReplayLabelText;
        [SerializeField] private Text continueOrReplaySubText;
        [SerializeField] private Image continueOrReplayButtonImage;
        [SerializeField] private Sprite btnBlueSprite;
        [SerializeField] private Sprite btnGoldSprite;
        [SerializeField] private Button exitButton;

        [Header("Credits Modal (DEC-15)")]
        [SerializeField] private GameObject creditsModal;
        [SerializeField] private Text softwareDevNameText;
        [SerializeField] private Text mathExpertNameText;
        [SerializeField] private Text academicAdvisorText;
        [SerializeField] private Text institutionText;
        [SerializeField] private Button creditsCloseButton;

        // Runtime state
        public int ComputedScore { get; private set; } = 0;
        public int TotalCorrect { get; private set; } = 0;
        public int TotalWrong { get; private set; } = 0;
        public int TotalAnswered { get; private set; } = 0;
        public float SessionDurationSeconds { get; private set; } = 0f;
        public bool IsCreditsOpen => creditsModal != null && creditsModal.activeSelf;
        public bool IsSessionCompleted => TotalAnswered >= GameSession.MaxQuestionsPerSession;

        private void Start()
        {
            InitializeScreen();
        }

        public void InitializeScreen()
        {
            ReadSessionData();
            SetupScoreDisplay();
            SetupStatisticsCards();
            SetupFeedbackBox();
            SetupActionButtons();
            SetupCreditsModal();
            SetupListeners();

            SafePlayVictoryFanfare();
        }

        private void ReadSessionData()
        {
            if (GameSession.Instance != null)
            {
                TotalCorrect = GameSession.Instance.TotalCorrect;
                TotalAnswered = GameSession.Instance.TotalAnswered;
                SessionDurationSeconds = GameSession.Instance.SessionTimeSeconds;
            }
            else
            {
                // Fallback default untuk EditMode / Testing langsung
                TotalCorrect = 4;
                TotalAnswered = 5;
                SessionDurationSeconds = 155f; // 02:35
            }

            TotalWrong = Mathf.Max(0, TotalAnswered - TotalCorrect);
            ComputedScore = CalculateScorePercentage(TotalCorrect, TotalAnswered);
        }

        public int CalculateScorePercentage(int correct, int answered)
        {
            if (answered <= 0) return 0;
            return Mathf.RoundToInt((float)correct / answered * 100f);
        }

        public string FormatSessionTime(float seconds)
        {
            if (seconds < 0f) seconds = 0f;
            int mins = Mathf.FloorToInt(seconds / 60f);
            int secs = Mathf.FloorToInt(seconds % 60f);
            return $"{mins:00}:{secs:00}";
        }

        public string GetFeedbackHeadline(int score)
        {
            if (score >= 80) return "Very good!";
            if (score >= 60) return "Good job!";
            return "Nice effort!";
        }

        public string GetFeedbackSubtitle(int score)
        {
            if (score >= 80) return "Outstanding! Keep up the brilliant math journey!";
            if (score >= 60) return "Keep practicing to become better and sharper!";
            return "Don't give up! Every mistake is a step to wisdom!";
        }

        private void SetupScoreDisplay()
        {
            if (scoreBigText != null) scoreBigText.text = ComputedScore.ToString();
            if (maxScoreText != null) maxScoreText.text = "out of 100";
        }

        private void SetupStatisticsCards()
        {
            if (statCorrectCountText != null) statCorrectCountText.text = TotalCorrect.ToString();
            if (statCorrectTotalText != null) statCorrectTotalText.text = $"out of {TotalAnswered}";

            if (statWrongCountText != null) statWrongCountText.text = TotalWrong.ToString();
            if (statWrongTotalText != null) statWrongTotalText.text = $"out of {TotalAnswered}";

            if (statTimeDisplayText != null) statTimeDisplayText.text = FormatSessionTime(SessionDurationSeconds);
        }

        private void SetupFeedbackBox()
        {
            if (headlineFeedbackText != null) headlineFeedbackText.text = GetFeedbackHeadline(ComputedScore);
            if (subtitleFeedbackText != null) subtitleFeedbackText.text = GetFeedbackSubtitle(ComputedScore);
        }

        private void SetupActionButtons()
        {
            if (IsSessionCompleted)
            {
                // Sesi 10 soal penuh (DEC-08): Play Again
                if (continueOrReplayLabelText != null) continueOrReplayLabelText.text = "PLAY AGAIN";
                if (continueOrReplaySubText != null) continueOrReplaySubText.text = "Start New Adventure";
                if (continueOrReplayButtonImage != null && btnGoldSprite != null)
                {
                    continueOrReplayButtonImage.sprite = btnGoldSprite;
                }
            }
            else
            {
                // Soal masih di bawah 10: Continue Question
                if (continueOrReplayLabelText != null) continueOrReplayLabelText.text = "CONTINUE QUESTION";
                if (continueOrReplaySubText != null) continueOrReplaySubText.text = "To add more Points";
                if (continueOrReplayButtonImage != null && btnBlueSprite != null)
                {
                    continueOrReplayButtonImage.sprite = btnBlueSprite;
                }
            }
        }

        private void SetupCreditsModal()
        {
            if (creditsModal != null) creditsModal.SetActive(false);

            if (softwareDevNameText != null) softwareDevNameText.text = "Yogi Ario";
            if (mathExpertNameText != null) mathExpertNameText.text = "Nisa Aprilia Wati";
            if (academicAdvisorText != null) academicAdvisorText.text = "Dr. Resty Wulanningrum, M.Kom.";
            if (institutionText != null) institutionText.text = "Universitas Nusantara PGRI Kediri";
        }

        private void SetupListeners()
        {
            if (continueOrReplayButton != null)
            {
                continueOrReplayButton.onClick.RemoveAllListeners();
                continueOrReplayButton.onClick.AddListener(OnContinueOrReplayClicked);
            }

            if (exitButton != null)
            {
                exitButton.onClick.RemoveAllListeners();
                exitButton.onClick.AddListener(OnExitClicked);
            }

            if (creditsCloseButton != null)
            {
                creditsCloseButton.onClick.RemoveAllListeners();
                creditsCloseButton.onClick.AddListener(OnCloseCreditsClicked);
            }
        }

        public void OnContinueOrReplayClicked()
        {
            SafePlayButtonClick();

            if (IsSessionCompleted)
            {
                // Reset sesi dan mulai petualangan baru di Level Selection
                GameSession.Instance?.ResetSession();
                SceneNavigator.LoadLevelSelection();
            }
            else
            {
                // Lanjut ke soal berikutnya di Mission Card
                SceneNavigator.LoadMissionCard();
            }
        }

        public void OnExitClicked()
        {
            SafePlayButtonClick();

            // Sesuai DEC-15: Membuka Credits Modal resmi tim
            if (creditsModal != null)
            {
                creditsModal.SetActive(true);
            }
            else
            {
                // Fallback jika modal tidak ditautkan: kembali ke Splash
                GameSession.Instance?.ResetSession();
                SceneNavigator.LoadSplashScreen();
            }
        }

        public void OnCloseCreditsClicked()
        {
            SafePlayButtonClick();

            if (creditsModal != null) creditsModal.SetActive(false);

            // Selesai melihat credits, reset sesi dan kembali ke layar awal
            GameSession.Instance?.ResetSession();
            SceneNavigator.LoadSplashScreen();
        }

        private void SafePlayButtonClick()
        {
            try { AudioManager.Instance?.PlayButtonClick(); }
            catch { /* Hindari kegagalan saat testing / headless */ }
        }

        private void SafePlayVictoryFanfare()
        {
            try { AudioManager.Instance?.PlayCorrectAnswer(); }
            catch { /* Hindari kegagalan saat testing / headless */ }
        }
    }
}
