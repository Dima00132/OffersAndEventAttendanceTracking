using System.Drawing;

namespace ScannerAndDistributionOfQRCodes.Model.Interface
{
    public interface IDecodeQRCode
    {
        string Decode(Bitmap image);
    }
}
