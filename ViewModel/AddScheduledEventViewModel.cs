using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScannerAndDistributionOfQRCodes.Model;
using ScannerAndDistributionOfQRCodes.Navigation;
using ScannerAndDistributionOfQRCodes.Service.Interface;
using ScannerAndDistributionOfQRCodes.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerAndDistributionOfQRCodes.ViewModel
{
    public partial class  AddScheduledEventViewModel :ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ILocalDbService _localDbService;
        private WholeEvent _whole;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddScheduledEventCommand))]
        private string _nameEvent;
        [ObservableProperty]
        private DateTime _minDate = DateTime.Now;

        
        [ObservableProperty]
        private DateTime _date;
        [ObservableProperty]
        private TimeSpan _time;
        [ObservableProperty]
        private string _messageText = string.Empty;

        public AddScheduledEventViewModel(INavigationService navigationService, ILocalDbService localDbService)
        {
            _navigationService = navigationService;
            _localDbService = localDbService;
        }
        public override Task OnNavigatingTo(object? parameter, object? parameterSecond = null)
        {
            if (parameter is WholeEvent whole)
                _whole = whole;
            return base.OnNavigatingTo(parameter);
        }

        [RelayCommand(CanExecute = nameof(CheckNameEvent))]
        public async Task AddScheduledEvent()
        {
            var newDate = new DateTime(Date.Year, Date.Month, Date.Day, Time.Hours, Time.Minutes,0);
            var sheduledEvent = new ScheduledEvent(NameEvent, newDate)
            {
                MessageText =MessageText,
            };
            _whole.Add(sheduledEvent);

            //_localDbService.CreateAndUpdate(sheduledEvent, _whole);
            _localDbService.Create(sheduledEvent);
            _localDbService.Update(_whole);

            await _navigationService.NavigateBackUpdate();
        }

        public bool CheckNameEvent() => !string.IsNullOrEmpty(NameEvent);
    }
}
