using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Nursery.Data;
using Nursery.Models;
using Microsoft.AspNetCore.Localization;

namespace Nursery.Controllers
{
    [Route("api/[controller]/[action]")]
    public class NurserySubscriptionsController : Controller
    {
        private NurseryContext _context;

        public NurserySubscriptionsController(NurseryContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var nurserysubscription = _context.NurserySubscription.Select(i => new {
                i.NurserySubscriptionId,
                i.NurseryId,
                i.PlanId,
                i.StartDate,
                i.EndDate,
                i.Price,
                i.Remarks
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "NurserySubscriptionId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(nurserysubscription, loadOptions));
        }
         [HttpGet]
        public async Task<IActionResult> GetByNurseryId(int nurseryId, DataSourceLoadOptions loadOptions) {
            var nurserysubscription = _context.NurserySubscription.Where(c=>c.NurseryId== nurseryId).Select(i => new {
                i.NurserySubscriptionId,
                i.NurseryId,
                i.PlanId,
                i.StartDate,
                i.EndDate,
                i.Price,
                i.Remarks
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "NurserySubscriptionId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(nurserysubscription, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new NurserySubscription();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.NurserySubscription.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.NurserySubscriptionId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.NurserySubscription.FirstOrDefaultAsync(item => item.NurserySubscriptionId == key);
            if(model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task Delete(int key) {
            var model = await _context.NurserySubscription.FirstOrDefaultAsync(item => item.NurserySubscriptionId == key);

            _context.NurserySubscription.Remove(model);
            await _context.SaveChangesAsync();
        }


      
        [HttpGet]
        public async Task<IActionResult> NurseryMemberLookup(DataSourceLoadOptions loadOptions)
        {

            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.NurseryMember
                               orderby i.NurseryTlEn
                             
                               select new
                               {
                                   Value = i.NurseryMemberId,
                                   Text = i.NurseryTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.NurseryMember
                           orderby i.NurseryTlAr
                           
                           select new
                           {
                               Value = i.NurseryMemberId,
                               Text = i.NurseryTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));

        }
        [HttpGet]
        public async Task<IActionResult> ActivePlanLookup(DataSourceLoadOptions loadOptions,int?CountryId)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (CountryId != null)
            {
               

                var lookupEn = from i in _context.Plan
                               where i.CountryId == CountryId && i.IsActive != false && i.IsActive != null
                               orderby i.PlanTlEn
                               select new
                               {
                                   Value = i.PlanId,
                                   Text = i.PlanTlEn
                               };



                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            else
            {
                


                var lookupEn = from i in _context.Plan
                               where  i.IsActive != false && i.IsActive != null
                               orderby i.PlanTlEn
                               select new
                               {
                                   Value = i.PlanId,
                                   Text = i.PlanTlEn
                               };



                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
           

        }
        [HttpGet]
        public async Task<IActionResult> PlanLookup(DataSourceLoadOptions loadOptions) {
            
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.Plan
                           
                               orderby i.PlanTlEn
                               select new
                               {
                                   Value = i.PlanId,
                                   Text = i.PlanTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.Plan
                         
                           orderby i.PlanTlAr
                           select new
                           {
                               Value = i.PlanId,
                               Text = i.PlanTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));

        }

        private void PopulateModel(NurserySubscription model, IDictionary values) {
            string NURSERY_SUBSCRIPTION_ID = nameof(NurserySubscription.NurserySubscriptionId);
            string NURSERY_ID = nameof(NurserySubscription.NurseryId);
            string PLAN_ID = nameof(NurserySubscription.PlanId);
            string START_DATE = nameof(NurserySubscription.StartDate);
            string END_DATE = nameof(NurserySubscription.EndDate);
            string PRICE = nameof(NurserySubscription.Price);
            string REMARKS = nameof(NurserySubscription.Remarks);

            if(values.Contains(NURSERY_SUBSCRIPTION_ID)) {
                model.NurserySubscriptionId = Convert.ToInt32(values[NURSERY_SUBSCRIPTION_ID]);
            }

            if(values.Contains(NURSERY_ID)) {
                model.NurseryId = Convert.ToInt32(values[NURSERY_ID]);
            }

            if(values.Contains(PLAN_ID)) {
                model.PlanId = Convert.ToInt32(values[PLAN_ID]);
            }

            if(values.Contains(START_DATE)) {
                model.StartDate = values[START_DATE] != null ? Convert.ToDateTime(values[START_DATE]) : (DateTime?)null;
            }

            if(values.Contains(END_DATE)) {
                model.EndDate = values[END_DATE] != null ? Convert.ToDateTime(values[END_DATE]) : (DateTime?)null;
            }

            if(values.Contains(PRICE)) {
                model.Price = values[PRICE] != null ? Convert.ToDouble(values[PRICE], CultureInfo.InvariantCulture) : (double?)null;
            }

            if(values.Contains(REMARKS)) {
                model.Remarks = Convert.ToString(values[REMARKS]);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState) {
            var messages = new List<string>();

            foreach(var entry in modelState) {
                foreach(var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }
    }
}