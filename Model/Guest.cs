using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Exchange.WebServices.Data;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using MailKit.Security;
using MimeKit.Utils;
using Bytescout.Spreadsheet.Charts;
using DocumentFormat.OpenXml.Wordprocessing;
using ScannerAndDistributionOfQRCodes.Service.Interface;
using System.Net.Mail;
using ScannerAndDistributionOfQRCodes.Data.Message;
using ScannerAndDistributionOfQRCodes.Data.QRCode;
using ScannerAndDistributionOfQRCodes.Data.Message.Mail;


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

        private User _user = new();
        [Column("user")]
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private Mail _mail = new();
        [Column("mail")]
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public Mail Mail
        {
            get => _mail;
            set => SetProperty(ref _mail, value);
        }

        private VerificationQRCode _verificatQRCode = new();
        [Column("verification_qr_code")]
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public VerificationQRCode VrificatQRCode
        {
            get => _verificatQRCode;
            set => SetProperty(ref _verificatQRCode, value);
        }
        public DateTime ArrivalTime { get; set; }

        public Guest()
        {
        }

        public Guest SetSurname(string surname)
        {
            User.Surname = RemoveSpaces(surname);
            return this;
        }
        public Guest SetName(string name)
        {
            User.Name = RemoveSpaces(name);
            return this;
        }
        public Guest SetPatronymic(string patronymic)
        {
            User.Patronymic = RemoveSpaces(patronymic);
            return this;
        }
        public Guest SetMail(string mailAddress)
        {
            if (Mail.MailAddress.Equals(mailAddress))
                return this;
            Mail.Change(mailAddress);
            VrificatQRCode.Change(User,Mail);
           return this;
        }
        private string RemoveSpaces(string value)
            =>value.Replace(" ", "");

        public StatisticsGuest GetStatisticsGuest()
        {
            return new StatisticsGuest(User.Surname,User.Name,User.Patronymic,Mail.IsMessageSent?"OK":"ON"
                    , VrificatQRCode.IsVerifiedQRCode?"OK":"ON", VrificatQRCode.IsVerifiedQRCode?ArrivalTime.ToString():"ON");
        }
    }
}
