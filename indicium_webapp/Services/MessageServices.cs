using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using indicium_webapp.Models;
using System.Text;

namespace indicium_webapp.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private const string companyName = "Indicium";
        private const string applicationName = "Event Planner";
        private const string dateFormat = "yyyyMMddTHHmmssZ";

        public AuthMessageSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(Options.SmtpUsername));
            emailMessage.To.Add(new MailboxAddress(email));
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder();

            bodyBuilder.HtmlBody = message;

            emailMessage.Body = bodyBuilder.ToMessageBody();

            return Send(emailMessage);
        }

        public Task SendCalendarInviteAsync(string email, Activity activity)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(Options.SmtpUsername));
            emailMessage.To.Add(new MailboxAddress(email));
            emailMessage.Subject = "Inschrijving: " + activity.Name;

            var bodyBuilder = new BodyBuilder();

            byte[] icalFile = Encoding.UTF8.GetBytes(ActivityToIcal(activity));

            bodyBuilder.HtmlBody = "<h1>" + activity.Name + "</h1> " +
                "<b>Beschrijving:</b> " + activity.Description + "<br>" +
                "<b>Wanneer:</b> " + activity.StartDateTime.ToUniversalTime().ToString() + "<br>" +
                "<b>Waar:</b> " + "Daltonlaan 200" + "<br><br>" +
                "<i>Tip: klik op de bijlage om dit event toe tevoegen aan jouw agenda.<i>";

            bodyBuilder.Attachments.Add("event.ics", icalFile);

            emailMessage.Body = bodyBuilder.ToMessageBody();

            return Send(emailMessage);
        }

        private String ActivityToIcal(Activity activity)
        {
            string newLine = "\r\n";
            return "BEGIN:VCALENDAR" + newLine +
                "VERSION:2.0" + newLine +
                "PRODID:-//" + companyName + "//" + applicationName + "//EN" + newLine +
                "METHOD:PUBLISH" + newLine +
                "BEGIN:VEVENT" + newLine +
                "SUMMARY:" + activity.Name + newLine +
                "UID:" + Guid.NewGuid() + newLine +
                "DTSTART:" + activity.StartDateTime.ToUniversalTime().ToString(dateFormat) + newLine +
                "DTEND:" + activity.EndDateTime.ToUniversalTime().ToString(dateFormat) + newLine +
                "DTSTAMP:" + DateTime.Now.ToUniversalTime().ToString(dateFormat) + newLine +
                "LOCATION:" + "Daltonlaan 200" + newLine +
                "DESCRIPTION:" + activity.Description + newLine +
                "END:VEVENT" + newLine +
                "END:VCALENDAR" + newLine;
        }

        private Task Send(MimeMessage emailMessage)
        {
            var task = Task.Run(async () =>
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(Options.SmtpHost, Int32.Parse(Options.SmtpPort), SecureSocketOptions.SslOnConnect).ConfigureAwait(false);
                    await client.AuthenticateAsync(Options.SmtpUsername, Options.SmtpPassword).ConfigureAwait(false);
                    await client.SendAsync(emailMessage).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }
            });

            return task;
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}