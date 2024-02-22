using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Nursery.Data;
using Nursery.Models;
using Microsoft.AspNetCore.Authorization;
using NToastNotify;

namespace Nursery.Areas.Admin.Pages.Subscriptions
{
    public class DetailsModel : PageModel
    {
        private NurseryContext _context;
        private readonly IToastNotification _toastNotification;
        public PaymentMethod paymentMethod { get; set; }
        public DetailsModel(NurseryContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }


        public Nursery.Models.NurserySubscription Subscriptions { get; set; }
         public  Models.PlanType planType;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Subscriptions = await _context.NurserySubscription
                                .Include(s => s.Plan).Include(c => c.Nursery)
                                .FirstOrDefaultAsync(m => m.NurserySubscriptionId == id);

                if (Subscriptions == null)
                {
                    return Redirect("../Error");
                }
                if (Subscriptions.PaymentMethodId!=null)
                {
                    paymentMethod = _context.PaymentMethod.FirstOrDefault(c => c.PaymentMethodId == Subscriptions.PaymentMethodId);
                }
                if (Subscriptions.PlanTypeId != 0)
                {
                    planType = _context.PlanTypes.FirstOrDefault(e => e.PlanTypeId == Subscriptions.PlanTypeId);
                }
               
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostPayment(int id)
        {
            try
            {
                Subscriptions = await _context.NurserySubscription
                                  .Include(s => s.Plan).Include(c => c.Nursery)
                                  .FirstOrDefaultAsync(m => m.NurserySubscriptionId == id);

                if (Subscriptions == null)
                {
                    return Redirect("../Error");
                }
                if (Subscriptions.PaymentMethodId != null)
                {
                    paymentMethod = _context.PaymentMethod.FirstOrDefault(c => c.PaymentMethodId == Subscriptions.PaymentMethodId);
                }
                Subscriptions.IsActive = true;
                _context.Attach(Subscriptions).State = EntityState.Modified;
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
            return Page();
        }
    }
}
