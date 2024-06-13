using Camera.MAUI;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ScannerAndDistributionOfQRCodes;
using ScannerAndDistributionOfQRCodes.Data.Message;
using ScannerAndDistributionOfQRCodes.Model;
using ScannerAndDistributionOfQRCodes.Navigation;
using ScannerAndDistributionOfQRCodes.Service;
using ScannerAndDistributionOfQRCodes.Service.Interface;
using ScannerAndDistributionOfQRCodes.View;
using ScannerAndDistributionOfQRCodes.ViewModel;


namespace ScannerAndDistributionOfQRCodes
{
    public static class ServicesExtensions
    {
        public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
        {

            builder.Services.AddTransient<ScannerQRCodeViewModel>().AddTransient<ScannerQRCodePage>();
            builder.Services.AddTransient<GuestListViewModel>().AddTransient<GuestListPage>();

            //var mailAccaunt = new MailAccount("TestMailSendr@yandex.ru", "cwufaysygkohokyr",
            //    new User("Иванов", "Иван", "Иванович"),
            //    new MailServer("smtp.yandex.ru", 465, true));

            //var mailAccaunt = new MailAccount("6686967", "testsend@nizhny.online", "6a8dtydwniakm3fgmy1zrn1q93yd1o176k39b96y",
            //new User("Иванов", "Иван", "Иванович"),
            //new MailServer("smtp.go1.unisender.ru", 465, true));


            //builder.Services.AddSingleton<IMailAccount, MailAccount>((x)=>mailAccaunt);
            builder.Services.AddSingleton<IMailAccount, MailAccount>();
            builder.Services.AddSingleton<IPopupService, PopupService>()
                .AddTransientPopup<GuestListFromDocumentPopup, GuestListFromDocumentViewModel>()
                .AddTransientPopup<DisplayAlertSendingMessagesErrorPopup, DisplayAlertSendingMessagesErrorViewModel>()
                .AddTransientPopup<DisplayAlertSendMessageProgressPopup, DisplayAlertSendMessageProgressViewModel>();
            //builder.Services.AddSingleton<IPopupService, PopupService>().AddTransientPopup<DisplayAlertSendingMessagesErrorPopup, DisplayAlertSendingMessagesErrorViewModel>();
            //builder.Services .AddTransient<GuestListFromDocumentViewModel>().AddTransient<GuestListFromDocumentPopup>();


            //builder.Services.AddTransient<ScannerQRCodePage>();
            //builder.Services.AddTransient<GuestListPage>();

            builder.Services.AddTransient<AddScheduledEventViewModel>().AddTransient<AddScheduledEventPage>();
            builder.Services.AddTransient<GuestVerificationTableViewModel>().AddTransient<GuestVerificationTablePage>();

            builder.Services.AddTransient<SettingsPage>().AddTransient<SettingsViewModel>();

            builder.Services.AddSingleton<IDataService, DataService>();

            builder.Services.AddSingleton<ILocalDbService, LocalDbService>();

            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddTransient<ListOfEventsPage>().AddTransient<ListOfEventsViewModel>();

            builder.Services.AddSingleton<MainPage>().AddSingleton<MainViewModel>();


            return builder;
        }
    }


    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureServices()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

