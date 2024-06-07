using ScannerAndDistributionOfQRCodes.Model;

namespace ScannerAndDistributionOfQRCodes.Data.Message.Interface
{
    public interface ITextMessage : IMessage
    {
       MessageText MessageText { get; }
    }
}
