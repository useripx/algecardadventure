using System.Collections.Generic;
using NUnit.Framework;
using AlgeCardAdventure.Data;

namespace AlgeCardAdventure.Tests
{
    [TestFixture]
    public class QuestionRepositoryTests
    {
        private QuestionRepository _repository;

        [SetUp]
        public void Setup()
        {
            _repository = new QuestionRepository();
            bool loaded = _repository.LoadFromStreamingAssets("banksoal.json");
            Assert.IsTrue(loaded, "Gagal memuat banksoal.json dari StreamingAssets.");
        }

        [Test]
        public void TC_DATA_01_BankSoal_LoadsSuccessfully()
        {
            Assert.IsTrue(_repository.IsLoaded, "Repository harus dalam status ter-load.");
            Assert.IsNotNull(_repository.BankData, "BankData tidak boleh bernilai null.");
            Assert.AreEqual("AlgeCard Adventure Question Bank", _repository.BankData.title);
        }

        [Test]
        public void TC_DATA_02_QuestionCount_TotalIs60_AndLevelsAre20Each()
        {
            var allQuestions = _repository.GetAllQuestions();
            Assert.AreEqual(60, allQuestions.Count, "Total seluruh soal harus tepat 60.");

            var easyAll = _repository.GetQuestionsByLevel("Easy", eligibleOnly: false);
            var normalAll = _repository.GetQuestionsByLevel("Normal", eligibleOnly: false);
            var hardAll = _repository.GetQuestionsByLevel("Hard", eligibleOnly: false);

            Assert.AreEqual(20, easyAll.Count, "Level Easy harus memiliki 20 soal.");
            Assert.AreEqual(20, normalAll.Count, "Level Normal harus memiliki 20 soal.");
            Assert.AreEqual(20, hardAll.Count, "Level Hard harus memiliki 20 soal.");
        }

        [Test]
        public void TC_DATA_03_UniqueQuestionIds_All60AreUnique()
        {
            var allQuestions = _repository.GetAllQuestions();
            var ids = new HashSet<string>();

            foreach (var q in allQuestions)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(q.id), "ID soal tidak boleh kosong.");
                bool added = ids.Add(q.id);
                Assert.IsTrue(added, $"Ditemukan duplikasi ID soal: {q.id}");
            }

            Assert.AreEqual(60, ids.Count, "Harus ada tepat 60 ID unik.");
        }

        [Test]
        public void TC_DATA_04_FlaggedQuestions_ExcludedFromEligiblePool()
        {
            // Verifikasi 4 soal khusus sesuai OPEN-04
            var normal5 = _repository.GetQuestionById("NORMAL-005");
            var normal7 = _repository.GetQuestionById("NORMAL-007");
            var normal20 = _repository.GetQuestionById("NORMAL-020");
            var hard6 = _repository.GetQuestionById("HARD-006");

            Assert.IsNotNull(normal5);
            Assert.IsNotNull(normal7);
            Assert.IsNotNull(normal20);
            Assert.IsNotNull(hard6);

            Assert.IsFalse(normal5.IsEligibleForPlay, "NORMAL-005 tidak boleh eligible untuk mode rilis.");
            Assert.IsFalse(normal7.IsEligibleForPlay, "NORMAL-007 tidak boleh eligible untuk mode rilis.");
            Assert.IsFalse(normal20.IsEligibleForPlay, "NORMAL-020 tidak boleh eligible untuk mode rilis.");
            Assert.IsFalse(hard6.IsEligibleForPlay, "HARD-006 tidak boleh eligible untuk mode rilis.");

            // Verifikasi hitungan pool playable
            var easyEligible = _repository.GetQuestionsByLevel("Easy", eligibleOnly: true);
            var normalEligible = _repository.GetQuestionsByLevel("Normal", eligibleOnly: true);
            var hardEligible = _repository.GetQuestionsByLevel("Hard", eligibleOnly: true);

            Assert.AreEqual(20, easyEligible.Count, "Easy eligible harus tetap 20.");
            Assert.AreEqual(17, normalEligible.Count, "Normal eligible harus 17 (20 - 3 flagged).");
            Assert.AreEqual(19, hardEligible.Count, "Hard eligible harus 19 (20 - 1 flagged).");
        }

        [Test]
        public void TC_DATA_05_TrueFalse_NullHandledSafely()
        {
            var allQuestions = _repository.GetAllQuestions();
            foreach (var q in allQuestions)
            {
                // Tidak boleh melempar NullReferenceException
                bool ready = q.HasTrueFalseReady;
                Assert.IsFalse(ready, $"Soal {q.id} belum memiliki konten True/False yang disetujui, HasTrueFalseReady harus false.");
            }
        }

        [Test]
        public void TC_DATA_06_RandomMissionBatch_Generates5UniqueCards_ExcludingCompleted()
        {
            var batch = _repository.GetRandomMissionBatch("Easy", count: 5);
            Assert.AreEqual(5, batch.Count, "Batch harus mengembalikan tepat 5 kartu.");

            var batchIds = new HashSet<string>();
            foreach (var q in batch)
            {
                Assert.IsTrue(batchIds.Add(q.id), $"Batch mengandung duplikasi kartu: {q.id}");
            }

            // Uji pengecualian kartu yang telah selesai (anti-duplikasi)
            var excluded = new HashSet<string> { batch[0].id, batch[1].id };
            var nextBatch = _repository.GetRandomMissionBatch("Easy", count: 5, excludeIds: excluded);

            foreach (var q in nextBatch)
            {
                Assert.IsFalse(excluded.Contains(q.id), $"Kartu yang dikecualikan {q.id} muncul kembali di batch berikutnya.");
            }
        }
    }
}
