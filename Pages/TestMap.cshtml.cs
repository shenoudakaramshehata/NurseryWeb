using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Nursery.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Nursery.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.IO;
using MimeKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System.Net;
using System.Net.Mail;
namespace Nursery.Pages
{
    public class TestMapModel : PageModel
    {
        private readonly NurseryContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        public NurserySubscription nurserySubscription { set; get; }
        public NurseryMember NurseryMember { set; get; }
        public List<NurseryImage> NurseryImages { set; get; }
        public ApplicationUser user { set; get; }
        private IHostingEnvironment _env;
        private readonly IConfiguration _configuration;
        FattorhResult FattoraResStatus { set; get; }
        public static bool expired = false;
        private readonly ILogger<FattorahSuccessModel> _logger;
        string res { set; get; }

        public TestMapModel(ILogger<FattorahSuccessModel> logger, NurseryContext context, IEmailSender emailSender, UserManager<ApplicationUser> userManager, IHostingEnvironment env, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
            _env = env;
            _configuration = configuration;
            _logger = logger;
        }
        
       
        public void OnGet()
        {
            ////var webRoot = _env.WebRootPath;

            ////var pathToFile = _env.WebRootPath
            ////       + Path.DirectorySeparatorChar.ToString()
            ////       + "Templates"
            ////       + Path.DirectorySeparatorChar.ToString()
            ////       + "EmailTemplate"
            ////       + Path.DirectorySeparatorChar.ToString()
            ////       + "Email.html";
            ////var builder = new BodyBuilder();
            ////using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            ////{

            ////    builder.HtmlBody = SourceReader.ReadToEnd();

            ////}
            ////var nurserySubscriptionStartDate = "11-03-2023";
            ////var nurserySubscriptionEndDate = "11-05-2024";
            ////var totalCost = 35;
            ////var nurseryObjNurseryTlAr = "ليفربول";
            ////var planObjPlanTlAr = "سنة + شهرين مجاني";

            ////string messageBody = string.Format(builder.HtmlBody,
            ////   nurserySubscriptionStartDate,
            ////   nurserySubscriptionEndDate,
            ////   totalCost,
            ////   nurseryObjNurseryTlAr,
            ////   planObjPlanTlAr

            ////   );
            ////var nurseryObjEmail = "Liverpoolsschoolkw@gmail.com";
            //var nurseryObjEmail = "shenouda0128992@gmail.com";
            ////var nurseryObjEmail = "mail@alhadhanah.com";
            //var messageBody = "hello";
            //await _emailSender.SendEmailAsync(nurseryObjEmail, "Nursery Subscription", messageBody);
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            ////create the mail message
            ///
           // System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            //create the mail message
                MailMessage mail = new MailMessage();

            //set the addresses
                mail.From = new MailAddress("info@alhadhanah.com"); //IMPORTANT: This must be same as your smtp authentication address.
            mail.To.Add("shenouda0128992@gmail.com");

            //set the content
                mail.Subject = "Nursery Subscribtion";
            mail.Body = "This is from system.net.mail using C sharp with smtp authentication.";
            //send the message
            SmtpClient smtp = new SmtpClient("mail.alhadhanah.com");

        //IMPORANT: Your smtp login email MUST be same as your FROM address. 
                NetworkCredential Credentials = new NetworkCredential("info@alhadhanah.com", "Complex@123");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = Credentials;
            smtp.Port = 25;    //alternative port number is 8889
            smtp.EnableSsl = false;
            smtp.Send(mail);


            //return Page();
        }




    }
}

  
