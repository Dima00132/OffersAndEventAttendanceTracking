using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Maui.Core.Extensions;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScannerAndDistributionOfQRCodes.Data.Message;

namespace ScannerAndDistributionOfQRCodes.Model
{
    [Table("whole_event")]
    public sealed class WholeEvent : ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }
        public WholeEvent()
        {
        }

        private ObservableCollection<ScheduledEvent> _scheduledEvent = [];
        [Column("whole_vents")]
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public ObservableCollection<ScheduledEvent> ScheduledEvents
        {
            get => _scheduledEvent;
            set => SetProperty(ref _scheduledEvent, value);
        }
        public WholeEvent SortedCategories()
        {
            ScheduledEvents = ScheduledEvents.OrderByDescending((x)=>x.Id).ToObservableCollection();
            return this;
        }
        public void Add(ScheduledEvent wholeEvent)
        {
            if (ScheduledEvents is null)
                return;
            ScheduledEvents.Insert(0, wholeEvent);
        }
        public void Remove(ScheduledEvent wholeEvent)
            =>ScheduledEvents?.Remove(wholeEvent);
        public ObservableCollection<ScheduledEvent> GetWholeEvents()
            =>ScheduledEvents;
    }
}
