using AForge.Video;
using AForge.Video.DirectShow;
using System.Windows.Forms;


namespace ScannerAndDistributionOfQRCodes.Data.QRCode
{
    public sealed class ScannerQR
    {
        private FilterInfoCollection _infoCollection;
        private VideoCaptureDevice _captureDevice;
        private readonly NewFrameEventHandler _frameEventHandler;
        private readonly VideoSourceErrorEventHandler _sourceErrorEventArgs;
        public bool IsCameraLaunched { get; private set; }

        public ScannerQR(NewFrameEventHandler frameEventHandler , VideoSourceErrorEventHandler sourceErrorEventArgs)
        {
            _frameEventHandler = frameEventHandler;
            _sourceErrorEventArgs = sourceErrorEventArgs;
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
            _captureDevice = new VideoCaptureDevice(videoDevice);
            _captureDevice.NewFrame -= _frameEventHandler;
            _captureDevice.NewFrame += _frameEventHandler;
            _captureDevice.VideoSourceError += new VideoSourceErrorEventHandler(_sourceErrorEventArgs);
            return this;
        }

        public void StartCamera()
        {
            IsCameraLaunched = true;
            _captureDevice?.Start();
        }
        public void StopCamera()
        {
            IsCameraLaunched = false;
            _captureDevice?.SignalToStop();
        }
    }
}
