using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.Maui.Animations;
using ScannerAndDistributionOfQRCodes.Model;
using ScannerAndDistributionOfQRCodes.Navigation;
using ScannerAndDistributionOfQRCodes.Service.Interface;
using ScannerAndDistributionOfQRCodes.View;
using ScannerAndDistributionOfQRCodes.View.NewsletterView;
using ScannerAndDistributionOfQRCodes.ViewModel.Base;

namespace ScannerAndDistributionOfQRCodes.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ILocalDbService _localDbService;

        public MainViewModel(INavigationService navigationService, ILocalDbService localDbService)
        {
            _navigationService = navigationService;
            _localDbService = localDbService;
        }

        public RelayCommand SettingsCommand => new(async () =>
        {
            await _navigationService.NavigateByPageAsync<SettingsPage>();
        });
        public RelayCommand EventCommand => new(async () =>
        {
            await _navigationService.NavigateByPageAsync<ListOfEventsPage>();
        });

        public RelayCommand SendingMessagesCommand => new (async () =>
        {
            await _navigationService.NavigateByPageAsync<MailingPage>();
        });
    }
}
