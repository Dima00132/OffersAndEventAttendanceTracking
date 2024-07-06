using CommunityToolkit.Mvvm.ComponentModel;
using ScannerAndDistributionOfQRCodes.Data.Message;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ScannerAndDistributionOfQRCodes.Model
{
    [Table("user_data")]
    public sealed partial class User : ObservableObject, IComparable, IComparable<User>
    {
        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("guest_id")]
        [ForeignKey(typeof(Guest))]
        public int СardQuestionId { get; set; }

        [Column("mail_account_id")]
        [ForeignKey(typeof(MailAccount))]
        public int MailAccountId { get; set; }


        private string _surname = string.Empty;
        public string Surname
        {
            get => _surname;
            set => SetProperty(ref _surname, value);
        }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _patronymic = string.Empty;
        public string Patronymic
        {
            get => _patronymic;
            set => SetProperty(ref _patronymic, value);
        }

        public User()
        {
        }

        public User(string surname, string name, string patronymic)
        {
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
        }

        public void Change(string surname, string name, string patronymic)
        {
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
        }

        public override string ToString()
            => $"{Surname} {Name} {Patronymic}";

        public int CompareTo(User other)
        {
            if (other is null)
                return -1;
            return other.Name.CompareTo(Name) + other.Patronymic.CompareTo(Patronymic) + other.Surname.CompareTo(Surname);
        }

        public int CompareTo(object obj)
        {
            var user = obj as User;
            return this.CompareTo(user);
        }
    }
}
