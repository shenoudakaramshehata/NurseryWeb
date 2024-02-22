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

namespace Nursery.Areas.Admin.Pages.Banners
{
    public class AddModel : PageModel
    {
        private NurseryContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;

        public AddModel(NurseryContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;

        }


        public void OnGet()
        {

        }
        public IActionResult OnPost(Models.Banner model)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                if (model.EntityTypeId==1)
                {
                    model.EntityId=Request.Form["NurseryId"];
                }

                if (model.EntityTypeId == 2)
                {
                    if (model.EntityId == null|| model.EntityId=="")
                    {
                        _toastNotification.AddErrorToastMessage("enter link");
                        return Page();
                    }

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
                    model.BannerPic = uniqeFileName;
                }
                _context.Banner.Add(model);
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Banner Added successfully");
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
