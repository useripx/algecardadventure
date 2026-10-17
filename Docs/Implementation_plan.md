# Implementation Plan — AlgeCard Adventure (Unity Android)

**Versi:** 1.1-draft  
**Tanggal:** 17 Oktober 2026  
**Status:** Draft untuk ditinjau dan disetujui pemilik proyek; implementasi belum diizinkan oleh dokumen ini saja.  
**Pemilik proyek:** Yogi Ario  
**Engine:** Unity 6000.6.6f1, Universal 2D (berdasarkan informasi terakhir; agen wajib memverifikasi di workspace)  
**Platform target:** Android, portrait, single-player, offline  
**Repositori GitHub:** https://github.com/useripx/algecardadventure.git

> **Approval gate:** dokumen ini adalah rencana kerja, bukan izin untuk langsung mengubah kode, scene, aset, konfigurasi, atau Git. Agen harus berhenti dan menunggu persetujuan eksplisit Yogi sebelum memulai perubahan implementasi.

> **Petunjuk Khusus Github:** Tolong buatkam README>md untuk githubnya dengan sayarat gunakan logo di folder C:\Users\yogia\AlgeCard Adventure\Docs\Ui Template\logo.png\screen.png untuk readmenya diatas sendiri lokasi logonya dan README wajib ada Badge terkait yang ada di projek ini apa saja yang ada serta badge ukuran repo wajib ada

---

## 1. Tujuan Produk

Membangun **AlgeCard Adventure**, game edukasi aljabar berbasis kartu untuk perangkat Android. Game memakai tampilan 2D portrait dengan dunia kartun fantasi, panel kayu/perkamen, kartu angka dan variabel, serta maskot jangkrik hijau dan oranye.

Pemain memilih level, mengambil satu kartu misi dari lima kartu tertutup, mengerjakan misi melalui papan kartu aljabar, menjawab True/False, melihat penjelasan, lalu melihat hasil skor. Satu sesi maksimal mencatat 10 soal yang dijawab.

### Batasan MVP

- Offline dan single-player; tidak membutuhkan akun, backend, server, leaderboard online, atau sinkronisasi cloud.
- Bahasa seluruh UI pemain: **English**.
- Level UI: **Easy, Normal, Hard**.
- Alur enam kelompok layar, dari Splash Screen hingga Score Results.
- Skor sesi bersifat kumulatif dan berbasis persentase jawaban benar.
- Tidak menambahkan shop, iklan, in-app purchase, multiplayer, fitur guru, atau fitur lain di luar MVP tanpa persetujuan.

## 2. Sumber Kebenaran dan Aturan Konflik

Gunakan prioritas berikut saat menemukan perbedaan:

1. Keputusan terbaru yang disetujui pemilik proyek.
2. Implementation Plan ini setelah disetujui.
3. `banksoal.json` untuk teks soal, model persamaan, langkah solusi, jawaban, level, ID, dan status tinjauan.
4. Gambar rancangan UI/prototipe sebagai referensi visual; HTML/CSS/JavaScript bukan kode produksi Unity.
5. Dokumen dan instruksi agen lain yang tidak bertentangan dengan sumber di atas.

Jika sumber berbeda atau sebuah detail belum ada, agen tidak boleh mengarang atau mengoreksinya secara diam-diam. Catat masalahnya, jelaskan opsi dan dampaknya, lalu minta keputusan.

## 3. Keputusan yang Sudah Ditetapkan

| ID | Keputusan | Ketentuan untuk agen |
|---|---|---|
| DEC-01 | Platform Unity Android | Gunakan Unity/C#; jangan memindahkan proyek ke framework lain. |
| DEC-02 | Orientasi portrait | Uji Canvas scaling, safe area, dan keterbacaan pada rasio layar ponsel yang berbeda. |
| DEC-03 | Tiga label level | UI menampilkan tepat `Easy`, `Normal`, `Hard`. Jangan tampilkan `Medium` di UI. |
| DEC-04 | Pemetaan bank soal | `Medium` dari sumber dipetakan ke UI `Normal`; ID soal tetap stabil. |
| DEC-05 | Sumber soal | `banksoal.json` adalah sumber utama isi bank soal. Jangan menanamkan teks soal satu per satu di script atau scene. |
| DEC-06 | Bahasa UI | Semua teks yang dilihat pemain menggunakan bahasa Inggris. Dokumen internal boleh berbahasa Indonesia. |
| DEC-07 | Kartu misi | Satu batch menampilkan hingga lima kartu misi tertutup; pilih satu kartu untuk membuka misi. Jangan mengganti soal terpilih saat berpindah layar. |
| DEC-08 | Batas sesi | Maksimal 10 soal yang dijawab per sesi. Tahap-tahap papan aljabar tidak dihitung sebagai soal terpisah. |
| DEC-09 | Skor | `score = (correctAnswers / questionsAnswered) * 100`, ditampilkan sebagai bilangan bulat. Jawaban salah tetap dihitung sebagai soal yang dijawab. |
| DEC-10 | Anti-duplikasi | Jangan menawarkan kembali soal yang sudah selesai dalam sesi yang sama. Jangan menggandakan soal hanya untuk memenuhi jumlah kartu. |
| DEC-11 | Feedback True/False | Pilihan pemain dikunci setelah satu kali submit; tampilkan feedback selama 3 detik, kemudian tampilkan penjelasan. |
| DEC-12 | Keluar sesi | Exit memerlukan konfirmasi; setelah dikonfirmasi, sesi aktif direset dan kembali ke Splash Screen. |
| DEC-13 | Review UI | Kerjakan satu layar pada satu waktu. Tunggu persetujuan eksplisit sebelum memulai layar bernomor berikutnya. |
| DEC-14 | Proteksi proyek | Jangan menghapus atau menimpa scene/aset/proyek sumber; pertahankan `.meta` dan GUID Unity. |

## 4. Keputusan Terbuka — Harus Diselesaikan Sebelum Fitur Terkait

Keputusan berikut belum dapat dianggap selesai hanya dengan menulis kode. Agen harus menyajikan opsi, rekomendasi, dan dampak lalu meminta persetujuan.

### OPEN-01 — Pemilihan topik

**Temuan:** struktur `banksoal.json` mengelompokkan soal berdasarkan `ui_level`, tetapi tidak memiliki `topic_id` atau label topik. Karena itu, UI pemilihan topik tidak dapat difilter secara akurat dari JSON saat ini tanpa membuat klasifikasi tambahan.

- **Opsi A (rekomendasi MVP):** pilih level saja, kemudian lanjut ke Mission Cards; topik spesifik tidak dipilih pada versi pertama.
- **Opsi B:** buat klasifikasi topik untuk setiap soal melalui proses terpisah yang ditinjau pemilik proyek, lalu tambahkan metadata topik ke skema JSON.

Agen tidak boleh mengarang pemetaan topik. Implementasi Screen 02 yang final menunggu keputusan ini.

### OPEN-02 — Pemetaan soal ke empat tahap papan kartu

**Temuan:** `banksoal.json` berisi `equation_model` dan `solution_steps`, tetapi tidak berisi definisi slot/kartu untuk empat tahap papan. Ini belum cukup untuk menjamin drag-and-drop dan validasi kartu yang benar untuk semua 60 soal.

- **Opsi A (lebih terkontrol):** buat data pemetaan papan yang terkurasi dan divalidasi untuk tiap soal yang playable; soal tanpa pemetaan tidak boleh dipilih pada mode papan.
- **Opsi B:** membangun generator dan evaluator ekspresi aljabar yang lebih umum dari `equation_model`/`solution_steps`; cakupan, batasan matematika, dan kompleksitasnya harus disetujui sebelum dikerjakan.

Agen harus membuat proof-of-concept kecil dan menjelaskan cakupan sebelum memilih opsi. Jangan mengklaim parser matematika umum tersedia jika yang dibuat hanya validasi urutan slot.

### OPEN-03 — Konten True/False

**Temuan:** pada `banksoal.json`, `true_false.statement`, `true_false.correct_answer`, dan `true_false.explanation` masih `null` karena tidak tersedia dalam PDF sumber.

Agen tidak boleh menebak kontennya atau menampilkan `null` di UI. Sebelum Screen 05 dihubungkan ke seluruh bank soal, konten True/False harus dibuat sebagai pekerjaan terpisah dan divalidasi secara matematis. Perubahan konten harus tercatat dan disetujui.

### OPEN-04 — Empat soal yang membutuhkan tinjauan sumber

Jangan mengaktifkan soal berstatus berikut pada rilis final sebelum ditinjau:

- `NORMAL-005`, `NORMAL-007`, `NORMAL-020`: `needs_clarification`.
- `HARD-006`: `needs_source_check`.

Pertahankan `review_status` dan `review_note`; jangan mengubah redaksi atau jawaban diam-diam. Saat pengacakan, soal ini harus dilewati untuk mode rilis sampai statusnya disetujui.

---

## 5. Alur Layar

### 01 — Splash Screen

- Tampilkan identitas/logo AlgeCard Adventure dan ilustrasi utama.
- Teks tombol/petunjuk: `Tap anywhere to start`.
- Tap membawa pemain ke Screen 02.
- Jika gambar background sudah mengandung logo, jangan menambahkan logo kedua kali.

### 02 — Level

- Tampilkan `Easy`, `Normal`, `Hard`.
- Simpan level pilihan ke sesi.
- Detail pemilihan topik mengikuti keputusan OPEN-01.
- Aksi berikutnya menuju Mission Cards.

### 03 — Mission Cards

- Tampilkan hingga lima kartu tertutup dari level terpilih.
- Acak kartu dari soal eligible yang belum selesai dalam sesi.
- Kartu yang dipilih dibuka dan menampilkan `title` serta `question_text` berbahasa Inggris.
- Tombol `Continue` membawa `questionId` yang sama ke papan aljabar.
- Selama animasi shuffle, input kartu dinonaktifkan.
- Jika soal eligible kurang dari lima, tampilkan jumlah yang tersedia. Jangan menambahkan duplikat untuk mengisi ruang.
- Jika tidak ada soal playable tersisa, tampilkan keadaan habis soal yang jelas; jangan mengulang soal diam-diam.

### 04 — Algebra Card Board

- Tampilkan teks soal di bagian atas.
- Papan memiliki empat tahap berurutan; jangan menampilkan label internal `4a`, `4b`, `4c`, `4d` pada UI pemain.
- Sediakan Number Cards dan Variable Cards dengan target sentuh yang nyaman. Pada layar sempit, gunakan tray yang lebih ringkas atau dapat dibuka/tutup; jangan mengecilkan kartu hingga sulit dibaca.
- Pemain dapat meletakkan kartu ke slot, menghapus/mengganti kartu, dan menggunakan `Continue`/`Review` sesuai rancangan yang disetujui.
- Tahap yang tidak valid tetap terbuka dan memberikan petunjuk singkat dalam bahasa Inggris.
- `Review` kembali ke tahap sebelumnya tanpa menghapus penempatan valid secara tak terduga.
- Tahap-tahap board bukan soal terpisah dan tidak memengaruhi skor.
- Implementasi yang berlaku untuk satu soal harus memakai mapping papan yang terverifikasi; jangan menganggap `solution_steps` otomatis dapat diubah menjadi slot kartu tanpa validasi.

### 05 — True or False

Terdiri dari tiga keadaan runtime, bukan tiga panel yang tampil bersamaan:

1. Statement: tampilkan satu pernyataan dengan tombol `True` dan `False`.
2. Feedback: kunci jawaban yang dikirim, lalu tampilkan feedback benar atau salah selama 3 detik.
3. Explanation: tampilkan langkah alasan, hasil evaluasi, dan kesimpulan. Tombol `Continue` melanjutkan ke Score Results.

Screen 05 harus tetap diblokir dari bank soal yang belum memiliki True/False terverifikasi. Jangan membuat pernyataan dari `null` saat runtime.

### 06 — Score Results

- Tampilkan skor sesi, jumlah benar, jumlah salah, jumlah soal dijawab, dan waktu sesi jika timer benar-benar diimplementasikan.
- Jangan gunakan angka placeholder seperti `4/5` atau waktu statis pada build produksi.
- `Continue` mempertahankan semua hitungan sesi dan kembali ke Mission Cards selama jumlah soal dijawab kurang dari 10.
- Setelah soal ke-10, akhiri sesi dan jangan memulai soal ke-11.
- `Exit` menampilkan konfirmasi; `Cancel` kembali ke hasil, sedangkan konfirmasi menghapus sesi aktif dan kembali ke Splash Screen.

---

## 6. Spesifikasi `banksoal.json`

### Sumber dan lokasi runtime

- File sumber yang disediakan: `banksoal.json`.
- Saat disetujui untuk integrasi, salin versi yang telah ditinjau ke `Assets/StreamingAssets/banksoal.json` sebagai data yang dibundel bersama game.
- Pastikan tidak ada dua salinan yang diedit secara independen. Jika salinan kerja dan salinan runtime berbeda, dokumentasikan proses sinkronisasinya.
- Tidak boleh ada kebutuhan internet untuk membaca bank soal.
- Di Android, baca `StreamingAssets` dengan pendekatan yang kompatibel dengan platform (misalnya `UnityWebRequest` jika path berupa URL/arsip APK); jangan mengasumsikan `File.ReadAllText` selalu bekerja pada Android.

### Struktur yang tersedia sekarang

- `schema_version`, `title`, `language`, `source`, `game_configuration`, `levels`.
- `levels[]` berisi `source_level`, `ui_level`, `questions[]`.
- Tiap soal meliputi `id`, `source_number`, `source_level`, `ui_level`, `title`, `question_text`, `equation_model[]`, `solution_steps[]`, `final_answer`, `answer_summary`, `review_status`, `review_note`, dan `true_false`.
- Jumlah data: 60 soal; 20 Easy, 20 Normal (dipetakan dari Medium sumber), 20 Hard.

### Ketentuan integrasi

1. Gunakan ID stabil, misalnya `EASY-001`, `NORMAL-001`, `HARD-001`; jangan mengubahnya untuk mengurutkan tampilan.
2. Pilih berdasarkan `ui_level` yang cocok dengan level UI aktif.
3. Simpan satu `QuestionRepository` atau layanan sejenis sebagai satu-satunya pintu akses ke bank soal. UI tidak boleh melakukan parsing JSON sendiri.
4. Validasi JSON saat dimuat: keberadaan level, ID unik, teks yang wajib ada, status review, dan nilai nullable.
5. Tangani JSON gagal dibaca dengan pesan error yang dapat ditindaklanjuti; jangan membuat soal palsu sebagai fallback.
6. Abaikan soal non-approved dengan `review_status` `needs_clarification` atau `needs_source_check` pada pemilihan soal rilis.
7. `equation_model` adalah array; satu soal dapat memuat lebih dari satu persamaan. Jangan memaksakan semua soal menjadi satu persamaan atau satu jawaban string.
8. Jangan mengubah bahasa sumber Inggris atau nilai matematis tanpa persetujuan.
9. True/False yang masih `null` harus ditangani sebagai konten yang belum tersedia, bukan sebagai jawaban `False`.
10. Tambahkan test yang membaca file JSON aktual dan memverifikasi jumlah, ID, pemetaan level, serta status review.

### Aturan pengacakan

- Pilih hanya soal yang sesuai dengan level aktif, belum dijawab pada sesi saat ini, dan telah disetujui untuk dimainkan.
- Buat batch berisi maksimum lima soal unik. Shuffle hanya kandidat eligible.
- Pertahankan soal terpilih yang sedang aktif sampai selesai atau sesi dibatalkan secara eksplisit.
- Jika pool playable tidak cukup untuk mencapai 10 soal, jelaskan kondisi tersebut dan minta pemilik proyek menentukan kebijakan berikutnya; jangan menggandakan soal.

---

## 7. Aturan Sesi dan Skor

Gunakan satu objek sesi sebagai sumber kebenaran. Data minimum:

- `selectedLevel`
- `currentQuestionId`
- `completedQuestionIds`
- `questionsAnswered`
- `correctAnswers`
- `wrongAnswers`
- `score`
- `sessionStartTime` / `elapsedTime` jika timer disetujui
- `currentBoardStage`
- `hasSubmittedTrueFalse`
- `isSessionFinished`

Formula:

```text
score = questionsAnswered == 0
    ? 0
    : Round((correctAnswers / questionsAnswered) * 100)
```

Ketentuan:

- Jawaban True/False final adalah momen soal dihitung.
- Jawaban benar menambah `correctAnswers` dan `questionsAnswered`.
- Jawaban salah menambah `wrongAnswers` dan `questionsAnswered`.
- Jawaban yang sama tidak boleh terhitung dua kali meskipun tombol ditekan berulang.
- Skor dapat ditampilkan `0` saat belum ada soal dijawab; jangan membagi dengan nol.
- Lanjut ke soal berikutnya tidak mereset skor, waktu sesi, atau ID selesai.
- Maksimum 10 soal per sesi.
- Keluar dan konfirmasi mengakhiri sesi aktif.

---

## 8. Visual dan Aset

- Arah visual: 2D kartun cerah, petualangan fantasi, panel kayu/perkamen, dekorasi daun, lingkungan hutan/kastil, kartu aljabar, maskot jangkrik hijau dan oranye.
- Konsistenkan warna, outline, tipografi, bentuk tombol, skala maskot, dan gaya panel pada seluruh layar.
- Gunakan referensi yang sudah diberikan, termasuk gambar `01 Splash Screen.png` sampai `06 Score Results.png`, bila tersedia di workspace agen.
- Sebelum mengimpor, inventarisasi aset dan periksa bahwa file benar-benar tersedia. Jangan menganggap file lokal sesi ChatGPT otomatis ada di direktori Unity.
- Jangan mengubah atau menimpa sumber gambar asli. Salin ke folder kerja bila diperlukan.
- Gunakan aset/font lokal di build offline; jangan mengandalkan CDN, Google Fonts runtime, URL gambar, atau layanan web.
- Pertahankan rasio logo; jangan menaruh logo dua kali di atas gambar yang sudah mengandungnya.
- Gunakan TextMeshPro jika sesuai dengan konfigurasi proyek yang sudah ada.
- Untuk Canvas gunakan `Scale With Screen Size` dan safe-area handling. Target resolusi referensi dapat ditetapkan setelah memeriksa mockup; verifikasi di beberapa rasio layar portrait.

---

## 9. Arsitektur dan Struktur Target

Agen harus memeriksa struktur proyek Unity aktual terlebih dahulu. Gunakan folder/konvensi yang sudah ada bila layak; jangan memindahkan atau menggandakan folder hanya karena struktur berikut berbeda.

Struktur yang disarankan jika belum ada konvensi setara:

```text
Assets/
└── AlgeCardAdventure/
    ├── Art/
    │   ├── Backgrounds/
    │   ├── Cards/
    │   ├── Mascots/
    │   ├── Logo/
    │   └── UI/
    ├── Audio/
    ├── Fonts/
    ├── Prefabs/
    ├── Scenes/
    │   ├── 01_SplashScreen.unity
    │   ├── 02_Level.unity
    │   ├── 03_MissionCards.unity
    │   ├── 04_AlgebraCardBoard.unity
    │   ├── 05_TrueOrFalse.unity
    │   └── 06_ScoreResults.unity
    ├── Scripts/
    │   ├── Core/
    │   ├── Data/
    │   ├── Gameplay/
    │   ├── Session/
    │   └── UI/
    └── Tests/
        ├── EditMode/
        └── PlayMode/
Assets/StreamingAssets/banksoal.json
Docs/
├── Implementation_plan.md
├── Task_Status.md
├── Decision_Log.md
└── Test_Report.md
changelog/
```

Komponen/layanan yang mungkin diperlukan (nama dapat mengikuti konvensi proyek):

- `GameSession` — status sesi aktif.
- `GameFlowController` / `SceneNavigator` — navigasi enam layar.
- `QuestionRepository` — memuat, memvalidasi, dan mengambil soal dari JSON.
- `MissionDeckService` — membentuk dan mengacak batch kartu eligible.
- `AlgebraBoardController` — interaksi kartu dan transisi tahap.
- `BoardStageValidator` — validasi slot yang eksplisit/terkurasi, bukan parser aljabar universal kecuali opsi tersebut disetujui.
- `TrueFalseController` — state statement, feedback, timer 3 detik, explanation, dan lock submit.
- `ScoreCalculator` — formula skor tunggal.
- Controller tiap layar — hanya menangani UI dan meneruskan aksi ke service, tidak menyimpan salinan skor/bank soal.

Arsitektur harus tetap sederhana. Jangan menambahkan backend, framework besar, atau dependency pihak ketiga tanpa alasan dan persetujuan.

---

## 10. Status Proyek Saat Rencana Dibuat

Informasi terakhir dari pemilik proyek:

- Unity Editor `6000.6.6f1` telah dibuka.
- Proyek `AlgeCard Adventure` dibuat menggunakan template **Universal 2D**.
- Path yang dilaporkan: `C:\Users\yogia\AlgeCard Adventure`.
- Scene awal sebelumnya bernama `SampleScene`; status penyimpanan sebagai `01_SplashScreen` perlu diverifikasi langsung oleh agen.
- Status Git, branch saat ini, konfigurasi remote, serta keberadaan direktori `Docs/` belum diverifikasi oleh dokumen ini.

Informasi di atas adalah snapshot, bukan jaminan keadaan workspace sekarang. Agen wajib memeriksa keadaan aktual sebelum mengubah apa pun.

---

## 11. Rencana Implementasi Bertahap

### Phase 0 — Preflight Workspace dan Git

**Aktivitas:**

1. Baca `universal-rule.md`, `AGENTS.md`, `GEMINI.md`, dan plan yang disetujui jika tersedia di repositori.
2. Pastikan path proyek, versi Unity, scene, paket, folder, aset, serta status kompilasi.
3. Periksa `git status`, branch aktif, dan remote secara read-only terlebih dahulu.
4. Cocokkan repo dengan `https://github.com/useripx/algecardadventure.git`; jangan langsung melakukan `init`, push, pull, atau perubahan branch sebelum memeriksa kondisi dan meminta persetujuan bila repo belum terhubung.
5. Inventarisasi referensi UI dan `banksoal.json`; catat file yang tersedia/hilang.
6. Buat daftar file yang akan dibuat/diubah dan tes untuk batch pertama.

**Tidak boleh:** menghapus scene/aset, upgrade paket Unity, mereset repo, mengganti project settings, atau menimpa file.

**Exit gate:** laporkan temuan, konflik, rencana file, dan risiko. Tunggu persetujuan eksplisit sebelum perubahan implementasi.

### Phase 1 — Fondasi dan Integrasi Data

**Aktivitas setelah disetujui:**

- Siapkan folder data runtime dan integrasikan `banksoal.json` dengan proses sinkronisasi yang jelas.
- Implementasikan `QuestionRepository` serta validasi format/ID/status/level.
- Tambahkan test data repository dan pengacakan tanpa UI.
- Pastikan data JSON bisa dibaca di Editor dan sediakan jalur baca yang kompatibel untuk Android.
- Jangan membuat UI yang bergantung pada topik yang belum tersedia.

**Exit gate:** hasil test repository dan ringkasan soal eligible per level ditinjau. Tidak boleh melaporkan pass jika test belum dijalankan.

### Phase 2 — Fondasi UI Portrait

**Aktivitas setelah disetujui:**

- Implementasikan pengaturan portrait dan safe area.
- Buat komponen gaya bersama yang benar-benar digunakan oleh layar pertama.
- Gunakan aset lokal dan jangan menimpa sumber.

**Exit gate:** tampilkan screenshot/preview, daftar file berubah, serta hasil compile/test. Minta persetujuan sebelum mulai Screen 01 jika fondasi sudah memenuhi syarat.

### Phase 3 — Screen 01: Splash Screen

- Implementasikan satu layar sesuai referensi yang disetujui.
- Pastikan instruksi `Tap anywhere to start` jelas dan navigasi berfungsi.
- Uji portrait scaling dan safe area.

**Approval gate:** kirim screenshot/preview; berhenti sampai pemilik menyatakan Screen 01 sudah sesuai.

### Phase 4 — Screen 02: Level

- Implementasikan pilihan `Easy`, `Normal`, `Hard`.
- Terapkan keputusan OPEN-01 mengenai topik sebelum finalisasi navigasi.
- Pastikan level pilihan bertahan saat berpindah layar.

**Approval gate:** tampilkan layar dan interaksinya; berhenti untuk persetujuan.

### Phase 5 — Screen 03: Mission Cards

- Tampilkan hingga lima kartu tertutup.
- Implementasikan shuffle state, reveal satu kartu, dan tombol `Continue`.
- Gunakan `QuestionRepository`; soal aktif harus tetap sama sampai board terbuka.
- Blokir soal yang status tinjauannya belum disetujui atau yang belum playable sesuai keputusan OPEN-02/OPEN-03.

**Approval gate:** tampilkan keadaan tertutup dan terbuka serta bukti bahwa ID/teks soal yang diteruskan tidak berubah; minta persetujuan.

### Phase 6 — Screen 04: Algebra Card Board

- Mulai dari satu soal pilot yang mapping board-nya telah disetujui dan divalidasi.
- Terapkan empat tahap, drag/tap interaction yang disetujui, clear/replace, Continue, Review, dan validasi.
- Tambahkan soal lain hanya ketika mapping-nya terverifikasi.

**Approval gate:** demonstrasikan keempat tahap dengan soal pilot, termasuk input salah dan perbaikan; tunggu persetujuan.

### Phase 7 — Screen 05: True or False dan Explanation

- Mulai hanya jika soal pilot memiliki True/False statement, truth value, dan explanation yang sudah disetujui serta diuji.
- Implementasikan state feedback, lock submit, delay nyata 3 detik, dan explanation.
- Pastikan timer dibatalkan/ditangani aman saat layar/session ditinggalkan.

**Approval gate:** demonstrasikan jawaban benar dan salah, anti-double-submit, waktu 3 detik, dan penjelasan; tunggu persetujuan.

### Phase 8 — Screen 06: Score Results dan Loop Sesi

- Sambungkan hasil nyata dari `GameSession` dan `ScoreCalculator`.
- Uji Continue mempertahankan progres dan Exit terkonfirmasi mereset sesi.
- Uji batas tepat 10 soal dan kondisi bank eligible habis.

**Approval gate:** demonstrasikan skor setelah benar/salah, Continue, Exit, serta batas sesi; tunggu persetujuan.

### Phase 9 — Integrasi, QA, dan Android Build

- Jalankan EditMode/PlayMode tests yang tersedia, review Console, dan tes alur end-to-end.
- Bangun Android jika Android Build Support tersedia.
- Catat hasil yang benar-benar diuji, perangkat/resolusi, versi Unity, isu tersisa, dan langkah menjalankan game.
- Jangan mengklaim build Android berhasil jika tidak dijalankan dan diverifikasi.

**Exit gate:** pemilik meninjau bukti test/build dan memutuskan apakah MVP siap diterima.

---

## 12. File Target dan Dampak yang Diperkirakan

Daftar ini adalah **perkiraan target**, bukan izin membuat semua file sekaligus. Agen wajib memperbaruinya setelah preflight dan hanya menyentuh file yang diperlukan untuk batch yang telah disetujui.

| File/lokasi | Tujuan | Waktu |
|---|---|---|
| `Docs/Implementation_plan.md` | Rencana implementasi yang disetujui | Sebelum coding |
| `Assets/StreamingAssets/banksoal.json` | Data soal runtime yang dibundel offline | Phase 1, setelah persetujuan |
| `Assets/.../Scripts/Data/QuestionRepository.cs` | Membaca, memvalidasi, memilih soal JSON | Phase 1 |
| `Assets/.../Scripts/Session/GameSession.cs` | Menjaga progres/ID/skor/batas sesi | Saat diperlukan dan disetujui |
| `Assets/.../Scripts/Gameplay/MissionDeckService.cs` | Membentuk batch maksimum lima soal eligible | Setelah repository |
| `Assets/.../Scripts/Gameplay/BoardStageValidator.cs` | Validasi mapping kartu per tahap yang disetujui | Setelah OPEN-02 selesai |
| `Assets/.../Scripts/Gameplay/TrueFalseController.cs` | State True/False, timer, penjelasan | Setelah OPEN-03 selesai |
| `Assets/.../Scripts/Gameplay/ScoreCalculator.cs` | Formula skor satu sumber | Sebelum Screen 06 |
| `Assets/.../Scenes/01_SplashScreen.unity` hingga `06_ScoreResults.unity` | Enam kelompok layar | Bertahap, satu layar per approval gate |
| `Assets/.../Tests/EditMode/` dan `PlayMode/` | Uji logika dan integrasi | Setiap phase terkait |
| `Docs/Task_Status.md` | Status kerja berbasis bukti | Saat implementasi dimulai |
| `Docs/Decision_Log.md` | Catat keputusan yang disetujui | Saat keputusan dibuat |
| `Docs/Test_Report.md` | Bukti tes dan hasil | Setelah test dijalankan |
| `changelog/` | Catatan perubahan bertanggal | Setiap batch implementasi |

`Assets/...` berarti subfolder aktual harus dipastikan pada Phase 0; jangan membuat dua struktur alternatif.

---

## 13. Rencana Pengujian dan Kriteria Penerimaan

### EditMode/unit tests

- JSON berhasil dimuat dan memiliki 3 level UI dengan masing-masing 20 soal sebelum filtering berdasarkan status review.
- ID unik dan tidak kosong.
- `Medium` sumber termapping ke `Normal` UI.
- `equation_model` dan `solution_steps` dapat berisi lebih dari satu item.
- Soal dengan status `needs_clarification`/`needs_source_check` tidak masuk pool playable rilis.
- `true_false` bernilai null ditangani sebagai belum tersedia; tidak berubah menjadi jawaban `False`.
- Pengacakan hanya mengambil soal dari level aktif dan tidak menduplikasi ID dalam batch/sesi.
- Skor: 0/1 benar = 0; 1/1 = 100; 1/2 = 50; 2/3 ditampilkan 67; 4/5 ditampilkan 80.
- Submit jawaban berulang hanya mencatat sekali.
- Sesi tidak pernah memulai soal ke-11.
- Board validator menerima slot valid dan menolak slot kosong/salah sesuai mapping pilot yang disetujui.

### PlayMode/manual tests

- Tap Splash menuju Level.
- Level yang dipilih tersimpan dan dipakai untuk pool soal yang benar.
- Mission Cards menampilkan maksimal lima kartu eligible; hanya kartu yang disentuh dibuka.
- Tombol Continue meneruskan `questionId` dan soal yang sama.
- Seluruh tahap board pilot dapat dikerjakan dan direview tanpa kehilangan penempatan valid secara tak terduga.
- True/False benar dan salah memperlihatkan feedback yang tepat, timer berjalan 3 detik, lalu explanation muncul.
- Score Results menampilkan data dinamis dan tidak menggunakan nilai dummy.
- Continue menjaga progres; Exit setelah konfirmasi mengakhiri sesi.
- Kondisi soal habis tidak menyebabkan duplikasi otomatis.
- Tidak ada error kritis Console pada alur yang diuji.
- Semua layar tetap terbaca dan dapat disentuh pada target portrait aspect ratios.
- Uji Android dijalankan jika toolchain tersedia; jika tidak, jelaskan secara spesifik bagian yang belum terverifikasi.

### Aturan bukti

Status task hanya boleh `Done` bila ada bukti yang diamati: test output, hasil compile, screenshot, atau build. Jangan menyatakan “100% PASS” tanpa hasil uji yang nyata.

---

## 14. Tata Kelola Agen, Git, dan Keamanan

1. Baca aturan kerja proyek sebelum coding; `universal-rule.md` dan `AGENTS.md` menjadi instruksi tata kelola jika tersedia di repositori.
2. **No plan, no code. No explicit approval, no code.** Untuk perubahan baru atau bug berikutnya, ajukan rencana kecil tersendiri jika cakupan/risk-nya tidak tercakup oleh plan yang disetujui.
3. Sebelum perubahan, laporkan file target, opsi/trade-off bila ada, dan cara pengujian.
4. Jangan menghapus aset/scene, mengubah GUID `.meta`, meng-upgrade package Unity, melakukan reset/rebase destruktif, atau menimpa file sumber tanpa persetujuan.
5. Periksa status Git dan branch sebelum perintah Git apa pun.
6. Repositori target: `https://github.com/useripx/algecardadventure.git`.
7. Jangan commit/push langsung ke `main`. Gunakan `develop` dan feature branch setelah keadaan repo diverifikasi; pemilik proyek yang mengendalikan merge ke `main`.
8. Jika remote, branch, atau autentikasi belum siap, laporkan dan minta persetujuan; jangan menginisialisasi atau push secara otomatis.
9. Jangan memasukkan token, password, API key, atau rahasia ke source control/log.
10. Pada akhir setiap batch, laporkan file yang berubah, tes yang benar-benar dijalankan, hasil, known issues, dan langkah verifikasi pengguna.
11. Perbarui status, Decision Log, Test Report, dan changelog bila relevan; jangan mengklaim dokumen diperbarui jika tidak benar-benar diubah.
12. Kerjakan satu layar pada satu waktu dan tunggu persetujuan sebelum meneruskan ke layar berikutnya.

---

## 15. Definition of Done — MVP

MVP dapat diajukan untuk penerimaan saat:

- Enam kelompok layar ada di Unity, berbahasa Inggris, dan memiliki gaya visual konsisten.
- Game dapat berjalan offline tanpa mengunduh soal, font, ilustrasi, atau efek dari internet saat runtime.
- `banksoal.json` dibaca melalui repository; soal tidak di-hardcode di UI/script.
- Pemilihan level, pemilihan misi, ID soal aktif, deduplikasi, dan kondisi bank habis bekerja sesuai spesifikasi.
- Hanya soal dengan status dan konten gameplay yang disetujui dapat dimainkan.
- Board pilot memiliki mapping slot yang terdokumentasi dan validasi yang diuji; batasan cakupan parser disebutkan dengan jujur.
- True/False statement, jawaban, dan explanation telah diverifikasi untuk setiap soal yang playable.
- Feedback 3 detik dan anti-double-submit bekerja.
- Skor kumulatif mengikuti formula, dan batas sesi 10 soal ditegakkan.
- Continue, Exit, error JSON, dan pool soal habis punya perilaku yang dapat dipahami pemain.
- Automated/manual test report menyatakan hasil sebenarnya; isu terbuka tercatat.
- Build Android berhasil dibuat dan diuji jika lingkungan/toolchain tersedia, atau keterbatasan toolchain disebut jelas.
- Tidak ada file/aset asli yang tertimpa dan tidak ada commit/push langsung ke `main` oleh agen.

---

## 16. Changelog

- **v1.1-draft — 17 Oktober 2026:** Menambahkan integrasi bank soal `banksoal.json`, tautan repositori GitHub, status proyek Unity terakhir, aturan review-status, serta decision gates untuk topik, mapping papan kartu, dan konten True/False yang belum tersedia. Memisahkan rencana dari izin implementasi.
- **v1.0 reference:** Mengacu pada rancangan Implementasi Antigravity/Unity dan workflow review UI sebelumnya; diperlakukan sebagai referensi, bukan pengganti keputusan terbaru.
