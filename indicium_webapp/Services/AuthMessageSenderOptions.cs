using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Services
{
    public class AuthMessageSenderOptions
    {
        public string SmtpHost { get; set; }
        public string SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
    }
}
