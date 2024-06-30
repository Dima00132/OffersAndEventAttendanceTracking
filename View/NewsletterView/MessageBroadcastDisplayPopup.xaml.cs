using CommunityToolkit.Maui.Views;
using ScannerAndDistributionOfQRCodes.ViewModel.NewsletterViewModel;

namespace ScannerAndDistributionOfQRCodes.View.NewsletterView;

public partial class MessageBroadcastDisplayPopup: Popup
{
	public MessageBroadcastDisplayPopup(MessageBroadcastDisplayViewModel messageBroadcastDisplayViewModel)
	{
		InitializeComponent();
		BindingContext = messageBroadcastDisplayViewModel;
	}

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry)
        {
            if (entry.Text.Length > 0)
            {
                parserButton.IsVisible = true;
                imageEntry.IsVisible = true;
            }
            else
            {
                imageEntry.IsVisible = false;
                parserButton.IsVisible = false;
                sendButton.IsEnabled = false;
            }
        }
    }

    private void sendButton_Clicked(object sender, EventArgs e)
    {
        stateSend.IsVisible = true;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        stateSend.IsVisible = false;
    }
}