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

    public sealed class MailServer : IComparable, IComparable<MailServer>
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

        public string Server { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool ConnectionProtection { get; set; }

        public void Change(string server, int port, bool connectionProtection)
        {
            Server = server;
            Port = port;
            ConnectionProtection = connectionProtection;
        }

        public int CompareTo(MailServer other)
        {
            if (other is null)
                return -1;
            return other.Port.CompareTo(Port) + other.Server.CompareTo(Server) + other.ConnectionProtection.CompareTo(ConnectionProtection);
        }

        public int CompareTo(object obj)
        {
            var server = obj as MailServer;
            return this.CompareTo(server);
        }
    }

    public sealed class MailAccount : IMailAccount, IComparable, IComparable<MailAccount>
    {
        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("guest_id")]
        public int GuestId { get; set; }

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

        public void Change(string mailID, string mailAddress, string password)
        {
            MailAddress = mailAddress;
            Password = password;
            MailID = mailID;
        }

        public void Create(string mailID, string mailAddress, string password, User userData, MailServer mailServer)
        {
            MailAddress = mailAddress;
            Password = password;
            UserData = userData;
            MailServer = mailServer;
            MailID = mailID;
        }

        public int CompareTo(MailAccount other)
        {
            if (other is null)
                return -1;
    
            return other.MailAddress.CompareTo(MailAddress)+ other.Password.CompareTo(Password) 
                + other.UserData.CompareTo(UserData)+ other.MailServer.CompareTo(MailServer) +other.MailID.CompareTo(MailID);
        }

        public int CompareTo(object obj)
        {
            var account = obj as MailAccount;
            return this.CompareTo(account);
        }

        public string MailAddress { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        [Column("user")]
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public User UserData { get; set; } = new User();

        [Column("mail_server")]
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public MailServer MailServer { get; set; } = new MailServer();

        public string MailID { get; set; } = string.Empty;
    }
}
