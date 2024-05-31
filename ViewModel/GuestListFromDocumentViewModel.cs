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
using ScannerAndDistributionOfQRCodes.Data.Parser;
using ScannerAndDistributionOfQRCodes.Service.PopupService.Interface;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using System.ComponentModel;
using System.Security.Policy;
using System.Diagnostics.CodeAnalysis;

namespace ScannerAndDistributionOfQRCodes.ViewModel
{
    public sealed class CustomGuestMailComparer : IEqualityComparer<Guest>
    {
        public bool Equals(string? x, string? y)
        {
            if (x is null || y is null) return false;
            return x.ToLower() == y.ToLower();

        }

        public bool Equals(Guest? x, Guest? y)
        {
            if (x is null || y is null) return false;
            return x.Mail.MailAddress == y.Mail.MailAddress;
        }

        public int GetHashCode([DisallowNull] Guest obj)
        {
            return obj.Mail.GetHashCode();
        }
    }


    public partial class GuestListFromDocumentViewModel : ViewModelBase
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

        private ScheduledEvent _scheduledEvent;
        public GuestListFromDocumentViewModel(ILocalDbService localDbService)
        {
            _localDbService = localDbService;
        }

        public RelayCommand<Popup> SaveCommand => new(async (popup) =>
        {
            ///

            foreach (var item in Guests)
            {
                ////
                var isGuestOnList = _scheduledEvent.Guests.Where(x => x.VrificatQRCode.QRHashCode.Equals(item.VrificatQRCode.QRHashCode)).Count() != 0?true:false;
                if (isGuestOnList)
                    continue;
                _scheduledEvent.Guests.Add(item);
                _localDbService.Create(item);
            }

           // var value = Application.Current.MainPage.DisplayAlert("Уведомление", "Отправить преглошение?","Да", "Нет");
            

            _localDbService.Update(_scheduledEvent);
            popup.Close();
        });

        //[RelayCommand]
        //public async Task Close(Popup popup)
        //{
        //    popup.Close();
        //    await DisplayToast("Closed using button");
        //}

        public RelayCommand<Popup> CancelCommand => new(async (popup) =>
        {
            popup.Close();
        });


        private bool CheckingListFilling(List<Guest> guests)
        {
            return guests.Count == 0;
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
                //var isInMainList = _scheduledEvent.SearchForGuestByQRHashCode(item.VrificatQRCode.QRHashCode) is null?true:false;
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

        internal async void ListOfParsedGuests(ScheduledEvent scheduledEvent, FileResult result, IParser xlsxParser)
        {
            List<Guest> listGuest = new();
            _scheduledEvent = scheduledEvent;
            try
            {
                var stream = await result.OpenReadAsync();
                listGuest = xlsxParser.Pars(stream);              
            }
            catch(Exception  ex)
            {
                DisplayAlertError($"Произлшла ошибка при чтение файла!\n {ex.Message}");
                return;
            }


         
            if (CheckingListFilling(listGuest)) 
            {
                DisplayAlertError($"Файл {result.FileName} не содержит необходимых данных");
                return;
            }

            if (RepetitionCheck(listGuest))
                DisplayAlertError("Количество уникальных гостей 0");
        }
    }
}
