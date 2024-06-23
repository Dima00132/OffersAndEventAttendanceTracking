using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.Drawing.Diagrams;
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

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangeScheduledEventCommand))]
        private string _nameEvent;
        [ObservableProperty]
        private DateTime _minDate = DateTime.Now;


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangeScheduledEventCommand))]
        private DateTime _date;

        [ObservableProperty]
        private TimeSpan _time;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangeScheduledEventCommand))]
        private string _messageText ;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangeScheduledEventCommand))]
        private string _organizationData ;

        public EditorEventViewModel(INavigationService navigationService, ILocalDbService localDbService)
        {
            _navigationService = navigationService;
            _localDbService = localDbService;
        }


        [RelayCommand(CanExecute = nameof(CheckEvent))]
        public async Task ChangeScheduledEvent()
        {
            var newDate = new DateTime(Date.Year, Date.Month, Date.Day, Time.Hours, Time.Minutes, 0);
            _scheduledEvent.Change(NameEvent, newDate);
            _scheduledEvent.MessageText.Change(MessageText, OrganizationData);
            _localDbService.Update(_scheduledEvent.MessageText);
            _localDbService.Update(_scheduledEvent);
            await _navigationService.NavigateBack().ConfigureAwait(false);
        }

        public bool CheckEvent()
        {
            if(_scheduledEvent is null)
                return false;
            return !string.IsNullOrEmpty(NameEvent) & Date >= DateTime.Now & !string.IsNullOrEmpty(MessageText) 
                & !string.IsNullOrEmpty(OrganizationData);
        }

        public override Task OnNavigatingTo(object? parameter, object? parameterSecond = null)
        {
            if(parameter is ScheduledEvent scheduledEvent )
            {
                _scheduledEvent = scheduledEvent;
                NameEvent = scheduledEvent.NameEvent;
                Date = scheduledEvent.Date;
                Time =  new TimeSpan(scheduledEvent.Date.Hour, scheduledEvent.Date.Minute, scheduledEvent.Date.Second);
                MessageText = scheduledEvent.MessageText.Text;
                OrganizationData = scheduledEvent.MessageText.OrganizationData;
            }
            return base.OnNavigatingTo(parameter, parameterSecond);
        }
    }
}
 