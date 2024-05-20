using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using ScannerAndDistributionOfQRCodes.Model;
using ScannerAndDistributionOfQRCodes.Navigation;
using ScannerAndDistributionOfQRCodes.Service.Interface;
using ScannerAndDistributionOfQRCodes.ViewModel.Base;

namespace ScannerAndDistributionOfQRCodes.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ILocalDbService _localDbService;


        public WholeEvent Whole { get; set; }
        public ObservableCollection<ScheduledEvent> Scheduleds { get; set; }
        public MainViewModel(INavigationService navigationService, ILocalDbService localDbService)
        {
            _navigationService = navigationService;
            _localDbService = localDbService;
            Whole = localDbService.GetWholeEvent();

            Scheduleds = Whole
                .SortedCategories()
                .GetWholeEvents();
            //Learn.SortedCategories((x)=> x.LastActivity);
            //Learn.SetUpdateDbEvent(OnEventHandlerLearn);

            //Learn.SortedCategories((x) => x.LastActivity);
        }
        public RelayCommand<ScheduledEvent> TapCommand => new(async (scheduledEvent) =>
        {
            await _navigationService.NavigateByViewModel<ScannerQRCodeViewModel>(scheduledEvent);
        });
    }
}
