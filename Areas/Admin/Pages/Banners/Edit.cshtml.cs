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
using NToastNotify;
using Nursery.Data;
using Nursery.Models;

namespace Nursery.Areas.Admin.Pages.Banners
{
    public class EditModel : PageModel
    {
        private NurseryContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;

        public EditModel(NurseryContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;

        }
        [BindProperty(SupportsGet = true)]
        public bool ArLang { get; set; }
        [BindProperty]
        public Banner panner { get; set; }
        [BindProperty]
        public int EntityId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return Redirect("../Error");
            }
            panner = _context.Banner.Where(c => c.BannerId == id).FirstOrDefault();
            if (panner == null)
            {
                return Redirect("../Error");
            }
            if (panner.EntityTypeId==2)
            {
                EntityId = 0;
            }
            else
            {
                EntityId = int.Parse(panner.EntityId);
                panner.EntityId = "";
            }

            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();
            if (BrowserCulture == "en-US")
            
                ArLang = false;
               
            else
              ArLang = true;


            return Page();
        }
        public IActionResult OnPost(int? id)
        
        {
            

            try
            {
                var model = _context.Banner.Where(c => c.BannerId == id).FirstOrDefault();
                if (model==null)
                {
                    return Redirect("../Error");
                }
                
                if (panner.EntityTypeId == 1)
                {
                    model.EntityId = Request.Form["NurseryId"];
                }
               
                if (panner.EntityTypeId == 2)
                {
                    if (panner.EntityId == null|| panner.EntityId=="")
                    {
                        _toastNotification.AddErrorToastMessage("enter link");

                        panner.BannerPic = model.BannerPic;
                       
                        return Page();
                    }
                    model.EntityId = panner.EntityId;
                }

                var uniqeFileName = "";

                if (Response.HttpContext.Request.Form.Files.Count() > 0)
                {
                    string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/Banner");
                    string ext = Path.GetExtension(Response.HttpContext.Request.Form.Files[0].FileName);
                    uniqeFileName = Guid.NewGuid().ToString("N") + ext;
                    string uploadedImagePath = Path.Combine(uploadFolder, uniqeFileName);
                    using (FileStream fileStream = new FileStream(uploadedImagePath, FileMode.Create))
                    {
                        Response.HttpContext.Request.Form.Files[0].CopyTo(fileStream);
                    }
                    var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/Banner/" + model.BannerPic);
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                    model.BannerPic = uniqeFileName;
                }
                model.EntityTypeId = panner.EntityTypeId;
                model.BannerIsActive = panner.BannerIsActive;
                model.BannerOrderIndex = panner.BannerOrderIndex;
                _context.Attach(model).State = EntityState.Modified;
                 _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Banner Edited successfully");

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
