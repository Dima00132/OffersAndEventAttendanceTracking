﻿using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using SQLiteNetExtensions.Attributes;
using DocumentFormat.OpenXml.Wordprocessing;
using ScannerAndDistributionOfQRCodes.Model;
using ScannerAndDistributionOfQRCodes.Data.Message.Mail;
using System;


namespace ScannerAndDistributionOfQRCodes.Data.QRCode
{
    [Table("verified_qr_code")]
    public sealed partial class VerificationQRCode : ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("guest_id")]
        [ForeignKey(typeof(Guest))]
        public int GuestId { get; set; }

        private bool _isVerifiedQRCode;
        public bool IsVerifiedQRCode
        {
            get => _isVerifiedQRCode;
            set => SetProperty(ref _isVerifiedQRCode, value);
        }
        public string QRHashCode { get; set; }

        public VerificationQRCode(User user, Mail mail)
        {
            QRHashCode = GenerateQRHashCode(user, mail.MailAddress);
        }

        public VerificationQRCode()
        {
        }

        public Stream GetStreamEncodeQRCode()
            =>EncodeQRCode.EncodeStream(QRHashCode);
        
        public void Change(User user, Mail mail)
        {
            QRHashCode = GenerateQRHashCode(user, mail.MailAddress);
        }

        private string GenerateQRHashCode(User user, string mailAddress)
            => GeneratorUniqueQRHashCode
                .Generate(user.Name, user.Surname, user.Patronymic, mailAddress);

        public bool CompareQRHashCode(string hash) =>
            QRHashCode.Equals(hash);

        public override bool Equals(object obj)
        {
            if (obj is VerificationQRCode verificationQRCode)
            {
                if (verificationQRCode.QRHashCode is null | verificationQRCode.IsVerifiedQRCode  ==  null)
                    return false;
                return verificationQRCode.QRHashCode.Equals(QRHashCode) & IsVerifiedQRCode == verificationQRCode.IsVerifiedQRCode;
            }
                
            return false;
        }

        //public override int GetHashCode()
        //{
        //    if()
        //    var qRHashCode = QRHashCode.GetHashCode();
        //    var verifiedQRCode = IsVerifiedQRCode.GetHashCode();
        //    unchecked
        //    {
        //        return qRHashCode + verifiedQRCode;
        //    } 
        
    }
}
