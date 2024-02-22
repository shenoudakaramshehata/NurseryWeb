using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NToastNotify;
using Nursery.Data;
using Nursery.Models;


namespace Nursery.Areas.Admin.Pages.SystemConfigration
{
    public class DeleteFeedBackModel : PageModel
    {
        private readonly NurseryContext _context;
        
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;
        public DeleteFeedBackModel(NurseryContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;

        }
        [BindProperty]
        public ContactUs Msg { get; set; }
     
  


        public IActionResult OnGetAsync(int id)
        {

            try
            {
                Msg = _context.ContactUs.FirstOrDefault(e => e.ContactId == id);

                if (Msg == null)
                {
                    return Redirect("../Error");
                }
            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }



            return Page();
        }




        public async Task<IActionResult> OnPostAsync(int id)
        {


            try
            {
                Msg = _context.ContactUs.FirstOrDefault(e => e.ContactId == id);
                    _context.ContactUs.Remove(Msg);
                    await _context.SaveChangesAsync();
                    
                    _toastNotification.AddSuccessToastMessage("Message Deleted successfully");

            }
            
            catch (Exception)

            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
                return Page();

            }

            return RedirectToPage("./UsersFeedBacks");
        }

    }
}
