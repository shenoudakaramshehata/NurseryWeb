using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Nursery.Data;
using Nursery.Models;

namespace Nursery.Areas.Admin.Pages.Users
{
    public class DeleteNurseryModel : PageModel
    {
        private NurseryContext _context;
        private readonly IToastNotification _toastNotification;

        public DeleteNurseryModel(NurseryContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }
        [BindProperty]
        public NurseryMember nurseryDetails { get; set; }

        public Area areaDetails { get; set; }
        public Country country { get; set; }
        public City city { set; get; }

        public PaymentMethod paymentMethod { get; set; }

        public AgeCategory ageCategory { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {

            try
            {
                nurseryDetails = await _context.NurseryMember.Include(c => c.Area).Include(c => c.NurserySubscription).FirstOrDefaultAsync(m => m.NurseryMemberId == id);

                if (nurseryDetails == null)
                {
                    return Redirect("../Error");
                }
                areaDetails = await _context.Area.Include(c => c.City.Country).FirstOrDefaultAsync(m => m.AreaId == nurseryDetails.AreaId);
                //paymentMethod = _context.PaymentMethod.FirstOrDefault(c => c.PaymentMethodId == nurseryDetails.PaymentMethodId);
                ageCategory = _context.AgeCategory.FirstOrDefault(c => c.AgeCategoryId == nurseryDetails.AgeCategoryId);
                country = _context.Country.Find(nurseryDetails.CountryId);
                city = _context.City.Find(nurseryDetails.CityId);

            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
            }



            return Page();
        }
        public async Task<IActionResult> OnPost(int id)
        {
            try
            {
                var nurseryDetailsObj = await _context.NurseryMember.Include(c => c.NurserySubscription).Include(c => c.Area).FirstOrDefaultAsync(m => m.NurseryMemberId == id);
                if (nurseryDetailsObj == null)
                {
                    return Redirect("../Error");
                }
                var subChildren = _context.Child.Where(e => e.NurseryMemberId == nurseryDetailsObj.NurseryMemberId).ToList();
                 _context.Child.RemoveRange(subChildren);
                 _context.NurseryMember.Remove(nurseryDetailsObj);
                 await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Nursery Deleted Successfully");


            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong ");
            }
            return Redirect("./NurseryList");
        }

    }
}
