using ScannerAndDistributionOfQRCodes.ViewModel;

namespace ScannerAndDistributionOfQRCodes.View;

public partial class ListOfEventsPage : ContentPage
{
	public ListOfEventsPage(ListOfEventsViewModel listOfEventsViewModel )
	{
		InitializeComponent();
		BindingContext = listOfEventsViewModel;
	}
}