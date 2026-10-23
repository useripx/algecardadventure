# Panduan Lengkap Ekspor Proyek ke APK Android — AlgeCard Adventure

> **Target Pembaca:** Yogi Ario (Lead Developer & Project Owner)  
> **Versi Unity:** Unity 6 (`6000.6.6f1`) Universal 2D  
> **Target OS:** Android 8.0 (Oreo / API Level 26) ke atas  
> **Format File Output:** Standalone APK (`.apk`) untuk instalasi langsung di smartphone Android  
> **Tujuan:** Pengujian langsung (Playtest Manual) pada perangkat fisik sebelum memasuki Phase 9 (Final Polish & Production Release).

---

## 📑 Daftar Isi
1. [Prasyarat & Persiapan Modul Android](#1-prasyarat--persiapan-modul-android)
2. [Membuka Proyek & Konfigurasi Build Settings](#2-membuka-proyek--konfigurasi-build-settings)
3. [Konfigurasi Player Settings Khusus Android](#3-konfigurasi-player-settings-khusus-android)
4. [Langkah Ekspor Build APK](#4-langkah-ekspor-build-apk)
5. [Cara Memasang (Install) APK di Smartphone Android](#5-cara-memasang-install-apk-di-smartphone-android)
6. [Checklist Pengujian Playtest di HP](#6-checklist-pengujian-playtest-di-hp)
7. [Solusi Kendala Umum (Troubleshooting)](#7-solusi-kendala-umum-troubleshooting)

---

## 1. Prasyarat & Persiapan Modul Android

Sebelum melakukan ekspor di Unity Editor, pastikan modul **Android Build Support** sudah aktif di Unity Hub:

1. Buka **Unity Hub**.
2. Masuk ke tab **Installs**.
3. Cari versi **Unity 6000.6.6f1**, klik ikon gerigi ⚙️ (atau titik tiga) lalu pilih **Add Modules**.
4. Pastikan komponen berikut tercentang:
   - ✅ **Android Build Support**
   - ✅ **Android SDK & NDK Tools**
   - ✅ **OpenJDK**
5. *(Catatan Teknis Proyek: Modul AndroidPlayer, OpenJDK, Android SDK, dan NDK bawaan Unity 6 telah terverifikasi terpasang di sistem komputer Anda).*

---

## 2. Membuka Proyek & Konfigurasi Build Settings

1. Buka proyek **AlgeCard Adventure** melalui Unity Hub menggunakan editor **Unity 6000.6.6f1**.
2. Pada menu atas Unity Editor, pilih menu:  
   **File** ➔ **Build Settings...** (atau tekan pintasan `Ctrl + Shift + B`).
3. Pada jendela **Build Settings**:
   - Di daftar **Platform** (kolom kiri), pilih **Android**.
   - Jika tombol di pojok kanan bawah bertuliskan **Switch Platform**, klik tombol tersebut dan tunggu proses kompilasi aset selesai (beberapa menit jika baru pertama kali beralih).
   - Setelah beralih, logo Unity akan berpindah ke samping platform **Android**.
4. Periksa daftar **Scenes In Build** di bagian atas. Pastikan 6 adegan utama tercentang dan berurutan sesuai indeks berikut:
   - `[0]` `Assets/AlgeCardAdventure/Scenes/01_SplashScreen.unity`
   - `[1]` `Assets/AlgeCardAdventure/Scenes/02_Level.unity`
   - `[2]` `Assets/AlgeCardAdventure/Scenes/03_MissionCard.unity`
   - `[3]` `Assets/AlgeCardAdventure/Scenes/04_AlgebraCardBoard.unity`
   - `[4]` `Assets/AlgeCardAdventure/Scenes/05_TrueOrFalse.unity`
   - `[5]` `Assets/AlgeCardAdventure/Scenes/06_ScoreResults.unity`
   *(Scene bawaan `SampleScene.unity` pastikan tidak dicentang).*

---

## 3. Konfigurasi Player Settings Khusus Android

Di jendela **Build Settings**, klik tombol **Player Settings...** di pojok kiri bawah. Tab **Project Settings ➔ Player** akan terbuka:

### A. Pengaturan Umum (Bagian Atas)
- **Company Name:** `YogiArio` (atau `AlgeCard`)
- **Product Name:** `AlgeCard Adventure`
- **Version:** `1.0.0`
- **Default Icon:** Pilih logo resmi game jika diinginkan.

### B. Tab Android (Ikon Robot Hijau 🤖)

#### 1. Resolution and Presentation
- **Default Orientation:** Pilih **Portrait** (Sesuai **DEC-02**, orientasi terkunci tegak).
- **Allowed Orientations for Auto Rotation:**
  - Nonaktifkan Landscape Right & Landscape Left.
  - Hanya izinkan Portrait.

#### 2. Other Settings
Gulir ke bawah ke bagian **Other Settings**:
- **Identification:**
  - **Package Name:** `com.yogiario.algecardadventure`  
    *(Format: `com.[NamaPengembang].[NamaGame]`, semua huruf kecil tanpa spasi).*
  - **Version:** `1.0.0`
  - **Bundle Version Code:** `1`
  - **Minimum API Level:** `Android 8.0 'Oreo' (API Level 26)`  
    *(Mendukung lebih dari 95% smartphone Android modern).*
  - **Target API Level:** `Automatic (highest installed)`  
    *(Akan otomatis menyesuaikan dengan SDK Android bawaan Unity 6).*
- **Configuration:**
  - **Scripting Backend:**
    - Pilih **Mono** jika ingin proses build **sangat cepat** khusus untuk uji coba/playtest lokal di HP.
    - Pilih **IL2CPP** jika ingin performa maksimal 64-bit (wajib untuk Google Play Store nantinya, proses build awal membutuhkan waktu kompilasi C++ lebih lama).
  - **Target Architectures:**
    - Jika menggunakan **Mono:** centang **ARMv7**.
    - Jika menggunakan **IL2CPP:** centang **ARM64** (dan **ARMv7** jika ingin mendukung HP Android tipe lama).

#### 3. Publishing Settings (Penandatanganan / Keystore)
- Untuk **Playtest Mandiri / Uji Coba Pribadi**:  
  Anda **TIDAK PERLU** membuat keystore produksi. Biarkan opsi default menggunakan **Debug Keystore** bawaan Unity. File APK tetap dapat langsung diinstal dan dimainkan di HP Anda.

---

## 4. Langkah Ekspor Build APK

1. Kembali ke jendela **Build Settings** (`Ctrl + Shift + B`).
2. **PENTING:** Pastikan opsi **Build App Bundle (Google Play)** dalam keadaan **TIDAK DICENTANG** (unchecked).  
   *(Jika tercentang, file output berupa `.aab` yang tidak bisa langsung diinstal di HP).*
3. Pastikan opsi **Development Build** tidak dicentang (kecuali Anda ingin melihat pesan error/log konsol di layar HP saat testing).
4. Klik tombol **Build** di pojok kanan bawah.
5. Jendela File Explorer akan muncul meminta lokasi penyimpanan file:
   - Buat folder baru di luar folder `Assets`, misalnya: `Builds/Android/`.
   - Beri nama file: `AlgeCardAdventure.apk`.
   - Klik **Save**.
6. Unity akan menjalankan proses **Gradle Build**:
   - Unity mengompilasi skrip C#, memaketkan aset sprite, audio prosedural, JSON bank soal, dan membuat file biner APK.
   - Waktu proses build berkisar antara **2 hingga 8 menit** tergantung spesifikasi komputer.
7. Setelah selesai, jendela folder tempat file `AlgeCardAdventure.apk` berada akan otomatis terbuka.

---

## 5. Cara Memasang (Install) APK di Smartphone Android

Pilih salah satu metode termudah berikut untuk mentransfer dan memasang APK ke HP Anda:

### Metode 1: Menggunakan Kabel Data USB (Paling Direkomendasikan)
1. Hubungkan smartphone Android Anda ke PC/Laptop menggunakan kabel USB.
2. Pada notifikasi koneksi USB di layar HP, pilih mode **Transfer File (MTP)**.
3. Di komputer, salin (Copy) file `AlgeCardAdventure.apk`.
4. Buka penyimpanan internal HP Anda melalui File Explorer PC, lalu tempel (Paste) file APK ke folder yang mudah ditemukan, misalnya folder **Download**.
5. Lepaskan kabel atau tetap terhubung.
6. Di HP Anda, buka aplikasi **File Manager** (atau *Pengelola File*).
7. Masuk ke folder **Download**, lalu ketuk file `AlgeCardAdventure.apk`.
8. Jika muncul peringatan *"Demi keamanan, ponsel Anda tidak diizinkan menginstal aplikasi tidak dikenal dari sumber ini"*:
   - Ketuk **Setelan (Settings)**.
   - Aktifkan toggle **Izinkan dari sumber ini (Allow from this source)**.
   - Tekan tombol kembali (Back).
9. Ketuk **Instal (Install)** dan tunggu proses pemasangan hingga selesai.
10. Ketuk **Buka (Open)** untuk mulai memainkan game!

### Metode 2: Menggunakan Google Drive / Chat Pribadi
1. Unggah file `AlgeCardAdventure.apk` ke akun **Google Drive** Anda (atau kirim melalui Telegram Saved Messages / WhatsApp Web ke nomor sendiri).
2. Di HP Anda, buka Google Drive dan unduh file APK tersebut.
3. Setelah selesai diunduh, ketuk notifikasi unduhan atau buka melalui File Manager.
4. Lakukan instalasi seperti pada langkah Metode 1.

### Metode 3: Melalui Perintah ADB (Bagi Pengembang / USB Debugging)
Jika Anda sudah mengaktifkan **Developer Options** dan **USB Debugging** pada HP:
1. Hubungkan HP ke PC via kabel USB.
2. Buka terminal PowerShell di folder tempat file APK berada.
3. Jalankan perintah:
   ```bash
   adb install -r AlgeCardAdventure.apk
   ```
4. Aplikasi akan langsung terpasang di HP secara otomatis dalam hitungan detik.

> ⚠️ **Catatan Google Play Protect:**  
> Saat pertama kali memasang APK buatan sendiri, Google Play Protect mungkin menampilkan pop-up kuning bertuliskan *"Blocked by Play Protect"* karena aplikasi belum terdaftar di Google Play Store.  
> Cukup ketuk **More details (Rincian lebih lanjut)** lalu pilih **Install anyway (Tetap instal)**.

---

## 6. Checklist Pengujian Playtest di HP

Gunakan daftar periksa berikut saat mencoba game di HP Anda untuk memastikan kualitas dan kenyamanan bermain:

- [ ] **Layar 01 (Splash Screen):**
  - Apakah logo dan latar belakang proporsional dan tidak terpotong oleh poni/punch-hole kamera (Safe Area aktif)?
  - Apakah animasi pulsing *"Tap to Start"* berjalan lembut?
  - Apakah ketukan pertama berhasil membuka layar berikutnya?
- [ ] **Layar 02 (Level Selection):**
  - Apakah ketiga kartu tombol (*Easy*, *Normal*, *Hard*) responsif saat disentuh?
  - Apakah musik santai (*Cozy Fantasy BGM*) dan suara klik tombol (*SFX*) terdengar jernih dari speaker HP?
- [ ] **Layar 03 (Mission Cards):**
  - Apakah formasi kipas 5 kartu tampil tajam (tidak pecah/buram)?
  - Apakah tombol emas *"SHUFFLE HERE"* memicu animasi riffle kocok kartu dengan lancar tanpa lag?
  - Apakah modal kartu terbuka rapi dan teks soal matematika dari `banksoal.json` terbaca nyaman?
- [ ] **Layar 04 (Algebra Card Board):**
  - Apakah slot jawaban dapat diisi dengan mengetuk kartu angka dari sidebar (*Tap-to-Slot*)?
  - Apakah slot yang sudah terisi bisa dikosongkan kembali (*Tap-to-Clear*)?
  - Apakah navigasi D-pad dan tombol *"CLEAR"* berfungsi optimal?
  - Apakah auto-scroll baris aljabar berjalan mulus di layar sentuh?
- [ ] **Layar 05 (True or False):**
  - Apakah badge √ TRUE (hijau) atau ✕ FALSE (merah) tampil otomatis sebagai indikator penilaian (bukan tombol tebakan)?
  - Apakah hitung mundur 3 detik berjalan dan panel pembuktian substitusi penjelasan matematika muncul jelas?
- [ ] **Layar 06 (Score Results):**
  - Apakah persentase skor akhir, 3 kartu statistik (Benar, Salah, Waktu), dan maskot tampil memukau?
  - Apakah tombol *"CONTINUE QUESTION"* atau *"PLAY AGAIN"* bekerja sesuai batas 10 soal (**DEC-08**)?
  - Apakah tombol *"EXIT"* memunculkan dialog modal **Credits & Attribution** resmi yang bersih tanpa tumpang tindih teks:
    - **Yogi Ario** (*Software Developer*)
    - **Nisa Aprilia Wati** (*Game Planner & Math Expert*)
    - **Dr. Resty Wulanningrum, M.Kom.** (*Academic Advisor*)
    - **Universitas Nusantara PGRI Kediri** (*Academic Institution*)
  - Apakah tombol *"CLOSE"* pada Credits mereset sesi dan kembali ke Splash Screen dengan benar?

---

## 7. Solusi Kendala Umum (Troubleshooting)

| Gejala Masalah | Penyebab Utama | Solusi Praktis |
|---|---|---|
| **Build Error: "Android SDK / NDK not found"** | Lokasi SDK/NDK di Preferences terputus atau tidak tercentang. | Masuk ke menu Unity Editor: **Edit ➔ Preferences ➔ External Tools**. Pastikan centang pada *Android SDK Tools Installed with Unity*, *Android NDK*, dan *JDK* dalam keadaan aktif (centang). |
| **Output menghasilkan file `.aab`, bukan `.apk`** | Opsi App Bundle aktif di Build Settings. | Buka **Build Settings** (`Ctrl + Shift + B`), hilangkan tanda centang pada opsi **Build App Bundle (Google Play)**, lalu klik **Build** ulang. |
| **Error saat instalasi di HP: "App not installed / Paket tampaknya korup"** | Arsitektur CPU tidak cocok atau ada versi game lama yang menggunakan signature berbeda. | 1) Hapus (Uninstall) versi lama game dari HP jika pernah terpasang. 2) Di Player Settings, pastikan arsitektur **ARM64** dan **ARMv7** tercentang. |
| **Layar game hitam / "No cameras rendering" di HP** | Konfigurasi kamera URP belum terhubung ke Canvas. | Hal ini sudah diatasi pada scene generator kita dengan menyematkan komponen URP `UniversalAdditionalCameraData` pada Main Camera dan `Canvas.worldCamera`. |
| **Aplikasi langsung force close saat dibuka di HP** | Dependency ECall / metode native tanpa wrapper aman. | Seluruh logika backend controller (`AlgebraBoardController`, `TrueFalseController`, `ScoreResultsController`) telah diproteksi dengan try-catch defensif sehingga aman dijalankan di Android runtime. |

---

*Panduan ini disusun secara khusus untuk repositori **AlgeCard Adventure** oleh AI Agent Partner Yogi Ario sesuai Kerangka Tata Kelola Universal.*
