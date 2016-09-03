using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdp_Net_proj.Services
{
    public interface IMailService
    {
        void DebugSendMail(string to, string from, string subject, string body);
        Task SendEmailAsync(string to, string from, string subject, string message);
    }
}
