using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScannerAndDistributionOfQRCodes.Data.Message;
using ScannerAndDistributionOfQRCodes.Data.Message.Mail;
using ScannerAndDistributionOfQRCodes.Data.Parser;
using ScannerAndDistributionOfQRCodes.Data.Parser.Interface;
using ScannerAndDistributionOfQRCodes.Model;
using ScannerAndDistributionOfQRCodes.Navigation;
using ScannerAndDistributionOfQRCodes.ViewModel.Base;


namespace ScannerAndDistributionOfQRCodes.ViewModel.NewsletterViewModel
{
    public sealed partial class MessageBroadcastDisplayViewModel : ViewModelBase
    {
        private string _subject;
        private MailAccount _mailAccount;
        private string _imageFile;
        private MessageText _textMessage;
        private List<Dictionary<string, string>> _listxlsxParser;
        [ObservableProperty]
        private List<Mail> _listMail = [];
        [ObservableProperty]
        private string _columnNumber;

        [ObservableProperty]
        private bool _isReadyToShip;

        [ObservableProperty]
        private int _countOfMils;

        [ObservableProperty]
        private int _countOfCorrectMils;

        [ObservableProperty]
        private int _countSendMessages;

        [ObservableProperty]
        private int _countUnsendMessages;

        [ObservableProperty]
        private bool _isSendMessages;

        [ObservableProperty]
        private bool _isErrorMessages;
        [ObservableProperty]
        private bool _isMessagesDoNotSend = true;
        [ObservableProperty]
        private List<ErrorMessage<Mail>> _errorMessages = [];
        private IPopupService _popupService;
        private readonly INavigationService _navigationService;

        public MessageBroadcastDisplayViewModel(INavigationService navigationService, IPopupService popupService)
        {
            _navigationService = navigationService;
            _popupService = popupService;
        }

        public RelayCommand<Popup> CancelCommand => new((popup) =>
        {
            if (IsSendMessages)
                _navigationService?.NavigateBackAsync();
            popup?.Close();
        });

        public RelayCommand TextChangedCommand => new(() =>
        {
            IsReadyToShip = false;
        });

        public RelayCommand SendCommand => new(async () =>
        {
            await Task.Run(() => 
            {
                IsReadyToShip = false; 
                IsSendMessages = true;
                IsMessagesDoNotSend = false;
            }).ConfigureAwait(false);
            await SendAsync().ConfigureAwait(true);
        });

        private async Task SendAsync()
        {       
            ErrorMessages.Clear();
            foreach (var item in ListMail.Where(x=>x.IsValidMail))
            {
                try
                {
                    using var stream = string.IsNullOrEmpty(_imageFile) ? null : new FileStream(_imageFile, FileMode.Open);

                    await Task.Run(() =>
                    {
                        item.SendingMessages(_subject, _textMessage,
                        _mailAccount,stream: stream);
                    }).ConfigureAwait(false);

                    CountSendMessages += item.IsMessageSent ? 1:0;
                }
                catch (Exception ex) 
                {

                    ErrorMessages.Add(new ErrorMessage<Mail>(item, ex.Message));
                    CountUnsendMessages++;
                }

            }
            if (ErrorMessages.Count != 0)
                IsErrorMessages = true;
            IsMessagesDoNotSend = true;
        }
        public RelayCommand<ErrorMessage<Mail>> ViewErrorMessageCommand => new(async (x) =>
        {
            await Application.Current.MainPage.DisplayAlert("Сообщение ошибки", $"{x.Message}", "Ок").ConfigureAwait(false);
        });
        public RelayCommand ParserCommand => new(async()=>
        {
            FilePickerFileType customFileType =
            new(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                    { DevicePlatform.WinUI, new[] { ".xlsx" } }
            });

            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = customFileType
            }).ConfigureAwait(true);
            await ListOfParsedAsync(result, new XlsxParser()).ConfigureAwait(false);
        });

        public async Task ListOfParsedAsync(FileResult result,IParser xlsxParser)
        {
            IsSendMessages = IsErrorMessages = IsReadyToShip = false;
            if (result is null)
                return;

            var stream = await result.OpenReadAsync().ConfigureAwait(true);
            _listxlsxParser = xlsxParser.Pars(stream);

            if (CheckingListFilling(_listxlsxParser))
            {
                DisplayAlertErrorAsync($"Файл {result.FileName} не содержит необходимых или корректных данных");
                return;
            }
            IsReadyToShip = true;
        }

 
        private bool CheckingListFilling(List<Dictionary<string, string>> listxlsxParser)
        {
            ListMail.Clear();
            foreach (var item in listxlsxParser)
            {
                if (item.TryGetValue(ColumnNumber, out string value))
                {
                    var mail = new Mail(value);
                    ListMail.Add(mail);
                }
            }

            CountOfCorrectMils = ListMail.Count(x => x.IsValidMail);
            CountOfMils = ListMail.Count;
            return CountOfCorrectMils == 0;
        }

        private async Task DisplayAlertErrorAsync(string errorMessage) 
            => await Application.Current.MainPage.DisplayAlert("Предупреждение", errorMessage, "Ок").ConfigureAwait(false);
        public void MessageBroadcast(MailAccount mailAccount, string imageFile, MessageText textMessage,string subject)
        {
            _subject = string.IsNullOrEmpty(subject) ? string.Empty : subject;
            _mailAccount = mailAccount;
            _imageFile = imageFile;
            _textMessage = textMessage;
        }
    }
}
