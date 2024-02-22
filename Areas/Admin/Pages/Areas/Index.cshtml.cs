using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nursery.Data;
using Nursery.Models;
using Microsoft.EntityFrameworkCore;

namespace Nursery.Areas.Admin.Pages.Areas
{
    public class IndexModel : PageModel
    {


        private NurseryContext _context;
        
        public IndexModel(NurseryContext context)
        {
            _context = context;
            
        }
      
        public ActionResult OnGet()
        {
            return Page();
        }
    }
}
    