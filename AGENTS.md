# AGENTS.md — AI Agent Governance Index

> **STATUS:** Berkas Indeks Resmi AI Agent  
> **PEMILIK PROYEK:** Yogi Ario  
> **PANDUAN UTAMA:** [Docs/Rules/universal-rule.md](Docs/Rules/universal-rule.md) & [Docs/Rules/AGENTS.md](Docs/Rules/AGENTS.md)  
> **PRINSIP UTAMA:** *No Plan, No Code. No Approval, No Code.*

Seluruh AI Agent (Antigravity, Gemini, Claude, Cursor, dll.) yang bekerja pada repositori **AlgeCard Adventure** WAJIB membaca, memahami, dan mematuhi aturan kerja pada dokumen:

1. **[Docs/Rules/universal-rule.md](Docs/Rules/universal-rule.md)** — Kerangka Standar Kerja Universal & 8 Pilar Governance.
2. **[Docs/Rules/AGENTS.md](Docs/Rules/AGENTS.md)** — Instruksi Kerja Spesifik Proyek AlgeCard Adventure & Aturan Gameplay.
3. **[Docs/Rules/GEMINI.md](Docs/Rules/GEMINI.md)** — Rangkuman Eksekutif Aturan Kerja AI.
4. **[Docs/Implementation_plan.md](Docs/Implementation_plan.md)** — Rencana Induk Implementasi Proyek.

### Aturan Pokok yang Wajib Diingat:
- **Pilar 1:** DILARANG KERAS membuat atau mengedit kode sebelum rencana tertulis disetujui eksplisit oleh Yogi Ario.
- **Pilar 2:** Branch `main` tertutup untuk commit/push langsung AI Agent. Semua pekerjaan di branch `develop` atau `feature/*`.
- **Pilar 3:** File rahasia dan konfigurasi lokal terlindungi `.gitignore`.
- **Pilar 4:** Folder `Docs/Ui Template/` berstatus **READ-ONLY**.
- **Pilar 5:** Perbarui `Docs/Task_Status.md`, `Docs/Decision_Log.md`, dan `changelog/` pada setiap batch selesai.
- **Pilar 6:** Definisi selesai mensyaratkan **100% PASS** pada test suite otomatis.
- **Pilar 7:** Game offline single-player tanpa backend/server/cloud.
- **Pilar 8:** Tanya pencadangan riwayat chat ke `Docs/chat/riwayat<ddmmyyyy>.md` di setiap penutup respon.
