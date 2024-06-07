using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScannerAndDistributionOfQRCodes.Data.Message;
using ScannerAndDistributionOfQRCodes.Model;
using ScannerAndDistributionOfQRCodes.Service.Interface;
using ScannerAndDistributionOfQRCodes.ViewModel.Base;
using System.Collections.ObjectModel;

namespace ScannerAndDistributionOfQRCodes.ViewModel
{
    public partial class DisplayAlertSendMessageProgressViewModel : ViewModelBase
    {
        private readonly IPopupService _popupService;
        private ILocalDbService _localDbService;
        private ScheduledEvent _scheduledEvent;
        private IMailAccount _mailAccount;
        private List<Guest> _gueets;

        [ObservableProperty]
        private int _progressSend;

        [ObservableProperty]
        private int _countMessages;

        [ObservableProperty]
        private int _progressErrorSend;

        [ObservableProperty]
        private bool _isEnabledSend = true;


        public DisplayAlertSendMessageProgressViewModel(IPopupService popupService)
        {
            _popupService = popupService;
        }

        public RelayCommand<Popup> CancelCommand => new(async (popup) =>
        {
            popup.Close();
        });

        public void ListOfErrorMessage(Func<Guest, bool> funcWhere, Guest[] gueets, ScheduledEvent scheduledEvent, IMailAccount mailAccount, ILocalDbService localDbService)
        {
            _localDbService = localDbService;
            _scheduledEvent = scheduledEvent;
            _mailAccount = mailAccount;
            _gueets = gueets.Where(funcWhere is null ? (x) => true : funcWhere).ToList();
            CountMessages = _gueets.Count;
            IsEnabledSend = CountMessages != 0;   
        }


      
        public  RelayCommand SendCommand => new(async () =>
        {
            await SendMessageAsync(_scheduledEvent, _mailAccount, _gueets).ConfigureAwait(false);
        });

        private async Task SendMessageAsync(ScheduledEvent scheduledEvent, IMailAccount mailAccount,List<Guest> gueets)
        {
            if (!InternetCS.IsConnectedToInternet())
            {
                await Application.Current.MainPage
                    .DisplayAlert("Предупреждение", "Отсутствует доступ к интернету!", "ОK")
                    .ConfigureAwait(false);
                return;
            }

            
            List<ErrorMessage<Guest>> errorMessages = [];

            foreach (var item in gueets)
            {
                try
                {
                    item.Mail.SendingMessages(scheduledEvent.NameEvent, scheduledEvent.MessageText, 
                        mailAccount, item.User.ToString(), item.VrificatQRCode.GetStreamEncodeQRCode());
                    _localDbService.Update(item.Mail);
                    ProgressSend++;
                }
                catch (SendMailMessageException ex)
                {
                    ProgressErrorSend++;
                    errorMessages.Add(new ErrorMessage<Guest>(item, ex.Message));
                }
            }
            if (errorMessages.Count != 0)
                await DisplayAlertSendingMessagesErrorAsync(errorMessages).ConfigureAwait(false);
            IsEnabledSend = false;
            _localDbService.Update(scheduledEvent);
        }

        private async Task DisplayAlertSendingMessagesErrorAsync(List<ErrorMessage<Guest>> errorMessages)
        {
            await _popupService
                .ShowPopupAsync<DisplayAlertSendingMessagesErrorViewModel>(onPresenting: viewModel => viewModel.ListOfErrorMessage(errorMessages))
                .ConfigureAwait(false);
        }


        //public void ListOfErrorMessage(List<ErrorMessage<Guest>> errorMessages)
        //{
        //    Guests = new ObservableCollection<ErrorMessage<Guest>>(errorMessages);
        //    CountGuest = errorMessages.Count;
        //}
    }
}