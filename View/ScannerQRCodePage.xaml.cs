namespace ScannerAndDistributionOfQRCodes.View;

public partial class ScannerQRCodePage : ContentPage
{
    public ScannerQRCodePage()
    {
        InitializeComponent();
    }

    public ScannerQRCodePage(ScannerQRCodeViewModel scannerQRCodeViewModel):base()
	{
		
		BindingContext = scannerQRCodeViewModel;

    }
}