
using Microsoft.Maui.ApplicationModel.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ScannerAndDistributionOfQRCodes.Model.Message
{
    public static class EmailValidator
    {
        public static bool CheckEmailValidator(string mail)
            =>!string.IsNullOrEmpty(mail) && CheckingEmailFormat(mail); 





        //private static bool CheckSyntaxCheck(string mail)
        //{
        //    var emailValidatorSyntax = Email.EmailValidator.CheckSyntaxCheck(mail);
        //    return true;  
        //}

        private static bool CheckingEmailFormat(string mail)
            => System.Text.RegularExpressions.Regex.IsMatch(mail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        //private static bool IsEmailValid(string mail)
        //{
        //    var emailValid = EmailValidator.IsEmailValid(mail);
        //    return emailValid;
        //}
    }
}
