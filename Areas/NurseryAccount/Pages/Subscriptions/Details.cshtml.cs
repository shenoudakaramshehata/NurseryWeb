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
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Nursery.Areas.NurseryAccount.Pages.Subscriptions
{
    public class DetailsModel : PageModel
    {
        private readonly IToastNotification _toastNotification;
        private NurseryContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DetailsModel(NurseryContext context, UserManager<ApplicationUser> userManager, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
            _userManager = userManager;
        }

        public NurserySubscription Subscriptions { get; set; }

        public async Task<IActionResult> OnGetAsync(int id )
        {
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);
                Subscriptions = await _context.NurserySubscription
                                .Include(s => s.Plan).Include(c => c.Nursery)
                                .FirstOrDefaultAsync(m => m.NurserySubscriptionId == id);


                if (Subscriptions == null)
                {
                    return Redirect("../Error");
                }

                if (Subscriptions.NurseryId != user.EntityId)
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
