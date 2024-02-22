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
    public class EditNurseryModel : PageModel
    {
        private readonly NurseryContext _context;
        private readonly IToastNotification _toastNotification;
    
        private readonly IWebHostEnvironment _hostEnvironment;
       
        [BindProperty]
        public EditRegistrationModelVm registrationModel { get; set; }
        public static int NursId { get; set; }

        public HttpClient httpClient { get; set; }
  
        public static List<City> StaiticCitesList = new List<City>();
        public List<City> Cites = new List<City>();
        public List<Language> LanguageList = new List<Language>();
        public List<int> itemsSelected = new List<int>();

        public EditNurseryModel(NurseryContext context, 
          IWebHostEnvironment hostEnvironment, IToastNotification toastNotification, IConfiguration configuration)
        {
            _context = context;
           
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;
            httpClient = new HttpClient();
            
         


        }
        public async Task<IActionResult> OnGet(int? id)
        {
            if (id != null)
            {
                NursId = id.Value;
                var nurseryObj = _context.NurseryMember.Include(e => e.NurseryImage).Include(e => e.NurseryStudyLanguages).Where(e => e.NurseryMemberId == id).FirstOrDefault();
                var subObj = _context.NurserySubscription.Where(e => e.NurseryId == id).OrderByDescending(e=>e.NurserySubscriptionId).FirstOrDefault();

                registrationModel = new EditRegistrationModelVm()
                {
                    NurseryTlAr = nurseryObj.NurseryTlAr,
                    NurseryTlEn = nurseryObj.NurseryTlEn,
                    NurseryDescAr = nurseryObj.NurseryDescAr,
                    NurseryDescEn = nurseryObj.NurseryDescEn,
                    Email = nurseryObj.Email,
                    Phone1 = nurseryObj.Phone1,
                    Phone2 = nurseryObj.Phone2,
                    Fax = nurseryObj.Fax,
                    Mobile = nurseryObj.Mobile,
                    Facebook = nurseryObj.Facebook,
                    Twitter = nurseryObj.Twitter,
                    Instagram = nurseryObj.Instagram,
                    WorkTimeFrom = nurseryObj.WorkTimeFrom,
                    WorkTimeTo = nurseryObj.WorkTimeTo,
                    AgeCategoryId = nurseryObj.AgeCategoryId,
                    TransportationService = nurseryObj.TransportationService.Value,
                    SpecialNeeds = nurseryObj.SpecialNeeds.Value,
                    Language = nurseryObj.Language,
                    CountryId = _context.Area.Include(c => c.City).FirstOrDefault(c => c.AreaId == nurseryObj.AreaId)?.City.CountryId,
                    CityId = _context.Area.FirstOrDefault(c => c.AreaId == nurseryObj.AreaId)?.CityId,
                    AreaId = nurseryObj.AreaId,
                    Address = nurseryObj.Address,
                    Lat = nurseryObj.Lat,
                    Lng = nurseryObj.Lng,
                    AvailableSeats = nurseryObj.AvailableSeats,
                    IsActive = nurseryObj.IsActive,
                    Banner = nurseryObj.Banner,
                    Logo= nurseryObj.Logo,
                    Password= nurseryObj.Password,
                };
                LanguageList = _context.Languages.ToList();
                if (nurseryObj.NurseryStudyLanguages.Count != 0)
                {
                    foreach (var item in nurseryObj.NurseryStudyLanguages)
                    {
                        var nurseryLanguage = _context.Languages.Where(e => e.LanguageId == item.LanguageId).FirstOrDefault();
                        if (nurseryLanguage != null)
                        {
                           itemsSelected.Add(nurseryLanguage.LanguageId);
                        }
                    }
                }

                

            }
        
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
                var nurseryMemberObj = _context.NurseryMember.Include(e => e.NurseryImage).Where(e => e.NurseryMemberId == NursId).FirstOrDefault();
                var subObj = _context.NurserySubscription.Where(e => e.NurseryId == NursId).OrderByDescending(e => e.NurserySubscriptionId).FirstOrDefault();
                registrationModel.Email = nurseryMemberObj.Email;
                registrationModel.Password = nurseryMemberObj.Password;
               

                if (!ModelState.IsValid)
                {
                    return Page();
                }

                string Ids = Request.Form["ItemIds"];
                List<int> ItemsIds = new List<int>();
                ItemsIds = Ids.Split(',').Select(Int32.Parse).ToList();
                // string languages = "";
                List<NurseryStudyLanguage> nurseryStudyLanguages = new List<NurseryStudyLanguage>();

                for (int item = 0; item < ItemsIds.Count; item++)
                {
                    NurseryStudyLanguage nurseryStudy = new NurseryStudyLanguage()
                    {

                        LanguageId = ItemsIds[item],
                        NurseryId = nurseryMemberObj.NurseryMemberId
                    };
                    nurseryStudyLanguages.Add(nurseryStudy);



                }
                var list = _context.NurseryStudyLanguages.Where(e => e.NurseryId == nurseryMemberObj.NurseryMemberId);
                _context.NurseryStudyLanguages.RemoveRange(list);
                _context.NurseryStudyLanguages.AddRange(nurseryStudyLanguages);
                nurseryMemberObj.NurseryTlAr = registrationModel.NurseryTlAr;
                nurseryMemberObj.NurseryTlEn = registrationModel.NurseryTlEn;
                nurseryMemberObj.NurseryDescAr = registrationModel.NurseryDescAr;
                nurseryMemberObj.NurseryDescEn = registrationModel.NurseryDescEn;
                nurseryMemberObj.Email = registrationModel.Email;
                nurseryMemberObj.Phone1 = registrationModel.Phone1;
                nurseryMemberObj.Phone2 = registrationModel.Phone2;
                nurseryMemberObj.Fax = registrationModel.Fax;
                nurseryMemberObj.Mobile = registrationModel.Mobile;
                nurseryMemberObj.Facebook = registrationModel.Facebook;
                nurseryMemberObj.Twitter = registrationModel.Twitter;
                nurseryMemberObj.Instagram = registrationModel.Instagram;
                nurseryMemberObj.WorkTimeFrom = registrationModel.WorkTimeFrom;
                nurseryMemberObj.WorkTimeTo = registrationModel.WorkTimeTo;
                nurseryMemberObj.AgeCategoryId = registrationModel.AgeCategoryId;
                nurseryMemberObj.TransportationService = registrationModel.TransportationService;
                nurseryMemberObj.SpecialNeeds = registrationModel.SpecialNeeds;
                nurseryMemberObj.Language = registrationModel.Language;
                nurseryMemberObj.CountryId = _context.Area.Include(c => c.City).FirstOrDefault(c => c.AreaId == registrationModel.AreaId)?.City.CountryId;
                nurseryMemberObj.CityId = _context.Area.FirstOrDefault(c => c.AreaId == registrationModel.AreaId)?.CityId;
                nurseryMemberObj.AreaId = registrationModel.AreaId;
                nurseryMemberObj.Address = registrationModel.Address;
                nurseryMemberObj.Lat = registrationModel.Lat;
                nurseryMemberObj.Lng = registrationModel.Lng;
                nurseryMemberObj.AvailableSeats = registrationModel.AvailableSeats;
                nurseryMemberObj.IsActive = registrationModel.IsActive;
               
                if (Logo != null)
                {
                    string folder = "Images/NurseryMember/";
                    nurseryMemberObj.Logo = UploadImage(folder, Logo);
                }
                else
                {
                    nurseryMemberObj.Logo = registrationModel.Logo;

                }
                if (Banner != null)
                {
                    string folder = "Images/NurseryMember/";
                    nurseryMemberObj.Banner = UploadImage(folder, Banner);
                }
                else
                {
                    nurseryMemberObj.Banner = registrationModel.Banner;
                }
                List<NurseryImage> NurseryImageList = new List<NurseryImage>();
                if (ImageFielsList.Count != 0)
                {
                    foreach (var item in ImageFielsList)
                    {
                        var NurseryImageObj = new NurseryImage();
                        string folder = "Images/NurseryMember/";
                        NurseryImageObj.Pic = UploadImage(folder, item);
                        NurseryImageObj.NurseryId = nurseryMemberObj.NurseryMemberId;
                        NurseryImageList.Add(NurseryImageObj);

                    }
                    _context.NurseryImage.AddRange(NurseryImageList);

                }
                _context.Attach(nurseryMemberObj).State = EntityState.Modified;
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Nursery Edited Successfully");



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
