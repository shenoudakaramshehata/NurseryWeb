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
using Nursery.Data;
using Nursery.Models;
using NToastNotify;

namespace Nursery.Areas.Admin.Pages.Advertisements
{
    public class DeleteModel : PageModel
    {

        private NurseryContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IStringLocalizer<SharedResource> _sharedResource;
        private readonly IToastNotification _toastNotification;
        public DeleteModel(NurseryContext context, IWebHostEnvironment hostEnvironment
            , IStringLocalizer<SharedResource> sharedResource, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _sharedResource = sharedResource;
            _toastNotification = toastNotification;
        }
        [BindProperty(SupportsGet = true)]
        public bool ArLang { get; set; }

        [BindProperty]
        public Adz adz { get; set; }
        [BindProperty]
        public string EntityName { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
          
            adz =await _context.Adz.Include(c => c.EntityType).Include(c => c.Country).Where(c => c.AdzId == id).FirstOrDefaultAsync();

            if (adz == null)
            {
                return Redirect("../Error");
            }
            if (adz.EntityTypeId == 2)
            {
                EntityName = adz.EntityId;
            }
            else
            {
                EntityName = _context.NurseryMember.Find(int.Parse(adz.EntityId))?.NurseryTlAr;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            adz = await _context.Adz.Include(c => c.EntityType).Include(c => c.Country).Where(c => c.AdzId == id).FirstOrDefaultAsync();

            if (adz == null)
            {
                return Redirect("../Error");
            }
            if (adz.EntityTypeId == 2)
            {
                EntityName = adz.EntityId;
            }
            else
            {
                EntityName = _context.NurseryMember.Find(int.Parse(adz.EntityId))?.NurseryTlAr;
            }
            try
            {
                _context.Adz.Remove(adz);
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Advertisement Deleted Successfully");
                return RedirectToPage("Index");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went Error Try again ");
                return Page();
            }
            
        }


    }
}
