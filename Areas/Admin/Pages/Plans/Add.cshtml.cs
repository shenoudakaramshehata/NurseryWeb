using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Nursery.Data;
using Nursery.Models;

namespace Nursery.Areas.Admin.Pages.Plans
{
    public class AddModel : PageModel
    {
        private NurseryContext _context;
        private readonly IToastNotification _toastNotification;

        public AddModel(NurseryContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }

        public void OnGet()
        { }
        public IActionResult OnPost(Plan model)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                _context.Plan.Add(model);
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Plan Added successfully");
            }
            catch (Exception)
            {


                _toastNotification.AddErrorToastMessage("Something went wrong");
                return Page();
            }


            return Redirect("./Index");

        }
    }
}
