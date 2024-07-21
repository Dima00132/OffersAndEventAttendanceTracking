# Что такое - OffersAndEventAttendanceTracking
OffersAndEventAttendanceTracking - это приложения MAUI C# для "Корпоративный Университет Правительства Нижегородской Области" работающее на Windows. Приложение преднозначено для контроля посещения мероприятия за счет проверки гостей по qr - коду и ведения статистики псещения. В данной прриложение так же присутствует рассылки сообщений , таких как qr коды приглошенным гостям и сообщений разный собыий у "Корпоративный Университет Правительства Нижегородской Области" через массовую рассылку.

# Основные библиотеки при создание приложения
## Рассылка сообщений
-Для рассылки использовалась библиотека  [MailKit](https://github.com/jstedfast/MailKit) .
Рассылку реализована следующем образом.
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/28022da1eaf6e9099ea075a946abf893a1e65198/Data/Message/EmailMessage.cs#L44-L88
## Сканер QR-кода 
Для сканирования использовалась библиотека (доступ к камере ) [AForge.NET]( https://github.com/andrewkirillov/AForge.NET) .
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/07dcd011ba0bab0ed600d8f360b048d05b2878d7/Data/QRCode/ScannerQR.cs#L6-L48

GetVideoInputDevice - возвращает FilterInfoCollection в котором находится список камер на данном устройстве
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/46ef395ee7a74febf29d8d576c917a87da17e11d/Data/QRCode/ScannerQR.cs#L20-L24

ConnectingCamera создает объект VideoCaptureDevice и подписывается на его события
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/46ef395ee7a74febf29d8d576c917a87da17e11d/Data/QRCode/ScannerQR.cs#L25-L36

ScannerQR принимает делигат UpdateQrCode
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/46ef395ee7a74febf29d8d576c917a87da17e11d/ViewModel/ScannerQRCodeViewModel.cs#L205-L224

UpdateQrCode получаем изображение с камеры и устанавливаем его в QRImage тем самом оно выводится на экран , полсле проверяет на начилие в нем qr кода и если он присутствует то провемяем на соответствие в списки гостей. Если код совпал то камера выключается а гость указывается как прибывшим с отметкой времяни прибытия
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/46ef395ee7a74febf29d8d576c917a87da17e11d/ViewModel/ScannerQRCodeViewModel.cs#L207-L223

Проверка совпадений кода со списком гостей
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/46ef395ee7a74febf29d8d576c917a87da17e11d/ViewModel/ScannerQRCodeViewModel.cs#L226-L245

## Кодинг и  Декодинг QR кода
Для кодирования использовалась библиотека [QRCoder](https://github.com/codebude/QRCoder) .
Для декодирова ния использовалась библиотека [ZXing.Net](https://github.com/micjahn/ZXing.Net).
### EncodeQRCode 
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/46ef395ee7a74febf29d8d576c917a87da17e11d/Data/QRCode/EncodeQRCode.cs#L7-L37
### DecodeQRCode 
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/46ef395ee7a74febf29d8d576c917a87da17e11d/Data/QRCode/DecodeQRCode.cs#L19-L36


