# Что такое - OffersAndEventAttendanceTracking
OffersAndEventAttendanceTracking — это приложение на MAUI C# для «Корпоративного Университета Правительства Нижегородской Области», работающее на Windows.

Приложение предназначено для контроля посещения мероприятий за счёт проверки гостей по QR-коду и ведения статистики посещений. В приложении также предусмотрена рассылка сообщений, таких как QR-коды приглашённым гостям, а также уведомлений о различных событиях «Корпоративного Университета» через массовую рассылку.

# Основные библиотеки при создание приложения
## Рассылка сообщений
-Для рассылки использовалась библиотека  [MailKit](https://github.com/jstedfast/MailKit) .
Рассылку реализована следующем образом.
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/28022da1eaf6e9099ea075a946abf893a1e65198/Data/Message/EmailMessage.cs#L44-L88
## Камера и сканер QR-кода
Чтобы получить доступ к камере, используется библиотека [AForge.NET]( https://github.com/andrewkirillov/AForge.NET) .
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/07dcd011ba0bab0ed600d8f360b048d05b2878d7/Data/QRCode/ScannerQR.cs#L6-L48

GetVideoInputDevice возвращает FilterInfoCollection, в котором содержится список камер на данном устройстве.
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/46ef395ee7a74febf29d8d576c917a87da17e11d/Data/QRCode/ScannerQR.cs#L20-L24

ConnectingCamera создаёт объект VideoCaptureDevice и подписывается на его события.
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/46ef395ee7a74febf29d8d576c917a87da17e11d/Data/QRCode/ScannerQR.cs#L25-L36

ScannerQR принимает делегаты NewFrameEventHandler (UpdateQrCode) и VideoSourceErrorEventHandler (ErrorCamera).

NewFrameEventHandler (UpdateQrCode) подписывается на события _captureDevice.NewFrame для захвата изображения с веб-камеры.

VideoSourceErrorEventHandler подписывается на события _captureDevice.VideoSourceError и срабатывает, если при запуске веб-камеры возникает ошибка.
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/46ef395ee7a74febf29d8d576c917a87da17e11d/ViewModel/ScannerQRCodeViewModel.cs#L205-L224
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/46db5c9c4aaefcf57942c8fead2cf8c4d0f4ee88/ViewModel/ScannerQRCodeViewModel.cs#L62-L66

В методе UpdateQrCode мы получаем изображение с камеры и устанавливаем его в QRImage. Затем изображение выводится на экран. После этого мы проверяем, есть ли на изображении QR-код. Если он присутствует, то мы сверяем его со списком гостей.

Если код совпадает с одним из кодов из списка, камера выключается, а гость отмечается как прибывший с указанием времени прибытия.
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/46ef395ee7a74febf29d8d576c917a87da17e11d/ViewModel/ScannerQRCodeViewModel.cs#L207-L223

Проверка совпадений QR кода со списком гостей 
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/46ef395ee7a74febf29d8d576c917a87da17e11d/ViewModel/ScannerQRCodeViewModel.cs#L226-L245

## Кодинг и  Декодинг QR кода
Для кодирования использовалась библиотека [QRCoder](https://github.com/codebude/QRCoder) .
Для декодирова ния использовалась библиотека [ZXing.Net](https://github.com/micjahn/ZXing.Net).
### EncodeQRCode 
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/46ef395ee7a74febf29d8d576c917a87da17e11d/Data/QRCode/EncodeQRCode.cs#L7-L37
### DecodeQRCode 
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/46ef395ee7a74febf29d8d576c917a87da17e11d/Data/QRCode/DecodeQRCode.cs#L19-L36

## Парсер XLSX файла
Для парсера использовалась библиотека [Open-XML-SDK](https://github.com/dotnet/Open-XML-SDK/tree/main).Она предназначена для добавления группы гостей.
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/981bf3b46c23016299c2f840e8131beb65fed4d3/Data/Parser/XlsxParser.cs#L20-L103

## Запись в  XLSX файл
Для записи в XLSX файл использовалась библиотека [Aspose.Cells-for-.NET](https://github.com/aspose-cells/Aspose.Cells-for-.NET) .
Предназначен для создания XLSX файла со статистикой гостей по данному мероприятию 
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/981bf3b46c23016299c2f840e8131beb65fed4d3/Data/Record%20File/RecordXlsx.cs#L13-L43



# Основные задачи приложения
### *Создания  (редактрования , удаления) мепоприятей
В приложении есть функции для создания, изменения и удаления мероприятий.
Также доступен просмотр статистики — списка гостей, информации о присутствующих на мероприятии и времени их прибытия. Ещё можно отправить приглашения.
Статистику можно вывести в файл формата xlsx.
![](https://github.com/Dima00132/Pictured-to-describe-the-scanner-appliction/blob/main/%D0%A1%D0%BD%D0%B8%D0%BC%D0%BE%D0%BA%20%D1%8D%D0%BA%D1%80%D0%B0%D0%BD%D0%B0%202024-07-21%20203418.png)

