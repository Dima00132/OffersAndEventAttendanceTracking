using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScannerAndDistributionOfQRCodes.Service.Interface;

namespace ScannerAndDistributionOfQRCodes.Service
{
    public class DataService : IDataService
    {
        public Task<T> Get<T>(string key, T defaultValue)
        {
            var result =  Preferences.Default.Get(key, defaultValue);
            return Task.FromResult(result);
        }

        public Task Save<T>(string key, T defaultValue)
        {
            Preferences.Default.Set(key, defaultValue);
            return Task.CompletedTask;
        }
    }
}
