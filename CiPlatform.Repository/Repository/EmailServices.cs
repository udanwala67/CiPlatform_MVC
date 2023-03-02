using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CiPlatform.Repository.Repository
{
    public class EmailServices
    {
        public void SendEmailAsync(string remail, string subject, string message)
        {
            MailMessage mailMessage = new MailMessage();
            var senderMail = "npsmtp217@gmail.com";
            mailMessage.From = new MailAddress(senderMail);
            mailMessage.To.Add(remail.ToString());
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(senderMail, "xkeufuuvbmvupdgq");
            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);
        }
    }
}

