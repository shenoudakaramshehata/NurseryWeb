using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NToastNotify;
using Nursery.Data;
using Nursery.Models;

namespace Nursery.Areas.Admin.Pages.Plans
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
        public Plan plan { get; set; }

        [BindProperty]
        public string countryName { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
        
            try
            {
                plan = await _context.Plan.FirstOrDefaultAsync(m => m.PlanId == id);
                if (plan == null)
                {
                    return Redirect("../Error");
                }
            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
            countryName = _context.Country.FirstOrDefault(c => c.CountryId == plan.CountryId)?.CountryTlAr;
            return Page();
        }





    }
}
