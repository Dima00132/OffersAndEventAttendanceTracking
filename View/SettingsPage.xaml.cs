using Microsoft.Maui.Controls;
using ScannerAndDistributionOfQRCodes.ViewModel;
using System.Collections.Generic;

namespace ScannerAndDistributionOfQRCodes.View;

public partial class SettingsPage : ContentPage
{
    List<Frame> frames = new List<Frame>();
    public SettingsPage(SettingsViewModel settingsViewModel)
    {
        InitializeComponent();
        BindingContext = settingsViewModel;
        frames.AddRange([userFrame, mailServerFrame, domainDailFrame, uisenderGOFrame]);
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        CloseFrames();
        listMinu.IsVisible = !listMinu.IsVisible;
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
        foreach (var frame in frames.Where(x=>x!= dontCloseFrame))
            frame.IsVisible = false;
    }
}