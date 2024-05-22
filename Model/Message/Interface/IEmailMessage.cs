namespace ScannerAndDistributionOfQRCodes.Model.Message
{
    public interface IEmailMessage : ITextMessage
    {
        string Subject { get;  }
    }
}
