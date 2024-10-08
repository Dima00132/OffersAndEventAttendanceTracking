﻿using AForge.Video;
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
using ScannerAndDistributionOfQRCodes.ViewModel;


namespace ScannerAndDistributionOfQRCodes
{


    public sealed partial class MainPage : ContentPage
    {
        private readonly MainViewModel _mainViewModel;

        public MainPage(MainViewModel mainViewModel)
        {

            InitializeComponent();
            BindingContext = mainViewModel;
            _mainViewModel = mainViewModel;
        }

        //protected override void OnDisappearing()
        //{
        //    _mainViewModel.OnUpdateDbService();
        //    base.OnDisappearing();
        //}

    }
    

}
