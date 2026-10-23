using System;
using System.Collections.Generic;
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
                if (ReferenceEquals(_instance, null))
                {
                    TryResolveUnityInstance();
                    if (ReferenceEquals(_instance, null))
                    {
                        _instance = (GameSession)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(GameSession));
                    }
                }
                return _instance;
            }
            set => _instance = value;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        private static void TryResolveUnityInstance()
        {
            try
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
            catch
            {
                // Defensif untuk lingkungan non-Unity runtime/testing
            }
        }

        public string CurrentLevel { get; private set; } = "Easy";
        public int TotalAnswered { get; private set; } = 0;
        public int CorrectCount { get; private set; } = 0;
        public int TotalCorrect => CorrectCount;
        public int TotalWrong => Mathf.Max(0, TotalAnswered - CorrectCount);
        public float SessionTimeSeconds => Application.isPlaying ? Mathf.Max(0f, Time.time - _sessionStartTime) : 0f;
        public int CurrentStreak { get; private set; } = 0;
        public bool LastBoardSolutionCorrect { get; set; } = true;
        public QuestionModel CurrentQuestion { get; set; }
        public bool IsSessionActive { get; private set; } = false;
        public List<string> CompletedQuestionIds { get; } = new List<string>();

        private float _sessionStartTime;

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
            CompletedQuestionIds.Clear();
            IsSessionActive = true;
            _sessionStartTime = Application.isPlaying ? Time.time : 0f;

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
        /// Menandai sebuah soal telah selesai dikerjakan agar tidak diulang dalam sesi yang sama (DEC-10).
        /// </summary>
        public void MarkQuestionCompleted(string questionId)
        {
            if (string.IsNullOrWhiteSpace(questionId)) return;
            if (!CompletedQuestionIds.Contains(questionId))
            {
                CompletedQuestionIds.Add(questionId);
            }
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
                CurrentStreak++;
            }
            else
            {
                CurrentStreak = 0;
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
            CurrentStreak = 0;
            LastBoardSolutionCorrect = true;
            CurrentQuestion = null;
            CompletedQuestionIds.Clear();
            IsSessionActive = false;
            _sessionStartTime = 0f;

            OnSessionReset?.Invoke();
        }
    }
}
