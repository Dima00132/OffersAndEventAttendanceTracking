using ScannerAndDistributionOfQRCodes.Model;
using MailKit.Net.Smtp;

namespace ScannerAndDistributionOfQRCodes.Data.Message
{
    public interface IMailAccount
    {
        string MailAddress { get; }

        string MailID { get; }
        string Password { get; }
        User UserData { get; }
        MailServer MailServer { get; }
    }

    public sealed record MailServer(string Server, int Port, bool ConnectionProtection);

    public class MailAccount : IMailAccount
    {
        public MailAccount()
        {
        }

        public MailAccount(string mailID ,string mailAddress, string password, User userData, MailServer mailServer)
        {
            MailAddress = mailAddress;
            Password = password;
            UserData = userData;
            MailServer = mailServer;
            MailID = mailID;
        }

        public string MailAddress { get; }
        public string Password { get; }
        public User UserData { get; }
        public MailServer MailServer { get; }

        public string MailID { get; }
    }
}
