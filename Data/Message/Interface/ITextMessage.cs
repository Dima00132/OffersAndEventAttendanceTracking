namespace ScannerAndDistributionOfQRCodes.Data.Message.Interface
{
    public interface ITextMessage : IMessage
    {
        string Text { get; }
    }
}
