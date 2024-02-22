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
    public class PlansController : Controller
    {
        private NurseryContext _context;

        public PlansController(NurseryContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var plan = _context.Plan.Select(i => new {
                i.PlanId,
                i.PlanTlAr,
                i.PlanTlEn,
                i.IsActive,
                i.Price,
                i.DurationInMonth,
                i.CountryId
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "PlanId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(plan, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Plan();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Plan.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.PlanId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Plan.FirstOrDefaultAsync(item => item.PlanId == key);
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
            var model = await _context.Plan.FirstOrDefaultAsync(item => item.PlanId == key);

            _context.Plan.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> CountryLookup(DataSourceLoadOptions loadOptions)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.Country
                               orderby i.CountryTlEn
                               select new
                               {
                                   Value = i.CountryId,
                                   Text = i.CountryTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.Country
                           orderby i.CountryTlAr
                           select new
                           {
                               Value = i.CountryId,
                               Text = i.CountryTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));

        }

        [HttpGet]
        public async Task<IActionResult> PlanTypeLookup(DataSourceLoadOptions loadOptions)
        {
            
                var lookup = from i in _context.PlanTypes
                              
                               select new
                               {
                                   Value = i.PlanTypeId,
                                   Text = i.Title
                               };
                return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
            
        }


        private void PopulateModel(Plan model, IDictionary values) {
            string PLAN_ID = nameof(Plan.PlanId);
            string PLAN_TL_AR = nameof(Plan.PlanTlAr);
            string PLAN_TL_EN = nameof(Plan.PlanTlEn);
            string IS_ACTIVE = nameof(Plan.IsActive);
            string PRICE = nameof(Plan.Price);
            string DURATION_IN_MONTH = nameof(Plan.DurationInMonth);
            string COUNTRY_ID = nameof(Plan.CountryId);

            if(values.Contains(PLAN_ID)) {
                model.PlanId = Convert.ToInt32(values[PLAN_ID]);
            }

            if(values.Contains(PLAN_TL_AR)) {
                model.PlanTlAr = Convert.ToString(values[PLAN_TL_AR]);
            }

            if(values.Contains(PLAN_TL_EN)) {
                model.PlanTlEn = Convert.ToString(values[PLAN_TL_EN]);
            }

            if(values.Contains(IS_ACTIVE)) {
                model.IsActive = values[IS_ACTIVE] != null ? Convert.ToBoolean(values[IS_ACTIVE]) : (bool?)null;
            }

            if(values.Contains(PRICE)) {
                model.Price = values[PRICE] != null ? Convert.ToDouble(values[PRICE], CultureInfo.InvariantCulture) : (double?)null;
            }

            if(values.Contains(DURATION_IN_MONTH)) {
                model.DurationInMonth = values[DURATION_IN_MONTH] != null ? Convert.ToInt32(values[DURATION_IN_MONTH]) : (int?)null;
            }

            if(values.Contains(COUNTRY_ID)) {
                model.CountryId = values[COUNTRY_ID] != null ? Convert.ToInt32(values[COUNTRY_ID]) : (int?)null;
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