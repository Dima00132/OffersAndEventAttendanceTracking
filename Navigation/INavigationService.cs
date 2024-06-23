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
        Task NavigateToMainPage(object? parameter = null);
        Task NavigateByPage<T>(object? parameter = null, object? parameterSecond = null) where T : Page;
        public Task NavigateByViewModel<T>(object? parameter = null) where T : ViewModelBase;
        Task NavigateBack();
        Task NavigateBackUpdate();
    }
}
