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
    public class CountriesController : Controller
    {
        private NurseryContext _context;

        public CountriesController(NurseryContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var country = _context.Country.Select(i => new {
                i.CountryId,
                i.CountryTlAr,
                i.CountryTlEn,
                i.CountryPic,
                i.CountryIsActive,
                i.CountryOrderIndex
            });

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "CountryId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(country, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Country();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Country.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.CountryId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Country.FirstOrDefaultAsync(item => item.CountryId == key);
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
            var model = await _context.Country.FirstOrDefaultAsync(item => item.CountryId == key);

            _context.Country.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(Country model, IDictionary values) {
            string COUNTRY_ID = nameof(Country.CountryId);
            string COUNTRY_TL_AR = nameof(Country.CountryTlAr);
            string COUNTRY_TL_EN = nameof(Country.CountryTlEn);
            string COUNTRY_PIC = nameof(Country.CountryPic);
            string COUNTRY_IS_ACTIVE = nameof(Country.CountryIsActive);
            string COUNTRY_ORDER_INDEX = nameof(Country.CountryOrderIndex);

            if(values.Contains(COUNTRY_ID)) {
                model.CountryId = Convert.ToInt32(values[COUNTRY_ID]);
            }

            if(values.Contains(COUNTRY_TL_AR)) {
                model.CountryTlAr = Convert.ToString(values[COUNTRY_TL_AR]);
            }

            if(values.Contains(COUNTRY_TL_EN)) {
                model.CountryTlEn = Convert.ToString(values[COUNTRY_TL_EN]);
            }

            if(values.Contains(COUNTRY_PIC)) {
                model.CountryPic = Convert.ToString(values[COUNTRY_PIC]);
            }

            if(values.Contains(COUNTRY_IS_ACTIVE)) {
                model.CountryIsActive = values[COUNTRY_IS_ACTIVE] != null ? Convert.ToBoolean(values[COUNTRY_IS_ACTIVE]) : (bool?)null;
            }

            if(values.Contains(COUNTRY_ORDER_INDEX)) {
                model.CountryOrderIndex = values[COUNTRY_ORDER_INDEX] != null ? Convert.ToInt32(values[COUNTRY_ORDER_INDEX]) : (int?)null;
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