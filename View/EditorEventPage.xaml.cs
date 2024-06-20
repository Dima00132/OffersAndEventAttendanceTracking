using ScannerAndDistributionOfQRCodes.ViewModel;

namespace ScannerAndDistributionOfQRCodes.View;

public partial class EditorEventPage : ContentPage
{
	public EditorEventPage(EditorEventViewModel editorEventViewModel)
	{
		InitializeComponent();
		BindingContext = editorEventViewModel;
	}
}