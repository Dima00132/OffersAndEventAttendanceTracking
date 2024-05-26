using Camera.MAUI;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using Microsoft.Extensions.Logging;
using ScannerAndDistributionOfQRCodes;
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


            builder.Services.AddSingleton<IPopupService, PopupService>().AddTransientPopup<GuestListFromDocumentPopup, GuestListFromDocumentViewModel>();
            //builder.Services .AddTransient<GuestListFromDocumentViewModel>().AddTransient<GuestListFromDocumentPopup>();


            //builder.Services.AddTransient<ScannerQRCodePage>();
            //builder.Services.AddTransient<GuestListPage>();

            builder.Services.AddTransient<AddScheduledEventViewModel>().AddTransient<AddScheduledEventPage>();
            builder.Services.AddTransient<GuestVerificationTableViewModel>().AddTransient<GuestVerificationTablePage>();


            builder.Services.AddSingleton<IDataService, DataService>();

            builder.Services.AddSingleton<ILocalDbService, LocalDbService>();

            builder.Services.AddSingleton<INavigationService, NavigationService>();

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

