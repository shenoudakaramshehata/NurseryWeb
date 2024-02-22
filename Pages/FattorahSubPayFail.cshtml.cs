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

namespace Nursery.Pages
{
    public class FattorahSubPayFailModel : PageModel
    {
        private readonly NurseryContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        public NurseryMember NurseryMember { set; get; }
        public NurserySubscription nurserySubscription { set; get; }

        public List<NurseryImage> NurseryImages { set; get; }
        public ApplicationUser user { set; get; }
        private IHostingEnvironment _env;
        private readonly IConfiguration _configuration;
        FattorhResult FattoraResStatus { set; get; }
        public static bool expired = false;
        string res { set; get; }
        private readonly ILogger<FattorahSubPayFailModel> _logger;
        public FattorahSubPayFailModel(ILogger<FattorahSubPayFailModel> logger,NurseryContext context, IEmailSender emailSender, UserManager<ApplicationUser> userManager, IHostingEnvironment env, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
            _env = env;
            _configuration = configuration;
            _logger = logger;
        }
        public FattorahPaymentResult fattorahPaymentResult { get; set; }
        static string token = "rLtt6JWvbUHDDhsZnfpAhpYk4dxYDQkbcPTyGaKp2TYqQgG7FGZ5Th_WD53Oq8Ebz6A53njUoo1w3pjU1D4vs_ZMqFiz_j0urb_BH9Oq9VZoKFoJEDAbRZepGcQanImyYrry7Kt6MnMdgfG5jn4HngWoRdKduNNyP4kzcp3mRv7x00ahkm9LAK7ZRieg7k1PDAnBIOG3EyVSJ5kK4WLMvYr7sCwHbHcu4A5WwelxYK0GMJy37bNAarSJDFQsJ2ZvJjvMDmfWwDVFEVe_5tOomfVNt6bOg9mexbGjMrnHBnKnZR1vQbBtQieDlQepzTZMuQrSuKn-t5XZM7V6fCW7oP-uXGX-sMOajeX65JOf6XVpk29DP6ro8WTAflCDANC193yof8-f5_EYY-3hXhJj7RBXmizDpneEQDSaSz5sFk0sV5qPcARJ9zGG73vuGFyenjPPmtDtXtpx35A-BVcOSBYVIWe9kndG3nclfefjKEuZ3m4jL9Gg1h2JBvmXSMYiZtp9MR5I6pvbvylU_PP5xJFSjVTIz7IQSjcVGO41npnwIxRXNRxFOdIUHn0tjQ-7LwvEcTXyPsHXcMD8WtgBh-wxR8aKX7WPSsT1O8d8reb2aR7K3rkV3K82K_0OgawImEpwSvp9MNKynEAJQS6ZHe_J_l77652xwPNxMRTMASk1ZsJL";
        static string testURL = "https://apitest.myfatoorah.com/v2/GetPaymentStatus";
        static string liveURL = "https://api.myfatoorah.com/v2/GetPaymentStatus";

        public async Task<IActionResult> OnGet(string paymentId)
        {
            if (paymentId == null)
            {
                return RedirectToPage("SomethingwentError");
            }
            bool Fattorahstatus = bool.Parse(_configuration["FattorahStatus"]);
            var TestToken = _configuration["TestToken"];
            var LiveToken = _configuration["LiveToken"];

            var GetPaymentStatusRequest = new
            {
                Key = paymentId,
                KeyType = "paymentId"
            };

            var GetPaymentStatusRequestJSON = JsonConvert.SerializeObject(GetPaymentStatusRequest);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (Fattorahstatus) // fattorah live
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LiveToken);
                var httpContent = new StringContent(GetPaymentStatusRequestJSON, System.Text.Encoding.UTF8, "application/json");
                var responseMessage = client.PostAsync(liveURL, httpContent);
                res = await responseMessage.Result.Content.ReadAsStringAsync();
                FattoraResStatus = JsonConvert.DeserializeObject<FattorhResult>(res);
            }
            else                 // fattorah test
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TestToken);
                var httpContent = new StringContent(GetPaymentStatusRequestJSON, System.Text.Encoding.UTF8, "application/json");
                var responseMessage = client.PostAsync(testURL, httpContent);
                res = await responseMessage.Result.Content.ReadAsStringAsync();
                FattoraResStatus = JsonConvert.DeserializeObject<FattorhResult>(res);
            }
        

            if (FattoraResStatus.IsSuccess == true)
            {
                Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(res);
                fattorahPaymentResult = jObject["Data"].ToObject<FattorahPaymentResult>();
                int NurserySubId = 0;
                bool checkRes = int.TryParse(fattorahPaymentResult.UserDefinedField, out NurserySubId);
                if (fattorahPaymentResult.InvoiceStatus == "Paid")
                {
                    try
                    {
                        if (fattorahPaymentResult.UserDefinedField != null)
                        {

                            if (checkRes)
                            {
                                //if (expired == false)
                                //{
                                    nurserySubscription = _context.NurserySubscription.Where(e => e.NurserySubscriptionId == NurserySubId).FirstOrDefault();
                                    var planObj = _context.Plan.Where(e => e.PlanId == nurserySubscription.PlanId).FirstOrDefault();
                                    var nurseryObj = _context.NurseryMember.Where(e => e.NurseryMemberId == nurserySubscription.NurseryId).FirstOrDefault();
                                    user = await _userManager.FindByNameAsync(nurseryObj.Email);
                                    NurseryImages = _context.NurseryImage.Where(e => e.NurseryId == nurseryObj.NurseryMemberId).ToList();
                                    nurserySubscription.IsActive = true;
                                    nurserySubscription.PaymentID = paymentId;
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
                                    //expired = true;
                                //}

                                return Page();
                            }
                        }
                        return RedirectToPage("SomethingwentError");

                    }
                    catch (Exception)
                    {
                        return RedirectToPage("SomethingwentError");
                    }


                }
                else
                {
                    try
                    {
                        if (fattorahPaymentResult.UserDefinedField != null)
                        {
                            if (checkRes)
                            {
                               // if (expired == false)
                                //{
                                    nurserySubscription = _context.NurserySubscription.Where(e => e.NurserySubscriptionId == NurserySubId).FirstOrDefault();
                                    _context.NurserySubscription.Remove(nurserySubscription);
                                    _context.SaveChanges();
                                    //expired = true;
                                //}
                                return Page();
                            }
                            return RedirectToPage("SomethingwentError");
                        }
                    }

                    catch (Exception)
                    {
                        return RedirectToPage("SomethingwentError");
                    }
                }

            }
            return RedirectToPage("SomethingwentError");
        }
    }
}

