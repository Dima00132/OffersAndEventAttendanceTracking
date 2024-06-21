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
        //[NotifyCanExecuteChangedFor(nameof(AddScheduledEventCommand))]
        private string _nameEvent;
        [ObservableProperty]
        private DateTime _minDate = DateTime.Now;


        [ObservableProperty]
        private DateTime _date;
        [ObservableProperty]
        private TimeSpan _time;
        [ObservableProperty]
        private string _messageText ;

        [ObservableProperty]
        private string _organizationData ;

        public EditorEventViewModel(INavigationService navigationService, ILocalDbService localDbService)
        {
            _navigationService = navigationService;
            _localDbService = localDbService;
        }


        [RelayCommand(CanExecute = nameof(CheckNameEvent))]
        public async Task AddScheduledEvent()
        {
            //var newDate = new DateTime(Date.Year, Date.Month, Date.Day, Time.Hours, Time.Minutes, 0);
            //var text = $"{newDate.ToString("D")} в {newDate.ToString("HH:mm")} <br/>{MessageText}";
            //var nameEvent = NameEvent.Replace('\r', ' ').Replace('\n', ' ');
            //var sheduledEvent = new ScheduledEvent(nameEvent, newDate)
            //{
            //    MessageText = new MessageText(text, OrganizationData)
            //};
            //_whole.Add(sheduledEvent);
            //_localDbService.Create(sheduledEvent);
            //_localDbService.Create(sheduledEvent.MessageText);
            //_localDbService.Update(_whole);

            //await _navigationService.NavigateBackUpdate();
        }

        public bool CheckNameEvent() => !string.IsNullOrEmpty(NameEvent);

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
