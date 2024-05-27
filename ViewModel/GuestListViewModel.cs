
using CommunityToolkit.Maui.Core;
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ScannerAndDistributionOfQRCodes.ViewModel
{
    public partial class GuestListViewModel:ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ILocalDbService _localDbService;
        private readonly IPopupService popupService;
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

        [ObservableProperty]
        private int _countGuest;
        [ObservableProperty]
        private int _countSendMessage;


        public GuestListViewModel(INavigationService navigationService, ILocalDbService localDbService, IPopupService popupService)
        {
            _navigationService = navigationService;
            _localDbService = localDbService;
            this.popupService = popupService;
        }


        private async Task OnSearchTextChangedAsync(object keyword)
        {
            var query = keyword as string;
            if (string.IsNullOrEmpty(query))
            {
                Guests = _scheduledEvent.Guests;
                return;
            }

            if (!string.IsNullOrEmpty(query) && query.Length >= 1)
            {
                var data = await Task.FromResult(_scheduledEvent.FindsQuestionByRequest(query));
                if (data is not null)
                    Guests = new ObservableCollection<Guest>(data);
            }
        }

        public RelayCommand<string?> PerformSearchCommand => new(async (string? query) => await OnSearchTextChangedAsync(query));




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

        public RelayCommand<Guest> SendGuestCommand => new(async (gueet) =>
        {
            SendGuestMessage(gueet,true);
        });


        private void SendGuestMessage(Guest gueet, bool resendMessage)
        {
            if (!InternetCS.IsConnectedToInternet())
            {
                Application.Current.MainPage.DisplayAlert("Предупреждение", "Отсутствует доступ к интернету!", "ОK");
                return;
            }

            gueet.SendingMessages(_scheduledEvent.NameEvent, _scheduledEvent.MessageText, _localDbService, resendMessage);
            ///////
           // _scheduledEvent.SendMessageEvent?.Invoke(_scheduledEvent.NameEvent, _scheduledEvent.MessageText, _localDbService, resendMessage);
            _localDbService.Update(_scheduledEvent);
        }

        private void SendMessage( bool resendMessage)
        {
            if (!InternetCS.IsConnectedToInternet())
            {
                Application.Current.MainPage.DisplayAlert("Предупреждение", "Отсутствует доступ к интернету!", "ОK");
                return;
            }
            ///////
            ///
            foreach (var item in Guests)
                item.SendingMessages(_scheduledEvent.NameEvent, _scheduledEvent.MessageText, _localDbService, resendMessage);
            //_scheduledEvent.SendMessageEvent?.Invoke(_scheduledEvent.NameEvent, _scheduledEvent.MessageText, _localDbService, resendMessage);
            _localDbService.Update(_scheduledEvent);
        }

        public RelayCommand SendCommand => new(async () =>
        {
            SendMessage(false);
        });

        public  RelayCommand ParseCommand => new(async () =>
        {
            FilePickerFileType? customFileType =
            new(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { ".xlsx" } }
            });

            var result = await FilePicker.PickAsync(new PickOptions 
            { 
                FileTypes = customFileType
            });

            if (result is null)
                return;

            var stream = await result.OpenReadAsync();
            

            await ShowPopup(stream);

     
        });

        public async Task ShowPopup(Stream stream)
        {
            var pars = new XlsxParser();
            var newGuest = pars.Pars(stream);
            await popupService.ShowPopupAsync<GuestListFromDocumentViewModel>(onPresenting: viewModel => viewModel.GuestList(_scheduledEvent, newGuest));
            
        }


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
            CountGuest--;
            _localDbService.Update(_scheduledEvent);
        });

        private Guest CreationGuest()
        {
            IsVisibleAddGuest = false;
            var guest = new Guest().SetSurname(Surname).SetPatronymic(Patronymic).SetName(Name).SetMail(Mail);
            _scheduledEvent.SendMessageEvent += guest.GetUpSubscriptionForSendingMessages();
            _localDbService.Create(guest);
            CountGuest++;
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
            foreach (var item in guests)
            {
                CountSendMessage += item.IsMessageSent ? 1 : 0;
                scheduled.SendMessageEvent += item.GetUpSubscriptionForSendingMessages();
            }
           CountGuest = guests.Count;
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
