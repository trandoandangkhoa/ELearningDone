using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLearning.Application.Email
{
    public interface IEmailSender
    {
        void SendEmail(Message message, string from, string nameFrom);
        void ReplyEmail(Message message, string from, string nameFrom, string to);
    }
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly IConfiguration _configuration;
        public EmailSender(EmailConfiguration emailConfig, IConfiguration configuration)
        {
            _emailConfig = emailConfig;
            _configuration = configuration;
        }

        public void ReplyEmail(Message message, string from, string nameFrom, string to)
        {
            var emailMessage = CreateReplyMessage(message, from, nameFrom, to);
            Send(emailMessage);
        }
        private MimeMessage CreateReplyMessage(Message message, string from, string nameFrom, string to)
        {
            var emailMessage = new MimeMessage();
            _emailConfig.From = _configuration.GetValue<string>("EmailConfiguration:From");
            emailMessage.From.Add(new MailboxAddress("Booking Calender", from));
            emailMessage.To.Add(MailboxAddress.Parse(to));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };

            var bodyBuilder = new BodyBuilder { HtmlBody = string.Format(message.Content) };

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }
        public void SendEmail(Message message, string from, string nameFrom)
        {
            var emailMessage = CreateEmailMessage(message, from, nameFrom);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message, string from, string nameFrom)
        {
            var emailMessage = new MimeMessage();
            _emailConfig.From = from;
            emailMessage.From.Add(new MailboxAddress(nameFrom, from));
            emailMessage.To.Add(MailboxAddress.Parse(_configuration.GetValue<string>("EmailConfiguration:To")));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };

            return emailMessage;
        }
        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    client.Send(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
