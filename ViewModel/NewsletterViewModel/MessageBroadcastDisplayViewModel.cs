﻿using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Utilities.IO;
using ScannerAndDistributionOfQRCodes.Data.Message;
using ScannerAndDistributionOfQRCodes.Data.Message.Mail;
using ScannerAndDistributionOfQRCodes.Data.Parser;
using ScannerAndDistributionOfQRCodes.Data.Parser.Interface;
using ScannerAndDistributionOfQRCodes.Model;
using ScannerAndDistributionOfQRCodes.Navigation;
using ScannerAndDistributionOfQRCodes.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerAndDistributionOfQRCodes.ViewModel.NewsletterViewModel
{
    public partial class MessageBroadcastDisplayViewModel : ViewModelBase
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
        private List<string> _errorMessages= [];
        private readonly INavigationService _navigationService;

        public MessageBroadcastDisplayViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public RelayCommand<Popup> CancelCommand => new(async (popup) =>
        {
            if (IsSendMessages)
                _navigationService?.NavigateBack();
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
                    ErrorMessages.Add(ex.Message);
                    CountUnsendMessages++;
                }

            }
            if (ErrorMessages.Count != 0)
                IsErrorMessages = true;
            IsMessagesDoNotSend = true;
        }


        public RelayCommand ParserCommand => new(async()=>
        {
            FilePickerFileType? customFileType =
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
                if (item.TryGetValue(ColumnNumber, out string? value))
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
