# 💇‍♂️ KuaforApp - Kuaför Salon Randevu Yönetim Sistemi

Modern kuaför salonları için geliştirilmiş kapsamlı randevu yönetim sistemi. Müşterilerin kolayca randevu alabilmesi, AI destekli saç stili önerileri alabilmesi ve salon yöneticilerinin işletmelerini etkin bir şekilde yönetebilmesi için tasarlanmıştır.

## 📸 Ekran Görüntüsü

*[Buraya projenin ana ekran görüntüsü eklenecek]*

## 🛠️ Kullanılan Teknolojiler

### Backend
- **.NET 8.0** - Modern C# framework
- **ASP.NET Core MVC** - Web uygulama mimarisi
- **Entity Framework Core 8.0.0** - ORM ve veritabanı yönetimi
- **PostgreSQL** - İlişkisel veritabanı
- **ASP.NET Core Identity** - Kimlik doğrulama ve yetkilendirme

### AI Mikroservisi
- **Python 3.x** - AI işlemleri için
- **Flask 2.0.1** - Web framework
- **OpenCV 4.5.3.56** - Görüntü işleme
- **NumPy 1.21.2** - Matematiksel işlemler

### Frontend
- **Razor Views** - Server-side rendering
- **Bootstrap** - Responsive UI framework
- **JavaScript/jQuery** - Client-side etkileşim

## ✨ Özellikler

### 🔐 Kullanıcı Yönetimi
- Güvenli kayıt ve giriş sistemi
- Role-based yetkilendirme (User/Admin)
- Profil yönetimi

### 🏪 Salon Yönetimi
- Salon ekleme, düzenleme ve silme
- Çalışma saatleri ve günleri yönetimi
- Salon resmi yükleme

### 💇‍♀️ Hizmet Yönetimi
- Hizmet kategorileri ve fiyatlandırma
- Hizmet süreleri tanımlama
- Salon bazlı hizmet yönetimi

### 👥 Çalışan Yönetimi
- Çalışan profilleri ve çalışma saatleri
- Hizmet-çalışan eşleştirmesi
- Çalışma günleri yönetimi

### 📅 Randevu Sistemi
- Online randevu alma
- Tarih ve saat seçimi
- Randevu durumu takibi (Beklemede/Onaylandı/Reddedildi)
- Çakışma kontrolü

### 🤖 AI Destekli Öneriler
- Fotoğraf yükleme
- Saç stili önerileri
- Kişiselleştirilmiş öneriler

### 📊 Admin Dashboard
- Sistem genel görünümü
- Randevu yönetimi
- Kullanıcı yönetimi
- İstatistikler

## 🚀 Kurulum Talimatları

### Gereksinimler
- .NET 8.0 SDK
- PostgreSQL 12+
- Python 3.8+
- Git

### 1. Projeyi Klonlayın
```bash
git clone https://github.com/mustaffaerdogan/KuaforApp.git
cd KuaforApp
```

### 2. Veritabanı Kurulumu
```bash
# PostgreSQL'de veritabanı oluşturun
createdb kuaforapp

# Connection string'i güncelleyin (appsettings.json)
"ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=kuaforapp;Username=your_username;Password=your_password"
}
```

### 3. .NET Bağımlılıklarını Yükleyin
```bash
cd KuaforApp
dotnet restore
```

### 4. Veritabanı Migration'larını Çalıştırın
```bash
dotnet ef database update
```

### 5. AI Mikroservisini Kurun
```bash
cd PythonAI
pip install -r requirements.txt
```

## ⚡ Komutlar

### .NET Komutları
```bash
# Projeyi derle
dotnet build

# Geliştirme modunda çalıştır
dotnet run

# Test'leri çalıştır
dotnet test

# Migration oluştur
dotnet ef migrations add [MigrationName]

# Migration'ları uygula
dotnet ef database update
```

### Python AI Servisi
```bash
# AI servisini başlat
cd PythonAI
python app.py

# Servis http://localhost:5000 adresinde çalışacak
```

### Veritabanı Komutları
```bash
# Migration'ları geri al
dotnet ef database update [PreviousMigrationName]

# Migration'ları kaldır
dotnet ef migrations remove
```

## 🔌 API Endpointleri

### Kimlik Doğrulama
| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | `/Account/Login` | Giriş sayfası |
| POST | `/Account/Login` | Giriş işlemi |
| GET | `/Account/Register` | Kayıt sayfası |
| POST | `/Account/Register` | Kayıt işlemi |
| POST | `/Account/Logout` | Çıkış işlemi |

### Salon Yönetimi
| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | `/Salons` | Salon listesi |
| GET | `/Salons/Create` | Salon oluşturma formu |
| POST | `/Salons/Create` | Salon oluşturma |
| GET | `/Salons/Edit/{id}` | Salon düzenleme formu |
| POST | `/Salons/Edit/{id}` | Salon düzenleme |
| POST | `/Salons/Delete/{id}` | Salon silme |

### Randevu Yönetimi
| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | `/Appointments` | Randevu listesi |
| GET | `/Appointments/Create` | Randevu oluşturma formu |
| POST | `/Appointments/Create` | Randevu oluşturma |
| GET | `/Appointments/Details/{id}` | Randevu detayları |

### AI Önerileri
| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | `/AIRecommendation` | AI öneri sayfası |
| POST | `/api/AIRecommendation/recommendation` | Fotoğraf yükleme ve öneri alma |

## 👥 Kullanıcı Rolleri ve Yetkilendirme

### User (Normal Kullanıcı)
- ✅ Profil görüntüleme ve düzenleme
- ✅ Salon listesini görüntüleme
- ✅ Randevu alma ve yönetme
- ✅ AI önerileri alma
- ❌ Admin paneline erişim

### Admin (Sistem Yöneticisi)
- ✅ Tüm User yetkileri
- ✅ Salon ekleme, düzenleme, silme
- ✅ Hizmet yönetimi
- ✅ Çalışan yönetimi
- ✅ Randevu onaylama/reddetme
- ✅ Sistem ayarları
- ✅ Kullanıcı yönetimi

### Şifre Gereksinimleri
- En az 6 karakter
- En az 1 büyük harf
- En az 1 küçük harf
- En az 1 rakam
- En az 1 özel karakter

## 🤖 AI Mikroservis Açıklaması

AI mikroservisi, Flask tabanlı Python uygulaması olarak çalışır ve saç stili önerileri sağlar.

### Özellikler
- **Fotoğraf İşleme**: OpenCV ile görüntü analizi
- **Öneri Sistemi**: Yüz şekli ve saç tipine göre öneriler
- **RESTful API**: JSON tabanlı iletişim
- **Dosya Yönetimi**: Güvenli dosya yükleme ve temizleme

### Teknik Detaylar
- **Port**: 5000
- **Framework**: Flask 2.0.1
- **Görüntü İşleme**: OpenCV 4.5.3.56
- **Dosya Formatları**: PNG, JPG, JPEG

### API Kullanımı
```bash
curl -X POST -F "photo=@image.jpg" http://localhost:5000/recommendation
```

## 🔧 Ortam Değişkenleri

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=kuaforapp;Username=postgres;Password=your_password"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Python AI Servisi
```python
# app.py içinde
UPLOAD_FOLDER = 'uploads'
ALLOWED_EXTENSIONS = {'png', 'jpg', 'jpeg'}
```

## 📁 Dosya/Klasör Yapısı

```
KuaforApp/
├── Controllers/                 # MVC Controllers
│   ├── HomeController.cs       # Ana sayfa
│   ├── AccountController.cs    # Kimlik doğrulama
│   ├── SalonsController.cs     # Salon yönetimi
│   ├── ServicesController.cs   # Hizmet yönetimi
│   ├── EmployeesController.cs  # Çalışan yönetimi
│   ├── AppointmentsController.cs # Randevu yönetimi
│   ├── AIRecommendationController.cs # AI önerileri
│   ├── AdminController.cs      # Admin dashboard
│   └── AdminAppointmentsController.cs # Admin randevu yönetimi
├── Models/                     # Entity modelleri
│   ├── ApplicationDbContext.cs # EF DbContext
│   ├── ApplicationUser.cs      # Kullanıcı modeli
│   ├── Salon.cs               # Salon modeli
│   ├── Service.cs             # Hizmet modeli
│   ├── Employee.cs            # Çalışan modeli
│   ├── Appointment.cs         # Randevu modeli
│   ├── AIRecommendation.cs    # AI öneri modeli
│   └── UserRole.cs            # Rol enum'u
├── Views/                     # Razor Views
│   ├── Home/                  # Ana sayfa view'ları
│   ├── Account/               # Giriş/kayıt view'ları
│   ├── Salons/                # Salon view'ları
│   ├── Services/              # Hizmet view'ları
│   ├── Employees/             # Çalışan view'ları
│   ├── Appointments/          # Randevu view'ları
│   ├── AIRecommendation/      # AI öneri view'ları
│   ├── Admin/                 # Admin view'ları
│   └── Shared/                # Paylaşılan layout'lar
├── ViewModels/                # View modelleri
│   ├── AppointmentViewModel.cs
│   ├── AdminDashboardViewModel.cs
│   ├── AccountViewModels.cs
│   └── EmployeeViewModels.cs
├── Migrations/                # EF Migrations
├── wwwroot/                   # Statik dosyalar
│   ├── css/                   # Stil dosyaları
│   ├── js/                    # JavaScript dosyaları
│   ├── lib/                   # Kütüphaneler (Bootstrap)
│   └── uploads/               # Yüklenen dosyalar
├── PythonAI/                  # AI mikroservisi
│   ├── app.py                 # Flask uygulaması
│   ├── requirements.txt       # Python bağımlılıkları
│   └── uploads/               # Geçici dosya yükleme
├── Program.cs                 # Uygulama başlangıç noktası
├── appsettings.json           # Yapılandırma
└── KuaforApp.csproj           # Proje dosyası
```

## 🎯 Kullanım Senaryoları

### Müşteri Senaryoları

#### 1. Kayıt ve Giriş
1. Ana sayfadan "Kayıt Ol" butonuna tıklama
2. Kişisel bilgileri girme
3. E-posta doğrulama
4. Giriş yapma

#### 2. Randevu Alma
1. Salon listesini görüntüleme
2. İstediği salonu seçme
3. Hizmet ve çalışan seçimi
4. Tarih ve saat belirleme
5. Randevu onaylama

#### 3. AI Önerileri
1. "AI Önerileri" sayfasına gitme
2. Fotoğraf yükleme
3. Önerileri görüntüleme
4. Önerileri kaydetme

### Admin Senaryoları

#### 1. Salon Yönetimi
1. Admin paneline giriş
2. "Salonlar" bölümüne gitme
3. Yeni salon ekleme
4. Mevcut salonları düzenleme

#### 2. Randevu Yönetimi
1. "Randevular" bölümüne gitme
2. Bekleyen randevuları görüntüleme
3. Randevuları onaylama/reddetme
4. Randevu detaylarını inceleme

## 🐛 Bilinen Hatalar ve Geliştirme Planı

### Bilinen Hatalar
- [ ] AI servisi production'da güvenlik açıkları
- [ ] Dosya yükleme boyut sınırlamaları
- [ ] CORS yapılandırması sadece localhost
- [ ] Email bildirimleri eksik

### Geliştirme Planı
- [ ] **v2.0** - Mobil uygulama geliştirme
- [ ] **v2.1** - Ödeme sistemi entegrasyonu
- [ ] **v2.2** - Gelişmiş raporlama sistemi
- [ ] **v2.3** - SMS bildirimleri
- [ ] **v2.4** - Çoklu dil desteği
- [ ] **v2.5** - API dokümantasyonu (Swagger)

### TODO Listesi
- [ ] Unit testler ekleme
- [ ] Integration testler yazma
- [ ] Performance optimizasyonu
- [ ] Security audit
- [ ] Docker containerization
- [ ] CI/CD pipeline kurulumu

## 🤝 Katkı Rehberi

### Geliştirme Ortamı Kurulumu
1. Projeyi fork edin
2. Local repository oluşturun
3. Feature branch oluşturun: `git checkout -b feature/amazing-feature`
4. Değişikliklerinizi commit edin: `git commit -m 'Add amazing feature'`
5. Branch'inizi push edin: `git push origin feature/amazing-feature`
6. Pull Request oluşturun

### Kod Standartları
- **C#**: Microsoft C# coding conventions
- **JavaScript**: ESLint kuralları
- **CSS**: BEM metodolojisi
- **Git**: Conventional commits

### Test Yazma
- Unit testler için xUnit kullanın
- Integration testler için TestServer kullanın
- UI testler için Selenium kullanın

### Pull Request Süreci
1. Issue oluşturun
2. Feature branch oluşturun
3. Kod yazın ve test edin
4. Pull Request açın
5. Code review sürecini bekleyin
6. Merge edildikten sonra branch'i silin

## 👨‍💻 Geliştiriciler

- **Mustafa Erdoğan** - B221210308
- **Ali Aydın** - B221210373

Sakarya Üniversitesi 3. Sınıf Web Programlama dönem projesi.

## 📞 İletişim

- **E-posta**: [mustafaerdoganu@gmail.com]
- **GitHub**: [https://github.com/mustaffaerdogan/KuaforApp]

---

⭐ Bu projeyi beğendiyseniz yıldız vermeyi unutmayın!