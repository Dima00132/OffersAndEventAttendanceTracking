
using Microsoft.Maui.ApplicationModel.DataTransfer;
using ScannerAndDistributionOfQRCodes.ViewModel;

namespace ScannerAndDistributionOfQRCodes.View;

public partial class GuestListPage : ContentPage
{
    public GuestListPage()
    {
        InitializeComponent();
    }
    private void DragGestureRecognizer_DragStarting_1(object sender, DragStartingEventArgs e)
    {
        var label = (sender as Element)?.Parent as Label;
        e.Data.Properties.Add("Text", label.Text);
    }


    protected override void OnAppearing()
    {
        if (BindingContext is GuestListViewModel model)
            _ = model.UpdateCommand;
        base.OnAppearing();
    }
    public GuestListPage(GuestListViewModel guestListViewModel) : base()
	{
		
		BindingContext = guestListViewModel;

    }
}