using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Nursery.Data;
using Nursery.Models;

namespace Nursery.Areas.Admin.Pages.Cities
{
    public class DetailsModel : PageModel
    {
        private NurseryContext _context;
        private readonly IToastNotification _toastNotification;

        public DetailsModel(NurseryContext context, IToastNotification toastNotification)
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
                city = await _context.City.Include(c=>c.Country).FirstOrDefaultAsync(m => m.CityId == id);
                
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

    }
}
