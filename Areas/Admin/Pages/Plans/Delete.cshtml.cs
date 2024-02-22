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

namespace Nursery.Areas.Admin.Pages.Plans
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




        public async Task<IActionResult> OnPostAsync(int id)
        {
            plan = await _context.Plan.FindAsync(id);
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();
            if (!ModelState.IsValid)
            {
                return Page();
            }
          

            try
            {
                if (plan != null)
                {
                    if(_context.NurserySubscription.Any(e=>e.PlanId==plan.PlanId))
                    {
                        if (BrowserCulture == "en-US")

                            _toastNotification.AddErrorToastMessage("You cannot delete this Plan");

                        else
                            _toastNotification.AddErrorToastMessage("لا يمكن مسح هذه الخطة");
                    }
                    _context.Plan.Remove(plan);
                    await _context.SaveChangesAsync();
                 
                    if (BrowserCulture == "en-US")

                        _toastNotification.AddSuccessToastMessage("Plan Deleted successfully");

                    else
                        _toastNotification.AddSuccessToastMessage("تم مسح الخطة بنجاح");
                   


                }
            }
            catch (Exception)

            {
                if (BrowserCulture == "en-US")

                    _toastNotification.AddErrorToastMessage("You cannot delete this Plan");

                else
                    _toastNotification.AddSuccessToastMessage("لا يمكن مسح هذه الخطة");
               
                return Page();

            }

            return RedirectToPage("./Index");
        }

    }
}
