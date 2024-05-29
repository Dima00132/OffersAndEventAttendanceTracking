using ScannerAndDistributionOfQRCodes.Model;

namespace ScannerAndDistributionOfQRCodes.Data.Message
{
    public interface IMailAccount
    {
        string MailAddress { get; }
        string Password { get; }
        User UserData { get; }
    }

    public class MailAccount : IMailAccount
    {
        public MailAccount()
        {
        }

        public MailAccount(string mailAddress, string password, User userData)
        {
            MailAddress = mailAddress;
            Password = password;
            UserData = userData;
        }

        public string MailAddress { get; }
        public string Password { get; }
        public User UserData { get; }
    }
}
