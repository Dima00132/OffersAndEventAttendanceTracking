using ScannerAndDistributionOfQRCodes.Model;
using ScannerAndDistributionOfQRCodes.Navigation;
using ScannerAndDistributionOfQRCodes.Service.Interface;
using ScannerAndDistributionOfQRCodes.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerAndDistributionOfQRCodes.ViewModel
{
    public partial class EditorEventViewModel:ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ILocalDbService _localDbService;

        private ScheduledEvent _scheduledEvent;

        public EditorEventViewModel(INavigationService navigationService, ILocalDbService localDbService)
        {
            _navigationService = navigationService;
            _localDbService = localDbService;
        }

        public override Task OnNavigatingTo(object? parameter, object? parameterSecond = null)
        {
            if(parameter is ScheduledEvent scheduledEvent )
            {
                _scheduledEvent = scheduledEvent;
            }
            return base.OnNavigatingTo(parameter, parameterSecond);
        }
    }
}
