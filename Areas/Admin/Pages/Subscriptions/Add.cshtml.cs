using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nursery.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace Nursery.Areas.Admin.Pages.Subscriptions
{
    public class AddModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private NurseryContext _context;
        private readonly IToastNotification _toastNotification;


        public AddModel(NurseryContext context,
            ApplicationDbContext db, IToastNotification toastNotification)
        {
            _context = context;
            _db = db;
            _toastNotification = toastNotification;
        }


        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public Nursery.Models.NurserySubscription Subscription { get; set; }
        public void OnGet(int NurseryId)
        {
            Id = NurseryId;
        }

        public async Task<IActionResult> OnPostAsync(Nursery.Models.NurserySubscription Subscription)
        {
            try
            {
                Subscription.StartDate = DateTime.Now;
                Subscription.NurseryId = Id;
                Subscription.PaymentMethodId = 2;
                var plan = await _context.Plan.FindAsync(Subscription.PlanId);
                var planTypeObj = await _context.PlanTypes.FindAsync(Subscription.PlanTypeId);
                Subscription.Price = plan.Price+ planTypeObj.Cost;
                Subscription.EndDate = DateTime.Now.AddMonths(plan.DurationInMonth.Value);
                await _context.NurserySubscription.AddAsync(Subscription);
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Subscription Added Successfully");

            }
            catch (Exception)
            {
                _toastNotification.AddSuccessToastMessage("Somthing Went Error");

            }

            return LocalRedirect("/admin/Subscriptions/index?id="+ Id);
        }
    }
}
