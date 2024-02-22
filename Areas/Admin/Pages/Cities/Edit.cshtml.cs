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

namespace Nursery.Areas.Admin.Pages.Cities
{
    public class EditModel : PageModel
    {

        private NurseryContext _context;
        private readonly IToastNotification _toastNotification;

        public EditModel(NurseryContext context,  IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }


        [BindProperty]
        public City city { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {

                city = await _context.City.FirstOrDefaultAsync(m => m.CityId == id);
                if (city == null)
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




        public async Task<IActionResult> OnPostAsync(int ?id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            try
            {
                var model = _context.City.Where(c => c.CityId == id.Value).FirstOrDefault();
                if (model == null)
                {
                    return Page();
                }
              

                model.CityIsActive = city.CityIsActive;
                model.CityTlAr = city.CityTlAr;
                model.CityTlEn = city.CityTlEn;
                model.CityOrderIndex = city.CityOrderIndex;
                model.CountryId = city.CountryId;

                _context.Attach(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("City Edited successfully");

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
