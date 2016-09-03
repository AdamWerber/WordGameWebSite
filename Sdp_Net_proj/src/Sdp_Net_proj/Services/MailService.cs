using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdp_Net_proj.Services
{
    public class MailService : IMailService
    {
        public void DebugSendMail(string to, string from, string subject, string body)
        {
            Debug.WriteLine($"Sending Mail: To: {to} From: {from} Subject: {subject}");

        }

        public async Task SendEmailAsync(string to, string from, string subject, string message)
        {
            var emailMessage = new MimeMessage(); //  represent the new mail will be sending

            emailMessage.From.Add(new MailboxAddress("", from));
            emailMessage.To.Add(new MailboxAddress("Web Site Mail", to));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = "The sender mail is: " + from + "\n\n Message:\n" + message };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587);

                client.AuthenticationMechanisms.Remove("XOAUTH2");

                await client.AuthenticateAsync("WebSiteMailHandler@gmail.com", "@abRZ!#8Rr").ConfigureAwait(false);

                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }
    }
}
