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
    public sealed class Mail : ObservableObject, IMail, ISendingMessages
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

        public void Change(string newMailAddress)
        {
            MailAddress = newMailAddress;
        }


        private string SetMailAddress(string mail)
        {
            if (!string.IsNullOrEmpty(MailAddress) & mail.Equals(MailAddress))
                return MailAddress;
            IsMessageSent = false;
            var mailWithoutSpaces = mail.Replace(" ", "");
            IsValidMail =  EmailValidator.CheckEmailValidatorAll(mailWithoutSpaces);
            return mailWithoutSpaces;
        }

        public override bool Equals(object obj)
        {
            if (obj is Mail mail)
                return mail.MailAddress.Equals(MailAddress);
            if (obj is string mailStr)
                return MailAddress.Equals(mailStr);
            return false;
        }

        public override int GetHashCode() => MailAddress.GetHashCode();

        public void  SendingMessages(string subject, MessageText messageText, IMailAccount mailAccount, string userName = "", Stream stream = null)
        {
            var emailSetnd = new EmailYandexMessage(subject, messageText, userName, MailAddress,
                mailAccount, stream);
            try
            {
                IsMessageSent = emailSetnd.Send();
            }
            catch (SendMailMessageException ex)
            {
                IsMessageSent = false;
                throw new SendMailMessageException(ex.Message,ex.InnerException);
            }
        }
    }
}
