using ScannerAndDistributionOfQRCodes.ViewModel;

namespace ScannerAndDistributionOfQRCodes.View;

public partial class AddScheduledEventPage : ContentPage
{
	public AddScheduledEventPage(AddScheduledEventViewModel addScheduledEventViewModel)
	{
		InitializeComponent();
		BindingContext = addScheduledEventViewModel;
	}


    private void Date_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "Date" | e.PropertyName == "Time")
        {
            labelDate.Text = $"Вы выбрали {datePicker.Date.ToString("D")} {timePicker.Time}";
        }
    }
}