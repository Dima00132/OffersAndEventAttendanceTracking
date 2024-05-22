namespace ScannerAndDistributionOfQRCodes.Model.Message
{
    public interface IImageMessage : IMessage
    {
        Stream SreamImage { get;  }
    }
}
