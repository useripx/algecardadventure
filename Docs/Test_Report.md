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

---

## 📋 Ringkasan Hasil Pengujian Terakhir

- **Total Kasus Uji:** 28
- **Lulus (PASS):** 28 (100%)
- **Gagal (FAIL):** 0
- **Error:** 0
- **Tingkat Kelulusan:** **100% PASS (0 GAGAL / 0 ERROR)** (**Pilar 6**)
