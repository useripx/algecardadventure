<p align="center">
  <img src="Docs/Ui Template/logo.png/screen.png" alt="AlgeCard Adventure Logo" width="380" />
</p>

<h1 align="center">AlgeCard Adventure</h1>

<p align="center">
  <strong>Game Edukasi Matematika Berbasis Kartu Aljabar untuk Siswa Sekolah Menengah</strong>
</p>

<p align="center">
  <a href="https://github.com/useripx/algecardadventure"><img src="https://img.shields.io/github/repo-size/useripx/algecardadventure?color=blue&logo=github" alt="Repo Size" /></a>
  <img src="https://img.shields.io/badge/Unity-6000.6.6f1-black?logo=unity" alt="Unity Version" />
  <img src="https://img.shields.io/badge/Render%20Pipeline-Universal%202D%20(URP)-0078D4?logo=unity" alt="URP 2D" />
  <img src="https://img.shields.io/badge/Platform-Android%20Mobile-brightgreen?logo=android" alt="Platform Android" />
  <img src="https://img.shields.io/badge/Language-C%23-239120?logo=csharp" alt="Language C#" />
  <img src="https://img.shields.io/badge/Mode-Offline%20Single--Player-orange" alt="Offline Mode" />
  <img src="https://img.shields.io/badge/Orientation-Portrait-purple" alt="Portrait" />
  <img src="https://img.shields.io/badge/License-Proprietary-red" alt="License" />
</p>

---

## 📖 Tentang Proyek

**AlgeCard Adventure** adalah permainan edukasi matematika 2D portrait untuk perangkat Android. Menggabungkan konsep petualangan fantasi kartun cerah, panel kayu/perkamen, kartu angka dan variabel, serta maskot jangkrik hijau dan oranye untuk membantu siswa memahami persamaan linear aljabar langkah-demi-langkah.

Game ini dirancang murni **offline, single-player**, mandiri tanpa ketergantungan server/cloud, dengan seluruh teks antarmuka (*in-game UI*) berbahasa Inggris untuk standar kompetisi internasional.

---

## 🎮 Alur Permainan (Official Game Flowchart)

Dokumentasi rancang bangun sistem dan spesifikasi edukasi lengkap tersedia di **[Docs/Game_Description.md](Docs/Game_Description.md)**.

```mermaid
flowchart TD
    %% Styling Classes
    classDef startEnd fill:#2563eb,stroke:#1d4ed8,stroke-width:2px,color:#fff;
    classDef screen fill:#f8fafc,stroke:#334155,stroke-width:2px,color:#0f172a;
    classDef process fill:#fef3c7,stroke:#d97706,stroke-width:2px,color:#78350f;
    classDef condition fill:#e0e7ff,stroke:#4338ca,stroke-width:2px,color:#1e1b4b;
    classDef modal fill:#f1f5f9,stroke:#64748b,stroke-width:1.5px,stroke-dasharray: 5 5,color:#334155;

    Start([Game Launch / Start]):::startEnd --> S1[01 Splash Screen<br/>Tap Anywhere to Start]:::screen
    
    S1 --> S2[02 Level Selection<br/>Choose Easy, Normal, or Hard]:::screen
    S2 -->|Player Selects Difficulty| S3[03 Mission Cards<br/>5 Face-Down Cards Displayed]:::screen
    S2 -->|Back Button| S1
    
    S3 -->|Shuffle Button| S3
    S3 -->|Player Selects 1 Card| M_Card[Mission Card Revealed Modal<br/>Displays Question, Points, & Difficulty]:::modal
    M_Card -->|Continue Button| S4[04 Algebra Card Board<br/>Linear Equation Workspace]:::screen
    
    subgraph BoardSection [Algebra Board: 4-Stage Equation Solving]
        S4 --> Stg1[Stage 1: Build Initial Equation]:::process
        Stg1 -->|Row Solved & Validated| Stg2[Stage 2: Algebra Operation Step 1]:::process
        Stg2 -->|Row Solved & Validated| Stg3[Stage 3: Algebra Operation Step 2]:::process
        Stg3 -->|Row Solved & Validated| Stg4[Stage 4: Final Variable Result]:::process
        
        Stg4 -->|Complete Quest Button| AutoEval[System Automatically Evaluates Board Solution]:::process
    end
    
    AutoEval --> S5[05 True or False<br/>Automatic Evaluation & Step-by-Step Explanation]:::screen
    
    subgraph TFSection [Evaluation & Mathematical Explanation]
        S5 --> CheckAnswer{Is Board Solution Correct?}:::condition
        CheckAnswer -->|Yes| BadgeTrue[Show Green Status Badge: √ TRUE]:::process
        CheckAnswer -->|No| BadgeFalse[Show Red Status Badge: ✕ FALSE]:::process
        
        BadgeTrue --> Timer3s[3-Second Timer / Tap Screen to Skip]:::process
        BadgeFalse --> Timer3s
        
        Timer3s --> RevealExpl[Reveal Step-by-Step Proof & Substitution: LHS = RHS]:::process
        RevealExpl --> UpdateScore[Record Cumulative Score & Session Streak]:::process
        UpdateScore --> BtnNext[Player Taps NEXT Button]:::process
    end
    
    BtnNext --> S6[06 Score Results<br/>Adventure Results & Statistics Overview]:::screen
    
    subgraph ScoreSection [Score Results & Session Loop]
        S6 --> CheckLimit{Questions Answered < 10?}:::condition
        CheckLimit -->|Yes: Total < 10| BtnContQ[CONTINUE QUESTION Button<br/>Select Next Mission Card]:::process
        BtnContQ --> S3
        
        CheckLimit -->|No: Total = 10 Max| BtnPlayAgain[PLAY AGAIN Button<br/>Reset Session & Pick Difficulty]:::process
        BtnPlayAgain --> S2
        
        S6 -->|EXIT Button| M_Credits[Credits & Attribution Modal<br/>Yogi Ario, Nisa Aprilia Wati, Dr. Resty Wulanningrum]:::modal
        M_Credits -->|Close Credits Button| ResetSessionSplash[Reset Session & Return to Splash Screen]:::process
        ResetSessionSplash --> S1
    end
```

1. **01 Splash Screen:** Layar pembuka dengan logo resmi dan interaksi *“Tap anywhere to start”*.
2. **02 Level Selection:** Pilihan tingkat kesulitan (`Easy`, `Normal`, `Hard`).
3. **03 Mission Cards:** Batch berisi 5 kartu misi tertutup yang diacak dari bank soal eligible. Pemain memilih 1 kartu.
4. **04 Algebra Card Board:** Papan kerja penyelesaian persamaan bertahap (4 tahap: Membangun persamaan, operasi langkah 1, operasi langkah 2, dan hasil akhir variabel). Tersedia dek kartu angka (0-9), variabel (x, y, a, b), operator (+, -, ×, ÷, =), tombol 3D Clear, dan navigasi D-Pad.
5. **05 True or False:** Evaluasi otomatis kebenaran pengerjaan papan aljabar, status badge √ TRUE / ✕ FALSE, timer feedback 3 detik, penjelasan pembuktian substitusi, dan tombol `NEXT ❯` menuju hasil nilai.
6. **06 Score Results:** Ringkasan hasil petualangan, persentase skor kumulatif, kartu performa, navigasi lanjut misi (soal < 10), main ulang (soal = 10), dan tombol Exit dengan modal Credits resmi.

---

## 🛠️ Spesifikasi Teknologi

- **Engine:** Unity `6000.6.6f1` (Universal 2D)
- **Render Pipeline:** Universal Render Pipeline (URP 17.6.0)
- **Bahasa Pemrograman:** C# (.NET Standard 2.1)
- **UI Framework:** Unity UI (uGUI 2.6.0) & TextMeshPro
- **Testing Framework:** Unity Test Framework (UTF 1.8.0 - EditMode & PlayMode)
- **Target Platform:** Android Mobile (Portrait Orientation)
- **Sumber Data:** Lokal `Assets/StreamingAssets/banksoal.json` (60 Soal: 20 Easy, 20 Normal, 20 Hard)

---

## 🏛️ Tata Kelola & Standar Kerja (Universal Governance)

Proyek ini mematuhi **8 Pilar Standar Kerja Universal**:

- **Pilar 1 (ZERO-CODE Approval Gate):** Tidak ada perubahan kode atau konfigurasi tanpa rencana tertulis yang disetujui pemilik proyek.
- **Pilar 2 (Git Branching Strategy):** Branch `main` adalah produksi tertutup (hanya di-merge oleh Yogi Ario). Seluruh integrasi dikerjakan di branch `develop` atau `feature/*`.
- **Pilar 3 (Zero Secret Leakage):** Dilindungi `.gitignore` standar sejak hari pertama.
- **Pilar 4 (Asset Protection):** Folder [Docs/Ui Template/](Docs/Ui%20Template) berstatus **READ-ONLY**.
- **Pilar 5 (Documentation-First):** Dilengkapi [Task Status](Docs/Task_Status.md), [Decision Log](Docs/Decision_Log.md), dan [Changelog](changelog/).
- **Pilar 6 (100% Automated Testing Gate):** Seluruh logika inti diverifikasi dengan Unit Test otomatis sebelum rilis batch.
- **Pilar 7 (Lean & Resilient):** Game berjalan 100% offline tanpa ketergantungan pihak ketiga.
- **Pilar 8 (Proactive & Consultative):** Komunikasi transparan berbasis opsi dan pencadangan riwayat sesi secara berkala.

---

## 🚀 Panduan Memulai (Development Setup)

1. Pastikan **Unity Hub** dan Unity Editor versi **`6000.6.6f1`** dengan modul **Android Build Support** terpasang.
2. Clone repositori:
   ```bash
   git clone https://github.com/useripx/algecardadventure.git
   cd algecardadventure
   git checkout develop
   ```
3. Buka proyek melalui Unity Hub.
4. Jalankan pengujian otomatis melalui menu Unity:
   `Window` → `General` → `Test Runner` → pilih tab `EditMode` → klik `Run All`.

---

## 👤 Pemilik Proyek & Lisensi

- **Project Owner & Lead Developer:** Yogi Ario
- **Hak Cipta:** &copy; 2026 AlgeCard Adventure. Seluruh hak cipta dilindungi.
