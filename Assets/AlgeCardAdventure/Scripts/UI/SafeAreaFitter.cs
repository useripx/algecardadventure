using UnityEngine;

namespace AlgeCardAdventure.UI
{
    /// <summary>
    /// Menyesuaikan RectTransform penampung UI dengan Screen.safeArea pada perangkat seluler.
    /// Mencegah elemen UI terpotong poni (notch), punch-hole kamera, atau bilah navigasi Android.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [ExecuteAlways]
    public class SafeAreaFitter : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Rect _lastSafeArea = Rect.zero;
        private Vector2Int _lastScreenSize = Vector2Int.zero;
        private ScreenOrientation _lastOrientation = ScreenOrientation.AutoRotation;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            Refresh();
        }

        private void Update()
        {
            Refresh();
        }

        public void Refresh()
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }

            Rect currentSafeArea = Screen.safeArea;
            Vector2Int currentScreenSize = new Vector2Int(Screen.width, Screen.height);
            ScreenOrientation currentOrientation = Screen.orientation;

            if (currentSafeArea != _lastSafeArea || currentScreenSize != _lastScreenSize || currentOrientation != _lastOrientation)
            {
                _lastSafeArea = currentSafeArea;
                _lastScreenSize = currentScreenSize;
                _lastOrientation = currentOrientation;

                ApplySafeArea(currentSafeArea, new Vector2(currentScreenSize.x, currentScreenSize.y));
            }
        }

        /// <summary>
        /// Menerapkan safe area ke anchorMin dan anchorMax.
        /// Metode ini bersifat publik untuk memungkinkan pengujian unit (EditMode Unit Tests).
        /// </summary>
        public void ApplySafeArea(Rect safeArea, Vector2 screenSize)
        {
            if (_rectTransform == null || screenSize.x <= 0f || screenSize.y <= 0f) return;

            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= screenSize.x;
            anchorMin.y /= screenSize.y;
            anchorMax.x /= screenSize.x;
            anchorMax.y /= screenSize.y;

            // Clamp nilai antara 0 dan 1
            anchorMin.x = Mathf.Clamp01(anchorMin.x);
            anchorMin.y = Mathf.Clamp01(anchorMin.y);
            anchorMax.x = Mathf.Clamp01(anchorMax.x);
            anchorMax.y = Mathf.Clamp01(anchorMax.y);

            _rectTransform.anchorMin = anchorMin;
            _rectTransform.anchorMax = anchorMax;
            _rectTransform.offsetMin = Vector2.zero;
            _rectTransform.offsetMax = Vector2.zero;
        }

        /// <summary>
        /// Menghitung vektor anchor min dan max secara statis untuk pengujian matematis.
        /// </summary>
        public static (Vector2 anchorMin, Vector2 anchorMax) CalculateAnchors(Rect safeArea, Vector2 screenSize)
        {
            if (screenSize.x <= 0f || screenSize.y <= 0f)
            {
                return (Vector2.zero, Vector2.one);
            }

            Vector2 min = new Vector2(
                Mathf.Clamp01(safeArea.x / screenSize.x),
                Mathf.Clamp01(safeArea.y / screenSize.y)
            );

            Vector2 max = new Vector2(
                Mathf.Clamp01((safeArea.x + safeArea.width) / screenSize.x),
                Mathf.Clamp01((safeArea.y + safeArea.height) / screenSize.y)
            );

            return (min, max);
        }
    }
}
