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

namespace Nursery.Areas.Admin.Pages.Subscriptions
{
    public class DeleteModel : PageModel
    {
        private NurseryContext _context;
        private readonly IToastNotification _toastNotification;
        public PaymentMethod paymentMethod { get; set; }
      
        public  static int SubId { get; set; }
        public DeleteModel(NurseryContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }


        public Nursery.Models.NurserySubscription Subscriptions { get; set; }
        public Models.PlanType planType;

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
                if (Subscriptions.PaymentMethodId != null)
                {
                    paymentMethod = _context.PaymentMethod.FirstOrDefault(c => c.PaymentMethodId == Subscriptions.PaymentMethodId);
                }
                if (Subscriptions.PlanTypeId != 0)
                {
                    planType = _context.PlanTypes.FirstOrDefault(e => e.PlanTypeId == Subscriptions.PlanTypeId);
                }
                SubId = Subscriptions.NurseryId;
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
            return Page();
        }


        public async Task<IActionResult> OnPostAsync(int id)
        {
            Subscriptions = await _context.NurserySubscription.FindAsync(id);
           
            if (!ModelState.IsValid)
            {
                return Page();
            }


            try
            {
                if (Subscriptions != null)
                {


                    _context.NurserySubscription.Remove(Subscriptions);
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Subscription Deleted successfully");

                   


                }
            }
            catch (Exception)

            {

                    _toastNotification.AddErrorToastMessage("Somthing Went Error");

               

                return Page();

            }

            return LocalRedirect("/admin/Subscriptions/index?id=" + SubId);
        }

    }
}
