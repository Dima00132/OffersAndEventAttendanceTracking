using AForge.Video;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Drawing;
using ScannerAndDistributionOfQRCodes.ViewModel.Base;
using AForge.Video.DirectShow;
using System.Collections.ObjectModel;
using ScannerAndDistributionOfQRCodes.Model;
using ScannerAndDistributionOfQRCodes.Service.Interface;
using ScannerAndDistributionOfQRCodes.Data.QRCode;



namespace ScannerAndDistributionOfQRCodes
{
    public sealed partial class ScannerQRCodeViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ImageSource _qRImage;
        [ObservableProperty]
        private ObservableCollection<string> _itemsPicker = [];

        [ObservableProperty]
        private Guest _guest;

        [ObservableProperty]
        private ScheduledEvent _scheduledEvent;

        [ObservableProperty]
        private bool _isEditor = false;

        [ObservableProperty]
        private bool _isCameraLaunched = false;

        private int _currentDeviceCameraName;
        public int CurrentDeviceCameraName
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
        private readonly ScannerQR _scannerQR;
        private Dictionary<string, string> _monikerStringName = [];
        private readonly ILocalDbService _localDbService;

        public ScannerQRCodeViewModel(ILocalDbService localDbService)
        {
            _scannerQR = new ScannerQR(UpdateQrCode);
            var filterInfoCollection = _scannerQR.GetVideoInputDevice();
            SetMonikerStringName(filterInfoCollection);
            SetItemsPicker();
            SetImageOfCameraOn();
            _localDbService = localDbService;
        }

        public override Task OnNavigatingToAsync(object parameter, object parameterSecond = null)
        {
            if (parameter is ScheduledEvent scheduledEvent)
            {
                ScheduledEvent = scheduledEvent;
                CountHowManyGuestsHaveArrived();
            }
            return base.OnNavigatingToAsync(parameter);
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

        private string GetMonikerString(int currentDeviceCameraName)
        {
            if (currentDeviceCameraName == -1)
                return string.Empty;
            return _monikerStringName[ItemsPicker[currentDeviceCameraName]];
        }
        private void SetItemsPicker()
        {
            for (int i = 0; i < _monikerStringName.Count; i++)
                ItemsPicker.Add(_monikerStringName.ElementAt(i).Key);
        }
        private void SetMonikerStringName(FilterInfoCollection filterInfoCollection)
        {
            for (int i = 0; i < filterInfoCollection.Count; i++)
                _monikerStringName[filterInfoCollection[i].Name] = filterInfoCollection[i].MonikerString;
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
            TurnCamera(IsCameraLaunched);
        }

        private void TurnCamera(bool turnOnOrOff)
        {
            IsCameraLaunched = turnOnOrOff ? TurnOffCamera() : TurnOnCamera();
        }

        private bool TurnOnCamera()
        {
            _scannerQR.StartCamera();
            return true;
        }
        private bool TurnOffCamera()
        {
            SetImageOfCameraOn();
            _scannerQR.StopCamera();    
            return false;
        }

        public void Close() 
            => IsCameraLaunched = TurnOffCamera();

        private void SetImageOfCameraOn()
            =>QRImage = ImageSource.FromFile("camera_is_on.png");


        public void CountHowManyGuestsHaveArrived()
        {
            ScheduledEvent.CountArrivedGuests = ScheduledEvent.Guests.Count(x => x.VrificatQRCode.IsVerifiedQRCode);
        }


        public RelayCommand BackCommand => new(() =>
        {
            IsEditor = false;
        });

        public RelayCommand CnfirmCommand => new(() =>
        {
            IsEditor = false;
            CountHowManyGuestsHaveArrived();
            Guest.VrificatQRCode.IsVerifiedQRCode = true;
            Guest.ArrivalTime = DateTime.Now;
            ScheduledEvent.CountArrivedGuests++;
            _localDbService.Update(Guest.VrificatQRCode);
            _localDbService.Update(Guest);
            _localDbService.Update(ScheduledEvent);
        });

        private void UpdateQrCode(object obj,NewFrameEventArgs eventArgs)
        {
            var bitmap = (Bitmap)eventArgs.Frame.Clone();
            using MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            var old = stream.ToArray();
            if (old is not null)
            {

                using Stream streamImage = new MemoryStream(old);
                QRImage = ImageSource.FromStream(() => streamImage);
                var result = DecodeQRCode.Decode(bitmap);
                if (!string.IsNullOrEmpty(result))
                {
                    SearchByQRHash(result);    
                    IsCameraLaunched = TurnOffCamera();

                }
            }
        }

        private void CheckForRepeatedEventAttendance(VerificationQRCode verificationQR)
        {
            if (verificationQR.IsVerifiedQRCode)
               Application.Current.MainPage.DisplayAlert("Предепреждение", "Данный гость уже присутствует на мероприятие!", "Ок");
        }

        private bool SearchByQRHash(string value)
        {
            if(ScheduledEvent.SearchForGuestByQRHashCode(value) is Guest guest)
            {
                Guest = guest;
                MainThread.BeginInvokeOnMainThread(()=>CheckForRepeatedEventAttendance(Guest.VrificatQRCode));
                return IsEditor = true;
            }
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage.DisplayAlert("Предепреждение", $"Данного QR-code нет в списке \n {value}", "Ок");
            });
            return false;
        }
    }
}
