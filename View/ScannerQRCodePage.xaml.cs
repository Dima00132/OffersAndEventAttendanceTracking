namespace ScannerAndDistributionOfQRCodes.View;

public sealed partial class ScannerQRCodePage : ContentPage
{
    public ScannerQRCodePage()
    {
        InitializeComponent();
    }

    public ScannerQRCodePage(ScannerQRCodeViewModel scannerQRCodeViewModel):base()
	{
		
		BindingContext = scannerQRCodeViewModel;

    }

    protected override void OnDisappearing()
    {
        if (BindingContext is ScannerQRCodeViewModel viewModel)
            viewModel.Close();
        base.OnDisappearing();
    }
}