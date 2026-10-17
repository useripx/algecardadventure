using System;
using UnityEngine;
using AlgeCardAdventure.Data;
using AlgeCardAdventure.Core;

namespace AlgeCardAdventure.Session
{
    /// <summary>
    /// Mengelola status sesi bermain aktif (Singleton), meliputi tingkat kesulitan,
    /// batas 10 soal per sesi (DEC-08), progres jawaban benar, dan soal yang sedang dimainkan.
    /// </summary>
    public class GameSession : MonoBehaviour
    {
        public const int MaxQuestionsPerSession = 10;

        private static GameSession _instance;

        public static GameSession Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<GameSession>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("GameSession");
                        _instance = go.AddComponent<GameSession>();
                        if (Application.isPlaying)
                        {
                            DontDestroyOnLoad(go);
                        }
                    }
                }
                return _instance;
            }
        }

        public string CurrentLevel { get; private set; } = "Easy";
        public int TotalAnswered { get; private set; } = 0;
        public int CorrectCount { get; private set; } = 0;
        public QuestionModel CurrentQuestion { get; private set; }
        public bool IsSessionActive { get; private set; } = false;

        public event Action<string> OnSessionStarted;
        public event Action<int, int> OnAnswerRecorded;
        public event Action OnSessionReset;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                if (Application.isPlaying)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Memulai sesi bermain baru dengan level yang dipilih (Easy, Normal, Hard).
        /// </summary>
        public void StartNewSession(string level)
        {
            if (string.IsNullOrWhiteSpace(level))
            {
                level = "Easy";
            }

            // Normalisasi penamaan level (DEC-03 & DEC-04)
            string normalized = level.Trim();
            if (normalized.Equals("Medium", StringComparison.OrdinalIgnoreCase))
            {
                normalized = "Normal";
            }

            if (!normalized.Equals("Easy", StringComparison.OrdinalIgnoreCase) &&
                !normalized.Equals("Normal", StringComparison.OrdinalIgnoreCase) &&
                !normalized.Equals("Hard", StringComparison.OrdinalIgnoreCase))
            {
                normalized = "Easy";
            }

            CurrentLevel = normalized;
            TotalAnswered = 0;
            CorrectCount = 0;
            CurrentQuestion = null;
            IsSessionActive = true;

            Debug.Log($"[GameSession] Sesi baru dimulai untuk level: {CurrentLevel}");
            OnSessionStarted?.Invoke(CurrentLevel);
        }

        /// <summary>
        /// Menetapkan soal aktif yang dipilih dari kartu misi (Screen 03).
        /// </summary>
        public void SetCurrentQuestion(QuestionModel question)
        {
            CurrentQuestion = question;
        }

        /// <summary>
        /// Mencatat hasil jawaban dari sebuah soal (benar atau salah).
        /// </summary>
        public void RecordAnswer(bool isCorrect)
        {
            if (!IsSessionActive) return;

            TotalAnswered++;
            if (isCorrect)
            {
                CorrectCount++;
            }

            OnAnswerRecorded?.Invoke(TotalAnswered, CorrectCount);
        }

        /// <summary>
        /// Menilai apakah sesi telah mencapai batas maksimal 10 soal (DEC-08).
        /// </summary>
        public bool HasReachedLimit()
        {
            return TotalAnswered >= MaxQuestionsPerSession;
        }

        /// <summary>
        /// Menghitung persentase skor akhir sesi menggunakan ScoreCalculator (DEC-09).
        /// </summary>
        public int GetCurrentScore()
        {
            return ScoreCalculator.CalculateScore(CorrectCount, TotalAnswered);
        }

        /// <summary>
        /// Mengakhiri dan mereset sesi aktif (misalnya saat menekan Exit).
        /// </summary>
        public void ResetSession()
        {
            TotalAnswered = 0;
            CorrectCount = 0;
            CurrentQuestion = null;
            IsSessionActive = false;

            OnSessionReset?.Invoke();
        }
    }
}
