using AForge.Video;
using AForge.Video.DirectShow;
using Microsoft.Maui.ApplicationModel;
using System.Drawing;
using System.Windows;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using QRCoder;
using ZXing;
using IronBarCode;
using System;
using Microsoft.Maui.Controls;
using ZXing.QrCode.Internal;

using ZXing.Common;
using ZXing.Windows.Compatibility;
using System.Reflection;
using System.Runtime.InteropServices;


namespace ScannerAndDistributionOfQRCodes
{


    public partial class MainPage : ContentPage
    {
        int count = 0;


        public MainPage( ScannerQRCodeViewModel mainQR)
        {
            InitializeComponent();
            BindingContext = MainQR= mainQR;
            mainQR.MainPage = this;
            mainQR.Toma();
            //timer1 = Application.Current.Dispatcher.CreateTimer();
            //timer1.Interval = TimeSpan.FromSeconds(1);
            //timer1.Tick += Timer_Tick;

            //ScannerQRCodeViewModel = mainQR;
        }




        private void Button_Clicked_1(object sender, EventArgs e)
        {


            //Bitmap image;
        
          
            //var byr = (Bitmap)caemtra.Source;
            ////using (var ms = new MemoryStream(byr.Result))
            ////{
            ////    image = new Bitmap(ms);
            ////}

            //using (image)
            //{
            //    LuminanceSource source;
            //    source = new BitmapLuminanceSource(image);
            //    BinaryBitmap bitmap = new BinaryBitmap(new HybridBinarizer(source));
            //    Result result = new MultiFormatReader().decode(bitmap);
            //    if (result != null)
            //    {
            //        editor.Text = result.Text;
            //        _captureDevice.Stop();
            //        timer1.Stop();
            //    }
                
               
            //}

            //(sender, EventArgs.Empty);
        }

        public async void Timer_Tick(object? sender, EventArgs e)
        {
            if (caemtra.Source is not null)
            {

                var options = new ZXing.Common.DecodingOptions { PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE }, TryHarder = true };

                var reader = new ZXing.Windows.Compatibility.BarcodeReader()
                {
                    AutoRotate = false,
                    TryInverted = false,
                    Options = options
                };

                //use GlobalHistogramBinarizer for best result
                //var dreader = new BarcodeReader(null, null,
                //        ls => new GlobalHistogramBinarizer(ls)) { AutoRotate = false, TryInverted = false, Options = options };

                var by = ConvertImageSourceToBytesAsync(caemtra.Source);
                var result = reader.Decode(by.Result);

                if (result is not null)
                {
                    editor.Text = result.Text;
                    captureDevice.Stop();
                    timer1.Stop();
                }


            }

        }

        public async Task<Bitmap> ConvertImageSourceToBytesAsync(ImageSource imageSource)
        {
            Stream stream = await ((StreamImageSource)imageSource).Stream(CancellationToken.None);
            byte[] bytesAvailable = new byte[stream.Length];
            stream.Read(bytesAvailable, 0, bytesAvailable.Length);

            return new Bitmap(stream);
        }

        FilterInfoCollection infoCollection;
        VideoCaptureDevice captureDevice;
        private object locker = new object();
        private IDispatcherTimer timer1;

        public ScannerQRCodeViewModel MainQR { get; }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            infoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            captureDevice = new VideoCaptureDevice(infoCollection[0].MonikerString);
            captureDevice.NewFrame += MainQR.UpdateQrCodeAsync ;
            captureDevice.Start();
            //ScannerQRCodeViewModel.timer1.Start();
        }



        private void GenerateQRCode(string text)
        {
            if (string.IsNullOrEmpty(text))
                text = string.Empty;
            QRCodeGenerator generator = new QRCodeGenerator();
            QRCodeData codeData = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.L);
            PngByteQRCode qRCode = new PngByteQRCode(codeData);

            byte[] qrCodeBytes = qRCode.GetGraphic(20);

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                QrCodeImage.Source = ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));

            });

        }
        private async void CaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            var bitmap = (Bitmap)eventArgs.Frame.Clone();
            //MainThread.BeginInvokeOnMainThread(async () =>
            //{
                

           
                    

                    MemoryStream stream = new MemoryStream();
                    bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                    var old = stream.ToArray();
         
                if (old is not null)
                {



                    caemtra.Source = ImageSource.FromStream(() => new MemoryStream(old));
                //using var stream = new MemoryStream(imageBytes);
                //QRImage = ImageSource.FromStream(() => stream); // <--- this doesn't work

                // var stream = new MemoryStream(imageBytes);
                // QRImage = ImageSource.FromStream(() => stream); // <--- this works

                //QRImage = ImageSource.FromStream(() => new MemoryStream(imageBytes)); // <--- this works
                //var reader = new ZXing.Windows.Compatibility.BarcodeReader();
                //var res = reader.Decode((Bitmap)QRImage);
                }
            //});
        }

        private void editor_Completed(object sender, EventArgs e)
        {
            var text = string.Empty;

            if (!string.IsNullOrEmpty(editor.Text))
                text = editor.Text;
            GenerateQRCode(text);
        }

     



        //private async void CaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        //{

        //    var bitmap = (Bitmap)eventArgs.Frame.Clone();
        //    MemoryStream stream = new MemoryStream();
        //    bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);


        //    lock (locker)
        //    {
        //        //var memor = new MemoryStream(byt)
        //        //{
        //        //    Position = 0,
        //        //};
        //        image.Source = ImageSource.FromStream(() => stream);
        //    }


        //}





        //public static byte[] BitmapToBytes(Bitmap bitmap)
        //{

        //}


    }
    

}
