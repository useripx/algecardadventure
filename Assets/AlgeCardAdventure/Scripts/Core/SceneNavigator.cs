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
                Debug.LogError("[SceneNavigator] Nama scene tujuan tidak boleh kosong.");
                return;
            }

            Debug.Log($"[SceneNavigator] Berpindah menuju scene: {sceneName}");
            OnBeforeSceneLoad?.Invoke(sceneName);

            if (Application.CanStreamedLevelBeLoaded(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogWarning($"[SceneNavigator] Scene '{sceneName}' belum didaftarkan di Build Settings. Mengabaikan eksekusi runtime.");
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

        public static void GoToCredits()
        {
            LoadScene(SceneCredits);
        }
    }
}
