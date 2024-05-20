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
    public class Guest
    {
        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("scheduled_event_id")]
        [ForeignKey(typeof(ScheduledEvent))]
        public int ScheduledEventId { get; set; }

        public string Surname { get;private set; }
        public string Name { get; private set; }
        public string Patronymic { get; private set; }
        public string Mail { get; private set; }
        public string QRHashCode { get; private set; }

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
        public void SetMail(string mail)
        {
            var mailWithoutSpaces = RemoveSpaces(mail);
            //Проверка валидности посты
            Mail = mailWithoutSpaces;
            QRHashCode = GenerateQRHashCode();
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

        }
    }
}
