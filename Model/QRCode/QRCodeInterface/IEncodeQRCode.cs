using Image = Microsoft.Maui.Controls.Image;

namespace ScannerAndDistributionOfQRCodes.Model.QRCode.QRCodeInterface
{
    public interface IEncodeQRCode
    {
        Image Encode(string text);
        Stream EncodeStream(string text);
    }
}
