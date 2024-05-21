using ScannerAndDistributionOfQRCodes.ViewModel;

namespace ScannerAndDistributionOfQRCodes.View;

public partial class GuestListPage : ContentPage
{
    public GuestListPage()
    {
        InitializeComponent();
    }

    public GuestListPage(GuestListViewModel guestListViewModel) : base()
	{
		
		BindingContext = guestListViewModel;

    }
}