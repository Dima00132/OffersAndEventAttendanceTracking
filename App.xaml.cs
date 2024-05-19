namespace ScannerAndDistributionOfQRCodes
{
    public partial class App : Application
    {
        public App()
        {
         
            InitializeComponent();

            MainPage = new MainPage(new ScannerQRCodeViewModel());
        }
    }
}
