﻿using MimeKit.Utils;
using MimeKit;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using ScannerAndDistributionOfQRCodes.Data.Message.Interface;

namespace ScannerAndDistributionOfQRCodes.Data.Message
{
    public class ErrorMessage<T>(T ErrorObject, string Message)
    {
        public T ErrorObject { get; } = ErrorObject;
        public string Message { get; } = Message;
    }

    [Serializable]
    public class SendMailMessageException : Exception
    {
        public SendMailMessageException() { }

        public SendMailMessageException(string message)
            : base(message) { }

        public SendMailMessageException(string message, Exception inner)
            : base(message, inner) { }
    }

    public class EmailYandexMessage : IEmailMessage, IImageMessage
    {
        public EmailYandexMessage(string text, string subject, string receiverName, string toAddress, IMailAccount from, Stream sreamImage)
        {
            Text = text;
            Subject = subject;
            ReceiverName = receiverName;
            From = from;
            ToAddress = toAddress;
            SreamImage = sreamImage;
        }

        public string Text { get; }
        public string Subject { get; }
        public string ReceiverName { get; }
        public IMailAccount From { get; }
        public string ToAddress { get; }
        public Stream SreamImage { get; }

        public bool Send()
        {
            try
            {
                var message = new MimeMessage();

                //From.MailAddress
                message.From.Add(new MailboxAddress($"{From.UserData.Surname} {From.UserData.Name} {From.UserData.Patronymic}", From.MailAddress));
                message.To.Add(new MailboxAddress(ReceiverName, ToAddress));
                message.Subject = Subject;

                var body = new BodyBuilder();
                var imageFoot = body.LinkedResources.Add("qr", SreamImage);
                imageFoot.ContentId = MimeUtils.GenerateMessageId();
                body.HtmlBody = $@"<img src=""cid:{imageFoot.ContentId}""/><br /><div style=""border-top:3px solid #61028d"">&nbsp;</div><p>{Text}</p>";
                message.Body = body.ToMessageBody();

                using var client = new SmtpClient();
                client.Connect("smtp.yandex.ru", 465, true);

                //From.MailAddress;
                // From.Password;
                client.Authenticate(From.MailAddress, From.Password);

                client.Send(message);
                client.Disconnect(true);
            }
            catch (Exception ex)
            {

                throw new SendMailMessageException(ex.Message,ex);
            }
            return true;
            
        }

    }
}
