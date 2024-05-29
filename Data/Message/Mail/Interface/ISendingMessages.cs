using ScannerAndDistributionOfQRCodes.Data.Message;
using ScannerAndDistributionOfQRCodes.Model;


namespace ScannerAndDistributionOfQRCodes.Data.Message.Mail.Interface
{
    public interface ISendingMessages
    {
        void SendingMessages(string subject, string messageText, IMailAccount mailAccount, User user = null, Stream stream = null);
    }
}
