using indicium_webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Services
{
    public interface IEmailSender
    {
        AuthMessageSenderOptions Options { get; }

        Task SendEmailAsync(string email, string subject, string message);
        Task SendCalendarInviteAsync(string email, Activity activity);
    }
}
