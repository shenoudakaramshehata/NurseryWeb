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
    public class AdzsController : Controller
    {
        private NurseryContext _context;

        public AdzsController(NurseryContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var adz = _context.Adz.Select(i => new {
                i.AdzId,
                i.AdzPic,
                i.EntityTypeId,
                i.EntityId,
                i.AdzIsActive,
                i.AdzOrderIndex,
                i.CountryId
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "AdzId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(adz, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Adz();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Adz.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.AdzId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Adz.FirstOrDefaultAsync(item => item.AdzId == key);
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
            var model = await _context.Adz.FirstOrDefaultAsync(item => item.AdzId == key);
            _context.Adz.Remove(model);
            await _context.SaveChangesAsync();
        }
        [HttpGet]
        public async Task<IActionResult> CountryLookup(DataSourceLoadOptions loadOptions) {
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
        public async Task<IActionResult> EntityTypeLookup(DataSourceLoadOptions loadOptions) {


            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.EntityType
                               orderby i.EntityTypeTlen
                               select new
                               {
                                   Value = i.EntityTypeId,
                                   Text = i.EntityTypeTlen
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.EntityType
                           orderby i.EntityTypeTlar
                           select new
                           {
                               Value = i.EntityTypeId,
                               Text = i.EntityTypeTlar
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));


        }
        [HttpGet]
        public async Task<IActionResult> NurseryLookup(DataSourceLoadOptions loadOptions)
        {

            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.NurseryMember
                               orderby i.NurseryTlEn
                               where  i.NurserySubscription.OrderByDescending(e=>e.NurserySubscriptionId).FirstOrDefault().IsActive
                              && i.Area.AreaIsActive == true
                              && i.Area.City.CityIsActive == true
                              && i.Area.City.Country.CountryIsActive == true
                               select new
                               {
                                   Value = i.NurseryMemberId,
                                   Text = i.NurseryTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.NurseryMember
                           orderby i.NurseryTlAr
                           where i.NurserySubscription.OrderByDescending(e => e.NurserySubscriptionId).FirstOrDefault().IsActive
                             && i.Area.AreaIsActive == true
                             && i.Area.City.CityIsActive == true
                             && i.Area.City.Country.CountryIsActive == true
                           select new
                           {
                               Value = i.NurseryMemberId,
                               Text = i.NurseryTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));

        }

        private void PopulateModel(Adz model, IDictionary values) {
            string ADZ_ID = nameof(Adz.AdzId);
            string ADZ_PIC = nameof(Adz.AdzPic);
            string ENTITY_TYPE_ID = nameof(Adz.EntityTypeId);
            string ENTITY_ID = nameof(Adz.EntityId);
            string ADZ_IS_ACTIVE = nameof(Adz.AdzIsActive);
            string ADZ_ORDER_INDEX = nameof(Adz.AdzOrderIndex);
            string COUNTRY_ID = nameof(Adz.CountryId);

            if(values.Contains(ADZ_ID)) {
                model.AdzId = Convert.ToInt32(values[ADZ_ID]);
            }

            if(values.Contains(ADZ_PIC)) {
                model.AdzPic = Convert.ToString(values[ADZ_PIC]);
            }

            if(values.Contains(ENTITY_TYPE_ID)) {
                model.EntityTypeId = values[ENTITY_TYPE_ID] != null ? Convert.ToInt32(values[ENTITY_TYPE_ID]) : (int?)null;
            }

            if(values.Contains(ENTITY_ID)) {
                model.EntityId = Convert.ToString(values[ENTITY_ID]);
            }

            if(values.Contains(ADZ_IS_ACTIVE)) {
                model.AdzIsActive = values[ADZ_IS_ACTIVE] != null ? Convert.ToBoolean(values[ADZ_IS_ACTIVE]) : (bool?)null;
            }

            if(values.Contains(ADZ_ORDER_INDEX)) {
                model.AdzOrderIndex = values[ADZ_ORDER_INDEX] != null ? Convert.ToInt32(values[ADZ_ORDER_INDEX]) : (int?)null;
            }

            if(values.Contains(COUNTRY_ID)) {
                model.CountryId = Convert.ToInt32(values[COUNTRY_ID]);
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