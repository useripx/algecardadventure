using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using AlgeCardAdventure.Audio;

namespace AlgeCardAdventure.UI
{
    /// <summary>
    /// Komponen pemicu universal: Menekan dan menahan logo selama 3 detik
    /// (Long-Press >= 3s) untuk membuka Math Companion Chatbot (Prof. Hopper & Pip).
    /// Ketukan biasa (< 3s) tidak memicu chatbot sehingga game tetap berpenampilan murni offline.
    /// </summary>
    public class LogoLongPressTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [Header("Konfigurasi Tahanan")]
        [Tooltip("Durasi tahanan dalam detik yang diperlukan untuk memicu Chatbot")]
        [SerializeField] private float holdDurationThreshold = 3.0f;

        [Tooltip("Skala pembesaran lembut visual saat logo ditahan")]
        [SerializeField] private float holdScaleMultiplier = 1.05f;

        [Header("Event Pemicu")]
        [SerializeField] private UnityEvent onLongPressTriggered = new UnityEvent();

        public static event Action OnUniversalChatbotTriggered;

        private bool _isHolding = false;
        private float _holdTimer = 0f;
        private Vector3 _originalScale = Vector3.one;

        public bool IsHolding => _isHolding;
        public float CurrentHoldTime => _holdTimer;
        public float HoldThreshold => holdDurationThreshold;

        private void Awake()
        {
            _originalScale = transform.localScale;
            if (_originalScale == Vector3.zero)
            {
                _originalScale = Vector3.one;
            }
        }

        private void OnDisable()
        {
            ResetTriggerState();
        }

        private void Update()
        {
            if (!_isHolding) return;

            _holdTimer += Time.unscaledDeltaTime;

            // Animasi mikro membesar lembut saat ditahan
            float progress = Mathf.Clamp01(_holdTimer / holdDurationThreshold);
            transform.localScale = Vector3.Lerp(_originalScale, _originalScale * holdScaleMultiplier, progress);

            if (_holdTimer >= holdDurationThreshold)
            {
                TriggerChatbot();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isHolding = true;
            _holdTimer = 0f;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ResetTriggerState();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ResetTriggerState();
        }

        private void TriggerChatbot()
        {
            ResetTriggerState();

            // Mainkan suara chime/klik
            AudioManager.Instance?.PlayButtonClick();

            Debug.Log("[LogoLongPressTrigger] Logo ditahan selama 3 detik! Membuka Math Companion Chatbot...");

            // Invokasi event lokal & universal
            onLongPressTriggered?.Invoke();
            OnUniversalChatbotTriggered?.Invoke();
        }

        public void ResetTriggerState()
        {
            _isHolding = false;
            _holdTimer = 0f;
            transform.localScale = _originalScale;
        }

        /// <summary>
        /// Pengujian unit testing bantuan (simulasi).
        /// </summary>
        public void SimulateTick(float deltaTime)
        {
            if (!_isHolding) return;
            _holdTimer += deltaTime;
            if (_holdTimer >= holdDurationThreshold)
            {
                TriggerChatbot();
            }
        }
    }
}
