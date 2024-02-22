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

namespace Nursery.Areas.Admin.Pages.Countries
{
    public class IndexModel : PageModel
    {


        private NurseryContext _context;
        
        public IndexModel(NurseryContext context)
        {
            _context = context;
            
        }
        [BindProperty(SupportsGet = true)]
        public List<Country> countryList { get; set; }
        public async Task<IActionResult> OnGet()
        {
            countryList =await  _context.Country.ToListAsync();
            return Page();
        }
    }
}
    