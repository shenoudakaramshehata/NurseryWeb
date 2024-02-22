using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Nursery.Data;
using Nursery.Models;
using NToastNotify;
using Nursery.Entities.Notification;

namespace Nursery.Areas.Admin.Pages.PublicNotifications
{
    public class SendModel : PageModel
    {
        private NurseryContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly INotificationService _notificationService;

        public SendModel(NurseryContext context, INotificationService notificationService, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
            _notificationService = notificationService;


        }
        [BindProperty]
        public PublicNotification publicNotification { get; set; }

        public IActionResult OnGetAsync(int id)
        {

            //publicNotification = _context.PublicNotification.Include(c => c.EntityType).Include(c => c.Country).Where(c => c.PublicNotificationId == id).FirstOrDefault();
            publicNotification = _context.PublicNotification.Include(c => c.EntityTypeNotify).Include(c => c.Country).Where(c => c.PublicNotificationId == id).FirstOrDefault();

            if (publicNotification == null)
            {
                return Redirect("../Error");
            }
                publicNotification.EntityNameAr = _context.NurseryMember.FirstOrDefault(c => c.NurseryMemberId == publicNotification.EntityId)?.NurseryTlAr;
                publicNotification.EntityNameEn = _context.NurseryMember.FirstOrDefault(c => c.NurseryMemberId == publicNotification.EntityId)?.NurseryTlEn;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {



            try
            {
                //publicNotification = _context.PublicNotification.Include(c => c.EntityType).Include(c => c.Country).Where(c => c.PublicNotificationId == id).FirstOrDefault();
                publicNotification = _context.PublicNotification.Include(c => c.EntityTypeNotify).Include(c => c.Country).Where(c => c.PublicNotificationId == id).FirstOrDefault();

                if (publicNotification == null)
                {
                    return Redirect("../Error");
                }
                publicNotification.EntityNameAr = _context.NurseryMember.FirstOrDefault(c => c.NurseryMemberId == publicNotification.EntityId)?.NurseryTlAr;
                publicNotification.EntityNameEn = _context.NurseryMember.FirstOrDefault(c => c.NurseryMemberId == publicNotification.EntityId)?.NurseryTlEn;
                if (publicNotification != null)
                {

                    var PublicDeviceList = _context.PublicDevice.Where(c => c.CountryId == publicNotification.CountryId).ToList();
                    foreach (var item in PublicDeviceList)
                    {

                        var notificationModel = new NotificationModel();
                        notificationModel.DeviceId = item.DeviceId;
                        notificationModel.IsAndroiodDevice = item.IsAndroiodDevice;
                        notificationModel.Title = publicNotification.Title;
                        notificationModel.Body = publicNotification.Body;
                        notificationModel.EntityId = publicNotification.EntityId;
                        //notificationModel.EntityTypeId= publicNotification.EntityTypeId;
                        var result = await _notificationService.SendNotification(notificationModel);   
                        if (result.IsSuccess)
                        {
                            var publicNotificationDeviceExiest = _context.PublicNotificationDevice.Any(c => c.PublicNotificationId == publicNotification.PublicNotificationId
                             && c.PublicDeviceId == item.PublicDeviceId);
                            if (!publicNotificationDeviceExiest)
                            {
                                var publicNotificationDevice = new PublicNotificationDevice()
                                {
                                    PublicNotificationId = publicNotification.PublicNotificationId,
                                    PublicDeviceId = item.PublicDeviceId,
                                    IsRead = false
                                };
                                _context.PublicNotificationDevice.Add(publicNotificationDevice);
                                _context.SaveChanges();
                            }
                           

                        }
                    }

                }
                _toastNotification.AddSuccessToastMessage("Notification Sent successfully");

            }
            catch (Exception)

            {

                _toastNotification.AddErrorToastMessage("Sothing want Be Wrong");
                return Page();

            }

            return RedirectToPage("./Index");
        }
    }
}
