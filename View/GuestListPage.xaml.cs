
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

    private void DropGestureRecognizer_Drop_1(object sender, DropEventArgs e)
    {
     
        //if (e.Data.GetDataPresent(DataFormats.FileDrop))
        //{
        //    // можно же перетянуть много файлов, так что....
        //    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

        //    // делаешь что-то
        //}
    }
    public GuestListPage(GuestListViewModel guestListViewModel) : base()
	{
		
		BindingContext = guestListViewModel;

    }
}