using CommunityToolkit.Mvvm.ComponentModel;
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

namespace ScannerAndDistributionOfQRCodes.ViewModel
{
    public partial class SettingsViewModel:ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ILocalDbService _localDbService;
        private readonly IMailAccount _mailAccount;

        [ObservableProperty]
        //[NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string _surname = string.Empty;

        [ObservableProperty]
        //[NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string _name = string.Empty;

        [ObservableProperty]
        //[NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string _patronymic= string.Empty;

        [ObservableProperty]
       // [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string _mail = string.Empty;

        [ObservableProperty]
        public string _server;

        [ObservableProperty]
        public int _port;

        [ObservableProperty]
        public bool _connectionProtection;

        [ObservableProperty]
        public string _mailAddress;

        [ObservableProperty]
        public string _mailID;
        [ObservableProperty]
        public string _password;


 
        public SettingsViewModel(INavigationService navigationService, ILocalDbService localDbService)
        {
            _navigationService = navigationService;
            _localDbService = localDbService;
            // mailAccount = new MailAccount();
            //mailAccount.Create("6686967", "testsend@nizhny.online", "6a8dtydwniakm3fgmy1zrn1q93yd1o176k39b96y",
            //new User("Иванов", "Иван", "Иванович"),
            //new MailServer("smtp.go1.unisender.ru", 465, true));

            //localDbService.Create(mailAccount.MailServer);
            //localDbService.Create(mailAccount.UserData);
            //localDbService.Create(mailAccount);

            
            _mailAccount = localDbService.GetMailAccount();

            Name = _mailAccount.UserData.Name;
            Surname = _mailAccount.UserData.Surname;
            Patronymic = _mailAccount.UserData.Patronymic;

            Server = _mailAccount.MailServer.Server;
            Port = _mailAccount.MailServer.Port;
            ConnectionProtection = _mailAccount.MailServer.ConnectionProtection;

            MailAddress = _mailAccount.MailAddress;

            MailID = _mailAccount.MailID;
            Password = _mailAccount.Password;
        }

    }
}
