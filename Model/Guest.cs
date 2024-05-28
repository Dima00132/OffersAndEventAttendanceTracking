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
using static QRCoder.PayloadGenerator;
using System.Net.Mail;


namespace ScannerAndDistributionOfQRCodes.Model
{
    [Table("mail")]
    public partial class Mail : ObservableObject
    {

        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("guest_id")]
        [ForeignKey(typeof(Guest))]
        public int GuestId { get; set; }

        private string _mailAddress = string.Empty;
        public string MailAddress
        {
            get => _mailAddress;
            set 
            {
                var address =  SetMailAddress(value);
                    
                SetProperty(ref _mailAddress, address); 
            }
        }

        private bool _isValidMail;
        public bool IsValidMail
        {
            get => _isValidMail;
            set => SetProperty(ref _isValidMail, value);
        }

        private bool _isMessageSent;
        public Mail(string mailAddress)
        {
            MailAddress = mailAddress;
        }

        public Mail()
        {
        }

        public bool IsMessageSent
        {
            get => _isMessageSent;
            set => SetProperty(ref _isMessageSent, value);
        }


        private string SetMailAddress(string mail)
        {
            if (!string.IsNullOrEmpty(MailAddress) & mail.Equals(MailAddress))
                return MailAddress;

            IsMessageSent = false;
            var mailWithoutSpaces = mail.Replace(" ", "");

            IsValidMail = CheckEmailValidator(mailWithoutSpaces);
            //Проверка валидности посты
           //MailAddress = mailWithoutSpaces;
            return mailWithoutSpaces;
        }


        private bool CheckEmailValidator(string mail)
            => !string.IsNullOrEmpty(mail) && CheckingEmailFormat(mail);

        private bool CheckingEmailFormat(string mail)
            => System.Text.RegularExpressions.Regex.IsMatch(mail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        public override bool Equals(object? obj)
        {
            if (obj is Mail mail)
                return mail.MailAddress.Equals(MailAddress);
            if (obj is string mailStr)
                return MailAddress.Equals(mailStr);
            return false;
        }

        public void SendingMessages(string nameEvent, string messageText, IMailAccount mailAccount, User user = null, Stream stream = null)
        {
            //var encodeQRCode = new EncodeQRCode();

            // _qRCodeImge = encodeQRCode.Encode(QRHashCode);

            //var stream = encodeQRCode.EncodeStream(QRHashCode);

            // new MailAccount("TestMailSendr@yandex.ru", "cwufaysygkohokyr", new User("1", "1", "1"))

            string userName = user is null ? string.Empty : user.ToString();

            var emailSetnd = new EmailYandexMessage(messageText, nameEvent, userName, MailAddress,
                mailAccount, stream);
            //var emailSetnd = new EmailYandexMessage(scheduled.MessageText, scheduled.NameEvent, $"{Surname} {Name} {Patronymic}", Mail,
            //    new MailAccount("TestMailSendr@yandex.ru", "cwufaysygkohokyr",new User("1","1","1")), stream);
            ///
            IsMessageSent = emailSetnd.Send();
            //localDbService.Update(this);
            //}
        }

        public void SendingMessagesGuest(string nameEvent, string messageText, Guest guest, IMailAccount mailAccount)
        {
            var encodeQRCode = new EncodeQRCode();

            //_qRCodeImge = encodeQRCode.Encode(user.QRHashCode);

            var stream = encodeQRCode.EncodeStream(guest.VrificatQRCode.QRHashCode);

            // new MailAccount("TestMailSendr@yandex.ru", "cwufaysygkohokyr", new User("1", "1", "1"))
            SendingMessages(nameEvent, messageText, mailAccount, guest.User, stream);


            //var emailSetnd = new EmailYandexMessage(messageText, nameEvent, $"{user}", MailAddress,
            //    mailAccount, stream);
            ////var emailSetnd = new EmailYandexMessage(scheduled.MessageText, scheduled.NameEvent, $"{Surname} {Name} {Patronymic}", Mail,
            ////    new MailAccount("TestMailSendr@yandex.ru", "cwufaysygkohokyr",new User("1","1","1")), stream);
            /////
            //IsMessageSent = emailSetnd.Send();
            //localDbService.Update(this);
            //}
        }


        //public void SendingMessages(string nameEvent, string messageText, ILocalDbService localDbService, bool resendMessage = false)
        //{
        //    if (IsMessageSent & !resendMessage)
        //        return;

        //    var encodeQRCode = new EncodeQRCode();
        //    // _qRCodeImge = encodeQRCode.Encode(QRHashCode);

        //    var stream = encodeQRCode.EncodeStream(QRHashCode);

        //    var emailSetnd = new EmailYandexMessage(messageText, nameEvent, $"{User}", Mail,
        //        new MailAccount("TestMailSendr@yandex.ru", "cwufaysygkohokyr", new User("1", "1", "1")), stream);
        //    //var emailSetnd = new EmailYandexMessage(scheduled.MessageText, scheduled.NameEvent, $"{Surname} {Name} {Patronymic}", Mail,
        //    //    new MailAccount("TestMailSendr@yandex.ru", "cwufaysygkohokyr",new User("1","1","1")), stream);
        //    ///
        //    IsMessageSent = emailSetnd.Send();
        //    localDbService.Update(this);
        //    //}
        //}
    }

    [Table("verified_qr_code")]
    public partial class VerificationQRCode : ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("guest_id")]
        [ForeignKey(typeof(Guest))]
        public int GuestId { get; set; }

        private bool _isVerifiedQRCode;    
        public bool IsVerifiedQRCode
        {
            get => _isVerifiedQRCode;
            set => SetProperty(ref _isVerifiedQRCode, value);
        }
        public string QRHashCode { get; set; }

        public VerificationQRCode(User user, string mailAddress)
        {
            QRHashCode = GenerateQRHashCode(user, mailAddress);
        }

        public VerificationQRCode()
        {
        }

        private string GenerateQRHashCode(User user, string mailAddress)
            =>GeneratorUniqueQRHashCode
                .Generate(user.Name, user.Surname, user.Patronymic, mailAddress);

        public bool CompareQRHashCode(string hash) =>
            QRHashCode.Equals(hash);
    }

    [Table("guest")]
    public partial class Guest:ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("scheduled_event_id")]
        [ForeignKey(typeof(ScheduledEvent))]
        public int ScheduledEventId { get; set; }

        private User _user = new();
        [Column("user")]
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }


        private Mail _mail = new();
        [Column("mail")]
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public Mail Mail
        {
            get => _mail;
            set => SetProperty(ref _mail, value);
        }

       
        private VerificationQRCode _verificatQRCode;
        [Column("verification_qr_code")]
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public VerificationQRCode VrificatQRCode
        {
            get => _verificatQRCode;
            set => SetProperty(ref _verificatQRCode, value);
        }

        
        //private bool _isVerifiedQRCode;
        //public bool IsVerifiedQRCode
        //{
        //    get => _isVerifiedQRCode;
        //    set => SetProperty(ref _isVerifiedQRCode, value);
        //}

        //private bool _isValidMail;
        //public bool IsValidMail
        //{
        //    get => _isValidMail;
        //    set => SetProperty(ref _isValidMail, value);
        //}

        //private bool _isMessageSent;
        //public bool IsMessageSent
        //{
        //    get => _isMessageSent;
        //    set => SetProperty(ref _isMessageSent, value);
        //}

        //public string QRHashCode { get; private set; }

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
        public Guest SetMail(string mailAddress)
        {
            var newMail  = new Mail(mailAddress);

            if (!Mail.MailAddress.Equals(newMail.MailAddress))
                VrificatQRCode = new VerificationQRCode(User, newMail.MailAddress);

            Mail = newMail;
            //if (Mail is not null && Mail.Equals(mail))
            //    return this;

            //if (!string.IsNullOrEmpty(Mail.MailAddress))
            //    Mail.IsMessageSent = false;
            //var mailWithoutSpaces = RemoveSpaces(mail);

            //Mail.IsValidMail = EmailValidator.CheckEmailValidator(mailWithoutSpaces);
            ////Проверка валидности посты
            //Mail = mailWithoutSpaces;
            // QRHashCode = GenerateQRHashCode();
           //if (!Mail.MailAddress.Equals(newMail.MailAddress))
           //     VrificatQRCode = new VerificationQRCode(User, Mail.MailAddress);
           return this;
        }

        private string GenerateQRHashCode()
        {
            var uniqueQR = GeneratorUniqueQRHashCode
                .Generate(User.Name, User.Surname, User.Patronymic, Mail.MailAddress);
            return uniqueQR;
        }

        private string RemoveSpaces(string value)
            =>value.Replace(" ", "");

        //public SendMessage GetUpSubscriptionForSendingMessages()
        //    => SendingMessages;

        //public void SendingMessages(string nameEvent, string messageText, ILocalDbService localDbService, bool resendMessage = false)
        //{
        //    if (IsMessageSent & !resendMessage)
        //        return;

        //    var encodeQRCode = new EncodeQRCode();
        //    // _qRCodeImge = encodeQRCode.Encode(QRHashCode);

        //    var stream = encodeQRCode.EncodeStream(QRHashCode);

        //    var emailSetnd = new EmailYandexMessage(messageText, nameEvent, $"{User}", Mail,
        //        new MailAccount("TestMailSendr@yandex.ru", "cwufaysygkohokyr", new User("1", "1", "1")), stream);
        //    //var emailSetnd = new EmailYandexMessage(scheduled.MessageText, scheduled.NameEvent, $"{Surname} {Name} {Patronymic}", Mail,
        //    //    new MailAccount("TestMailSendr@yandex.ru", "cwufaysygkohokyr",new User("1","1","1")), stream);
        //    ///
        //    IsMessageSent = emailSetnd.Send();
        //    localDbService.Update(this);
        //    //}
        //}
    }
}
