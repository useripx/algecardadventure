using NUnit.Framework;
using UnityEngine;
using AlgeCardAdventure.Session;

namespace AlgeCardAdventure.Tests
{
    [TestFixture]
    public class GameSessionTests
    {
        private GameObject _sessionGo;
        private GameSession _session;

        [SetUp]
        public void Setup()
        {
            _sessionGo = new GameObject("TestSession");
            _session = _sessionGo.AddComponent<GameSession>();
        }

        [TearDown]
        public void Teardown()
        {
            if (_sessionGo != null)
            {
                Object.DestroyImmediate(_sessionGo);
            }
        }

        [Test]
        public void TC_SESSION_01_StartNewSession_ValidLevels_SetsCurrentLevelCorrectly()
        {
            _session.StartNewSession("Easy");
            Assert.AreEqual("Easy", _session.CurrentLevel);
            Assert.IsTrue(_session.IsSessionActive);

            _session.StartNewSession("Normal");
            Assert.AreEqual("Normal", _session.CurrentLevel);

            _session.StartNewSession("Hard");
            Assert.AreEqual("Hard", _session.CurrentLevel);
        }

        [Test]
        public void TC_SESSION_02_StartNewSession_Medium_MapsToNormal_DEC04()
        {
            _session.StartNewSession("Medium");
            Assert.AreEqual("Normal", _session.CurrentLevel);
        }

        [Test]
        public void TC_SESSION_03_StartNewSession_InvalidOrNullLevel_FallsBackToEasy()
        {
            _session.StartNewSession(null);
            Assert.AreEqual("Easy", _session.CurrentLevel);

            _session.StartNewSession("InvalidLevel123");
            Assert.AreEqual("Easy", _session.CurrentLevel);
        }

        [Test]
        public void TC_SESSION_04_RecordAnswer_UpdatesTotalsAndScore()
        {
            _session.StartNewSession("Easy");

            _session.RecordAnswer(true);
            Assert.AreEqual(1, _session.TotalAnswered);
            Assert.AreEqual(1, _session.CorrectCount);
            Assert.AreEqual(100, _session.GetCurrentScore());

            _session.RecordAnswer(false);
            Assert.AreEqual(2, _session.TotalAnswered);
            Assert.AreEqual(1, _session.CorrectCount);
            Assert.AreEqual(50, _session.GetCurrentScore());
        }

        [Test]
        public void TC_SESSION_05_MaxQuestionsLimit_ReturnsTrueAt10Questions_DEC08()
        {
            _session.StartNewSession("Normal");

            for (int i = 0; i < 9; i++)
            {
                _session.RecordAnswer(true);
                Assert.IsFalse(_session.HasReachedLimit());
            }

            _session.RecordAnswer(true);
            Assert.AreEqual(10, _session.TotalAnswered);
            Assert.IsTrue(_session.HasReachedLimit());
        }

        [Test]
        public void TC_SESSION_06_ResetSession_ClearsActiveStateAndCounters()
        {
            _session.StartNewSession("Hard");
            _session.RecordAnswer(true);
            _session.RecordAnswer(true);

            _session.ResetSession();

            Assert.AreEqual(0, _session.TotalAnswered);
            Assert.AreEqual(0, _session.CorrectCount);
            Assert.IsFalse(_session.IsSessionActive);
            Assert.IsNull(_session.CurrentQuestion);
        }
    }
}
