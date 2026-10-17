using System.Collections.Generic;

namespace AlgeCardAdventure.Data
{
    /// <summary>
    /// Kontrak akses data bank soal AlgeCard Adventure.
    /// Memisahkan logika UI/Gameplay dari format serialisasi JSON.
    /// </summary>
    public interface IQuestionRepository
    {
        bool IsLoaded { get; }
        BankSoalData BankData { get; }

        /// <summary>
        /// Memuat data langsung dari string JSON (memudahkan isolasi Unit Test).
        /// </summary>
        bool LoadFromJsonString(string jsonContent);

        /// <summary>
        /// Memuat data dari berkas StreamingAssets.
        /// </summary>
        bool LoadFromStreamingAssets(string fileName = "banksoal.json");

        /// <summary>
        /// Mengambil seluruh soal dari seluruh level.
        /// </summary>
        IReadOnlyList<QuestionModel> GetAllQuestions();

        /// <summary>
        /// Mengambil seluruh soal berdasarkan level UI ("Easy", "Normal", "Hard").
        /// Jika eligibleOnly = true, hanya mengembalikan soal dengan review_status 'transcribed' (OPEN-04).
        /// </summary>
        IReadOnlyList<QuestionModel> GetQuestionsByLevel(string uiLevel, bool eligibleOnly = true);

        /// <summary>
        /// Mencari soal spesifik berdasarkan ID unik (misal "EASY-001").
        /// </summary>
        QuestionModel GetQuestionById(string id);

        /// <summary>
        /// Mengambil batch acak kartu misi (maksimal count, default 5 kartu)
        /// dengan mengecualikan soal yang sudah pernah diselesaikan dalam sesi aktif (anti-duplikasi).
        /// </summary>
        List<QuestionModel> GetRandomMissionBatch(string uiLevel, int count = 5, ICollection<string> excludeIds = null);
    }
}
