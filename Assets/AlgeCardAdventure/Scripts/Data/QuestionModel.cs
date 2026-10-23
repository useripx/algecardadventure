using System;

namespace AlgeCardAdventure.Data
{
    /// <summary>
    /// Model data untuk konten evaluasi True/False dan pembahasannya.
    /// </summary>
    [Serializable]
    public class TrueFalseData
    {
        public string statement;
        public bool correct_answer;
        public string explanation;
        public string content_origin;

        /// <summary>
        /// Menandakan apakah konten True/False telah tersedia dan valid secara matematis.
        /// Mengikuti OPEN-03, nilai statement null menandakan konten belum tersedia (bukan jawaban False).
        /// </summary>
        public bool HasContent => !string.IsNullOrWhiteSpace(statement);
    }

    /// <summary>
    /// Model data tunggal entitas soal AlgeCard Adventure.
    /// </summary>
    [Serializable]
    public class QuestionModel
    {
        public string id;
        public int source_number;
        public string source_level;
        public string ui_level;
        public string title;
        public string question_text;
        public string[] equation_model;
        public string[] solution_steps;
        public string final_answer;
        public string answer_summary;
        public string review_status;
        public string review_note;
        public string method;
        public string elimination_op;
        public TrueFalseData true_false;

        /// <summary>
        /// Sesuai OPEN-04, hanya soal dengan review_status 'transcribed' yang eligible untuk mode rilis.
        /// Soal 'needs_clarification' dan 'needs_source_check' otomatis diabaikan sampai disetujui Yogi.
        /// </summary>
        public bool IsEligibleForPlay => string.Equals(review_status, "transcribed", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Memeriksa apakah soal ini memiliki evaluasi True/False yang siap ditampilkan di Screen 05.
        /// </summary>
        public bool HasTrueFalseReady => true_false != null && true_false.HasContent;
    }
}
