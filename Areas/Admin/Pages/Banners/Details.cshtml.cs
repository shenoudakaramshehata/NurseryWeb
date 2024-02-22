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

namespace Nursery.Areas.Admin.Pages.Banners
{
    public class DetailsModel : PageModel
    {
        private NurseryContext _context;
        public DetailsModel(NurseryContext context)
        {
            _context = context;
        }
       

        [BindProperty]
        public Banner banner { get; set; }
        [BindProperty]
        public string EntityName { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return Redirect("../Error");
            }
            banner = _context.Banner.Include(c => c.EntityType).Where(c => c.BannerId == id).FirstOrDefault();
            if (banner == null)
            {
                return Redirect("../Error");
            }
            if (banner.EntityTypeId == 2)
            {
                EntityName = banner.EntityId;
            }
            else
            {
                EntityName = _context.NurseryMember.Find(int.Parse(banner.EntityId))?.NurseryTlAr;
            }
           
            return Page();
        }


    }
}
