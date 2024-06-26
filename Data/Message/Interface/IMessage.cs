﻿using CommunityToolkit.Mvvm.ComponentModel;
using ScannerAndDistributionOfQRCodes.Data.Message;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ScannerAndDistributionOfQRCodes.Data.Message.Interface
{
    public interface IMessage
    {
        bool Send();
        string ToAddress { get; }
        IMailAccount From { get; }
    }
}
