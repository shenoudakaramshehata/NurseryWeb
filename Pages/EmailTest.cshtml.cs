using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System.Net;
using System.Net.Mail;

namespace Nursery.Pages
{
    public class EmailTestModel : PageModel
    {
        public void OnGet()
        {
			System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
			//create the mail message 
			MailMessage mail = new MailMessage();

			//set the addresses 
			mail.From = new MailAddress("info@alhadhanah.com"); //IMPORTANT: This must be same as your smtp authentication address.
			mail.To.Add("shenouda0128992@gmail.com");

			//set the content 
			mail.Subject = "This is an email";
			mail.Body = "This is from system.net.mail using C sharp with smtp authentication.";
			//send the message 
			SmtpClient smtp = new SmtpClient("mail.alhadhanah.com");

			//IMPORANT:  Your smtp login email MUST be same as your FROM address. 
			NetworkCredential Credentials = new NetworkCredential("info@alhadhanah.com", "Complex@123");
			smtp.UseDefaultCredentials = false;
			smtp.Credentials = Credentials;
			smtp.Port = 25;    //alternative port number is 8889
			smtp.EnableSsl = false;
			smtp.Send(mail);
		}
    }
}
