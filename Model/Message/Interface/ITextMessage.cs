namespace ScannerAndDistributionOfQRCodes.Model.Message
{
    public interface ITextMessage : IMessage
    {
        string Text { get;  }
    }
}
