using AForge.Video;
using Camera.MAUI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Common;
using ZXing.Windows.Compatibility;
using ZXing;
using ScannerAndDistributionOfQRCodes.ViewModel.Base;
using ScannerAndDistributionOfQRCodes.Model.QRCode;
using ScannerAndDistributionOfQRCodes.Model.Interface;
using System.Reflection;
using AForge.Video.DirectShow;
using System.Threading;
using System.Collections.ObjectModel;


namespace ScannerAndDistributionOfQRCodes
{
    public partial class ScannerQRCodeViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ImageSource _qRImage;
        [ObservableProperty]
        private ObservableCollection<string> _itemsPicker = [];

        //[ObservableProperty]
        //private Guest _guest;



        [ObservableProperty]
        private bool _isCameraLaunched = false;

        private string _currentDeviceCameraName;
        public string CurrentDeviceCameraName
        {
            get => _currentDeviceCameraName;
            set
            {
                SetProperty(ref _currentDeviceCameraName, value);
                _isChangesDeviceCamera = true;
            }
        }

        private bool _isChangesDeviceCamera = false;
        private bool _isConnecting = false;

        private ScannerQR _scannerQR;
        private IDecodeQRCode _decodeQRCode;
        private IEncodeQRCode _encodeQRCode;
        

        Dictionary<string, string> MonikerStringName = new();




        public ScannerQRCodeViewModel()
        {
            _scannerQR = new ScannerQR(UpdateQrCodeAsync);
            var filterInfoCollection = _scannerQR.GetVideoInputDevice();
            SetMonikerStringName(filterInfoCollection);
            SetItemsPicker();

            _decodeQRCode = new DecodeQRCode();
            _encodeQRCode = new EncodeQRCode();
            SetImageOfCameraOn();
        }


        private bool  ConnectingCamera()
        {
            if (CheckingConnectionAndChangingCamera())
                return false;

            var currentCameraName = GetMonikerString(CurrentDeviceCameraName);
            if (CameraNameValidityCheck(currentCameraName))
                return true;
       
                    
            _scannerQR.ConnectingCamera(currentCameraName);
            _isConnecting = true;
            _isChangesDeviceCamera = false;

            return false;
        }

        private string GetMonikerString(string currentDeviceCameraName)
        {
            if (string.IsNullOrEmpty(currentDeviceCameraName))
                return string.Empty;
            return MonikerStringName[ItemsPicker[int.Parse(currentDeviceCameraName)]];
        }
        private void SetItemsPicker()
        {
            for (int i = 0; i < MonikerStringName.Count; i++)
                ItemsPicker.Add(MonikerStringName.ElementAt(i).Key);
        }
        private void SetMonikerStringName(FilterInfoCollection filterInfoCollection)
        {
            for (int i = 0; i < filterInfoCollection.Count; i++)
                MonikerStringName[filterInfoCollection[i].Name] = filterInfoCollection[i].MonikerString;
        }

        private bool CheckingConnectionAndChangingCamera()
    => _isConnecting & !_isChangesDeviceCamera;

        private bool CameraNameValidityCheck(string currentCameraName)
        {
            if (!string.IsNullOrEmpty(currentCameraName))
                return false;
            Application.Current.MainPage.DisplayAlert("Уведомление", "Выберите камеру!", "ОK");
            return true;
        }
        private  bool CheckingAvailabilityOfCameras()
        {
            if (ItemsPicker.Count != 0)
                return false;
            Application.Current.MainPage.DisplayAlert("Уведомление", "Отсутствует модуль камеры!", "ОK");
            return true;
        }

        [RelayCommand]
        public  void StartScanner()
        {
            if (CheckingAvailabilityOfCameras())
                return;
           
            if (ConnectingCamera())
                return;

            if (IsCameraLaunched)
            {
                TurnOffCamera();
            }
            else
            {
                TurnOnCamera();
            }
           
        }
        private void TurnOnCamera()
        {
            _scannerQR.StartCamera();
            IsCameraLaunched = true;
        }
        private void TurnOffCamera()
        {
            SetImageOfCameraOn();
            _scannerQR.StopCamera();
            IsCameraLaunched = false;
        }

        private void SetImageOfCameraOn()
            =>QRImage = ImageSource.FromFile("camera_is_on.png");
       


        private void UpdateQrCodeAsync(object obj,NewFrameEventArgs eventArgs)
        {
            var bitmap = (Bitmap)eventArgs.Frame.Clone();
            using MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            var old = stream.ToArray();
            if (old is not null)
            {
                using Stream streami = new MemoryStream(old);
                QRImage = ImageSource.FromStream(() => streami);
                var result = _decodeQRCode.Decode(bitmap);
                if (!string.IsNullOrEmpty(result))
                {
                    ///camera_is_on.png

                    TurnOffCamera();
                }
            }
        }
       
    }
}
