using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using SQLiteNetExtensions.Attributes;
using DocumentFormat.OpenXml.Wordprocessing;
using ScannerAndDistributionOfQRCodes.Data.Message;

/* Необъединенное слияние из проекта "ScannerAndDistributionOfQRCodes (net8.0-android)"
До:
using ScannerAndDistributionOfQRCodes.Data.QRCode;
После:
using ScannerAndDistributionOfQRCodes.Data.QRCode;
using ScannerAndDistributionOfQRCodes;
using ScannerAndDistributionOfQRCodes.Model;
using ScannerAndDistributionOfQRCodes.Data.Message.Mail;
*/
using ScannerAndDistributionOfQRCodes.Data.QRCode;
using ScannerAndDistributionOfQRCodes.Model;
using ScannerAndDistributionOfQRCodes.Data.Message.Mail.Interface;


namespace ScannerAndDistributionOfQRCodes.Data.Message.Mail
{
    [Table("mail")]
    public partial class Mail : ObservableObject, IMail, ISendingMessages, ISendingMessagesGuest
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
                var address = SetMailAddress(value);

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
        public bool IsMessageSent
        {
            get => _isMessageSent;
            set => SetProperty(ref _isMessageSent, value);
        }
        public Mail(string mailAddress)
        {
            MailAddress = mailAddress;
        }

        public Mail()
        {
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

        public override int GetHashCode() => MailAddress.GetHashCode();

        public void SendingMessages(string subject, string messageText, IMailAccount mailAccount, User user = null, Stream stream = null)
        {
            //var encodeQRCode = new EncodeQRCode();

            // _qRCodeImge = encodeQRCode.Encode(QRHashCode);

            //var stream = encodeQRCode.EncodeStream(QRHashCode);

            // new MailAccount("TestMailSendr@yandex.ru", "cwufaysygkohokyr", new User("1", "1", "1"))

            string userName = user is null ? string.Empty : user.ToString();

            var emailSetnd = new EmailYandexMessage(messageText, subject, userName, MailAddress,
                mailAccount, stream);
            //var emailSetnd = new EmailYandexMessage(scheduled.MessageText, scheduled.NameEvent, $"{Surname} {Name} {Patronymic}", Mail,
            //    new MailAccount("TestMailSendr@yandex.ru", "cwufaysygkohokyr",new User("1","1","1")), stream);
            ///
            IsMessageSent = emailSetnd.Send();
            //localDbService.Update(this);
            //}
        }

        public void SendingMessagesGuest(string subject, string messageText, Guest guest, IMailAccount mailAccount)
        {
            var encodeQRCode = new EncodeQRCode();

            //_qRCodeImge = encodeQRCode.Encode(user.QRHashCode);

            var stream = encodeQRCode.EncodeStream(guest.VrificatQRCode.QRHashCode);

            // new MailAccount("TestMailSendr@yandex.ru", "cwufaysygkohokyr", new User("1", "1", "1"))
            SendingMessages(subject, messageText, mailAccount, guest.User, stream);


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
}
