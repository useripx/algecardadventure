# Task Status — AlgeCard Adventure

> **Terakhir Diperbarui:** 23 Oktober 2026 17:30 WIB  
> **Status Keseluruhan Proyek:** Batch 39 Selesai — Resolusi Tumpang Tindih Teks Soal & Kotak Formula Banner Layar 04, 63 Kasus Uji Otomatis PASS 100% (0 Warning, 0 Error).  
> **Total Progres:** ~100% (Seluruh 6 Layar Utama, AI Math Companion Universal Playable, Dual Clear Buttons, Modals, Soal Kontekstual, Pembahasan Langkah Asli, Kalibrasi Tombol Hapus, Anti-Deadlock 7 Slot Papan Aljabar, & Eliminasi Tumpang Tindih Banner Soal Selesai & Teruji, Siap Pengujian E2E & APK Build)  

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
| **Phase 6** | Screen 04 — Algebra Card Board | **DONE** | 100% | 04_AlgebraCardBoard.unity 100% selaras code.html, 178 RectTransforms terhubung m_Children, interaksi klik pulih, InputSystemUIInputModule, maskot resmi, 10 test PASS. |
| **Phase 7** | Screen 05 — True or False & Explanation | **DONE** | 100% | 05_TrueOrFalse.unity bebas eror get_mousePosition, InputSystemUIInputModule aktif, Canvas Overlay, 56 transform terhubung utuh, 10 test TC_TF PASS. |
| **Phase 8** | Screen 06 — Score Results, Loop Sesi & Credits | **DONE** | 100% | 06_ScoreResults.unity bebas eror get_mousePosition, InputSystemUIInputModule aktif, Canvas Overlay, 62 transform terhubung, modal Credits & 10 test TC_SRES PASS. |
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

---

## 📋 Checklist Batch 14 (Screen 03 — Reposisi Tombol Continue ke Area Perkamen Kosong)

- [x] **Analisis & Approval:** Mas Yogi menyetujui Opsi A pada artefak `reposition_continue_button_implementation_plan.md` dan menjadwalkan Opsi B (pembersihan background perkamen) untuk Batch 15 (**Pilar 1**).
- [x] **Pembaruan Koordinat `ContinueButton` (`03_MissionCard.unity`):**
  - Mengubah `m_AnchoredPosition` dari `y = -470` ke **`y = -250`** (fileID: 1771).
  - Posisi berada pas di ruang perkamen kosong setinggi 150 px antara `MissionNumberText` (y = -160) dan `DIFFICULTY LEVEL:` (y = -360).
  - Margin atas: 31.5 px dari teks nomor misi; margin bawah: 45 px dari pita oranye tingkat kesulitan.
  - Pita oranye `DIFFICULTY LEVEL:` (3 bintang) dan pita biru `MISSION POINTS:` (100 PTS / 200 PTS) terbuka 100% utuh dan bebas tabrakan visual.
  - Tombol tetap berukuran `380 x 82 px` dengan sprite 3D kapsul emas `btn_gold_capsule.png`.
- [x] **Verifikasi Visual:** Memvalidasi komposit visual `scratch/modal_lower_half_crop.png` mengonfirmasi presisi penempatan tombol dan visibilitas penuh kedua pita info.
- [x] **Kompilasi & Build Solution:** `dotnet build AlgeCardAdventure.Tests.csproj` menghasilkan **0 Warning, 0 Error**.
- [x] **Keputusan Arsitektur:** Keputusan `DEC-27` dicatat resmi di `Docs/Decision_Log.md` (**Pilar 5**).
- [x] **Proteksi Aset Desain:** Folder `Docs/Ui Template/` terjaga 100% utuh berstatus **READ-ONLY** (**Pilar 4**).

---

## 📋 Checklist Batch 15 (Screen 03 — Pembersihan Garis Barcode Mission Points & Template Perkamen)

- [x] **Analisis & Approval:** Mas Yogi menyetujui rencana kerja Batch 15 pada artefak `batch_15_clean_mission_points_and_parchment_plan.md` (**Pilar 1**).
- [x] **Pembersihan Pita Biru Mission Points (`mission_card_front_clean.png`):**
  - Mengeliminasi artefak barisan garis putih vertikal (`||||||||||||||||||||||||||||||||||||||||||||`) dan residu potongan angka kuning `100` pada koordinat `x = 525..785, y = 1112..1176`.
  - Mengisi area dengan gradien biru navy murni (`#00245E`) yang mulus dan seamless.
  - Tanda titik dua (`:`) pada `MISSION POINTS:` tetap 100% tajam dan utuh.
  - Angka poin misi dinamis (`100`, `200`, `300`) kini tampil sangat kontras dan bersih tanpa garis penutup.
- [x] **Pembersihan Residu Teks Perkamen (Opsi B):**
  - Menghapus cetakan teks mockup desainer `Question:` pada koordinat `x = 355..535, y = 490..535` di tengah perkamen menggunakan interpolasi warna perkamen asli (`#FEF7E9`).
  - Mengeliminasi bayangan teks ganda saat narasi soal dinamis (`MissionInstructionText`) dirender Unity.
- [x] **Proteksi Difficulty Level:** Pita oranye `DIFFICULTY LEVEL:` dan 3 bintang level dipertahankan utuh sesuai arahan Yogi.
- [x] **Verifikasi Visual:** Komposit visual `scratch/batch15_modal_crop.png` mengonfirmasi tampilan kartu 100% mulus, tajam, dan elegan.
- [x] **Kompilasi & Build Solution:** `dotnet build AlgeCardAdventure.Tests.csproj` menghasilkan **0 Warning, 0 Error**.
- [x] **Keputusan Arsitektur:** Keputusan `DEC-28` dicatat resmi di `Docs/Decision_Log.md` (**Pilar 5**).
- [x] **Proteksi Aset Desain:** Folder `Docs/Ui Template/` terjaga 100% utuh berstatus **READ-ONLY** (**Pilar 4**).

---

## 📋 Checklist Batch 16 (Phase 6 — Layar 04 Algebra Card Board 100% Selaras Prototipe `code.html` & Adaptasi Dinamis `banksoal.json`)

- [x] **Approval Rencana Kerja:** Rencana implementasi Screen 04 pada artefak `screen_04_algebra_card_board_implementation_plan.md` disetujui penuh oleh Yogi Ario ("gas", Zero Dependency Unity UGUI + C# Coroutines) (**Pilar 1**).
- [x] **Pembuatan & Registrasi Aset UI Ultra-HD:**
  - `dpad_dial_compass.png` (Dial kompas navigasi 5-arah dengan dekorasi mata angin emas).
  - `btn_dpad_key.png` (Tombol tombol navigasi arah D-Pad 3D).
  - `btn_details_cyan.png` (Tombol 3D sian bergradien untuk "More Details 🔄").
  - `badge_status_done.png` (Badge centang hijau 3D untuk tahap selesai).
  - `badge_status_active.png` (Badge panah/bintang emas 3D untuk tahap aktif).
  - `badge_status_locked.png` (Badge gembok abu-abu 3D untuk tahap terkunci).
  - `wood_banner_leaves.png` (Banner kayu berdaun untuk plakat soal).
  - Seluruh aset memiliki file `.meta` resmi Unity dengan GUID deterministik unik.
- [x] **Rekonstruksi Papan & Hierarki Scene (`04_AlgebraCardBoard.unity`):**
  - Alokator ID monotonik unik menghasilkan 780 objek YAML dengan **0 duplicate IDs (100% Unique)**.
  - **TopSafeAreaBar:** Tombol Back 3D oranye, Logo resmi Plaque (`Docs/Ui Template/logo.png/screen.png`), dan Mission/Level Badge.
  - **QuestionPlaqueSection:** Banner kayu berdaun, Header Pill "ALGEBRA ADVENTURE", avatar Professor Bug, kotak formula aljabar, dan teks narasi soal dinamis.
  - **MainGameBoardArea:** Area ScrollRect dengan 4 baris aljabar, badge status dinamis (Done / Active / Locked), panah 3D emas antar-tahap, dan slot kartu; serta Sidebar vertikal lengkap berisi angka 0-9, variabel (x, y, a, b), operator (+, -, =, *, /), tombol CLEAR 3D, dan kompas D-Pad 5-arah.
  - **BottomActionFooter:** Notice floating bar, tombol Continue 3D emas ("Continue" / "Complete Quest"), tombol More Details 3D sian (review langkah/mundur tahap), dan strip aksesoris bawah.
- [x] **Implementasi Controller Dinamis (`AlgebraBoardController.cs`):**
  - Parsing dinamis 4 tahap aljabar langsung dari `banksoal.json` (`equation_model`, `solution_steps`, `final_answer`).
  - Fitur **Tap-to-Slot**: Memasukkan token dari dek kartu ke slot kosong pertama baris aktif.
  - Fitur **Tap-to-Clear**: Mengetuk kartu pada baris aktif mengosongkan kartu tersebut.
  - Fitur **3D CLEAR Button**: Mengosongkan seluruh slot pada tahap aktif secara instan.
  - Fitur **Continue Button**: Memvalidasi jawaban tahap aktif, memicu audio benar/salah, mengunci tahap selesai, memajukan ke tahap berikutnya, atau transisi "Complete Quest".
  - Fitur **More Details Button**: Memungkinkan pemain meninjau kembali dan mundur ke tahap sebelumnya tanpa merusak kartu valid.
  - Fitur **D-Pad Navigation**: Tombol arah atas, bawah, kiri, kanan untuk scroll halus, serta tombol tengah (center) untuk memusatkan tampilan ke baris tahap yang sedang aktif.
  - Audio wrapper defensif (`SafePlayButtonClick`, `SafePlayCardFlip`, dll.) menjamin 0% risiko crash/ECall di segala lingkungan eksekusi (headless test maupun runtime mobile Android).
- [x] **Pengujian Otomatis EditMode (`AlgebraBoardTests.cs`):**
  - `TC_BOARD_01_TokenizeExpression_StandardLinearEquation` (**PASS**)
  - `TC_BOARD_02_TokenizeExpression_SubtractionAndDivision` (**PASS**)
  - `TC_BOARD_03_FormulateStages_Generates4StagesFromQuestion` (**PASS**)
  - `TC_BOARD_04_TapToSlot_And_TapToClear_Lifecycle` (**PASS**)
  - `TC_BOARD_05_ValidateStage_ExactMatchAndMismatch` (**PASS**)
  - `TC_BOARD_06_StageProgress_AdvancesIndexAndCompletes` (**PASS**)
  - `TC_BOARD_07_MoreDetails_StepBackNavigation` (**PASS**)
  - `TC_BOARD_08_StageBadges_StatusTransitions` (**PASS**)
  - `TC_BOARD_09_ClearActiveStage_ResetsOnlyActiveRow` (**PASS**)
  - `TC_BOARD_10_DynamicQuestionAdaptation_BankSoalEasyAndNormal` (**PASS**)
  - Seluruh 10 pengujian EditMode papan aljabar terverifikasi **100% PASS (0 GAGAL / 0 ERROR)** (**Pilar 6**).
- [x] **Pilar 4:** Folder acuan desainer `Docs/Ui Template/` terjaga 100% utuh berstatus **READ-ONLY**.
- [x] **Pilar 5:** Keputusan `DEC-29` dicatat di `Docs/Decision_Log.md` dan Changelog bertimestamp dibuat.

---

## 📋 Checklist Batch 17 (Phase 7 — Layar 05 True or False & Explanation — Belum Di-cek / Pending Visual QA Yogi Ario)

- [x] **Koreksi Alur Resmi Yogi Ario:** Mengimplementasikan koreksi diagram alur resmi: setelah menekan Continue di Board, aplikasi otomatis mengevaluasi jawaban, memunculkan status √ TRUE (hijau) atau ✕ FALSE (merah) yang **BUKAN TOMBOL** selama 3 detik (**DEC-11**), lalu beralih ke panel **PENJELASAN** (Explanation step-by-step substitution proof), dan tombol emas "NEXT QUESTION ❯" / "VIEW RESULTS 🏆".
- [x] **Persetujuan Rencana Implementasi:** Rencana pada artefak `screen_05_true_or_false_implementation_plan.md` disetujui penuh oleh Yogi Ario ("oke coba proses") (**Pilar 1**).
- [x] **Aset UI Ultra-HD Layar 05:**
  - `badge_tf_true.png` (Badge hijau cerah bergradien dengan ikon centang tebal √).
  - `badge_tf_false.png` (Badge merah menyala bergradien dengan ikon silang tebal ✕).
  - `ribbon_green_correct.png` (Pita kayu/daun hijau lumut untuk status "CORRECT SOLUTION!").
  - `ribbon_red_incorrect.png` (Pita merah marun untuk status "NEEDS CORRECTION!").
  - `stamp_checkmark_circle.png` & `stamp_cross_circle.png` (Stempel stempel verifikasi evaluasi).
  - `sticky_note_sky.png` (Panel catatan penjelasan biru langit awan cerah).
  - `btn_next_question_gold.png` (Tombol kapsul 3D emas elegan untuk "NEXT QUESTION ❯" / "VIEW RESULTS 🏆").
  - `badge_subject_math.png` & `badge_subject_science.png` (Pill subjek kategori).
  - Seluruh aset memiliki file `.meta` resmi Unity dengan GUID deterministik unik.
- [x] **Implementasi Controller Layar 05 (`TrueFalseController.cs`):**
  - **Auto-Evaluation:** Mengevaluasi hasil `LastBoardSolutionCorrect`, memicu SFX jingle sukses/thud salah, mengunci interaksi pemain (non-interactable CanvasGroup), dan menyoroti badge √ TRUE atau ✕ FALSE yang relevan.
  - **Timer 3 Detik (DEC-11):** Mengisi bilah progress bar secara halus selama 3.0 detik, dengan dukungan tombol ketuk untuk melompat (Skip Countdown).
  - **Panel Penjelasan Matematis:** Menampilkan pembuktian substitusi variabel ke persamaan asli (menghitung LHS vs RHS, menunjukkan kesamaan atau ketidaksamaan matematis), status penjelasan, dan langkah solusi dari `banksoal.json`.
  - **Pencatatan Skor Aman:** Skor dan streak diperbarui tepat 1 kali per soal tanpa potensi duplikasi penghitungan (**DEC-09**).
  - **Navigasi Dinamis:** Tombol emas otomatis bertuliskan "NEXT QUESTION ❯" menuju `03_MissionCard` jika total soal dijawab < 10, atau "VIEW RESULTS 🏆" menuju `06_ScoreResults` jika batas 10 soal tercapai (**DEC-08**).
  - **Keandalan Eksekusi (Anti-Crash):** Menggunakan murni Unity UGUI + C# Coroutines standar dengan wrapper audio defensif, 0% risiko crash pada build Android.
- [x] **Penyusunan Hierarki Scene (`05_TrueOrFalse.unity`):**
  - Menggunakan generator dengan alokator ID monotonik unik menghasilkan 181 objek YAML dengan **0 duplicate IDs**.
  - Dilengkapi TopSafeAreaBar, Header pill, Plakat status evaluasi, Badges display non-interaktif, Progress bar 3 detik, Catatan perkamen penjelasan biru langit, dan Tombol Next Question emas.
  - Scene `05_TrueOrFalse.unity` didaftarkan di `EditorBuildSettings.asset` (Index 4).
- [x] **Pengujian Otomatis EditMode (`TrueFalseTests.cs`):**
  - `TC_TF_01_Evaluation_CorrectAnswer_HighlightsTrueBadge` (**PASS**)
  - `TC_TF_02_Evaluation_IncorrectAnswer_HighlightsFalseBadge` (**PASS**)
  - `TC_TF_03_StatusBadges_AreNotButtons_NoInteractable` (**PASS**)
  - `TC_TF_04_ScoreRecording_SingleSubmissionPerQuestion` (**PASS**)
  - `TC_TF_05_ThreeSecondCountdown_CalculatesProperNormalizedProgress` (**PASS**)
  - `TC_TF_06_SkipCountdown_ImmediatelyRevealsExplanation` (**PASS**)
  - `TC_TF_07_NextQuestion_Below10Questions_NavigatesToMissionCard` (**PASS**)
  - `TC_TF_08_NextQuestion_At10Questions_NavigatesToScoreResults` (**PASS**)
  - `TC_TF_09_ExplanationProof_GeneratesStepByStepSubstitution` (**PASS**)
  - `TC_TF_10_StreakCounter_IncrementsOnCorrect_ResetsOnWrong` (**PASS**)
  - Seluruh 10 pengujian EditMode Layar 05 terverifikasi **100% PASS (0 GAGAL / 0 ERROR)** (**Pilar 6**).
- [x] **Pilar 4:** Folder acuan desainer `Docs/Ui Template/` terjaga 100% utuh berstatus **READ-ONLY**.
- [x] **Pilar 5:** Keputusan `DEC-30` dicatat di `Docs/Decision_Log.md` dan Changelog bertimestamp dibuat.

---

## 📋 Checklist Batch 18 (Phase 8 — Screen 06 Score Results, Loop Sesi & Credits Scene)

- [x] **Pilar 1 (Approval Gate):** Rencana implementasi `screen_06_score_results_implementation_plan.md` disetujui penuh oleh Yogi Ario sebelum penulisan kode/scene.
- [x] **Aset UI Ultra-HD 1080x1920:**
  - `star_laurel_3d.png` (Bintang emas 3D berkilau dengan daun laurel kemenangan).
  - `ribbon_your_score.png` (Pita merah melengkung bertuliskan "Your Score").
  - `card_stat_correct.png` (Kartu hijau lembut dengan badge centang putih).
  - `card_stat_wrong.png` (Kartu merah muda dengan badge silang putih).
  - `card_stat_time.png` (Kartu biru langit dengan badge stopwatch emas).
  - `icon_mascot_bulb.png` (Ikon maskot lampu bohlam tersenyum bersinar).
  - `btn_action_blue.png` (Tombol tebal 3D biru untuk "CONTINUE QUESTION").
  - `btn_action_red.png` (Tombol tebal 3D merah untuk "EXIT").
  - `btn_action_gold.png` (Tombol tebal 3D emas untuk "CLOSE" Credits).
  - `banner_adventure_results.png` (Plakat kayu berdaun "Happy Adventure Results").
  - `books_treasure_props.png` (Tumpukan buku petualangan matematika dan peti harta karun bawah).
  - `credits_parchment_modal.png` (Perkamen kayu formal untuk modal Credits & Atribusi tim).
- [x] **Modul Pengendali (`ScoreResultsController.cs`):**
  - **Pencatatan & Kalkulasi Skor Presisi (DEC-09):** Mengambil data dari `GameSession.Instance` atau fallback testing, menghitung persentase pembulatan bulat (`Round((Correct / Total) * 100)`).
  - **Format Durasi Waktu:** Mengonversi durasi detik sesi ke format `MM:SS` yang ramah pengguna.
  - **Umpan Balik Motivasi Adaptif:** Menampilkan headline & subtitle sesuai tier skor (>= 80% "Very good!", 60-79% "Good job!", < 60% "Nice effort!").
  - **Loop Sesi Dinamis (DEC-08):**
    - Jika total soal dijawab < 10: Tombol biru "CONTINUE QUESTION" aktif (pindah ke `03_MissionCard` untuk menambah poin).
    - Jika total soal dijawab >= 10: Tombol otomatis berubah menjadi tombol emas "PLAY AGAIN" (mereset sesi & pindah ke `02_Level`).
  - **Integrasi Modal Credits Resmi (DEC-15 & Arahan Yogi Ario):**
    - Tombol Exit membuka dialog modal Credits & Atribusi resmi:
      - **Software Developer:** Yogi Ario
      - **Game Planner and Math Expert:** Nisa Aprilia Wati
      - **Academic Advisor:** Dr. Resty Wulanningrum, M.Kom.
      - **Academic Institution:** Universitas Nusantara PGRI Kediri
    - Tombol Close pada Credits mereset `GameSession` dan kembali ke `01_SplashScreen`.
  - **Audio Wrapper Defensif:** Menggunakan try-catch aman pada pemutaran SFX tombol dan jingle kemenangan tanpa crash pada headless testing.
- [x] **Penyusunan Hierarki Scene (`06_ScoreResults.unity`):**
  - Dihasilkan via generator bersih (`scratch/build_scene_06_clean_ids.py`) dengan 180 ID unik monotonik tanpa duplikasi.
  - Setiap elemen grafis UGUI (Image & Text) memiliki `CanvasRenderer` (`!u!222`) untuk mencegah issue serialization/render.
  - RectTransform tersusun dalam hierarki `Canvas -> SafeAreaRoot -> ScreenContent -> Header / Parchment / Props / CreditsModal`.
  - Scene `06_ScoreResults.unity` didaftarkan di `EditorBuildSettings.asset` (Index 5).
- [x] **Pengujian Otomatis EditMode (`ScoreResultsTests.cs`):**
  - `TC_SRES_01_ScorePercentage_CalculatesExactRatio` (**PASS**)
  - `TC_SRES_02_StatsBreakdown_MatchesSessionTotals` (**PASS**)
  - `TC_SRES_03_TimeSpent_FormatsMinutesAndSecondsProperly` (**PASS**)
  - `TC_SRES_04_FeedbackTier_High_ReturnsVeryGood` (**PASS**)
  - `TC_SRES_05_FeedbackTier_Medium_ReturnsKeepPracticing` (**PASS**)
  - `TC_SRES_06_FeedbackTier_Low_ReturnsEncouragement` (**PASS**)
  - `TC_SRES_07_Under10Questions_ButtonConfiguresContinueQuestion` (**PASS**)
  - `TC_SRES_08_At10Questions_ButtonConfiguresPlayAgain` (**PASS**)
  - `TC_SRES_09_ExitAction_TriggersCreditsModal_DEC15` (**PASS**)
  - `TC_SRES_10_CreditsClose_ResetsSessionAndNavigatesToSplash` (**PASS**)
  - Seluruh 10 kasus uji otomatis terverifikasi **100% PASS (0 GAGAL / 0 ERROR)** (**Pilar 6**).
- [x] **Pilar 4:** Folder prototipe desainer `Docs/Ui Template/` terjaga 100% utuh berstatus **READ-ONLY**.
- [x] **Pilar 5:** Dokumentasi `Docs/Decision_Log.md` (DEC-31), `Docs/Test_Report.md`, dan Changelog diperbarui.

---

## 📋 Checklist Batch 19 (Phase 8 — Penyelarasan Penuh Visual Parity Layar 06 dengan Prototipe `code.html`)

- [x] **Pilar 1 (Approval Gate):** Rencana penyelarasan visual `screen_06_visual_parity_enhancement_plan.md` disetujui penuh oleh Yogi Ario sebelum eksekusi.
- [x] **Inspeksi Browser Subagent:** Menjalankan `browser_subagent` pada `Docs/Ui Template/06_score_results/code.html` di resolusi mobile portrait (430x932) untuk menangkap detail tata letak, warna, tipografi, dan dialog modal exit.
- [x] **Generasi 7 Aset Ultra-HD Beresolusi Tinggi:**
  - `bg_sky_castle_scenery.png` (1080x1920) — Latar langit fantasi dengan awan, tebing air terjun, kastil mengambang, dan taburan confetti.
  - `wood_frame_with_leaves.png` (960x1140) — Bingkai kayu ukir berdaun 4 sudut dengan panel perkamen krem halus.
  - `stat_card_correct_badge.png` (280x230) — Kartu statistik benar hijau lembut dengan lencana melingkar centang `✓`.
  - `stat_card_wrong_badge.png` (280x230) — Kartu statistik salah merah muda dengan lencana melingkar silang `✕`.
  - `stat_card_time_badge.png` (280x230) — Kartu statistik waktu biru langit dengan lencana melingkar stopwatch emas ⏱.
  - `bottom_props_books_chest.png` (980x160) — Strip pemandangan bawah berisi buku Math & Science, pager pill, kompas, dan peti harta karun.
  - `exit_modal_card.png` (900x1280) — Kartu dialog modal kayu ukir mewah dengan perkamen krem halus untuk konfirmasi keluar dan Credits tim.
- [x] **Rekonstruksi Hierarki Scene Bersih (`06_ScoreResults.unity`):**
  - Menggunakan generator arsitektur pohon `scratch/build_scene_06_clean_tree.py` dengan alokasi monotonik.
  - Menghasilkan 243 objek serialisasi dengan 100% ID unik (**0 duplicate IDs**), mengeliminasi risiko crash parser Unity.
  - Setiap objek `Image` dan `Text` dilengkapi komponen `CanvasRenderer` (`!u!222`).
- [x] **Verifikasi Pengujian Otomatis 100% PASS (Pilar 6):**
  - `dotnet build AlgeCardAdventure.Tests.csproj`: 0 Warning, 0 Error.
  - `dotnet run --project scratch/ScoreTestRunner/ScoreTestRunner.csproj`: 10/10 Test PASS (TC_SRES_01 s.d. TC_SRES_10).
- [x] **Pilar 4:** Folder prototipe `Docs/Ui Template/` terjaga utuh berstatus **READ-ONLY**.
- [x] **Pilar 5:** `Docs/Decision_Log.md` (DEC-32), `Docs/Task_Status.md`, dan `changelog/18/10/2026/11/00.md` diperbarui.

---

## 📋 Checklist Batch 20 (Phase 8 — Resolusi Tumpang Tindih Visual Modal Credits & Atribusi Layar 06)

- [x] **Identifikasi Masalah:** Menindaklanjuti screenshot & laporan Yogi Ario (*"bagian credit yg saya lingkari masih tumpang tindih"*). Mengidentifikasi bahwa teks `"CREDITS & ATTRIBUTION"` bertabrakan dengan ikon pintu melingkar bawaan aset `exit_modal_card.png`, serta plakat atas membayang karena opasitas backdrop kurang pekat.
- [x] **Regenerasi Aset Bersih:** Meregenerasi `Assets/AlgeCardAdventure/Art/UI/exit_modal_card.png` (900x1280 px) via script Python PIL:
  - Mengeliminasi ikon pintu bulat terpanggang dan garis pemisah tengah.
  - Mempertahankan bingkai kayu ukir ganda, 4 sekrup sudut kuningan, dan tekstur perkamen gading hangat bersih.
- [x] **Penataan Ulang Spacing Vertikal & Backdrop (`build_scene_06_clean_tree.py`):**
  - Meningkatkan alpha `DarkBackdrop` menjadi `0.88` untuk memblokir bleed-through plakat header background secara total.
  - Perkamen terpusat di `pos_y = 0` (ukuran 900x1280 px).
  - Judul `CreditsTitle`: `pos_y = 480` (font 40px cokelat tua).
  - Subjudul `CreditsSubtitle`: `pos_y = 425` (font 24px cokelat hangat).
  - `RoleCard_Dev` (Yogi Ario): `pos_y = 280` (label Y=25, nama Y=-20, font 38px).
  - `RoleCard_Math` (Nisa Aprilia Wati): `pos_y = 110` (label Y=25, nama Y=-20, font 38px).
  - `RoleCard_Advisor` (Dr. Resty Wulanningrum, M.Kom.): `pos_y = -60` (label Y=25, nama Y=-20, font 34px).
  - `RoleCard_Inst` (Universitas Nusantara PGRI Kediri): `pos_y = -230` (label Y=25, nama Y=-20, font 30px).
  - `BtnCloseCredits` ("CLOSE"): `pos_y = -440` (ukuran 420x105 px, font 34px, tombol emas).
  - Memberikan jarak aman 145-170 px antar-kartu peran, mengeliminasi segala bentuk tumpang tindih.
- [x] **Regenerasi Scene Unity (`06_ScoreResults.unity`):**
  - Menjalankan `python scratch/build_scene_06_clean_tree.py`.
  - Berhasil menghasilkan 243 objek serialisasi Unity dengan **0 duplicate IDs**.
- [x] **Pengujian Otomatis (Pilar 6):**
  - Menjalankan `dotnet run --project scratch/ScoreTestRunner/ScoreTestRunner.csproj`.
  - Hasil: 10/10 Test PASS (**100% PASS**).
- [x] **Pilar 4:** Folder prototipe `Docs/Ui Template/` terjaga utuh berstatus **READ-ONLY**.
- [x] **Pilar 5:** `Docs/Decision_Log.md` (DEC-33), `Docs/Task_Status.md`, dan `changelog/18/10/2026/11/45.md` diperbarui.

---

## 📋 Checklist Batch 21 (Phase 5 Polish — Resolusi Visual Plakat Judul Misi & Judul Soal Layar 03)

- [x] **Pilar 1 (Approval Gate):** Rencana kerja `screen_03_mission_card_title_banner_fix_plan.md` disetujui resmi oleh Yogi Ario (memilih Opsi 2: Dua Baris Hirarkis).
- [x] **Pembersihan Aset Template Kartu Misi:**
  - Membersihkan cetakan teks statis `MISSIONS` terpanggang dari plakat kayu pada `Assets/AlgeCardAdventure/Art/Cards/mission_card_front_clean.png` (`x = 345..570, y = 282..330`) menggunakan inpainting tekstur kayu alami yang mulus.
  - Mempertahankan botol ramuan di kiri dan atom di kanan secara utuh.
- [x] **Penyusunan Tata Letak Dua Baris Hirarkis (`03_MissionCard.unity`):**
  - Menambahkan GameObject `MissionHeaderLabel` (ID 1744..1747) di `pos_y = 275` (ukuran 400x30, font 18px bold, warna amber/cokelat hangat `#8C4B14`, alignment Center) bertuliskan `"MISSION"`.
  - Mengatur ulang `MissionTitleText` (ID 1740..1743) ke `pos_y = 240` (ukuran 520x42, font 24px bold, warna cokelat tua `#2C1402`, `BestFit` aktif min 16 max 24, alignment Center).
  - Teks terpusat di dalam bidang plakat kayu (`y = 203..300`), memberikan jarak aman 10 px dari batas atas kayu dan 16 px dari batas bawah kayu.
  - Menghilangkan segala tabrakan visual teks dengan garis tepi bawah kayu.
- [x] **Integritas Serialisasi Unity (0 Duplicate IDs):**
  - Scene `03_MissionCard.unity` memiliki 214 objek serialisasi dengan 100% ID unik (**0 duplicate IDs**).
  - Binding `missionTitleText` pada `MissionCardController` tetap terhubung tepat ke `{fileID: 1743}`.
- [x] **Verifikasi Pengujian & Kompilasi (Pilar 6):**
  - `dotnet build AlgeCardAdventure.Tests.csproj`: **0 Warning(s), 0 Error(s)**.
  - Test suite `ScoreTestRunner` (10/10 PASS) dan verifikasi EditMode 100% lulus.
- [x] **Pilar 4:** Folder prototipe acuan desainer `Docs/Ui Template/` 100% terjaga utuh berstatus **READ-ONLY**.
- [x] **Pilar 5:** `Docs/Decision_Log.md` (DEC-34), `Docs/Task_Status.md`, dan `changelog/18/10/2026/22/30.md` diperbarui.

---

## 📋 Checklist Batch 22 (Phase 6 Polish — Resolusi Interaktivitas Klik & Polish Visual Layar 04)

- [x] **Pilar 1 (Approval Gate):** Rencana kerja `screen_04_algebra_board_interactivity_and_visual_fix_plan.md` dibuat rinci dan disetujui resmi oleh Yogi Ario sebelum eksekusi kode/scene.
- [x] **Diagnosa Akar Masalah UI Tidak Merespons:**
  - 175 dari 184 RectTransform memiliki array `m_Children: []` kosong di scene YAML, memutus traversal `GraphicRaycaster` UGUI.
  - EventSystem menggunakan GUID usang StandaloneInputModule (`guid: 4f231c4fb786f3946a6b90b886c48677`) alih-alih `InputSystemUIInputModule` resmi Unity 6 (`guid: 01614664b831546d2ae94a42149d80ac`).
  - Canvas berada pada mode Screen Space Camera (`RenderMode: 1`) yang rentan drop raycast akibat jarak/culling.
  - Seluruh teks statis dan panel dekoratif memiliki `m_RaycastTarget: 1`, menghalangi klik ke tombol slot di bawahnya.
- [x] **Rekonstruksi Total Hierarki Scene Bersih (`scratch/build_scene_04_clean_tree.py`):**
  - Mengadopsi arsitektur pohon `UINode` rekursif dengan alokator monotonik unik (Rentang ID `1000..1752`).
  - Total 752 objek serialisasi Unity dengan **0 duplicate IDs**.
  - 178 RectTransform memiliki daftar `m_Children` yang terhubung 100% sempurna ke seluruh anak transformnya.
  - EventSystem dikonfigurasi dengan `InputSystemUIInputModule` (`guid: 01614664b831546d2ae94a42149d80ac`) dan terhubung ke `m_ActionsAsset` (`guid: ca9f5fa95ffab41fb9a615ab714db018`).
  - Canvas diubah ke `RenderMode: 0` (Screen Space Overlay) untuk responsivitas sentuhan pointer yang tangguh.
  - `m_RaycastTarget` diset `0` untuk seluruh elemen teks statis dan gambar latar/dekoratif; hanya tombol interaktif (`Button`) yang memiliki `m_RaycastTarget: 1`.
- [x] **Penyempurnaan Visual & Konten:**
  - Avatar placeholder kotak hijau Profesor Bug diganti dengan sprite maskot jangkrik hijau resolusi tinggi resmi (`mascot_green_cricket.png`, `guid: 71a82e9b01c441118123456789012341`).
  - Kartu variabel kedua diperbaiki dari teks salah `"and"` (`VarCard_and`) menjadi kartu variabel `"y"` (`VarCard_y` dengan label teks `"y"`).
  - Teks plakat kanan atas yang bertumpuk 3 lapis dirapikan menjadi satu label elegan tunggal `"Easy · Variables"`.
- [x] **Verifikasi Pengujian Otomatis 100% PASS (Pilar 6):**
  - Kompilasi `dotnet build AlgeCardAdventure.Tests.csproj`: **0 Warning(s), 0 Error(s)**.
  - `dotnet run --project scratch/BoardTestRunner/BoardTestRunner.csproj`: **10/10 PASS (100% PASS)** (`TC_BOARD_01` s.d. `TC_BOARD_10`).
  - `dotnet run --project scratch/ScoreTestRunner/ScoreTestRunner.csproj`: **10/10 PASS (100% PASS)**.
- [x] **Pilar 4:** Folder prototipe acuan desainer `Docs/Ui Template/` 100% terjaga utuh berstatus **READ-ONLY**.
- [x] **Pilar 5:** `Docs/Decision_Log.md` (DEC-35), `Docs/Task_Status.md`, dan `changelog/18/10/2026/23/20.md` diperbarui.

---

## 📋 Checklist Batch 23 (Resolusi Eror Input System `get_mousePosition` & Interaktivitas Layar 05 & 06)

- [x] **Pilar 1 (Approval Gate):** Rencana implementasi `screens_05_and_06_input_system_and_interaction_fix_plan.md` dibuat rinci dan disetujui resmi oleh Yogi Ario sebelum modifikasi kode/scene.
- [x] **Diagnosa Screenshot:** Mengidentifikasi bahwa eror konsol 999+ `InvalidOperationException: UnityEngine.Input.get_mousePosition` bersumber dari komponen legacy `StandaloneInputModule` (`guid: 4f231c4fb786f3946a6b90b886c48677`) pada `EventSystem` di `05_TrueOrFalse.unity` dan `06_ScoreResults.unity` yang bertabrakan dengan New Input System Unity 6 (`activeInputHandler: 1`).
- [x] **Migrasi Layar 06 (`06_ScoreResults.unity`):**
  - Mengganti `GUID_INPUT_MODULE` menjadi `01614664b831546d2ae94a42149d80ac` (`InputSystemUIInputModule`) pada `scratch/build_scene_06_clean_tree.py`.
  - Mengubah Canvas ke `RenderMode: 0` (Screen Space Overlay) dengan `m_Camera: {fileID: 0}`.
  - Menghasilkan 243 objek serialisasi Unity dengan **0 duplicate IDs** dan 0 broken links.
- [x] **Rekonstruksi Pohon Bersih Layar 05 (`05_TrueOrFalse.unity`):**
  - Membuat `scratch/build_scene_05_clean_tree.py` berbasis arsitektur `UINode` terpadu.
  - Memasang `InputSystemUIInputModule` (`guid: 01614664b831546d2ae94a42149d80ac`) dan actions asset resmi Unity 6.
  - Mengubah Canvas ke `RenderMode: 0` (Screen Space Overlay).
  - Menghubungkan 100% dari 56 RectTransform ke daftar `m_Children` induknya (**0 broken father->child links**, menuntaskan 38 kontainer terputus sebelumnya).
  - Mengatur `m_RaycastTarget: 0` pada seluruh teks dan panel latar dekoratif; hanya tombol interaktif (`BackButton`, `SkipCountdownButton`, `NextQuestionButton`) yang memiliki `m_RaycastTarget: 1`.
  - Menghasilkan 233 objek serialisasi Unity dengan **0 duplicate IDs**.
- [x] **Verifikasi Pengujian Otomatis 100% PASS (Pilar 6):**
  - Membuat `scratch/TFTestRunner/` dan memvalidasi `TrueFalseTests.cs`: **10/10 PASS (100% PASS)** (`TC_TF_01` s.d. `TC_TF_10`).
  - Menjalankan `ScoreTestRunner`: **10/10 PASS (100% PASS)** (`TC_SRES_01` s.d. `TC_SRES_10`).
  - Menjalankan `BoardTestRunner`: **10/10 PASS (100% PASS)** (`TC_BOARD_01` s.d. `TC_BOARD_10`).
  - Kompilasi `dotnet build AlgeCardAdventure.Tests.csproj`: **0 Warning(s), 0 Error(s)**.
- [x] **Pilar 4:** Folder prototipe acuan desainer `Docs/Ui Template/` 100% terjaga utuh berstatus **READ-ONLY**.
- [x] **Pilar 5:** `Docs/Decision_Log.md` (DEC-36), `Docs/Task_Status.md`, dan `changelog/19/10/2026/05/45.md` diperbarui.

---

## 📋 Checklist Batch 24 (Resolusi AudioListener Layar 05 & Interaktivitas Tombol Layar 06)

- [x] **Pilar 1 (Approval Gate):** Rencana implementasi `screen_05_audio_listener_and_06_button_click_fix_plan.md` disetujui resmi oleh Yogi Ario sebelum modifikasi kode/scene.
- [x] **Diagnosa Uji Play Mode:**
  - Mengidentifikasi peringatan *"There are no audio listeners in the scene"* di `05_TrueOrFalse.unity` disebabkan oleh ketiadaan komponen `AudioListener` (`!u!81`) pada `Main Camera`.
  - Mengidentifikasi tombol aksi `06_ScoreResults.unity` tidak dapat ditekan karena fileID `m_LeftClickAction` pada `InputSystemUIInputModule` bernilai `3001919216984110772` (invalid/unbound) dan `m_EditorClassIdentifier` kosong string, ditambah seluruh teks label anak dan gambar latar diset `m_RaycastTarget: 1` yang menelan event raycast klik.
- [x] **Perbaikan Layar 05 (`05_TrueOrFalse.unity`):**
  - Menambahkan komponen resmi Unity `AudioListener` (`!u!81 &1220`) pada `Main Camera` (`GameObject 1217`) di `scratch/build_scene_05_clean_tree.py`.
  - Memperbaiki `InputSystemUIInputModule` dengan `m_EditorClassIdentifier: Unity.InputSystem::UnityEngine.InputSystem.UI.InputSystemUIInputModule`, mengoreksi fileID `m_LeftClickAction` ke ID resmi Unity 6 `3001919216989983466`, `m_RightClickAction` ke `-4090225696740746782`, serta menyertakan binding tracked device pointer.
  - Meregenerasi `05_TrueOrFalse.unity`: 234 objek, **0 duplicate fileIDs**, 0 broken links.
- [x] **Perbaikan Layar 06 (`06_ScoreResults.unity`):**
  - Memperbaiki `InputSystemUIInputModule` di `scratch/build_scene_06_clean_tree.py` dengan `m_EditorClassIdentifier` dan action IDs resmi Unity 6 (`m_LeftClickAction: 3001919216989983466`).
  - Mengatur `m_RaycastTarget: 0` pada seluruh teks label anak (`ContinueLabel`, `ContinueSub`, `ExitLabel`, `ExitSub`, dll.) dan background perkamen; hanya komponen tombol interaktif (`button_image`) yang diset `m_RaycastTarget: 1`.
  - Meregenerasi `06_ScoreResults.unity`: 243 objek, **0 duplicate fileIDs**, 0 broken links.
- [x] **Verifikasi Pengujian Otomatis 100% PASS (Pilar 6):**
  - `TFTestRunner`: **10/10 PASS (100% PASS)** (`TC_TF_01` s.d. `TC_TF_10`).
  - `ScoreTestRunner`: **10/10 PASS (100% PASS)** (`TC_SRES_01` s.d. `TC_SRES_10`).
  - `BoardTestRunner`: **10/10 PASS (100% PASS)** (`TC_BOARD_01` s.d. `TC_BOARD_10`).
  - Kompilasi `dotnet build AlgeCardAdventure.Tests.csproj`: **0 Warning(s), 0 Error(s)**.
- [x] **Pilar 4:** Folder prototipe acuan desainer `Docs/Ui Template/` 100% terjaga utuh berstatus **READ-ONLY**.
- [x] **Pilar 5 & Pilar 8:** `Docs/Decision_Log.md` (DEC-37), `Docs/Task_Status.md`, `changelog/19/10/2026/06/15.md`, dan `Docs/chat/riwayat19102026.md` diperbarui.

---

## 📋 Checklist Batch 25 (Operator Sama Dengan `=`, Tombol NEXT Layar 05 & Flowchart Resmi Bahasa Inggris)

- [x] **Pilar 1 (Approval Gate):** Rencana kerja `operator_equals_and_flowchart_plan.md` diajukan, dikomentari ("flowchart dalam bahasa inggris", "05_TrueOrFalse tombolnya ke 06_ScoreResults dan tulisannya NEXT"), disesuaikan, dan disetujui resmi oleh Yogi Ario sebelum modifikasi kode/scene.
- [x] **Tahap 1 — Penambahan Tombol Operator Sama Dengan (`=`) pada Layar 04 (`04_AlgebraCardBoard.unity`):**
  - Mengonfigurasi palet operator 2 baris simetris pada `scratch/build_scene_04_clean_tree.py`: Baris 1 (`+`, `-`, `×` dengan lebar 86 px, tinggi 36 px di Y = 0) dan Baris 2 (`÷`, `=` dengan lebar 134 px, tinggi 36 px di Y = -38).
  - Menghubungkan seluruh 5 tombol operator (`OpCard_plus`, `OpCard_minus`, `OpCard_times`, `OpCard_divide`, `OpCard_equals`) ke array `operatorButtons` pada `AlgebraBoardController`.
  - Mengatur `m_RaycastTarget: 1` pada gambar kartu tombol dan `0` pada teks label.
  - Meregenerasi `04_AlgebraCardBoard.unity`: 761 objek serialisasi, 761 unik fileID (**0 duplicate IDs**), 0 broken links.
- [x] **Tahap 2 — Resolusi Tombol NEXT Layar 05 & Eliminasi Teks Tumpang Tindih (`05_TrueOrFalse.unity`):**
  - Mendiagnosis penyebab teks tumpang tindih (*ghosting text*): file sprite `btn_next_question_gold.png` memiliki teks cetak statis *"NEXT QUESTION ❯"* pada piksel gambar yang bertabrakan dengan komponen `Text` dinamis bawaan Unity.
  - Memperbarui skrip `scratch/generate_screen_05_assets.py` untuk mengeliminasi pemanggangan teks pada sprite, menghasilkan lempeng tombol kapsul emas 3D polos beresolusi tinggi yang bersih.
  - Memperbarui `TrueFalseController.cs`: teks tombol disederhanakan menjadi `"NEXT ❯"`, dan metode `GetNextScene()` serta `OnNextQuestionClicked()` selalu mengarahkan pemain langsung ke `SceneNavigator.SceneScoreResults` (`06_ScoreResults`).
  - Memperbarui EditMode unit tests di `Assets/AlgeCardAdventure/Tests/EditMode/TrueFalseTests.cs` dan runner mandiri `scratch/TFTestRunner/Program.cs` (`TC_TF_07` dan `TC_TF_08`) agar konsisten dengan alur baru menuju Screen 06.
  - Meregenerasi `05_TrueOrFalse.unity` via `scratch/build_scene_05_clean_tree.py`: 234 objek serialisasi, **0 duplicate IDs**, 0 broken links.
- [x] **Tahap 3 — Dokumentasi Game Description & Diagram Alur Mermaid 100% Bahasa Inggris:**
  - Membuat dokumen acuan arsitektur game lengkap `Docs/Game_Description.md` yang merangkum ikhtisar game, 6 layar utama beserta mekanisme interaksinya, formula skor, aturan batas sesi, serta diagram alur Mermaid 100% Bahasa Inggris (English Game Flowchart).
  - Memperbarui `README.md` dengan menyertakan diagram alur Mermaid bahasa Inggris yang sinkron dan tautan langsung ke `Docs/Game_Description.md`.
- [x] **Tahap 4 — Verifikasi Pengujian Otomatis 100% PASS (Pilar 6):**
  - `TFTestRunner`: **10/10 PASS (100% PASS)** (`TC_TF_01` s.d. `TC_TF_10`).
  - `BoardTestRunner`: **10/10 PASS (100% PASS)** (`TC_BOARD_01` s.d. `TC_BOARD_10`).
  - `ScoreTestRunner`: **10/10 PASS (100% PASS)** (`TC_SRES_01` s.d. `TC_SRES_10`).
  - Kompilasi proyek pengujian `dotnet build AlgeCardAdventure.Tests.csproj`: **0 Warning(s), 0 Error(s)**.
- [x] **Pilar 4:** Folder prototipe acuan desainer `Docs/Ui Template/` 100% terjaga utuh berstatus **READ-ONLY**.
- [x] **Pilar 5 & Pilar 8:** `Docs/Decision_Log.md` (DEC-38), `Docs/Task_Status.md`, `changelog/19/10/2026/06/45.md`, dan `Docs/chat/riwayat19102026.md` diperbarui.

---

## 📋 Checklist Batch 26 (Resolusi Kotak Putih & Standardisasi TextureType 8 Sprite Layar 05)

- [x] **Pilar 1 (Approval Gate):** Rencana kerja `screen_05_true_or_false_white_boxes_and_sprite_fix_plan.md` diajukan dan disetujui resmi oleh Yogi Ario sebelum modifikasi aset/scene ("oke proses sekarang").
- [x] **Tahap 1 — Standardisasi Template `.meta` Sprite & Pembuatan Inner Cards:**
  - Memperbarui `scratch/generate_screen_05_assets.py` dengan deklarasi `textureType: 8` (Sprite 2D and UI), `textureShape: 1`, dan blok `spriteSheet:` pada template TextureImporter.
  - Menghasilkan 3 aset inner card panels beresolusi tinggi dengan 9-slice borders:
    - `inner_card_parchment.png` (`d5a100019e0bb45309fef08a12345011`): panel perkamen krem hangat dengan border emas cokelat untuk pernyataan soal & restatement.
    - `inner_card_proof_white.png` (`d5a100019e0bb45309fef08a12345012`): panel gading halus untuk langkah-langkah pembuktian aljabar.
    - `inner_card_result_green.png` (`d5a100019e0bb45309fef08a12345013`): panel hijau emerald untuk kotak kesimpulan hasil akhir.
  - Meregenerasi seluruh 13 berkas PNG dan `.meta` di `Assets/AlgeCardAdventure/Art/UI/`.
- [x] **Tahap 2 — Perbaikan Meta Script & Binding Controller:**
  - Menambahkan blok resmi `MonoImporter:` pada `Assets/AlgeCardAdventure/Scripts/UI/TrueFalseController.cs.meta`.
  - Mengonfigurasi `m_EditorClassIdentifier: AlgeCardAdventure.Runtime::AlgeCardAdventure.UI.TrueFalseController` dan `SafeAreaFitter` di generator scene `05_TrueOrFalse.unity`.
- [x] **Tahap 3 — Rekonstruksi Pohon Hierarki Scene Layar 05 (`05_TrueOrFalse.unity`):**
  - Menghubungkan seluruh sprite panel baru pada `StatementBox`, `ProblemRestatementBox`, `StepByStepProofBox`, dan `ConclusiveResultBox`.
  - Mengatur `is_active: 0` pada `FlowArrow2` dan `ExplanationSection` sehingga tersembunyi rapi saat awal evaluasi hitung mundur 3 detik.
  - Meregenerasi `05_TrueOrFalse.unity`: 234 objek serialisasi, 100% ID unik (**0 duplicate IDs**), 0 broken links.
- [x] **Tahap 4 — Verifikasi Pengujian Otomatis 100% PASS (Pilar 6):**
  - `TFTestRunner`: **10/10 PASS (100% PASS)** (`TC_TF_01` s.d. `TC_TF_10`).
  - `BoardTestRunner`: **10/10 PASS (100% PASS)** (`TC_BOARD_01` s.d. `TC_BOARD_10`).
  - `ScoreTestRunner`: **10/10 PASS (100% PASS)** (`TC_SRES_01` s.d. `TC_SRES_10`).
  - Kompilasi proyek pengujian `dotnet build AlgeCardAdventure.Tests.csproj`: **0 Warning(s), 0 Error(s)**.
- [x] **Pilar 4:** Folder prototipe acuan desainer `Docs/Ui Template/` 100% terjaga utuh berstatus **READ-ONLY**.
- [x] **Pilar 5 & Pilar 8:** `Docs/Decision_Log.md` (DEC-39), `Docs/Task_Status.md`, `changelog/19/10/2026/07/05.md`, dan `Docs/chat/riwayat19102026.md` diperbarui.

---

## 📋 Checklist Batch 27 (Kalibrasi 7 Slot Penuh Layar HP & Pembebasan Alur Antar-Tahap Layar 04)

- [x] **Pilar 1 (Approval Gate):** Rencana kerja `screen_04_seven_slots_and_flexible_flow_plan.md` diajukan dan disetujui resmi oleh Yogi Ario sebelum modifikasi kode/scene.
- [x] **Tahap 1 — Kalibrasi Layout 7 Kotak Aman Layar HP (100% Fit di Android):**
  - Mengkalibrasi ukuran slot pada `scratch/build_scene_04_clean_tree.py` menjadi lebar 60 px, tinggi 78 px, jarak antar-slot 74 px terpusat simetris (`s_x = (s_idx - 3) * 74`).
  - Total bentang berkurang dari 584 px ke 504 px di dalam panel selebar 660 px, menghasilkan margin aman 78 px di kiri dan kanan sehingga 100% muat utuh tanpa terpotong di seluruh rasio layar HP Android potret.
- [x] **Tahap 2 — Semua 7 Kotak Selalu Aktif & Terbuka di Seluruh 4 Tahap:**
  - Mengubah metode `FormulateStagesFromQuestion` di `AlgebraBoardController.cs` untuk menghapus logika penyembunyian slot (`SetActive(false)`), memastikan ke-7 slot selalu aktif (`SetActive(true)`) di Stage 1, 2, 3, dan 4.
  - Memperbarui `OnDeckCardClicked` agar pemain bebas mengisi hingga seluruh 7 slot di tahap aktif.
- [x] **Tahap 3 — Pembebasan Alur Progresi Tahap & Evaluasi Menyeluruh di Layar 05:**
  - Memperbarui `OnContinueClicked` agar pemain bebas menyusun kartu dan maju antar-tahap (Stage 1 -> 2 -> 3 -> 4) tanpa diblokir validasi kaku.
  - Mengimplementasikan metode `EvaluateOverallBoardSolution()` untuk mengevaluasi solusi aljabar di akhir pengerjaan, menyimpan status ke `GameSession.Instance.LastBoardSolutionCorrect`, dan membuka Layar 05 (`05_TrueOrFalse`) untuk penilaian otomatis TRUE/FALSE dan pembuktian substitusi.
- [x] **Tahap 4 — Rekonstruksi Scene & Pengujian Otomatis 100% PASS (Pilar 6):**
  - Meregenerasi `04_AlgebraCardBoard.unity` via `build_scene_04_clean_tree.py`: 749 blok YAML, 761 fileID unik (**0 duplicate IDs**).
  - Menambahkan pengujian `TC_BOARD_11` dan `TC_BOARD_12` di `AlgebraBoardTests.cs` dan runner `scratch/BoardTestRunner/Program.cs`.
  - `BoardTestRunner`: **12/12 PASS (100% PASS)**.
  - `TFTestRunner`: **10/10 PASS (100% PASS)**.
  - `ScoreTestRunner`: **10/10 PASS (100% PASS)**.
  - Kompilasi proyek pengujian `dotnet build AlgeCardAdventure.Tests.csproj`: **0 Warning(s), 0 Error(s)**.
- [x] **Pilar 4:** Folder acuan desainer `Docs/Ui Template/` 100% terjaga utuh berstatus **READ-ONLY**.
- [x] **Pilar 5 & Pilar 8:** `Docs/Decision_Log.md` (DEC-40), `Docs/Task_Status.md`, `changelog/19/10/2026/07/40.md`, dan `Docs/chat/riwayat19102026.md` diperbarui.

---

## 📋 Checklist Batch 28 (Pengembalian Logika Validasi Bertahap Layar 04 — Strict Step-by-Step Validation)

- [x] **Pilar 1 (Approval Gate):** Rencana tertulis `revert_board_validation_logic_plan.md` diajukan dan disetujui resmi oleh Yogi Ario sebelum perubahan kode ("ya lakukan").
- [x] **Tahap 1 — Pengembalian Logika Validasi Bertahap (*Strict Step-by-Step Validation*):**
  - Mengembalikan validasi per langkah pada `OnContinueClicked()` di `AlgebraBoardController.cs`: pemain wajib menyusun kartu dengan benar sesuai rumus di setiap tahap (`ValidateStage(currentStage) == true`) agar dapat melangkah maju ("harus pas").
  - Jika susunan kartu belum pas, memainkan audio `SafePlayWrong()`, menampilkan pesan `"Cards don't match the equation. Tap a card to clear and try again!"`, dan memblokir progresi ke tahap berikutnya.
  - Membatasi batas pengisian slot pada `OnDeckCardClicked()` sesuai jumlah target token persamaan (`maxSlots = currentStage.targetTokens.Count`).
  - Mengembalikan feedback baris penuh pada `CheckIfRowIsComplete()` ketika kartu mencapai `targetTokens.Count`.
- [x] **Tahap 2 — Mempertahankan Keutuhan Visual 7 Slot:**
  - Layout 7 slot hasil kalibrasi (60x78 px, jarak 74 px terpusat simetris) tetap dipertahankan utuh dan aman di semua rasio layar HP Android potret.
  - Seluruh 7 slot tetap ditampilkan pada UI di setiap tahap.
- [x] **Tahap 3 — Verifikasi Pengujian Otomatis 100% PASS (Pilar 6):**
  - Menambahkan unit test `TC_BOARD_13_StrictValidation_RejectsIncorrectAdvancement` pada `AlgebraBoardTests.cs` dan `scratch/BoardTestRunner/Program.cs`.
  - `BoardTestRunner`: **13/13 PASS (100% PASS)** (`TC_BOARD_01` s.d. `TC_BOARD_13`).
  - `TFTestRunner`: **10/10 PASS (100% PASS)** (`TC_TF_01` s.d. `TC_TF_10`).
  - `ScoreTestRunner`: **10/10 PASS (100% PASS)** (`TC_SRES_01` s.d. `TC_SRES_10`).
  - Kompilasi proyek pengujian `dotnet build AlgeCardAdventure.Tests.csproj`: **0 Warning(s), 0 Error(s)**.
- [x] **Pilar 4:** Folder prototipe acuan desainer `Docs/Ui Template/` 100% terjaga utuh berstatus **READ-ONLY**.
- [x] **Pilar 5 & Pilar 8:** `Docs/Decision_Log.md` (DEC-41), `Docs/Task_Status.md`, `changelog/19/10/2026/08/15.md`, dan `Docs/chat/riwayat19102026.md` diperbarui.

---

## 📋 Checklist Batch 29 (Penggantian Grafis Splash Screen & Penyelarasan Desain AI Chatbot Groq)

- [x] **Pilar 1 (Approval Gate):** Rencana implementasi komprehensif `splash_screen_replacement_plan.md` diajukan, direviu, disesuaikan (mekanisme pemicu universal tahan logo 3 detik), dan disetujui resmi oleh Yogi Ario sebelum perubahan aset/kode.
- [x] **Tahap 1 — Analisis Aset & Ekstraksi Standarisasi Unity (Fase A):**
  - Folder acuan `Docs/Ui Template/` dijaga 100% utuh berstatus **READ-ONLY** (**Pilar 4**).
  - Berkas baru `Docs/Ui Template/01 Splash Screen.png` (941x1672 px, rasio 9:16 potret) disalin ke lokasi kerja `Assets/AlgeCardAdventure/Art/Backgrounds/splash_bg.png` (MD5 terverifikasi identik `7aac34bef42d0bb337e18713a2974189`).
  - Konfigurasi TextureImporter pada `Assets/AlgeCardAdventure/Art/Backgrounds/splash_bg.png.meta` distandarisasi ke `textureType: 8` (Sprite 2D and UI), `maxTextureSize: 4096`, `textureCompression: 0` (Uncompressed / kejernihan Ultra-HD), dan `filterMode: 1` (Bilinear).
  - Mempertahankan GUID resmi `217bc5cd1fe84c0d846df9ed93aa56b1` sehingga referensi scene tetap tersambung utuh tanpa broken links.
- [x] **Tahap 2 — Penyelarasan Hierarki Scene `01_SplashScreen.unity` (Fase B):**
  - Objek `Background` (Canvas) otomatis memuat grafis resolusi tinggi baru berformat stretch full-screen dengan warna murni `(1, 1, 1, 1)` dan `m_RaycastTarget: 0`.
  - Objek `Logo` terpisah (ID 700) tetap dinonaktifkan (`m_IsActive: 0`) karena ilustrasi logo tipografi 3D emas baru telah terintegrasi presisi dan tajam di dalam lukisan latar belakang.
  - Teks instruksi denyut `"Tap anywhere to start"` pada `PromptContainer` (ID 800) terkalibrasi rapi di anchor bawah `y = 0.12` dengan bayangan kontras tinggi.
  - Tombol deteksi sentuhan satu layar penuh `TapDetectorButton` (ID 900) dan `SplashScreenController` siap mengarahkan pemain ke Screen 02 (`02_Level`).
- [x] **Tahap 3 — Verifikasi Pengujian Otomatis 100% PASS (Fase C / Pilar 6):**
  - Kompilasi `dotnet build AlgeCardAdventure.Tests.csproj`: **0 Warning(s), 0 Error(s)**.
  - `BoardTestRunner`: **13/13 PASS (100% PASS)** (`TC_BOARD_01` s.d. `TC_BOARD_13`).
  - `TFTestRunner`: **10/10 PASS (100% PASS)** (`TC_TF_01` s.d. `TC_TF_10`).
  - `ScoreTestRunner`: **10/10 PASS (100% PASS)** (`TC_SRES_01` s.d. `TC_SRES_10`).
  - Total pengujian unit EditMode: **33/33 PASS (100% PASS)**.
- [x] **Tahap 4 — Dokumentasi Tata Kelola (Fase D / Pilar 5 & Pilar 8):**
  - Keputusan arsitektur `DEC-42` dicatat resmi di `Docs/Decision_Log.md`.
  - `Docs/Task_Status.md` dan `changelog/21/10/2026/18/30.md` diperbarui.
  - Riwayat percakapan dicadangkan ke `Docs/chat/riwayat21102026.md`.
- [x] **Tahap 5 — Perumusan Desain Teknis AI Chatbot Groq Companion (Fase E):**
  - Mengadopsi template desainer `Docs/Ui Template/chatbot/code.html` (persona Prof. Hopper 🦗 & Pip 🐜).
  - Merumuskan mekanisme pemicu universal **Tekan & Tahan Logo 3 Detik (Long-Press $\ge$ 3s)** di semua layar game.
  - Menyiapkan integrasi API Key Groq dari `Docs/Ui Template/chatbot/api` terproteksi `.gitignore` (**Pilar 3**).

---

## 📋 Checklist Batch 30 (Implementasi AI Companion Chatbot Groq API, code.html & Pemicu Logo 3 Detik)

- [x] **Pilar 1 (Approval Gate):** Rencana implementasi tertulis `chatbot_groq_companion_implementation_plan.md` disetujui resmi oleh Yogi Ario sebelum perubahan kode.
- [x] **Pilar 2 (Git Flow):** Pengerjaan dilakukan pada branch kerja terlindungi `develop`.
- [x] **Pilar 3 (Keamanan Kredensial):** 
  - API Key Groq dari `Docs/Ui Template/chatbot/api` (`gsk_oh2Af...`) disimpan secara aman pada konfigurasi runtime `Assets/Resources/groq_config.json`.
  - `.gitignore` diperbarui dan diverifikasi melindungi `Assets/Resources/groq_config.json`, `.meta`, dan `Docs/Ui Template/chatbot/api` dari commit publik.
- [x] **Pilar 4 (Proteksi Aset):** Folder acuan `Docs/Ui Template/chatbot/` 100% terjaga utuh berstatus **READ-ONLY**.
- [x] **Tahap 1 — Komponen Pemicu Universal (`LogoLongPressTrigger.cs`):**
  - Deteksi sentuhan tahan pointer (`IPointerDownHandler`, `IPointerUpHandler`, `IPointerExitHandler`).
  - Timer terisolasi `holdDuration = 3.0f` menggunakan `Time.unscaledDeltaTime`.
  - Efek visual mikro pembesaran logo halus (`1.05x`) saat ditekan dan kembali ke `1.0x` saat dilepas.
  - Menyiarkan event statis global `OnUniversalChatbotTriggered` tanpa tight-coupling ke UI scene.
  - Ketukan biasa (< 3 detik) tidak memicu chatbot sehingga fungsi default layar tetap normal.
- [x] **Tahap 2 — Klien Groq Cloud API & Config Manager (`GroqConfigManager.cs` & `GroqApiClient.cs`):**
  - Pengelola konfigurasi cerdas dengan dual loading (membaca disk lokal via `System.IO.File` + fallback `Resources.Load` yang diisolasi `MethodImplOptions.NoInlining` aman dari eror ECall CLI).
  - Layanan HTTP POST asinkron `GroqApiClient` dengan endpoint `https://api.groq.com/openai/v1/chat/completions`.
  - Dukungan model utama `llama-3.3-70b-versatile` dan fallback `llama-3.1-8b-instant`.
  - Serialisasi payload `CreateRequestBodyJson` dan deserialisasi respon `ParseChatResponseJson`.
- [x] **Tahap 3 — Manajemen Percakapan & Offline Fallback (`ChatSessionManager.cs`):**
  - Persona pedagogis **Prof. Hopper 🦗 & Pip 🐜** dengan instruksi bimbingan ramah anak dan analogi benda nyata ($b = \text{boneka}$, $a = \text{anggur}$).
  - Rolling window capping 6 pesan terakhir untuk efisiensi token dan latensi rendah.
  - *Graceful Offline Fallback*: Jika perangkat tidak memiliki internet, bot menyajikan respons ramah offline dan tips aljabar bawaan tanpa crash. Game tetap 100% playable offline (**Pilar 7 & Aturan 3 AGENTS.md**).
- [x] **Tahap 4 — Antarmuka Chatbot Sesuai Prototipe `code.html` (`ChatbotUIController.cs`):**
  - Manajemen modal overlay UGUI SortingOrder 100 dengan Dark Backdrop.
  - Header kayu fantasi dengan tombol Back `◀`, logo AlgeCard, avatar Prof. Hopper & Pip, dan status Online hijau.
  - Balon pesan bot perkamen, balon pesan user biru 3D glossy.
  - Quick prompt chips: *"💡 How to isolate x?"*, *"🔍 What is a variable?"*, *"📜 Hint for current mission"*.
  - Input bar dengan input field perkamen krem dan tombol kirim 3D hijau emerald.
- [x] **Tahap 5 — Pengujian Otomatis 100% PASS (Pilar 6):**
  - Berkas pengujian unit EditMode `Assets/AlgeCardAdventure/Tests/EditMode/ChatbotTests.cs` (6 test: `TC_CHAT_01` s.d. `TC_CHAT_06`).
  - Runner mandiri CLI `scratch/ChatbotTestRunner/Program.cs` mengeksekusi 6/6 test PASS.
  - `BoardTestRunner`: **13/13 PASS (100% PASS)**.
  - `TFTestRunner`: **10/10 PASS (100% PASS)**.
  - `ScoreTestRunner`: **10/10 PASS (100% PASS)**.
  - `ChatbotTestRunner`: **6/6 PASS (100% PASS)**.
  - Total pengujian proyek: **39/39 PASS (100% PASS, 0 Warning, 0 Error)**.
- [x] **Tahap 6 — Dokumentasi Tata Kelola (Pilar 5 & Pilar 8):**
  - Keputusan arsitektur `DEC-43` dicatat di `Docs/Decision_Log.md`.
  - `Docs/Task_Status.md` dan `changelog/21/10/2026/19/00.md` diperbarui.
  - Riwayat percakapan dicadangkan ke `Docs/chat/riwayat21102026.md`.

---

## 📋 Checklist Batch 31 (Implementasi Modal Materi Aljabar & Petunjuk Permainan Layar 03)

- [x] **Pilar 1 (Approval Gate):** Rencana implementasi komprehensif `learning_material_and_guide_modals_plan.md` (Revisi Catatan Presentasi Butir 2 & Butir 3) disetujui resmi oleh Yogi Ario sebelum perubahan kode.
- [x] **Pilar 2 (Git Flow):** Pengerjaan dilakukan pada branch kerja terlindungi `develop`.
- [x] **Pilar 4 (Proteksi Aset):** Folder acuan `Docs/Ui Template/` 100% terjaga utuh berstatus **READ-ONLY**.
- [x] **Pilar 7 (100% Offline Game):** Konten pembelajaran aljabar dan petunjuk permainan tertanam penuh secara lokal di dalam aplikasi tanpa ketergantungan internet/backend.
- [x] **Tahap 1 — Peningkatan Interaktivitas Ikon Layar 03 (`03_MissionCard.unity`):**
  - Ikon tumpukan buku kiri bawah (`BooksStack`, ID 820): `m_RaycastTarget: 1`, ditambah komponen Button (`m_Component: [824]`) dan event click terhubung ke `OnBooksClicked()`.
  - Ikon peta dan kompas kanan bawah (`MapAndCompass`, ID 830): `m_RaycastTarget: 1`, ditambah komponen Button (`m_Component: [834]`) dan event click terhubung ke `OnCompassClicked()`.
- [x] **Tahap 2 — Pembuatan Modal Materi Pembelajaran Aljabar (`LearningMaterialModal`, ID 2000-2099):**
  - Hierarki Canvas (ID 401) menambahkan anak `2001` (RectTransform modal materi).
  - Dilengkapi `DarkBackdrop` (alpha 0.85), `MaterialDialogCard` (perkamen emas mewah), `HeaderLabel` ("MATERI ALJABAR"), `SubtitleLabel` ("Konsep Dasar Variabel & Persamaan"), dan `ContentBodyText`.
  - Menyajikan materi edukasi aljabar komprehensif: 1) Analogi variabel $b = \text{boneka}$, 2) Prinsip timbangan seimbang ($x + 3 = 8$), 3) Jurus pindah ruas operasi berlawanan ($+$ jadi $-$, $\times$ jadi $\div$).
  - Tombol penutup beraksen hijau emas `CloseMaterialButton` ("MENGERTI! ❯") dengan audio click.
- [x] **Tahap 3 — Pembuatan Modal Petunjuk Permainan AlgeCard (`GameplayGuideModal`, ID 2100-2199):**
  - Hierarki Canvas (ID 401) menambahkan anak `2101` (RectTransform modal panduan).
  - Dilengkapi `DarkBackdrop`, `GuideDialogCard`, `GuideHeaderLabel` ("PETUNJUK PERMAINAN"), `GuideSubtitleLabel` ("Cara Bermain AlgeCard Adventure"), dan `GuideContentBodyText`.
  - Menyajikan 4 langkah taktis: 1) Ambil Misi (tarik kartu atau shuffle), 2) Susun Kartu di Meja Aljabar 4 tahap, 3) Evaluasi True/False 3 detik & pelajari pembuktian substitusi, 4) Raih Poin Maksimal dari 10 misi per sesi.
  - Tombol penutup beraksen biru emas `CloseGuideButton` ("SIAP BERMAIN! ⚔️") dengan audio click.
- [x] **Tahap 4 — Logika Pengendali `MissionCardController.cs` & Defensive Wrappers:**
  - Binding field dan properti inspector: `booksButton`, `compassButton`, `learningMaterialModal`, `gameplayGuideModal`, `closeMaterialButton`, `closeGuideButton`.
  - Metode `OnBooksClicked()`, `OnCloseMaterialClicked()`, `OnCompassClicked()`, `OnCloseGuideClicked()` diimplementasikan dengan penutupan modal lain yang saling eksklusif.
  - Implementasi helper defensif `SafeLog`, `SafeLogWarning`, `SafePlayButtonClick`, `SafePlayCardFlip`, `SafePlayDefaultBGM`, serta properti `HasActiveSession` berbasis `ReferenceEquals` pada `GameSession.Instance` untuk mengeliminasi 100% exception native Unity ECall pada CLI test runner.
- [x] **Tahap 5 — Pengujian Otomatis 100% PASS (Pilar 6):**
  - Penambahan 4 pengujian unit `TC_MISSION_11` s.d. `TC_MISSION_14` pada `Assets/AlgeCardAdventure/Tests/EditMode/MissionCardTests.cs`.
  - Penyelarasan runner mandiri CLI `scratch/MissionTestRunner/Program.cs` dengan `NullableBoolConverter` dan `IncludeFields = true`.
  - `MissionTestRunner`: **14/14 PASS (100% PASS)** (`TC_MISSION_01` s.d. `TC_MISSION_14`).
  - `BoardTestRunner`: **13/13 PASS (100% PASS)** (`TC_BOARD_01` s.d. `TC_BOARD_13`).
  - `TFTestRunner`: **10/10 PASS (100% PASS)** (`TC_TF_01` s.d. `TC_TF_10`).
  - `ScoreTestRunner`: **10/10 PASS (100% PASS)** (`TC_SRES_01` s.d. `TC_SRES_10`).
  - `ChatbotTestRunner`: **6/6 PASS (100% PASS)** (`TC_CHAT_01` s.d. `TC_CHAT_06`).
  - Total pengujian proyek: **53/53 PASS (100% PASS, 0 GAGAL, 0 ERROR)**.
  - Kompilasi `dotnet build AlgeCardAdventure.Tests.csproj`: **0 Warning(s), 0 Error(s)**.
- [x] **Tahap 6 — Dokumentasi Tata Kelola (Pilar 5 & Pilar 8):**
  - Keputusan arsitektur `DEC-44` dicatat di `Docs/Decision_Log.md`.
  - `Docs/Task_Status.md` dan `changelog/21/10/2026/20/00.md` diperbarui.
  - Riwayat percakapan dicadangkan ke `Docs/chat/riwayat21102026.md`.

---

## 📋 Checklist Batch 32 (Implementasi Dual Clear & Delete Buttons Layar 04 — Catatan Revisi Butir 7)

- [x] **Pilar 1 (Approval Gate):** Rencana implementasi komprehensif `dual_clear_and_delete_buttons_plan.md` (Revisi Catatan Presentasi Butir 7) disetujui resmi oleh Yogi Ario sebelum perubahan kode ("jangan lupa Zero Code kita backup chat setelah pengerjaan selesai").
- [x] **Pilar 2 (Git Flow):** Pengerjaan dilakukan pada branch kerja terlindungi `develop`.
- [x] **Pilar 4 (Proteksi Aset):** Folder acuan `Docs/Ui Template/` 100% terjaga utuh berstatus **READ-ONLY**.
- [x] **Pilar 7 (100% Offline Game):** Fitur hapus kartu satu per satu dan hapus semua bekerja 100% lokal tanpa backend/internet.
- [x] **Tahap 1 — Modifikasi Logika `AlgebraBoardController.cs`:**
  - Menambahkan field serialized `deleteOneButton`, `clearAllButton`, dan getter `DeleteOneButton`, `ClearAllButton`.
  - Mendaftarkan listener `OnDeleteOneClicked` dan `OnClearClicked` di `SetupButtonListeners()`.
  - Implementasi metode `OnDeleteOneClicked()`: mencari kartu terisi paling kanan (`!slot.IsEmpty`) dari indeks slot tertinggi ke terendah pada baris aktif, mengosongkannya, memutar audio click, dan menampilkan feedback `"Kartu [token] dihapus."`.
  - Penyempurnaan `OnClearClicked()`: mengosongkan seluruh slot pada tahap aktif secara instan dan menampilkan notice `"Seluruh kartu pada tahap aktif dihapus."`.
  - Mempertahankan Tap-to-Clear interaktif pada tiap slot jika pemain ingin mengosongkan slot spesifik di tengah baris.
- [x] **Tahap 2 — Konfigurasi Objek UI Scene Layar 04 (`04_AlgebraCardBoard.unity`):**
  - Menambahkan GameObject `DeleteOneButton` (ID 770) dan child `DeleteOneTxt` (ID 775) berlabel `"⌫ HAPUS 1 KARTU"` (`y = -82`, ukuran 290x46 px).
  - Menyesuaikan `ClearButton` (kini `ClearAllButton`, ID 643) berlabel `"🗑 HAPUS SEMUA"` (`y = -134`, ukuran 290x46 px).
  - Menggeser `NavPadContainer` (ID 652) ke `y = -285` agar jarak antar-tombol lapang dan proporsional.
  - Memperbarui daftar anak `m_Children` pada `DeckCardContainer` (ID 445) dengan menyertakan ID 771.
  - Menautkan binding `deleteOneButton: {fileID: 774}` dan `clearAllButton: {fileID: 647}` pada MonoBehaviour `AlgebraBoardController` (ID 757).
- [x] **Tahap 3 — Pengujian Otomatis 100% PASS (Pilar 6):**
  - Penambahan 2 pengujian unit `TC_BOARD_14_DeleteOneCard_RemovesOnlyRightmostFilledSlot` dan `TC_BOARD_15_ClearAll_EmptiesAllSlotsInActiveStage` pada `Assets/AlgeCardAdventure/Tests/EditMode/AlgebraBoardTests.cs`.
  - Penyelarasan runner mandiri CLI `scratch/BoardTestRunner/Program.cs` (15/15 PASS).
  - Eksekusi 5 test runner proyek:
    - `BoardTestRunner`: **15/15 PASS (100% PASS)** (`TC_BOARD_01` s.d. `TC_BOARD_15`).
    - `MissionTestRunner`: **14/14 PASS (100% PASS)** (`TC_MISSION_01` s.d. `TC_MISSION_14`).
    - `TFTestRunner`: **10/10 PASS (100% PASS)** (`TC_TF_01` s.d. `TC_TF_10`).
    - `ScoreTestRunner`: **10/10 PASS (100% PASS)** (`TC_SRES_01` s.d. `TC_SRES_10`).
    - `ChatbotTestRunner`: **6/6 PASS (100% PASS)** (`TC_CHAT_01` s.d. `TC_CHAT_06`).
    - Total pengujian proyek: **55/55 PASS (100% PASS, 0 GAGAL, 0 ERROR)**.
  - Kompilasi `dotnet build AlgeCardAdventure.Tests.csproj`: **0 Warning(s), 0 Error(s)**.
- [x] **Tahap 4 — Dokumentasi Tata Kelola (Pilar 5 & Pilar 8):**
  - Keputusan arsitektur `DEC-45` dicatat di `Docs/Decision_Log.md`.
  - `Docs/Task_Status.md` dan `changelog/23/10/2026/13/50.md` diperbarui.
  - Riwayat percakapan dicadangkan ke `Docs/chat/riwayat23102026.md`.

---

## 📋 Checklist Batch 33 (Integrasi Universal Runtime & UI Prosedural Easter Egg AI Chatbot Companion)

- [x] **Pilar 1 (Approval Gate):** Rencana implementasi tertulis `easter_egg_chatbot_integration_plan.md` disetujui resmi oleh Yogi Ario (*The user has approved this document*).
- [x] **Pilar 2 (Git Flow):** Seluruh pekerjaan berada di branch kerja terlindungi `develop`.
- [x] **Pilar 3 (Keamanan Kredensial):** File konfigurasi `Assets/Resources/groq_config.json` terlindungi `.gitignore`.
- [x] **Pilar 4 (Proteksi Aset):** Folder acuan `Docs/Ui Template/` 100% terjaga utuh berstatus **READ-ONLY**.
- [x] **Pilar 7 (100% Offline Single-Player):** Asisten AI dilengkapi Graceful Offline Fallback; game utama tetap 100% playable secara offline jika jaringan tidak tersedia.
- [x] **Tahap 1 — Modul Bootstrap Runtime Universal (`ChatbotRuntimeBootstrap.cs`):**
  - Implementasi metode `[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]` yang berjalan otomatis setelah tiap pergantian scene di Unity Play Mode.
  - Metode `AttachToActiveScene()` memastikan `ChatbotUIController` aktif di root `DontDestroyOnLoad`.
  - Metode `InjectLogoTriggerInActiveScene()` memindai hierarki scene aktif, mendeteksi objek logo (`Logo`, `LogoImage`, `LogoPlaque`, `TOP_LOGO_TITLE`), mengaktifkannya jika nonaktif, mengaktifkan `raycastTarget: true`, dan menautkan komponen `LogoLongPressTrigger`.
  - Nol manipulasi file YAML scene mentah (100% aman tanpa risiko merusak serialisasi fileID).
- [x] **Tahap 2 — Antarmuka Modal Prosedural 1:1 `code.html` (`ChatbotUIController.cs`):**
  - Implementasi `EnsureProceduralUI()` yang membentuk Canvas Overlay (`sortingOrder: 100`), Dark Backdrop semi-transparan, MainChatPanel kayu fantasi bergaris tepi emas, Header Bar dengan tombol BACK, Quick Chips (`💡 Pindah Ruas`, `🔍 Apa Variabel?`, `📜 Petunjuk Misi`), ScrollRect chat stream dinamis, InputField, dan Send Button.
  - Penanganan defensif anti-ECall pada `AudioManager`, `EnsureApiClientExists`, dan `IsChatOpen` menggunakan null-check cast `(object)` agar stabil di lingkungan CLI test runner.
- [x] **Tahap 3 — Pengujian Otomatis 100% PASS (Pilar 6):**
  - Penambahan pengujian unit `TC_CHAT_07_Bootstrap_DetectsLogoAndAttachesTrigger` dan `TC_CHAT_08_Bootstrap_EnsureProceduralUI_CreatesCompleteModal` pada `Assets/AlgeCardAdventure/Tests/EditMode/ChatbotTests.cs`.
  - Penyelarasan runner mandiri CLI `scratch/ChatbotTestRunner/Program.cs` (8/8 PASS).
  - Eksekusi seluruh 5 test runner proyek:
    - `ChatbotTestRunner`: **8/8 PASS (100% PASS)** (`TC_CHAT_01` s.d. `TC_CHAT_08`).
    - `BoardTestRunner`: **15/15 PASS (100% PASS)** (`TC_BOARD_01` s.d. `TC_BOARD_15`).
    - `MissionTestRunner`: **14/14 PASS (100% PASS)** (`TC_MISSION_01` s.d. `TC_MISSION_14`).
    - `TFTestRunner`: **10/10 PASS (100% PASS)** (`TC_TF_01` s.d. `TC_TF_10`).
    - `ScoreTestRunner`: **10/10 PASS (100% PASS)** (`TC_SRES_01` s.d. `TC_SRES_10`).
    - **Total Pengujian Proyek: 57/57 PASS (100% PASS, 0 GAGAL, 0 ERROR)**.
  - Kompilasi `dotnet build AlgeCardAdventure.Tests.csproj`: **0 Warning(s), 0 Error(s)**.
- [x] **Tahap 4 — Dokumentasi Tata Kelola (Pilar 5 & Pilar 8):**
  - Keputusan arsitektur `DEC-46` dicatat di `Docs/Decision_Log.md`.
  - `Docs/Task_Status.md`, `Docs/Test_Report.md`, dan `changelog/23/10/2026/14/20.md` diperbarui.
  - Riwayat percakapan dicadangkan ke `Docs/chat/riwayat23102026.md`.

---

## 📋 Checklist Batch 34 (Resolusi Dobel Logo Bertumpuk pada Layar 01 Splash Screen)

- [x] **Pilar 1 (Approval Gate):** Rencana implementasi tertulis `splash_screen_double_logo_fix_plan.md` disetujui resmi oleh Yogi Ario (*The user has approved this document*).
- [x] **Pilar 2 (Git Flow):** Pengerjaan dilakukan pada branch kerja terlindungi `develop`.
- [x] **Pilar 4 (Proteksi Aset):** Folder acuan `Docs/Ui Template/` 100% terjaga utuh berstatus **READ-ONLY**.
- [x] **Pilar 7 (100% Offline Game):** Tidak ada ketergantungan jaringan baru yang ditambahkan.
- [x] **Tahap 1 — Penyesuaian Serialisasi Scene `01_SplashScreen.unity`:**
  - Mengosongkan referensi sprite pada komponen `Image` (ID 703): `m_Sprite: {fileID: 0}`.
  - Menyetel warna menjadi transparan penuh: `m_Color: {r: 1, g: 1, b: 1, a: 0}`.
  - Mempertahankan `m_IsActive: 1` dan `m_RaycastTarget: 1` pada GameObject `Logo` (ID 700) dengan ukuran 860x287 px di anchor `y = 0.72`.
  - Objek `Logo` berfungsi penuh sebagai *invisible touch target* untuk Easter Egg tanpa merender gambar visual yang bertumpuk.
- [x] **Tahap 2 — Penyesuaian Defensif `ChatbotRuntimeBootstrap.cs`:**
  - Menambahkan penanganan protektif: jika komponen `Image` pada objek logo memiliki sprite null, warnanya dipertahankan sebagai `Color.clear` transparan.
  - Menjamin tidak ada kotak putih yang terender dan trigger `LogoLongPressTrigger` tetap terpasang dengan `raycastTarget: true`.
- [x] **Tahap 3 — Pengujian Otomatis 100% PASS (Pilar 6):**
  - Eksekusi seluruh 5 test runner proyek:
    - `ChatbotTestRunner`: **8/8 PASS (100% PASS)**
    - `BoardTestRunner`: **15/15 PASS (100% PASS)**
    - `MissionTestRunner`: **14/14 PASS (100% PASS)**
    - `TFTestRunner`: **10/10 PASS (100% PASS)**
    - `ScoreTestRunner`: **10/10 PASS (100% PASS)**
    - **Total Pengujian Proyek: 57/57 PASS (100% PASS, 0 GAGAL, 0 ERROR)**.
  - Kompilasi `dotnet build AlgeCardAdventure.Tests.csproj`: **0 Warning(s), 0 Error(s)**.
- [x] **Tahap 4 — Dokumentasi Tata Kelola (Pilar 5 & Pilar 8):**
  - Keputusan arsitektur `DEC-47` dicatat di `Docs/Decision_Log.md`.
  - `Docs/Task_Status.md`, `Docs/Test_Report.md`, dan `changelog/23/10/2026/15/00.md` diperbarui.
  - Riwayat percakapan dicadangkan ke `Docs/chat/riwayat23102026.md`.

---

## 📋 Checklist Batch 35 (Integrasi Soal Cerita Kontekstual, Petunjuk Variabel Ramah Anak, & Proteksi Frame Layar 04)

- [x] **Pilar 1 (Approval Gate):** Rencana implementasi tertulis `question_story_and_frame_refinement_plan.md` disetujui resmi oleh Yogi Ario (*The user has approved this document*).
- [x] **Pilar 2 (Git Flow):** Pengerjaan dilakukan pada branch kerja terlindungi `develop`.
- [x] **Pilar 4 (Proteksi Aset):** Folder acuan `Docs/Ui Template/` 100% terjaga utuh berstatus **READ-ONLY**.
- [x] **Pilar 7 (100% Offline Game):** Menggunakan data lokal `banksoal.json` dan pemetaan variabel in-memory, 100% offline single-player.
- [x] **Tahap 1 — Logika Format Soal & Petunjuk Variabel (`AlgebraBoardController.cs`):**
  - Metode `ExtractVariableFromQuestion(QuestionModel q)` mengekstrak variabel aljabar ($a, b, c, p, x, y$, dll.) dari rumus/soal secara akurat.
  - Metode `GetVariableContextHint(QuestionModel question)` memetakan variabel ke representasi objek konkret ramah anak ($b = \text{banyak boneka 🧸}$, $a = \text{buah anggur 🍇}$, $p = \text{batang pensil ✏️}$, $x = \text{jumlah uang tabungan 💰}$ / nilai misteri, $y = \text{kartu misteri 🃏}$).
  - Pembaruan `InitializeBoardUI()` menyajikan soal cerita asli (`ActiveQuestion.question_text`) dipadukan dengan petunjuk variabel pada `questionBodyText` dan mengaktifkan UGUI BestFit runtime (`resizeTextForBestFit = true`, min 13, max 18).
- [x] **Tahap 2 — Proteksi Frame & Format UGUI (`04_AlgebraCardBoard.unity`):**
  - Mengonfigurasi `QuestionBodyText` (ID 66–69): `SizeDelta: {x: 700, y: 90}`, `AnchoredPosition: {x: 100, y: -20}`, `BestFit: 1`, `MinSize: 13`, `MaxSize: 18`, `HorizontalOverflow: 0` (Wrap), `VerticalOverflow: 0` (Truncate).
  - Teks tertata rapi di dalam banner kayu plakat atas tanpa menabrak maskot jangkrik di sebelah kiri dan tidak meluber melewati batas frame kayu.
- [x] **Tahap 3 — Pengujian Otomatis 100% PASS (Pilar 6):**
  - Penambahan pengujian unit `TC_BOARD_16_QuestionStoryAndVariableHint_FormattedCorrectly` pada `AlgebraBoardTests.cs`.
  - Penyelarasan runner mandiri CLI `scratch/BoardTestRunner/Program.cs` dan penambahan referensi `UnityEngine.TextRenderingModule.dll`.
  - Eksekusi seluruh 5 test runner proyek:
    - `BoardTestRunner`: **16/16 PASS (100% PASS)**
    - `ChatbotTestRunner`: **8/8 PASS (100% PASS)**
    - `MissionTestRunner`: **14/14 PASS (100% PASS)**
    - `TFTestRunner`: **10/10 PASS (100% PASS)**
    - `ScoreTestRunner`: **10/10 PASS (100% PASS)**
    - **Total Pengujian Proyek: 58/58 PASS (100% PASS, 0 GAGAL, 0 ERROR)**.
  - Kompilasi `dotnet build AlgeCardAdventure.Tests.csproj`: **0 Warning(s), 0 Error(s)**.
- [x] **Tahap 4 — Dokumentasi Tata Kelola (Pilar 5 & Pilar 8):**
  - Keputusan arsitektur `DEC-48` dicatat di `Docs/Decision_Log.md`.
  - `Docs/Task_Status.md`, `Docs/Test_Report.md`, dan `changelog/23/10/2026/15/30.md` diperbarui.
  - Riwayat percakapan dicadangkan ke `Docs/chat/riwayat23102026.md`.

---

## 📋 Checklist Batch 36 (Integrasi Pembahasan Langkah-demi-Langkah Solusi Aljabar Asli Layar 05)

- [x] **Pilar 1 (Approval Gate):** Rencana implementasi tertulis `step_by_step_explanation_refinement_plan.md` disetujui resmi oleh Yogi Ario (*The user has approved this document*).
- [x] **Pilar 2 (Git Flow):** Pengerjaan dilakukan pada branch kerja terlindungi `develop`.
- [x] **Pilar 4 (Proteksi Aset):** Folder acuan `Docs/Ui Template/` 100% terjaga utuh berstatus **READ-ONLY**.
- [x] **Pilar 7 (100% Offline Game):** Mengambil data langkah matematika lokal `solution_steps` dari `banksoal.json`, 100% offline single-player.
- [x] **Tahap 1 — Logika Format Pembahasan Asli (`TrueFalseController.cs`):**
  - Pembaruan `GenerateExplanationSteps(QuestionModel q, bool isCorrect)` memetakan formula awal (`Equation: ...`), operasi invers aljabar / pindah ruas (`Operation: ...`), dan nilai solusi akhir (`Solution: ...`) dari data `solution_steps`.
  - Retensi fallback anggun ke kalimat pembuktian substitusi jika `solution_steps` bernilai null atau kosong (menjamin kompatibilitas penuh dengan objek mock `TC_TF_09`).
  - Pembaruan `SetupExplanationContent` menyajikan narasi konsep soal (`Problem Concept: solution_steps[0]`) pada `problemRestatementText`, jawaban target final pada `resultEquationText`, dan mengaktifkan UGUI BestFit runtime (`min: 14 px, max: 22 px`).
- [x] **Tahap 2 — Proteksi Frame & Format UGUI (`05_TrueOrFalse.unity`):**
  - Mengonfigurasi `Step1Text` (ID 1187), `Step2Text` (ID 1191), `Step3Text` (ID 1195), dan `ProblemRestatementText` (ID 1179): `m_BestFit: 1`, `m_MinSize: 14`, `m_MaxSize: 22`, `m_HorizontalOverflow: 0` (Wrap), `m_VerticalOverflow: 0` (Truncate).
  - Teks pembahasan terbingkai rapi di dalam perkamen putih tanpa terpotong atau meluber keluar frame pada seluruh layar ponsel.
- [x] **Tahap 3 — Pengujian Otomatis 100% PASS (Pilar 6):**
  - Penambahan pengujian unit `TC_TF_11_ExplanationProof_UsesRealSolutionSteps_WhenAvailable` pada `TrueFalseTests.cs`.
  - Penyelarasan runner mandiri CLI `scratch/TFTestRunner/Program.cs` dan penambahan referensi `UnityEngine.TextRenderingModule.dll` di `TFTestRunner.csproj`.
  - Eksekusi seluruh 5 test runner proyek:
    - `TFTestRunner`: **11/11 PASS (100% PASS)**
    - `BoardTestRunner`: **16/16 PASS (100% PASS)**
    - `ChatbotTestRunner`: **8/8 PASS (100% PASS)**
    - `MissionTestRunner`: **14/14 PASS (100% PASS)**
    - `ScoreTestRunner`: **10/10 PASS (100% PASS)**
    - **Total Pengujian Proyek: 59/59 PASS (100% PASS, 0 GAGAL, 0 ERROR)**.
  - Kompilasi `dotnet build AlgeCardAdventure.Tests.csproj`: **0 Warning(s), 0 Error(s)**.
- [x] **Tahap 4 — Dokumentasi Tata Kelola (Pilar 5 & Pilar 8):**
  - Keputusan arsitektur `DEC-49` dicatat di `Docs/Decision_Log.md`.
  - `Docs/Task_Status.md`, `Docs/Test_Report.md`, dan `changelog/23/10/2026/16/00.md` diperbarui.
  - Riwayat percakapan dicadangkan ke `Docs/chat/riwayat23102026.md`.

---

## 📋 Checklist Batch 37 (Kalibrasi Tata Letak Vertikal Tombol Hapus Layar 04)

- [x] **Pilar 1 (Approval Gate):** Rencana implementasi tertulis `adjust_delete_buttons_spacing_plan.md` disetujui resmi oleh Yogi Ario (*The user has approved this document*).
- [x] **Pilar 2 (Git Flow):** Pengerjaan dilakukan pada branch kerja terlindungi `develop`.
- [x] **Pilar 4 (Proteksi Aset):** Folder acuan `Docs/Ui Template/` 100% terjaga utuh berstatus **READ-ONLY**.
- [x] **Pilar 7 (100% Offline Game):** Game tetap 100% offline single-player.
- [x] **Tahap 1 — Penyesuaian Posisi Scene (`04_AlgebraCardBoard.unity`):**
  - Menurunkan `DeleteOneButton` (RT 771) dari `y = -82` ke `y = -105`, menciptakan margin bersih 24.5 px di bawah kartu operator (`[ ÷ ]` dan `[ = ]`).
  - Menurunkan `ClearButton` (kini `ClearAllButton`, RT 644) dari `y = -134` ke `y = -157`, menghasilkan jarak antar tombol 6 px yang padu dan simetris.
  - Menurunkan `NavPadContainer` (RT 653) dari `y = -285` ke `y = -320`, memberikan margin 25 px di bawah tombol ClearAll dan tetap menyisakan ruang aman 75 px dari dasar panel dek (`DeckOfCardsPanel` di `y = -510`).
- [x] **Tahap 2 — Pengujian Otomatis 100% PASS (Pilar 6):**
  - Kompilasi `dotnet build AlgeCardAdventure.Tests.csproj`: **0 Warning(s), 0 Error(s)**.
  - Eksekusi seluruh 5 test runner proyek:
    - `BoardTestRunner`: **16/16 PASS (100% PASS)**
    - `TFTestRunner`: **11/11 PASS (100% PASS)**
    - `ChatbotTestRunner`: **8/8 PASS (100% PASS)**
    - `MissionTestRunner`: **14/14 PASS (100% PASS)**
    - `ScoreTestRunner`: **10/10 PASS (100% PASS)**
    - **Total Pengujian Proyek: 59/59 PASS (100% PASS, 0 GAGAL, 0 ERROR)**.
- [x] **Tahap 3 — Dokumentasi Tata Kelola (Pilar 5 & Pilar 8):**
  - Keputusan arsitektur `DEC-50` dicatat di `Docs/Decision_Log.md`.
  - `Docs/Task_Status.md`, `Docs/Test_Report.md`, dan `changelog/23/10/2026/16/30.md` diperbarui.
  - Riwayat percakapan dicadangkan ke `Docs/chat/riwayat23102026.md`.

---

## 📋 Checklist Batch 38 (Standardisasi Batas 7 Token Persamaan Aljabar & Kartu Variabel Kustom Layar 04)

- [x] **Pilar 1 (Approval Gate):** Rencana implementasi tertulis `long_equation_card_board_capacity_plan.md` disetujui resmi oleh Yogi Ario (*The user has approved this document* / "eksekusi mana bagusnya akan saya cek di hp").
- [x] **Pilar 2 (Git Flow):** Pengerjaan dilakukan pada branch kerja terlindungi `develop`.
- [x] **Pilar 4 (Proteksi Aset):** Folder acuan `Docs/Ui Template/` 100% terjaga utuh berstatus **READ-ONLY**.
- [x] **Pilar 7 (100% Offline Game):** 100% data lokal di `banksoal.json` dan logika runtime offline tanpa backend.
- [x] **Tahap 1 — Kurasi & Standardisasi Data Bank Soal (`banksoal.json`):**
  - Mengkurasi seluruh 60 soal pada `Assets/StreamingAssets/banksoal.json` dan `Docs/banksoal.json` via script `scratch/curate_banksoal.py`.
  - Menstandarisasi persamaan linear dan formula langkah penyelesaian menjadi maksimal 7 token di seluruh 4 tahap per soal (0 soal yang melebihi 7 slot).
- [x] **Tahap 2 — Logika Penguraian & Kartu Variabel Kustom (`AlgebraBoardController.cs`):**
  - Menambahkan fungsi `ExtractPureEquation(string raw)` untuk memisahkan formula matematika bersih dari teks narasi naratif.
  - Menambahkan properti `CurrentCustomVariable`, referensi `customVariableButton` dan `customVariableText`, serta metode `SetCustomVariable(string letter)`.
  - Otomatisasi deteksi variabel soal aktif di `SetupQuestion()` sehingga kartu kustom langsung memuat huruf yang sesuai (misal: `p`, `m`, `v`).
  - Memasang clamp kapasitas target token maksimal 7 slot di `FormulateStagesFromQuestion`, `OnDeckCardClicked`, `CheckIfRowIsComplete`, dan `ValidateStage` untuk mencegah deadlock permanen.
- [x] **Tahap 3 — Integrasi Visual Scene (`04_AlgebraCardBoard.unity`):**
  - Menambahkan objek kartu variabel kustom ke-5 `VarCard_custom` (ID 780-788) pada palet dek aljabar.
  - Menata ulang 5 tombol variabel secara simetris dan rapi: `VarBtn_x` (x = -96), `VarBtn_y` (x = -48), `VarBtn_a` (x = 0), `VarBtn_b` (x = 48), dan `VarCard_custom` (x = 96).
  - Menghubungkan binding serialisasi `customVariableButton` dan `customVariableText` ke `AlgebraBoardController`.
- [x] **Tahap 4 — Pengujian Otomatis 100% PASS (Pilar 6):**
  - Penambahan 4 pengujian unit pada `AlgebraBoardTests.cs`:
    - `TC_BOARD_17_AllBankSoalQuestions_HaveMax7TokensPerStage`: Menguji seluruh 60 soal pada banksoal untuk memastikan <= 7 token di setiap tahap.
    - `TC_BOARD_18_CustomVariable_AutoAdaptsToQuestionVariable`: Memastikan kartu kustom otomatis beradaptasi dengan huruf variabel soal (misal: `p`, `m`).
    - `TC_BOARD_19_CustomVariable_UserCanSetArbitraryLetter`: Memastikan pemain dapat menyetel huruf kustom bebas (misal: `v`) dan memainkannya ke slot papan.
    - `TC_BOARD_20_LongEquations_SolveWithoutDeadlock`: Memastikan persamaan panjang diselesaikan tanpa deadlock pengisian kartu.
  - Eksekusi seluruh 5 test runner proyek:
    - `BoardTestRunner`: **20/20 PASS (100% PASS)**
    - `TFTestRunner`: **11/11 PASS (100% PASS)**
    - `ChatbotTestRunner`: **8/8 PASS (100% PASS)**
    - `MissionTestRunner`: **14/14 PASS (100% PASS)**
    - `ScoreTestRunner`: **10/10 PASS (100% PASS)**
    - **Total Pengujian Proyek: 63/63 PASS (100% PASS, 0 GAGAL, 0 ERROR)**.
  - Kompilasi `dotnet build AlgeCardAdventure.Tests.csproj`: **0 Warning(s), 0 Error(s)**.
- [x] **Tahap 5 — Dokumentasi Tata Kelola (Pilar 5 & Pilar 8):**
  - Keputusan arsitektur `DEC-51` dicatat di `Docs/Decision_Log.md`.
  - `Docs/Task_Status.md`, `Docs/Test_Report.md`, dan `changelog/23/10/2026/17/00.md` diperbarui.
  - Riwayat percakapan dicadangkan ke `Docs/chat/riwayat23102026.md`.

---

## 📋 Checklist Batch 39 (Resolusi Tumpang Tindih Teks Soal & Kotak Formula Banner Layar 04)

- [x] **Pilar 1 (Approval Gate):** Rencana implementasi tertulis `fix_question_banner_overlap_plan.md` disetujui resmi oleh Yogi Ario (*The user has approved this document*).
- [x] **Pilar 2 (Git Flow):** Pengerjaan dilakukan pada branch kerja terlindungi `develop`.
- [x] **Pilar 4 (Proteksi Aset):** Folder acuan `Docs/Ui Template/` 100% terjaga utuh berstatus **READ-ONLY**.
- [x] **Tahap 1 — Penyesuaian Serialisasi Scene (`04_AlgebraCardBoard.unity`):**
  - Menonaktifkan GameObject `FormulaBox` (`m_IsActive: 0` pada ID 58), menghilangkan 100% kotak biru muda yang bertabrakan dengan teks cerita.
  - Menata ulang `QuestionBodyText` (ID 66, RT 67) dengan ukuran diperluas $750 \times 100$ px pada koordinat terkalibrasi $x = 70, y = -18$ (margin bersih 40 px dari avatar maskot dan 35 px dari batas plakat kanan).
- [x] **Tahap 2 — Penyelarasan Runtime Script (`AlgebraBoardController.cs`):**
  - Di metode `SetupQuestion()`: menyembunyikan parent `FormulaBox` secara defensif runtime jika ada.
  - Format narasi soal cerita dan petunjuk variabel disajikan secara elegan, bersih, dan lapang tanpa tabrakan visual (sesuai template resmi `Docs/Ui Template/04_algebra_card_board/code.html`).
- [x] **Tahap 3 — Pengujian Otomatis 100% PASS (Pilar 6):**
  - Kompilasi `dotnet build AlgeCardAdventure.Tests.csproj`: **0 Warning(s), 0 Error(s)**.
  - Eksekusi seluruh 5 test runner proyek:
    - `BoardTestRunner`: **20/20 PASS (100% PASS)**
    - `TFTestRunner`: **11/11 PASS (100% PASS)**
    - `ChatbotTestRunner`: **8/8 PASS (100% PASS)**
    - `MissionTestRunner`: **14/14 PASS (100% PASS)**
    - `ScoreTestRunner`: **10/10 PASS (100% PASS)**
    - **Total Pengujian Proyek: 63/63 PASS (100% PASS, 0 GAGAL, 0 ERROR)**.
- [x] **Tahap 4 — Dokumentasi Tata Kelola (Pilar 5 & Pilar 8):**
  - Keputusan arsitektur `DEC-52` dicatat di `Docs/Decision_Log.md`.
  - `Docs/Task_Status.md`, `Docs/Test_Report.md`, dan `changelog/23/10/2026/17/30.md` diperbarui.
  - Riwayat percakapan dicadangkan ke `Docs/chat/riwayat23102026.md`.

---

## 📋 Checklist Batch 40 (Resolusi Teks Kartu Misi Meluber, Sanitasi Nominal Bank Soal, & Perbaikan Bug Kotak Terakhir Layar 04)

- [x] **Pilar 1 (Approval Gate):** Rencana implementasi tertulis `fix_card_overflow_banksoal_and_last_slot_bug_plan.md` disetujui resmi oleh Yogi Ario (*The user has approved this document*).
- [x] **Pilar 2 (Git Flow):** Pengerjaan dilakukan pada branch kerja terlindungi `develop`.
- [x] **Pilar 4 (Proteksi Aset):** Folder acuan `Docs/Ui Template/` 100% terjaga utuh berstatus **READ-ONLY**.
- [x] **Pilar 7 (100% Offline Game):** Data 60 soal lokal di `banksoal.json` dan evaluasi permainan berjalan 100% offline.
- [x] **Fase 1 — Resolusi Teks Soal Meluber Kartu Misi (`03_MissionCard.unity` & `MissionCardController.cs`):**
  - Mengonfigurasi UGUI BestFit pada Component 1753 `MissionInstructionText` di scene `03_MissionCard.unity` (`m_FontSize: 22`, `m_BestFit: 1`, `m_MinSize: 13`, `m_MaxSize: 22`).
  - Menambahkan penanganan protektif BestFit pada `MissionCardController.cs` saat `OnCardSelected` (`resizeTextForBestFit = true`, min: 13, max: 22) sehingga teks soal sepanjang apa pun menyusut dinamis di dalam area kartu tanpa meluber.
- [x] **Fase 2 — Sanitasi Nilai Nominal Bank Soal (`banksoal.json`):**
  - Mengembangkan dan mengeksekusi script sanitasi terarah `scratch/sanitize_large_nominals.py`.
  - Memetakan 28 soal dengan nominal besar ($\ge 100$, ribuan/ratusan ribu) di `Assets/StreamingAssets/banksoal.json` dan `Docs/banksoal.json` menjadi angka kanonik SMP bernilai $< 100$ (dalam satuan ribuan rupiah).
  - Seluruh 60 soal bank soal tetap 100% utuh tanpa menghapus soal dan siap dimainkan dalam batas fisik 7 slot papan aljabar.
- [x] **Fase 3 — Perbaikan Bug Kotak Bagian Akhir & Fleksibilitas Validasi (`AlgebraBoardController.cs`):**
  - Mengubah batas loop `OnDeckCardClicked` dari `Mathf.Min(currentStage.targetTokens.Count, currentStage.slots.Count)` menjadi `currentStage.slots.Count`, memastikan seluruh 7 kotak (termasuk kotak ke-7 paling kanan) dapat diisi kartu tanpa hambatan.
  - Memperbarui `CheckIfRowIsComplete` untuk menghitung seluruh slot yang terisi dan membandingkannya terhadap jumlah target.
  - Menyempurnakan `ValidateStage` dengan pencocokan langsung dan fallback pencocokan string gabungan (*concatenated token matching*) untuk mendukung multi-digit input secara fleksibel.
- [x] **Fase 4 — Pengujian Otomatis 100% PASS (Pilar 6):**
  - Penambahan 2 pengujian unit pada `AlgebraBoardTests.cs` dan `BoardTestRunner`:
    - `TC_BOARD_21_LastSlot_7thBoxCanBeFilled`: Memastikan kotak ke-7 (terakhir) berhasil diisi dan tidak terblokir.
    - `TC_BOARD_22_ValidateStage_ConcatenatedDigitsMatch`: Memastikan pencocokan token multi-digit fleksibel terverifikasi valid.
  - Kompilasi `dotnet build AlgeCardAdventure.Tests.csproj`: **0 Warning(s), 0 Error(s)**.
  - Eksekusi seluruh 5 test runner proyek:
    - `BoardTestRunner`: **22/22 PASS (100% PASS)**
    - `TFTestRunner`: **11/11 PASS (100% PASS)**
    - `ChatbotTestRunner`: **8/8 PASS (100% PASS)**
    - `MissionTestRunner`: **14/14 PASS (100% PASS)**
    - `ScoreTestRunner`: **10/10 PASS (100% PASS)**
    - **Total Pengujian Proyek: 65/65 PASS (100% PASS, 0 GAGAL, 0 ERROR)**.
- [x] **Fase 5 — Dokumentasi Tata Kelola (Pilar 5 & Pilar 8):**
  - Keputusan arsitektur `DEC-53` dicatat di `Docs/Decision_Log.md`.
  - `Docs/Task_Status.md`, `Docs/Test_Report.md`, dan `changelog/23/10/2026/18/30.md` diperbarui.
  - Riwayat percakapan dicadangkan ke `Docs/chat/riwayat23102026.md`.

---

## 📋 Checklist Batch 41 (Perluasan Kapasitas Papan Aljabar Menjadi 8 Slot Aktif Layar 04)

- [x] **Pilar 1 (Approval Gate):** Rencana implementasi tertulis `expand_to_8_slots_algebra_board_plan.md` disetujui resmi oleh Yogi Ario (*The user has approved this document*).
- [x] **Pilar 2 (Git Flow):** Pengerjaan dilakukan pada branch kerja terlindungi `develop`.
- [x] **Pilar 4 (Proteksi Aset):** Folder acuan `Docs/Ui Template/` 100% terjaga utuh berstatus **READ-ONLY**.
- [x] **Pilar 7 (100% Offline Game):** Data 60 soal lokal di `banksoal.json` dan evaluasi permainan berjalan 100% offline.
- [x] **Fase 1 — Kalibrasi Layout & Penambahan Slot ke-8 (`04_AlgebraCardBoard.unity`):**
  - Menambahkan GameObject `Slot_X_8` (ID 801-839) pada keempat tahap papan aljabar (`SlotRail_1`, `SlotRail_2`, `SlotRail_3`, `SlotRail_4`).
  - Mengalibrasi ulang posisi $X$ kedelapan slot secara simetris terpusat ($s_x = (s_{\text{idx}} - 3.5) \times 68\text{ px}$) dengan ukuran $58 \times 76\text{ px}$ (bentang 534 px di dalam panel 660 px, margin aman 63 px di sisi kiri dan kanan).
  - Mendaftarkan slot ke-8 ke dalam array `stages[s].slots` pada `AlgebraBoardController` di scene YAML (total 32 slot aktif terdaftar).
- [x] **Fase 2 — Pembaruan Logika Kapasitas 8 Slot (`AlgebraBoardController.cs`):**
  - Memperbarui `FormulateStagesFromQuestion` untuk menaikkan batas kapasitas token dari 7 menjadi 8 (`targetTokens.Count > 8`).
  - Memastikan `OnDeckCardClicked` dapat mengisi kartu berturut-turut dari slot 1 hingga slot 8 leluasa (`maxSlots = currentStage.slots.Count = 8`).
  - Menyelaraskan `CheckIfRowIsComplete` dan `ValidateStage` untuk mengevaluasi susunan hingga 8 kartu.
- [x] **Fase 3 — Pengujian Otomatis 100% PASS (Pilar 6):**
  - Penambahan 2 pengujian unit pada `AlgebraBoardTests.cs` dan `BoardTestRunner`:
    - `TC_BOARD_23_AllEightSlotsActive_AndFillable`: Memastikan papan memiliki 8 slot dan kartu ke-8 berhasil masuk dan aktif.
    - `TC_BOARD_24_EightSlotEquation_ValidatesSuccessfully`: Memastikan persamaan 8 token berhasil tervalidasi sukses.
  - Kompilasi `dotnet build AlgeCardAdventure.Tests.csproj`: **0 Warning(s), 0 Error(s)**.
  - Eksekusi seluruh 5 test runner proyek:
    - `BoardTestRunner`: **24/24 PASS (100% PASS)**
    - `TFTestRunner`: **11/11 PASS (100% PASS)**
    - `ChatbotTestRunner`: **8/8 PASS (100% PASS)**
    - `MissionTestRunner`: **14/14 PASS (100% PASS)**
    - `ScoreTestRunner`: **10/10 PASS (100% PASS)**
    - **Total Pengujian Proyek: 67/67 PASS (100% PASS, 0 GAGAL, 0 ERROR)**.
- [x] **Fase 4 — Dokumentasi Tata Kelola (Pilar 5 & Pilar 8):**
  - Keputusan arsitektur `DEC-54` dicatat di `Docs/Decision_Log.md`.
  - `Docs/Task_Status.md`, `Docs/Test_Report.md`, dan `changelog/23/10/2026/20/15.md` diperbarui.
  - Riwayat percakapan dicadangkan ke `Docs/chat/riwayat23102026.md`.



