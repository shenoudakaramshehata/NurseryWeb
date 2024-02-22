using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nursery.Data;

namespace Nursery.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly NurseryContext _context;

        public IndexModel(NurseryContext context)
        {
            _context = context;
        }
        [BindProperty(SupportsGet = true)]

        public string url { get; set; }
        public void OnGet()
        {
            url = $"{this.Request.Scheme}://{this.Request.Host}";

        }
    }
}
