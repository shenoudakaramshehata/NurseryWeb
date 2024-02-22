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
    public class failedModel : PageModel
    {
        private readonly NurseryContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public NurseryMember NurseryMember { set; get; }
        public List<NurseryImage> NurseryImages { set; get; }
        public ApplicationUser user { set; get; }
        public List<NurserySubscription> subscribtion { get; set; }
        public failedModel(NurseryContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        //public async Task<IActionResult> OnGetAsync(string payment_type, string PaymentID, string Result, int OrderID, DateTime? PostDate, string TranID,
        //string Ref, string TrackID, string Auth)
        //{
        //    try
        //    {
        //        if (OrderID != 0)
        //        {
        //            NurseryMember = _context.NurseryMember.FirstOrDefault(e => e.NurseryMemberId == OrderID);
        //            user = await _userManager.FindByNameAsync(NurseryMember.Email);
        //            subscribtion = _context.NurserySubscription.Where(e => e.NurseryId == NurseryMember.NurseryMemberId).ToList();
        //            _context.NurserySubscription.RemoveRange(subscribtion);
        //            _context.NurseryMember.Remove(NurseryMember);
        //            await _userManager.DeleteAsync(user);
        //            _context.SaveChanges();
        //            return Page();
        //        }

        //        return RedirectToPage("SomethingwentError", new { Message = "SomeThing Went Error,try again" });
        //    }
        //    catch (Exception)
        //    {
        //        return RedirectToPage("SomethingwentError", new { Message = "Something went wrong" });
        //    }
        //}
        ///
        public async Task<IActionResult> OnGetAsync(string payment_type, string PaymentID, string Result, int OrderID, DateTime? PostDate, string TranID,
        string Ref, string TrackID, string Auth)
        {
            try
            {
                if (OrderID != 0)
                {
                    var subscribtionObj = _context.NurserySubscription.Where(e => e.NurserySubscriptionId == OrderID).FirstOrDefault();
                    NurseryMember = _context.NurseryMember.FirstOrDefault(e => e.NurseryMemberId == subscribtionObj.NurseryId);
                    user = await _userManager.FindByNameAsync(NurseryMember.Email);
                    subscribtion = _context.NurserySubscription.Where(e => e.NurseryId == NurseryMember.NurseryMemberId).ToList();
                    _context.NurserySubscription.Remove(subscribtionObj);
                    //_context.NurseryMember.Remove(NurseryMember);
                    //await _userManager.DeleteAsync(user);
                    _context.SaveChanges();
                    
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
