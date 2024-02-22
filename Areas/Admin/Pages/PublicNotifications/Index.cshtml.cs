using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nursery.Data;
using Nursery.Models;
using Microsoft.AspNetCore.Localization;

namespace Nursery.Areas.Admin.Pages.PublicNotifications
{
    public class IndexModel : PageModel
    {

        private NurseryContext _context;
        public IndexModel(NurseryContext context)
        {
            _context = context;
        }
        
        [BindProperty(SupportsGet = true)]
        public List<PublicNotification> PublicNotificationLst { get; set; }

        public void OnGet()
        {
            PublicNotificationLst = _context.PublicNotification.ToList();
            
            foreach (var item in PublicNotificationLst)
            {
                    item.EntityNameAr = _context.NurseryMember.FirstOrDefault(c => c.NurseryMemberId == item.EntityId)?.NurseryTlAr;
                    item.EntityNameEn = _context.NurseryMember.FirstOrDefault(c => c.NurseryMemberId == item.EntityId)?.NurseryTlEn;
                
            }
        }
    }
}
