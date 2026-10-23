using System;
using System.Collections.Generic;
using UnityEngine;

namespace AlgeCardAdventure.AI
{
    /// <summary>
    /// Mengelola riwayat percakapan sesi aktif, system prompt pedagogis,
    /// batas memori rolling window (token-efficient), dan graceful offline fallback.
    /// </summary>
    public class ChatSessionManager
    {
        private const int MaxHistoryMessages = 6;

        public const string DefaultSystemPrompt = 
            "You are Prof. Hopper (a wise, friendly green cricket mathematics professor) " +
            "and Pip (an energetic orange cricket apprentice), the math companion duo of the mobile educational game 'AlgeCard Adventure'! " +
            "Your quest is to help middle school students master algebra (variables, coefficients, constants, solving linear equations) " +
            "with encouraging, cheerful, fantasy adventure guidance.\n\n" +
            "Key Pedagogical Rules:\n" +
            "1. Always explain abstract variables using friendly, concrete examples (e.g. b = boneka/dolls, a = anggur/grapes, p = pensil/pencils, c = cake/kue).\n" +
            "2. Guide the student step-by-step using the two-pan balance rule (doing the same operation to both sides).\n" +
            "3. Never give away raw puzzle answers directly; provide hints, clues, and encouraging questions.\n" +
            "4. Respond naturally in the language used by the student (Indonesian or English).\n" +
            "5. Keep responses concise, warm, and structured with clear bullet points or emojis.";

        private List<ChatMessage> _messages = new List<ChatMessage>();

        public IReadOnlyList<ChatMessage> Messages => _messages;

        public ChatSessionManager()
        {
            ResetSession();
        }

        public void ResetSession()
        {
            _messages.Clear();
            _messages.Add(new ChatMessage("system", DefaultSystemPrompt));
        }

        public void AddUserMessage(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            _messages.Add(new ChatMessage("user", text.Trim()));
            CapHistory();
        }

        public void AddAssistantMessage(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            _messages.Add(new ChatMessage("assistant", text.Trim()));
            CapHistory();
        }

        /// <summary>
        /// Membatasi riwayat pesan rolling window agar hemat token dan latensi rendah di ponsel.
        /// </summary>
        public void CapHistory()
        {
            // System prompt (index 0) selalu dipertahankan
            while (_messages.Count > MaxHistoryMessages + 1)
            {
                _messages.RemoveAt(1);
            }
        }

        /// <summary>
        /// Menghasilkan respons lokal ramah saat perangkat offline atau jaringan bermasalah.
        /// </summary>
        public string GetOfflineFallbackResponse(string query)
        {
            string lower = (query ?? "").ToLowerInvariant();

            if (lower.Contains("isolate") || lower.Contains("ruas") || lower.Contains("pindah"))
            {
                return "🦗 **Prof. Hopper:**\n" +
                       "Ribbit! Saat offline pun rahasia aljabar tetap berlaku! " +
                       "Untuk mengisolasi variabel, gunakan operasi lawan pada kedua ruas:\n" +
                       "• Jika ada penjumlahan (**+5**), **kurangi 5** di kedua ruas!\n" +
                       "• Jika ada pengurangan (**-3**), **tambahkan 3** di kedua ruas!\n" +
                       "Contoh: $y - 3 = 5 \\rightarrow y = 5 + 3 = 8$!";
            }
            else if (lower.Contains("variabel") || lower.Contains("variable") || lower.Contains("koefisien") || lower.Contains("coefficient"))
            {
                return "🐜 **Pip:**\n" +
                       "Variabel itu seperti kotak misteri ajaib! Huruf mewakili benda nyata:\n" +
                       "• $b$ = banyak boneka 🧸\n" +
                       "• $a$ = banyak buah anggur 🍇\n" +
                       "• $p$ = harga sebatang pensil ✏️\n" +
                       "Sedangkan angka di depan variabel (seperti angka **3** pada $3x$) disebut **Koefisien**!";
            }
            else if (lower.Contains("hint") || lower.Contains("soal") || lower.Contains("misi") || lower.Contains("mission"))
            {
                return "🦗 **Prof. Hopper & Pip:**\n" +
                       "Tips Petualangan Misi:\n" +
                       "1. Baca soal cerita dan tentukan apa yang dicari (jadikan variabel $x$ atau $y$).\n" +
                       "2. Masukkan kartu angka dan operator di papan tahap 1 untuk membentuk persamaan awal.\n" +
                       "3. Geser suku angka ke ruas kanan pada tahap 2 dan 3 hingga tersisa nilai variabel murni!";
            }

            return "🦗 **Prof. Hopper & Pip (Offline Mode):**\n" +
                   "Ribbit! Kami sedang offline di hutan fantasi AlgeCard. " +
                   "Kamu tetap bisa berpetualang dan membaca materi ringkasan aljabar di buku meja! " +
                   "Sambungkan kembali ke internet jika ingin berdiskusi bebas dengan kami.";
        }
    }
}
