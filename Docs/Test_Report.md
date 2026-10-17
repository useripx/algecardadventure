# Test Report — AlgeCard Adventure

> **STATUS:** Laporan Verifikasi Pengujian Nyata  
> **PRINSIP:** *Jangan Pernah Bilang Selesai Jika Belum Terbukti Lulus Uji* (**Pilar 6**)  
> **TARGET KEBERHASILAN:** 100% PASS (0 GAGAL / 0 ERROR)  

---

## 🧪 Riwayat Eksekusi Pengujian

### Batch 1: Setup Workspace, Git, Dokumentasi Baseline, & Data Layer Foundation
- **Tanggal:** 17 Oktober 2026
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

---

## 📋 Ringkasan Hasil Pengujian Terakhir

- **Total Kasus Uji:** 12
- **Lulus (PASS):** 12 (100%)
- **Gagal (FAIL):** 0
- **Error:** 0
- **Tingkat Kelulusan:** **100% PASS (0 GAGAL / 0 ERROR)** (**Pilar 6**)
