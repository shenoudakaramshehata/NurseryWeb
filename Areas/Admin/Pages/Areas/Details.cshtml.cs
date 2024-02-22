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

namespace Nursery.Areas.Admin.Pages.Areas
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
        public Area area { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
           
            try
            {
                area = await _context.Area.Include(c => c.City.Country).FirstOrDefaultAsync(m => m.AreaId == id);
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

    }
}
