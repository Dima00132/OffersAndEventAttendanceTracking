using CommunityToolkit.Mvvm.ComponentModel;
using ScannerAndDistributionOfQRCodes.Model.QRCode;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Exchange.WebServices.Data;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using MailKit.Security;
using MimeKit.Utils;
using ScannerAndDistributionOfQRCodes.Model.Message;
using Bytescout.Spreadsheet.Charts;
using DocumentFormat.OpenXml.Wordprocessing;
using ScannerAndDistributionOfQRCodes.Service.Interface;


namespace ScannerAndDistributionOfQRCodes.Model
{

    [Table("guest")]
    public partial class Guest:ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("scheduled_event_id")]
        [ForeignKey(typeof(ScheduledEvent))]
        public int ScheduledEventId { get; set; }

        private User _user = new User();
        [Column("user")]
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

    
        private string _mail;
        public string Mail
        {
            get => _mail;
            set => SetProperty(ref _mail, value);
        }

        private bool _isVerifiedQRCode;
        public bool IsVerifiedQRCode
        {
            get => _isVerifiedQRCode;
            set => SetProperty(ref _isVerifiedQRCode, value);
        }

        private bool _isValidMail;
        public bool IsValidMail
        {
            get => _isValidMail;
            set => SetProperty(ref _isValidMail, value);
        }

        private bool _isMessageSent;
        public bool IsMessageSent
        {
            get => _isMessageSent;
            set => SetProperty(ref _isMessageSent, value);
        }

        public string QRHashCode { get; set; }

        public Guest(){}

        public Guest SetSurname(string surname)
        {
            User.Surname = RemoveSpaces(surname);
           // Surname = RemoveSpaces(surname);
            return this;
        }
        public Guest SetName(string name)
        {
            User.Name = RemoveSpaces(name);
            //Name =  RemoveSpaces(name);
            return this;
        }
        public Guest SetPatronymic(string patronymic)
        {
            User.Patronymic = RemoveSpaces(patronymic);
            //Patronymic = RemoveSpaces(patronymic);
            return this;
        }
        public Guest SetMail(string mail)
        {
            if (Mail is not null && Mail.Equals(mail))
                return this;

            if (!string.IsNullOrEmpty(Mail))
                IsMessageSent = false;
            var mailWithoutSpaces = RemoveSpaces(mail);

            IsValidMail = EmailValidator.CheckEmailValidator(mailWithoutSpaces);
            //Проверка валидности посты
            Mail = mailWithoutSpaces;
            QRHashCode = GenerateQRHashCode();
            return this;
        }

        private string GenerateQRHashCode()
        {
            var uniqueQR = GeneratorUniqueQRHashCode
                .Generate(User.Name, User.Surname, User.Patronymic, Mail);
            return uniqueQR;
        }

        private string RemoveSpaces(string value)
            =>value.Replace(" ", "");

        public SendMessage GetUpSubscriptionForSendingMessages()
            => SendingMessages;

        public void SendingMessages(string nameEvent, string messageText, ILocalDbService localDbService, bool resendMessage = false)
        {
            if (IsMessageSent & !resendMessage)
                return;

            var encodeQRCode = new EncodeQRCode();
            // _qRCodeImge = encodeQRCode.Encode(QRHashCode);

            var stream = encodeQRCode.EncodeStream(QRHashCode);

            var emailSetnd = new EmailYandexMessage(messageText, nameEvent, $"{User}", Mail,
                new MailAccount("TestMailSendr@yandex.ru", "cwufaysygkohokyr", new User("1", "1", "1")), stream);
            //var emailSetnd = new EmailYandexMessage(scheduled.MessageText, scheduled.NameEvent, $"{Surname} {Name} {Patronymic}", Mail,
            //    new MailAccount("TestMailSendr@yandex.ru", "cwufaysygkohokyr",new User("1","1","1")), stream);
            ///
            IsMessageSent = emailSetnd.Send();
            localDbService.Update(this);
            //}
        }
    }
}
