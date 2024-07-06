using ScannerAndDistributionOfQRCodes.Navigation;

namespace ScannerAndDistributionOfQRCodes
{
    public sealed partial class App : Application
    {

       // private readonly ISettingsApplication _settingsApplication;

        public App(INavigationService navigationService)
        {
            // _initializing = new InitializingApplicationSettings(navigationService, dataService);
            InitializeComponent();
            MainPage = new NavigationPage();
            navigationService.NavigateToMainPageAsync();
           
        }

        //protected override Window CreateWindow(IActivationState activationState)
        //{
        //    Window window = base.CreateWindow(activationState);

        //    // Manipulate Window object

        //    return window;
        //}

        protected override Window CreateWindow(IActivationState activationState) =>
            new Window(MainPage)
            {
                MaximumHeight = 700,
                MaximumWidth = 1200,
                MinimumHeight = 700,
                MinimumWidth = 1200,

            };
    }
}
