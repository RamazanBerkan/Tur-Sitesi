using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ArgedeSP.Contracts.Helpers.Mail
{
    public class EmailSender : IEmailSender2
    {
        private string host;
        private int port;
        private bool enableSSL;
        private string userName;
        private string password;

        // Get our parameterized configuration
        public EmailSender(string host, int port, bool enableSSL, string userName, string password)
        {
            this.host = host;
            this.port = port;
            this.enableSSL = enableSSL;
            this.userName = userName;
            this.password = password;
        }

        // Use our configuration to send the email by using SmtpClient
        public Task SendEmailAsync(string email, string subject, string htmlMessage, string projectName)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };

            var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(userName, password),
                EnableSsl = enableSSL
            };


            MailAddress from = new MailAddress(userName, projectName);
            MailAddress to = new MailAddress(email, projectName);

            MailMessage mailMessage = new MailMessage(from, to);
            mailMessage.Body = htmlMessage;
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;

            return client.SendMailAsync(
                mailMessage
            );
        }
    }
    public interface IEmailSender2
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage,string projectName);
    }
}
