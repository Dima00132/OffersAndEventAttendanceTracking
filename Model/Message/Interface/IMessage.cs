using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ScannerAndDistributionOfQRCodes.Model.Message
{
    public interface IMessage
    {
        bool Send();
        string ToAddress { get;  }
        IMailAccount From { get; }
    }
}
