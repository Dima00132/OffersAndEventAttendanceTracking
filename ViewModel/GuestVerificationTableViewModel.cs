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
    public partial class GuestVerificationTableViewModel:ViewModelBase
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

        public override Task OnUpdate()
        {
            _scannerQRCodeViewModel?.OnUpdate();
            _guestListViewModel?.OnUpdate();
            return base.OnUpdate();
        }

        public override Task OnUpdateDbService()
        {
            _scannerQRCodeViewModel?.OnUpdateDbService();
            _guestListViewModel?.OnUpdateDbService();
            return base.OnUpdateDbService();
        }

        public override Task OnNavigatingTo(object? parameter, object? parameterSecond = null)
        {
            if (parameter is ScheduledEvent scheduledEvent)
            {
                _scannerQRCodeViewModel?.OnNavigatingTo(scheduledEvent);
                _guestListViewModel?.OnNavigatingTo(scheduledEvent);

            }
            return base.OnNavigatingTo(parameter);
        }
    }
}
