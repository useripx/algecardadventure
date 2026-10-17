using NUnit.Framework;
using UnityEngine;
using AlgeCardAdventure.Audio;

namespace AlgeCardAdventure.Tests
{
    [TestFixture]
    public class AudioManagerTests
    {
        private GameObject _audioGo;
        private AudioManager _audioManager;

        [SetUp]
        public void Setup()
        {
            _audioGo = new GameObject("TestAudioManager");
            _audioManager = _audioGo.AddComponent<AudioManager>();
        }

        [TearDown]
        public void Teardown()
        {
            if (_audioGo != null)
            {
                Object.DestroyImmediate(_audioGo);
            }
        }

        [Test]
        public void TC_AUDIO_01_InitialState_NotMutedAndDefaultVolumes()
        {
            Assert.IsFalse(_audioManager.IsMuted);
            Assert.Greater(_audioManager.BgmVolume, 0f);
            Assert.Greater(_audioManager.SfxVolume, 0f);
        }

        [Test]
        public void TC_AUDIO_02_MuteControls_ToggleAndSetMuteWorkCorrectly()
        {
            _audioManager.SetMute(true);
            Assert.IsTrue(_audioManager.IsMuted);

            _audioManager.ToggleMute();
            Assert.IsFalse(_audioManager.IsMuted);
        }

        [Test]
        public void TC_AUDIO_03_VolumeClamping_ClampsBetweenZeroAndOne()
        {
            _audioManager.SetBgmVolume(1.5f);
            Assert.AreEqual(1.0f, _audioManager.BgmVolume);

            _audioManager.SetBgmVolume(-0.5f);
            Assert.AreEqual(0.0f, _audioManager.BgmVolume);

            _audioManager.SetSfxVolume(2.0f);
            Assert.AreEqual(1.0f, _audioManager.SfxVolume);
        }

        [Test]
        public void TC_AUDIO_04_ProceduralAudio_GeneratesValidAudioClips()
        {
            var btnClip = ProceduralAudio.CreateButtonClickClip();
            Assert.IsNotNull(btnClip);
            Assert.Greater(btnClip.length, 0f);

            var flipClip = ProceduralAudio.CreateCardFlipClip();
            Assert.IsNotNull(flipClip);
            Assert.Greater(flipClip.length, 0f);

            var correctClip = ProceduralAudio.CreateCorrectJingleClip();
            Assert.IsNotNull(correctClip);
            Assert.Greater(correctClip.length, 0f);

            var wrongClip = ProceduralAudio.CreateWrongThudClip();
            Assert.IsNotNull(wrongClip);
            Assert.Greater(wrongClip.length, 0f);
        }
    }
}
