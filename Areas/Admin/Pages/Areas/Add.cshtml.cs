using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Nursery.Data;

namespace Nursery.Areas.Admin.Pages.Areas
{
    public class AddModel : PageModel
    {
        private NurseryContext _context;
        private readonly IToastNotification _toastNotification;
        public AddModel(NurseryContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }

        public void OnGet()
        {
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
        public IActionResult OnPost(Models.Area model)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                _context.Area.Add(model);
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Area Added successfully");
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
