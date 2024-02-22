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

namespace Nursery.Pages
{

    public class TestPhatoraModel : PageModel
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

        public static List<City> StaiticCitesList = new List<City>();
        public List<City> Cites = new List<City>();


        public TestPhatoraModel(NurseryContext context, IEmailSender emailSender,
            UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;
            httpClient = new HttpClient();
            _emailSender = emailSender;

        }
        public async Task<IActionResult> OnGet()
        {
            return Page();
        }
        public IActionResult OnGetFillCityList(string Values)
        {

            int countryId = 0;
            bool checkTrue = int.TryParse(Values, out countryId);
            var lookup = from i in _context.City
                         orderby i.CityId
                         where i.CountryId == countryId
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
                         where i.CountryId == countryId
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
                         where i.CityId == cityId

                         select new
                         {
                             Value = i.AreaId,
                             Text = i.AreaTlEn
                         };

            return new JsonResult(lookup);
        }

        //public async Task<IActionResult> OnPost(IFormFile Banner, IFormFile Logo, IFormFileCollection ImageFielsList)
        //{
        //    try
        //    {

        //        if (!ModelState.IsValid)
        //        {
        //            return Page();
        //        }
        //        var userExists = await _userManager.FindByEmailAsync(registrationModel.Email);
        //        if (userExists != null)
        //        {
        //            _toastNotification.AddErrorToastMessage("User Email is aleady Exists");
        //            return Page();
        //        }
        //        var nurseryMember = new NurseryMember();
        //        nurseryMember.NurseryTlAr = registrationModel.NurseryTlAr;
        //        nurseryMember.NurseryTlEn = registrationModel.NurseryTlEn;
        //        nurseryMember.NurseryDescAr = registrationModel.NurseryDescAr;
        //        nurseryMember.NurseryDescEn = registrationModel.NurseryDescEn;
        //        nurseryMember.Email = registrationModel.Email;
        //        nurseryMember.Phone1 = registrationModel.Phone1;
        //        nurseryMember.Phone2 = registrationModel.Phone2;
        //        nurseryMember.Fax = registrationModel.Fax;
        //        nurseryMember.Mobile = registrationModel.Mobile;
        //        nurseryMember.Facebook = registrationModel.Facebook;
        //        nurseryMember.Twitter = registrationModel.Twitter;
        //        nurseryMember.Instagram = registrationModel.Instagram;
        //        nurseryMember.WorkTimeFrom = registrationModel.WorkTimeFrom;
        //        nurseryMember.WorkTimeTo = registrationModel.WorkTimeTo;
        //        nurseryMember.AgeCategoryId = registrationModel.AgeCategoryId;
        //        nurseryMember.TransportationService = registrationModel.TransportationService;
        //        nurseryMember.SpecialNeeds = registrationModel.SpecialNeeds;
        //        nurseryMember.Language = registrationModel.Language;
        //        nurseryMember.CountryId = _context.Area.Include(c => c.City).FirstOrDefault(c => c.AreaId == registrationModel.AreaId)?.City.CountryId;
        //        nurseryMember.CityId = _context.Area.FirstOrDefault(c => c.AreaId == registrationModel.AreaId)?.CityId;
        //        nurseryMember.AreaId = registrationModel.AreaId;
        //        nurseryMember.Address = registrationModel.Address;
        //        nurseryMember.Lat = registrationModel.Lat;
        //        nurseryMember.Lng = registrationModel.Lng;
        //        nurseryMember.AvailableSeats = registrationModel.AvailableSeats;
        //        //nurseryMember.PaymentMethodId = registrationModel.PaymentMethodId;
        //        //nurseryMember.IsActive = false;



        //        if (Logo != null)
        //        {
        //            string folder = "Images/NurseryMember/";
        //            nurseryMember.Logo = UploadImage(folder, Logo);
        //        }
        //        if (Banner != null)
        //        {
        //            string folder = "Images/NurseryMember/";
        //            nurseryMember.Banner = UploadImage(folder, Banner);
        //        }
        //        List<NurseryImage> NurseryImageList = new List<NurseryImage>();
        //        if (ImageFielsList.Count != 0)
        //        {
        //            foreach (var item in ImageFielsList)
        //            {
        //                var NurseryImageObj = new NurseryImage();
        //                string folder = "Images/NurseryMember/";
        //                NurseryImageObj.Pic = UploadImage(folder, item);
        //                NurseryImageList.Add(NurseryImageObj);

        //            }
        //            nurseryMember.NurseryImage = NurseryImageList;

        //        }





        //        _context.NurseryMember.Add(nurseryMember);
        //        _context.SaveChanges();
        //        var plan = _context.Plan.Find(registrationModel.PlanId);
        //        var planType = _context.PlanTypes.Find(registrationModel.PlanTypeId);
        //        var TotalCost = plan.Price + planType.Cost;
        //        var subscription = new NurserySubscription();
        //        subscription.PlanId = registrationModel.PlanId;
        //        subscription.NurseryId = nurseryMember.NurseryMemberId;
        //        subscription.Price = plan.Price;
        //        subscription.StartDate = DateTime.Now;
        //        subscription.EndDate = DateTime.Now.AddMonths(plan.DurationInMonth.Value);
        //        _context.NurserySubscription.Add(subscription);
        //        _context.SaveChanges();

        //        var user = new ApplicationUser
        //        {
        //            UserName = registrationModel.Email,
        //            Email = registrationModel.Email,
        //            PhoneNumber = registrationModel.Mobile,
        //            EntityId = nurseryMember.NurseryMemberId,
        //            EntityName = 2

        //        };

        //        var result = await _userManager.CreateAsync(user, registrationModel.Password);

        //        if (!result.Succeeded)
        //        {
        //            _context.NurseryMember.Remove(nurseryMember);
        //            _context.SaveChanges();
        //            _toastNotification.AddErrorToastMessage("User creation failed! Please check user details and try again.");
        //            return Page();



        //        }
        //        await _userManager.AddToRoleAsync(user, "Nursery");
        //        if (nurseryMember.PaymentMethodId == 1)
        //        {
        //            var requesturl = "https://api.upayments.com/test-payment";
        //            var fields = new
        //            {
        //                merchant_id = "1201",
        //                username = "test",
        //                password = "test",
        //                order_id = nurseryMember.NurseryMemberId,
        //                total_price = TotalCost,
        //                test_mode = 0,
        //                CstFName = nurseryMember.NurseryTlEn,
        //                CstEmail = nurseryMember.Email,
        //                CstMobile = nurseryMember.Phone1,
        //                api_key = "jtest123",
        //                success_url = "https://localhost:44354/success",
        //                error_url = "https://localhost:44354/failed"
        //                //success_url = "http://codewarenet-001-site9.dtempurl.com/success",
        //                //error_url = "http://codewarenet-001-site9.dtempurl.com/failed"

        //            };
        //            var content = new StringContent(JsonConvert.SerializeObject(fields), Encoding.UTF8, "application/json");
        //            var task = httpClient.PostAsync(requesturl, content);
        //            var res = await task.Result.Content.ReadAsStringAsync();
        //            var paymenturl = JsonConvert.DeserializeObject<paymenturl>(res);
        //            if (paymenturl.status == "success")

        //            {
        //                return Redirect(paymenturl.paymentURL);
        //            }
        //            else
        //            {
        //                _context.NurserySubscription.Remove(subscription);
        //                _context.NurseryMember.Remove(nurseryMember);
        //                await _userManager.DeleteAsync(user);
        //                return Redirect("../Error");
        //            }

        //        }

        //        await _emailSender.SendEmailAsync(
        //          registrationModel.Email,
        //      $"Welcome To {registrationModel.NurseryTlEn} ...",
        //         $"Thank You For Registration");
        //        return RedirectToPage("/Thankyou");

        //       // _toastNotification.AddSuccessToastMessage("Nursery Added Successfully");

        //    }
        //    catch (Exception e)
        //    {

        //        _toastNotification.AddErrorToastMessage("Something went Error Please Try again.");

        //    }

        //    return Page();
        //}
        //private string UploadImage(string folderPath, IFormFile file)
        //    {

        //        folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

        //        string serverFolder = Path.Combine(_hostEnvironment.WebRootPath, folderPath);

        //        file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

        //        return folderPath;
        //    }
        //    public IActionResult OnGetCountryIds(int CountryId)
        //    {
        //        var Cites = _context.City.Where(e => e.CountryId == CountryId).ToList();
        //        return new JsonResult(Cites);
        //    }

        //}
    }
    
}
