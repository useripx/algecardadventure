# Decision Log — AlgeCard Adventure

> **STATUS:** Catatan Resmi Keputusan Arsitektur & Teknis  
> **PEMILIK PROYEK:** Yogi Ario  

Dokumen ini mencatat seluruh keputusan arsitektural, teknis, dan operasional penting yang telah disetujui selama pengembangan proyek AlgeCard Adventure sesuai **Pilar 5 Standar Kerja Universal**.

---

## 🏛️ Log Keputusan yang Ditetapkan

| ID | Tanggal | Topik | Keputusan | Rationale / Alasan | Status Persetujuan |
|---|---|---|---|---|:---:|
| **DEC-01** | 17/10/2026 | Engine & Platform | Menggunakan Unity `6000.6.6f1` Universal 2D untuk target Android Mobile. | Platform utama game edukasi seluler dengan rendering performan URP 2D. | **APPROVED** |
| **DEC-02** | 17/10/2026 | Orientasi Layar | Orientasi terkunci Portrait (`defaultScreenOrientation: 0`). | Mengikuti standar rancangan UI mockup dan kenyamanan penggunaan satu tangan siswa. | **APPROVED** |
| **DEC-03** | 17/10/2026 | Label Tingkat Level | Tepat tiga opsi: `Easy`, `Normal`, `Hard`. Tidak menggunakan label 'Grade/Class 7/8/9' atau 'Medium'. | Standarisasi kompetisi internasional dan kemudahan navigasi siswa. | **APPROVED** |
| **DEC-04** | 17/10/2026 | Pemetaan Level Data | Data level `Medium` pada dokumen sumber dipetakan menjadi `Normal` pada UI. | Menjaga konsistensi data sumber tanpa mengubah isi matematis soal. | **APPROVED** |
| **DEC-05** | 17/10/2026 | Sumber Soal Tunggal | `banksoal.json` adalah Single Source of Truth konten soal. Dilarang hardcode soal di script/scene. | Kemudahan maintenance, pengujian terpusat, dan pemisahan data dari logic. | **APPROVED** |
| **DEC-06** | 17/10/2026 | Bahasa UI Pemain | Seluruh teks yang terlihat oleh pemain menggunakan bahasa Inggris secara konsisten. | Kebutuhan kompetisi internasional. Dokumen internal pengembang berbahasa Indonesia. | **APPROVED** |
| **DEC-07** | 17/10/2026 | Batch Kartu Misi | Maksimal 5 kartu tertutup per batch; pemain memilih 1 kartu. Soal yang sama diteruskan ke board. | Memberikan sensasi kejutan petualangan tanpa mengubah konteks soal. | **APPROVED** |
| **DEC-08** | 17/10/2026 | Batas Sesi | Maksimal 10 soal dijawab per sesi. Empat tahap board adalah 1 misi tunggal, bukan 4 soal. | Menjaga durasi bermain edukatif yang ideal dan tidak membosankan bagi siswa. | **APPROVED** |
| **DEC-09** | 17/10/2026 | Formula Skor | $\text{Score} = \text{Round}\left(\frac{\text{Benar}}{\text{Dijawab}} \times 100\right)$ (bilangan bulat). Salah tetap dihitung sebagai soal dijawab. | Skor berbasis rasio akurasi kumulatif yang adil dan transparan. | **APPROVED** |
| **DEC-10** | 17/10/2026 | Anti-Duplikasi Soal | Soal yang sudah selesai dijawab tidak boleh muncul kembali dalam sesi yang sama. | Mencegah pengulangan soal yang sama dalam 1 sesi 10 soal. | **APPROVED** |
| **DEC-11** | 17/10/2026 | Feedback True/False | Pilihan pemain dikunci setelah submit, feedback tampil 3 detik, baru kemudian explanation. | Mencegah double-scoring dan memberi jeda refleksi bagi pemain. | **APPROVED** |
| **DEC-12** | 17/10/2026 | Exit Dialog | Keluar sesi memerlukan popup konfirmasi; jika disetujui, sesi aktif direset dan kembali ke Splash. | Melindungi pemain dari ketidaksengajaan memencet tombol keluar. | **APPROVED** |
| **DEC-13** | 17/10/2026 | Review Gate UI | Pengembangan UI dikerjakan bertahap satu per satu (Screen 01 s.d. 06) dengan persetujuan Yogi. | Memastikan kualitas visual dan interaksi terverifikasi sebelum lanjut. | **APPROVED** |
| **DEC-14** | 17/10/2026 | Proteksi Aset & Git | `Docs/Ui Template/` berstatus Read-Only; branch `main` terlindungi, AI bekerja di `develop`. | Mencegah kerusakan aset asli dan menjamin stabilitas rilis produksi. | **APPROVED** |

---

## 🔍 Keputusan Terbuka (Open Decisions Resolution)

| ID | Topik | Keputusan yang Disetujui | Rationale & Dampak |
|---|---|---|---|
| **OPEN-01** | Pemilihan Topik | **Opsi A (MVP Level-Only):** Pemain hanya memilih tingkat kesulitan (`Easy`, `Normal`, `Hard`), langsung masuk ke kartu misi tanpa filter topik. | `banksoal.json` saat ini belum memiliki field `topic_id`. Opsi A menghindari asumsi fiktif. |
| **OPEN-02** | Board Stage Mapping | **Opsi A (Pilot Curated Mapping):** Menggunakan 1 soal pilot terkurasi (persamaan kanonis `3x + 5 = 20`) dengan pemetaan slot validasi eksplisit. | Menghindari risiko crash dari runtime algebra parser universal yang belum teruji. |
| **OPEN-03** | Konten True/False | **Opsi A (Null-Safe & Pilot Content):** Repository menangani `null` dengan aman; konten T/F untuk soal pilot disusun dan divalidasi matematis sebelum Screen 05. | Memastikan game engine tidak crash saat memuat 60 soal yang field T/F-nya bernilai `null`. |
| **OPEN-04** | 4 Soal Flagged | **Opsi A (Auto-Filter Flagged Questions):** `QuestionRepository` otomatis mengecualikan 4 soal flagged (`NORMAL-005`, `NORMAL-007`, `NORMAL-020`, `HARD-006`) dari pool playable. | Menjamin 100% soal yang dimainkan siswa bebas dari keraguan matematis/redaksi. |
