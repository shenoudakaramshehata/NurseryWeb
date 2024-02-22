using Nursery.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nursery.Data;
using Microsoft.AspNetCore.Mvc;

namespace Nursery.Pages
{
    public class SubPayFailModel : PageModel
    {
        private readonly NurseryContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
   
        public ApplicationUser user { set; get; }
        public List<NurserySubscription> subscribtion { get; set; }
        public SubPayFailModel(NurseryContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        public IActionResult OnGetAsync(string payment_type, string PaymentID, string Result, int OrderID, DateTime? PostDate, string TranID,
        string Ref, string TrackID, string Auth)
        {
            try
            {
                if (OrderID != 0)
                {
                    var subscribtionObj = _context.NurserySubscription.Where(e => e.NurserySubscriptionId == OrderID).FirstOrDefault();
                    _context.NurserySubscription.Remove(subscribtionObj);
                    _context.SaveChanges();
                    return Page();
                }

                return RedirectToPage("SomethingwentError");
            }
            catch (Exception)
            {
                return RedirectToPage("SomethingwentError");
            }
        }
    }
}

