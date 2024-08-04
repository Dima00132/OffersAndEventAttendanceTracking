# Что такое - OffersAndEventAttendanceTracking
OffersAndEventAttendanceTracking — это приложение на MAUI C# для «Корпоративного Университета Правительства Нижегородской Области», работающее на Windows.

Приложение предназначено для контроля посещения мероприятий за счёт проверки гостей по QR-коду и ведения статистики посещений. В приложении также предусмотрена рассылка сообщений, таких как QR-коды приглашённым гостям, а также уведомлений о различных событиях «Корпоративного Университета Правительства Нижегородской Области» через массовую рассылку.

# Основные библиотеки при создание приложения
## Рассылка сообщений
-Для рассылки использовалась библиотека  [MailKit](https://github.com/jstedfast/MailKit) .
Рассылку реализована следующем образом.
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/e6d5617e05fb7a344fab799348fdc6db48b4c3f9/Data/Message/EmailMessage.cs#L44-L88
## Камера и сканер QR-кода
Чтобы получить доступ к камере, используется библиотека [AForge.NET]( https://github.com/andrewkirillov/AForge.NET) .
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/e6d5617e05fb7a344fab799348fdc6db48b4c3f9/Data/QRCode/ScannerQR.cs#L6-L51

GetVideoInputDevice возвращает FilterInfoCollection, в котором содержится список камер на данном устройстве.
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/e6d5617e05fb7a344fab799348fdc6db48b4c3f9/Data/QRCode/ScannerQR.cs#L22-L26

ConnectingCamera создаёт объект VideoCaptureDevice и подписывается на его события.
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/e6d5617e05fb7a344fab799348fdc6db48b4c3f9/Data/QRCode/ScannerQR.cs#L27-L39

ScannerQR принимает делегаты NewFrameEventHandler (UpdateQrCode) и VideoSourceErrorEventHandler (ErrorCamera).

NewFrameEventHandler (UpdateQrCode) подписывается на события _captureDevice.NewFrame для захвата изображения с веб-камеры.

VideoSourceErrorEventHandler подписывается на события _captureDevice.VideoSourceError и срабатывает, если при запуске веб-камеры возникает ошибка.
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/e6d5617e05fb7a344fab799348fdc6db48b4c3f9/ViewModel/ScannerQRCodeViewModel.cs#L210-L228
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/e6d5617e05fb7a344fab799348fdc6db48b4c3f9/ViewModel/ScannerQRCodeViewModel.cs#L62-L66

В методе UpdateQrCode мы получаем изображение с камеры и устанавливаем его в QRImage. Затем изображение выводится на экран. После этого мы проверяем, есть ли на изображении QR-код. Если он присутствует, то мы сверяем его со списком гостей.

Если код совпадает с одним из кодов из списка, камера выключается, а гость отмечается как прибывший с указанием времени прибытия.
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/e6d5617e05fb7a344fab799348fdc6db48b4c3f9/ViewModel/ScannerQRCodeViewModel.cs#L210-L228

Проверка совпадений QR кода со списком гостей 
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/e6d5617e05fb7a344fab799348fdc6db48b4c3f9/ViewModel/ScannerQRCodeViewModel.cs#L230-L250

## Кодинг и  Декодинг QR кода
Для кодирования использовалась библиотека [QRCoder](https://github.com/codebude/QRCoder) .
Для декодирова ния использовалась библиотека [ZXing.Net](https://github.com/micjahn/ZXing.Net).
### EncodeQRCode 
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/e6d5617e05fb7a344fab799348fdc6db48b4c3f9/Data/QRCode/EncodeQRCode.cs#L7-L37
### DecodeQRCode 
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/e6d5617e05fb7a344fab799348fdc6db48b4c3f9/Data/QRCode/DecodeQRCode.cs#L19-L36

## Парсер XLSX файла
Для парсера использовалась библиотека [Open-XML-SDK](https://github.com/dotnet/Open-XML-SDK/tree/main).Она предназначена для добавления группы гостей.
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/e6d5617e05fb7a344fab799348fdc6db48b4c3f9/Data/Parser/XlsxParser.cs#L20-L103

## Запись в  XLSX файл
Для записи в XLSX файл использовалась библиотека [Aspose.Cells-for-.NET](https://github.com/aspose-cells/Aspose.Cells-for-.NET) .
Предназначен для создания XLSX файла со статистикой гостей по данному мероприятию 
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/e6d5617e05fb7a344fab799348fdc6db48b4c3f9/Data/Record%20File/RecordXlsx.cs#L13-L43

# Навигация
## Интерфейс навигации
Навигация в приложении реализована следующим образом.
Класс навигации должен реализовывать интерфейс INavigationService, который указывает основное поведение. При создании приложения использовался паттерн [MVVM](https://ru.wikipedia.org/wiki/Model-View-ViewModel). В этом паттерне главное правило заключается в том, что ViewModel не должен знать о View. В интерфейсе указывается способ перехода. На момент написания для проверки использовался переход по View, но также возможен переход по ViewModel.
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/e6d5617e05fb7a344fab799348fdc6db48b4c3f9/Navigation/INavigationService.cs#L10-L18

## Класс реализующий интерфейс INavigationService
В основном следует обратить внимание на свойство для навигации (Navigation) и на readonly поле (IServiceProvider)_services.
*Основная задача навигации, как следует из названия, — переход по страницам. Свойство всегда возвращает текущую навигацию (Application.Current?.MainPage?.Navigation).
*(IServiceProvider) _services мы получаем в конструкторе класса, он позволяет использовать встроенные зависимости.
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/e6d5617e05fb7a344fab799348fdc6db48b4c3f9/Navigation/NavigationService.cs#L9-L159
## Внедрения зависимостей 
Внедрение зависимостей происходит в методе расширения MauiAppBuilder ConfigureServices.

В нём мы добавляем основные сервисы в IServiceCollection, такие как View-ViewModel, NavigationService — INavigationService, IPopupService — PopupService, IMailAccount — MailAccount, IDataService — DataService, ILocalDbService — LocalDbService.
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/e6d5617e05fb7a344fab799348fdc6db48b4c3f9/MauiProgram.cs#L20-L45

# Загрузка и сохранение данных
Тип сохранения обговорён с Корпоративным университетом Правительства Нижегородской области, и выбран вариант сохранения на запускаемое устройство. Возможно реализовать сохранение в базу данных.

Сохранения реализуются через [SQLite](https://learn.microsoft.com/ru-ru/dotnet/standard/data/sqlite/?tabs=netcore-cli). Есть класс LocalDbService, реализующий интерфейс ILocalDbService. Всё взаимодействие с ILocalDbService происходит при получении сервиса View Model. ILocalDbService передаётся как зависимость класса при создании (если такая зависимость имеется).

### ILocalDbService
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/e6d5617e05fb7a344fab799348fdc6db48b4c3f9/Service/Interface/ILocalDbService.cs#L7-L21
### LocalDbService
https://github.com/Dima00132/OffersAndEventAttendanceTracking/blob/e6d5617e05fb7a344fab799348fdc6db48b4c3f9/Service/LocalDbService.cs#L19-L172

# Основные задачи приложения
## Создания  (редактрования , сатистика , удаления) мепоприятей
В приложении есть функции для создания, изменения и удаления мероприятий.
Также доступен просмотр статистики — списка гостей, информации о присутствующих на мероприятии и времени их прибытия. Ещё можно отправить приглашения.
Статистику можно вывести в файл формата xlsx.

## Сканирование QR кода гостя 
Основная задача — чтобы гости, пришедшие на мероприятие, предоставили QR-код, который был выслан им на почту.

Если гость есть в списке, он отмечается как «Пришедший» с отметкой времени. Если же гостя нет в списках или он уже присутствует на мероприятии, появится сообщение с указанием, что гость уже присутствует или что QR-кода гостя нет в общем списке.

Если же гость не может предоставить QR-код для прохода, его можно проверить по списку гостей и отметить посещение вручную.

## Список гостей ( добавление , рассылка QR кода , добавление гостей из XLSX файла)
В списке гостей можно добавить информацию о госте в виде (Фамилия Имя Отчество, e-mail). Все поля обязательны для заполнения. При добавлении почты происходит проверка на правильность введения адреса домена.

Добавление через файл XLSХ. Перед добавлением гостей необходимо сначала указать, в каких столбцах находятся необходимые данные. Также гости из файла не будут добавлены, если они уже присутствуют в списке гостей.
Если в адресе почты будет указан неверный домен, гостю будет присвоен статус «Ошибка почты».
Гости со статусом «Ошибка почты» не получат приглашения.

Гости могут получать рассылку:

1. Автоматически: рассылка отправляется всем гостям, кроме тех, кто уже получил приглашение.
2. Вручную: гость получает индивидуальное приглашение при выборе его в списке и нажатии кнопки «Отправить сообщение».
   
При возникновении ошибки во время отправки приглашения автоматически отображается сообщение об этом с указанием гостя и причины ошибки.

## Настройки пользователя 
«Персонализация» — одна из настроек приложения, которая позволяет отправлять сообщения через сервис [Unisender Go](https://go1.unisender.ru/ru).

Персонализацию можно настроить следующим образом:
### 1. Фамилия, Имя, Отчество получателя;
### 2. Маил-сервер — параметры соединения по SMTP в [Unisender Go](https://go1.unisender.ru/ru):
 * smtp.go1.unisender.ru, smtp.go2.unisender .ru — указание домена;
 * порт — 25*, 465 или 587;
 * Включение зашифрованного соединения.
### 3. Последняя строка — заполнение данных из [Unisender Go](https://go1.unisender.ru/ru), которые включают.
* Доменную почту, имеющуюся у пользователя (после приобретения домена и создания соответствующей почты). 
* Также требуется указать аккаунт пользователя в [Unisender Go](https://go1.unisender.ru/ru).
* Пароль — API-ключ пользователя или project_api_key.




## Уведомления о различных событиях «Корпоративного Университета Правительства Нижегородской Области» через массовую рассылку.
Последняя часть приложения – это сообщение о событиях, которое представляет собой рассылку. При отправке рассылки в список получателей добавляют адресатов из файла XLSX.

В тексте сообщения содержатся следующие части:

1. Тема рассылки;
2. Текст рассылки;
3. Изображение (При необходимости);
4. Данные организации (обязательно для отправки сообщения).

Прогресс отправки похож на прогресс отправки QR-кода или сообщения об ошибке.

