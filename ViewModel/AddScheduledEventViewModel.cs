using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScannerAndDistributionOfQRCodes.Model;
using ScannerAndDistributionOfQRCodes.Navigation;
using ScannerAndDistributionOfQRCodes.Service.Interface;
using ScannerAndDistributionOfQRCodes.ViewModel.Base;
using SixLabors.ImageSharp.Drawing;
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
        private DateTime _date = DateTime.Now;
        [ObservableProperty]
        private TimeSpan _time;
        [ObservableProperty]
        private string _messageText = string.Empty;

        [ObservableProperty]
        private string _organizationData = $"КОНТАКТЫ\r\n<br/>\n+7 (831) 262-19-04 \r\n<br/>\n Аренда залов - нажмите 1\r\n<br/>\nОбучение, повышение квалификации и кадровый подбор - нажмите 2\r\n<br/>\nОрганизационные вопросы - нажмите 3 \r\n<br/>\ninfo@kupnokreml.ru\r\n<br/>\nг. Нижний Новгород,\r\n<br/>\nул. Почаинская, д. 17, КК1";

        public AddScheduledEventViewModel(INavigationService navigationService, ILocalDbService localDbService)
        {
            _navigationService = navigationService;
            _localDbService = localDbService;
        }
        public override Task OnNavigatingToAsync(object? parameter, object? parameterSecond = null)
        {
            if (parameter is WholeEvent whole)
                _whole = whole;
            return base.OnNavigatingToAsync(parameter);
        }

        [RelayCommand(CanExecute = nameof(CheckNameEvent))]
        public async Task AddScheduledEvent()
        {
            var newDate = new DateTime(Date.Year, Date.Month, Date.Day, Time.Hours, Time.Minutes, 0);
            var text = $"{newDate.ToString("D")} в {newDate.ToString("HH:mm")} <br/>{MessageText}";
            var nameEvent = NameEvent.Replace('\r', ' ').Replace('\n',' ') ;
            var sheduledEvent = new ScheduledEvent(nameEvent, newDate)
            {
                MessageText =new  MessageText(text, OrganizationData)
            };
            _whole.Add(sheduledEvent);
            _localDbService.Create(sheduledEvent);
            _localDbService.Create(sheduledEvent.MessageText);
            _localDbService.Update(_whole);

            await _navigationService.NavigateBackUpdate();
        }

        public bool CheckNameEvent() => !string.IsNullOrEmpty(NameEvent);
    }
}
