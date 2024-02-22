using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Nursery.Data;
using Nursery.Models;

namespace Nursery.Areas.Admin.Pages.Areas
{
    public class EditModel : PageModel
    {

        private NurseryContext _context;
        private readonly IToastNotification _toastNotification;

        public EditModel(NurseryContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }


        [BindProperty(SupportsGet = true)]
        public Area area { get; set; }
        [BindProperty]
        public int countryId { get; set; }

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
        public async Task<IActionResult> OnGetAsync(int id)
        {
            
            try
            {

                var temp = await _context.Area.Include(c => c.City.Country).FirstOrDefaultAsync(m => m.AreaId == id);
                area.CityId = temp.CityId;
                area.AreaTlAr = temp.AreaTlAr;
                area.AreaTlEn = temp.AreaTlEn;
                area.AreaIsActive = temp.AreaIsActive;
                area.AreaOrderIndex = temp.AreaOrderIndex;
                countryId = temp.City.Country.CountryId;

                if (area == null)
                {
                    return Redirect("../Error");
                }
            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
            }

           

            return Page();
        }




        public async Task<IActionResult> OnPostAsync(int id , Area areaMdel)
        {
            var model = await _context.Area.Include(c => c.City.Country).FirstOrDefaultAsync(m => m.AreaId == id);
            area.CityId = model.CityId;
            area.AreaTlAr = model.AreaTlAr;
            area.AreaTlEn = model.AreaTlEn;
            area.AreaIsActive = model.AreaIsActive;
            area.AreaOrderIndex = model.AreaOrderIndex;
            countryId = model.City.Country.CountryId;
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            try
            {
              
                if (model == null)
                {
                    return Page();
                }
              

                model.AreaIsActive = areaMdel.AreaIsActive;
                model.AreaTlAr = areaMdel.AreaTlAr;
                model.AreaTlEn = areaMdel.AreaTlEn;
                model.AreaOrderIndex = areaMdel.AreaOrderIndex;
                model.CityId = areaMdel.CityId;

                _context.Attach(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Area Edited successfully");

            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
                return Page();
            }


            return RedirectToPage("./Index");
        }

       
    }
}
