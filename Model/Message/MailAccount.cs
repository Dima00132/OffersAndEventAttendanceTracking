namespace ScannerAndDistributionOfQRCodes.Model.Message
{
    public class MailAccount
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
