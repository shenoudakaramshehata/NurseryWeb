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
    public class NurseryDetailsModel : PageModel
    {
        private NurseryContext _context;
        private readonly IToastNotification _toastNotification;

        public NurseryDetailsModel(NurseryContext context, IToastNotification toastNotification)
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
                nurseryDetails = await _context.NurseryMember.Include(c=>c.Area).Include(c=>c.NurserySubscription).FirstOrDefaultAsync(m => m.NurseryMemberId == id);
                
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
        public async Task<IActionResult> OnPostPayment(int id)
        {
            try
            {
                nurseryDetails = await _context.NurseryMember.Include(c => c.Area).FirstOrDefaultAsync(m => m.NurseryMemberId == id);
                if (nurseryDetails == null)
                {
                    return Redirect("../Error");
                }
                areaDetails = await _context.Area.Include(c => c.City.Country).FirstOrDefaultAsync(m => m.AreaId == nurseryDetails.AreaId);
                //paymentMethod = _context.PaymentMethod.FirstOrDefault(c => c.PaymentMethodId == nurseryDetails.PaymentMethodId);
                ageCategory = _context.AgeCategory.FirstOrDefault(c => c.AgeCategoryId == nurseryDetails.AgeCategoryId);
                country = _context.Country.Find(nurseryDetails.CountryId);
                city = _context.City.Find(nurseryDetails.CityId);
                //nurseryDetails.IsActive = true;
                _context.Attach(nurseryDetails).State = EntityState.Modified;
                await _context.SaveChangesAsync();

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
                areaDetails = await _context.Area.Include(c => c.City.Country).FirstOrDefaultAsync(m => m.AreaId == nurseryDetailsObj.AreaId);
                //paymentMethod = _context.PaymentMethod.FirstOrDefault(c => c.PaymentMethodId == nurseryDetails.PaymentMethodId);
                ageCategory = _context.AgeCategory.FirstOrDefault(c => c.AgeCategoryId == nurseryDetailsObj.AgeCategoryId);
                country = _context.Country.Find(nurseryDetailsObj.CountryId);
                city = _context.City.Find(nurseryDetailsObj.CityId);
                if (nurseryDetailsObj.IsSlider == false)
                {
                    nurseryDetailsObj.IsSlider = nurseryDetails.IsSlider;
                }
                nurseryDetailsObj.IsActive = nurseryDetails.IsActive;
                _context.Attach(nurseryDetailsObj).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Nursery Edited Successfully");


            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
            return Redirect("./NurseryList");
        }
        

}
}
