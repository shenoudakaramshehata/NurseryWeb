using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Nursery.Data;
using Nursery.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nursery.Areas.Admin.Pages.Advertisements
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
        public Adz adz { get; set; }
        [BindProperty]
        public int? EntityId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                if (id == null)
                {
                    return Redirect("../Error");
                }
                adz = _context.Adz.Where(c => c.AdzId == id).FirstOrDefault();
                if (adz == null)
                {
                    return Redirect("../Error");
                }
                if (adz.EntityTypeId == 1)
                {
                    EntityId = int.Parse(adz.EntityId);
                    adz.EntityId = "";
                }
              

                var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
                var BrowserCulture = locale.RequestCulture.UICulture.ToString();
                if (BrowserCulture == "en-US")

                    ArLang = false;

                else
                    ArLang = true;


                return Page();
            

            }
            catch(Exception e)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");

                return Page();
            }
           
        }
        public IActionResult OnGetFillNurseryList(string values)
        {

            int countryId = 0;
            bool checkTrue = int.TryParse(values, out countryId);
            var lookup = from i in _context.NurseryMember
                         orderby i.NurseryMemberId
                         where i.CountryId == countryId && i.NurserySubscription.OrderByDescending(a => a.NurserySubscriptionId).FirstOrDefault().IsActive == true
                         select new
                         {
                             Value = i.NurseryMemberId,
                             Text = i.NurseryTlEn
                         };
            return new JsonResult(lookup);
        }
        public IActionResult OnPost(int? id)
        
        {
            

            try
            {
                var model = _context.Adz.Where(c => c.AdzId == id).FirstOrDefault();
                if (model==null)
                {
                    return Redirect("../Error");
                }
                
                if (adz.EntityTypeId == 1)
                {
                    model.EntityId = Request.Form["NurseryId"];
                    var nursery = _context.NurseryMember.FirstOrDefault(e => e.NurseryMemberId ==int.Parse(model.EntityId));
                    if (nursery.CountryId!=adz.CountryId)
                    {
                        _toastNotification.AddErrorToastMessage("Select Nursery in this country");
                        return Page();
                    }
                    
                }
               
                if (adz.EntityTypeId == 2)
                {
                    if (adz.EntityId == null|| adz.EntityId=="")
                    {

                        _toastNotification.AddErrorToastMessage("enter link");
                        adz.AdzPic = model.AdzPic;
                        return Page();
                    }
                    model.EntityId = adz.EntityId;
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
                    var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/Adz/" + model.AdzPic);
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                    model.AdzPic = uniqeFileName;
                }
                model.EntityTypeId = adz.EntityTypeId;
                model.AdzIsActive = adz.AdzIsActive;
                model.AdzOrderIndex = adz.AdzOrderIndex;
                model.CountryId = adz.CountryId;
                _context.Attach(model).State = EntityState.Modified;
                 _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Advertising Edited successfully");
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
