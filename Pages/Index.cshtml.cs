using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NToastNotify;
using Nursery.Data;
using Nursery.Entities;
using Nursery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using System.IO;

using System.Text;

namespace Nursery.Pages
{
    public class IndexModel : PageModel


    {
        private readonly ILogger<IndexModel> _logger;
        private readonly NurseryContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;
        public HttpClient httpClient { get; set; }
        public IndexModel(NurseryContext context, ILogger<IndexModel> logger, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;
            httpClient = new HttpClient();

        }
        [BindProperty]
        public Newsletter newsletter { get; set; }
        [BindProperty]
        public ContactUs contactUs { get; set; }
        
        [BindProperty]
        public IEnumerable<Country> Countries { get; set; }
        [BindProperty]
        public RegistrationModel registrationModel { get; set; }
        public void OnGet()
        {

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
                    return Page();
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
                //nurseryMember.PaymentMethodId = registrationModel.PaymentMethodId;
                //nurseryMember.IsActive = false;


                if (Logo != null)
                {
                    string folder = "Images/Images/NurseryMember/";
                    nurseryMember.Logo = await UploadImage(folder, Logo);
                }
                if (Banner != null)
                {
                    string folder = "Images/Images/NurseryMember/";
                    nurseryMember.Banner = await UploadImage(folder, Banner);
                }
                List<NurseryImage> NurseryImageList = new List<NurseryImage>();
                if (ImageFielsList.Count != 0)
                {
                    foreach (var item in ImageFielsList)
                    {
                        var NurseryImageObj = new NurseryImage();
                        string folder = "Images/Images/NurseryMember/";
                        NurseryImageObj.Pic = await UploadImage(folder, item);
                        NurseryImageList.Add(NurseryImageObj);

                    }
                    nurseryMember.NurseryImage = NurseryImageList;

                }





                _context.NurseryMember.Add(nurseryMember);
                _context.SaveChanges();
                var plan = _context.Plan.Find(registrationModel.PlanId);
                var subscription = new NurserySubscription();
                subscription.PlanId = registrationModel.PlanId;
                subscription.NurseryId = nurseryMember.NurseryMemberId;
                subscription.Price = plan.Price;
                subscription.StartDate = DateTime.Now;
                subscription.EndDate = DateTime.Now.AddMonths(plan.DurationInMonth.Value);
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
                    _toastNotification.AddErrorToastMessage("User creation failed! Please check user details and try again.");
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
                        order_id = nurseryMember.NurseryMemberId,
                        total_price = plan.Price,
                        test_mode = 0,
                        CstFName = nurseryMember.NurseryTlEn,
                        CstEmail = nurseryMember.Email,
                        CstMobile = nurseryMember.Phone1,
                        api_key = "jtest123",
                        success_url = "https://localhost:44354/success",
                        error_url = "https://localhost:44354/failed"
                        //success_url = "http://codewarenet-001-site9.dtempurl.com/success",
                        //error_url = "http://codewarenet-001-site9.dtempurl.com/failed"

                    };
                    var content = new StringContent(JsonConvert.SerializeObject(fields), Encoding.UTF8, "application/json");
                    var task = httpClient.PostAsync(requesturl, content);
                    var res = await task.Result.Content.ReadAsStringAsync();
                    var paymenturl = JsonConvert.DeserializeObject<paymenturl>(res);
                    if (paymenturl.status == "success")

                    {
                        return Redirect("../Error");
                    }
                    else
                    {
                        _context.NurserySubscription.Remove(subscription);
                        _context.NurseryMember.Remove(nurseryMember);
                        await _userManager.DeleteAsync(user);
                        return Redirect("../Error");
                    }

                }




            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went Error Please Try again.");
                return Page();
            }
            return Page();
        }
        private async Task<string> UploadImage(string folderPath, IFormFile file)
        {

            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_hostEnvironment.WebRootPath, folderPath);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return folderPath;
        }

        public IActionResult OnPostAddNewsletter()

        {
            if (!ModelState.IsValid)
            {
                return Redirect("./Index");
            }
            try
            {
                 newsletter.Date = DateTime.Now;
                _context.Newsletter.Add(newsletter);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                return Redirect("./Index");
            }

            return Redirect("./Index");

        }
        public IActionResult OnPostAddContactUs()
        {
            if (!ModelState.IsValid)
            {
                return Redirect("./Index");
            }
            try
            {

                contactUs.TransDate = DateTime.Now;
                _context.ContactUs.Add(contactUs);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                return Redirect("./Index");

            }

            return Redirect("./Index");

        }
  
    }
}
