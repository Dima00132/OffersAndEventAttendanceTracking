using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Animations;
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
    [Table("scheduled_event")]
    public sealed partial class ScheduledEvent : ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("guest_id")]
        [ForeignKey(typeof(Guest))]
        public int GuestId { get; set; }

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


        public ScheduledEvent(string nameEvent, DateTime date)
        {
            NameEvent = nameEvent;
            Date = date;
        }

    }
}
