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
    public class AgeCategoriesController : Controller
    {
        private NurseryContext _context;

        public AgeCategoriesController(NurseryContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var agecategory = _context.AgeCategory.Select(i => new {
                i.AgeCategoryId,
                i.AgeCategoryTlAr,
                i.AgeCategoryTlEn,
                i.from,
                i.to,
                i.IsActive

            });

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "AgeCategoryId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(agecategory, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new AgeCategory();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.AgeCategory.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.AgeCategoryId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.AgeCategory.FirstOrDefaultAsync(item => item.AgeCategoryId == key);
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
        public async Task<IActionResult> Delete(int key) {
            if (_context.NurseryMember.Any(c => c.AgeCategoryId == key))
            {
                var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
                var BrowserCulture = locale.RequestCulture.UICulture.ToString();
                if (BrowserCulture == "en-US")
                    return StatusCode(409, "You cannot delete this Age Category");
                else
                    return StatusCode(409, "لا يمكنك مسح هذةالفئة العمرية");

            }
            var model = await _context.AgeCategory.FirstOrDefaultAsync(item => item.AgeCategoryId == key);

            _context.AgeCategory.Remove(model);
            await _context.SaveChangesAsync();
            return Ok();

        }


        private void PopulateModel(AgeCategory model, IDictionary values) {
            string AGE_CATEGORY_ID = nameof(AgeCategory.AgeCategoryId);
            string AGE_CATEGORY_TL_AR = nameof(AgeCategory.AgeCategoryTlAr);
            string AGE_CATEGORY_TL_EN = nameof(AgeCategory.AgeCategoryTlEn);
            string AGE_CATEGORY_FROM = nameof(AgeCategory.from);
            string AGE_CATEGORY_TO = nameof(AgeCategory.to);
            string IS_ACTIVE = nameof(AgeCategory.IsActive);
            if (values.Contains(AGE_CATEGORY_ID)) {
                model.AgeCategoryId = Convert.ToInt32(values[AGE_CATEGORY_ID]);
            }
            if (values.Contains(AGE_CATEGORY_FROM))
            {
                model.from = Convert.ToInt32(values[AGE_CATEGORY_FROM]);
            }
            if (values.Contains(AGE_CATEGORY_TO))
            {
                model.to = Convert.ToInt32(values[AGE_CATEGORY_TO]);
            }
            if (values.Contains(AGE_CATEGORY_TL_AR)) {
                model.AgeCategoryTlAr = Convert.ToString(values[AGE_CATEGORY_TL_AR]);
            }

            if(values.Contains(AGE_CATEGORY_TL_EN)) {
                model.AgeCategoryTlEn = Convert.ToString(values[AGE_CATEGORY_TL_EN]);
            }
            if (values.Contains(IS_ACTIVE))
            {
                model.IsActive = Convert.ToBoolean(values[IS_ACTIVE]);

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