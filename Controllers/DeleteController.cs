using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Nursery.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nursery.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DeleteController : Controller
    {
        private readonly NurseryContext _context;
      
        private readonly IWebHostEnvironment _hostEnvironment;


        public DeleteController(NurseryContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
         
            _hostEnvironment = hostEnvironment;


        }


        [HttpPost]
        public async Task<object> DeleteAdz(int id)
        {
            try
            {
                
               
                var adz = await _context.Adz.FindAsync(id);

                if (adz == null)
                    return 0;
              
                _context.Adz.Remove(adz);
                _context.SaveChanges();
                return 1;
            }
            catch (Exception)

            {
                return -1;

            }
        }


    }
}
