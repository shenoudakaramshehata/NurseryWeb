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

namespace Nursery.Areas.Admin.Pages.Plans
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


        [BindProperty]
        public Plan plan { get; set; }


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

           

            return Page();
        }




        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            try
            {
                var model = _context.Plan.Where(c => c.PlanId == id).FirstOrDefault();
                if (model == null)
                {
                    return Page();
                }
              

                model.IsActive = plan.IsActive;
                model.PlanTlAr = plan.PlanTlAr;
                model.PlanTlEn = plan.PlanTlEn;
                model.DurationInMonth = plan.DurationInMonth;
                model.CountryId = plan.CountryId;
                model.Price = plan.Price;

                _context.Attach(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Plann Edited successfully");

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
