using Image = Microsoft.Maui.Controls.Image;

namespace ScannerAndDistributionOfQRCodes.Model.Interface
{
    public interface IEncodeQRCode
    {
        Image Encode(string text);
    }
}
