using System;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using System.Text;

namespace indicium_webapp.Services
{
    class Mailer
    {
        //static void Main(string[] args)
        //{
        //    var ical = new iCalendar("Borrel", DateTime.Now.AddDays(7), DateTime.Now.AddDays(7).AddHours(1), "Daltonlaan 200\\, Utrecht", "Gezellig wat eten en een borrel.");

        //    SendEmailAsync(ical);

        //    Console.WriteLine("Done!");
        //    Console.ReadKey();

        //}

        static public async void SendEmailAsync(iCalendar ical)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Indicium", "info@indicium.tech"));
            emailMessage.To.Add(new MailboxAddress("Wesley de Kraker", "wesley.dekraker@student.hu.nl"));
            emailMessage.Subject = "Voorbeeld Event: " + ical.Summary;

            var bodyBuilder = new BodyBuilder();

            byte[] icalFile = Encoding.UTF8.GetBytes(ical.ToString());

            bodyBuilder.HtmlBody = "<h1>" + ical.Summary + "</h1> " +
                "<b>Beschrijving:</b> " + ical.Description + "<br>" +
                "<b>Wanneer:</b> " + ical.DateTimeStart.ToUniversalTime().ToString() + "<br>" +
                "<b>Waar:</b> " + ical.Location + "<br><br>" +
                "<i>Tip: klik op de bijlage om dit event toe tevoegen aan jouw agenda.<i>";

            bodyBuilder.Attachments.Add("event.ics", icalFile);

            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.transip.email", 465, SecureSocketOptions.SslOnConnect).ConfigureAwait(false);
                await client.AuthenticateAsync("info@indicium.tech", "wachtwoord").ConfigureAwait(false);
                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }
    }

    public class iCalendar
    {
        private const string companyName = "Indicium";
        private const string applicationName = "Event Planner";
        private const string dateFormat = "yyyyMMddTHHmmssZ";

        public iCalendar(string summary, DateTime dateTimeStart, DateTime dateTimeEnd, string location, string description)
        {
            Summary = summary;
            DateTimeStart = dateTimeStart;
            DateTimeEnd = dateTimeEnd;
            Location = location;
            Description = description;
        }

        public string Summary { get; private set; }

        public DateTime DateTimeStart { get; set; }

        public DateTime DateTimeEnd { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            string newLine = "\r\n";
            return "BEGIN:VCALENDAR" + newLine +
                "VERSION:2.0" + newLine +
                "PRODID:-//" + companyName + "//" + applicationName + "//EN" + newLine +
                "METHOD:PUBLISH" + newLine +
                "BEGIN:VEVENT" + newLine +
                "SUMMARY:" + Summary + newLine +
                "UID:" + Guid.NewGuid() + newLine +
                "DTSTART:" + DateTimeStart.ToUniversalTime().ToString(dateFormat) + newLine +
                "DTEND:" + DateTimeEnd.ToUniversalTime().ToString(dateFormat) + newLine +
                "DTSTAMP:" + DateTime.Now.ToUniversalTime().ToString(dateFormat) + newLine +
                "LOCATION:" + Location + newLine +
                "DESCRIPTION:" + Description + newLine +
                "END:VEVENT" + newLine +
                "END:VCALENDAR" + newLine;
        }
    }
}
