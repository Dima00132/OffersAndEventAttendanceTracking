using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScannerAndDistributionOfQRCodes.Data.Message;
using ScannerAndDistributionOfQRCodes.Model;
using ScannerAndDistributionOfQRCodes.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerAndDistributionOfQRCodes.ViewModel
{
    public partial class DisplayAlertSendingMessagesErrorViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<ErrorMessage<Guest>> _guests = [];

        [ObservableProperty]
        private int _countGuest;

        public RelayCommand<ErrorMessage<Guest>> ViewErrorMessageCommand => new(async (x) =>
        {
            await Application.Current.MainPage.DisplayAlert("Сообщение ошибки", $"{x.Message}", "Ок");
        });

        public RelayCommand<Popup> CancelCommand => new(async (popup) =>
        {
            popup.Close();
        });

        public void ListOfErrorMessage(List<ErrorMessage<Guest>> errorMessages)
        {
            Guests = new ObservableCollection<ErrorMessage<Guest>>(errorMessages);
            CountGuest = errorMessages.Count;
        }
    }
}
