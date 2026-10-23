using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using AlgeCardAdventure.UI;
using AlgeCardAdventure.Core;

namespace AlgeCardAdventure.Editor
{
    /// <summary>
    /// Builder editor otomatis untuk menyusun scene 01_SplashScreen secara deterministik.
    /// Menjamin seluruh referensi komponen, sprite, canvas scaler, dan event system terhubung rapi.
    /// </summary>
    public static class SplashScreenSceneBuilder
    {
        private const string SceneDirectory = "Assets/AlgeCardAdventure/Scenes";
        private const string ScenePath = "Assets/AlgeCardAdventure/Scenes/01_SplashScreen.unity";

        [MenuItem("Tools/AlgeCard Adventure/Build Splash Screen Scene")]
        public static void BuildScene()
        {
            if (!Directory.Exists(SceneDirectory))
            {
                Directory.CreateDirectory(SceneDirectory);
                AssetDatabase.Refresh();
            }

            // 1. Buat scene kosong baru
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // 2. Kamera Utama (Orthographic 2D, tema hijau hutan gelap #1B261D)
            var camGo = new GameObject("Main Camera");
            camGo.tag = "MainCamera";
            var cam = camGo.AddComponent<Camera>();
            cam.orthographic = true;
            cam.orthographicSize = 5f;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0.11f, 0.15f, 0.12f, 1f);
            camGo.AddComponent<AudioListener>();
            var camData = camGo.AddComponent<UniversalAdditionalCameraData>();
            camData.renderPostProcessing = false;
            camGo.transform.position = new Vector3(0, 0, -10);

            // 3. (Global Light 2D dikelola langsung di scene — tidak dibuat via builder
            //    agar tidak memerlukan dependency Unity.RenderPipelines.Universal.2D.Runtime)

            // 4. EventSystem untuk interaksi UI
            var eventGo = new GameObject("EventSystem");
            eventGo.AddComponent<EventSystem>();
            eventGo.AddComponent<StandaloneInputModule>();

            // 5. Canvas (Screen Space Overlay, Scaler Portrait 1080x1920)
            var canvasGo = new GameObject("Canvas");
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGo.AddComponent<GraphicRaycaster>();
            var scaler = canvasGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            // 6. Latar Belakang (Background) full-screen
            var bgGo = new GameObject("Background");
            bgGo.transform.SetParent(canvasGo.transform, false);
            var bgRect = bgGo.AddComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;
            var bgImage = bgGo.AddComponent<Image>();
            var bgSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/AlgeCardAdventure/Art/Backgrounds/splash_bg.png");
            if (bgSprite != null)
            {
                bgImage.sprite = bgSprite;
            }
            bgImage.color = Color.white;

            // 7. SafeArea Container (Pelindung notch dan bar navigasi)
            var safeAreaGo = new GameObject("SafeAreaContainer");
            safeAreaGo.transform.SetParent(canvasGo.transform, false);
            var safeRect = safeAreaGo.AddComponent<RectTransform>();
            safeRect.anchorMin = Vector2.zero;
            safeRect.anchorMax = Vector2.one;
            safeRect.offsetMin = Vector2.zero;
            safeRect.offsetMax = Vector2.zero;
            safeAreaGo.AddComponent<SafeAreaFitter>();

            // 8. Logo Resmi AlgeCard Adventure (posisi atas-tengah)
            var logoGo = new GameObject("Logo");
            logoGo.transform.SetParent(safeAreaGo.transform, false);
            var logoRect = logoGo.AddComponent<RectTransform>();
            logoRect.anchorMin = new Vector2(0.5f, 0.72f);
            logoRect.anchorMax = new Vector2(0.5f, 0.72f);
            logoRect.pivot = new Vector2(0.5f, 0.5f);
            logoRect.sizeDelta = new Vector2(860, 287);
            var logoImage = logoGo.AddComponent<Image>();
            var logoSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/AlgeCardAdventure/Art/Logo/logo.png");
            if (logoSprite != null)
            {
                logoImage.sprite = logoSprite;
            }
            logoImage.preserveAspect = true;

            // 9. Prompt Container & Efek Denyut Teks (posisi bawah)
            var promptGo = new GameObject("PromptContainer");
            promptGo.transform.SetParent(safeAreaGo.transform, false);
            var promptRect = promptGo.AddComponent<RectTransform>();
            promptRect.anchorMin = new Vector2(0.5f, 0.12f);
            promptRect.anchorMax = new Vector2(0.5f, 0.12f);
            promptRect.pivot = new Vector2(0.5f, 0.5f);
            promptRect.sizeDelta = new Vector2(750, 120);
            promptGo.AddComponent<PulsingTextEffect>();

            // Teks "Tap anywhere to start"
            var textGo = new GameObject("PromptText");
            textGo.transform.SetParent(promptGo.transform, false);
            var textRect = textGo.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
            var text = textGo.AddComponent<Text>();
            text.text = "Tap anywhere to start";
            text.fontSize = 44;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.white;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf") ?? Resources.GetBuiltinResource<Font>("Arial.ttf");

            // Shadow pada teks agar kontras tajam di atas latar belakang
            var shadow = textGo.AddComponent<Shadow>();
            shadow.effectColor = new Color(0, 0, 0, 0.9f);
            shadow.effectDistance = new Vector2(3, -3);

            // 10. Tombol Pendeteksi Ketukan Satu Layar Penuh
            var tapGo = new GameObject("TapDetectorButton");
            tapGo.transform.SetParent(canvasGo.transform, false);
            tapGo.transform.SetAsLastSibling();
            var tapRect = tapGo.AddComponent<RectTransform>();
            tapRect.anchorMin = Vector2.zero;
            tapRect.anchorMax = Vector2.one;
            tapRect.offsetMin = Vector2.zero;
            tapRect.offsetMax = Vector2.zero;
            var tapImage = tapGo.AddComponent<Image>();
            tapImage.color = new Color(0, 0, 0, 0); // Transparan penuh
            tapImage.raycastTarget = true;
            var tapButton = tapGo.AddComponent<Button>();

            // 11. Controller pada Canvas
            var controller = canvasGo.AddComponent<SplashScreenController>();
            var serializedObj = new SerializedObject(controller);
            serializedObj.FindProperty("tapDetectorButton").objectReferenceValue = tapButton;
            serializedObj.FindProperty("promptContainer").objectReferenceValue = promptGo;
            serializedObj.ApplyModifiedProperties();

            // Simpan Scene
            EditorSceneManager.SaveScene(scene, ScenePath);
            Debug.Log($"[SplashScreenSceneBuilder] Scene berhasil dibuat dan disimpan: {ScenePath}");

            // Daftarkan scene ke Build Settings
            RegisterSceneInBuildSettings(ScenePath);
        }

        private static void RegisterSceneInBuildSettings(string path)
        {
            var scenes = EditorBuildSettings.scenes;
            bool exists = false;
            foreach (var s in scenes)
            {
                if (s.path == path)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                var newScenes = new EditorBuildSettingsScene[scenes.Length + 1];
                newScenes[0] = new EditorBuildSettingsScene(path, true);
                for (int i = 0; i < scenes.Length; i++)
                {
                    newScenes[i + 1] = scenes[i];
                }
                EditorBuildSettings.scenes = newScenes;
                Debug.Log($"[SplashScreenSceneBuilder] Scene {path} didaftarkan ke Build Settings pada index 0.");
            }
        }
    }
}
