# Implementation Plan — Batch 15 (Opsi B): Pembersihan Garis Tertutup pada Mission Points & Template Perkamen

> **STATUS:** PENDING APPROVAL (APPROVAL GATE PILAR 1)  
> **TANGGAL:** 18 Oktober 2026  
> **TARGET PENGERJAAN:** Batch 15 (Opsi B) — Asset Template Cleanup  
> **PENGUSUL:** Edward (Pair Programmer)  
> **OTORITAS PERSETUJUAN:** Yogi Ario (Project Owner)  
> **PRINSIP UTAMA:** *No Plan, No Code. No Approval, No Code.*

---

## 1. 🎯 Tujuan & Ruang Lingkup

### A. Tujuan
1. **Menghilangkan Garis Putih & Residu Angka pada Mission Points:**  
   Membersihkan artefak garis-garis vertikal putih (menyerupai *barcode* / pagar) dan residu potongan angka kuning yang menempel pada pita biru `MISSION POINTS:` di berkas templat kartu `Assets/AlgeCardAdventure/Art/Cards/mission_card_front_clean.png`.
2. **Pembersihan Residu Teks Perkamen (Opsi B):**  
   Menghapus teks cetak bawaan desainer (`Question:`) di bagian tengah perkamen agar background kartu menjadi 100% mulus polos tanpa bayangan teks mockup yang bertabrakan dengan teks soal dinamis Unity.

---

## 2. 🔍 Investigasi Mendalam Akar Masalah (Root Cause)

Berdasarkan inspeksi forensik piksel pada `Assets/AlgeCardAdventure/Art/Cards/mission_card_front_clean.png`:

### A. Masalah Garis pada Mission Points
Saat pembuatan templat kartu awal pada Batch 9, terjadi ketidaksempurnaan saat menutup angka 100 statis:
- Di sebelah kanan teks `MISSION POINTS:` (mulai dari koordinat `x = 535` s.d. `785`, pada pita biru `y = 1112` s.d. `1176`), terdapat barisan artefak garis vertikal putih `||||||||||||||||||||||||||||||||||||||||||||` dan sisa potongan angka kuning `100` di bawahnya.
- Akibatnya, saat Unity merender komponen `MissionPointsText` (teks poin dinamis kuning), angka poin tersebut bertabrakan langsung dengan garis-garis putih ini sehingga terlihat kotor dan terpotong.

### B. Masalah Teks "Question:" di Perkamen
- Pada koordinat `x = 374..511, y = 498..523` di tengah perkamen, terdapat kata **`Question:`** yang tercetak permanen di gambar latar.
- Saat Unity merender narasi soal dinamis (`MissionInstructionText`), teks soal dari `banksoal.json` menimpa kata `Question:` ini sehingga teks terlihat ganda / berbayang (*ghost text*).

---

## 3. 🖼️ Bukti Visual Perbandingan (Before vs After)

### Sebelum Pembersihan (Kondisi Saat Ini):
![Garis Barcode & Residu Angka pada Mission Points](C:\Users\yogia\.gemini\antigravity-ide\brain\ef22e4ee-b939-400f-aed2-b9272c15ecf7\banner_detail_before.png)
*(Terdapat garis-garis vertikal putih dan potongan angka kuning di samping teks MISSION POINTS:)*

---

### Sesudah Pembersihan (Hasil Uji Coba Presisi Edward):
![Hasil Pembersihan Mulus](C:\Users\yogia\.gemini\antigravity-ide\brain\ef22e4ee-b939-400f-aed2-b9272c15ecf7\perfect_clean_preview.png)
*(Pita biru MISSION POINTS: bersih total dan mulus; kata 'Question:' di perkamen terhapus 100% tanpa bekas)*

---

## 4. 💡 Rincian Solusi Teknis & Algoritma Pembersihan

Edward akan mengeksekusi skrip pembersih grafis terkalibrasi (`scratch/apply_clean_card_template.py`) untuk memproses berkas kerja `Assets/AlgeCardAdventure/Art/Cards/mission_card_front_clean.png`:

1. **Pita Biru `MISSION POINTS:`:**
   - Menjaga teks putih `MISSION POINTS:` dan titik dua (`:`) pada `x = 160..514` tetap 100% tajam dan utuh.
   - Mengambil profil gradien vertikal warna biru navy murni (`#00245E`) dari sisi kiri ribbon (`x = 170..210`).
   - Melakukan *in-painting* halus pada rentang `x = 525..785` sehingga area angka poin menjadi latar biru polos yang sempurna.
2. **Area Perkamen Tengah:**
   - Menghapus cetakan teks `Question:` pada rentang `x = 355..535, y = 490..535`.
   - Mengisi area tersebut dengan interpolasi gradien warna perkamen asli (`#FEF7E9` $\rightarrow$ `#FCF4DE`) dari piksel sekitarnya, menghilangkan seluruh jejak teks mockup tanpa merusak tekstur perkamen maupun ikon di pinggir (jam, kalender, not musik).
3. **Pita Oranye `DIFFICULTY LEVEL:`:**
   - Sesuai konfirmasi Mas Yogi (*"untuk Difficulty level sudah pas"*), area ini dipertahankan utuh dan stabil.

---

## 5. 📁 File Target yang Terpengaruh

| File | Status | Keterangan |
|---|---|---|
| `Assets/AlgeCardAdventure/Art/Cards/mission_card_front_clean.png` | **MODIFIKASI ASET KERJA** | Diperbarui dengan hasil pembersihan pita biru dan perkamen. |
| `Docs/Task_Status.md` | **UPDATE** | Mencatat penyelesaian Batch 15 (Opsi B). |
| `Docs/Decision_Log.md` | **UPDATE** | Mencatat keputusan arsitektur `DEC-28`. |
| `changelog/18/10/2026/<jam>/<menit>.md` | **BUAT BARU** | Catatan rilis bertimestamp lokal. |

> [!IMPORTANT]
> **Proteksi Pilar 4:** Folder referensi asli desainer [Docs/Ui Template/](file:///c:/Users/yogia/AlgeCard%20Adventure/Docs/Ui%20Template) tetap terlindungi penuh dan berstatus **READ-ONLY**. Hanya berkas aset kerja di `Assets/` yang diperbarui.

---

## 6. 🧪 Rencana Pengujian & Kriteria Keberhasilan (Pilar 6)

1. **Verifikasi Visual Template Kartu:**
   - Memastikan tidak ada lagi garis putih barcode maupun residu kuning di sebelah kanan `MISSION POINTS:`.
   - Memastikan titik dua (`:`) pada `MISSION POINTS:` tetap terlihat jelas dan tajam.
   - Memastikan perkamen bebas dari kata `Question:`.
2. **Verifikasi Render Unity Game View:**
   - Membuka dan merender kartu misi dengan angka poin dinamis (`100`, `200`, `300`) dari `MissionCardController`.
   - Angka poin kuning tampil kontras, bersih, dan tajam di atas latar biru navy polos.
3. **Kompilasi & Test Suite Otomatis:**
   - `dotnet build AlgeCardAdventure.Tests.csproj` menghasilkan **0 Warning, 0 Error**.
   - Seluruh unit tests EditMode tetap **100% PASS**.

---

## 7. 🛑 Gerbang Persetujuan (Approval Gate)

Sesuai **Pilar 1 Universal Governance**:
> *Edward tidak akan memodifikasi file aset kerja `mission_card_front_clean.png` sebelum rencana kerja tertulis ini disetujui secara eksplisit oleh Mas Yogi Ario.*

Silakan periksa rencana dan perbandingan visual di atas. Jika Mas Yogi menyetujuinya, silakan berikan konfirmasi:  
👉 **"Setuju"** atau **"Lanjut"**, dan Edward akan segera mengeksekusi pembersihan ini secara presisi!
