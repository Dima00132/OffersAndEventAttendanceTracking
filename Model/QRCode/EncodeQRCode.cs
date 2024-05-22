using QRCoder;
using Image = Microsoft.Maui.Controls.Image;
using ScannerAndDistributionOfQRCodes.Model.Interface;

namespace ScannerAndDistributionOfQRCodes.Model.QRCode
{
    public sealed class EncodeQRCode:IEncodeQRCode
    {
        public Image Encode(string text)
        {
            if (string.IsNullOrEmpty(text))
                text = string.Empty;
            QRCodeGenerator generator = new QRCodeGenerator();
            QRCodeData codeData = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.L);
            PngByteQRCode qRCode = new PngByteQRCode(codeData);
            Image imageSource = null;

            byte[] qrCodeBytes = qRCode.GetGraphic(20);

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                imageSource.Source = ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));
            });
            return imageSource;
        }

        public Stream EncodeStream(string text)
        {
            if (string.IsNullOrEmpty(text))
                text = string.Empty;
            QRCodeGenerator generator = new QRCodeGenerator();
            QRCodeData codeData = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.L);
            PngByteQRCode qRCode = new PngByteQRCode(codeData);
            Image imageSource = null;

            byte[] qrCodeBytes = qRCode.GetGraphic(20);

            //MainThread.BeginInvokeOnMainThread(async () =>
            //{
            //    imageSource.Source = ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));
            //});
            return new MemoryStream(qrCodeBytes);
        }
    }
}
