# Newsletter Application
Bu proje, .NET MVC kullanılarak geliştirilmiş bir Newsletter uygulamasıdır. Temel amacı, RabbitMQ kullanımını öğrenmek ve bu süreçte Clean Architecture, CQRS ve Domain Events gibi  yazılım geliştirme tekniklerini  uygulamaktır.
"Projeyi geliştirirken, RabbitMQ ile entegre edilen farklı tüketici yapıları olan Console Application ve Background Service kullanıldı. Her birinin sistemdeki rolü ve sağladığı avantajlar gözlemlendi. Öğrenmek ve daha derinlemesine anlamak isteyenler için, bu yapıların nasıl kullanıldığını gösteren kodlar projemde mevcuttur ve incelenebilir.

## Kullanılan Teknolojiler
- **MVC**: Model-View-Controller mimarisi.
- **CQRS Pattern**: Command Query Responsibility Segregation, komut ve sorguların ayrıştırılması.
- **Clean Architecture**: Bağımlılıkların düşük olması ve bağımsızlık sağlanması amacıyla katmanlı mimari.
- **RabbitMQ**: Mesaj kuyruğu yönetimi.
- **Result Pattern**: İşlemlerden dönüş türleriyle ilgili standart bir yapı.
- **Domain Event**: Domain olaylarının yönetilmesi.
- **MS SQL**: Veritabanı yönetimi.

## Kullanılan Kütüphaneler
- **Identity**: Kimlik ve yetkilendirme işlemleri için.
- **MediatR**: CQRS deseni uygulamalarında mediator kullanımı.
- **AutoMapper**: Nesneler arası otomatik tip dönüşümleri.
- **CTS.Result**: İşlem sonuçlarını standart bir formatta döndürmek için.
- **Scrutor**: (Şu anda inaktif) - Uygulama başlatılırken servis kaydı yapmak için.
- **Bogus**:  Subscriber listesine tek tek mail gönderebilmek içn fake veriler oluşturmak için.
- **FluentEmail.Smtp (smtp4dev )**: Geliştirme sürecinde mail gönderimlerini test etmek için

## Özellikler

### Kullanıcı Girişi
- Cookie tabanlı kimlik doğrulama yapısı.
- Home, Login ve Newsletter sayfaları.
- Otomatik atanan bir kullanıcı eklendi.
- Yetki olmayan sayfaya giriş yapıldığında yönlendirilecek sayfa eklendi.

### Newsletter Yönetimi - Consumer: Console.App
- Yeni blog ekleme.
- SeedData ile Subscriber listesine fake veri basma.
- Yazımı tamamlanan blogları mail olarak göndermek için kuyruk sistemi kullanılacağından Queue oluşturuldu. Örnek: Blog publish oldu, 1000 tane mail kuyruğa gönderildi.
- Blog yazısının Publish edilmesi sürecinde DomainEvent kullanıldı. 
- Kuyruğu dinleyecek bir proje eklendi. "Newsletter.Consumer" console.app .
- Mail göndermek için fake bir mail yapısı (smtp4dev) kullanıldı.

### Newsletter Yönetimi - Consumer: BackgroundService
- Publish edilmeyen blog yazısını Publish'e çekip değiştirebilmek için(ChangeStatus) Checkbox eklendi.
- Kuyruğu dinleyecek bir Background Service yazıldı.(Dependency Injection için Service Tool kullanıldı.)

##

### RabbitMQ ile Asenkron İletişim
RabbitMQ, projede asenkron iletişim sağlamak için kritik bir rol oynar. Örneğin, bir uygulamanın bir bölümünde oluşturulan veri veya bilgi, başka bir bölümde işlenmek üzere RabbitMQ kullanılarak gönderilir. Bu işlem asenkron gerçekleşir; yani bir bölüm işini bitirip mesajı gönderdikten sonra, diğer bölüm mesajı alıp işlemeye hazır olduğunda işleme başlar. Bu, mesajı gönderen bölümün kendi işlemlerine devam etmesine olanak tanır ve mesajın alınıp işlenmesini beklemez.


### RabbitMQ (Consumer) Yapıları
Proje içerisinde mesajları işlemek için iki farklı tüketici yapısı kullanılmıştır:
1. **Console Application:** Bu yaklaşım, uygulamanın açık olduğu sürece çalışır, uygulama kapandığında durduğundan, basit ve hızlı bir şekilde mesajları dinlemek ve işlemek için kullanılmıştır.
2. **Background Service:** Bu yaklaşım ise, uygulamadan abğımsız olarak çalıştığı için, sürekli çalışan bir servis gerektiren durumlar için tercih edilmiştir. 

Her iki tüketici tipi de mesajları alıp işlemek için kullanılmış olup, kullanım amaçlarına göre farklılıklar göstermektedir. Console uygulaması daha çok test ve basit uygulamalar için uygundur, Background Service ise uygulamanın sürekli çalışmasını gerektiren ve daha fazla yönetim ihtiyacı olan durumlar için daha uygun bir çözümdür.


### RabbitMQ Kullanımı Avantajları
1. **Performans Artışı:** Uygulamanın farklı bölümleri birbirlerini beklemek zorunda kalmaz, bu da genel performansı önemli ölçüde artırır.
2. **Kaynak Kullanımının Optimizasyonu:** Asenkron iletişim sayesinde, sistem kaynakları daha verimli kullanılır ve daha fazla işlem paralel olarak yürütülebilir.
3. **Dayanıklılık ve Hata Toleransı:** Bir bölümdeki hata veya gecikme diğer bölümleri etkilemez, bu da uygulamanın genel dayanıklılığını artırır ve hataların daha kolay yönetilmesini sağlar.


## Test Talimatları
Projeyi başarıyla test etmek için aşağıdaki adımları takip edin:

### Consumer olarak: Console Application (Newsletter.Consumer) Kullanımı İçin Test Talimatları
- **Uygulamayı Çalıştırın**: Uygulamayı yerel geliştirme ortamınızda başlatın.
- **Seed Data Yükleme**: Postman kullanarak veritabanına test verileri ekleyebilrsiniz.
- ** Blog Ekleme**: Uygulama üzerinden yeni bir blog yazısı ekleyin.
- **Mesaj Kuyruğunu Kontrol Et**: RabbitMQ yönetim panelinden mesaj kuyruğunu kontrol edin ve yeni eklenen blogun kuyruğa düzgün bir şekilde eklendiğini doğrulayın
- **Consumer Console Uygulamasını Çalıştır**: PowerShell üzerinde `dotnet run` komutuyla veya Visual Studio'da solution properties altında multiple startup project seçeneğini kullanarak `Newsletter.Consumer` ve `Newsletter.MVC` projelerini başlatın.
- **SMTP Kontrolü**: smtp4dev kullanarak, gönderilen maillerin iletilip iletilmediğini kontrol edin.

### Consumer olarak: Background Service Kullanımı İçin Test Talimatları 
- Uygulamanın bir parçası olarak Background Service otomatik olarak çalışmaya başlayacaktır. Eğer manuel başlatma gerekiyorsa, ilgili servisi başlatın.
- **Blog Yayınlama**: eni bir blog yazısı ekleyin ve yayınlayın. Background Service, blog yayınlandığında ilgili işlemleri otomatik olarak tetikleyecektir.
- **E-posta Gönderimini Kontrol Et**: Background Service tarafından tetiklenen e-posta gönderim işlemlerinin başarılı olup olmadığını kontrol etmek için SMTP server logs'larını inceleyin.
