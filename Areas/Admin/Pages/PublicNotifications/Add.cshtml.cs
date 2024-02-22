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

namespace Nursery.Areas.Admin.Pages.PublicNotifications
{
    public class AddModel : PageModel
    {
        private NurseryContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;

        public AddModel(NurseryContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;

        }


        public void OnGet()
        {

        }
        public IActionResult OnGetFillNurseryList(string Values)
        {

            int countryId = 0;
            bool checkTrue = int.TryParse(Values, out countryId);
            var lookup = from i in _context.NurseryMember
                         orderby i.NurseryMemberId
                         where i.CountryId == countryId&& i.NurserySubscription.OrderByDescending(e => e.NurserySubscriptionId).FirstOrDefault().IsActive
                         && i.Area.AreaIsActive==true
                         &&i.Area.City.CityIsActive==true
                         &&i.Area.City.Country.CountryIsActive==true
                         select new
                         {
                             Value = i.NurseryMemberId,
                             Text = i.NurseryTlEn
                         };
            return new JsonResult(lookup);
        }
        public IActionResult OnPost(PublicNotification model)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                 //model.EntityTypeId = 1;
                // model.EntityTypeNotifyId = 1;
                model.Date = DateTime.Now;

                _context.PublicNotification.Add(model);
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Notification Added successfully");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
                return Page();
            }
          
            return Redirect("./Index");

        }
    }
}
