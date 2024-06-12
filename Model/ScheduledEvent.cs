using CommunityToolkit.Mvvm.ComponentModel;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Maui.Animations;
using ScannerAndDistributionOfQRCodes.Service.Interface;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ColumnAttribute = SQLite.ColumnAttribute;
using ForeignKeyAttribute = SQLiteNetExtensions.Attributes.ForeignKeyAttribute;
using TableAttribute = SQLite.TableAttribute;

namespace ScannerAndDistributionOfQRCodes.Model
{
    [Table("scheduled_event")]
    public sealed partial class ScheduledEvent : ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("whole_event_id")]
        [ForeignKey(typeof(WholeEvent))]
        public int WholeEventId { get; set; }

        private ObservableCollection<Guest> _guests = [];

        [Column("guests")]
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public ObservableCollection<Guest> Guests
        {
            get => _guests;
            set =>  SetProperty(ref _guests, value);
        }

        [ObservableProperty]
        private string _nameEvent;
        [ObservableProperty]
        private DateTime _date;
        [ObservableProperty]
        private int _countArrivedGuests;

        private MessageText _messageText = new();
        [Column("mssage_text")]
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public MessageText MessageText
        {
            get => _messageText;
            set => SetProperty(ref _messageText, value);
        }
        public ScheduledEvent()
        {
        }
        public ScheduledEvent(string nameEvent, DateTime date)
        {
            NameEvent = nameEvent;
            Date = date;
        }
        public IEnumerable<Guest> FindsQuestionByRequest(string request)
        {
            var result = Guests
                .Where(x => x.User.ToString().Length >= request.Length)
                .Where(x => CompareUser(x.User.Name, request) | CompareUser(x.User.ToString(), request)
                | CompareUser(x.User.Surname,  request)| CompareUser(x.User.Patronymic, request));
            return result;
        }
        private bool CompareUser(string name, string request)
            => String.Compare(name, 0, request, 0, request.Length, StringComparison.OrdinalIgnoreCase) == 0;
        public Guest? SearchForGuestByQRHashCode(string hash)
            =>Guests.FirstOrDefault(x => x.VrificatQRCode.CompareQRHashCode(hash),null);
    }
}
