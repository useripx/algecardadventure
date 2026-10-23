using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlgeCardAdventure.AI;
using AlgeCardAdventure.Audio;

namespace AlgeCardAdventure.UI
{
    /// <summary>
    /// Kontroler antarmuka Chatbot "Math Companion (Prof. Hopper & Pip)".
    /// Sepenuhnya selaras dengan acuan prototipe Docs/Ui Template/chatbot/code.html:
    /// - Canvas Overlay (SortingOrder 100).
    /// - Header Kayu 3D, tombol Back, dan info status Prof. Hopper & Pip (Online).
    /// - ScrollRect percakapan dinamis (balon perkamen untuk bot, balon biru glossy untuk pemain).
    /// - Bilah rekomendasi pertanyaan cepat (Quick Prompt Chips).
    /// - Bilah input perkamen dengan tombol kirim 3D hijau.
    /// - Terhubung ke Groq Cloud LLM API dan graceful offline fallback.
    /// </summary>
    public class ChatbotUIController : MonoBehaviour
    {
        public static ChatbotUIController Instance { get; private set; }

        [Header("Root & Navigasi")]
        [SerializeField] private GameObject chatbotModalRoot;
        [SerializeField] private Button closeBackButton;
        [SerializeField] private Button soundToggleButton;

        [Header("Area Percakapan (ScrollRect)")]
        [SerializeField] private ScrollRect chatScrollRect;
        [SerializeField] private RectTransform chatContentContainer;
        [SerializeField] private GameObject botBubblePrefab;
        [SerializeField] private GameObject userBubblePrefab;
        [SerializeField] private GameObject systemAnnouncementObject;

        [Header("Bilah Rekomendasi Pertanyaan (Quick Chips)")]
        [SerializeField] private Button chipIsolateVariable;
        [SerializeField] private Button chipWhatIsVariable;
        [SerializeField] private Button chipMissionHint;

        [Header("Bilah Input & Tombol Kirim")]
        [SerializeField] private InputField chatInputField;
        [SerializeField] private Button sendButton;
        [SerializeField] private GameObject typingIndicator;

        private ChatSessionManager _sessionManager;
        private bool _isAwaitingResponse = false;
        private bool _isModalOpenExplicit = false;

        public bool IsChatOpen
        {
            get
            {
                if (_isModalOpenExplicit) return true;
                try
                {
                    return ((object)chatbotModalRoot != null) && chatbotModalRoot.activeSelf;
                }
                catch
                {
                    return false;
                }
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                try { DontDestroyOnLoad(gameObject); } catch { }
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            if (_sessionManager == null)
            {
                _sessionManager = new ChatSessionManager();
            }

            EnsureProceduralUI();

            // Sembunyikan modal di awal secara default
            try
            {
                if ((object)chatbotModalRoot != null)
                {
                    chatbotModalRoot.SetActive(false);
                }
            }
            catch { }

            if (typingIndicator != null)
            {
                typingIndicator.SetActive(false);
            }

            SetupListeners();
        }

        private void OnEnable()
        {
            LogoLongPressTrigger.OnUniversalChatbotTriggered += OpenChatbot;
        }

        private void OnDisable()
        {
            LogoLongPressTrigger.OnUniversalChatbotTriggered -= OpenChatbot;
        }

        private void SetupListeners()
        {
            if (closeBackButton != null)
            {
                closeBackButton.onClick.RemoveAllListeners();
                closeBackButton.onClick.AddListener(CloseChatbot);
            }

            if (sendButton != null)
            {
                sendButton.onClick.RemoveAllListeners();
                sendButton.onClick.AddListener(OnSendClicked);
            }

            if (chatInputField != null)
            {
                chatInputField.onEndEdit.RemoveAllListeners();
                chatInputField.onEndEdit.AddListener(OnInputEndEdit);
            }

            // Quick Chips listeners
            if (chipIsolateVariable != null)
            {
                chipIsolateVariable.onClick.RemoveAllListeners();
                chipIsolateVariable.onClick.AddListener(() => SubmitQuery("How do I isolate the variable in an equation?"));
            }

            if (chipWhatIsVariable != null)
            {
                chipWhatIsVariable.onClick.RemoveAllListeners();
                chipWhatIsVariable.onClick.AddListener(() => SubmitQuery("What is a variable and a coefficient?"));
            }

            if (chipMissionHint != null)
            {
                chipMissionHint.onClick.RemoveAllListeners();
                chipMissionHint.onClick.AddListener(() => SubmitQuery("Can you give me a helpful hint for solving linear equations?"));
            }
        }

        public void OpenChatbot()
        {
            _isModalOpenExplicit = true;

            try
            {
                if ((object)chatbotModalRoot == null)
                {
                    EnsureProceduralUI();
                }

                if ((object)chatbotModalRoot != null)
                {
                    chatbotModalRoot.SetActive(true);
                    SafePlayButtonClick();

                    // Pastikan GroqApiClient aktif
                    EnsureApiClientExists();

                    // Bersihkan input field dan fokus
                    if ((object)chatInputField != null)
                    {
                        chatInputField.text = "";
                    }

                    ScrollToBottom();
                }
            }
            catch
            {
                // Defensif untuk lingkungan non-engine CLI test runner
            }
        }

        public void CloseChatbot()
        {
            _isModalOpenExplicit = false;

            try
            {
                if ((object)chatbotModalRoot != null)
                {
                    SafePlayButtonClick();
                    chatbotModalRoot.SetActive(false);
                }
            }
            catch
            {
                // Defensif untuk lingkungan non-engine CLI test runner
            }
        }

        private void SafePlayButtonClick()
        {
            try
            {
                AudioManager.Instance?.PlayButtonClick();
            }
            catch
            {
                // Defensif untuk lingkungan non-engine CLI test runner
            }
        }

        private void OnInputEndEdit(string text)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                OnSendClicked();
            }
        }

        public void OnSendClicked()
        {
            if (_isAwaitingResponse) return;

            string text = chatInputField != null ? chatInputField.text : "";
            if (string.IsNullOrWhiteSpace(text)) return;

            if (chatInputField != null)
            {
                chatInputField.text = "";
            }

            SubmitQuery(text);
        }

        public void SubmitQuery(string query)
        {
            if (_isAwaitingResponse || string.IsNullOrWhiteSpace(query)) return;

            _isAwaitingResponse = true;
            SafePlayButtonClick();

            // 1. Tampilkan Balon Pesan Pengguna di UI
            SpawnUserBubble(query);

            // 2. Tambahkan ke Memori Percakapan Sesi
            _sessionManager.AddUserMessage(query);

            // 3. Tampilkan Indikator Mengetik
            if (typingIndicator != null)
            {
                typingIndicator.SetActive(true);
            }
            ScrollToBottom();

            // 4. Periksa Koneksi Internet (Aturan 3 Graceful Offline Fallback)
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                StartCoroutine(DelayedOfflineResponseRoutine(query));
                return;
            }

            // 5. Kirim ke Groq Cloud API
            EnsureApiClientExists();

            var messageList = new List<ChatMessage>(_sessionManager.Messages);
            GroqApiClient.Instance.SendChatCompletion(
                messageList,
                onSuccess: (reply) =>
                {
                    _isAwaitingResponse = false;
                    if (typingIndicator != null) typingIndicator.SetActive(false);

                    _sessionManager.AddAssistantMessage(reply);
                    SpawnBotBubble(reply);
                    AudioManager.Instance?.PlayCorrect();
                    ScrollToBottom();
                },
                onError: (error) =>
                {
                    _isAwaitingResponse = false;
                    if (typingIndicator != null) typingIndicator.SetActive(false);

                    Debug.LogWarning($"[ChatbotUIController] Fallback ke respons lokal: {error}");
                    string fallbackReply = _sessionManager.GetOfflineFallbackResponse(query);
                    _sessionManager.AddAssistantMessage(fallbackReply);
                    SpawnBotBubble(fallbackReply);
                    ScrollToBottom();
                }
            );
        }

        private IEnumerator DelayedOfflineResponseRoutine(string query)
        {
            yield return new WaitForSeconds(0.6f); // Memberi jeda alami ~600ms

            _isAwaitingResponse = false;
            if (typingIndicator != null) typingIndicator.SetActive(false);

            string offlineReply = _sessionManager.GetOfflineFallbackResponse(query);
            _sessionManager.AddAssistantMessage(offlineReply);
            SpawnBotBubble(offlineReply);
            ScrollToBottom();
        }

        private void SpawnUserBubble(string text)
        {
            if (chatContentContainer == null) return;

            if (userBubblePrefab != null)
            {
                GameObject bubbleGo = Instantiate(userBubblePrefab, chatContentContainer);
                Text label = bubbleGo.GetComponentInChildren<Text>();
                if (label != null) label.text = text;
            }
            else
            {
                CreateTextBubble("YOU", text, new Color(0.05f, 0.52f, 0.78f, 1f), TextAnchor.MiddleRight);
            }
        }

        private void SpawnBotBubble(string text)
        {
            if (chatContentContainer == null) return;

            if (botBubblePrefab != null)
            {
                GameObject bubbleGo = Instantiate(botBubblePrefab, chatContentContainer);
                Text label = bubbleGo.GetComponentInChildren<Text>();
                if (label != null) label.text = text;
            }
            else
            {
                CreateTextBubble("Prof. Hopper & Pip", text, new Color(0.98f, 0.95f, 0.88f, 1f), TextAnchor.MiddleLeft);
            }
        }

        private void CreateTextBubble(string sender, string message, Color bgColor, TextAnchor align)
        {
            if (chatContentContainer == null) return;

            GameObject bubbleGo = new GameObject("Bubble_" + sender);
            bubbleGo.transform.SetParent(chatContentContainer, false);

            var rect = bubbleGo.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(860f, 0f);

            var img = bubbleGo.AddComponent<Image>();
            img.color = bgColor;

            var outline = bubbleGo.AddComponent<Outline>();
            outline.effectColor = new Color(0.3f, 0.2f, 0.1f, 0.25f);
            outline.effectDistance = new Vector2(2, -2);

            var bubbleLayout = bubbleGo.AddComponent<VerticalLayoutGroup>();
            bubbleLayout.padding = new RectOffset(20, 20, 16, 16);
            bubbleLayout.childControlWidth = true;
            bubbleLayout.childControlHeight = true;
            bubbleLayout.childForceExpandWidth = true;
            bubbleLayout.childForceExpandHeight = false;

            var bubbleCsf = bubbleGo.AddComponent<ContentSizeFitter>();
            bubbleCsf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            GameObject textGo = new GameObject("Text");
            textGo.transform.SetParent(bubbleGo.transform, false);

            var t = textGo.AddComponent<Text>();
            t.text = $"<b>{sender}:</b>\n{message}";
            t.fontSize = 22;
            t.color = (align == TextAnchor.MiddleRight) ? Color.white : new Color(0.15f, 0.10f, 0.05f, 1f);
            t.alignment = align;
            t.font = GetDefaultFont();
            t.lineSpacing = 1.15f;
        }

        private void ScrollToBottom()
        {
            try
            {
                if (chatScrollRect != null)
                {
                    Canvas.ForceUpdateCanvases();
                    chatScrollRect.verticalNormalizedPosition = 0f;
                }
            }
            catch
            {
            }
        }

        private void EnsureApiClientExists()
        {
            try
            {
                if (GroqApiClient.Instance == null)
                {
                    GameObject clientGo = new GameObject("GroqApiClient");
                    clientGo.AddComponent<GroqApiClient>();
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[ChatbotUIController] EnsureApiClientExists skipped: {ex.Message}");
            }
        }

        /// <summary>
        /// Membangun antarmuka modal Chatbot secara prosedural jika belum dikonfigurasi melalui Inspector.
        /// Sepenuhnya selaras dengan acuan prototipe Docs/Ui Template/chatbot/code.html.
        /// </summary>
        public void EnsureProceduralUI()
        {
            if (chatbotModalRoot != null) return;

            try
            {
                Font uiFont = GetDefaultFont();

            // 1. Root Canvas Overlay (SortingOrder 100)
            GameObject canvasGo = new GameObject("ChatbotOverlayCanvas");
            canvasGo.transform.SetParent(this.transform, false);

            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;

            var scaler = canvasGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            scaler.matchWidthOrHeight = 0.5f;

            canvasGo.AddComponent<GraphicRaycaster>();
            chatbotModalRoot = canvasGo;

            // 2. Dark Backdrop
            GameObject backdropGo = new GameObject("DarkBackdrop");
            backdropGo.transform.SetParent(canvasGo.transform, false);
            var backdropRect = backdropGo.AddComponent<RectTransform>();
            backdropRect.anchorMin = Vector2.zero;
            backdropRect.anchorMax = Vector2.one;
            backdropRect.offsetMin = Vector2.zero;
            backdropRect.offsetMax = Vector2.zero;
            var backdropImg = backdropGo.AddComponent<Image>();
            backdropImg.color = new Color(0.02f, 0.05f, 0.10f, 0.88f);

            // 3. Main Dialog Panel
            GameObject panelGo = new GameObject("MainChatPanel");
            panelGo.transform.SetParent(canvasGo.transform, false);
            var panelRect = panelGo.AddComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0.5f);
            panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            panelRect.sizeDelta = new Vector2(980f, 1680f);
            panelRect.anchoredPosition = Vector2.zero;

            var panelImg = panelGo.AddComponent<Image>();
            panelImg.color = new Color(0.09f, 0.14f, 0.22f, 0.98f);

            var panelOutline = panelGo.AddComponent<Outline>();
            panelOutline.effectColor = new Color(0.85f, 0.65f, 0.20f, 0.85f);
            panelOutline.effectDistance = new Vector2(3, -3);

            // 4. Header Bar
            GameObject headerGo = new GameObject("HeaderBar");
            headerGo.transform.SetParent(panelGo.transform, false);
            var headerRect = headerGo.AddComponent<RectTransform>();
            headerRect.anchorMin = new Vector2(0f, 1f);
            headerRect.anchorMax = new Vector2(1f, 1f);
            headerRect.pivot = new Vector2(0.5f, 1f);
            headerRect.sizeDelta = new Vector2(0f, 140f);
            headerRect.anchoredPosition = Vector2.zero;

            var headerImg = headerGo.AddComponent<Image>();
            headerImg.color = new Color(0.13f, 0.20f, 0.32f, 1f);

            // Tombol Back di Header
            GameObject backBtnGo = new GameObject("CloseBackButton");
            backBtnGo.transform.SetParent(headerGo.transform, false);
            var backBtnRect = backBtnGo.AddComponent<RectTransform>();
            backBtnRect.anchorMin = new Vector2(0f, 0.5f);
            backBtnRect.anchorMax = new Vector2(0f, 0.5f);
            backBtnRect.pivot = new Vector2(0f, 0.5f);
            backBtnRect.sizeDelta = new Vector2(160f, 60f);
            backBtnRect.anchoredPosition = new Vector2(30f, 0f);

            var backBtnImg = backBtnGo.AddComponent<Image>();
            backBtnImg.color = new Color(0.85f, 0.65f, 0.20f, 1f);

            closeBackButton = backBtnGo.AddComponent<Button>();
            closeBackButton.targetGraphic = backBtnImg;

            GameObject backTxtGo = new GameObject("Text");
            backTxtGo.transform.SetParent(backBtnGo.transform, false);
            var backTxtRect = backTxtGo.AddComponent<RectTransform>();
            backTxtRect.anchorMin = Vector2.zero;
            backTxtRect.anchorMax = Vector2.one;
            backTxtRect.offsetMin = Vector2.zero;
            backTxtRect.offsetMax = Vector2.zero;
            var backTxt = backTxtGo.AddComponent<Text>();
            backTxt.text = "◀ BACK";
            backTxt.font = uiFont;
            backTxt.fontSize = 22;
            backTxt.fontStyle = FontStyle.Bold;
            backTxt.alignment = TextAnchor.MiddleCenter;
            backTxt.color = Color.white;

            // Judul Header
            GameObject titleGo = new GameObject("HeaderTitle");
            titleGo.transform.SetParent(headerGo.transform, false);
            var titleRect = titleGo.AddComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.5f, 0.5f);
            titleRect.anchorMax = new Vector2(0.5f, 0.5f);
            titleRect.sizeDelta = new Vector2(500f, 40f);
            titleRect.anchoredPosition = new Vector2(30f, 16f);

            var titleTxt = titleGo.AddComponent<Text>();
            titleTxt.text = "MATH COMPANION";
            titleTxt.font = uiFont;
            titleTxt.fontSize = 28;
            titleTxt.fontStyle = FontStyle.Bold;
            titleTxt.alignment = TextAnchor.MiddleCenter;
            titleTxt.color = new Color(0.99f, 0.83f, 0.30f, 1f);

            // Subtitle / Status Header
            GameObject subGo = new GameObject("HeaderSubtitle");
            subGo.transform.SetParent(headerGo.transform, false);
            var subRect = subGo.AddComponent<RectTransform>();
            subRect.anchorMin = new Vector2(0.5f, 0.5f);
            subRect.anchorMax = new Vector2(0.5f, 0.5f);
            subRect.sizeDelta = new Vector2(500f, 30f);
            subRect.anchoredPosition = new Vector2(30f, -22f);

            var subTxt = subGo.AddComponent<Text>();
            subTxt.text = "Prof. Hopper 🦗 & Pip 🐜 · 🟢 Online";
            subTxt.font = uiFont;
            subTxt.fontSize = 18;
            subTxt.alignment = TextAnchor.MiddleCenter;
            subTxt.color = new Color(0.20f, 0.83f, 0.60f, 1f);

            // 5. Input Footer Bar
            GameObject inputBarGo = new GameObject("InputFooterBar");
            inputBarGo.transform.SetParent(panelGo.transform, false);
            var inputBarRect = inputBarGo.AddComponent<RectTransform>();
            inputBarRect.anchorMin = new Vector2(0f, 0f);
            inputBarRect.anchorMax = new Vector2(1f, 0f);
            inputBarRect.pivot = new Vector2(0.5f, 0f);
            inputBarRect.sizeDelta = new Vector2(0f, 95f);
            inputBarRect.anchoredPosition = new Vector2(0f, 20f);

            // InputField
            GameObject inputGo = new GameObject("InputField");
            inputGo.transform.SetParent(inputBarGo.transform, false);
            var inputRect = inputGo.AddComponent<RectTransform>();
            inputRect.anchorMin = new Vector2(0f, 0.5f);
            inputRect.anchorMax = new Vector2(0f, 0.5f);
            inputRect.pivot = new Vector2(0f, 0.5f);
            inputRect.sizeDelta = new Vector2(710f, 70f);
            inputRect.anchoredPosition = new Vector2(30f, 0f);

            var inputImg = inputGo.AddComponent<Image>();
            inputImg.color = new Color(0.96f, 0.96f, 0.94f, 1f);

            chatInputField = inputGo.AddComponent<InputField>();

            GameObject placeholderGo = new GameObject("Placeholder");
            placeholderGo.transform.SetParent(inputGo.transform, false);
            var phRect = placeholderGo.AddComponent<RectTransform>();
            phRect.anchorMin = Vector2.zero;
            phRect.anchorMax = Vector2.one;
            phRect.offsetMin = new Vector2(20f, 0f);
            phRect.offsetMax = new Vector2(-20f, 0f);
            var phTxt = placeholderGo.AddComponent<Text>();
            phTxt.text = "Tanya aljabar pada Prof. Hopper...";
            phTxt.font = uiFont;
            phTxt.fontSize = 20;
            phTxt.fontStyle = FontStyle.Italic;
            phTxt.alignment = TextAnchor.MiddleLeft;
            phTxt.color = new Color(0.5f, 0.5f, 0.5f, 0.8f);

            GameObject inputTextGo = new GameObject("Text");
            inputTextGo.transform.SetParent(inputGo.transform, false);
            var itRect = inputTextGo.AddComponent<RectTransform>();
            itRect.anchorMin = Vector2.zero;
            itRect.anchorMax = Vector2.one;
            itRect.offsetMin = new Vector2(20f, 0f);
            itRect.offsetMax = new Vector2(-20f, 0f);
            var inTxt = inputTextGo.AddComponent<Text>();
            inTxt.font = uiFont;
            inTxt.fontSize = 22;
            inTxt.alignment = TextAnchor.MiddleLeft;
            inTxt.color = new Color(0.12f, 0.12f, 0.12f, 1f);

            chatInputField.textComponent = inTxt;
            chatInputField.placeholder = phTxt;

            // Tombol Kirim
            GameObject sendBtnGo = new GameObject("SendButton");
            sendBtnGo.transform.SetParent(inputBarGo.transform, false);
            var sendBtnRect = sendBtnGo.AddComponent<RectTransform>();
            sendBtnRect.anchorMin = new Vector2(1f, 0.5f);
            sendBtnRect.anchorMax = new Vector2(1f, 0.5f);
            sendBtnRect.pivot = new Vector2(1f, 0.5f);
            sendBtnRect.sizeDelta = new Vector2(180f, 70f);
            sendBtnRect.anchoredPosition = new Vector2(-30f, 0f);

            var sendBtnImg = sendBtnGo.AddComponent<Image>();
            sendBtnImg.color = new Color(0.05f, 0.65f, 0.35f, 1f);

            sendButton = sendBtnGo.AddComponent<Button>();
            sendButton.targetGraphic = sendBtnImg;

            GameObject sendTxtGo = new GameObject("Text");
            sendTxtGo.transform.SetParent(sendBtnGo.transform, false);
            var sendTxtRect = sendTxtGo.AddComponent<RectTransform>();
            sendTxtRect.anchorMin = Vector2.zero;
            sendTxtRect.anchorMax = Vector2.one;
            sendTxtRect.offsetMin = Vector2.zero;
            sendTxtRect.offsetMax = Vector2.zero;
            var sendTxt = sendTxtGo.AddComponent<Text>();
            sendTxt.text = "KIRIM ❯";
            sendTxt.font = uiFont;
            sendTxt.fontSize = 22;
            sendTxt.fontStyle = FontStyle.Bold;
            sendTxt.alignment = TextAnchor.MiddleCenter;
            sendTxt.color = Color.white;

            // Typing Indicator
            GameObject typingGo = new GameObject("TypingIndicator");
            typingGo.transform.SetParent(panelGo.transform, false);
            var typingRect = typingGo.AddComponent<RectTransform>();
            typingRect.anchorMin = new Vector2(0f, 0f);
            typingRect.anchorMax = new Vector2(1f, 0f);
            typingRect.pivot = new Vector2(0.5f, 0f);
            typingRect.sizeDelta = new Vector2(0f, 30f);
            typingRect.anchoredPosition = new Vector2(0f, 118f);

            var typingTxt = typingGo.AddComponent<Text>();
            typingTxt.text = "Prof. Hopper sedang mengetik... 🦗";
            typingTxt.font = uiFont;
            typingTxt.fontSize = 18;
            typingTxt.fontStyle = FontStyle.Italic;
            typingTxt.alignment = TextAnchor.MiddleCenter;
            typingTxt.color = new Color(0.30f, 0.85f, 0.65f, 1f);
            typingIndicator = typingGo;
            typingIndicator.SetActive(false);

            // 6. Quick Chips Panel
            GameObject chipsPanelGo = new GameObject("QuickChipsPanel");
            chipsPanelGo.transform.SetParent(panelGo.transform, false);
            var chipsRect = chipsPanelGo.AddComponent<RectTransform>();
            chipsRect.anchorMin = new Vector2(0f, 0f);
            chipsRect.anchorMax = new Vector2(1f, 0f);
            chipsRect.pivot = new Vector2(0.5f, 0f);
            chipsRect.sizeDelta = new Vector2(0f, 60f);
            chipsRect.anchoredPosition = new Vector2(0f, 150f);

            chipIsolateVariable = CreateChipButton(chipsPanelGo, "ChipIsolate", "💡 Pindah Ruas", new Vector2(-305f, 0f), uiFont);
            chipWhatIsVariable = CreateChipButton(chipsPanelGo, "ChipVariable", "🔍 Apa Variabel?", new Vector2(0f, 0f), uiFont);
            chipMissionHint = CreateChipButton(chipsPanelGo, "ChipHint", "📜 Petunjuk Misi", new Vector2(305f, 0f), uiFont);

            // 7. Chat Stream ScrollArea
            GameObject scrollGo = new GameObject("ChatScrollArea");
            scrollGo.transform.SetParent(panelGo.transform, false);
            var scrollRectTransform = scrollGo.AddComponent<RectTransform>();
            scrollRectTransform.anchorMin = new Vector2(0f, 0f);
            scrollRectTransform.anchorMax = new Vector2(1f, 1f);
            scrollRectTransform.offsetMin = new Vector2(30f, 220f);
            scrollRectTransform.offsetMax = new Vector2(-30f, -155f);

            chatScrollRect = scrollGo.AddComponent<ScrollRect>();
            chatScrollRect.horizontal = false;
            chatScrollRect.vertical = true;
            chatScrollRect.movementType = ScrollRect.MovementType.Clamped;

            // Viewport
            GameObject viewportGo = new GameObject("Viewport");
            viewportGo.transform.SetParent(scrollGo.transform, false);
            var vpRect = viewportGo.AddComponent<RectTransform>();
            vpRect.anchorMin = Vector2.zero;
            vpRect.anchorMax = Vector2.one;
            vpRect.offsetMin = Vector2.zero;
            vpRect.offsetMax = Vector2.zero;
            viewportGo.AddComponent<RectMask2D>();
            chatScrollRect.viewport = vpRect;

            // Content
            GameObject contentGo = new GameObject("Content");
            contentGo.transform.SetParent(viewportGo.transform, false);
            var contentRect = contentGo.AddComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0f, 1f);
            contentRect.anchorMax = new Vector2(1f, 1f);
            contentRect.pivot = new Vector2(0.5f, 1f);
            contentRect.sizeDelta = new Vector2(0f, 0f);

            var vlg = contentGo.AddComponent<VerticalLayoutGroup>();
            vlg.padding = new RectOffset(16, 16, 16, 16);
            vlg.spacing = 16f;
            vlg.childControlWidth = true;
            vlg.childControlHeight = true;
            vlg.childForceExpandWidth = true;
            vlg.childForceExpandHeight = false;

            var csf = contentGo.AddComponent<ContentSizeFitter>();
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            chatScrollRect.content = contentRect;
            chatContentContainer = contentRect;

            // Pasang pesan sambutan awal
            SpawnBotBubble("Ribbit! Halo petualang aljabar! Aku Prof. Hopper 🦗 dan ini asistenku Pip 🐜. Ada konsep atau soal yang membingungkanmu? Tanyakan langsung atau klik tombol petunjuk di bawah!");

            // Hubungkan seluruh event listener
            SetupListeners();
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[ChatbotUIController] EnsureProceduralUI skipped or failed: {ex.Message}");
            }
        }

        private Button CreateChipButton(GameObject parent, string name, string label, Vector2 pos, Font font)
        {
            GameObject btnGo = new GameObject(name);
            btnGo.transform.SetParent(parent.transform, false);
            var rect = btnGo.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(290f, 52f);
            rect.anchoredPosition = pos;

            var img = btnGo.AddComponent<Image>();
            img.color = new Color(0.18f, 0.28f, 0.45f, 1f);

            var btn = btnGo.AddComponent<Button>();
            btn.targetGraphic = img;

            GameObject txtGo = new GameObject("Text");
            txtGo.transform.SetParent(btnGo.transform, false);
            var txtRect = txtGo.AddComponent<RectTransform>();
            txtRect.anchorMin = Vector2.zero;
            txtRect.anchorMax = Vector2.one;
            txtRect.offsetMin = Vector2.zero;
            txtRect.offsetMax = Vector2.zero;
            var t = txtGo.AddComponent<Text>();
            t.text = label;
            t.font = font;
            t.fontSize = 18;
            t.fontStyle = FontStyle.Bold;
            t.alignment = TextAnchor.MiddleCenter;
            t.color = Color.white;

            return btn;
        }

        private static Font GetDefaultFont()
        {
            try
            {
                return Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf")
                    ?? Resources.GetBuiltinResource<Font>("Arial.ttf")
                    ?? Font.CreateDynamicFontFromOSFont("Arial", 16);
            }
            catch
            {
                return null;
            }
        }
    }
}

