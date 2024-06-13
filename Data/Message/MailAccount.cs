using ScannerAndDistributionOfQRCodes.Model;
using MailKit.Net.Smtp;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ScannerAndDistributionOfQRCodes.Data.Message
{
    public interface IMailAccount
    {
        string MailAddress { get; set; }

        string MailID { get; set; }
        string Password { get; set; }
        User UserData { get; set; }
        MailServer MailServer { get; set; }

       void Create(string mailID, string mailAddress, string password, User userData, MailServer mailServer);
    }

    public sealed class MailServer
    {
        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("mail_account_id")]
        [ForeignKey(typeof(MailAccount))]
        public int MailAccountId { get; set; }

        public MailServer()
        {
        }

        public MailServer(string server, int port, bool connectionProtection)
        {
            Server = server;
            Port = port;
            ConnectionProtection = connectionProtection;
        }

        public string Server { get; set; }
        public int Port { get; set; }
        public bool ConnectionProtection { get; set; }
    }

    public sealed class MailAccount : IMailAccount
    {
        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("guest_id")]
        public int СardQuestionId { get; set; }

        public MailAccount()
        {
        }

        //public MailAccount(string mailID ,string mailAddress, string password, User userData, MailServer mailServer)
        //{
        //    MailAddress = mailAddress;
        //    Password = password;
        //    UserData = userData;
        //    MailServer = mailServer;
        //    MailID = mailID;
        //}

        public void Create(string mailID, string mailAddress, string password, User userData, MailServer mailServer)
        {
            MailAddress = mailAddress;
            Password = password;
            UserData = userData;
            MailServer = mailServer;
            MailID = mailID;
        }

        public string MailAddress { get; set; }
        public string Password { get; set; }

        [Column("user")]
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public User UserData { get; set; }

        [Column("mail_server")]
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public MailServer MailServer { get; set; }

        public string MailID { get; set; }
    }
}
