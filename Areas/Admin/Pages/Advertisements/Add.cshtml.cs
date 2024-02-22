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

namespace Nursery.Areas.Admin.Pages.Advertisements
{
    public class AddModel : PageModel
    {
        private NurseryContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;

        public AddModel(NurseryContext context, IWebHostEnvironment hostEnvironment,IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;

        }
   

        public void OnGet()
        {

        }
        public IActionResult OnGetFillNurseryList(string values)
        {

            int countryId = 0;
            bool checkTrue = int.TryParse(values, out countryId);
            var lookup = from i in _context.NurseryMember
                         orderby i.NurseryMemberId
                         where i.CountryId == countryId && i.NurserySubscription.OrderByDescending(a=>a.NurserySubscriptionId).FirstOrDefault().IsActive == true 
                         select new
                         {
                             Value = i.NurseryMemberId,
                             Text = i.NurseryTlEn
                         };
            return new JsonResult(lookup);
        }
        
        public IActionResult OnPost(Models.Adz model)
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
                    var nursery = _context.NurseryMember.FirstOrDefault(e => e.NurseryMemberId == int.Parse(model.EntityId));
                    if (nursery.CountryId != model.CountryId)
                    {
                        _toastNotification.AddErrorToastMessage("Select Nursery in the country Selected");
                        return Page();
                    }
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
                    string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/Adz");
                    string ext = Path.GetExtension(Response.HttpContext.Request.Form.Files[0].FileName);
                    uniqeFileName = Guid.NewGuid().ToString("N") + ext;
                    string uploadedImagePath = Path.Combine(uploadFolder, uniqeFileName);
                    using (FileStream fileStream = new FileStream(uploadedImagePath, FileMode.Create))
                    {
                        Response.HttpContext.Request.Form.Files[0].CopyTo(fileStream);
                    }
                    model.AdzPic = uniqeFileName;
                }
                _context.Adz.Add(model);
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Advertising Added successfully");

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
