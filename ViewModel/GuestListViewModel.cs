using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScannerAndDistributionOfQRCodes.Model;
using ScannerAndDistributionOfQRCodes.Model.Message;
using ScannerAndDistributionOfQRCodes.Model.Parser;
using ScannerAndDistributionOfQRCodes.Navigation;
using ScannerAndDistributionOfQRCodes.Service.Interface;
using ScannerAndDistributionOfQRCodes.View;
using ScannerAndDistributionOfQRCodes.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ScannerAndDistributionOfQRCodes.ViewModel
{
    public partial class GuestListViewModel:ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ILocalDbService _localDbService;

        private ScheduledEvent _scheduledEvent;
       
        private ObservableCollection<Guest> _guests;
        public ObservableCollection<Guest> Guests
        {
            get => _guests;
            set => SetProperty(ref _guests, value);

        }

    


        [ObservableProperty]
        private Guest _guest;



        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string _surname;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string _name;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string _patronymic;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string _mail;

        [ObservableProperty]
        private bool _isEditor = false;

        [ObservableProperty]
        private bool _isVisibleAddGuest = false; 
        [ObservableProperty]
        private bool _isVisibleChangeGuest = false;

        public GuestListViewModel(INavigationService navigationService, ILocalDbService localDbService)
        {
            _navigationService = navigationService;
            _localDbService = localDbService;

        }


        public RelayCommand<Guest> ChangeCommand => new(async (guest) =>
        {
            Cancel();
            Guest = guest;
            InstallationValues(guest);
            IsVisibleChangeGuest = true;
            IsEditor = true;
        });

        public RelayCommand AddGuestCommand => new(async () =>
        {
            Cancel();
            IsVisibleAddGuest = true;
            IsEditor = true;
        });


        public RelayCommand SendCommand => new(async () =>
        {
            _scheduledEvent.SendMessageEvent?.Invoke(_scheduledEvent,EventArgs.Empty);
        });

        public RelayCommand ParseCommand => new(async () =>
        {
            var pars = new XlsxParser();
            pars.Pars("C:\\Users\\dima7\\OneDrive\\Рабочий стол\\TestPars.xlsx");
        });



        [RelayCommand(CanExecute = nameof(CheckNameEvent))]
        public async Task Save()
        {
            if (IsVisibleAddGuest)
                Guests.Add( CreationGuest());
            if (IsVisibleChangeGuest)
                ChangeGuest(Guest);
            IsEditor = false;
            ClearValues();
            _localDbService.Update(_scheduledEvent);
        }

        [RelayCommand]
        public void Cancel()
        {
            IsVisibleAddGuest = false;
            IsVisibleChangeGuest = false;
            IsEditor = false;
            ClearValues();
        }

        public RelayCommand<Guest> DeleteCommand => new(async (guest) =>
        {
            _scheduledEvent.SendMessageEvent -= guest.GetUpSubscriptionForSendingMessages();
            Guests.Remove(guest);
            _localDbService.Update(_scheduledEvent);
        });

        private Guest CreationGuest()
        {
            IsVisibleAddGuest = false;
            var guest = new Guest().SetSurname(Surname).SetPatronymic(Patronymic).SetName(Name).SetMail(Mail);
            _scheduledEvent.SendMessageEvent += guest.GetUpSubscriptionForSendingMessages();
            _localDbService.Create(guest);
            return guest;
        }
        private void ChangeGuest(Guest guest)
        {
            guest.SetSurname(Surname).SetPatronymic(Patronymic).SetName(Name).SetMail(Mail);
            IsVisibleChangeGuest = false;
            _localDbService.Update(guest);
        }

     

        private void InstallationValues(Guest guest)
        {
            Surname = guest.User.Surname.ToString();
            Name = guest.User.Name.ToString();
            Patronymic = guest.User.Patronymic.ToString();
            Mail = guest.Mail.ToString();
        }
        private void ClearValues()
        {
            Surname = string.Empty;
            Name = string.Empty;
            Patronymic = string.Empty;
            Mail = string.Empty;
        }

        public bool CheckNameEvent()
        {
            return !string.IsNullOrEmpty(Surname) & !string.IsNullOrEmpty(Name)
                & !string.IsNullOrEmpty(Patronymic) & EmailValidator.CheckEmailValidator(Mail);
        }

        private void SubscribingToMessageSendingEvents(ScheduledEvent scheduled, ObservableCollection<Guest> guests)
        {
            isStart = false;
            foreach (var item in guests.Where(x => !x.IsMessageSent))
                scheduled.SendMessageEvent += item.GetUpSubscriptionForSendingMessages();

        }


        private bool isStart = true;

        public override Task OnNavigatingTo(object? parameter, object? parameterSecond = null)
        {
            if (parameter is ScheduledEvent scheduledEvent)
            {
                _scheduledEvent = scheduledEvent;
                Guests = scheduledEvent.Guests;
                SubscribingToMessageSendingEvents(_scheduledEvent, Guests);
            }
            return base.OnNavigatingTo(parameter);
        }
    }
}
