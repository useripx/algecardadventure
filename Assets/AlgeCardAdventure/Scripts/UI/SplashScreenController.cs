using UnityEngine;
using UnityEngine.UI;
using AlgeCardAdventure.Core;

namespace AlgeCardAdventure.UI
{
    /// <summary>
    /// Mengontrol interaksi pada Screen 01 — Splash Screen.
    /// Mendeteksi ketukan di sembarang area layar untuk memulai petualangan.
    /// </summary>
    public class SplashScreenController : MonoBehaviour
    {
        [Header("Komponen UI")]
        [Tooltip("Tombol transparan satu layar penuh untuk mendeteksi ketukan")]
        [SerializeField] private Button tapDetectorButton;

        [Tooltip("Penampung teks instruksi yang akan disembunyikan saat layar diketuk")]
        [SerializeField] private GameObject promptContainer;

        private bool _hasTriggered = false;

        private void Awake()
        {
            if (tapDetectorButton != null)
            {
                tapDetectorButton.onClick.AddListener(OnScreenTapped);
            }
        }

        private void Update()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            // Mendukung klik mouse atau sentuhan sembarang di Editor (jika legacy input aktif)
            if (!_hasTriggered && (Input.GetMouseButtonDown(0) || Input.anyKeyDown))
            {
                OnScreenTapped();
            }
#endif
        }

        public void OnScreenTapped()
        {
            if (_hasTriggered) return;
            _hasTriggered = true;

            Debug.Log("[SplashScreen] Layar diketuk! Memulai petualangan AlgeCard Adventure...");

            // Mainkan audio ketukan layar
            AlgeCardAdventure.Audio.AudioManager.Instance?.PlayButtonClick();

            if (promptContainer != null)
            {
                promptContainer.SetActive(false);
            }

            // Pindah ke Level Selection (Screen 02)
            SceneNavigator.GoToLevelSelection();
        }
    }
}
