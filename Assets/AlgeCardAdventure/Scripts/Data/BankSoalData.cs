using System;
using System.Collections.Generic;

namespace AlgeCardAdventure.Data
{
    /// <summary>
    /// Kategori level soal dan daftar pertanyaan di dalamnya.
    /// </summary>
    [Serializable]
    public class LevelData
    {
        public string source_level;
        public string ui_level;
        public List<QuestionModel> questions = new List<QuestionModel>();
    }

    /// <summary>
    /// Model konfigurasi game yang tersimpan dalam banksoal.json.
    /// </summary>
    [Serializable]
    public class GameConfiguration
    {
        public int question_count_total;
        public string true_false_content_policy;
        public string random_selection_policy;
    }

    /// <summary>
    /// Root model yang merepresentasikan struktur keseluruhan berkas banksoal.json.
    /// </summary>
    [Serializable]
    public class BankSoalData
    {
        public string schema_version;
        public string title;
        public string language;
        public GameConfiguration game_configuration;
        public List<LevelData> levels = new List<LevelData>();
    }
}
