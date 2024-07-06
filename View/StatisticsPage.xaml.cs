using ScannerAndDistributionOfQRCodes.ViewModel;

namespace ScannerAndDistributionOfQRCodes.View;

public sealed partial class StatisticsPage : ContentPage
{
	public StatisticsPage(StatisticsViewModel  statisticsViewModel)
	{
		InitializeComponent();
		BindingContext = statisticsViewModel;
	}

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

        listGuests.IsVisible = !listGuests.IsVisible;

        if (listGuests.IsVisible)
            image.SetAppTheme<FileImageSource>(Image.SourceProperty, "caret_up_black.png", "caret_up_white.png");
        else
            image.SetAppTheme<FileImageSource>(Image.SourceProperty, "caret_down_black.png", "caret_down_white.png");
    }
}	