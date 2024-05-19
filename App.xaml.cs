namespace ScannerAndDistributionOfQRCodes
{
    public partial class App : Application
    {
        public App()
        {
         
            InitializeComponent();

            MainPage = new MainPage(new ScannerQRCodeViewModel());
        }

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
