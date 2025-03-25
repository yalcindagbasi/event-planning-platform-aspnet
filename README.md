# Togethr (Akıllı Etkinlik Planlama Platformu)

Togethr, .NET 8 kullanılarak oluşturulmuş bir web uygulamasıdır. Kullanıcıların etkinlik oluşturmasına, yönetmesine ve etkinliklere katılmasına olanak tanır. Platform ayrıca kullanıcı kimlik doğrulaması, mesajlaşma ve profil yönetimi için özellikler içerir. Proje Asp .Net MVC yapısını ve API kullanımını öğrenme amaçlı geliştirilmiştir.

## Özellikler

- Kullanıcı Kaydı ve Kimlik Doğrulama
- Etkinlik Oluşturma ve Yönetimi
- Kullanıcı Profili Yönetimi
- Mesajlaşma Sistemi
- Etkinlik Konumları için Google Haritalar Entegrasyonu
- Kullanıcı ve Etkinlik Yönetimi için Yönetici Paneli

## Kullanılan Teknolojiler

- .NET 8
- Entity Framework Core
- PostgreSQL
- Google Haritalar API
- Bootstrap

## Başlarken

### Gereklilikler

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/)

### Kurulum

1. **Repository'i Klonla:**
```bash
git clone https://github.com/yalcindagbasi/smart-event-planning-platform.git 
cd smart-event-planning-platform
```
2. **Veritabanını ayarlayın:**

   Bir PostgreSQL veritabanı oluşturun ve `appsettings.json` içindeki bağlantı dizesini güncelleyin:
```bash
{ "ConnectionStrings": { "DefaultConnection": "Host=localhost;Database=YourDatabaseName;Username=YourUsername;Password=YourPassword" } }
```

3. **Apply database migrations:**
```bash
dotnet ef database update
```
4. **API Key'lerini ayarla:**
```bash
dotnet user-secrets init
dotnet user-secrets set "GoogleMapsApiKey" "YOUR_GOOGLE_MAPS_API_KEY"
```

5. **Çalıştır:**
```bash
dotnet run
```
## Proje Yapısı

- **Controllers**: HTTP isteklerini işlemek için MVC denetleyicilerini içerir.
- **Services**: İş mantığını ve veri erişim kodunu içerir.
- **Models**: Veri modellerini ve DTO'ları içerir.
- **Views**: Kullanıcı arayüzü için HTML&CSS görünümlerini içerir.
- **wwwroot**: CSS, JavaScript ve resimler gibi statik dosyaları içerir.

## Kullanım

### Kullanıcı Kaydı

Kullanıcılar kullanıcı adı, şifre, e-posta ve diğer profil bilgilerini girerek kayıt olabilirler.

### Etkinlik Önerisi
Kullanıcılara profillerinde seçtikleri ilgi alanlarına göre önerilen etkinlikler gösterilir. Ayrıca tüm etkinlikleri ve yaklaşan etkinlikleri de görebilirler.
![2025-03-25 14 20 28 localhost 7a8de0a22a24](https://github.com/user-attachments/assets/6ec8f716-bb90-41cb-96e7-5ee457910d1d)

### Etkinlik Yönetimi
Kullanıcılar etkinlik oluşturabilir, düzenleyebilir ve silebilir.  
![2025-03-25 13 52 36 localhost 85f5e832f07f](https://github.com/user-attachments/assets/da73ba7d-e142-4043-bb06-e4c321925aaf)

Ayrıca etkinlik ayrıntılarını görüntüleyebilir ve etkinliklere katılabilirler.
![2025-03-25 15 10 40 localhost 10d0e1bcc0b0](https://github.com/user-attachments/assets/ac5437bc-7aa5-4de5-ad5b-3bf73caf1ef0)

### Dashboard
Burada kullanıcıların tüm oluşturduğu ve katıldığı etkinliklerin bulunur.
![2025-03-25 14 22 01 localhost 6638993f67f5](https://github.com/user-attachments/assets/f949297b-ca64-4366-8c44-d87800f2b548)

### Puan Toplama & Profil
Kullanıcılar profilini görüntüleyebilir, düzenleyebilir veya silebilir. Ayrıca kullanıcı başka kişilerin profilini de inceleyebilir. Etkinlik oluşturmak ve bir etkinliğe katılmak kullanıcıya puan kazandırır.
![2025-03-25 15 10 15 localhost 88b47075e517](https://github.com/user-attachments/assets/7e6f3758-2fb8-47f8-b9f0-fe2ef427564d)

### Mesajlaşma
Kullanıcılar etkinliklerle ilgili mesajlar gönderebilir ve alabilir. Mesajlaşma sistemi, profil resimlerini ve zaman damgalarını görüntüleme özelliklerini içerir.
![2025-03-25 15 20 13 localhost 08a343af54ed](https://github.com/user-attachments/assets/9504c88f-9553-46d7-aeb1-ce2591331275)
