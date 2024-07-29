# Что такое - OffersAndEventAttendanceTracking
OffersAndEventAttendanceTracking — это приложение на MAUI C# для «Корпоративного Университета Правительства Нижегородской Области», работающее на Windows.

Приложение предназначено для контроля посещения мероприятий за счёт проверки гостей по QR-коду и ведения статистики посещений. В приложении также предусмотрена рассылка сообщений, таких как QR-коды приглашённым гостям, а также уведомлений о различных событиях «Корпоративного Университета» через массовую рассылку.

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

В методе UpdateQrCode мы получаем изображение с камеры и устанавливаем его в QRImage. Затем изображение выводится на экран. После этого мы проверяем, есть ли на изображении QR-код. Если он присутствует, то мы сверяем его со списком гостей.

Если код совпадает с одним из кодов из списка, камера выключается, а гость отмечается как прибывший с указанием времени прибытия.
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

## Парсер XLSX файла
Для парсера использовалась библиотека [Open-XML-SDK](https://github.com/dotnet/Open-XML-SDK/tree/main) .
Предназначен для добавления группы гостей
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/981bf3b46c23016299c2f840e8131beb65fed4d3/Data/Parser/XlsxParser.cs#L20-L103

## Запись в  XLSX файл
Для записи в XLSX файл использовалась библиотека [Aspose.Cells-for-.NET](https://github.com/aspose-cells/Aspose.Cells-for-.NET) .
Предназначен для создания XLSX файла со статистикой гостей по данному мероприятию 
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/981bf3b46c23016299c2f840e8131beb65fed4d3/Data/Record%20File/RecordXlsx.cs#L13-L43



# Основные задачи приложения
### *Создания  (редактрования , удаления) мепоприятей
В приложении присутствуют возможности создавать, редактировать и удалять мероприятия. Также имеется просмотр статистики, включающий список гостей с отметкой о присутствии на мероприятии, времени прибытия, отправке сообщения с приглашением.
![](https://github.com/Dima00132/Pictured-to-describe-the-scanner-application/blob/main/%D0%A1%D0%BD%D0%B8%D0%BC%D0%BE%D0%BA%20%D1%8D%D0%BA%D1%80%D0%B0%D0%BD%D0%B0%202024-07-21%20203418.png)

