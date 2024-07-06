using CommunityToolkit.Maui.Views;
using ScannerAndDistributionOfQRCodes.ViewModel;

namespace ScannerAndDistributionOfQRCodes.View;

public sealed partial class DisplayAlertSendMessageProgressPopup : Popup
{
	public DisplayAlertSendMessageProgressPopup(DisplayAlertSendMessageProgressViewModel displayAlertSendMessageProgressViewModel)
	{
		InitializeComponent();
		BindingContext = displayAlertSendMessageProgressViewModel;

    }
}