using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Nursery.Data;
using Nursery.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Nursery.Entities;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Http.Headers;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace Nursery.Areas.Admin.Pages.Users
{
    public class AddNurseryModel : PageModel
    {
        private readonly NurseryContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IEmailSender _emailSender;
        [BindProperty]
        public NurseryRegistrationVm registrationModel { get; set; }
        public HttpClient httpClient { get; set; }
        private IHostingEnvironment _env;
        public static List<City> StaiticCitesList = new List<City>();
        public  List<Language> LanguageList = new List<Language>();
        public List<City> Cites = new List<City>();
        public AddNurseryModel(NurseryContext context, IEmailSender emailSender,
            UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment, IHostingEnvironment env, IToastNotification toastNotification, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;
            httpClient = new HttpClient();
            _emailSender = emailSender;
            _env = env;


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
                         where i.CountryId == countryId && i.CityIsActive == true && i.Country.CountryIsActive == true
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
                         where i.CityId == cityId && i.AreaIsActive == true
                         && i.City.CityIsActive == true
                         && i.City.Country.CountryIsActive == true

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
                registrationModel.PaymentMethodId = 2; //Cash
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                var userExists = await _userManager.FindByEmailAsync(registrationModel.Email);
                if (userExists != null)
                {
                    _toastNotification.AddErrorToastMessage("User Email is aleady Exists");
                    return Page();
                }
                string Ids = Request.Form["ItemIds"];
                List<int> ItemsIds = new List<int>();
                List<NurseryStudyLanguage> nurseryStudyLanguages = new List<NurseryStudyLanguage>();
                ItemsIds = Ids.Split(',').Select(Int32.Parse).ToList();
                // string languages = "";
                for (int item = 0; item < ItemsIds.Count; item++)
                {
                    NurseryStudyLanguage nurseryStudy = new NurseryStudyLanguage()
                    {
                       
                        LanguageId = ItemsIds[item]
                    };
                    nurseryStudyLanguages.Add(nurseryStudy);

                    //var langObj = _context.Languages.Where(e => e.LanguageId == ItemsIds[item]).FirstOrDefault();
                    //if (langObj != null)
                    //{
                    //    languages += langObj.Title;
                    //}
                    //if (item != 0 || item != ItemsIds[ItemsIds.Count - 1])
                    //{
                    //    languages += " , ";
                    //}
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
                nurseryMember.IsActive = true;
                //nurseryMember.StudyLanguage = languages;
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
                subscription.IsActive = true;
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
                double totalCost = subscription.Price.Value;
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
                   subscription.StartDate.Value.ToShortDateString(),
                   subscription.EndDate.Value.ToShortDateString(),
                   totalCost,
                   nurseryMember.NurseryTlAr.ToString(),
                   plan.PlanTlAr

                   );

                await _emailSender.SendEmailAsync(nurseryMember.Email, "Nursery Subscription", messageBody);

                _toastNotification.AddSuccessToastMessage("Nursery Added Successfully");



            }
            catch (Exception e)
            {

                _toastNotification.AddErrorToastMessage("Something went Error Please Try again.");

            }
           
            return Redirect("./NurseryList");
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
