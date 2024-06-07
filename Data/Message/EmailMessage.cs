using MimeKit.Utils;
using MimeKit;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using ScannerAndDistributionOfQRCodes.Data.Message.Interface;
using MailKit;
using MailKit.Security;
using Microsoft.Exchange.WebServices.Data;
using MailKit.Net.Pop3;
using MailKit.Net.Imap;
using Aspose.Email;
using Microsoft.Maui.ApplicationModel.Communication;
using Aspose.Email.Tools.Verifications;
using ScannerAndDistributionOfQRCodes.Model;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ScannerAndDistributionOfQRCodes.Data.Message
{
    public sealed class ErrorMessage<T>(T ErrorObject, string Message)
    {
        public T ErrorObject { get; } = ErrorObject;
        public string Message { get; } = Message;

        private static Dictionary<SenderResponseCode, string> _errorMessage = new Dictionary<SenderResponseCode, string>()
        {
            [SenderResponseCode.NaN] = "NaN",
            [SenderResponseCode.MailSend] = "Сообщение отправлено!",
            [SenderResponseCode.MailDomainFormatError] = "Неправильно указан домен!",
            [SenderResponseCode.MailAddressFormatError] = "Некорректно адрес почты!"
        };

        public static ErrorMessage<T> GetErrorMessage(T ErrorObject, SenderResponseCode senderResponseCode)
            =>new ErrorMessage<T>(ErrorObject, _errorMessage[senderResponseCode]);
        
    }

    [Serializable]
    public sealed class SendMailMessageException : Exception
    {
        public SendMailMessageException() { }

        public SendMailMessageException(string message)
            : base(message) { }

        public SendMailMessageException(string message, Exception inner)
            : base(message, inner) { }
    }



    public class EmailYandexMessage(string subject, MessageText messageText, string receiverName, string toAddress, IMailAccount from, Stream sreamImage) : IEmailMessage, IImageMessage
    {
        public string Subject { get; } = subject;
        public MessageText MessageText { get; } = messageText;
        public string ReceiverName { get; } = receiverName;
        public IMailAccount From { get; } = from;
        public string ToAddress { get; } = toAddress;
        public Stream SreamImage { get; } = sreamImage;

        private bool CheckValidMailAddress(string mailAddress,out SenderResponseCode senderResponseCode)
        {
            var isValidFormat = !EmailValidator.CheckingEmailFormat(mailAddress);
            var isValidDomain = !EmailValidator.CheckEmailDomain(mailAddress);
            senderResponseCode = isValidFormat ? SenderResponseCode.MailAddressFormatError : isValidDomain ? SenderResponseCode.MailDomainFormatError : SenderResponseCode.NaN;
            return isValidFormat | isValidDomain;
        }

        private MimeEntity GetMessageBody(BodyBuilder body)
        {
            var imageFoot = body.LinkedResources.Add("qr", SreamImage);
            imageFoot.ContentId = MimeUtils.GenerateMessageId();
            body.HtmlBody = $@"<p>{ReceiverName}</p><br/><p>{MessageText.Text}</p><img src=""cid:{imageFoot.ContentId}""/><br /><div style=""border-top:3px solid #61028d"">&nbsp;</div><p>{MessageText.OrganizationData}</p>";
            return body.ToMessageBody();
        }

        public SenderResponseCode Send()
        {
            if (CheckValidMailAddress(ToAddress, out SenderResponseCode senderResponse))
                return senderResponse;
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress($"{From.UserData.Surname} {From.UserData.Name} {From.UserData.Patronymic}", From.MailAddress));
                message.To.Add(new MailboxAddress(ReceiverName, ToAddress));
                message.Subject = Subject;
                message.Body = GetMessageBody(new BodyBuilder());
                using var client = new SmtpClient();
                client.Connect(From.MailServer.Server, From.MailServer.Port, From.MailServer.ConnectionProtection);
                client.DeliveryStatusNotificationType = DeliveryStatusNotificationType.Full;
                client.Authenticate(Encoding.UTF8, From.MailID, From.Password);
                client.Send(message);
            }
            catch (Exception ex)
            {
                throw new SendMailMessageException(ex.Message,ex);
            }
            return SenderResponseCode.MailSend; 
        }
    }
}
