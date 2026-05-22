# 🏢 RoomService

Aplikasi web untuk manajemen reservasi ruangan. Dibangun dengan ASP.NET Core MVC, tersedia dua role pengguna yaitu **Admin** dan **Staff**.

---

## 🧰 Tech Stack

| Kegunaan | Teknologi |
|---|---|
| Framework | ASP.NET Core MVC (.NET 10) |
| Database | SQL Server |
| ORM | Entity Framework Core |
| Autentikasi | JWT disimpan di Cookie |
| Password | BCrypt.Net |
| Mapping | AutoMapper |
| Tampilan | Bootstrap 5 + Bootstrap Icons |

---

## 📁 Struktur Project

Project dibagi menjadi 4 layer:

```
RoomService/
│
├── 📂 Domain               → Inti bisnis: Entity & Enum
│   └── Entities/
│       ├── User.cs
│       ├── Room.cs
│       ├── HistoryReservationRoom.cs
│       ├── BaseEntity.cs
│       └── Enums/UserRole.cs
│
├── 📂 Application          → Logika bisnis: Service, Interface, DTO
│   ├── Common/JwtSettings.cs
│   ├── Dtos/               (UserDto, RoomDto, HistoryReservationRoomDto)
│   ├── Interfaces/         (IUserService, IRoomsService, dll)
│   ├── Models/ApiResponse.cs
│   ├── Mapper/MappingProfile.cs
│   └── Services/           (UserService, RoomService, dll)
│
├── 📂 Infrastructures      → Akses database: DbContext & Repository
│   ├── Persistence/
│   │   ├── AppDBContext.cs
│   │   └── Seed.cs         (data admin awal)
│   └── Repositories/       (UserRepository, RoomRepository, dll)
│
└── 📂 Web                  → Tampilan: Controller, View, ViewModel
    ├── Controllers/        (AuthController, AdminController, StaffController)
    ├── ViewModels/
    ├── Views/
    │   ├── Auth/Login.cshtml
    │   ├── Admin/          (Index, Ruangan)
    │   ├── Staff/          (Index)
    │   └── Shared/         (Layout, Sidebar, Partial Views)
    └── Program.cs
```

---

## 🔄 Alur Aplikasi

### Login
1. User buka halaman `/Auth/Login`
2. Masukkan email & password
3. Sistem cek ke database, verifikasi password dengan BCrypt
4. Jika berhasil → JWT Token disimpan di cookie
5. Redirect sesuai role:
   - **Admin** → `/Admin/Index`
   - **Staff** → `/Staff/Index`

---

### Halaman Admin

| Halaman | Fitur |
|---|---|
| `/Admin/Index` | Kelola pengguna (tambah, edit, soft delete, hard delete) |
| `/Admin/Ruangan` | Kelola ruangan + lihat history & reservasi berjalan |

---

### Halaman Staff

| Tab | Fitur |
|---|---|
| Daftar Ruangan | Lihat semua ruangan yang tersedia |
| Form Reservasi | Pilih ruangan & tanggal, lalu booking |
| Riwayat Reservasi | Lihat history reservasi milik sendiri |

---

### Aturan Bisnis

- ✅ Satu ruangan hanya bisa direservasi **sekali per tanggal**
- ✅ Tanggal booking **tidak boleh di masa lalu**
- ✅ Ruangan yang masih punya reservasi aktif **tidak bisa diedit/dihapus**
- ✅ Soft delete → akun dinonaktifkan (masih ada di database)
- ✅ Hard delete → akun dihapus permanen dari database

---

## ⚙️ Cara Set Up

### Yang Dibutuhkan Sebelum Mulai

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (lokal)
- Visual Studio 2022 / VS Code

---

### Langkah 1 — Clone Project

```bash
git clone <url-repository>
cd RoomService
```

---

### Langkah 2 — Atur Koneksi Database

Buka file **`Web/appsettings.Development.json`**, sesuaikan bagian ini:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=RoomServiceDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "SecretKey": "ganti-dengan-string-acak-minimal-32-karakter",
    "Issuer": "RoomService",
    "Audience": "RoomServiceClient",
    "ExpiryInMinutes": 60
  }
}
```

> 💡 Ganti nilai `SecretKey` dengan string panjang acak, misalnya: `roomservice-secret-key-2026-xYzAbC`

---

### Langkah 3 — Buat Database (Migrasi)

Ada 2 cara untuk menjalankan migrasi, pilih salah satu:

---

#### Cara A — Lewat Terminal / CMD

> Jalankan dari dalam folder **Infrastructures**

```bash
cd Infrastructures
```

Buat migrasi:
```bash
dotnet ef migrations add InitialCreate --startup-project ../Web
```

Terapkan ke database:
```bash
dotnet ef database update --startup-project ../Web
```

---

#### Cara B — Lewat NuGet Package Manager Console (Visual Studio)

Buka NuGet Package Manager Console di Visual Studio:
> **Tools → NuGet Package Manager → Package Manager Console**

Pastikan pengaturan di bagian atas console sudah seperti ini:

| Pengaturan | Nilai |
|---|---|
| Default project | `Infrastructures` |

Lalu jalankan perintah berikut satu per satu:

**Buat migrasi:**
```powershell
Add-Migration InitialCreate -StartupProject Web
```

**Terapkan ke database:**
```powershell
Update-Database -StartupProject Web
```

---

Database `RoomServiceDb` akan otomatis terbuat beserta tabel dan **akun admin default**.

---

### Langkah 4 — Jalankan Aplikasi

```bash
cd Web
dotnet run
```

Buka browser dan akses: `http://localhost:5000`

---

## 🔑 Akun Default

Akun admin dibuat otomatis saat migrasi pertama kali dijalankan.

| | |
|---|---|
| **Email** | `admin@roomservice.com` |
| **Password** | `Admin@123` |
| **Role** | Admin |

> ⚠️ Segera ganti password setelah login pertama kali.

---

## 🗄️ Database Script (Opsional)

Jika tidak ingin menggunakan EF Migrations, bisa buat database secara manual dengan script berikut di **SQL Server Management Studio**:

```sql
-- Buat database
CREATE DATABASE RoomServiceDb;
GO

USE RoomServiceDb;
GO

-- Tabel pengguna
CREATE TABLE Users (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Name        NVARCHAR(255) NOT NULL,
    Email       NVARCHAR(255) NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(50)  NOT NULL,
    Password    NVARCHAR(255) NOT NULL,
    Role        NVARCHAR(50)  NOT NULL,  -- nilai: 'Admin' atau 'Staff'
    IsDeleted   BIT           NOT NULL DEFAULT 0,
    CreatedAt   DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt   DATETIME2     NULL
);
GO

-- Tabel ruangan
CREATE TABLE Rooms (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    RoomName  NVARCHAR(255) NOT NULL,
    CreatedAt DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2     NULL
);
GO

-- Tabel riwayat reservasi
CREATE TABLE HistoryReservationRooms (
    Id                    INT IDENTITY(1,1) PRIMARY KEY,
    RoomId                INT           NOT NULL,
    UserEmail             NVARCHAR(255) NOT NULL,
    BookedDateReservation DATETIME2     NOT NULL,
    CreatedAt             DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt             DATETIME2     NULL,

    CONSTRAINT FK_Reservation_Room
        FOREIGN KEY (RoomId) REFERENCES Rooms(Id)
        ON DELETE NO ACTION
);
GO

-- Tabel migrasi EF (wajib ada jika project tetap pakai EF)
CREATE TABLE __EFMigrationsHistory (
    MigrationId    NVARCHAR(150) NOT NULL PRIMARY KEY,
    ProductVersion NVARCHAR(32)  NOT NULL
);
GO

-- Insert akun admin default
-- Password: Admin@123
INSERT INTO Users (Name, Email, PhoneNumber, Password, Role, IsDeleted, CreatedAt, UpdatedAt)
VALUES (
    'Administrator',
    'admin@roomservice.com',
    '08123456789',
    '$2a$11$psFE6UAQOjSryTMxQyNq2.ltx3tFRVOCeDE0/8AzstYGGERQWQ2lS',
    'Admin',
    0,
    '2024-01-01 00:00:00',
    NULL
);
GO
```

---

## 📝 Catatan

- Semua form di halaman Admin dikirim lewat **AJAX**, jadi halaman tidak reload penuh saat simpan/hapus data.
- JWT token disimpan sebagai **HttpOnly cookie** sehingga tidak bisa dibaca lewat JavaScript (lebih aman dari XSS).
- Email staff diambil otomatis dari token saat membuat reservasi, tidak bisa dimanipulasi dari form.
