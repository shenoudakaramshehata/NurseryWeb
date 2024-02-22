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
    public class ParentsController : Controller
    {
        private NurseryContext _context;

        public ParentsController(NurseryContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var parent = _context.Parent.Select(i => new {
                i.ParentId,
                i.ParentName,
                i.ParentAddress,
                i.ParentPhone,
                i.ParentEmail
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ParentId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(parent, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Parent();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Parent.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.ParentId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Parent.FirstOrDefaultAsync(item => item.ParentId == key);
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
            var model = await _context.Parent.FirstOrDefaultAsync(item => item.ParentId == key);

            _context.Parent.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(Parent model, IDictionary values) {
            string PARENT_ID = nameof(Parent.ParentId);
            string PARENT_NAME = nameof(Parent.ParentName);
            string PARENT_ADDRESS = nameof(Parent.ParentAddress);
            string PARENT_PHONE = nameof(Parent.ParentPhone);
            string PARENT_EMAIL = nameof(Parent.ParentEmail);

            if(values.Contains(PARENT_ID)) {
                model.ParentId = Convert.ToInt32(values[PARENT_ID]);
            }

            if(values.Contains(PARENT_NAME)) {
                model.ParentName = Convert.ToString(values[PARENT_NAME]);
            }

            if(values.Contains(PARENT_ADDRESS)) {
                model.ParentAddress = Convert.ToString(values[PARENT_ADDRESS]);
            }

            if(values.Contains(PARENT_PHONE)) {
                model.ParentPhone = Convert.ToString(values[PARENT_PHONE]);
            }

            if(values.Contains(PARENT_EMAIL)) {
                model.ParentEmail = Convert.ToString(values[PARENT_EMAIL]);
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