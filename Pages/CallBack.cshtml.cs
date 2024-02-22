
using MailKit.Search;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NToastNotify;
using Nursery.Data;
using Nursery.Migrations;
using Nursery.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nursery.Pages
{

    public class CallBackModel : PageModel
    {
        public NurserySubscription nurserySubscription { set; get; }
        public List<NurseryImage> NurseryImages { set; get; }
        public string PaymentStatus { get; set; }
        public NurseryMember NurseryMember { set; get; }

        public static bool expired = false;


        private readonly UserManager<ApplicationUser> _userManager;
        private readonly NurseryContext _context;
        private readonly IHostingEnvironment _env;
        private readonly IToastNotification _toastNotification;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public ApplicationUser user { set; get; }

        public CallBackModel(UserManager<ApplicationUser> userManager, NurseryContext context, IHostingEnvironment env
                                        , IToastNotification toastNotification, IEmailSender emailSender, IConfiguration configuration)
        {
            _userManager = userManager;
            _context = context;
            _env = env;
            _toastNotification = toastNotification;
            _configuration = configuration;
            _emailSender= emailSender;
        }

        public async Task<ActionResult> OnGet(string tap_id)
        {
            try
            {


                if(tap_id is null )
                {
                    return RedirectToPage("SomethingwentError");
                }

                string Id = tap_id;

                var client = new RestClient("https://api.tap.company/v2/charges/" + Id);
                var request = new RestRequest();

                bool IsProd = bool.Parse(_configuration["IsProd"]);
                var TestToken = _configuration["TestToken"];
                var LiveToken = _configuration["LiveToken"];

                if (IsProd) // fattorah live
                {

                    request.AddHeader("authorization", LiveToken);

                }
                else // fattorah test
                {

                    request.AddHeader("authorization", TestToken);

                }
                //request.AddHeader("authorization", "Bearer sk_test_XKokBfNWv6FIYuTMg5sLPjhJ");
                request.AddParameter("undefined", "{}", ParameterType.RequestBody);
                RestResponse response = client.Execute(request);

                var DeserializedResponse = JsonConvert.DeserializeObject<JObject>(response.Content);

                var Status = DeserializedResponse["status"].ToString();
                PaymentStatus=Status.ToString();

                var MetaData = DeserializedResponse["metadata"];
                var OrderID = int.Parse((string)(MetaData?["order_id"]));
               



                int NurserySubId = OrderID;
                if (Status == "CAPTURED")
                {
                   
                    if (expired == false)
                    {
                        nurserySubscription = _context.NurserySubscription.Where(e => e.NurserySubscriptionId == NurserySubId).FirstOrDefault();
                        var planObj = _context.Plan.Where(e => e.PlanId == nurserySubscription.PlanId).FirstOrDefault();
                        var nurseryObj = _context.NurseryMember.Where(e => e.NurseryMemberId == nurserySubscription.NurseryId).FirstOrDefault();
                        user = await _userManager.FindByNameAsync(nurseryObj.Email);
                        NurseryImages = _context.NurseryImage.Where(e => e.NurseryId == nurseryObj.NurseryMemberId).ToList();
                        nurserySubscription.IsActive = true;
                        //nurserySubscription.PaymentID = paymentId;
                        double totalCost = nurserySubscription.Price.Value;
                        var UpdatedOrder = _context.NurserySubscription.Attach(nurserySubscription);
                        UpdatedOrder.State = EntityState.Modified;
                        _context.SaveChanges();
                        var webRoot = _env.WebRootPath;

                        var pathToFile = _env.WebRootPath
                               + Path.DirectorySeparatorChar.ToString()
                               + "Templates"
                               + Path.DirectorySeparatorChar.ToString()
                               + "EmailTemplate"
                               + Path.DirectorySeparatorChar.ToString()
                               + "Email.html";
                        var builder = new BodyBuilder();
                        using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                        {

                            builder.HtmlBody = SourceReader.ReadToEnd();

                        }
                        string messageBody = string.Format(builder.HtmlBody,
                           nurserySubscription.StartDate.Value.ToShortDateString(),
                           nurserySubscription.EndDate.Value.ToShortDateString(),
                           totalCost,
                           nurseryObj.NurseryTlAr.ToString(),
                           planObj.PlanTlAr

                           );

                        await _emailSender.SendEmailAsync(nurseryObj.Email, "Nursery Subscription", messageBody);
                        expired = true;
                    }
                    return Page();



                }

                nurserySubscription = _context.NurserySubscription.Where(e => e.NurserySubscriptionId == NurserySubId).FirstOrDefault();
                NurseryMember = _context.NurseryMember.FirstOrDefault(e => e.NurseryMemberId == nurserySubscription.NurseryId);
                user = await _userManager.FindByNameAsync(NurseryMember.Email);
                var subscribtion = _context.NurserySubscription.Where(e => e.NurseryId == NurseryMember.NurseryMemberId).ToList();
                _context.NurserySubscription.Remove(nurserySubscription);
                //_context.NurseryMember.Remove(NurseryMember);
                //await _userManager.DeleteAsync(user);
                _context.SaveChanges();
                expired = true;



                return Page();
            }
            catch(Exception ex)
            {
            




                  return RedirectToPage("SomethingWentWrong",new {Message=ex.Message});
                //return Page();
            }
           
        }


    }
}
