using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerAndDistributionOfQRCodes.Model.Interface
{
    public interface IFilter
    {
        int Radius { get; }
        Image ProcessImage(Image inputImage);
    }
}
