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
using Microsoft.Extensions.Localization;
using NToastNotify;
using Nursery.Data;
using Nursery.Models;

namespace Nursery.Areas.Admin.Pages.Cities
{
    public class DeleteModel : PageModel
    {

        private NurseryContext _context;
        private readonly IToastNotification _toastNotification;

        public DeleteModel(NurseryContext context, IToastNotification toastNotification)
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
                city = await _context.City.Include(c => c.Country).FirstOrDefaultAsync(m => m.CityId == id);

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




        public async Task<IActionResult> OnPostAsync(int id)
        {
           
          
            try
            {
                city = await _context.City.Include(c => c.Country).FirstOrDefaultAsync(m => m.CityId == id);
                if (_context.Area.Any(c => c.CityId == id) )
                {
                    var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
                    var BrowserCulture = locale.RequestCulture.UICulture.ToString();
                    if (BrowserCulture == "en-US")

                        _toastNotification.AddErrorToastMessage("You cannot delete this City");

                    else
                        _toastNotification.AddErrorToastMessage("You cannot delete this City");

                   

                    return Page();
                }

             
                if (city != null)
                {
                    _context.City.Remove(city);
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("City Deleted successfully");

                }
            }
            catch (Exception)

            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
                city = await _context.City.Include(c => c.Country).FirstOrDefaultAsync(m => m.CityId == id);

                return Page();


            }

            return RedirectToPage("./Index");
        }

    }
}
