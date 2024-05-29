using AForge.Video;
using AForge.Video.DirectShow;
using Camera.MAUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScannerAndDistributionOfQRCodes.Data.QRCode
{
    public sealed class ScannerQR
    {
        private FilterInfoCollection _infoCollection;
        private VideoCaptureDevice _captureDevice;
        private readonly NewFrameEventHandler _frameEventHandler;

        public bool Is { get; private set; }


        public bool IsCameraLaunched { get; private set; }

        public ScannerQR(NewFrameEventHandler frameEventHandler)
        {
            _frameEventHandler = frameEventHandler;
        }



        public FilterInfoCollection GetVideoInputDevice()
        {
            _infoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            return _infoCollection;
        }
        public ScannerQR ConnectingCamera(string videoDevice)
        {
            if (string.IsNullOrEmpty(videoDevice) || _infoCollection is null)
            {
                _infoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                videoDevice = _infoCollection[0].MonikerString;
            }
            //_infoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            //_captureDevice = new VideoCaptureDevice(_infoCollection[0].MonikerString);
            _captureDevice = new VideoCaptureDevice(videoDevice);
            _captureDevice.NewFrame -= _frameEventHandler;
            _captureDevice.NewFrame += _frameEventHandler;

            return this;
        }

        private object obj = new object();
        public void StartCamera()
        {
            IsCameraLaunched = true;
            _captureDevice.Start();
        }
        public void StopCamera()
        {
            IsCameraLaunched = false;
            _captureDevice.SignalToStop();
        }


    }
}
