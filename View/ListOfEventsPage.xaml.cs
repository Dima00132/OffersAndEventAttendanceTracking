using ScannerAndDistributionOfQRCodes.ViewModel;

namespace ScannerAndDistributionOfQRCodes.View;

public sealed partial class ListOfEventsPage : ContentPage
{
	public ListOfEventsPage(ListOfEventsViewModel listOfEventsViewModel )
	{
		InitializeComponent();
		BindingContext = listOfEventsViewModel;
	}
}