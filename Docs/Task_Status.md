# Task Status — AlgeCard Adventure

> **Terakhir Diperbarui:** 17 Oktober 2026 15:30 WIB  
> **Status Keseluruhan Proyek:** Phase 3 Selesai — Screen 01 Splash Screen Siap Ditinjau  
> **Total Progres:** ~35% (Fondasi Workspace, Git, Data Layer, UI Portrait, & Screen 01)  

---

## 📊 Matriks Progres Fase Proyek

| Fase | Deskripsi | Status | Selesai | Catatan & Bukti |
|:---:|---|:---:|:---:|---|
| **Phase 0** | Preflight Workspace, Audit Git & Aturan | **DONE** | 100% | Laporan preflight disetujui Yogi, mode read-only sukses. |
| **Phase 1** | Setup Git, Baseline Dokumen, Data Repository & Test | **DONE** | 100% | Git branch develop aktif & terdorong ke GitHub. Data repository & 12 test PASS. |
| **Phase 2** | Fondasi UI Portrait & Safe Area | **DONE** | 100% | Orientation terkunci Portrait (0), SafeAreaFitter & Canvas Scaler 1080x1920 siap. |
| **Phase 3** | Screen 01 — Splash Screen | **DONE** | 100% | Scene 01_SplashScreen.unity siap, logo, background, pulsing text, tap-to-start aktif. |
| **Phase 4** | Screen 02 — Level Selection | PENDING | 0% | Menunggu approval review Screen 01 dari Yogi. |
| **Phase 5** | Screen 03 — Mission Cards & Shuffle | PENDING | 0% | Integrasi QuestionRepository ke kartu misi. |
| **Phase 6** | Screen 04 — Algebra Card Board | PENDING | 0% | Menggunakan soal pilot terkurasi (OPEN-02). |
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
- [x] Unit test `SafeAreaAndNavigationTests.cs` dibuat (TC-UI-01 s.d. 03 dan TC-NAV-01 s.d. 02).
- [x] Seluruh 17 kasus uji otomatis diverifikasi **100% PASS (0 GAGAL / 0 ERROR)** (**Pilar 6**).
- [x] Keputusan `DEC-15` (Credits Scene & Atribusi) dicatat di `Docs/Decision_Log.md`.
- [x] Dokumentasi `Docs/Task_Status.md` dan `Docs/Test_Report.md` diperbarui (**Pilar 5**).
- [x] Changelog `changelog/17/10/2026/15/30.md` dibuat (**Pilar 5**).
