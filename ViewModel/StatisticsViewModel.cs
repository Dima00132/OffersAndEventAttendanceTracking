using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Packaging;
using ScannerAndDistributionOfQRCodes.Model;
using ScannerAndDistributionOfQRCodes.Navigation;
using ScannerAndDistributionOfQRCodes.Service.Interface;
using ScannerAndDistributionOfQRCodes.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using System.Threading;
using ScannerAndDistributionOfQRCodes.Data.Record_File;
using Bytescout.Spreadsheet.Charts;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace ScannerAndDistributionOfQRCodes.ViewModel
{


    public partial class  StatisticsViewModel:ViewModelBase
    {
        [ObservableProperty]
        private ScheduledEvent _scheduledEvent;
        [ObservableProperty]
        public List<StatisticsGuest> _guests = [];
        [ObservableProperty]
        public bool _isEventWasHeld;
        [ObservableProperty]
        public int _countArrivedGuests;

        public string[] _title = ["Фамилия", "Имя", "Отчество", "Отправлено", "Явка", "Время прибытия"];

        private List<string[]> GetStatisticsOnGuests()
        {
            var guestList = new List<string[]>() { _title };
            foreach (var item in ScheduledEvent.Guests)
            {
                var statisics = item.GetStatisticsGuest();
                string[] guestString = [statisics.Surname, statisics.Name, statisics.Patronymic,
                    statisics.IsMessageSent, statisics.IsVerifiedQRCode, statisics.ArrivalTime];
                guestList.Add(guestString);
            }
            return guestList;
        }

        public RelayCommand SealCommand => new(async () => 
        {
            using var stream = new MemoryStream();
            var fileSaveResult = await FileSaver.Default.SaveAsync("NameFile.xlsx", stream);

            if (fileSaveResult.IsSuccessful)
            {
                try
                {
                    new RecordXlsx().Record(GetStatisticsOnGuests(), fileSaveResult.FilePath);
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", ex.Message, "Ок").ConfigureAwait(false);
                }
                await Application.Current.MainPage.DisplayAlert("Файл сохранен!", $"{fileSaveResult.FilePath}", "Ок").ConfigureAwait(false);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", fileSaveResult.Exception.Message, "Ок").ConfigureAwait(false);
            }
        });

        public override Task OnNavigatingToAsync(object? parameter, object? parameterSecond = null)
        {
            if (parameter is ScheduledEvent scheduledEvent)
            {
                ScheduledEvent = scheduledEvent;
                foreach (var item in scheduledEvent.Guests)
                    Guests.Add(item.GetStatisticsGuest());
                IsEventWasHeld = scheduledEvent.IsEventWasHeld;
                CountArrivedGuests = scheduledEvent.Guests.Count(x=>x.VrificatQRCode.IsVerifiedQRCode);
            }
            return base.OnNavigatingToAsync(parameter, parameterSecond);
        }
    }
}
