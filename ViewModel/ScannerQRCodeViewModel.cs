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

namespace ScannerAndDistributionOfQRCodes
{
    public partial class ScannerQRCodeViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ImageSource _qRImage;

        public MainPage MainPage;
        public IDispatcherTimer timer1;

        public ScannerQRCodeViewModel()
        {

           
        }


        //    //public IAsyncRelayCommand UpdateQrCodeCommand => new AsyncRelayCommand(UpdateQrCodeAsync);

        //    public async void CaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        //    {
        //        UpdateQrCodeAsync(eventArgs);


        public async void Toma()
        {
            //IDispatcherTimer timer;

            //timer1 = Threading.DispatcherTimer();
            //timer1.Interval = TimeSpan.FromMilliseconds(1000);
            //timer1.Tick += MainPage.Timer_Tick;
            

            //timer1 = Application.Current.Dispatcher.CreateTimer();
            //timer1.Interval = TimeSpan.FromSeconds(1);
            //timer1.Tick += MainPage.Timer_Tick;
        }

        //    }


        private async Task<byte[]> Get(Bitmap bitmap)
        {

            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            var old = stream.ToArray();
            var newB = new byte[old.Length];

            for (int i = 0; i < old.Length; i++)
            {
                newB[i] = old[i];
            }

            return newB;
        }

        //    private void GenerateQRCode(string text)
        //    {
        //        if (string.IsNullOrEmpty(text))
        //            text = string.Empty;
        //        QRCodeGenerator generator = new QRCodeGenerator();
        //        QRCodeData codeData = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.L);
        //        PngByteQRCode qRCode = new PngByteQRCode(codeData);

        //        byte[] qrCodeBytes = qRCode.GetGraphic(20);

        //        MainThread.BeginInvokeOnMainThread(async () =>
        //        {
        //            QrCodeImage.Source = ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));

        //        });

        //    }


        private void Button_Clicked_1(Bitmap image)
        {


           


        
            //using (var ms = new MemoryStream(byr.Result))
            //{
            //    image = new Bitmap(ms);
            //}

            using (image)
            {
                LuminanceSource source;
                source = new BitmapLuminanceSource(image);
                BinaryBitmap bitmap = new BinaryBitmap(new HybridBinarizer(source));
                Result result = new MultiFormatReader().decode(bitmap);
                if (result != null)
                {
                    //editor.Text = result.Text;
                    //_captureDevice.Stop();
                    timer1.Stop();
                }


            }

            //(sender, EventArgs.Empty);
        }
        public async void UpdateQrCodeAsync(object gg,NewFrameEventArgs eventArgs)
        {
            var bitmap = (Bitmap)eventArgs.Frame.Clone();
            //var imageBytes =  Get(bitmap);
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            var old = stream.ToArray();

            if (old is not null)
            {



                //QRImage = ImageSource.FromStream(() => new MemoryStream(by));
                //using var stream = new MemoryStream(imageBytes);
                //QRImage = ImageSource.FromStream(() => stream); // <--- this doesn't work

                //var stream = new MemoryStream(by.Result);
                //QRImage = ImageSource.FromStream(() => stream); // <--- this works

                 QRImage =  ImageSource.FromStream(() => new MemoryStream(old));
                Button_Clicked_1(bitmap);// <--- this works
                //var reader = new ZXing.Windows.Compatibility.BarcodeReader();
                //var res = reader.Decode((Bitmap)QRImage);
            }
        }
    }
}
