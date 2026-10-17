using NUnit.Framework;
using UnityEngine;
using AlgeCardAdventure.UI;
using AlgeCardAdventure.Core;

namespace AlgeCardAdventure.Tests
{
    [TestFixture]
    public class SafeAreaAndNavigationTests
    {
        [Test]
        public void TC_UI_01_SafeArea_StandardScreenNoNotch_AnchorsAreZeroToOne()
        {
            Rect fullSafe = new Rect(0, 0, 1080, 1920);
            Vector2 screenSize = new Vector2(1080, 1920);

            var (min, max) = SafeAreaFitter.CalculateAnchors(fullSafe, screenSize);

            Assert.AreEqual(0f, min.x, 0.001f);
            Assert.AreEqual(0f, min.y, 0.001f);
            Assert.AreEqual(1f, max.x, 0.001f);
            Assert.AreEqual(1f, max.y, 0.001f);
        }

        [Test]
        public void TC_UI_02_SafeArea_WithNotchAndNav_CalculatesNormalizedAnchors()
        {
            // Simulasi layar HP Android dengan notch atas 80px dan nav bar bawah 60px
            Rect notchedSafe = new Rect(0, 60, 1080, 1780);
            Vector2 screenSize = new Vector2(1080, 1920);

            var (min, max) = SafeAreaFitter.CalculateAnchors(notchedSafe, screenSize);

            Assert.AreEqual(0f, min.x, 0.001f);
            Assert.AreEqual(60f / 1920f, min.y, 0.001f);
            Assert.AreEqual(1f, max.x, 0.001f);
            Assert.AreEqual((60f + 1780f) / 1920f, max.y, 0.001f);
        }

        [Test]
        public void TC_UI_03_SafeArea_ZeroScreenSize_FallbackSafe()
        {
            Rect safe = new Rect(0, 0, 1080, 1920);
            Vector2 zeroScreen = Vector2.zero;

            var (min, max) = SafeAreaFitter.CalculateAnchors(safe, zeroScreen);

            Assert.AreEqual(Vector2.zero, min);
            Assert.AreEqual(Vector2.one, max);
        }

        [Test]
        public void TC_NAV_01_SceneConstants_AreCorrectlyDefined()
        {
            Assert.AreEqual("01_SplashScreen", SceneNavigator.SceneSplashScreen);
            Assert.AreEqual("02_Level", SceneNavigator.SceneLevel);
            Assert.AreEqual("03_MissionCard", SceneNavigator.SceneMissionCard);
            Assert.AreEqual("04_AlgebraCardBoard", SceneNavigator.SceneAlgebraCardBoard);
            Assert.AreEqual("05_TrueOrFalse", SceneNavigator.SceneTrueOrFalse);
            Assert.AreEqual("06_ScoreResults", SceneNavigator.SceneScoreResults);
            Assert.AreEqual("CreditsScene", SceneNavigator.SceneCredits);
        }

        [Test]
        public void TC_NAV_02_OnBeforeSceneLoad_EventFiresWithTargetScene()
        {
            string targetReceived = null;
            void Handler(string s) => targetReceived = s;

            SceneNavigator.OnBeforeSceneLoad += Handler;
            try
            {
                SceneNavigator.LoadScene(SceneNavigator.SceneLevel);
                Assert.AreEqual("02_Level", targetReceived);
            }
            finally
            {
                SceneNavigator.OnBeforeSceneLoad -= Handler;
            }
        }
    }
}
