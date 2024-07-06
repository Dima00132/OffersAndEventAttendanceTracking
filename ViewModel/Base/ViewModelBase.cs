using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerAndDistributionOfQRCodes.ViewModel.Base
{
    public class ViewModelBase:ObservableObject
    {
        public virtual Task OnNavigatingToAsync(object parameter, object parameterSecond = null)
             => Task.CompletedTask;
        public virtual Task OnNavigatedFromAsync(bool isForwardNavigation)
            => Task.CompletedTask;
        public virtual Task OnNavigatedToAsync()
            => Task.CompletedTask;
        public virtual Task OnUpdateAsync()
            => Task.CompletedTask;
        public virtual Task OnUpdateDbServiceAsync()
          => Task.CompletedTask;
        public virtual Task OnStartAsync()
            => Task.CompletedTask;

    }
}
