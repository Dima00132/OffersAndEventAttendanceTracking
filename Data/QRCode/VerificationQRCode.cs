using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using SQLiteNetExtensions.Attributes;
using DocumentFormat.OpenXml.Wordprocessing;
using ScannerAndDistributionOfQRCodes.Model;


namespace ScannerAndDistributionOfQRCodes.Data.QRCode
{
    [Table("verified_qr_code")]
    public partial class VerificationQRCode : ObservableObject
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

        public VerificationQRCode(User user, string mailAddress)
        {
            QRHashCode = GenerateQRHashCode(user, mailAddress);
        }

        public VerificationQRCode()
        {
        }

        private string GenerateQRHashCode(User user, string mailAddress)
            => GeneratorUniqueQRHashCode
                .Generate(user.Name, user.Surname, user.Patronymic, mailAddress);

        public bool CompareQRHashCode(string hash) =>
            QRHashCode.Equals(hash);

        public override bool Equals(object? obj)
        {
            if (obj is VerificationQRCode verificationQRCode)
                return verificationQRCode.QRHashCode.Equals(QRHashCode) & IsVerifiedQRCode == verificationQRCode.IsVerifiedQRCode;
            return false;
        }
    }
}
