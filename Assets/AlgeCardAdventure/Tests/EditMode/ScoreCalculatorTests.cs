using NUnit.Framework;
using AlgeCardAdventure.Session;

namespace AlgeCardAdventure.Tests
{
    [TestFixture]
    public class ScoreCalculatorTests
    {
        [Test]
        public void TC_SCORE_01_ZeroQuestionsAnswered_ReturnsZero()
        {
            int score = ScoreCalculator.CalculateScore(correctAnswers: 0, questionsAnswered: 0);
            Assert.AreEqual(0, score, "Jika belum ada soal dijawab, skor harus 0.");
        }

        [Test]
        public void TC_SCORE_02_OneCorrectOutOfOne_Returns100()
        {
            int score = ScoreCalculator.CalculateScore(correctAnswers: 1, questionsAnswered: 1);
            Assert.AreEqual(100, score, "1 benar dari 1 soal harus menghasilkan skor 100.");
        }

        [Test]
        public void TC_SCORE_03_OneCorrectOutOfTwo_Returns50()
        {
            int score = ScoreCalculator.CalculateScore(correctAnswers: 1, questionsAnswered: 2);
            Assert.AreEqual(50, score, "1 benar dari 2 soal harus menghasilkan skor 50.");
        }

        [Test]
        public void TC_SCORE_04_TwoCorrectOutOfThree_Returns67()
        {
            int score = ScoreCalculator.CalculateScore(correctAnswers: 2, questionsAnswered: 3);
            Assert.AreEqual(67, score, "2 benar dari 3 soal harus dibulatkan ke bilangan bulat 67.");
        }

        [Test]
        public void TC_SCORE_05_FourCorrectOutOfFive_Returns80()
        {
            int score = ScoreCalculator.CalculateScore(correctAnswers: 4, questionsAnswered: 5);
            Assert.AreEqual(80, score, "4 benar dari 5 soal harus menghasilkan skor 80.");
        }

        [Test]
        public void TC_SCORE_06_TenQuestionsCap_EnforcesMaxLimit()
        {
            Assert.IsTrue(ScoreCalculator.CanAcceptMoreQuestions(0));
            Assert.IsTrue(ScoreCalculator.CanAcceptMoreQuestions(9), "Soal ke-10 masih dapat diterima saat dijawab baru 9.");

            Assert.IsFalse(ScoreCalculator.CanAcceptMoreQuestions(10), "Setelah 10 soal dijawab, tidak boleh menerima soal ke-11.");
            Assert.IsTrue(ScoreCalculator.IsSessionCompleted(10), "Sesi harus berstatus selesai pada soal ke-10.");
            Assert.IsTrue(ScoreCalculator.IsSessionCompleted(11));
        }
    }
}
