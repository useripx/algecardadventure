using UnityEngine;

namespace AlgeCardAdventure.Session
{
    /// <summary>
    /// Service kalkulasi skor tunggal (Single Source of Truth).
    /// Mengimplementasikan formula skor persentase kumulatif AlgeCard Adventure.
    /// </summary>
    public static class ScoreCalculator
    {
        public const int MaxQuestionsPerSession = 10;

        /// <summary>
        /// Menghitung skor kumulatif berbasis persentase jawaban benar.
        /// Formula: Round((Correct / TotalAnswered) * 100).
        /// Jika belum ada soal dijawab, mengembalikan 0 (bebas zero-division).
        /// </summary>
        public static int CalculateScore(int correctAnswers, int questionsAnswered)
        {
            if (questionsAnswered <= 0)
            {
                return 0;
            }

            if (correctAnswers < 0) correctAnswers = 0;
            if (correctAnswers > questionsAnswered) correctAnswers = questionsAnswered;

            float ratio = (float)correctAnswers / questionsAnswered;
            return Mathf.RoundToInt(ratio * 100f);
        }

        /// <summary>
        /// Memeriksa apakah sesi masih dapat menerima pertanyaan baru (< 10 soal).
        /// </summary>
        public static bool CanAcceptMoreQuestions(int questionsAnswered)
        {
            return questionsAnswered < MaxQuestionsPerSession;
        }

        /// <summary>
        /// Menghitung apakah sesi telah mencapai batas maksimal (10 soal).
        /// </summary>
        public static bool IsSessionCompleted(int questionsAnswered)
        {
            return questionsAnswered >= MaxQuestionsPerSession;
        }
    }
}
