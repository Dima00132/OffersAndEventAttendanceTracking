﻿using Aspose.Email;
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

namespace ScannerAndDistributionOfQRCodes.ViewModel
{
    public partial class SettingsViewModel:ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ILocalDbService _localDbService;
        private readonly MailAccount _mailAccount;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveUserCommand))]
        private string _surname = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveUserCommand))]
        private string _name = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveUserCommand))]
        private string _patronymic= string.Empty;

       // [ObservableProperty]
       //// [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
       // private string _mail = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveMailServerCommand))]
        public string _server;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveMailServerCommand))]
        public int _port;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveMailServerCommand))]
        public bool _connectionProtection;

     
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveDomainDailCommand))]
        public string _mailAddress;
        
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveUisenderGOCommand))]
        public string _mailID;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveUisenderGOCommand))]
        public string _password;


 
        public SettingsViewModel(INavigationService navigationService, ILocalDbService localDbService)
        {
            _navigationService = navigationService;
            _localDbService = localDbService;
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

        [RelayCommand(CanExecute = nameof(CheckUser))]
        public void SaveUser()
        {
            _mailAccount.UserData.Change(Surname, Name, Patronymic);
            _localDbService.Update(_mailAccount.UserData);
        }

        private bool CheckUser() 
        {
            return _mailAccount.UserData.Name.CompareTo(Name) 
               + _mailAccount.UserData.Patronymic.CompareTo(Patronymic) 
                +_mailAccount.UserData.Surname.CompareTo(Surname) != 0;
        }


        [RelayCommand(CanExecute = nameof(CheckMailServer))]
        public void SaveMailServer()
        {
            _mailAccount.MailServer.Change(Server, Port, ConnectionProtection);
            _localDbService.Update(_mailAccount.MailServer);
        }

        private bool CheckMailServer() => _mailAccount.MailServer.Server.CompareTo(Server)
                + _mailAccount.MailServer.Port.CompareTo(Port)
                + _mailAccount.MailServer.Server.CompareTo(Server) + _mailAccount.MailServer.Port.CompareTo(Port) != 0;



        [RelayCommand(CanExecute = nameof(CheckDomainDail))]
        public void SaveDomainDail()
        {
            _mailAccount.Change(MailID, MailAddress, Password);
            _localDbService.Update(_mailAccount);
        }

        private bool CheckDomainDail()
        {
           return _mailAccount.MailServer.Server.CompareTo(Server) + _mailAccount.MailServer.Port.CompareTo(Port) != 0;
        }

        [RelayCommand(CanExecute = nameof(CheckUisenderGO))]
        public void SaveUisenderGO()
        {
            _mailAccount.Change(MailID, MailAddress, Password);
            _localDbService.Update(_mailAccount);
        }

        private bool CheckUisenderGO()
        {
            return _mailAccount.MailID.CompareTo(MailID) + _mailAccount.Password.CompareTo(Password) != 0;
        }

        private void UpdateMailAccount(MailAccount mailAccount)
        {
            if (mailAccount.CompareTo(_mailAccount) == 0)
                return;
        }
    }
}
