using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScannerAndDistributionOfQRCodes.Data.Message;
using ScannerAndDistributionOfQRCodes.Model;
using ScannerAndDistributionOfQRCodes.Navigation;
using ScannerAndDistributionOfQRCodes.Service.Interface;
using ScannerAndDistributionOfQRCodes.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerAndDistributionOfQRCodes.ViewModel.NewsletterViewModel
{
    public partial class MailingViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ILocalDbService _localDbService;
        private readonly IPopupService _popupService;
        private readonly MailAccount _mailAccount;

        [ObservableProperty]
        private string _textMessage;
        [ObservableProperty]
        private string _organizationData;

        [ObservableProperty]
        private string _imageFile;

        private Stream _streamImage;

  
        public MailingViewModel(INavigationService navigationService, ILocalDbService localDbService, IPopupService popupService)
        {
            _navigationService = navigationService;
            _localDbService = localDbService;
            _popupService = popupService;
            //_mailAccount = localDbService.GetMailAccount();

            //var mailAccount = new MailAccount();
            //mailAccount.Create("6686967", "testsend@nizhny.online", "6a8dtydwniakm3fgmy1zrn1q93yd1o176k39b96y",
            //new User("Иванов", "Иван", "Иванович"),
            //new MailServer("smtp.go1.unisender.ru", 465, true));
            //localDbService.Delete(_mailAccount);

            //localDbService.Create(mailAccount.MailServer);
            //localDbService.Create(mailAccount.UserData);
            //localDbService.Create(mailAccount);


            _mailAccount = localDbService.GetMailAccount();
        }

        public RelayCommand AddImageCommand => new(async() =>
        {
            FilePickerFileType? customFileType =
            new(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                    { DevicePlatform.WinUI, new[] { ".png" } }
            });

            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = customFileType
            }).ConfigureAwait(true);
            ImageFile = result.FullPath;
            _streamImage = await result.OpenReadAsync().ConfigureAwait(true);
        });

        public RelayCommand SendCommand => new(async () =>
        {
            var textMessage = new MessageText(TextMessage, _organizationData);
            await _popupService.ShowPopupAsync<MessageBroadcastDisplayViewModel>(onPresenting: viewModel => viewModel.MessageBroadcast(_mailAccount,_streamImage, textMessage)).ConfigureAwait(false);
        });

    }
}
