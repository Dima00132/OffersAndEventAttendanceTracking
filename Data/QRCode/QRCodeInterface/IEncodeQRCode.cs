using Image = Microsoft.Maui.Controls.Image;

namespace ScannerAndDistributionOfQRCodes.Data.QRCode.QRCodeInterface
{
    public interface IEncodeQRCode
    {
        Image Encode(string text);
        Stream EncodeStream(string text);
    }
}
