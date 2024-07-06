using ScannerAndDistributionOfQRCodes.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerAndDistributionOfQRCodes.Navigation
{
    public interface INavigationService
    {
        bool IsAnimated { get; set; }
        Task NavigateToMainPageAsync(object parameter = null);
        Task NavigateByPageAsync<T>(object parameter = null, object parameterSecond = null) where T : Page;
        public Task NavigateByViewModelAsync<T>(object parameter = null) where T : ViewModelBase;
        Task NavigateBackAsync();
        Task NavigateBackUpdateAsync();
    }
}
