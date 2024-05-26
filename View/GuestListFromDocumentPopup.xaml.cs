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

}