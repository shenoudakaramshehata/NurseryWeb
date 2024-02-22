using Nursery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nursery.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using MimeKit;

namespace Nursery.Pages
{
    public class successModel : PageModel
    {
        private readonly NurseryContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        public NurseryMember NurseryMember { set; get; }
        public NurserySubscription nurserySubscription { set; get; }

        public List<NurseryImage> NurseryImages { set; get; }
        public ApplicationUser user { set; get; }
        private IHostingEnvironment _env;
        public successModel(NurseryContext context, IEmailSender emailSender, UserManager<ApplicationUser> userManager, IHostingEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
            _env = env;
        }
        //public async Task<IActionResult> OnGetAsync(string payment_type,string PaymentID,string Result,int OrderID,DateTime? PostDate,string TranID,
        //string Ref, string TrackID, string Auth)
        //{
        //    try
        //    {
        //        if (OrderID != 0)
        //        {
        //            NurseryMember = _context.NurseryMember.FirstOrDefault(e => e.NurseryMemberId == OrderID);
        //            user = await _userManager.FindByNameAsync(NurseryMember.Email);
        //            NurseryImages = _context.NurseryImage.Where(e => e.NurseryId == NurseryMember.NurseryMemberId).ToList();
        //            NurseryMember.IsActive = true;
        //            NurseryMember.payment_type = payment_type;
        //            NurseryMember.PaymentID = PaymentID;
        //            NurseryMember.Result = Result;
        //            NurseryMember.PostDate = PostDate;
        //            NurseryMember.TranID = TranID;
        //            NurseryMember.Ref = Ref;
        //            NurseryMember.TrackID = TrackID;
        //            NurseryMember.Auth = Auth;
        //            double totalCost = _context.NurserySubscription.Where(e => e.NurseryId == NurseryMember.NurseryMemberId).FirstOrDefault().Price.Value;
        //            var UpdatedOrder = _context.NurseryMember.Attach(NurseryMember);
        //            UpdatedOrder.State = EntityState.Modified;
        //            _context.SaveChanges();
        //            await _emailSender.SendEmailAsync(
        //             NurseryMember.Email,
        //         $"Successfully Process : Total Payed Amount = {totalCost} KD ...",
        //            $"thanks for your paying.");
        //            return Page();
        //        }

        //        return RedirectToPage("SomethingwentError", new { Message = "SomeThing Went Error,try again" });

        //    }
        //    catch (Exception)
        //    {
        //        return RedirectToPage("SomethingwentError", new { Message = "Something went wrong" });
        //    }
        //}
        public async Task<IActionResult> OnGetAsync(string payment_type, string PaymentID, string Result, int OrderID, DateTime? PostDate, string TranID,
        string Ref, string TrackID, string Auth)
        {
            //var webRoot = _env.WebRootPath;

            //var pathToFile = _env.WebRootPath
            //       + Path.DirectorySeparatorChar.ToString()
            //       + "Templates"
            //       + Path.DirectorySeparatorChar.ToString()
            //       + "EmailTemplate"
            //       + Path.DirectorySeparatorChar.ToString()
            //       + "Email.html";
            //var builder = new BodyBuilder();
            //using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            //{

            //    builder.HtmlBody = SourceReader.ReadToEnd();

            //}
            //string messageBody = string.Format(builder.HtmlBody,
            //   "24/9/2022",
            //   "12/12/2022",
            //   "500",
            //   string.Format("{0:dddd, d MMMM yyyy}", DateTime.Now)
            //   );
            //await _emailSender.SendEmailAsync("ayamostafe5@gmail.com", "Nursery Subscription", messageBody);
            //return Page();
            try
            {


                if (OrderID != 0)
                {
                    nurserySubscription = _context.NurserySubscription.Where(e => e.NurserySubscriptionId == OrderID).FirstOrDefault();
                    var nurseryObj = _context.NurseryMember.Where(e => e.NurseryMemberId == nurserySubscription.NurseryId).FirstOrDefault();
                    user = await _userManager.FindByNameAsync(nurseryObj.Email);
                    NurseryImages = _context.NurseryImage.Where(e => e.NurseryId == nurseryObj.NurseryMemberId).ToList();
                    nurserySubscription.IsActive = true;
                    nurserySubscription.payment_type = payment_type;
                    nurserySubscription.PaymentID = PaymentID;
                    nurserySubscription.Result = Result;
                    nurserySubscription.PostDate = PostDate;
                    nurserySubscription.TranID = TranID;
                    nurserySubscription.Ref = Ref;
                    nurserySubscription.TrackID = TrackID;
                    nurserySubscription.Auth = Auth;
                    double totalCost = nurserySubscription.Price.Value;
                    var UpdatedOrder = _context.NurserySubscription.Attach(nurserySubscription);
                    UpdatedOrder.State = EntityState.Modified;
                    _context.SaveChanges();
                    var webRoot = _env.WebRootPath;

                    var pathToFile = _env.WebRootPath
                           + Path.DirectorySeparatorChar.ToString()
                           + "Templates"
                           + Path.DirectorySeparatorChar.ToString()
                           + "EmailTemplate"
                           + Path.DirectorySeparatorChar.ToString()
                           + "Email.html";
                    var builder = new BodyBuilder();
                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {

                        builder.HtmlBody = SourceReader.ReadToEnd();

                    }
                    string messageBody = string.Format(builder.HtmlBody,
                       nurserySubscription.StartDate.Value.ToShortDateString(),
                       nurserySubscription.EndDate.Value.ToShortDateString(),
                       totalCost,
                       string.Format("{0:dddd, d MMMM yyyy}", DateTime.Now)
                       );
                    await _emailSender.SendEmailAsync(nurseryObj.Email, "Nursery Subscription", messageBody);
                    return Page();
                }

                return RedirectToPage("SomethingwentError");

            }
            catch (Exception)
            {
                return RedirectToPage("SomethingwentError");
            }
        }
    }
    }


