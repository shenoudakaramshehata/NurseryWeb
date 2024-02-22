using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nursery.Data;
using Nursery.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nursery.Entities;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using System.Text;
using NToastNotify;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Http.Headers;
using MimeKit;
using Microsoft.Extensions.Configuration;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace Nursery.Pages
{
    public class NurseryRegisterModel : PageModel
    {
        private readonly NurseryContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IEmailSender _emailSender;
        [BindProperty]
        public NurseryRegistrationVm registrationModel { get; set; }
        public List<Language> LanguageList = new List<Language>();

        public HttpClient httpClient { get; set; }

        public static List<City> StaiticCitesList = new List<City>();
        public List<City> Cites = new List<City>();
        static string token = "rLtt6JWvbUHDDhsZnfpAhpYk4dxYDQkbcPTyGaKp2TYqQgG7FGZ5Th_WD53Oq8Ebz6A53njUoo1w3pjU1D4vs_ZMqFiz_j0urb_BH9Oq9VZoKFoJEDAbRZepGcQanImyYrry7Kt6MnMdgfG5jn4HngWoRdKduNNyP4kzcp3mRv7x00ahkm9LAK7ZRieg7k1PDAnBIOG3EyVSJ5kK4WLMvYr7sCwHbHcu4A5WwelxYK0GMJy37bNAarSJDFQsJ2ZvJjvMDmfWwDVFEVe_5tOomfVNt6bOg9mexbGjMrnHBnKnZR1vQbBtQieDlQepzTZMuQrSuKn-t5XZM7V6fCW7oP-uXGX-sMOajeX65JOf6XVpk29DP6ro8WTAflCDANC193yof8-f5_EYY-3hXhJj7RBXmizDpneEQDSaSz5sFk0sV5qPcARJ9zGG73vuGFyenjPPmtDtXtpx35A-BVcOSBYVIWe9kndG3nclfefjKEuZ3m4jL9Gg1h2JBvmXSMYiZtp9MR5I6pvbvylU_PP5xJFSjVTIz7IQSjcVGO41npnwIxRXNRxFOdIUHn0tjQ-7LwvEcTXyPsHXcMD8WtgBh-wxR8aKX7WPSsT1O8d8reb2aR7K3rkV3K82K_0OgawImEpwSvp9MNKynEAJQS6ZHe_J_l77652xwPNxMRTMASk1ZsJL";
        //static string baseURL = "https://apitest.myfatoorah.com";
        private readonly IConfiguration _configuration;
        public NurseryRegisterModel(NurseryContext context, IEmailSender emailSender,
            UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;
            httpClient = new HttpClient();
            _emailSender = emailSender;
            _configuration = configuration;

        }
        public async Task<IActionResult> OnGet()
        {
            LanguageList = _context.Languages.ToList();
            return Page();
        }
        public IActionResult OnGetFillCityList(string Values)
        {

            int countryId = 0;
            bool checkTrue = int.TryParse(Values, out countryId);
            var lookup = from i in _context.City
                         orderby i.CityId
                         where i.CountryId == countryId&&i.CityIsActive==true&&i.Country.CountryIsActive==true
                         select new
                         {
                             Value = i.CityId,
                             Text = i.CityTlEn
                         };
            return new JsonResult(lookup);
        }
        public IActionResult OnGetFillPlanList(string Values)
        {
            int countryId = 0;
            bool checkTrue = int.TryParse(Values, out countryId);
            var lookup = from i in _context.Plan
                         orderby i.PlanTlEn
                         where i.CountryId == countryId && i.IsActive != false && i.IsActive != null
                         orderby i.PlanTlEn
                         select new
                         {
                             Value = i.PlanId,
                             Text = i.PlanTlEn
                         };
            return new JsonResult(lookup);
        }
        public IActionResult OnGetFillAreaList(string Values)
        {
            int cityId = 0;
            bool checkTrue = int.TryParse(Values, out cityId);
            var lookup = from i in _context.Area
                         orderby i.AreaId
                         where i.CityId == cityId&&i.AreaIsActive==true
                         &&i.City.CityIsActive==true
                         &&i.City.Country.CountryIsActive==true

                         select new
                         {
                             Value = i.AreaId,
                             Text = i.AreaTlEn
                         };

            return new JsonResult(lookup);
        }

        public async Task<IActionResult> OnPost(IFormFile Banner, IFormFile Logo, IFormFileCollection ImageFielsList)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return Page();
                }
                var userExists = await _userManager.FindByEmailAsync(registrationModel.Email);
                if (userExists != null)
                {
                    _toastNotification.AddErrorToastMessage("User Email is aleady Exists");
                  

                    return RedirectToPage();
                }
                string Ids = Request.Form["ItemIds"];
                List<int> ItemsIds = new List<int>();
                List<NurseryStudyLanguage> nurseryStudyLanguages = new List<NurseryStudyLanguage>();
                ItemsIds = Ids.Split(',').Select(Int32.Parse).ToList();
              
                for (int item = 0; item < ItemsIds.Count; item++)
                {
                    NurseryStudyLanguage nurseryStudy = new NurseryStudyLanguage()
                    {

                        LanguageId = ItemsIds[item]
                    };
                    nurseryStudyLanguages.Add(nurseryStudy);

                   
                }

                var nurseryMember = new NurseryMember();
                nurseryMember.NurseryTlAr = registrationModel.NurseryTlAr;
                nurseryMember.NurseryTlEn = registrationModel.NurseryTlEn;
                nurseryMember.NurseryDescAr = registrationModel.NurseryDescAr;
                nurseryMember.NurseryDescEn = registrationModel.NurseryDescEn;
                nurseryMember.Email = registrationModel.Email;
                nurseryMember.Phone1 = registrationModel.Phone1;
                nurseryMember.Phone2 = registrationModel.Phone2;
                nurseryMember.Fax = registrationModel.Fax;
                nurseryMember.Mobile = registrationModel.Mobile;
                nurseryMember.Facebook = registrationModel.Facebook;
                nurseryMember.Twitter = registrationModel.Twitter;
                nurseryMember.Instagram = registrationModel.Instagram;
                nurseryMember.WorkTimeFrom = registrationModel.WorkTimeFrom;
                nurseryMember.WorkTimeTo = registrationModel.WorkTimeTo;
                nurseryMember.AgeCategoryId = registrationModel.AgeCategoryId;
                nurseryMember.TransportationService = registrationModel.TransportationService;
                nurseryMember.SpecialNeeds = registrationModel.SpecialNeeds;
                nurseryMember.Language = registrationModel.Language;
                nurseryMember.CountryId = _context.Area.Include(c => c.City).FirstOrDefault(c => c.AreaId == registrationModel.AreaId)?.City.CountryId;
                nurseryMember.CityId = _context.Area.FirstOrDefault(c => c.AreaId == registrationModel.AreaId)?.CityId;
                nurseryMember.AreaId = registrationModel.AreaId;
                nurseryMember.Address = registrationModel.Address;
                nurseryMember.Lat = registrationModel.Lat;
                nurseryMember.Lng = registrationModel.Lng;
                nurseryMember.AvailableSeats = registrationModel.AvailableSeats;
                // nurseryMember.PaymentMethodId = registrationModel.PaymentMethodId;
                //nurseryMember.IsActive = false;
                nurseryMember.Password = registrationModel.Password;
                nurseryMember.NurseryStudyLanguages = nurseryStudyLanguages;


                if (Logo != null)
                {
                    string folder = "Images/NurseryMember/";
                    nurseryMember.Logo = UploadImage(folder, Logo);
                }
                if (Banner != null)
                {
                    string folder = "Images/NurseryMember/";
                    nurseryMember.Banner = UploadImage(folder, Banner);
                }
                List<NurseryImage> NurseryImageList = new List<NurseryImage>();
                if (ImageFielsList.Count != 0)
                {
                    foreach (var item in ImageFielsList)
                    {
                        var NurseryImageObj = new NurseryImage();
                        string folder = "Images/NurseryMember/";
                        NurseryImageObj.Pic = UploadImage(folder, item);
                        NurseryImageList.Add(NurseryImageObj);

                    }
                    nurseryMember.NurseryImage = NurseryImageList;

                }





                _context.NurseryMember.Add(nurseryMember);
               
                _context.SaveChanges();
                //NurseryPassword nurseryPassword = new NurseryPassword()
                //{
                //    NurseryMemberId = nurseryMember.NurseryMemberId,
                //    Password = registrationModel.Password
                //};
                //_context.NurseryPasswords.Add(nurseryPassword);
                var plan = _context.Plan.Find(registrationModel.PlanId);
                var planType = _context.PlanTypes.Find(registrationModel.PlanTypeId);
                var TotalCost = plan.Price + planType.Cost;
                var subscription = new NurserySubscription();
                subscription.PlanId = registrationModel.PlanId;
                subscription.NurseryId = nurseryMember.NurseryMemberId;
                subscription.Price = TotalCost;
                subscription.StartDate = DateTime.Now;
                subscription.EndDate = DateTime.Now.AddMonths(plan.DurationInMonth.Value);
                subscription.PaymentMethodId = registrationModel.PaymentMethodId;
                subscription.PlanTypeId = registrationModel.PlanTypeId;
                _context.NurserySubscription.Add(subscription);
                _context.SaveChanges();

                var user = new ApplicationUser
                {
                    UserName = registrationModel.Email,
                    Email = registrationModel.Email,
                    PhoneNumber = registrationModel.Mobile,
                    EntityId = nurseryMember.NurseryMemberId,
                    EntityName = 2

                };

                var result = await _userManager.CreateAsync(user, registrationModel.Password);

                if (!result.Succeeded)
                {
                    _context.NurseryMember.Remove(nurseryMember);
                    _context.SaveChanges();
                    _toastNotification.AddErrorToastMessage("User creation failed! Please Enter Complex Password");
                    return Page();



                }
                await _userManager.AddToRoleAsync(user, "Nursery");



          







                if (registrationModel.PaymentMethodId == 1)
                {
                    var requesturl = "https://api.upayments.com/test-payment";
                    var fields = new
                    {
                        merchant_id = "1201",
                        username = "test",
                        password = "test",
                        order_id = subscription.NurserySubscriptionId,
                        total_price = TotalCost,
                        test_mode = 0,
                        CstFName = nurseryMember.NurseryTlEn,
                        CstEmail = nurseryMember.Email,
                        CstMobile = nurseryMember.Phone1,
                        api_key = "jtest123",
                        //success_url = "https://localhost:44354/success",
                        //error_url = "https://localhost:44354/failed"
                        success_url = "http://alhadhanah.com/success",
                        error_url = "http://alhadhanah.com/failed"

                    };
                    var content = new StringContent(JsonConvert.SerializeObject(fields), Encoding.UTF8, "application/json");
                    var task = httpClient.PostAsync(requesturl, content);
                    var res = await task.Result.Content.ReadAsStringAsync();
                    var paymenturl = JsonConvert.DeserializeObject<paymenturl>(res);
                    if (paymenturl.status == "success")

                    {
                        return Redirect(paymenturl.paymentURL);
                    }
                    else
                    {
                        _context.NurserySubscription.Remove(subscription);
                        _context.NurseryMember.Remove(nurseryMember);
                        await _userManager.DeleteAsync(user);
                        return Redirect("../Error");
                    }

                }
            

                else if (registrationModel.PaymentMethodId == 3)
                {
                    bool IsProd = bool.Parse(_configuration["IsProd"]);
                    var TestToken = _configuration["TestToken"];
                    var LiveToken = _configuration["LiveToken"];


                    var TapMessage = new
                    {
                        amount = TotalCost,
                        currency = "KWD",
                        threeDSecure = true,
                        save_card = false,
                        description = "Nursery Fees",
                        statement_descriptor = "Sample",
                        metadata = new
                        {
                            udf1 = user.Id,
                            CstFName = nurseryMember.NurseryTlEn,
                            CstEmail = nurseryMember.Email,
                            CstMobile = nurseryMember.Phone1,
                            order_id = subscription.NurserySubscriptionId,
                        },
                        reference = new
                        {
                            transaction = "txn_0001",
                        },
                        receipt = new
                        {
                            email = false,
                            sms = true
                        },
                        customer = new
                        {
                            first_name = user.UserName,
                            middle_name = "test",
                            last_name = "test",
                            email = user.UserName,
                            phone = new
                            {
                                country_code = "965",
                                number = "50143413"
                            }
                        },
                        merchant = new { id = "21900800" },
                        source = new { id = "src_kw.knet" },
                        redirect = new { url = "https://localhost:5001/CallBack" }
                    };


                    var sendPaymentRequestJSON1 = JsonConvert.SerializeObject(TapMessage);
                    var client = new RestClient("https://api.tap.company/v2/charges");
                    var request = new RestRequest();
                    request.AddHeader("content-type", "application/json");


                    if (IsProd) // fattorah live
                    {

                        request.AddHeader("authorization", LiveToken);
                        //end of TaP
                 
                    }
                    else // fattorah test
                    {

                        request.AddHeader("authorization", TestToken);

                    }
                    request.AddParameter("application/json", sendPaymentRequestJSON1, ParameterType.RequestBody);
                    RestResponse response = await client.PostAsync(request);

                    var DeserializeObjectResopnse = JsonConvert.DeserializeObject<JObject>(response.Content);

                    var Transaction = DeserializeObjectResopnse.GetValue("transaction");

                    var Url = Transaction["url"].ToString();

                    return Redirect(Url);

                }


                else if (registrationModel.PaymentMethodId == 2)
                {
                    var webRoot = _hostEnvironment.WebRootPath;

                    var pathToFile = _hostEnvironment.WebRootPath
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
                       subscription.StartDate.Value.ToShortDateString(),
                       subscription.EndDate.Value.ToShortDateString(),
                       TotalCost,
                       string.Format("{0:dddd, d MMMM yyyy}", DateTime.Now)
                       );
                    await _emailSender.SendEmailAsync(nurseryMember.Email, "Nursery Subscription", messageBody);
     
                    return RedirectToPage("/Thankyou");
                }

            }
            catch (Exception e)
            {

                _toastNotification.AddErrorToastMessage("Something went Error Please Try again.");

            }

            return Page();
        }
        private string UploadImage(string folderPath, IFormFile file)
        {
            string picIm = Guid.NewGuid().ToString() + "_" + file.FileName;

            folderPath += picIm;
            string serverFolder = Path.Combine(_hostEnvironment.WebRootPath, folderPath);

            file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return picIm;
        }
        public IActionResult OnGetCountryIds(int CountryId)
        {
            var Cites = _context.City.Where(e => e.CountryId == CountryId).ToList();
            return new JsonResult(Cites);
        }
    }
}