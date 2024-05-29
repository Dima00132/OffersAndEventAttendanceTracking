using System.Drawing;

namespace ScannerAndDistributionOfQRCodes.Data.QRCode.QRCodeInterface
{
    public interface IDecodeQRCode
    {
        string Decode(Bitmap image);
    }
}
