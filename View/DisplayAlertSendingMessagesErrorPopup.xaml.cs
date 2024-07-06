using CommunityToolkit.Maui.Views;
using ScannerAndDistributionOfQRCodes.ViewModel;

namespace ScannerAndDistributionOfQRCodes.View;

public sealed partial class DisplayAlertSendingMessagesErrorPopup : Popup
{
    public DisplayAlertSendingMessagesErrorPopup(DisplayAlertSendingMessagesErrorViewModel displayAlertSendingMessagesErrorViewModel)
	{
		InitializeComponent();
		BindingContext = displayAlertSendingMessagesErrorViewModel;
	}
}