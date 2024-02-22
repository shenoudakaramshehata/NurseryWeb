using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nursery.Data;
using Nursery.Models;
using System.Collections.Generic;
using System.Linq;

namespace Nursery.Areas.Admin.Pages.SystemConfigration
{
    public class UsersFeedBacksModel : PageModel
    {
        private readonly NurseryContext _context;
        public List<ContactUs> Msgs;
        public UsersFeedBacksModel(NurseryContext context)
        {
            _context = context;
           
        }
        public void OnGet()
        {
            Msgs = _context.ContactUs.ToList();
        }
    }
}
