using System.Drawing;

namespace ScannerAndDistributionOfQRCodes.Model.QRCode.QRCodeInterface
{
    public interface IDecodeQRCode
    {
        string Decode(Bitmap image);
    }
}
