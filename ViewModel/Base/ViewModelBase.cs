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
        public virtual Task OnNavigatingTo(object? parameter, object? parameterSecond = null)
             => Task.CompletedTask;
        public virtual Task OnNavigatedFrom(bool isForwardNavigation)
            => Task.CompletedTask;
        public virtual Task OnNavigatedTo()
            => Task.CompletedTask;
        public virtual Task OnUpdate()
            => Task.CompletedTask;
        public virtual Task OnUpdateDbService()
          => Task.CompletedTask;
        public virtual Task OnStart()
            => Task.CompletedTask;

    }
}
