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

namespace Nursery.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AreasController : Controller
    {
        private NurseryContext _context;

        public AreasController(NurseryContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var area = _context.Area.Select(i => new {
                i.AreaId,
                i.CityId,
                i.AreaTlAr,
                i.AreaTlEn,
                i.AreaIsActive,
                i.AreaOrderIndex,
                i.City
            });

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "AreaId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(area, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Area();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Area.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.AreaId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Area.FirstOrDefaultAsync(item => item.AreaId == key);
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
            var model = await _context.Area.FirstOrDefaultAsync(item => item.AreaId == key);

            _context.Area.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> CityLookup(DataSourceLoadOptions loadOptions,int? CountryId) {
            if (CountryId!=null)
            {
                var lookup = from i in _context.City
                           where i.CountryId== CountryId&& i.CityIsActive == true&&i.Country.CountryIsActive==true
                             orderby i.CityId

                             select new
                             {
                                 Value = i.CityId,
                                 Text = i.CityTlEn
                             };
                return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
            }
            else
            {
                var lookup = from i in _context.City
                    
                             orderby i.CityId
                             where i.CityIsActive == true && i.Country.CountryIsActive == true
                             select new
                             {
                                 Value = i.CityId,
                                 Text = i.CityTlEn
                             };
                return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
            }
            
        }
        [HttpGet]
        public async Task<IActionResult> CountryLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.Country
                         orderby i.CountryTlAr
                         where i.CountryIsActive==true
                         select new
                         {
                             Value = i.CountryId,
                             Text = i.CountryTlAr
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(Area model, IDictionary values) {
            string AREA_ID = nameof(Area.AreaId);
            string CITY_ID = nameof(Area.CityId);
            string AREA_TL_AR = nameof(Area.AreaTlAr);
            string AREA_TL_EN = nameof(Area.AreaTlEn);
            string AREA_IS_ACTIVE = nameof(Area.AreaIsActive);
            string AREA_ORDER_INDEX = nameof(Area.AreaOrderIndex);

            if(values.Contains(AREA_ID)) {
                model.AreaId = Convert.ToInt32(values[AREA_ID]);
            }

            if(values.Contains(CITY_ID)) {
                model.CityId = Convert.ToInt32(values[CITY_ID]);
            }

            if(values.Contains(AREA_TL_AR)) {
                model.AreaTlAr = Convert.ToString(values[AREA_TL_AR]);
            }

            if(values.Contains(AREA_TL_EN)) {
                model.AreaTlEn = Convert.ToString(values[AREA_TL_EN]);
            }

            if(values.Contains(AREA_IS_ACTIVE)) {
                model.AreaIsActive = values[AREA_IS_ACTIVE] != null ? Convert.ToBoolean(values[AREA_IS_ACTIVE]) : (bool?)null;
            }

            if(values.Contains(AREA_ORDER_INDEX)) {
                model.AreaOrderIndex = values[AREA_ORDER_INDEX] != null ? Convert.ToInt32(values[AREA_ORDER_INDEX]) : (int?)null;
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