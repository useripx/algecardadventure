using UnityEngine;
using UnityEngine.UI;

namespace AlgeCardAdventure.UI
{
    /// <summary>
    /// Memberikan efek bernapas / berkedip lembut (pulsing alpha) pada teks instruksi UI.
    /// Ringan dan tidak memerlukan animator controller eksternal.
    /// </summary>
    public class PulsingTextEffect : MonoBehaviour
    {
        [Tooltip("Kecepatan kedip/denyut")]
        [SerializeField] private float speed = 2.5f;

        [Tooltip("Alpha minimal")]
        [SerializeField] private float minAlpha = 0.25f;

        [Tooltip("Alpha maksimal")]
        [SerializeField] private float maxAlpha = 1.0f;

        private CanvasGroup _canvasGroup;
        private Graphic _graphic;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                _graphic = GetComponent<Graphic>();
            }
        }

        private void Update()
        {
            float t = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f;
            float currentAlpha = Mathf.Lerp(minAlpha, maxAlpha, t);

            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = currentAlpha;
            }
            else if (_graphic != null)
            {
                Color c = _graphic.color;
                c.a = currentAlpha;
                _graphic.color = c;
            }
        }
    }
}
