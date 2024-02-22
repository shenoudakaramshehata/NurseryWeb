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

namespace Nursery.Areas.Admin.Pages.Plans
{
    public class IndexModel : PageModel
    {



        private NurseryContext _context;
        private readonly IToastNotification _toastNotification;
        public IndexModel(NurseryContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }

        [BindProperty(SupportsGet = true)]
        public List<Plan> planList { get; set; }
        public async Task<IActionResult> OnGet()
        {
            try
            {
                planList = await _context.Plan.ToListAsync();
            }
            catch (Exception)
            {


                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
                return Page();

        }
    }
}
