namespace ScannerAndDistributionOfQRCodes.Data.Message.Interface
{
    public interface IImageMessage : IMessage
    {
        Stream SreamImage { get; }
    }
}
