using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Animations;
using ScannerAndDistributionOfQRCodes.Service.Interface;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColumnAttribute = SQLite.ColumnAttribute;
using ForeignKeyAttribute = SQLiteNetExtensions.Attributes.ForeignKeyAttribute;
using TableAttribute = SQLite.TableAttribute;

namespace ScannerAndDistributionOfQRCodes.Model
{
    public delegate void SendMessage(string nameEvent,string messageText,ILocalDbService localDbService,bool resendMessage = false);

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
            set
            {
                SetProperty(ref _guests, value);
                //
            }
        }

        [ObservableProperty]
        private string _nameEvent;
        [ObservableProperty]
        private DateTime _date;
        [ObservableProperty]
        private string _messageText = string.Empty;

        [Ignore]
        public SendMessage SendMessageEvent { get; set; }

        

        private int _countGuest;
        public int CountGuest
        {
            get => _countGuest;
            set => SetProperty(ref _countGuest, value);
        }

        public ScheduledEvent(string nameEvent, DateTime date)
        {
            NameEvent = nameEvent;
            Date = date;
        }

        public IEnumerable<Guest> FindsQuestionByRequest(string request)
        {
            var learnQuestions = Guests;
            var result = learnQuestions
                .Where(x => x.User.ToString().Length >= request.Length)
                .Where(x => String.Compare(x.User.ToString(), 0, request, 0, request.Length, StringComparison.OrdinalIgnoreCase) == 0);
            return result;
        }

        public void Add(Guest guest)
        {
            if (Guests is null)
                return;
            Guests.Insert(0, guest);
            ++CountGuest;
        }

        public void Remove(Guest guest)
        {
            if (Guests is null)
                return;

           Guests.Remove(guest);
           --CountGuest;
        }


        public ScheduledEvent()
        {
        }
    }
}
