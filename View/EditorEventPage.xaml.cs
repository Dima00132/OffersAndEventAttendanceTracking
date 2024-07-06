using ScannerAndDistributionOfQRCodes.ViewModel;

namespace ScannerAndDistributionOfQRCodes.View;

public sealed partial class EditorEventPage : ContentPage
{
	public EditorEventPage(EditorEventViewModel editorEventViewModel)
	{
		InitializeComponent();
		BindingContext = editorEventViewModel;
	}

    private void Date_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "Date" | e.PropertyName == "Time")
        {
            labelDate.Text = $" {datePicker.Date.ToString("D")} â {timePicker.Time.ToString("hh':'mm")}";
        }
    }
}