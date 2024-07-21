# Что такое - OffersAndEventAttendanceTracking
OffersAndEventAttendanceTracking - это приложения MAUI C# для Корпоративный университет Правительства Нижегородской области работающее на Windows. Приложение преднозначено для контроля посещения мероприятия за счет проверки гостей по qr - коду и ведения статистики псещения. В данной прриложение так же присутствует рассылки сообщений , таких как qr коды приглошенным гостям и сообщений разный собыий у Корпоративный университет Правительства Нижегородской области через массовую рассылку.

# Основуные библиотеки при создание приложжения
## Рассылка сообщений
-Для рассылки использовалась библиотека  [MailKit](https://github.com/jstedfast/MailKit) 

    public sealed class EmailYandexMessage(string subject, MessageText messageText, string receiverName, string toAddress, IMailAccount from, Stream sreamImage) : IEmailMessage, IImageMessage
    {
        public string Subject { get; } = subject;
        public MessageText MessageText { get; } = messageText;
        public string ReceiverName { get; } = receiverName;
        public IMailAccount From { get; } = from;
        public string ToAddress { get; } = toAddress;
        public Stream SreamImage { get; } = sreamImage;

        private MimeEntity GetMessageBody(BodyBuilder body)
        {
            var htmlImage = string.Empty;
            if (SreamImage is not null)
            {
                var imageFoot = body.LinkedResources.Add($"Image_{ToAddress}", SreamImage);
                imageFoot.ContentId = MimeUtils.GenerateMessageId();
                htmlImage = $"<img src=\"cid:{imageFoot.ContentId}\"/><br /><div style=\"border-top:3px solid #61028d\">&nbsp;";
            }
            body.HtmlBody = $@"<p>{ReceiverName}</p><br/><p>{MessageText.Text}</p>{htmlImage}</div><p>{MessageText.OrganizationData}</p>";
            //body.HtmlBody = $@"<p>{ReceiverName}</p><br/><p>{MessageText.Text}</p><img src=""cid:{imageFoot.ContentId}""/><br /><div style=""border-top:3px solid #61028d"">&nbsp;</div><p
            {MessageText.OrganizationData}</p>";
            return body.ToMessageBody();
        }

        public bool Send()
        {
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
            return true; 
        }
    }

