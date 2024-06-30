namespace ScannerAndDistributionOfQRCodes.Model
{
    public sealed class StatisticsGuest
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string IsMessageSent { get; set; }
        public string IsVerifiedQRCode { get; set; }
        public string ArrivalTime { get; set; }

        public StatisticsGuest( string surname, string name, string patronymic, string isMessageSent, string isVerifiedQRCode, string arrivalTime)
        {
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            IsMessageSent = isMessageSent;
            IsVerifiedQRCode = isVerifiedQRCode;
            ArrivalTime = arrivalTime;
        }


    }
}
