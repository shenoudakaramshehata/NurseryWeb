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

namespace Nursery.Areas.Admin.Pages.Countries
{
    public class DeleteModel : PageModel
    {

        
        private NurseryContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;

        public DeleteModel(NurseryContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;

        }
        [BindProperty]
        public Country country { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
           
            try
            {
                country = await _context.Country.FirstOrDefaultAsync(m => m.CountryId == id);
                if (country == null)
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
                country = await _context.Country.FirstOrDefaultAsync(m => m.CountryId == id);
                if (_context.City.Any(c => c.CountryId == id) || _context.Plan.Any(c => c.CountryId == id)||_context.Adz.Any(c => c.CountryId == id)
                    || _context.PublicDevice.Any(c => c.CountryId == id)||_context.PublicNotification.Any(c => c.CountryId == id))
                {
                    var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
                    var BrowserCulture = locale.RequestCulture.UICulture.ToString();
                    if (BrowserCulture == "en-US")

                        _toastNotification.AddErrorToastMessage("You cannot delete this Country");

                    else
                        _toastNotification.AddErrorToastMessage("لا يمكنك مسح هذه البلد");

                    
                    return Page();
                }
                var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/Country/" + country.CountryPic);
                country = await _context.Country.FindAsync(id);
                if (country != null)
                {
                    _context.Country.Remove(country);
                    await _context.SaveChangesAsync();
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                    _toastNotification.AddSuccessToastMessage("country Deleted successfully");

                }
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
