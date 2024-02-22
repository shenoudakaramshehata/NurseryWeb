using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nursery.Data;
using Nursery.Models;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Nursery.Areas.Admin.Pages.Cities
{
    public class IndexModel : PageModel
    {
        private NurseryContext _context;
        private readonly IToastNotification _toastNotification;

        public IndexModel(NurseryContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }

        [BindProperty(SupportsGet = true)]
        public List<City> cityList { get; set; }
        public async Task<IActionResult> OnGet()
        {
            try
            {
                cityList = await _context.City.ToListAsync();
            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }
            return Page();

        }
    }
}
    