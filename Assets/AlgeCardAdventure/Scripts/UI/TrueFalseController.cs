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
    /// Controller untuk Layar 05: True or False & Explanation.
    /// Mengimplementasikan evaluasi otomatis (bukan tombol tebakan) sesuai arahan Yogi Ario:
    /// 1. Otomatis menilai kebenaran pengerjaan papan aljabar dari Layar 04.
    /// 2. Menampilkan badge status √ TRUE / ✕ FALSE (Elemen Display, BUKAN TOMBOL).
    /// 3. Menampilkan kartu feedback selama 3 detik (DEC-11) dengan progress bar dan tombol Skip.
    /// 4. Membuka papan perkamen penjelasan matematis langkah-demi-langkah.
    /// 5. Tombol Next Question (jika < 10 soal) atau View Results (jika soal ke-10, akhir sesi).
    /// </summary>
    public class TrueFalseController : MonoBehaviour
    {
        [Header("Top Navigation & HUD")]
        [SerializeField] private Button backButton;
        [SerializeField] private Text missionTitleText;
        [SerializeField] private Text hudScoreText;
        [SerializeField] private Text hudStreakText;
        [SerializeField] private Text questionCounterBadgeText;

        [Header("Question Statement Board")]
        [SerializeField] private Text questionStatementText;
        [SerializeField] private GameObject badgeTrueObj;
        [SerializeField] private GameObject badgeFalseObj;
        [SerializeField] private GameObject badgeTrueHighlight;
        [SerializeField] private GameObject badgeFalseHighlight;

        [Header("State B: 3-Second Feedback (DEC-11)")]
        [SerializeField] private GameObject flowArrow1;
        [SerializeField] private GameObject correctFeedbackCard;
        [SerializeField] private GameObject incorrectFeedbackCard;
        [SerializeField] private Text correctCountdownText;
        [SerializeField] private Text incorrectCountdownText;
        [SerializeField] private Image correctProgressBar;
        [SerializeField] private Image incorrectProgressBar;
        [SerializeField] private Button skipCountdownButton;

        [Header("State C: Explanation Parchment Board")]
        [SerializeField] private GameObject flowArrow2;
        [SerializeField] private GameObject explanationSection;
        [SerializeField] private Text problemRestatementText;
        [SerializeField] private Text step1Text;
        [SerializeField] private Text step2Text;
        [SerializeField] private Text step3Text;
        [SerializeField] private Text resultEquationText;
        [SerializeField] private Text resultConclusionText;
        [SerializeField] private Button nextQuestionButton;
        [SerializeField] private Text nextQuestionButtonText;

        [Header("Scrolling")]
        [SerializeField] private ScrollRect mainScrollRect;
        [SerializeField] private RectTransform scrollContent;

        // Runtime State
        public bool IsEvaluated { get; private set; } = false;
        public bool IsCorrectEvaluation { get; private set; } = true;
        public bool IsExplanationRevealed { get; private set; } = false;
        public float CountdownDuration { get; set; } = 3.0f;
        public float RemainingCountdown { get; set; } = 3.0f;
        public QuestionModel ActiveQuestion { get; private set; }

        private Coroutine _countdownCoroutine;

        public TrueFalseController()
        {
            CountdownDuration = 3.0f;
            RemainingCountdown = 3.0f;
        }

        private void Start()
        {
            InitializeScreen();
        }

        public void InitializeScreen()
        {
            if (CountdownDuration <= 0f) CountdownDuration = 3.0f;
            if (RemainingCountdown <= 0f) RemainingCountdown = CountdownDuration;

            // Ambil soal aktif dari GameSession
            ActiveQuestion = GameSession.Instance?.CurrentQuestion;
            if (ActiveQuestion == null)
            {
                ActiveQuestion = CreateFallbackQuestion();
            }

            // Tentukan status kebenaran jawaban berdasarkan pengerjaan di board (Layar 04)
            bool isCorrect = true;
            if (GameSession.Instance != null)
            {
                isCorrect = GameSession.Instance.LastBoardSolutionCorrect;
            }

            // Catat hasil evaluasi ke GameSession (tepat 1 kali)
            if (!IsEvaluated)
            {
                IsEvaluated = true;
                IsCorrectEvaluation = isCorrect;
                GameSession.Instance?.RecordAnswer(isCorrect);
                if (ActiveQuestion != null)
                {
                    GameSession.Instance?.MarkQuestionCompleted(ActiveQuestion.id);
                }
            }

            SetupTopBarAndHUD();
            SetupQuestionStatementAndBadges(isCorrect);
            SetupExplanationContent(isCorrect);
            SetupButtonListeners();

            // Sembunyikan Explanation pada awal tampilan
            if (flowArrow2 != null) flowArrow2.SetActive(false);
            if (explanationSection != null) explanationSection.SetActive(false);
            IsExplanationRevealed = false;

            // Mainkan audio feedback
            if (isCorrect) SafePlayCorrect();
            else SafePlayWrong();

            // Mulai hitung mundur 3 detik (DEC-11)
            if (gameObject != null && gameObject.activeInHierarchy)
            {
                if (_countdownCoroutine != null) StopCoroutine(_countdownCoroutine);
                _countdownCoroutine = StartCoroutine(FeedbackCountdownRoutine(isCorrect));
            }
        }

        public void EvaluateSession(QuestionModel question, bool isCorrect)
        {
            if (CountdownDuration <= 0f) CountdownDuration = 3.0f;
            if (RemainingCountdown <= 0f) RemainingCountdown = CountdownDuration;

            ActiveQuestion = question ?? CreateFallbackQuestion();
            IsEvaluated = true;
            IsCorrectEvaluation = isCorrect;
            IsExplanationRevealed = false;
        }

        private void SetupTopBarAndHUD()
        {
            int currentScore = GameSession.Instance != null ? GameSession.Instance.GetCurrentScore() : 100;
            int currentStreak = GameSession.Instance != null ? GameSession.Instance.CurrentStreak : 1;
            int answeredCount = GameSession.Instance != null ? GameSession.Instance.TotalAnswered : 1;

            if (hudScoreText != null) hudScoreText.text = $"{currentScore * 10} pts";
            if (hudStreakText != null) hudStreakText.text = $"{currentStreak}";
            if (questionCounterBadgeText != null) questionCounterBadgeText.text = $"Question {answeredCount} of {GameSession.MaxQuestionsPerSession}";
            if (missionTitleText != null) missionTitleText.text = $"Mission {answeredCount}\nTrue or False?";
        }

        private void SetupQuestionStatementAndBadges(bool isCorrect)
        {
            string statement = GenerateStatement(ActiveQuestion);
            if (questionStatementText != null)
            {
                questionStatementText.text = statement;
            }

            // Badge status: √ TRUE / ✕ FALSE (BUKAN TOMBOL)
            if (badgeTrueObj != null) badgeTrueObj.SetActive(isCorrect);
            if (badgeFalseObj != null) badgeFalseObj.SetActive(!isCorrect);

            if (badgeTrueHighlight != null) badgeTrueHighlight.SetActive(isCorrect);
            if (badgeFalseHighlight != null) badgeFalseHighlight.SetActive(!isCorrect);

            // Tampilkan kartu feedback yang sesuai
            if (flowArrow1 != null) flowArrow1.SetActive(true);
            if (correctFeedbackCard != null) correctFeedbackCard.SetActive(isCorrect);
            if (incorrectFeedbackCard != null) incorrectFeedbackCard.SetActive(!isCorrect);
        }

        private void SetupExplanationContent(bool isCorrect)
        {
            string variable = ExtractVariable(ActiveQuestion);
            string finalAns = CleanAnswerNumber(ActiveQuestion?.final_answer);
            string eq = ActiveQuestion?.equation_model != null && ActiveQuestion.equation_model.Length > 0
                ? ActiveQuestion.equation_model[0]
                : "3x + 5 = 20";

            if (problemRestatementText != null)
            {
                if (ActiveQuestion?.solution_steps != null && ActiveQuestion.solution_steps.Length > 0 && !string.IsNullOrEmpty(ActiveQuestion.solution_steps[0]))
                {
                    problemRestatementText.text = $"Problem Concept:\n{ActiveQuestion.solution_steps[0]}";
                }
                else
                {
                    problemRestatementText.text = $"Let's check the statement:\nIf {variable} = {finalAns}, then the value of {eq}.";
                }
            }

            var steps = GenerateExplanationSteps(ActiveQuestion, isCorrect);
            if (step1Text != null && steps.Count > 0)
            {
                step1Text.resizeTextForBestFit = true;
                step1Text.resizeTextMinSize = 14;
                step1Text.resizeTextMaxSize = 22;
                step1Text.horizontalOverflow = HorizontalWrapMode.Wrap;
                step1Text.text = $"1. {steps[0]}";
            }
            if (step2Text != null && steps.Count > 1)
            {
                step2Text.resizeTextForBestFit = true;
                step2Text.resizeTextMinSize = 14;
                step2Text.resizeTextMaxSize = 22;
                step2Text.horizontalOverflow = HorizontalWrapMode.Wrap;
                step2Text.text = $"2. {steps[1]}";
            }
            if (step3Text != null && steps.Count > 2)
            {
                step3Text.resizeTextForBestFit = true;
                step3Text.resizeTextMinSize = 14;
                step3Text.resizeTextMaxSize = 22;
                step3Text.horizontalOverflow = HorizontalWrapMode.Wrap;
                step3Text.text = $"3. {steps[2]}";
            }

            if (resultEquationText != null)
            {
                resultEquationText.text = !string.IsNullOrEmpty(ActiveQuestion?.final_answer)
                    ? $"Final Answer: {ActiveQuestion.final_answer}"
                    : eq;
            }

            if (resultConclusionText != null)
            {
                resultConclusionText.text = isCorrect
                    ? "So, this statement is <color=#15803d><b>TRUE</b></color>."
                    : "So, this statement is <color=#b91c1c><b>FALSE</b></color>.";
            }

            // Tombol NEXT (selalu mengarah ke 06_ScoreResults)
            if (nextQuestionButtonText != null)
            {
                nextQuestionButtonText.text = "NEXT ❯";
            }
        }

        private void SetupButtonListeners()
        {
            if (skipCountdownButton != null)
            {
                skipCountdownButton.onClick.RemoveAllListeners();
                skipCountdownButton.onClick.AddListener(OnSkipClicked);
            }

            if (nextQuestionButton != null)
            {
                nextQuestionButton.onClick.RemoveAllListeners();
                nextQuestionButton.onClick.AddListener(OnNextQuestionClicked);
            }

            if (backButton != null)
            {
                backButton.onClick.RemoveAllListeners();
                backButton.onClick.AddListener(OnBackClicked);
            }
        }

        private IEnumerator FeedbackCountdownRoutine(bool isCorrect)
        {
            RemainingCountdown = CountdownDuration;
            Text countdownText = isCorrect ? correctCountdownText : incorrectCountdownText;
            Image progressBar = isCorrect ? correctProgressBar : incorrectProgressBar;

            while (RemainingCountdown > 0f)
            {
                if (countdownText != null)
                {
                    countdownText.text = $"{Mathf.CeilToInt(RemainingCountdown)}";
                }

                if (progressBar != null)
                {
                    progressBar.fillAmount = Mathf.Clamp01(RemainingCountdown / CountdownDuration);
                }

                yield return new WaitForSeconds(0.1f);
                RemainingCountdown -= 0.1f;
            }

            RemainingCountdown = 0f;
            if (countdownText != null) countdownText.text = "0";
            if (progressBar != null) progressBar.fillAmount = 0f;

            RevealExplanation();
        }

        public void OnSkipClicked()
        {
            SafePlayButtonClick();
            if (_countdownCoroutine != null)
            {
                StopCoroutine(_countdownCoroutine);
                _countdownCoroutine = null;
            }
            RemainingCountdown = 0f;
            RevealExplanation();
        }

        public void RevealExplanation()
        {
            IsExplanationRevealed = true;

            if (flowArrow2 != null) flowArrow2.SetActive(true);
            if (explanationSection != null) explanationSection.SetActive(true);

            // Auto-scroll smooth ke arah explanation section
            if (scrollContent != null)
            {
                StartCoroutine(SmoothScrollToY(450f));
            }
        }

        private IEnumerator SmoothScrollToY(float targetY)
        {
            if (scrollContent == null) yield break;

            Vector2 start = scrollContent.anchoredPosition;
            Vector2 end = new Vector2(start.x, targetY);
            float elapsed = 0f;
            float duration = 0.35f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
                scrollContent.anchoredPosition = Vector2.Lerp(start, end, t);
                yield return null;
            }
            scrollContent.anchoredPosition = end;
        }

        public void OnNextQuestionClicked()
        {
            SafePlayButtonClick();
            int answered = GameSession.Instance != null ? GameSession.Instance.TotalAnswered : 1;
            string targetScene = GetNextScene(answered);
            SceneNavigator.LoadScene(targetScene);
        }

        public void OnBackClicked()
        {
            SafePlayButtonClick();
            SceneNavigator.GoToMissionCard();
        }

        public static string GenerateStatement(QuestionModel q, string overrideVal = null)
        {
            if (q == null) return "If x = 5, then 3x + 5 = 20.";
            string variable = ExtractVariable(q);
            string finalAns = overrideVal ?? CleanAnswerNumber(q.final_answer);
            string eq = q.equation_model != null && q.equation_model.Length > 0 ? q.equation_model[0] : "3x + 5 = 20";
            return $"If {variable} = {finalAns}, then the value of {eq}.";
        }

        public static List<string> GenerateExplanationSteps(QuestionModel q, bool isCorrect)
        {
            var steps = new List<string>();
            string variable = ExtractVariable(q);
            string finalAns = CleanAnswerNumber(q?.final_answer);
            string eq = q?.equation_model != null && q.equation_model.Length > 0 ? q.equation_model[0] : "3x + 5 = 20";

            if (q?.solution_steps != null && q.solution_steps.Length > 1)
            {
                // Mengambil langkah aljabar nyata dari banksoal.json
                if (q.solution_steps.Length >= 4)
                {
                    // Item 0 adalah narasi konsep/definisi, item 1 adalah formula, item 2 adalah operasi invers/pindah ruas, item akhir adalah hasil
                    steps.Add($"Equation: {q.solution_steps[1]}");
                    steps.Add($"Operation: {q.solution_steps[2]}");
                    steps.Add($"Solution: {q.solution_steps[q.solution_steps.Length - 1]}");
                }
                else if (q.solution_steps.Length == 3)
                {
                    steps.Add($"Equation: {q.solution_steps[0]}");
                    steps.Add($"Operation: {q.solution_steps[1]}");
                    steps.Add($"Solution: {q.solution_steps[2]}");
                }
                else // Length == 2
                {
                    steps.Add($"Equation: {q.solution_steps[0]}");
                    steps.Add($"Operation: {q.solution_steps[1]}");
                    steps.Add($"Solution: {variable} = {finalAns}");
                }
            }
            else
            {
                // Fallback substitusi jika solution_steps tidak tersedia (misal mock testing)
                steps.Add($"Substitute {variable} = {finalAns} into the equation.");
                steps.Add($"Evaluate arithmetic substitution step.");
                steps.Add($"Result matches {eq}.");
            }

            return steps;
        }

        public static string GetNextScene(int totalAnswered = 0, int maxQuestions = GameSession.MaxQuestionsPerSession)
        {
            // Sesuai alur resmi AlgeCard Adventure: Dari Layar 05 (True/False & Explanation),
            // pemain selalu melanjutkan ke Layar 06 (Score Results) untuk melihat skor/statistik.
            return SceneNavigator.SceneScoreResults;
        }

        public static string GetResultConclusion(bool isCorrect)
        {
            return isCorrect ? "So, this statement is TRUE." : "So, this statement is FALSE.";
        }

        public static string ExtractVariable(QuestionModel q)
        {
            if (q == null) return "x";
            if (q.equation_model != null && q.equation_model.Length > 0)
            {
                foreach (char c in q.equation_model[0])
                {
                    if (char.IsLetter(c)) return c.ToString().ToLower();
                }
            }
            return "x";
        }

        public static string CleanAnswerNumber(string ans)
        {
            if (string.IsNullOrWhiteSpace(ans)) return "5";
            string digits = "";
            foreach (char c in ans)
            {
                if (char.IsDigit(c) || c == '-' || c == '.') digits += c;
            }
            return string.IsNullOrEmpty(digits) ? ans.Trim() : digits;
        }

        private QuestionModel CreateFallbackQuestion()
        {
            return new QuestionModel
            {
                id = "PILOT-001",
                ui_level = "Easy",
                title = "Solving for x",
                question_text = "Find the value of x in 3x + 5 = 20.",
                equation_model = new string[] { "3x + 5 = 20" },
                solution_steps = new string[] { "3x + 5 = 20", "3x = 20 - 5", "3x = 15", "x = 5" },
                final_answer = "5"
            };
        }

        private void SafePlayButtonClick()
        {
            try { AudioManager.Instance?.PlayButtonClick(); } catch { }
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
