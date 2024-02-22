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
    public class CitiesController : Controller
    {
        private NurseryContext _context;

        public CitiesController(NurseryContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var city = _context.City.Select(i => new {
                i.CityId,
                i.CityTlAr,
                i.CityTlEn,
                i.CityIsActive,
                i.CityOrderIndex,
                i.CountryId
            });

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "CityId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(city, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new City();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.City.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.CityId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.City.FirstOrDefaultAsync(item => item.CityId == key);
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
            var model = await _context.City.FirstOrDefaultAsync(item => item.CityId == key);

            _context.City.Remove(model);
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
        public async Task<IActionResult> PhaseLookup(DataSourceLoadOptions loadOptions)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.Phases
                               orderby i.TitleEN

                               select new
                               {
                                   Value = i.PhaseId,
                                   Text = i.TitleEN
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }

            var lookupAr = from i in _context.Phases
                           orderby i.TitleAR

                           select new
                           {
                               Value = i.PhaseId,
                               Text = i.TitleAR
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));

        }
        [HttpGet]
        public async Task<IActionResult> ActiveCountryLookup(DataSourceLoadOptions loadOptions)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.Country
                               where i.CountryIsActive == true
                               orderby i.CountryTlEn 
                               select new
                               {
                                   Value = i.CountryId,
                                   Text = i.CountryTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }

            var lookupAr = from i in _context.Country
                           where i.CountryIsActive == true
                           orderby i.CountryTlAr
                           select new
                           {
                               Value = i.CountryId,
                               Text = i.CountryTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));

        }
        [HttpGet]
        public async Task<IActionResult> PaymentMethodsLookup(DataSourceLoadOptions loadOptions)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.PaymentMethod.Where(e=>e.IsActive)
                               
                               select new
                               {
                                   Value = i.PaymentMethodId,
                                   Text = i.PaymentMethodTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }

            var lookupAr = from i in _context.PaymentMethod.Where(e => e.IsActive)

                           select new
                           {
                               Value = i.PaymentMethodId,
                               Text = i.PaymentMethodTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));

        }

        [HttpGet]
        public async Task<IActionResult> LanguagesLookup(DataSourceLoadOptions loadOptions)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.Languages

                               select new
                               {
                                   Value = i.LanguageId,
                                   Text = i.Title
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }

            var lookupAr = from i in _context.Languages

                           select new
                           {
                               Value = i.LanguageId,
                               Text = i.Title
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));

        }
        [HttpGet]
        public async Task<IActionResult> AreasLookup(DataSourceLoadOptions loadOptions,int? CityId)
        {
           
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                if (CityId==0)
                {
                    var lookupEn = from i in _context.Area
                                   where i.CityId == CityId&&i.AreaIsActive==true && i.City.CityIsActive == true&&i.City.Country.CountryIsActive==true
                                   select new
                                   {
                                       Value = i.AreaId,
                                       Text = i.AreaTlEn
                                   };
                    return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
                }
                else
                {
                    var lookupEn = from i in _context.Area
                                   where  i.AreaIsActive == true && i.City.CityIsActive == true && i.City.Country.CountryIsActive == true
                                   select new
                                   {
                                       Value = i.AreaId,
                                       Text = i.AreaTlEn
                                   };
                    return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
                }
                
            }
            if (CityId == 0)
            {
                var lookupAr = from i in _context.Area

                               where i.CityId == CityId&& i.AreaIsActive == true && i.City.CityIsActive == true && i.City.Country.CountryIsActive == true
                               select new
                               {

                                   Value = i.AreaId,
                                   Text = i.AreaTlAr
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));
            }
            else
            {
                var lookupAr = from i in _context.Area
                               where i.AreaIsActive == true && i.City.CityIsActive == true && i.City.Country.CountryIsActive == true

                               select new
                               {

                                   Value = i.AreaId,
                                   Text = i.AreaTlAr
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));
            }
            

        }


        private void PopulateModel(City model, IDictionary values) {
            string CITY_ID = nameof(City.CityId);
            string CITY_TL_AR = nameof(City.CityTlAr);
            string CITY_TL_EN = nameof(City.CityTlEn);
            string CITY_IS_ACTIVE = nameof(City.CityIsActive);
            string CITY_ORDER_INDEX = nameof(City.CityOrderIndex);
            string COUNTRY_ID = nameof(City.CountryId);

            if(values.Contains(CITY_ID)) {
                model.CityId = Convert.ToInt32(values[CITY_ID]);
            }

            if(values.Contains(CITY_TL_AR)) {
                model.CityTlAr = Convert.ToString(values[CITY_TL_AR]);
            }

            if(values.Contains(CITY_TL_EN)) {
                model.CityTlEn = Convert.ToString(values[CITY_TL_EN]);
            }

            if(values.Contains(CITY_IS_ACTIVE)) {
                model.CityIsActive = values[CITY_IS_ACTIVE] != null ? Convert.ToBoolean(values[CITY_IS_ACTIVE]) : (bool?)null;
            }

            if(values.Contains(CITY_ORDER_INDEX)) {
                model.CityOrderIndex = values[CITY_ORDER_INDEX] != null ? Convert.ToInt32(values[CITY_ORDER_INDEX]) : (int?)null;
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