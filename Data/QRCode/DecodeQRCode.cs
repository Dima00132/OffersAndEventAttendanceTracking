using Camera.MAUI;
using Microsoft.Maui.Controls;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Common;
using ZXing.Windows.Compatibility;
using ZXing;
using Image = Microsoft.Maui.Controls.Image;
using BitMiracle.LibTiff.Classic;
using ScannerAndDistributionOfQRCodes.Data.QRCode.QRCodeInterface;

namespace ScannerAndDistributionOfQRCodes.Data.QRCode
{
    public sealed class DecodeQRCode : IDecodeQRCode
    {
        public string Decode(Bitmap image)
        {
            using (image)
            {
                LuminanceSource source;
                source = new BitmapLuminanceSource(image);
                BinaryBitmap bitmap = new BinaryBitmap(new HybridBinarizer(source));
                Result result = new MultiFormatReader().decode(bitmap);
                if (result != null)
                    return result.Text;

            }
            return string.Empty;
        }

    }
}
