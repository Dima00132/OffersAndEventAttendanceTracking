using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ScannerAndDistributionOfQRCodes.Model
{
    [Table("user_data")]
    public partial class User : ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("guest_id")]
        [ForeignKey(typeof(Guest))]
        public int СardQuestionId { get; set; }

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

        public override string ToString()
            => $"{Surname} {Name} {Patronymic}";

    }
}
