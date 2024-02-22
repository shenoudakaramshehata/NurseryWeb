   using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Nursery.Data;
using Nursery.Models;

namespace Nursery.Areas.Admin.Pages.Subscriptions
{
    public class IndexModel : PageModel
    {
       public List<NurserySubscription> NurserySubscriptions;
        private NurseryContext _context;
        private readonly IToastNotification _toastNotification;
        [BindProperty]
        public int NurseryId { get; set; }

        public IndexModel(NurseryContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }
        public void OnGet(int id)
        {
            NurserySubscriptions = _context.NurserySubscription.Where(a => a.NurseryId == id).ToList();
            NurseryId = id;
        }
    }
}
