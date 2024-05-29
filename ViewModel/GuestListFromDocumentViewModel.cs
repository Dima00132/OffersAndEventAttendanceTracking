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

namespace ScannerAndDistributionOfQRCodes.ViewModel
{
    public partial class GuestListFromDocumentViewModel : ViewModelBase
    {
        private readonly ILocalDbService _localDbService;
        [ObservableProperty]
        private ObservableCollection<Guest> _guests;

        [ObservableProperty]
        private int _countGuest;

        [ObservableProperty]
        private bool _isRunning = true;

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
        public void GuestList(ScheduledEvent scheduledEvent, IParser parser)
        {
            //Guests = new ObservableCollection<Guest>(guests);
            //CountGuest = Guests.Count;
            //_scheduledEvent = scheduledEvent;
        }

        private void ChecklistGuest(List<Guest> guests)
        {
          
            foreach (var item in guests)
            {
                ////
                var isGuestOnList = _scheduledEvent.Guests.Where(x => x.VrificatQRCode.QRHashCode.Equals(item.VrificatQRCode.QRHashCode)).Count() != 0 ? true : false;
                if (isGuestOnList)
                {
                    Guests.Add(item);
                    CountGuest++;
                }


            }
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
                await Application.Current.MainPage.DisplayAlert("Ошибка чтения", $"Произлшла ошибка при чтение файла!\\n {ex}", "Ok");
                return;
            }
            
            //Thread.Sleep(10000);
            
            //IsRunning =false;
            ChecklistGuest(listGuest);
            //Guests = new ObservableCollection<Guest>(listGuest);
           // CountGuest = Guests.Count;
            
        }
    }
}
