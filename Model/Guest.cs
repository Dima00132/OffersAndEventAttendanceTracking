using CommunityToolkit.Mvvm.ComponentModel;
using ScannerAndDistributionOfQRCodes.Model.QRCode;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerAndDistributionOfQRCodes.Model
{

    [Table("guest")]
    public partial class Guest:ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("scheduled_event_id")]
        [ForeignKey(typeof(ScheduledEvent))]
        public int ScheduledEventId { get; set; }

        
        private string _surname;
        public string Surname  
        { 
            get => _surname;
            set=>SetProperty(ref _surname, value); 
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
    
        private string _mail;
        public string Mail
        {
            get => _mail;
            set => SetProperty(ref _mail, value);
        }
        public string QRHashCode { get;  set; }

        public bool IsMessageSent { get; set; }

        private Image _qRCodeImge;

        public Guest()
        {
        }

        public Guest SetSurname(string surname)
        {
            Surname = RemoveSpaces(surname);
            return this;
        }
        public Guest SetName(string name)
        {
            Name =  RemoveSpaces(name);
            return this;
        }
        public Guest SetPatronymic(string patronymic)
        {
            Patronymic = RemoveSpaces(patronymic);
            return this;
        }
        public Guest SetMail(string mail)
        {

            if (!string.IsNullOrEmpty(Mail))
                IsMessageSent = false;
            var mailWithoutSpaces = RemoveSpaces(mail);
            //Проверка валидности посты
            Mail = mailWithoutSpaces;
            QRHashCode = GenerateQRHashCode();
            return this;
        }

        private string GenerateQRHashCode()
        {
            var uniqueQR = GeneratorUniqueQRHashCode
                .Generate(Name, Surname, Patronymic, Mail, DateTime.Now.ToString());
            return uniqueQR;
        }

        private string RemoveSpaces(string value)
            =>value.Replace(" ", "");

        public EventHandler GetUpSubscriptionForSendingMessages()
            => SendingMessages;

        private void SendingMessages(object obj,EventArgs eventArgs)
        {
           var encodeQRCode = new EncodeQRCode();
            _qRCodeImge = encodeQRCode.Encode(QRHashCode);



            ////
            ///
            IsMessageSent = true;
        }
    }
}
