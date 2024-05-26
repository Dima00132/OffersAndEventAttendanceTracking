﻿using ScannerAndDistributionOfQRCodes.Navigation;

namespace ScannerAndDistributionOfQRCodes
{
    public partial class App : Application
    {

       // private readonly ISettingsApplication _settingsApplication;

        public App(INavigationService navigationService)
        {
            // _initializing = new InitializingApplicationSettings(navigationService, dataService);
            InitializeComponent();
            MainPage = new NavigationPage();
            navigationService.NavigateToMainPage();
           
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
                MaximumHeight = 750,
                MaximumWidth = 1300,
                MinimumHeight = 750,
                MinimumWidth = 1300,

            };
    }
}
