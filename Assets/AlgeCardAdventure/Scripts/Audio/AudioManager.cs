using UnityEngine;

namespace AlgeCardAdventure.Audio
{
    /// <summary>
    /// Pengelola audio terpusat (Singleton) untuk musik latar (BGM) dan efek suara (SFX).
    /// Mendukung pemutaran audio dinamis, kontrol bisu (mute), serta fallback audio prosedural.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;

        public static AudioManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<AudioManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("AudioManager");
                        _instance = go.AddComponent<AudioManager>();
                        if (Application.isPlaying)
                        {
                            DontDestroyOnLoad(go);
                        }
                    }
                }
                return _instance;
            }
        }

        [Header("Sumber Audio")]
        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Klip Audio Kustom (Opsional)")]
        [SerializeField] private AudioClip buttonClickClip;
        [SerializeField] private AudioClip cardFlipClip;
        [SerializeField] private AudioClip correctJingleClip;
        [SerializeField] private AudioClip wrongThudClip;
        [SerializeField] private AudioClip defaultBgmClip;

        // Klip prosedural bawaan (cache)
        private AudioClip _cachedButtonClick;
        private AudioClip _cachedCardFlip;
        private AudioClip _cachedCorrect;
        private AudioClip _cachedWrong;

        public bool IsMuted { get; private set; } = false;
        public float BgmVolume { get; private set; } = 0.8f;
        public float SfxVolume { get; private set; } = 1.0f;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                if (Application.isPlaying)
                {
                    DontDestroyOnLoad(gameObject);
                }
                InitializeSources();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void InitializeSources()
        {
            if (bgmSource == null)
            {
                bgmSource = gameObject.AddComponent<AudioSource>();
                bgmSource.loop = true;
                bgmSource.playOnAwake = false;
                bgmSource.volume = BgmVolume;
            }

            if (sfxSource == null)
            {
                sfxSource = gameObject.AddComponent<AudioSource>();
                sfxSource.loop = false;
                sfxSource.playOnAwake = false;
                sfxSource.volume = SfxVolume;
            }
        }

        public void PlayBGM(AudioClip clip, bool loop = true)
        {
            if (bgmSource == null) InitializeSources();
            if (clip == null) clip = defaultBgmClip;
            if (clip == null) return;

            if (bgmSource.clip == clip && bgmSource.isPlaying) return;

            bgmSource.clip = clip;
            bgmSource.loop = loop;
            bgmSource.mute = IsMuted;
            bgmSource.Play();
        }

        public void StopBGM()
        {
            if (bgmSource != null)
            {
                bgmSource.Stop();
            }
        }

        public void PlaySFX(AudioClip clip, float volumeScale = 1.0f)
        {
            if (IsMuted || clip == null) return;
            if (sfxSource == null) InitializeSources();

            sfxSource.PlayOneShot(clip, SfxVolume * volumeScale);
        }

        public void PlayButtonClick()
        {
            if (buttonClickClip != null)
            {
                PlaySFX(buttonClickClip);
            }
            else
            {
                if (_cachedButtonClick == null)
                {
                    _cachedButtonClick = ProceduralAudio.CreateButtonClickClip();
                }
                PlaySFX(_cachedButtonClick);
            }
        }

        public void PlayCardFlip()
        {
            if (cardFlipClip != null)
            {
                PlaySFX(cardFlipClip);
            }
            else
            {
                if (_cachedCardFlip == null)
                {
                    _cachedCardFlip = ProceduralAudio.CreateCardFlipClip();
                }
                PlaySFX(_cachedCardFlip);
            }
        }

        public void PlayCorrect()
        {
            if (correctJingleClip != null)
            {
                PlaySFX(correctJingleClip);
            }
            else
            {
                if (_cachedCorrect == null)
                {
                    _cachedCorrect = ProceduralAudio.CreateCorrectJingleClip();
                }
                PlaySFX(_cachedCorrect);
            }
        }

        public void PlayWrong()
        {
            if (wrongThudClip != null)
            {
                PlaySFX(wrongThudClip);
            }
            else
            {
                if (_cachedWrong == null)
                {
                    _cachedWrong = ProceduralAudio.CreateWrongThudClip();
                }
                PlaySFX(_cachedWrong);
            }
        }

        public void SetMute(bool mute)
        {
            IsMuted = mute;
            if (bgmSource != null) bgmSource.mute = IsMuted;
            if (sfxSource != null) sfxSource.mute = IsMuted;
        }

        public void ToggleMute()
        {
            SetMute(!IsMuted);
        }

        public void SetBgmVolume(float volume)
        {
            BgmVolume = Mathf.Clamp01(volume);
            if (bgmSource != null) bgmSource.volume = BgmVolume;
        }

        public void SetSfxVolume(float volume)
        {
            SfxVolume = Mathf.Clamp01(volume);
            if (sfxSource != null) sfxSource.volume = SfxVolume;
        }
    }
}
