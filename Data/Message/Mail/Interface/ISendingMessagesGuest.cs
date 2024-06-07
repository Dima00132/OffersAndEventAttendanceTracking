using ScannerAndDistributionOfQRCodes.Data.Message;
using ScannerAndDistributionOfQRCodes.Model;


namespace ScannerAndDistributionOfQRCodes.Data.Message.Mail.Interface
{
    public interface ISendingMessagesGuest
    {
        bool IsValidMail { get; set; }
        bool IsMessageSent { get; set; }
        SenderResponseCode SendingMessagesGuest(string subject, string messageText, Guest guest, IMailAccount mailAccount);
    }
}
