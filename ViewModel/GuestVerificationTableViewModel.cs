using CommunityToolkit.Mvvm.ComponentModel;
using ScannerAndDistributionOfQRCodes.Model;
using ScannerAndDistributionOfQRCodes.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerAndDistributionOfQRCodes.ViewModel
{
    public sealed partial class GuestVerificationTableViewModel:ViewModelBase
    {
        [ObservableProperty]
        private  ScannerQRCodeViewModel _scannerQRCodeViewModel;
        [ObservableProperty]
        private  GuestListViewModel _guestListViewModel;

        public GuestVerificationTableViewModel(ScannerQRCodeViewModel scannerQRCodeViewModel,GuestListViewModel guestListViewModel)
        {
            _scannerQRCodeViewModel = scannerQRCodeViewModel;
            _guestListViewModel = guestListViewModel;
        }

        public override Task OnUpdateAsync()
        {
            ScannerQRCodeViewModel?.OnUpdateAsync();
            GuestListViewModel?.OnUpdateAsync();
            return base.OnUpdateAsync();
        }

        public override Task OnUpdateDbServiceAsync()
        {
            ScannerQRCodeViewModel?.OnUpdateDbServiceAsync();
            GuestListViewModel?.OnUpdateDbServiceAsync();
            return base.OnUpdateDbServiceAsync();
        }

        public override Task OnNavigatingToAsync(object parameter, object parameterSecond = null)
        {
            if (parameter is ScheduledEvent scheduledEvent)
            {
                ScannerQRCodeViewModel?.OnNavigatingToAsync(scheduledEvent);
                GuestListViewModel?.OnNavigatingToAsync(scheduledEvent);
            }
            return base.OnNavigatingToAsync(parameter);
        }
    }
}
