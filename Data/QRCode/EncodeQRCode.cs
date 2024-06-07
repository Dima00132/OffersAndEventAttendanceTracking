using QRCoder;
using Image = Microsoft.Maui.Controls.Image;
using ScannerAndDistributionOfQRCodes.Data.QRCode.QRCodeInterface;

namespace ScannerAndDistributionOfQRCodes.Data.QRCode
{
    public static  class EncodeQRCode 
    {
        public static Image Encode(string text)
        {
            if (string.IsNullOrEmpty(text))
                text = string.Empty;
            QRCodeGenerator generator = new();
            QRCodeData codeData = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.L);
            PngByteQRCode qRCode = new(codeData);
            Image imageSource = null;

            byte[] qrCodeBytes = qRCode.GetGraphic(20);

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                imageSource.Source = ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));
            });
            return imageSource;
        }

        public static Stream EncodeStream(string text)
        {
            if (string.IsNullOrEmpty(text))
                text = string.Empty;
            QRCodeGenerator generator = new();
            QRCodeData codeData = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.L);
            PngByteQRCode qRCode = new(codeData);
            byte[] qrCodeBytes = qRCode.GetGraphic(20);
            return new MemoryStream(qrCodeBytes);
        }
    }
}
