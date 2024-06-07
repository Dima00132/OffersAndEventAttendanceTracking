
using Microsoft.Maui.ApplicationModel.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Net;
using Aspose.Email.Tools.Verifications;
using Aspose.Email;




namespace ScannerAndDistributionOfQRCodes.Data.Message
{



    public class InternetCS
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        public static bool IsConnectedToInternet()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }
    }



    public static class EmailValidator
    {


        public static bool CheckEmailValidatorAll(string emailAddress)
            => CheckingEmailFormat(emailAddress) && CheckingEmailFormat(emailAddress);

        public static bool CheckEmailDomain(string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress))
                return false;
            Aspose.Email.Tools.Verifications.EmailValidator ev = new();
            ev.Validate(emailAddress, out ValidationResult result);
            if (result.ReturnCode == ValidationResponseCode.ValidationSuccess)
                return true;
            return false;

        }

        public static bool CheckingEmailFormat(string emailAddress)
            => !string.IsNullOrEmpty(emailAddress) && System.Text.RegularExpressions.Regex.IsMatch(emailAddress, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        
    }
}

public enum SenderResponseCode
{
    MailSend = 0,
    MailAddressFormatError = 1,
    MailDomainFormatError = 2,
    NaN =3
}
