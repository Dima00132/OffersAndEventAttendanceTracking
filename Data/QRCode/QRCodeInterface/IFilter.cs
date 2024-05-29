using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerAndDistributionOfQRCodes.Data.QRCode.QRCodeInterface
{
    public interface IFilter
    {
        int Radius { get; }
        Image ProcessImage(Image inputImage);
    }
}
