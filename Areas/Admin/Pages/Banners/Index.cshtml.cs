using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nursery.Data;
using Nursery.Models;

namespace Nursery.Areas.Admin.Pages.Banners
{
    public class IndexModel : PageModel
    {

        private NurseryContext _context;
        public IndexModel(NurseryContext context)
        {
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public bool ArLang { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<Banner> BannerLst { get; set; }

        public void OnGet()
        {
            BannerLst = _context.Banner.ToList();
            foreach (var item in BannerLst)
            {
                if (item.EntityId==""|| item.EntityId==null)
                  continue;
                
                if (item.EntityTypeId==1)
                {
                    var id = Convert.ToInt32(item.EntityId);

                    item.EntityId = _context.NurseryMember.FirstOrDefault(c => c.NurseryMemberId == id)?.NurseryTlAr;
                }
              

            }
        }
    }
}
