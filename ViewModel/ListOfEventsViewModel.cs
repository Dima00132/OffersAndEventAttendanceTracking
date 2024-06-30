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
    public partial class ListOfEventsViewModel:ViewModelBase
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
            await _navigationService.NavigateByPage<AddScheduledEventPage>(Whole);
        });
        public RelayCommand<ScheduledEvent> TapCommand => new(async (scheduledEvent) =>
        {
            //var timeOfEvent = scheduledEvent.Date - DateTime.Now.AddDays(1);
            if (scheduledEvent.IsEventWasHeld)
            {
                await _navigationService.NavigateByPage<StatisticsPage>(scheduledEvent);
                return;
            }

            //var timeOfEvent = scheduledEvent.Date - DateTime.Now.AddDays(1);
            //if (timeOfEvent.Days < -1)
            //{
            //    await _navigationService.NavigateByPage<StatisticsPage>(scheduledEvent);
            //    return;
            //}
            await _navigationService.NavigateByPage<GuestVerificationTablePage>(scheduledEvent);
        });

        public RelayCommand<ScheduledEvent> DeleteCommand => new(async (scheduledEvent) =>
        {
            Whole.ScheduledEvents.Remove(scheduledEvent);
            Scheduleds.Remove(scheduledEvent);
            _localDbService.Update(Whole);
        });

        public RelayCommand<ScheduledEvent> StatisticsCommand => new(async (scheduledEvent) =>
        {
            await _navigationService.NavigateByPage<StatisticsPage>(scheduledEvent);
        });

        
        public RelayCommand<ScheduledEvent> EditorCommand => new(async (scheduledEvent) =>
        {
            await _navigationService.NavigateByPage<EditorEventPage>(scheduledEvent);
        });

        public override Task OnUpdateDbServiceAsync()
        {
            _localDbService.Update(Whole);
            return base.OnUpdateDbServiceAsync();
        }
    }
}
