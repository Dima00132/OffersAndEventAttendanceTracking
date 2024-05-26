using CommunityToolkit.Maui.Views;
using ScannerAndDistributionOfQRCodes.Service.PopupService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerAndDistributionOfQRCodes.Service.PopupService
{
    public class PopupService : IPopupService
    {
        public void ShowPopup(Popup popup)
        {
            Page page = Application.Current?.MainPage ?? throw new NullReferenceException();
            page.ShowPopup(popup);
        }
    }
}
