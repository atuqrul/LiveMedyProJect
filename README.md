Case Description

1.	Proje .Net Core 7 kullanılarak geliştirildi.
2.	Database olarak MSSQL kullanıldı.
3.	Code first yaklaşımı kullanıldı. Migration class ı hazır. Local db de Windows Authentication ile db create edilebilinir.
4.	User Rolleri Static olarak eklendi.
5.	UI tarafta farklı bir case belirtilmediği için MVC Core kullanıldı.
6.	Captcha için third party kullanıldı. Production da Google Recapthcha kullanılabilinir. (captcha.png)
7.	İki farklı role belirlendi. (Customer ve Manager)
8.	Customer rolündeki kullanıcı Ana sayfada bulunan “Davet Linki Gönder” formunu ve User List View unda bulunan toplam kullanıcı sayılarını göremiyor.
9.	Manager rolündeki kullanıcı “Kullanıcı Davet Linki” gönderebiliyor ve kullanıcı listesi sayfasında bulunan toplam sayı bilgilerini görebiliyor.
10.	Davet linki şifrelenerek kullanıcı email adresine link olarak gönderiliyor.
11.	Site üzerinden kullanıcılar Customer rolünde kaydediliyor.
12.	Davet linki üzerinden ise Manager rolünde kaydediliyorlar.
14.	Şifre güvenliği için regex kullanıldı.
15.	Kullanıcının girmiş olduğu email adresinin daha önce kullanılıp kullanılmadığı kontrol edilmiştir. 
