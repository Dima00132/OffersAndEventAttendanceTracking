using CommunityToolkit.Mvvm.ComponentModel;
using DocumentFormat.OpenXml.Wordprocessing;
using SQLite;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ColumnAttribute = SQLite.ColumnAttribute;
using ForeignKeyAttribute = SQLiteNetExtensions.Attributes.ForeignKeyAttribute;
using TableAttribute = SQLite.TableAttribute;

namespace ScannerAndDistributionOfQRCodes.Model
{
    [Table("message_text")]
    public sealed partial class MessageText : ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("scheduled_event_id")]
        [ForeignKey(typeof(ScheduledEvent))]
        public int ScheduledEventId { get; set; }

        [ObservableProperty]
        private string _text;

        [ObservableProperty]
        private string _organizationData = string.Empty;
        public MessageText() { }

        public MessageText(string text, string organizationData)
        {
            Text = text;
            OrganizationData = organizationData;
        }

        public void Change(string text, string organizationData)
        {
            Text = text;
            OrganizationData = organizationData;
        }
    }
}
