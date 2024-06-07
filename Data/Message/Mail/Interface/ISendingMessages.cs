using ScannerAndDistributionOfQRCodes.Data.Message;
using ScannerAndDistributionOfQRCodes.Model;


namespace ScannerAndDistributionOfQRCodes.Data.Message.Mail.Interface
{
    public interface ISendingMessages
    {
        SenderResponseCode SendingMessages(string subject, MessageText messageText, IMailAccount mailAccount, string userName = "", Stream stream = null);
    }
}
