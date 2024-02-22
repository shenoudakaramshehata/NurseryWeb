   using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Nursery.Data;

namespace Nursery.Areas.NurseryAccount.Pages.Subscriptions
{

    public class IndexModel : PageModel
    {
        private readonly IToastNotification _toastNotification;
        private NurseryContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(NurseryContext context, UserManager<ApplicationUser> userManager, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
            _userManager = userManager;


        }
        [BindProperty(SupportsGet = true)]
        public int nurseryId { get; set; }
        public async Task<IActionResult> OnGet()
        {
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);
                nurseryId = user.EntityId;

            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }
            return Page();
        }
    }
}
