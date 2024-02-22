using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Nursery.Data;
using Nursery.Models;

namespace Nursery.Areas.NurseryAccount.Pages
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
        [BindProperty]
        public NurseryMember nurseryDetails { get; set; }
        [BindProperty]
        public Area areaDetails { get; set; }

        [BindProperty]
        public PaymentMethod paymentMethod { get; set; }
        [BindProperty]
        public AgeCategory ageCategory { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
          
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId!=null)
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    nurseryDetails = await _context.NurseryMember.Include(c => c.Area).FirstOrDefaultAsync(m => m.NurseryMemberId == user.EntityId);

                }

                if (nurseryDetails == null)
                {
                    return Redirect("NurseryAccount/Error");
                }
                areaDetails = await _context.Area.Include(c => c.City.Country).FirstOrDefaultAsync(m => m.AreaId == nurseryDetails.AreaId);
                //paymentMethod = _context.PaymentMethod.FirstOrDefault(c => c.PaymentMethodId == nurseryDetails.PaymentMethodId);
                ageCategory = _context.AgeCategory.FirstOrDefault(c => c.AgeCategoryId == nurseryDetails.AgeCategoryId);

            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
            }



            return Page();
        }

    }
}
