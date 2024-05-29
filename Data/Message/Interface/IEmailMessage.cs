namespace ScannerAndDistributionOfQRCodes.Data.Message.Interface
{
    public interface IEmailMessage : ITextMessage
    {
        string Subject { get; }
    }
}
