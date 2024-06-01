
using Microsoft.Maui.ApplicationModel.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Net;




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
        public static bool CheckEmailValidator(string mail)
            => !string.IsNullOrEmpty(mail) && CheckingEmailFormat(mail);





        //private static bool CheckSyntaxCheck(string mail)
        //{
        //    var emailValidatorSyntax = Email.EmailValidator.CheckSyntaxCheck(mail);
        //    return true;  
        //}

        private static bool CheckingEmailFormat(string mail)
            => System.Text.RegularExpressions.Regex.IsMatch(mail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        
    }
}
