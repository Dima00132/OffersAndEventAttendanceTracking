using CommunityToolkit.Mvvm.ComponentModel;
using ScannerAndDistributionOfQRCodes.Model.QRCode;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Exchange.WebServices.Data;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using MailKit.Security;
using MimeKit.Utils;
using ScannerAndDistributionOfQRCodes.Model.Message;


namespace ScannerAndDistributionOfQRCodes.Model
{

    [Table("guest")]
    public partial class Guest:ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("scheduled_event_id")]
        [ForeignKey(typeof(ScheduledEvent))]
        public int ScheduledEventId { get; set; }

        
        private string _surname;
        public string Surname  
        { 
            get => _surname;
            set=>SetProperty(ref _surname, value); 
        }


        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _patronymic;
        public string Patronymic
        {
            get => _patronymic;
            set => SetProperty(ref _patronymic, value);
        }
    
        private string _mail;
        public string Mail
        {
            get => _mail;
            set => SetProperty(ref _mail, value);
        }
        public string QRHashCode { get;  set; }

        public bool IsMessageSent { get; set; }

        private Image _qRCodeImge;

        public Guest()
        {
        }

        public Guest SetSurname(string surname)
        {
            Surname = RemoveSpaces(surname);
            return this;
        }
        public Guest SetName(string name)
        {
            Name =  RemoveSpaces(name);
            return this;
        }
        public Guest SetPatronymic(string patronymic)
        {
            Patronymic = RemoveSpaces(patronymic);
            return this;
        }
        public Guest SetMail(string mail)
        {

            if (!string.IsNullOrEmpty(Mail))
                IsMessageSent = false;
            var mailWithoutSpaces = RemoveSpaces(mail);
            //Проверка валидности посты
            Mail = mailWithoutSpaces;
            QRHashCode = GenerateQRHashCode();
            return this;
        }

        private string GenerateQRHashCode()
        {
            var uniqueQR = GeneratorUniqueQRHashCode
                .Generate(Name, Surname, Patronymic, Mail, DateTime.Now.ToString());
            return uniqueQR;
        }

        private string RemoveSpaces(string value)
            =>value.Replace(" ", "");

        public EventHandler GetUpSubscriptionForSendingMessages()
            => SendingMessages;

        private void SendingMessages(object obj,EventArgs eventArgs)
        {

            if (obj is ScheduledEvent scheduled) 
            {

                var encodeQRCode = new EncodeQRCode();
                // _qRCodeImge = encodeQRCode.Encode(QRHashCode);

                var stream = encodeQRCode.EncodeStream(QRHashCode);

                var emailSetnd = new EmailYandexMessage(scheduled.MessageText, scheduled.NameEvent, $"{Surname} {Name} {Patronymic}", Mail,
                    new MailAccount("TestMailSendr@yandex.ru", "cwufaysygkohokyr",new UserData("1","1","1")), stream);
            ///
            IsMessageSent = emailSetnd.Send(); 
            }
        }


        //private AlternateView GetEmbeddedImage(Stream stream)
        //{
        //    LinkedResource res = new LinkedResource(stream);
        //    res.ContentId = Guid.NewGuid().ToString();
        //    string htmlBody = @"<img src='cid:" + res.ContentId + @"'/>";
        //    AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
        //    alternateView.LinkedResources.Add(res);
        //    return alternateView;
        //}

        //private bool Send(Stream stream)
        //{
        //    try
        //    {
        //        var message = new MimeMessage();
        //        message.From.Add(new MailboxAddress("От органезаторов", "TestMailSendr@yandex.ru"));
        //        message.To.Add(new MailboxAddress($"{Surname} {Name} {Patronymic}",Mail));
        //        message.Subject = NameEvent;

        //        var body = new BodyBuilder();
        //        //{
        //        //    HtmlBody = $"<html><body>Test messe</body></html>"
        //        //};
                

        //        var imageFoot = body.LinkedResources.Add("qr",stream);

        //        imageFoot.ContentId = MimeUtils.GenerateMessageId();
        //        body.HtmlBody = $@"<img src=""cid:{imageFoot.ContentId}"" /><br /><div style=""border-top:3px solid #61028d"">&nbsp;</div><p>{(object)MessageText}</p>";


        //        message.Body = body.ToMessageBody();



        //        using var client = new SmtpClient();
        //        client.Connect("smtp.yandex.ru", 465, true);

        //        // Note: only needed if the SMTP server requires authentication cwufaysygkohokyr
        //        client.Authenticate("TestMailSendr@yandex.ru", "cwufaysygkohokyr");

        //        client.Send(message);
        //        client.Disconnect(true);




        //        ////smtp.yandex.ru = определяет сам про почте 
        //        //SmtpClient smtpClient = new SmtpClient("smtp.yandex.com.tr", 465);
        //        //smtpClient.UseDefaultCredentials = true ;
        //        //smtpClient.EnableSsl = true;



        //        ////mlrulbmzwliwnpqf

        //        ////"TestMailSendr@yandex.ru" почта через аккаунт 
        //        //System.Net.NetworkCredential baseAuthenticationInfo =
        //        //    new System.Net.NetworkCredential("TestMailSendr@yandex.ru", "mlrulbmzwliwnpqf");
        //        //smtpClient.Credentials = baseAuthenticationInfo;

        //        ////"TestMailSendr@yandex.ru" почта через аккаунт 
        //        //MailAddress from = new MailAddress("TestMailSendr@yandex.ru", "От органезаторов");
        //        //MailAddress to = new MailAddress("dima79346@gmail.com", "Грстю");

        //        //MailMessage message = new MailMessage(from, to);

        //        ////куда придет ответ 
        //        //MailAddress replyTo = new MailAddress("TestMailSendr@yandex.ru");
        //        //message.ReplyToList.Add(replyTo);

        //        ////тема определяется от мероприятия 
        //        //message.Subject = "Test MailMessage";
        //        //message.SubjectEncoding = System.Text.Encoding.UTF8;

        //        //message.Body = "Текст прихода на события";
        //        //message.BodyEncoding = System.Text.Encoding.UTF8;
        //        //message.IsBodyHtml = true;

        //        //message.Priority = MailPriority.Normal;

        //        //message.AlternateViews.Add(GetEmbeddedImage(stream));


        //        //smtpClient.Send(message);
        //    }
        //    catch (Exception)
        //    {

        //        return false;
        //    }  
        //    return true;
        //}
    }
}
