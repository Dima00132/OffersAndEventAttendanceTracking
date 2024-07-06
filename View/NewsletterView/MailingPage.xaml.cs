using ScannerAndDistributionOfQRCodes.ViewModel.NewsletterViewModel;

namespace ScannerAndDistributionOfQRCodes.View.NewsletterView;

public sealed partial class MailingPage : ContentPage
{
	public MailingPage(MailingViewModel mailingListViewModel)
	{
		InitializeComponent();
		BindingContext = mailingListViewModel;
	}
}