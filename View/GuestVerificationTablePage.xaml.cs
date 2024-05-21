using ScannerAndDistributionOfQRCodes.ViewModel;

namespace ScannerAndDistributionOfQRCodes.View;

public partial class GuestVerificationTablePage : TabbedPage
{
    private readonly GuestVerificationTableViewModel _guestVerificationTableViewModel;
    public GuestVerificationTablePage(GuestVerificationTableViewModel guestVerificationTableViewModel)
    {
        InitializeComponent();
        BindingContext = _guestVerificationTableViewModel = guestVerificationTableViewModel;
    }

    protected override void OnAppearing()
    {
        _guestVerificationTableViewModel.OnUpdate();
        base.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        _guestVerificationTableViewModel.OnUpdateDbService();
        base.OnDisappearing();
    }
}