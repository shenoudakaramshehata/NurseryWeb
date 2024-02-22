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

namespace Nursery.Areas.Admin.Pages.Users
{
    public class ParentListModel : PageModel
    {
        private NurseryContext _context;
        private readonly IToastNotification _toastNotification;

        public ParentListModel(NurseryContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }
        [BindProperty(SupportsGet = true)]
        public List<Parent> parentList { get; set; }
        public async Task<IActionResult> OnGet()
        {
            try
            {
                parentList = await _context.Parent.ToListAsync();
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
            return Page();
        }
    }
}
