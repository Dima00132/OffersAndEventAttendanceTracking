using CommunityToolkit.Mvvm.ComponentModel;
using static QRCoder.PayloadGenerator;

namespace ScannerAndDistributionOfQRCodes.Model.Message
{
    public partial class UserData:ObservableObject
    {

        private string _surname;
        public string Surname
        {
            get => _surname;
            set => SetProperty(ref _surname, value);
        }


        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _patronymic;

        public UserData()
        {
        }

        public UserData(string surname, string name, string patronymic)
        {
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
        }

        public string Patronymic
        {
            get => _patronymic;
            set => SetProperty(ref _patronymic, value);
        }

    }


    public class MailAccount
    {
        public MailAccount()
        {
        }

        public MailAccount(string mailAddress, string password, UserData userData)
        {
            MailAddress = mailAddress;
            Password = password;
            UserData = userData;
        }

        public string MailAddress { get; }
        public string Password { get; }
        public UserData UserData { get; }
    }
    public interface IMessage
    {
        bool Send();
        string ToAddress { get;  }
        MailAccount From { get; }
    }
}
