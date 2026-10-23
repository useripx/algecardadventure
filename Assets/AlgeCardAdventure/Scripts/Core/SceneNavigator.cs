using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AlgeCardAdventure.Core
{
    /// <summary>
    /// Service navigasi terpusat untuk transisi antar-layar dalam AlgeCard Adventure.
    /// Mendukung transisi modular untuk seluruh 6 layar utama dan Credits scene.
    /// </summary>
    public static class SceneNavigator
    {
        public const string SceneSplashScreen = "01_SplashScreen";
        public const string SceneLevel = "02_Level";
        public const string SceneMissionCard = "03_MissionCard";
        public const string SceneAlgebraCardBoard = "04_AlgebraCardBoard";
        public const string SceneTrueOrFalse = "05_TrueOrFalse";
        public const string SceneScoreResults = "06_ScoreResults";
        public const string SceneCredits = "CreditsScene";

        public static event Action<string> OnBeforeSceneLoad;

        /// <summary>
        /// Memuat scene berdasarkan nama. Jika scene belum terdaftar di Build Settings,
        /// akan memberikan pesan log yang jelas dan ramah pengembang tanpa crash.
        /// </summary>
        public static void LoadScene(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                try { Debug.LogError("[SceneNavigator] Nama scene tujuan tidak boleh kosong."); } catch { }
                return;
            }

            try { Debug.Log($"[SceneNavigator] Berpindah menuju scene: {sceneName}"); } catch { }
            OnBeforeSceneLoad?.Invoke(sceneName);

            try
            {
                if (Application.CanStreamedLevelBeLoaded(sceneName))
                {
                    SceneManager.LoadScene(sceneName);
                }
                else
                {
                    Debug.LogWarning($"[SceneNavigator] Scene '{sceneName}' belum didaftarkan di Build Settings. Mengabaikan eksekusi runtime.");
                }
            }
            catch
            {
                // Defensif untuk lingkungan non-Unity runtime/testing
            }
        }

        public static void GoToLevelSelection()
        {
            LoadScene(SceneLevel);
        }

        public static void GoToSplashScreen()
        {
            LoadScene(SceneSplashScreen);
        }

        public static void GoToMissionCard()
        {
            LoadScene(SceneMissionCard);
        }

        public static void GoToAlgebraCardBoard()
        {
            LoadScene(SceneAlgebraCardBoard);
        }

        public static void GoToTrueOrFalse()
        {
            LoadScene(SceneTrueOrFalse);
        }

        public static void GoToScoreResults()
        {
            LoadScene(SceneScoreResults);
        }

        public static void GoToCredits()
        {
            LoadScene(SceneCredits);
        }

        // Aliases untuk konsistensi API navigasi
        public static void LoadSplashScreen() => GoToSplashScreen();
        public static void LoadLevelSelection() => GoToLevelSelection();
        public static void LoadMissionCard() => GoToMissionCard();
        public static void LoadAlgebraCardBoard() => GoToAlgebraCardBoard();
        public static void LoadTrueOrFalse() => GoToTrueOrFalse();
        public static void LoadScoreResults() => GoToScoreResults();
        public static void LoadCredits() => GoToCredits();
    }
}
