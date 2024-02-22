using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Nursery.Data;
using Nursery.Models;

namespace Nursery.Areas.Admin.Pages.Advertisements
{
    public class DetailsModel : PageModel
    {
        private NurseryContext _context;
        public DetailsModel(NurseryContext context)
        {
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public string countryName { get; set; }

        [BindProperty]
        public Adz adz { get; set; }
        [BindProperty]
        public string EntityName { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return Redirect("../Error");
            }
            adz = _context.Adz.Include(c => c.EntityType).Include(c => c.Country).Where(c => c.AdzId == id).FirstOrDefault();
            if (adz == null)
            {
                return Redirect("../Error");
            }
            if (adz.EntityTypeId == 2)
            {
                EntityName = adz.EntityId;
            }
            else
            {
                EntityName = _context.NurseryMember.Find(int.Parse(adz.EntityId))?.NurseryTlAr;
            }
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();
            if (BrowserCulture == "en-US")
            {
                countryName = adz.Country.CountryTlEn;
            }
            else

            {
                countryName = adz.Country.CountryTlAr;
            }

            return Page();
        }


    }
}
