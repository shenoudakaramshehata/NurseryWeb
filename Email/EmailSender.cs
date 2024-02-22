using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Email
{

    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            //create the mail message 
            //MailMessage mail = new MailMessage();

            ////set the addresses 
            //mail.From = new MailAddress("info@alhadhanah.com"); //IMPORTANT: This must be same as your smtp authentication address.
            //mail.To.Add(email);

            ////set the content 
            //mail.Subject = subject;
            //mail.Body = "This is from system.net.mail using C sharp with smtp authentication.";
            ////send the message 
            //SmtpClient smtp = new SmtpClient("mail.alhadhanah.com");

            ////IMPORANT:  Your smtp login email MUST be same as your FROM address. 
            //NetworkCredential Credentials = new NetworkCredential("info@alhadhanah.com", "Complex@123");
            //smtp.UseDefaultCredentials = false;
            //smtp.Credentials = Credentials;
            //smtp.Port = 25;    //alternative port number is 8889
            //smtp.EnableSsl = true;
            //smtp.Send(mail);
            ////string fromMail = "nurserysite1@gmail.com";
            //string fromPassword = "pjtspklspvjigryn";
            string fromMail = "info@alhadhanah.com";
            string fromPassword = "Complex@123";
            //string fromPassword = "pjtspklspvjigryn";
            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.Body = $"<html><body>{htmlMessage}</body></html>";
            message.IsBodyHtml = true;
            message.To.Add(email);

            //var smtpClient = new SmtpClient("smtp.gmail.com")
            //{
            //    Port = 587,
            //    Credentials = new NetworkCredential(fromMail, fromPassword),
            //    EnableSsl = true,
            //};
            var smtpClient = new SmtpClient("mail.alhadhanah.com")
            {
                Port = 25,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            smtpClient.Send(message);
        }

        public Task SendEmailAsync(Message message)
        {
            throw new System.NotImplementedException();
        }
    }
}
