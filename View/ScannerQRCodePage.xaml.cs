namespace ScannerAndDistributionOfQRCodes.View;

public partial class ScannerQRCodePage : ContentPage
{
	public ScannerQRCodePage(ScannerQRCodeViewModel scannerQRCodeViewModel)
	{
		InitializeComponent();
		BindingContext = scannerQRCodeViewModel;

    }
}