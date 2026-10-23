# Test Report — AlgeCard Adventure

> **STATUS:** Laporan Verifikasi Pengujian Nyata  
> **PRINSIP:** *Jangan Pernah Bilang Selesai Jika Belum Terbukti Lulus Uji* (**Pilar 6**)  
> **TARGET KEBERHASILAN:** 100% PASS (0 GAGAL / 0 ERROR)  

---

## 🧪 Riwayat Eksekusi Pengujian

### Batch 2: Fondasi UI Portrait, Safe Area & Screen 01 Splash Screen
- **Tanggal:** 17 Oktober 2026 15:30 WIB
- **Lingkungan Pengujian:** Unity `6000.6.6f1` (Windows 11) / Unity Test Framework 1.8.0
- **Tipe Uji:** EditMode Automated Unit Tests

| ID Kasus Uji | Modul / Komponen | Skenario yang Diuji | Hasil yang Diharapkan | Status Aktual |
|---|---|---|---|:---:|
| **TC-DATA-01** | `QuestionRepository` | Membaca berkas `banksoal.json` dari `StreamingAssets` | File terbaca utuh, objek data tidak null | **PASS** |
| **TC-DATA-02** | `QuestionRepository` | Verifikasi total soal dan pembagian level | Tepat 60 soal (20 Easy, 20 Normal, 20 Hard) | **PASS** |
| **TC-DATA-03** | `QuestionRepository` | Verifikasi keunikan ID soal | Tidak ada ID duplikat antar seluruh level | **PASS** |
| **TC-DATA-04** | `QuestionRepository` | Filtering soal flagged (`review_status != 'transcribed'`) | 4 soal flagged otomatis dikeluarkan dari playable pool (hanya 56 soal aktif) | **PASS** |
| **TC-DATA-05** | `QuestionRepository` | Penanganan data `true_false: null` | Nilai null tidak menyebabkan crash / null reference exception | **PASS** |
| **TC-DATA-06** | `QuestionRepository` | Pengambilan batch acak misi per level | Menghasilkan maksimal 5 soal eligible tanpa duplikasi dalam 1 batch | **PASS** |
| **TC-SCORE-01** | `ScoreCalculator` | 0 soal dijawab | Skor = 0 (penanganan aman zero-division) | **PASS** |
| **TC-SCORE-02** | `ScoreCalculator` | 1 dijawab, 1 benar | Skor = 100 | **PASS** |
| **TC-SCORE-03** | `ScoreCalculator` | 2 dijawab, 1 benar | Skor = 50 | **PASS** |
| **TC-SCORE-04** | `ScoreCalculator` | 3 dijawab, 2 benar | Skor = 67 (pembulatan bilangan bulat) | **PASS** |
| **TC-SCORE-05** | `ScoreCalculator` | 5 dijawab, 4 benar | Skor = 80 | **PASS** |
| **TC-SCORE-06** | `ScoreCalculator` | Batas maksimum 10 soal | Sesi tidak dapat memulai soal ke-11 | **PASS** |
| **TC-UI-01** | `SafeAreaFitter` | Layar standar tanpa notch/poni (1080x1920) | Menghasilkan `anchorMin=(0,0)` dan `anchorMax=(1,1)` | **PASS** |
| **TC-UI-02** | `SafeAreaFitter` | Layar HP bertakik (notch atas 80px, nav bar 60px) | Menghitung normalisasi anchor yang presisi dan aman dari terpotong | **PASS** |
| **TC-UI-03** | `SafeAreaFitter` | Kondisi resolusi layar nol / transisi | Fallback aman tanpa crash atau exception zero division | **PASS** |
| **TC-UI-04** | `SafeAreaFitter` | Reset offset saat menerapkan Safe Area | `offsetMin` dan `offsetMax` terbukti direset ke `(0,0)` | **PASS** |
| **TC-NAV-01** | `SceneNavigator` | Verifikasi konstanta string scene 6 layar & Credits | Seluruh nama scene terdefinisi stabil dan konsisten | **PASS** |
| **TC-NAV-02** | `SceneNavigator` | Event delegasi `OnBeforeSceneLoad` | Event terpanggil membawa nama scene tujuan secara tepat | **PASS** |

### Batch 3: Screen 02 Level Selection & Audio Foundation
- **Tanggal:** 17 Oktober 2026 16:15 WIB
- **Lingkungan Pengujian:** Unity `6000.6.6f1` (Windows 11) / Unity Test Framework 1.8.0
- **Tipe Uji:** EditMode Automated Unit Tests

| ID Kasus Uji | Modul / Komponen | Skenario yang Diuji | Hasil yang Diharapkan | Status Aktual |
|---|---|---|---|:---:|
| **TC-SESSION-01** | `GameSession` | Inisialisasi sesi dengan level valid (Easy, Normal, Hard) | Level tersimpan tepat dan sesi berstatus aktif | **PASS** |
| **TC-SESSION-02** | `GameSession` | Normalisasi level 'Medium' ke 'Normal' (DEC-04) | Otomatis terpetakan ke 'Normal' | **PASS** |
| **TC-SESSION-03** | `GameSession` | Penanganan level null / string acak tak valid | Fallback aman ke 'Easy' tanpa exception | **PASS** |
| **TC-SESSION-04** | `GameSession` | Pencatatan jawaban & pembaruan persentase skor | Total dan skor kumulatif terhitung akurat | **PASS** |
| **TC-SESSION-05** | `GameSession` | Verifikasi batas tepat 10 soal per sesi (DEC-08) | `HasReachedLimit()` bernilai `true` tepat pada soal ke-10 | **PASS** |
| **TC-SESSION-06** | `GameSession` | Reset sesi bermain | Counter soal, skor, dan soal aktif kembali ke 0 / null | **PASS** |
| **TC-AUDIO-01** | `AudioManager` | Status inisialisasi awal | Volume default > 0 dan tidak dalam kondisi bisu (mute) | **PASS** |
| **TC-AUDIO-02** | `AudioManager` | Kontrol toggle dan set mute | Status `IsMuted` berganti konsisten | **PASS** |
| **TC-AUDIO-03** | `AudioManager` | Clamping volume BGM dan SFX (0.0 s.d. 1.0) | Nilai di luar batas otomatis di-clamp aman | **PASS** |
| **TC-AUDIO-04** | `ProceduralAudio` | Pembuatan 4 klip suara sintesis bawaan | Klip valid, durasi > 0, dan dapat diputar audio engine | **PASS** |
| **TC-NAV-03** | `LevelSelectionController` | Pemilihan level 'Hard' | Menyimpan level 'Hard', inisialisasi batas 10 soal, dan berpindah ke Screen 03 | **PASS** |
| **TC-NAV-04** | `LevelSelectionController` | Klik tombol Back | Memicu navigasi kembali ke Screen 01 Splash Screen | **PASS** |

### Batch 4: Screen 03 Mission Cards & Cozy Fantasy BGM
- **Tanggal:** 17 Oktober 2026 17:05 WIB
- **Lingkungan Pengujian:** Unity `6000.6.6f1` (Windows 11) / Unity Test Framework 1.8.0
- **Tipe Uji:** EditMode Automated Unit Tests

| ID Kasus Uji | Modul / Komponen | Skenario yang Diuji | Hasil yang Diharapkan | Status Aktual |
|---|---|---|---|:---:|
| **TC-MISSION-01** | `QuestionRepository` | Penarikan 5 kartu misi acak per level | Tepat 5 soal unik, level sesuai, dan eligible dimainkan | **PASS** |
| **TC-MISSION-02** | `QuestionRepository` | Anti-duplikasi eksklusi completed IDs | Soal yang telah dikerjakan tidak muncul kembali di batch berikutnya | **PASS** |
| **TC-MISSION-03** | `GameSession` | Siklus `CompletedQuestionIds` | ID tersimpan unik, tidak dobel, dan bersih saat sesi direset | **PASS** |
| **TC-MISSION-04** | `ProceduralAudio` | Pembuatan klip Cozy Fantasy BGM (loop 8 detik) | Klip valid, loopable, frekuensi 22050Hz, dan bebas exception | **PASS** |
| **TC-MISSION-05** | `MissionCardController` | Pemilihan kartu dari deretan misteri | Mengisi objek `SelectedQuestion` secara akurat sesuai kartu yang diklik | **PASS** |
| **TC-MISSION-06** | `MissionCardController` | Klik tombol Continue | Menyimpan soal aktif ke sesi, mencatat completed ID, dan menuju Screen 04 | **PASS** |
| **TC-MISSION-07** | `MissionCardController` | Klik tombol Back to Cards | Menutup modal detail misi dan mengembalikan tampilan ke kartu kipas | **PASS** |
| **TC-MISSION-08** | `MissionCardController` | Eksekusi deal kartu awal | Mengaktifkan formasi kipas dan menandai `HasDealt` bernilai true | **PASS** |
| **TC-MISSION-09** | `MissionCardController` | Eksekusi `OnShuffleClicked` | Memulai urutan kocok, menandai `IsShuffling = true`, dan mengunci interaksi tombol | **PASS** |

### Batch 18: Screen 06 Score Results, Loop Sesi & Credits Scene
- **Tanggal:** 18 Oktober 2026 10:20 WIB
- **Lingkungan Pengujian:** Unity `6000.6.6f1` (Windows 11) / .NET 10.0 / C# 9.0
- **Tipe Uji:** EditMode Automated Unit Tests

| ID Kasus Uji | Modul / Komponen | Skenario yang Diuji | Hasil yang Diharapkan | Status Aktual |
|---|---|---|---|:---:|
| **TC_SRES_01** | `ScoreResultsController` | Kalkulasi persentase skor DEC-09 | Pembagian tepat dan pembulatan bulat (`Round((C/T)*100)`) | **PASS** |
| **TC_SRES_02** | `ScoreResultsController` | Rincian breakdown statistik | Jawaban benar + salah persis sama dengan total dijawab | **PASS** |
| **TC_SRES_03** | `ScoreResultsController` | Format waktu sesi (detik ke MM:SS) | Menghasilkan string waktu 2 digit yang rapi (`02:35`, `00:45`) | **PASS** |
| **TC_SRES_04** | `ScoreResultsController` | Feedback Tier Tinggi (Skor >= 80) | Headline "Very good!" & subtitle bernada pujian "Outstanding" | **PASS** |
| **TC_SRES_05** | `ScoreResultsController` | Feedback Tier Sedang (Skor 60-79) | Headline "Good job!" & subtitle "Keep practicing" | **PASS** |
| **TC_SRES_06** | `ScoreResultsController` | Feedback Tier Rendah (Skor < 60) | Headline "Nice effort!" & subtitle dorongan "Don't give up" | **PASS** |
| **TC_SRES_07** | `ScoreResultsController` | Percabangan tombol: < 10 soal | Mengonfigurasi tombol "CONTINUE QUESTION" untuk menambah poin | **PASS** |
| **TC_SRES_08** | `ScoreResultsController` | Percabangan tombol: >= 10 soal | Batas DEC-08 terpenuhi, tombol menjadi "PLAY AGAIN" | **PASS** |
| **TC_SRES_09** | `ScoreResultsController` | Atribusi formal Credits modal DEC-15 | Memvalidasi 4 entitas resmi (Yogi Ario, Nisa Aprilia Wati, Dr. Resty Wulanningrum, UNP Kediri) | **PASS** |
| **TC_SRES_10** | `ScoreResultsController` | Penutupan Credits & Reset State | Sesi dibersihkan dan siap kembali ke Splash Screen | **PASS** |

### Batch 32: Dual Clear & Delete Buttons Layar 04 (Catatan Revisi Butir 7)
- **Tanggal:** 23 Oktober 2026 14:00 WIB
- **Lingkungan Pengujian:** Unity `6000.6.6f1` (Windows 11) / .NET 8.0 SDK / C# 9.0
- **Tipe Uji:** EditMode Automated Unit Tests & Standalone CLI Test Runners

| ID Kasus Uji | Modul / Komponen | Skenario yang Diuji | Hasil yang Diharapkan | Status Aktual |
|---|---|---|---|:---:|
| **TC_BOARD_14** | `AlgebraBoardController` | Menghapus kartu satu per satu (`OnDeleteOneClicked`) | Hanya slot terisi paling kanan (indeks tertinggi) yang dikosongkan berturut-turut; slot di sebelah kiri tetap utuh; aman dipanggil saat papan kosong tanpa exception | **PASS** |
| **TC_BOARD_15** | `AlgebraBoardController` | Menghapus seluruh kartu aktif (`OnClearClicked`) | Seluruh slot pada baris aktif langsung dikosongkan seketika; slot pada baris lain tidak terganggu | **PASS** |

### Batch 33: Integrasi Universal Runtime & UI Prosedural Easter Egg AI Chatbot Companion
- **Tanggal:** 23 Oktober 2026 14:35 WIB
- **Lingkungan Pengujian:** Unity `6000.6.6f1` (Windows 11) / .NET 8.0 SDK / C# 9.0
- **Tipe Uji:** EditMode Automated Unit Tests & Standalone CLI Test Runners

| ID Kasus Uji | Modul / Komponen | Skenario yang Diuji | Hasil yang Diharapkan | Status Aktual |
|---|---|---|---|:---:|
| **TC_CHAT_01** | `LogoLongPressTrigger` | Tahan pointer sentuh >= 3.0 detik | Event `OnUniversalChatbotTriggered` berhasil memicu dan `CurrentHoldTime` reset | **PASS** |
| **TC_CHAT_02** | `LogoLongPressTrigger` | Lepas sentuhan lebih awal (< 3.0 detik) | Timer batal/reset, event chatbot tidak terpicu, klik biasa beroperasi normal | **PASS** |
| **TC_CHAT_03** | `GroqApiClient` | Struktur serialisasi payload JSON Groq API | Menyertakan model `llama-3.3-70b-versatile`, temperature `0.6`, dan array pesan | **PASS** |
| **TC_CHAT_04** | `ChatSessionManager` | Rolling window kapasitas memori percakapan | Membatasi percakapan maksimal 6 pesan tanpa kehilangan system prompt persona | **PASS** |
| **TC_CHAT_05** | `ChatSessionManager` | Graceful Offline Fallback pedagogis | Menyajikan tips pindah ruas / variabel tanpa crash saat internet terputus | **PASS** |
| **TC_CHAT_06** | `GroqConfigManager` | Pembacaan konfigurasi kredensial Groq API | Memuat API Key berawalan `gsk_` dari JSON terenkripsi/terlindungi `.gitignore` | **PASS** |
| **TC_CHAT_07** | `ChatbotRuntimeBootstrap` | Deteksi objek Logo di seluruh 6 scene aktif | Mengenali nama-nama logo resmi (`Logo`, `LogoImage`, `LogoPlaque`, `TOP_LOGO_TITLE`) dan menautkan trigger otomatis | **PASS** |
| **TC_CHAT_08** | `ChatbotUIController` | Konstruksi UI prosedural & status IsChatOpen | Membuka dan menutup modal obrolan secara mulus dengan transisi state IsChatOpen yang akurat | **PASS** |

### Batch 34: Resolusi Dobel Logo Bertumpuk pada Layar 01 Splash Screen
- **Tanggal:** 23 Oktober 2026 14:50 WIB
- **Lingkungan Pengujian:** Unity `6000.6.6f1` (Windows 11) / .NET 8.0 SDK / C# 9.0
- **Tipe Uji:** EditMode Automated Unit Tests & Standalone CLI Test Runners

| ID Kasus Uji | Modul / Komponen | Skenario yang Diuji | Hasil yang Diharapkan | Status Aktual |
|---|---|---|---|:---:|
| **TC_SPLASH_01** | `01_SplashScreen.unity` | Eliminasi logo visual bertumpuk ganda | Komponen `Image` objek `Logo` memiliki `m_Sprite: null` dan `m_Color.a = 0`, tidak merender gambar kedua di atas `splash_bg.png` | **PASS** |
| **TC_SPLASH_02** | `LogoLongPressTrigger` | Retensi Easter Egg touch target Splash Screen | Objek `Logo` tetap aktif dengan `raycastTarget: true` (860x287 px), klik dan tahan 3 detik tetap memicu modal obrolan AI | **PASS** |

### Batch 35: Integrasi Soal Cerita Kontekstual, Petunjuk Variabel Ramah Anak, & Proteksi Frame Layar 04
- **Tanggal:** 23 Oktober 2026 15:20 WIB
- **Lingkungan Pengujian:** Unity `6000.6.6f1` (Windows 11) / .NET 8.0 SDK / C# 9.0
- **Tipe Uji:** EditMode Automated Unit Tests & Standalone CLI Test Runners

| ID Kasus Uji | Modul / Komponen | Skenario yang Diuji | Hasil yang Diharapkan | Status Aktual |
|---|---|---|---|:---:|
| **TC_BOARD_16** | `AlgebraBoardController` | Format soal cerita naratif & petunjuk variabel konkret | Menampilkan soal cerita asli, menyisipkan petunjuk variabel ramah anak (contoh: $b = \text{banyak boneka}$, $a = \text{buah anggur}$), dan mengaktifkan UGUI BestFit (13–18px) | **PASS** |

### Batch 36: Integrasi Pembahasan Langkah-demi-Langkah Solusi Aljabar Asli Layar 05
- **Tanggal:** 23 Oktober 2026 15:45 WIB
- **Lingkungan Pengujian:** Unity `6000.6.6f1` (Windows 11) / .NET 8.0 SDK / C# 9.0
- **Tipe Uji:** EditMode Automated Unit Tests & Standalone CLI Test Runners

| ID Kasus Uji | Modul / Komponen | Skenario yang Diuji | Hasil yang Diharapkan | Status Aktual |
|---|---|---|---|:---:|
| **TC_TF_11** | `TrueFalseController` | Integrasi langkah solusi aljabar nyata (`solution_steps`) | Memetakan formula awal (`Equation`), operasi invers / pindah ruas (`Operation`), dan nilai solusi akhir (`Solution`) dari `banksoal.json` secara akurat | **PASS** |

### Batch 37: Kalibrasi Tata Letak Vertikal Tombol Hapus Layar 04
- **Tanggal:** 23 Oktober 2026 16:00 WIB
- **Lingkungan Pengujian:** Unity `6000.6.6f1` (Windows 11) / .NET 8.0 SDK / C# 9.0
- **Tipe Uji:** EditMode Automated Unit Tests & Standalone CLI Test Runners

| ID Kasus Uji | Modul / Komponen | Skenario yang Diuji | Hasil yang Diharapkan | Status Aktual |
|---|---|---|---|:---:|
| **TC_BOARD_14** | `AlgebraBoardController` | Eksekusi tombol Hapus 1 Kartu (`DeleteOneButton`) | Menghapus kartu terisi paling kanan di posisi baru $y = -105$ tanpa gangguan visual ke kartu operator | **PASS** |
| **TC_BOARD_15** | `AlgebraBoardController` | Eksekusi tombol Hapus Semua (`ClearAllButton`) | Mengosongkan seluruh slot aktif di posisi baru $y = -157$ dengan layout proporsional | **PASS** |

---

## 📋 Ringkasan Hasil Pengujian Terakhir (Batch 37)

- **Total Test Suite Mandiri Proyek:** 5 Runner
  - `BoardTestRunner`: **16/16 PASS** (`TC_BOARD_01` s.d. `TC_BOARD_16`)
  - `TFTestRunner`: **11/11 PASS** (`TC_TF_01` s.d. `TC_TF_11`)
  - `ChatbotTestRunner`: **8/8 PASS** (`TC_CHAT_01` s.d. `TC_CHAT_08`)
  - `MissionTestRunner`: **14/14 PASS** (`TC_MISSION_01` s.d. `TC_MISSION_14`)
  - `ScoreTestRunner`: **10/10 PASS** (`TC_SRES_01` s.d. `TC_SRES_10`)
- **Total Kasus Uji Terverifikasi:** 59 Test
- **Lulus (PASS):** 59 (100%)
- **Gagal (FAIL):** 0
- **Error:** 0
- **Tingkat Kelulusan:** **100% PASS (0 GAGAL / 0 ERROR)** (**Pilar 6**)



