using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerAndDistributionOfQRCodes.Service.Interface
{
    public interface IDataService
    {
        Task<T> Get<T>(string key, T defaultValue);
        Task Save<T>(string key, T defaultValue);
    }
}
