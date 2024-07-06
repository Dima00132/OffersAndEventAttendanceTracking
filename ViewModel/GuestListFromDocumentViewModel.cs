using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using ScannerAndDistributionOfQRCodes.Model;
using ScannerAndDistributionOfQRCodes.Service.Interface;
using ScannerAndDistributionOfQRCodes.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScannerAndDistributionOfQRCodes.Service.PopupService.Interface;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using System.ComponentModel;
using System.Security.Policy;
using System.Diagnostics.CodeAnalysis;
using Microsoft.IdentityModel.Tokens;
using ScannerAndDistributionOfQRCodes.Data.Parser.Interface;

namespace ScannerAndDistributionOfQRCodes.ViewModel
{
    public sealed class CustomGuestMailComparer : IEqualityComparer<Guest>
    {
        public bool Equals(string x, string y)
        {
            if (x is null || y is null) return false;
            return x.ToLower() == y.ToLower();

        }

        public bool Equals(Guest x, Guest y)
        {
            if (x is null || y is null) return false;
            return x.Mail.MailAddress == y.Mail.MailAddress;
        }

        public int GetHashCode([DisallowNull] Guest obj)
        {
            return obj.Mail.GetHashCode();
        }
    }


    public sealed partial class GuestListFromDocumentViewModel : ViewModelBase
    {
        private readonly ILocalDbService _localDbService;
        [ObservableProperty]
        private ObservableCollection<Guest> _guests = [];

        [ObservableProperty]
        private int _countGuest;

        [ObservableProperty]
        private bool _isRunning = true;

        [ObservableProperty]
        private bool _isError = false;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
        private string _surname;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
        private string _name;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
        private string _patronymic;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
        private string _mail;
        [ObservableProperty]
        private bool _isSkipFirstLine = true;

        private ScheduledEvent _scheduledEvent;

        private List<Dictionary<string, string>> _listxlsxParser = [];
        public GuestListFromDocumentViewModel(ILocalDbService localDbService)
        {
            _localDbService = localDbService;
        }


        [RelayCommand(CanExecute = nameof(CheckName))]
        public void Confirm()
        {
            var guests = GetGuests(_listxlsxParser);
            if (RepetitionCheck(guests))
                DisplayAlertError("Количество уникальных гостей 0");
        }

        private bool CheckName() => !string.IsNullOrEmpty(Name) & !string.IsNullOrEmpty(Surname) & !string.IsNullOrEmpty(Patronymic) & !string.IsNullOrEmpty(Mail);

        private List<Guest> GetGuests(List<Dictionary<string, string>> listxlsxParser)
        {
            
             var guests = new List<Guest>();
            var newLstxlsxParser = IsSkipFirstLine ? listxlsxParser.Skip(1) : listxlsxParser;
            foreach (var item in newLstxlsxParser)
            {
                if (!CheckingPresenceOfColumn(item, [Mail, Name, Surname, Patronymic]))
                    continue;
                var guest = new Guest().SetName(item[Name]).SetSurname(item[Surname])
                    .SetPatronymic(item[Patronymic]).SetMail(item[Mail]);
                guests.Add(guest);
            }
            return guests;
        }

        private bool CheckingPresenceOfColumn(Dictionary<string, string>  rowXlsx,params string[] ColumnName)
        {
            for (int i = 0; i < ColumnName.Length; i++)
                if (!rowXlsx.ContainsKey(ColumnName[i]))
                    return false;
            return true;
        }

        public RelayCommand<Popup> SaveCommand => new((popup) =>
        {
            foreach (var item in Guests)
            {
                var isGuestOnList = _scheduledEvent.Guests.Count(x => x.VrificatQRCode.QRHashCode.Equals(item.VrificatQRCode.QRHashCode)) != 0 ? true : false;
                if (isGuestOnList)
                    continue;
                _scheduledEvent.Guests.Add(item);
                _localDbService.Create(item);
            }
            _localDbService.Update(_scheduledEvent);
            popup.Close();
        });


        public RelayCommand<Popup> CancelCommand => new((popup) =>
        {
            popup.Close();
        });


        private bool CheckingListFilling(List<Dictionary<string, string>> listxlsxParser)
        {
            return listxlsxParser.Count == 0;
        }

        private void DisplayAlertError(string errorMessage)
        {
            IsError = true;
            Application.Current.MainPage.DisplayAlert("Предупреждение", errorMessage, "Ок");
        }

   
        private  bool RepetitionCheck(List<Guest> guests)
        {
            foreach (var item in guests)
            {
                var isInMainList = !_scheduledEvent.Guests.Contains(item, new CustomGuestMailComparer());
                var isInCurrentList = !Guests.Contains(item,new  CustomGuestMailComparer());
                if (isInMainList & isInCurrentList)
                {
                    Guests.Add(item);
                    CountGuest++;
                }
            }
            return Guests.Count == 0;
        }

        private async Task< FileResult> GetFileAsync()
        {
            FilePickerFileType customFileType =
                new(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".xlsx" } }
                });

            var result =  await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = customFileType
            }).ConfigureAwait(true);
            return result;
        }

        public async Task ListOfParsedGuestsAsync(ScheduledEvent scheduledEvent, IParser xlsxParser)
        {
            var result = await GetFileAsync();
            if(result is null)
            {
                IsError = true;
                return;
            }
            _scheduledEvent = scheduledEvent;
            var stream = await result.OpenReadAsync().ConfigureAwait(true);
            _listxlsxParser = xlsxParser.Pars(stream);

            //try
            //{
            //    var stream = await result.OpenReadAsync().ConfigureAwait(true);
            //    _listxlsxParser = xlsxParser.Pars(stream);              
            //}
            //catch(Exception  ex)
            //{
            //    DisplayAlertError($"Произлшла ошибка при чтение файла!\n {ex.Message} \n Повторите попытку заново!");
            //    return;
            //}
            if (CheckingListFilling(_listxlsxParser)) 
            {
                DisplayAlertError($"Файл {result.FileName} не содержит необходимых данных");
                return;
            }

            
        }
    }
}
