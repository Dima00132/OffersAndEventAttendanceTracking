using Microsoft.Maui.Controls;
using ScannerAndDistributionOfQRCodes.ViewModel;
using System.Collections.Generic;

namespace ScannerAndDistributionOfQRCodes.View;

public sealed partial class SettingsPage : ContentPage
{
    private static readonly List<Frame> _frames = [];
    
    public SettingsPage(SettingsViewModel settingsViewModel)
    {
        InitializeComponent();
        BindingContext = settingsViewModel;
        _frames.AddRange([userFrame, mailServerFrame, domainDailFrame, uisenderGOFrame]);
       
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        CloseFrames();
        listMinu.IsVisible = !listMinu.IsVisible;

        if (listMinu.IsVisible)
            image.SetAppTheme<FileImageSource>(Image.SourceProperty, "caret_up_black.png", "caret_up_white.png");
        else
            image.SetAppTheme<FileImageSource>(Image.SourceProperty, "caret_down_black.png", "caret_down_white.png");
        //Light = caret_up_black.png, Dark = caret_up_white.png
    }

    private void User_TextCell_Tapped(object sender, EventArgs e)
    {
        CloseFrames(userFrame);
        userFrame.IsVisible = true;
    }
    private void Mail_Server_TextCell_Tapped(object sender, EventArgs e)
    {
        CloseFrames(mailServerFrame);
        mailServerFrame.IsVisible = true;
    }
    private void Domain_Dail_TextCell_Tapped(object sender, EventArgs e)
    {
        CloseFrames(domainDailFrame); 
        domainDailFrame.IsVisible = true;
    }
    private void Unisender_GO_TextCell_Tapped(object sender, EventArgs e)
    {
        CloseFrames(uisenderGOFrame);
        uisenderGOFrame.IsVisible = true;
    }

    private void Cansel_Frame_Button_Clicked(object sender, EventArgs e)
    {
        CloseFrames();
    }

    private void CloseFrames(Frame dontCloseFrame = null)
    {
        foreach (var frame in _frames.Where(x=>x!= dontCloseFrame))
            frame.IsVisible = false;
    }
}