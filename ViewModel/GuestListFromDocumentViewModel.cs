using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScannerAndDistributionOfQRCodes.Model;
using ScannerAndDistributionOfQRCodes.Service.Interface;
using ScannerAndDistributionOfQRCodes.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerAndDistributionOfQRCodes.ViewModel
{
    public partial class GuestListFromDocumentViewModel : ViewModelBase
    {
        private readonly ILocalDbService _localDbService;
        [ObservableProperty]
        private ObservableCollection<Guest> _guests;

        [ObservableProperty]
        private int _countGuest;

        private ScheduledEvent _scheduledEvent;
        public GuestListFromDocumentViewModel(ILocalDbService localDbService)
        {
            _localDbService = localDbService;
        }

        public RelayCommand<Popup> SaveCommand => new(async (popup) =>
        {
            foreach (var item in Guests)
            {
                _scheduledEvent.SendMessageEvent += item.GetUpSubscriptionForSendingMessages();
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
        public void GuestList(ScheduledEvent scheduledEvent, List<Guest> guests)
        {
            Guests = new ObservableCollection<Guest>(guests);
            CountGuest = Guests.Count;
            _scheduledEvent = scheduledEvent;
        }
    }
}
