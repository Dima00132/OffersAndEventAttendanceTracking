# Что такое - OffersAndEventAttendanceTracking
OffersAndEventAttendanceTracking - это приложения MAUI C# для Корпоративный университет Правительства Нижегородской области работающее на Windows. Приложение преднозначено для контроля посещения мероприятия за счет проверки гостей по qr - коду и ведения статистики псещения. В данной прриложение так же присутствует рассылки сообщений , таких как qr коды приглошенным гостям и сообщений разный собыий у Корпоративный университет Правительства Нижегородской области через массовую рассылку.

# Основуные библиотеки при создание приложжения
## Рассылка сообщений
-Для рассылки использовалась библиотека  [MailKit](https://github.com/jstedfast/MailKit) .
Рассылку реализована слудующем оброзом.

https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/28022da1eaf6e9099ea075a946abf893a1e65198/Data/Message/EmailMessage.cs#L44-L88
