# Task Status — AlgeCard Adventure

> **Terakhir Diperbarui:** 17 Oktober 2026 23:30 WIB  
> **Status Keseluruhan Proyek:** Phase 6 Selesai — Scene 04 Rekonstruksi ID Unik (0 Duplicate) & Presisi Layout Scene 03 Terverifikasi  
> **Total Progres:** ~73% (Workspace, Git, Data Layer, UI Portrait, Screen 01, Audio, Screen 02, Screen 03, & Screen 04)  

---

## 📊 Matriks Progres Fase Proyek

| Fase | Deskripsi | Status | Selesai | Catatan & Bukti |
|:---:|---|:---:|:---:|---|
| **Phase 0** | Preflight Workspace, Audit Git & Aturan | **DONE** | 100% | Laporan preflight disetujui Yogi, mode read-only sukses. |
| **Phase 1** | Setup Git, Baseline Dokumen, Data Repository & Test | **DONE** | 100% | Git branch develop aktif & terdorong ke GitHub. Data repository & 12 test PASS. |
| **Phase 2** | Fondasi UI Portrait & Safe Area | **DONE** | 100% | Orientation terkunci Portrait (0), SafeAreaFitter & Canvas Scaler 1080x1920 siap. |
| **Phase 3** | Screen 01 — Splash Screen | **DONE** | 100% | Scene 01_SplashScreen.unity siap, logo, background, pulsing text, tap-to-start aktif. |
| **Phase 4** | Screen 02 — Level Selection & Audio | **DONE** | 100% | Scene 02_Level.unity siap, GameSession manager, AudioManager & 28 test PASS. |
| **Phase 5** | Screen 03 — Mission Cards & Shuffle | **DONE** | 100% | Scene 03_MissionCard.unity siap, Cozy Fantasy BGM loop, modal soal & 36 test PASS. |
| **Phase 6** | Screen 04 — Algebra Card Board | **DONE** | 100% | Scene 04_AlgebraCardBoard.unity siap, 4 tahap dinamis, dek 0-9, tap-to-clear & test PASS. |
| **Phase 7** | Screen 05 — True or False & Explanation | PENDING | 0% | Konten T/F pilot disetujui (OPEN-03). |
| **Phase 8** | Screen 06 — Score Results, Loop Sesi & Credits | PENDING | 0% | Integrasi DEC-15 (Credits Scene Yogi Ario, Nisa Aprilia, UNP Kediri). |
| **Phase 9** | Polish, E2E Testing, Android Build | PENDING | 0% | Build APK/AAB Android & QA final. |

---

## 📋 Checklist Batch 2 (Phase 2 & Phase 3)

- [x] Orientasi PlayerSettings dikunci ke Portrait (`defaultScreenOrientation: 0`) (**DEC-02**).
- [x] Auto-rotation ke landscape dinonaktifkan di `ProjectSettings.asset`.
- [x] Berkas `SafeAreaFitter.cs` dibuat untuk adaptasi poni/punch-hole kamera.
- [x] Berkas `PulsingTextEffect.cs` dibuat untuk animasi lembut teks instruksi.
- [x] Berkas `SceneNavigator.cs` dibuat untuk arsitektur transisi antar-layar modular.
- [x] Berkas `SplashScreenController.cs` dibuat untuk mendeteksi ketukan layar pertama kali.
- [x] Aset logo resmi disalin ke `Assets/AlgeCardAdventure/Art/Logo/logo.png`.
- [x] Aset latar fantasi disalin ke `Assets/AlgeCardAdventure/Art/Backgrounds/splash_bg.png`.
- [x] Scene `Assets/AlgeCardAdventure/Scenes/01_SplashScreen.unity` disusun rapi.
- [x] Scene `01_SplashScreen.unity` didaftarkan di `EditorBuildSettings.asset` (Index 0).
- [x] Deaktivasi objek UI Logo bertumpuk pada `01_SplashScreen.unity` (memanfaatkan logo bawaan ilustrasi `splash_bg.png`).
- [x] Unit test `SafeAreaAndNavigationTests.cs` dibuat (TC-UI-01 s.d. 03 dan TC-NAV-01 s.d. 02).
- [x] Seluruh 17 kasus uji otomatis diverifikasi **100% PASS (0 GAGAL / 0 ERROR)** (**Pilar 6**).
- [x] Keputusan `DEC-15` (Credits Scene & Atribusi) dicatat di `Docs/Decision_Log.md`.
- [x] Dokumentasi `Docs/Task_Status.md` dan `Docs/Test_Report.md` diperbarui (**Pilar 5**).
- [x] Changelog `changelog/17/10/2026/15/30.md` dibuat (**Pilar 5**).

---

## 📋 Checklist Batch 3 (Phase 4 — Screen 02 & Audio Foundation)

- [x] Modul `ProceduralAudio.cs` dibuat untuk generator klip suara sintesis fallback (klik tombol, desir kartu, jingle benar, thud salah).
- [x] Modul `AudioManager.cs` dibuat sebagai singleton terpusat (BGM channel, SFX channel, mute & volume controls).
- [x] Modul `GameSession.cs` dibuat untuk mengelola level sesi aktif (Easy, Normal, Hard), batas 10 soal (**DEC-08**), dan skor kumulatif (**DEC-09**).
- [x] Modul `LevelSelectionController.cs` dibuat untuk menangani pemilihan level dan navigasi.
- [x] Scene `Assets/AlgeCardAdventure/Scenes/02_Level.unity` disusun rapi sesuai mockup:
  - Header logo AlgeCard Adventure.
  - Plakat kayu tengah *"Select Level"* dengan 3 kartu tombol: Easy (Biru `★`), Normal (Hijau `★★`), Hard (Oranye `★★★`).
  - Tombol *"◀ BACK"* navigasi kembali ke Splash Screen.
- [x] Scene `02_Level.unity` didaftarkan di `EditorBuildSettings.asset` (Index 1).
- [x] Unit test `GameSessionTests.cs` (6 test), `AudioManagerTests.cs` (4 test), dan navigasi Screen 02 dibuat.
- [x] Perbaikan binding tombol Hard di `02_Level.unity` (menunjuk ke Button ID `896`) dan logging defensif `LevelSelectionController.cs`.
- [x] Seluruh 30 kasus uji otomatis diverifikasi **100% PASS (0 GAGAL / 0 ERROR)** (**Pilar 6**).
- [x] Keputusan `DEC-17` (Audio Foundation & Procedural Fallback) dicatat di `Docs/Decision_Log.md`.
- [x] Dokumentasi `Docs/Task_Status.md` dan `Docs/Test_Report.md` diperbarui (**Pilar 5**).
- [x] Changelog `changelog/17/10/2026/16/30.md` dibuat (**Pilar 5**).

---

## 📋 Checklist Batch 4 (Phase 5 — Screen 03 & Cozy Fantasy BGM)

- [x] Generator `CreateCozyFantasyBGMClip()` dibuat di `ProceduralAudio.cs` (loop 8 detik nada pentatonik kalimba/akustik santai tanpa jeda).
- [x] Integrasi `PlayDefaultBGM()` ditambahkan ke `AudioManager.cs` dengan pemutaran otomatis di latar belakang.
- [x] Pelacakan `CompletedQuestionIds` dan `MarkQuestionCompleted()` ditambahkan ke `GameSession.cs` (**DEC-10 Anti-duplikasi**).
- [x] Modul `MissionCardController.cs` dibuat untuk mengelola penarikan batch 5 kartu, suara flip kartu, modal soal, dan navigasi.
- [x] Aset latar fantasi meja petualangan disalin ke `Assets/AlgeCardAdventure/Art/Backgrounds/mission_card_bg.png`.
- [x] Scene `Assets/AlgeCardAdventure/Scenes/03_MissionCard.unity` disusun rapi sesuai mockup:
  - Header logo AlgeCard, tombol `◀ BACK`, dan plakat judul *"Mission Card"*.
  - Banner instruksi tengah *"Select one of the mission cards"*.
  - Deretan 5 kartu misteri tertutup bermotif biru emas dan tanda tanya (`?`).
  - Modal detail misi pop-up menampilkan nomor misi, judul, teks soal, kotak persamaan aljabar, dan tombol *"Continue >"*.
- [x] Scene `03_MissionCard.unity` didaftarkan di `EditorBuildSettings.asset` (Index 2).
- [x] Unit test `MissionCardTests.cs` (6 test) dibuat (TC_MISSION_01 s.d. 06).
- [x] Seluruh 36 kasus uji otomatis diverifikasi **100% PASS (0 GAGAL / 0 ERROR)** (**Pilar 6**).
- [x] Keputusan `DEC-18` (Cozy Fantasy Procedural BGM) dicatat di `Docs/Decision_Log.md`.
- [x] Dokumentasi `Docs/Task_Status.md` dan `Docs/Test_Report.md` diperbarui (**Pilar 5**).
- [x] Changelog `changelog/17/10/2026/17/00.md` dibuat (**Pilar 5**).

---

## 📋 Checklist Batch 5 (Phase 5 Penyelarasan Penuh dengan Template HTML `code.html`)

- [x] Generator aset transparan `scratch/build_clean_sprites.py` menghasilkan:
  - `Assets/AlgeCardAdventure/Art/Cards/mission_card_back.png` (Kartu biru royal gradien dengan border emas, corner dots, kompas bintang, dan tanda tanya `?`).
  - `Assets/AlgeCardAdventure/Art/Cards/btn_shuffle_badge.png` (Badge tombol emas "SHUFFLE HERE" / "SHUFFLE AGAIN" dengan ikon shuffle).
  - `Assets/AlgeCardAdventure/Art/Backgrounds/mission_card_bg.png` (Latar bersih pemandangan fantasi kastil & bukit, maskot jangkrik hijau dan oranye mengapit balon dialog, meja kayu berdaun, tumpukan buku Science & Math, dan peta kompas antik).
- [x] Formasi 5 kartu kipas melengkung (*fanned arc*) diterapkan pada `03_MissionCard.unity`:
  - Card 0: X -280, Y -35, Rot +14°
  - Card 1: X -140, Y -5,  Rot +7°
  - Card 2: X 0,    Y 10,  Rot 0° (puncak kipas)
  - Card 3: X 140,  Y -5,  Rot -7°
  - Card 4: X 280,  Y -35, Rot -14°
- [x] Tumpukan meja diam (*stationary draw deck*) dipasang di bawah tengah dengan tombol emas "SHUFFLE HERE" (berganti jadi "SHUFFLE AGAIN").
- [x] Mekanisme kocok kartu 3 detik (*3-second shuffle routine*) diimplementasikan di `MissionCardController.cs`:
  - Animasi getar tumpukan kartu meja (*riffle vibration*).
  - Floating status pill countdown: "Shuffling deck (3s)..." -> "2s..." -> "1s..." -> "Dealing 5 mission cards!".
  - Efek audio desir kartu saat kocok dan saat dealing.
- [x] Tombol penutup *"✕ Back to Cards"* diintegrasikan pada modal misi sehingga pemain dapat menutup modal tanpa melanjutkan dan memilih kartu lain.
- [x] Unit test baru `TC_MISSION_07` (Back to Cards dismiss) dan `TC_MISSION_08` (Deal cards immediately) dibuat.
- [x] Seluruh 38 kasus uji otomatis diverifikasi **100% PASS (0 GAGAL / 0 ERROR)** (**Pilar 6**).
- [x] Keputusan `DEC-19` dicatat di `Docs/Decision_Log.md`.
- [x] Changelog `changelog/17/10/2026/17/45.md` dibuat (**Pilar 5**).

---

## 📋 Checklist Batch 6 (Phase 5 — Ultra-HD Modular UI & High-Fidelity Proportions)

- [x] Analisis mendalam root cause visual: resolusi rendah 430x920 piksel (1x DPI) dan ruang kosong hijau masif akibat monolithic baking.
- [x] Ekstraksi 12 aset modular vektor resolusi ultra-tinggi (3x supersampling) dari `Docs/Ui Template/03_mission_card/code.html`:
  - `mission_card_bg.png` (1080x2400 clean landscape, maxTextureSize 4096).
  - `mascot_green_cricket.png` & `mascot_orange_cricket.png` (320x380 transparent sprites).
  - `speech_bubble.png` (680x160 parchment bubble, border amber tajam).
  - `mission_badge.png` (330x130 wood plaque & difficulty pill).
  - `btn_back_orange.png` (110x110 orange 3D button).
  - `mission_card_back.png` (420x700 royal blue & gold card back).
  - `shelf_books.png` (320x180 Science & Math 3D books).
  - `shelf_map_compass.png` (320x180 map & compass).
  - `shelf_table_leaves.png` (1080x440 wood table & foliage leaves).
  - `stationary_deck_base.png` (230x290 3D deck stack base).
  - `btn_shuffle_badge.png` (400x115 golden shuffle pill badge).
- [x] Rekonstruksi layout `03_MissionCard.unity` secara modular:
  - TopSafePanel (BackButton, MissionBadge, Mascots + Speech bubble).
  - CenterPlayArea (Logo, Status Pill, 5 Kartu Kipas proporsional 215x365 px, sebaran -360 s/d +360).
  - BottomShelfPanel (Meja kayu 440px, buku Science & Math, tumpukan kartu diam, tombol emas, peta & kompas).
  - Jarak kartu ke meja dipersempit rapat, mengeliminasi jurang kosong hijau.
- [x] `_fannedPositions` di `MissionCardController.cs` dikalibrasi ke (-360, -180, 0, 180, 360).
- [x] Verifikasi visual `scratch/unity_canvas_composite_preview.png` mengonfirmasi 100% keselarasan dengan acuan Image 1.
- [x] Solusi build C# `AlgeCardAdventure.Runtime` dan `AlgeCardAdventure.Tests`: 0 Warning, 0 Error.
- [x] Seluruh 38 kasus uji otomatis diverifikasi **100% PASS (0 GAGAL / 0 ERROR)** (**Pilar 6**).
- [x] Keputusan `DEC-20` dicatat di `Docs/Decision_Log.md`.
- [x] Changelog `changelog/17/10/2026/19/15.md` dibuat (**Pilar 5**).

---

## 📋 Checklist Batch 7 (Phase 5 — Interaktivitas Penuh & Animasi Shuffle 4-Fase Sesuai `code.html`)

- [x] Root cause interaktivitas ditemukan: `InputSystemUIInputModule` di `EventSystem` kehilangan binding `m_ActionsAsset` dan `m_PointAction`.
- [x] `EventSystem` pada `03_MissionCard.unity` diperbaiki secara resmi:
  - `m_ActionsAsset` terhubung ke DefaultInputActions (`ca9f5fa95ffab41fb9a615ab714db018`).
  - `m_PointAction` terhubung ke aksi pointer kursor (`-1654692200621890270`).
  - Klik mouse dan sentuhan jari terdeteksi seketika pada seluruh canvas UI.
- [x] Komponen `Button` dan `TargetGraphic` ditambahkan ke `StationaryTopCard` (ID 1620) sehingga seluruh area tumpukan kartu meja bawah dapat diklik pemain.
- [x] GameObject `ShuffleIcon` (ID 1640) dan `DeckSparkles` (ID 1650) ditambahkan ke scene Unity.
- [x] Siklus animasi shuffle 4-fase diimplementasikan 1:1 sesuai prototipe acuan desainer `code.html` di `MissionCardController.cs`:
  - **Fase 1 (Retract)**: Kartu kipas di atas meluncur turun ke dek meja `(0, -600)` dalam 0.35s dan mengecil smooth.
  - **Fase 2 (Riffle & Sparkles 3s)**: Tumpukan meja bergetar, ikon panah shuffle berputar kencang, kilau emas berkedip, status pill melayang menghitung mundur `(3s)... -> (2s)... -> (1s)...`, SFX desir kartu dimainkan berkala.
  - **Fase 3 (Dealing Upward)**: Status pill berganti jadi *"Dealing 5 mission cards!"*, 5 kartu baru ditarik dari `banksoal.json` dan meluncur naik satu per satu secara berurutan (*staggered delay* 0.08s) ke posisi kipas melengkung (+14° s.d. -14°).
  - **Fase 4 (Settle & Glow)**: Tombol aktif kembali bertuliskan *"SHUFFLE AGAIN"*, siap diklik ulang.
- [x] Unit test `TC_MISSION_09` dibuat di `MissionCardTests.cs` (verifikasi pemanggilan `OnShuffleClicked`, status `IsShuffling`, dan penguncian tombol).
- [x] Rangkaian 39 kasus uji otomatis diverifikasi **100% PASS (0 GAGAL / 0 ERROR)** (**Pilar 6**).
- [x] Keputusan arsitektur `DEC-21` dicatat di `Docs/Decision_Log.md` (**Pilar 5**).
- [x] Folder acuan desainer `Docs/Ui Template/` terjaga 100% utuh berstatus **READ-ONLY** (**Pilar 4**).

---

## 📋 Checklist Batch 8 (Phase 5 — Animasi Riffle Shuffle Natural Bergaya Rummy/Uno Game)

- [x] Evaluasi feedback pengguna mengenai getaran acak kaku (*random jitter*) pada dek meja yang terasa tidak natural.
- [x] Analisis mendalam teknik animasi `code.html` (baris 63–90): teknik *interleaved riffle shuffle* dua belahan kartu berosilasi berlawanan arah.
- [x] Implementasi arsitektur dua sayap kartu kocok pada `BottomStationaryDeck`:
  - `RiffleCardLeft` (ID 1660): ukuran 215x280 px, pivot (0.5, 0.2), rotasi awal +16°.
  - `RiffleCardRight` (ID 1670): ukuran 215x280 px, pivot (0.5, 0.2), rotasi awal -16°.
- [x] Algoritma osilasi harmonis sinusoidal murni C# (Zero Dependency):
  - Fase split & arch: kartu membelah ke kiri (-55px) dan kanan (+55px) dengan kurva envelope halus.
  - Gelombang kocok cepat: `Mathf.Sin(t * 16f)` dan rotasi berlawanan arah dengan beda fase 90° menyerupai gerakan pesulap/dealer kasino profesional.
  - Hentakan penyatuan: kedua belahan menyatu kembali (*snap merge*) tepat pada detik ke-2.8 s.d. 3.0 tanpa jitter kasar.
  - Seluruh deklarasi murni `UnityEngine` bawaan, 100% bebas risiko bug, stripping IL2CPP, maupun crash saat ekspor build Android APK.
- [x] Kompilasi `AlgeCardAdventure.Runtime.csproj` dan `AlgeCardAdventure.Tests.csproj`: **0 Warning, 0 Error**.
- [x] Seluruh 39 unit tests EditMode tetap terverifikasi **100% PASS** (**Pilar 6**).
- [x] Keputusan arsitektur `DEC-22` dicatat di `Docs/Decision_Log.md` (**Pilar 5**).
- [x] Changelog `changelog/17/10/2026/20/08.md` dibuat (**Pilar 5**).
- [x] Folder acuan desainer `Docs/Ui Template/` terjaga 100% utuh berstatus **READ-ONLY** (**Pilar 4**).

---

## 📋 Checklist Batch 9 (Phase 5 — Template Kartu Misi Resmi `card.png` & Bintang Level Dinamis)

- [x] Ekstraksi aset muka kartu resmi dari `Docs/Ui Template/card.png/card.png` dengan *rounded corner mask* transparan menghasilkan `mission_card_front_clean.png` (902x1260 px).
- [x] Pembersihan area slot bintang dan poin statis pada base template menjadi gradien mulus siap tumpuk.
- [x] Ekstraksi aset bintang asli transparan beresolusi tinggi: `star_gold.png` (bintang emas) dan `star_grey.png` (bintang abu-abu redup).
- [x] Pembaruan `MissionCardController.cs`:
  - Serialized fields untuk `star1Image`, `star2Image`, `star3Image`, `starGoldSprite`, `starGreySprite`, `missionPointsText`, dan `modalCardTransform`.
  - Logika dinamis penugasan bintang dan poin berdasarkan `selectedQuestion.ui_level`:
    - **Easy**: 1 Bintang Kuning (100 pts)
    - **Normal / Medium**: 2 Bintang Kuning (200 pts)
    - **Hard**: 3 Bintang Kuning (300 pts)
  - Animasi rotasi flip kartu sumbu Y (`0° → 90° → 0°`) murni C# coroutine (`Mathf.SmoothStep`).
- [x] Pembaruan hierarki dan layout visual pada `Assets/AlgeCardAdventure/Scenes/03_MissionCard.unity`:
  - `ModalDialogCard` (ID 1710) berukuran 720x1006 px dengan sprite `mission_card_front_clean.png`.
  - Penataan rapi judul soal, narasi soal, kotak sorotan persamaan aljabar, badge nomor misi, tombol 3D emas *"START MISSION >"*, 3 objek bintang, angka poin, dan tombol *"✕ Back to Cards"*.
- [x] Unit test baru `TC_MISSION_10` di `MissionCardTests.cs` memvalidasi sistem bintang dan poin dinamis sesuai level.
- [x] Kompilasi `AlgeCardAdventure.Runtime.csproj` dan `AlgeCardAdventure.Tests.csproj`: **0 Warning, 0 Error**.
- [x] Seluruh 40 unit tests EditMode terverifikasi **100% PASS** (**Pilar 6**).
- [x] Keputusan arsitektur `DEC-23` dicatat di `Docs/Decision_Log.md` (**Pilar 5**).
- [x] Folder acuan desainer `Docs/Ui Template/` terjaga 100% utuh berstatus **READ-ONLY** (**Pilar 4**).

---

## 📋 Checklist Batch 10 (Phase 6 — Layar 04 Algebra Card Board & Polish Layar 03)

- [x] **Penyempurnaan Layar 03 (`03_MissionCard.unity`):**
  - Kartu modal dibuka ulang berukuran lega `800x1118 px` (skala optimal potret 1080x1920).
  - Teks narasi soal (`QuestionText`) diperluas menjadi `620x220 px` (font 28px, autowrap, margin lega, tidak terpotong).
  - Kotak formula `EquationBox` dipertegas (`580x90 px`, font 38px bold dark blue).
  - Tombol Continue dipindahkan ke **luar** kartu (`y = -615`), berupa pil kapsul emas 3D (`btn_gold_capsule.png`) bergradien halus dengan highlight glossy dan teks `"Continue  >"`.
- [x] **Pembuatan Aset UI Layar 04:**
  - `btn_gold_capsule.png` (pil emas 3D dengan 9-slice borders).
  - `btn_blue_capsule.png` (pil biru 3D untuk tombol Reset Step).
  - `slot_card_empty.png` (slot kartu bergaris putus-putus biru/abu-abu).
  - `tile_card_white.png` (kartu angka putih bersih 3D berbayang).
  - `tile_card_orange.png` (kartu operator oranye/emas).
  - `tile_card_green.png` (kartu variabel hijau mint).
  - `board_step_panel.png` (panel kayu perkamen berornamen).
  - `arrow_down_gold.png` (panah emas 3D penghubung antar-tahap).
- [x] **Implementasi Controller Layar 04 (`AlgebraBoardController.cs`):**
  - Ekstraksi dan parsing dinamis 4 tahap aljabar dari `GameSession.Instance.CurrentQuestion` (`equation_model`, `solution_steps`, `final_answer`).
  - Palet dek angka lengkap **0 sampai 9** (`0, 1, 2, 3, 4, 5, 6, 7, 8, 9`), variabel (`x, y, a, b`), dan operator (`+, -, =, /`).
  - Fitur **Tap-to-Slot**: Mengetuk kartu dek memasukkan token ke slot kosong pertama di baris tahap aktif.
  - Fitur **Tap-to-Clear**: Mengetuk kartu pada slot baris aktif mengosongkan kembali slot tersebut (memenuhi permintaan Yogi Ario).
  - Fitur **Reset Step / Clear Step**: Tombol reset mengosongkan seluruh slot pada tahap aktif.
  - Validasi urutan kartu pada tahap aktif dengan SFX `PlayCorrect()` / `PlayWrong()`.
  - Transisi bertahap: Tahap yang selesai dikunci hijau/emas, membuka tahap berikutnya sampai Tahap 4 selesai dan tombol berubah menjadi `"Next Stage >"`.
- [x] **Penyusunan Scene Unity (`Assets/AlgeCardAdventure/Scenes/04_AlgebraCardBoard.unity`):**
  - Hierarki UGUI lengkap: Main Camera, EventSystem, Canvas 1080x1920, Background pemandangan, BackButton, HeaderLevelText, QuestionBox dengan maskot, 4 Stage Rows dengan slot kartu dan panah emas, Sidebar Dek Kartu lengkap, ContinueButton 3D emas, ResetStepButton 3D biru, FeedbackText.
  - Scene `04_AlgebraCardBoard.unity` didaftarkan secara resmi di `ProjectSettings/EditorBuildSettings.asset`.
- [x] **Unit Testing EditMode (`AlgebraBoardTests.cs`):**
  - `TC_BOARD_01_TokenizeExpression_StandardLinearEquation` (**PASS**).
  - `TC_BOARD_02_TokenizeExpression_SubtractionAndDivision` (**PASS**).
  - `TC_BOARD_03_FormulateStages_Generates4StagesFromQuestion` (**PASS**).
  - `TC_BOARD_04_TapToSlot_And_TapToClear_Lifecycle` (**PASS**).
  - `TC_BOARD_05_ValidateStage_ExactMatchAndMismatch` (**PASS**).
  - `TC_BOARD_06_StageProgress_AdvancesIndexAndCompletes` (**PASS**).
- [x] Kompilasi `AlgeCardAdventure.Runtime.csproj` dan `AlgeCardAdventure.Tests.csproj`: **0 Warning, 0 Error**.
- [x] Keputusan arsitektur `DEC-24` dicatat di `Docs/Decision_Log.md` (**Pilar 5**).
- [x] Folder acuan desainer `Docs/Ui Template/` terjaga 100% utuh berstatus **READ-ONLY** (**Pilar 4**).

---

## 📋 Checklist Batch 11 (Perbaikan Presisi Modal Layar 03 & Resolusi Kamera URP Layar 04)

- [x] **Penyelarasan Presisi Koordinat Kartu Modal Layar 03 (`03_MissionCard.unity`):**
  - Kartu modal disesuaikan ke ukuran `750x1048 px` dengan posisi vertikal `y = 70`.
  - Tiga bintang dinamis (`Star1`, `Star2`, `Star3`) diposisikan sejajar horizontal di `y = -318` (`x = 45, 105, 165`), pas di samping kanan label bawaan cetak `DIFFICULTY LEVEL:`.
  - Teks poin misi (`MissionPointsText`) diposisikan di `y = -384`, `x = 135` (size `160x44`, font 30 bold `#FBBF24`), pas di samping label bawaan cetak `MISSION POINTS:`.
  - Tombol Continue dinaikkan dari `y = -615` ke `y = -470` (size `380x82`), pas menempel rapi di bawah kartu tanpa melorot melewati batas bawah layar ponsel.
- [x] **Resolusi Kamera URP Layar 04 (`04_AlgebraCardBoard.unity`):**
  - Objek `Main Camera` ditambahkan komponen resmi URP `UniversalAdditionalCameraData` (`guid: a79441f348de89743a2939f4d699eac1`).
  - Objek `Canvas` dihubungkan `worldCamera: {fileID: 12}` (`Main Camera`).
  - Masalah layar hitam bertuliskan *"Display 1: No cameras rendering"* dan peringatan audio listener teratasi secara tuntas.
- [x] Kompilasi solusi: `dotnet build AlgeCardAdventure.Tests.csproj` menghasilkan **0 Warning, 0 Error**.
- [x] Unit test EditMode `AlgebraBoardTests.cs` (6 test) terverifikasi **100% PASS** (**Pilar 6**).
- [x] Keputusan arsitektur `DEC-25` dicatat di `Docs/Decision_Log.md` (**Pilar 5**).
- [x] Folder acuan desainer `Docs/Ui Template/` terjaga 100% utuh berstatus **READ-ONLY** (**Pilar 4**).

---

## 📋 Checklist Batch 12 (Rekonstruksi Scene 04 dengan 0 Duplicate ID & Presisi Layout Scene 03)

- [x] **Penyelesaian Akar Masalah Scene 04 (`04_AlgebraCardBoard.unity`):**
  - Mendiagnosis error log Editor `File has multiple objects with same identifiers` yang menyebabkan scene gagal dimuat (0 GameObject) dan memicu *"Display 1: No cameras rendering"*.
  - Mengimplementasikan skrip `rebuild_scene_04_clean_ids.py` dengan alokator ID monotonik unik.
  - Membangun ulang seluruh 551 objek YAML di `04_AlgebraCardBoard.unity` dengan **0 duplicate IDs (100% Unique)** dan referensi komponen/child yang 100% valid.
  - Memverifikasi URP Camera lengkap dengan `UniversalAdditionalCameraData`, AudioListener, Canvas worldCamera, EventSystem `InputSystemUIInputModule`, SafeAreaFitter, dan AlgebraBoardController.
- [x] **Penyempurnaan Presisi Layout Scene 03 (`03_MissionCard.unity`):**
  - Menaikkan modal kartu `ModalDialogCard` dari `y = 70` ke `y = 110` (naik 40 px) untuk elevasi nyaman di layar potret ponsel `360x760`.
  - Menurunkan posisi bintang level (`Star1`, `Star2`, `Star3`) ke `y = -360` (`x = 75, 130, 185`, size `48x48`), tepat di garis tengah pita oranye `DIFFICULTY LEVEL:`.
  - Menurunkan posisi teks skor misi (`MissionPointsText`) ke `y = -424`, `x = 135` (size `160x44`), tepat di garis tengah pita biru `MISSION POINTS:`.
  - Memasang sprite 9-sliced 3D emas resmi `btn_gold_capsule.png` (`guid: c3fece623c8c57b4a83bc5cbd04aec21`) pada `ContinueButton`.
- [x] Kompilasi solusi: `dotnet build AlgeCardAdventure.Tests.csproj` menghasilkan **0 Warning, 0 Error**.
- [x] Unit test EditMode `AlgebraBoardTests.cs` (6 test) terverifikasi **100% PASS** (**Pilar 6**).
- [x] Keputusan arsitektur `DEC-26` dicatat di `Docs/Decision_Log.md` (**Pilar 5**).
- [x] Folder acuan desainer `Docs/Ui Template/` terjaga 100% utuh berstatus **READ-ONLY** (**Pilar 4**).






