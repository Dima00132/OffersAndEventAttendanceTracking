using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Animations;
using ScannerAndDistributionOfQRCodes.Model;
using ScannerAndDistributionOfQRCodes.Navigation;
using ScannerAndDistributionOfQRCodes.Service.Interface;
using ScannerAndDistributionOfQRCodes.View;
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
        public RelayCommand AddCommand => new(async () =>
        {


            await _navigationService.NavigateByPage<AddScheduledEventPage>(Whole);



            //_localDbService.Create(learnCategory);
            //_localDbService.Update(Learn);
        });
        public RelayCommand<ScheduledEvent> TapCommand => new(async (scheduledEvent) =>
        {
            await _navigationService.NavigateByViewModel<ScannerQRCodeViewModel>(scheduledEvent);
        });
    }
}
