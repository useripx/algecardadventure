using UnityEngine;

namespace AlgeCardAdventure.Audio
{
    /// <summary>
    /// Generator audio prosedural ringan untuk efek suara game bawaan (SFX fallback).
    /// Menjamin game memiliki efek suara klik dan feedback instan tanpa ketergantungan file eksternal.
    /// </summary>
    public static class ProceduralAudio
    {
        /// <summary>
        /// Membuat klip suara klik tombol kayu/pop lembut (frekuensi ~600Hz menurun cepat ke ~200Hz).
        /// </summary>
        public static AudioClip CreateButtonClickClip()
        {
            const int sampleRate = 44100;
            const float duration = 0.08f;
            int totalSamples = (int)(sampleRate * duration);
            float[] samples = new float[totalSamples];

            for (int i = 0; i < totalSamples; i++)
            {
                float t = (float)i / sampleRate;
                float envelope = Mathf.Exp(-i * 45f / totalSamples); // Peluruhan eksponensial cepat
                float frequency = Mathf.Lerp(650f, 220f, (float)i / totalSamples);
                samples[i] = Mathf.Sin(2f * Mathf.PI * frequency * t) * envelope * 0.7f;
            }

            AudioClip clip = AudioClip.Create("Procedural_ButtonClick", totalSamples, 1, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }

        /// <summary>
        /// Membuat klip suara gesekan kartu (desir/swoosh halus).
        /// </summary>
        public static AudioClip CreateCardFlipClip()
        {
            const int sampleRate = 44100;
            const float duration = 0.12f;
            int totalSamples = (int)(sampleRate * duration);
            float[] samples = new float[totalSamples];

            for (int i = 0; i < totalSamples; i++)
            {
                float envelope = Mathf.Sin((float)i / totalSamples * Mathf.PI);
                float noise = (Random.value * 2f - 1f) * 0.35f;
                float lowSine = Mathf.Sin(2f * Mathf.PI * 300f * ((float)i / sampleRate)) * 0.25f;
                samples[i] = (noise + lowSine) * envelope;
            }

            AudioClip clip = AudioClip.Create("Procedural_CardFlip", totalSamples, 1, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }

        /// <summary>
        /// Membuat klip jingle nada benar (dua nada naik C5 -> G5).
        /// </summary>
        public static AudioClip CreateCorrectJingleClip()
        {
            const int sampleRate = 44100;
            const float duration = 0.35f;
            int totalSamples = (int)(sampleRate * duration);
            float[] samples = new float[totalSamples];

            int halfSamples = totalSamples / 2;
            for (int i = 0; i < totalSamples; i++)
            {
                float t = (float)i / sampleRate;
                float freq = (i < halfSamples) ? 523.25f : 783.99f; // C5 lalu G5
                float env = 1f - ((float)(i % halfSamples) / halfSamples);
                samples[i] = Mathf.Sin(2f * Mathf.PI * freq * t) * env * 0.6f;
            }

            AudioClip clip = AudioClip.Create("Procedural_CorrectJingle", totalSamples, 1, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }

        /// <summary>
        /// Membuat klip nada salah (thud lembut frekuensi rendah ~140Hz).
        /// </summary>
        public static AudioClip CreateWrongThudClip()
        {
            const int sampleRate = 44100;
            const float duration = 0.22f;
            int totalSamples = (int)(sampleRate * duration);
            float[] samples = new float[totalSamples];

            for (int i = 0; i < totalSamples; i++)
            {
                float t = (float)i / sampleRate;
                float env = Mathf.Exp(-i * 12f / totalSamples);
                float freq = Mathf.Lerp(160f, 90f, (float)i / totalSamples);
                samples[i] = Mathf.Sin(2f * Mathf.PI * freq * t) * env * 0.7f;
            }

            AudioClip clip = AudioClip.Create("Procedural_WrongThud", totalSamples, 1, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }
    }
}
