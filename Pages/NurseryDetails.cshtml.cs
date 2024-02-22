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


namespace Nursery.Pages
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

        public List<NurseryImage> nurseryImages = new List<NurseryImage>();

        public AgeCategory ageCategory { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {

            try
            {
                nurseryDetails = await _context.NurseryMember.Include(c => c.Area).Include(c => c.NurserySubscription).FirstOrDefaultAsync(m => m.NurseryMemberId == id);

                if (nurseryDetails == null)
                {
                    return Redirect("/SomethingwentError");
                }
                areaDetails = await _context.Area.Include(c => c.City.Country).FirstOrDefaultAsync(m => m.AreaId == nurseryDetails.AreaId);
                ageCategory = _context.AgeCategory.FirstOrDefault(c => c.AgeCategoryId == nurseryDetails.AgeCategoryId);
                country = _context.Country.Find(nurseryDetails.CountryId);
                city = _context.City.Find(nurseryDetails.CityId);
                nurseryImages = _context.NurseryImage.Where(e => e.NurseryImageId == id).ToList();

            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
            }



            return Page();
        }
       

    }
}
