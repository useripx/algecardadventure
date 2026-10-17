using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace AlgeCardAdventure.Data
{
    /// <summary>
    /// Implementasi resmi repository bank soal.
    /// Membaca dan memvalidasi JSON banksoal serta mengelola caching in-memory.
    /// </summary>
    public class QuestionRepository : IQuestionRepository
    {
        private BankSoalData _bankData;
        private readonly Dictionary<string, QuestionModel> _questionsById = new Dictionary<string, QuestionModel>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, List<QuestionModel>> _questionsByLevel = new Dictionary<string, List<QuestionModel>>(StringComparer.OrdinalIgnoreCase);
        private readonly List<QuestionModel> _allQuestions = new List<QuestionModel>();

        public bool IsLoaded => _bankData != null && _allQuestions.Count > 0;
        public BankSoalData BankData => _bankData;

        public bool LoadFromJsonString(string jsonContent)
        {
            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                Debug.LogError("[QuestionRepository] Konten JSON kosong atau null.");
                return false;
            }

            try
            {
                _bankData = JsonUtility.FromJson<BankSoalData>(jsonContent);
                if (_bankData == null || _bankData.levels == null || _bankData.levels.Count == 0)
                {
                    Debug.LogError("[QuestionRepository] Gagal melakukan deserialisasi BankSoalData dari JSON.");
                    return false;
                }

                RebuildIndexes();
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[QuestionRepository] Eksepsi saat mem-parse JSON: {ex.Message}");
                return false;
            }
        }

        public bool LoadFromStreamingAssets(string fileName = "banksoal.json")
        {
            string fullPath = Path.Combine(Application.streamingAssetsPath, fileName);

#if UNITY_ANDROID && !UNITY_EDITOR
            using (var request = UnityWebRequest.Get(fullPath))
            {
                request.SendWebRequest();
                while (!request.isDone) { }

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"[QuestionRepository] Gagal membaca {fileName} di Android StreamingAssets: {request.error}");
                    return false;
                }

                return LoadFromJsonString(request.downloadHandler.text);
            }
#else
            if (!File.Exists(fullPath))
            {
                Debug.LogError($"[QuestionRepository] Berkas tidak ditemukan pada path: {fullPath}");
                return false;
            }

            try
            {
                string jsonText = File.ReadAllText(fullPath);
                return LoadFromJsonString(jsonText);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[QuestionRepository] Gagal membaca berkas: {ex.Message}");
                return false;
            }
#endif
        }

        private void RebuildIndexes()
        {
            _questionsById.Clear();
            _questionsByLevel.Clear();
            _allQuestions.Clear();

            foreach (var level in _bankData.levels)
            {
                if (level == null || level.questions == null) continue;

                string uiLevelKey = level.ui_level ?? string.Empty;
                if (!_questionsByLevel.ContainsKey(uiLevelKey))
                {
                    _questionsByLevel[uiLevelKey] = new List<QuestionModel>();
                }

                foreach (var q in level.questions)
                {
                    if (q == null || string.IsNullOrWhiteSpace(q.id)) continue;

                    _allQuestions.Add(q);
                    _questionsById[q.id] = q;
                    _questionsByLevel[uiLevelKey].Add(q);
                }
            }
        }

        public IReadOnlyList<QuestionModel> GetAllQuestions()
        {
            return _allQuestions;
        }

        public IReadOnlyList<QuestionModel> GetQuestionsByLevel(string uiLevel, bool eligibleOnly = true)
        {
            if (string.IsNullOrWhiteSpace(uiLevel) || !_questionsByLevel.TryGetValue(uiLevel, out var list))
            {
                return Array.Empty<QuestionModel>();
            }

            if (!eligibleOnly)
            {
                return list;
            }

            var filtered = new List<QuestionModel>();
            foreach (var q in list)
            {
                if (q.IsEligibleForPlay)
                {
                    filtered.Add(q);
                }
            }
            return filtered;
        }

        public QuestionModel GetQuestionById(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;
            _questionsById.TryGetValue(id, out var question);
            return question;
        }

        public List<QuestionModel> GetRandomMissionBatch(string uiLevel, int count = 5, ICollection<string> excludeIds = null)
        {
            var eligibleQuestions = GetQuestionsByLevel(uiLevel, eligibleOnly: true);
            var candidates = new List<QuestionModel>();

            foreach (var q in eligibleQuestions)
            {
                if (excludeIds == null || !excludeIds.Contains(q.id))
                {
                    candidates.Add(q);
                }
            }

            if (candidates.Count == 0)
            {
                return new List<QuestionModel>();
            }

            // Shuffle kandidat menggunakan algoritma Fisher-Yates
            var rng = new System.Random();
            int n = candidates.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                var temp = candidates[i];
                candidates[i] = candidates[j];
                candidates[j] = temp;
            }

            int takeCount = Mathf.Min(count, candidates.Count);
            return candidates.GetRange(0, takeCount);
        }
    }
}
