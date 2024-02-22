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

namespace Nursery.Areas.Admin.Pages.Banners
{
    public class DeleteModel : PageModel
    {

        private NurseryContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;

        public DeleteModel(NurseryContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;

        }
        [BindProperty(SupportsGet = true)]
        public bool ArLang { get; set; }

        [BindProperty]
        public Banner banner { get; set; }
        [BindProperty]
        public string EntityName { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return Redirect("../Error");
            }
            banner = _context.Banner.Include(c=>c.EntityType).Where(c => c.BannerId == id).FirstOrDefault();
            if (banner == null)
            {
                return Redirect("../Error");
            }
            if (banner.EntityTypeId == 2)
            {
                EntityName = banner.EntityId;
            }
            else
            {
                EntityName = _context.NurseryMember.Find(int.Parse(banner.EntityId))?.NurseryTlAr;
            }
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();
            if (BrowserCulture == "en-US")

                ArLang = false;

            else
                ArLang = true;


            return Page();
        }




        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (id == null)
            {
                return Redirect("../Error");
            }

            try
            {
                var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/Banner/" + banner.BannerPic);

                
                banner = await _context.Banner.FindAsync(id);
                if (banner != null)
                {
                    _context.Banner.Remove(banner);
                    await _context.SaveChangesAsync();
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                    _context.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("Banner Deleted successfully");

                }
            }
            catch (Exception)

            {
                _toastNotification.AddErrorToastMessage("You cannot delete this item");
                return Page();

            }

            return RedirectToPage("./Index");
        }

    }
}
