using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nursery.Data;
using Nursery.Models;
using System.Collections.Generic;
using System.Linq;
namespace Nursery.Areas.Admin.Pages.SystemConfigration
{
    public class FeedBackDetailsModel : PageModel
    {
        private readonly NurseryContext _context;
        public ContactUs Msg;
        public FeedBackDetailsModel(NurseryContext context)
        {
            _context = context;

        }
        public void OnGet(int id)
        {
            Msg = _context.ContactUs.FirstOrDefault(e => e.ContactId == id);
        }
    }
}
