Kullanılan Teknolojiler:
-MVC
-CQRS Pattern
-Clean Architecture
-Result Pattern
-RabbitMQ
-Domain Event

**Kullanıcı Girişi yapıldı: Cookie yapısı kullanıldı.
**Newsletters-Home-Login sayfaları tasarlandı. 
**Otomatik atanan bir kullanıcı eklendi.

Kullanılan Kütüphaneler:
-Identity
-MediatR
-AutoMapper
-CTS.Result
-EntityFrameworkCore.InMemory
-Scrutor?(Şimdilik inaktif)

**>Kullanıcı Giriş yaptıktan sonra Newsletter sayfasında BLOG ekleme işlemi: Create()
**Subscriber listesine tek tek mail göndermek istendiğinde;
Seed data ile fake veriler basıldı. - Önce 5 tane blog ekliyoruz. (Bogus Kütüphanesi kullanıyoruz)
Subscriber listesi oluşturuyorum ve 1000 tane mail adresi kaydediyoruz.


**Yazılan Blogların kayıt işleminden sonra mail göndermek için kuyruk sistemi kullanıldı Queue oluşturuldu. 
Blog Publish edildikten sonra, 1000 tane maili  tek tek kuyruğa gönderiyoruz.

Burada, Domain Event kullanıyoruz. Nasıl?
Eğer Create işlemi esnasında Blog'un isPublish true ise Domain eventi tetiklenip, mail gönderme işleminin başlamasını bekliyoruz.


**Kuyruğu dinleyecek bir proje eklendi. Newsletter.Consumer console.app eklendi. Bu kuyruğu dinleyen consumer .
