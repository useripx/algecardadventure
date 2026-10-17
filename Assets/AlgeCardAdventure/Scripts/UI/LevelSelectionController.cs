using UnityEngine;
using UnityEngine.UI;
using AlgeCardAdventure.Session;
using AlgeCardAdventure.Audio;
using AlgeCardAdventure.Core;

namespace AlgeCardAdventure.UI
{
    /// <summary>
    /// Mengontrol interaksi pada Screen 02 — Level Selection.
    /// Menangani pemilihan tingkat kesulitan (Easy, Normal, Hard) dan tombol navigasi kembali.
    /// </summary>
    public class LevelSelectionController : MonoBehaviour
    {
        [Header("Tombol Pemilihan Level")]
        [SerializeField] private Button easyButton;
        [SerializeField] private Button normalButton;
        [SerializeField] private Button hardButton;

        [Header("Navigasi")]
        [SerializeField] private Button backButton;

        private void Awake()
        {
            if (easyButton != null)
            {
                easyButton.onClick.AddListener(() => OnLevelSelected("Easy"));
            }
            else
            {
                Debug.LogWarning("[LevelSelectionController] easyButton belum terpasang di Inspector!");
            }

            if (normalButton != null)
            {
                normalButton.onClick.AddListener(() => OnLevelSelected("Normal"));
            }
            else
            {
                Debug.LogWarning("[LevelSelectionController] normalButton belum terpasang di Inspector!");
            }

            if (hardButton != null)
            {
                hardButton.onClick.AddListener(() => OnLevelSelected("Hard"));
            }
            else
            {
                Debug.LogWarning("[LevelSelectionController] hardButton belum terpasang di Inspector!");
            }

            if (backButton != null)
            {
                backButton.onClick.AddListener(OnBackClicked);
            }
            else
            {
                Debug.LogWarning("[LevelSelectionController] backButton belum terpasang di Inspector!");
            }
        }

        public void OnLevelSelected(string level)
        {
            Debug.Log($"[LevelSelection] Pemain memilih tingkat kesulitan: {level}");

            // Mainkan suara klik tombol
            AudioManager.Instance?.PlayButtonClick();

            // Simpan level ke sesi game
            GameSession.Instance?.StartNewSession(level);

            // Berpindah ke Screen 03 (Mission Cards)
            SceneNavigator.GoToMissionCard();
        }

        public void OnBackClicked()
        {
            Debug.Log("[LevelSelection] Tombol Back diklik, kembali ke Splash Screen.");

            AudioManager.Instance?.PlayButtonClick();

            // Kembali ke Screen 01 (Splash Screen)
            SceneNavigator.GoToSplashScreen();
        }
    }
}
