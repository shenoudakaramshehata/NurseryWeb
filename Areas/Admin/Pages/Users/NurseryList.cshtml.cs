using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Nursery.Data;
using Nursery.Models;

namespace Nursery.Areas.Admin.Pages.Users
{
    public class NurseryListModel : PageModel
    {
        private NurseryContext _context;

        public NurseryListModel(NurseryContext context)
        {
            _context = context;
        }
        
        public void OnGet()
        {
            
        }
    }
}
