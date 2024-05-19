using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerAndDistributionOfQRCodes.Model
{
    public class ScannerQR
    {
        private  FilterInfoCollection _infoCollection;
        private VideoCaptureDevice _captureDevice;

        public string[] GetVideoInputDevice()
        {
            _infoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);

        }
        public void StartScan(NewFrameEventHandler eventHandler,string videoDevice)
        {
            if (string.IsNullOrEmpty(videoDevice) || _infoCollection is null)
            {
                _infoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                videoDevice = _infoCollection[0].MonikerString;
            }
            //_infoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            //_captureDevice = new VideoCaptureDevice(_infoCollection[0].MonikerString);
            _captureDevice = new VideoCaptureDevice(videoDevice);
            _captureDevice.NewFrame += eventHandler;
            _captureDevice.Start();

        }
    }
}
