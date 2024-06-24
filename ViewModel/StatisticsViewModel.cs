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

namespace ScannerAndDistributionOfQRCodes.ViewModel
{
    public partial class  StatisticsViewModel:ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ILocalDbService _localDbService;
        [ObservableProperty]
        private ScheduledEvent _scheduledEvent;
        [ObservableProperty]
        public List<string> _guests;
        [ObservableProperty]
        public bool _isEventWasHeld;
        [ObservableProperty]
        public int _countArrivedGuests;




        public StatisticsViewModel(INavigationService navigationService, ILocalDbService localDbService)
        {
            _navigationService = navigationService;
            _localDbService = localDbService;
        }

        private List<string[]> GetGuestsStatistics()
        {
            var guestList = new List<string[]>();
            foreach (var item in Guests)
                guestList.Add(item.Split('|'));
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
                    new RecordXlsx().Record(GetGuestsStatistics(), fileSaveResult.FilePath);
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



        public override Task OnNavigatingTo(object? parameter, object? parameterSecond = null)
        {
            if (parameter is ScheduledEvent scheduledEvent)
            {
                ScheduledEvent = scheduledEvent;
                Guests = scheduledEvent.Guests.Select(x=> x.GetStatisticsString()).ToList();
                IsEventWasHeld = scheduledEvent.IsEventWasHeld;
                CountArrivedGuests = scheduledEvent.Guests.Count(x=>x.VrificatQRCode.IsVerifiedQRCode);
            }
            return base.OnNavigatingTo(parameter, parameterSecond);
        }
    }
}
