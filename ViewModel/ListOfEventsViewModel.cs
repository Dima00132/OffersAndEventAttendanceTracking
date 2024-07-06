using CommunityToolkit.Mvvm.Input;
using ScannerAndDistributionOfQRCodes.Model;
using ScannerAndDistributionOfQRCodes.Navigation;
using ScannerAndDistributionOfQRCodes.Service.Interface;
using ScannerAndDistributionOfQRCodes.View;
using ScannerAndDistributionOfQRCodes.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerAndDistributionOfQRCodes.ViewModel
{
    public sealed partial class ListOfEventsViewModel:ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ILocalDbService _localDbService;


        public WholeEvent Whole { get; set; }
        public ObservableCollection<ScheduledEvent> Scheduleds { get; set; }
        public ListOfEventsViewModel(INavigationService navigationService, ILocalDbService localDbService)
        {
            _navigationService = navigationService;
            _localDbService = localDbService;
            Whole = localDbService.GetWholeEvent();

            Scheduleds = Whole
                .SortedCategories()
                .GetWholeEvents();
        }

  
        public RelayCommand AddCommand => new(async () =>
        {
            await _navigationService.NavigateByPageAsync<AddScheduledEventPage>(Whole);
        });
        public RelayCommand<ScheduledEvent> TapCommand => new(async (scheduledEvent) =>
        {
            if (scheduledEvent.IsEventWasHeld)
            {
                await _navigationService.NavigateByPageAsync<StatisticsPage>(scheduledEvent);
                return;
            }
            await _navigationService.NavigateByPageAsync<GuestVerificationTablePage>(scheduledEvent);
        });

        public RelayCommand<ScheduledEvent> DeleteCommand => new((scheduledEvent) =>
        {
            Whole.ScheduledEvents.Remove(scheduledEvent);
            Scheduleds.Remove(scheduledEvent);
            _localDbService.Update(Whole);
        });

        public RelayCommand<ScheduledEvent> StatisticsCommand => new(async (scheduledEvent) =>
        {
            await _navigationService.NavigateByPageAsync<StatisticsPage>(scheduledEvent);
        });

        
        public RelayCommand<ScheduledEvent> EditorCommand => new(async (scheduledEvent) =>
        {
            await _navigationService.NavigateByPageAsync<EditorEventPage>(scheduledEvent);
        });

        public override Task OnUpdateDbServiceAsync()
        {
            _localDbService.Update(Whole);
            return base.OnUpdateDbServiceAsync();
        }
    }
}
