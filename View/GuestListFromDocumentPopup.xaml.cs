using CommunityToolkit.Maui.Views;
using ScannerAndDistributionOfQRCodes.ViewModel;
namespace ScannerAndDistributionOfQRCodes.View;

public partial class GuestListFromDocumentPopup  : Popup
{
	public GuestListFromDocumentPopup(GuestListFromDocumentViewModel guestListFromDocumentViewModel)
	{
		InitializeComponent();
		BindingContext = guestListFromDocumentViewModel;

    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        confirmGrid.IsVisible = false;
        stackLayoutSelectingSpeakers.IsVisible = false;
        collectionGuests.IsVisible = true;
        gridSaveCancelCommand.IsVisible = true;
    }
}