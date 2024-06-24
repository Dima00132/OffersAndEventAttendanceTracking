
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Maui.Animations;
using Org.BouncyCastle.Tsp;
using ScannerAndDistributionOfQRCodes.Data.Message;
using ScannerAndDistributionOfQRCodes.Data.Parser;
using ScannerAndDistributionOfQRCodes.Model;
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
    public partial class GuestListViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ILocalDbService _localDbService;
        private readonly IPopupService popupService;
        private readonly IMailAccount _mailAccount;
        private bool isStart = true;

        [ObservableProperty]
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
        private int _countSendMessage;

        private int _сountNotValidMail;
        public int CountNotValidMail
        {
            get => _сountNotValidMail;
            set => SetProperty(ref _сountNotValidMail, value);

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
            await DisplayAlertSendMessageProgressAsync(gueet).ConfigureAwait(false);
        });
        public RelayCommand SendCommand => new(async () =>
        {
            await DisplayAlertSendMessageProgressAsync([.. Guests]).ConfigureAwait(false);
        });
        public RelayCommand ParseCommand => new(async () =>
        {
            await popupService.ShowPopupAsync<GuestListFromDocumentViewModel>(onPresenting: viewModel => viewModel.ListOfParsedGuests(_scheduledEvent, new XlsxParser())).ConfigureAwait(false);
            UpdateCountProperty();
            _localDbService.Update(_scheduledEvent);
        });
        public RelayCommand UpdateCommand => new(() =>
        {
            if (ScheduledEvent is null)
                return;
            Guests = null;
            Guests =  ScheduledEvent.Guests;
        });   
        public RelayCommand<Guest> DeleteCommand => new(async (guest) =>
        {
            Guests.Remove(guest);
            _localDbService.Update(_scheduledEvent);
        });

        public GuestListViewModel(INavigationService navigationService, ILocalDbService localDbService, IPopupService popupService)
        {
            _navigationService = navigationService;
            _localDbService = localDbService;
            this.popupService = popupService;
            _mailAccount = localDbService.GetMailAccount();
        }

        [RelayCommand(CanExecute = nameof(CheckNameEvent))]
        public async Task Save()
        {
            if (IsVisibleAddGuest)
                Guests.Add(CreationGuest());
            if (IsVisibleChangeGuest)
                ChangeGuest(Guest);
            IsEditor = false;
            ClearValues();
            UpdateCountProperty();
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
        private Guest CreationGuest()
        {
            IsVisibleAddGuest = false;
            var guest = new Guest().SetSurname(Surname).SetPatronymic(Patronymic).SetName(Name).SetMail(Mail);
            _localDbService.Create(guest);
            return guest;
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
                var data = await Task.FromResult(_scheduledEvent.FindsQuestionByRequest(query)).ConfigureAwait(false);
                if (data is not null)
                    Guests = new ObservableCollection<Guest>(data);
            }
        }
        private async Task DisplayAlertSendMessageProgressAsync(params Guest[] gueets)
        {
            if (!InternetCS.IsConnectedToInternet())
            {
                await Application.Current.MainPage.DisplayAlert("Предупреждение", "Отсутствует доступ к интернету!", "ОK").ConfigureAwait(false);
                return;
            }
            await popupService
                .ShowPopupAsync<DisplayAlertSendMessageProgressViewModel>(onPresenting: viewModel => viewModel.ProgresslListSendMessages(gueets, _scheduledEvent, _mailAccount, _localDbService))
                .ConfigureAwait(false);
            UpdateCountProperty();
        }
        private int GetNotValidMail() => Guests.Count((x) => !x.Mail.IsValidMail);
        private int GetCountSendMessage()
            => Guests.Count((x) => x.Mail.IsMessageSent);
        private void ChangeGuest(Guest guest)
        {
            guest.SetSurname(Surname).SetPatronymic(Patronymic).SetName(Name).SetMail(Mail);
            IsVisibleChangeGuest = false;
            _localDbService.Update(guest.Mail);
            _localDbService.Update(guest.VrificatQRCode);
            _localDbService.Update(guest);
        }
        private void InstallationValues(Guest guest)
        {
            Surname = guest.User.Surname.ToString();
            Name = guest.User.Name.ToString();
            Patronymic = guest.User.Patronymic.ToString();
            Mail = guest.Mail.MailAddress.ToString();
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
                & !string.IsNullOrEmpty(Patronymic) & EmailValidator.CheckingEmailFormat(Mail);
            // & EmailValidator.CheckEmailDomain(Mail)
        }
        private void UpdateCountProperty()
        {
            CountSendMessage = GetCountSendMessage();
            CountNotValidMail = GetNotValidMail();
        }
        public override Task OnNavigatingTo(object? parameter, object? parameterSecond = null)
        {
            if (parameter is ScheduledEvent scheduledEvent)
            {
                ScheduledEvent = scheduledEvent;
                Guests = scheduledEvent.Guests;
                UpdateCountProperty();
                isStart = false;
            }
            return base.OnNavigatingTo(parameter);
        }
    }
}
