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
    public class ChildrenController : Controller
    {
        private NurseryContext _context;

        public ChildrenController(NurseryContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int parentId,DataSourceLoadOptions loadOptions) {
            var child = _context.Child.Where(c=>c.ParentId==parentId).Include(c=>c.NurseryMember).Select(i => new {
                i.ChildId,
                i.ChildName,
                i.ChildPhone,
                i.ChildEmail,
                i.ChildImage,
                i.ParentId,
                i.AgeCategory,
                i.AgeCategoryId,
                i.NurseryMemberId,
                i.Parent,
                i.NurseryMember

            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ChildId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(child, loadOptions));
        }


        [HttpGet]
        public async Task<IActionResult> GetBynurseryId(int nurseryId, DataSourceLoadOptions loadOptions)
        {
            var child = _context.Child.Where(c => c.NurseryMemberId == nurseryId).Select(i => new {
                i.ChildId,
                i.ChildName,
                i.ChildPhone,
                i.ChildEmail,
                i.ChildImage,
                i.ParentId,
                i.AgeCategory,
                i.AgeCategoryId,
                i.NurseryMemberId,
                i.Parent,
                i.NurseryMember

            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ChildId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(child, loadOptions));
        }


        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Child();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Child.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.ChildId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Child.FirstOrDefaultAsync(item => item.ChildId == key);
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
            var model = await _context.Child.FirstOrDefaultAsync(item => item.ChildId == key);

            _context.Child.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> ParentLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Parent
                         orderby i.ParentName
                         select new {
                             Value = i.ParentId,
                             Text = i.ParentName
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> NurseryMemberLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.NurseryMember
                         orderby i.NurseryTlAr
                         where i.NurserySubscription.OrderByDescending(e => e.NurserySubscriptionId).FirstOrDefault().IsActive
                             && i.Area.AreaIsActive == true
                             && i.Area.City.CityIsActive == true
                             && i.Area.City.Country.CountryIsActive == true
                         select new {
                             Value = i.NurseryMemberId,
                             Text = i.NurseryTlAr
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> AgeCategoryLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.AgeCategory
                         orderby i.AgeCategoryTlAr
                         select new
                         {
                             Value = i.AgeCategoryId,
                             Text = i.AgeCategoryTlAr
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]

        private void PopulateModel(Child model, IDictionary values) {
            string CHILD_ID = nameof(Child.ChildId);
            string CHILD_NAME = nameof(Child.ChildName);
            string CHILD_PHONE = nameof(Child.ChildPhone);
            string CHILD_EMAIL = nameof(Child.ChildEmail);
            string CHILD_IMAGE = nameof(Child.ChildImage);
            string PARENT_ID = nameof(Child.ParentId);
            string AGE_CATEGORY_ID = nameof(Child.AgeCategoryId);
            string NURSERY_MEMBER_ID = nameof(Child.NurseryMemberId);

            if(values.Contains(CHILD_ID)) {
                model.ChildId = Convert.ToInt32(values[CHILD_ID]);
            }

            if(values.Contains(CHILD_NAME)) {
                model.ChildName = Convert.ToString(values[CHILD_NAME]);
            }

            if(values.Contains(CHILD_PHONE)) {
                model.ChildPhone = Convert.ToString(values[CHILD_PHONE]);
            }

            if(values.Contains(CHILD_EMAIL)) {
                model.ChildEmail = Convert.ToString(values[CHILD_EMAIL]);
            }

            if(values.Contains(CHILD_IMAGE)) {
                model.ChildImage = Convert.ToString(values[CHILD_IMAGE]);
            }

            if(values.Contains(PARENT_ID)) {
                model.ParentId = Convert.ToInt32(values[PARENT_ID]);
            }

            if (values.Contains(AGE_CATEGORY_ID))
            {
                model.AgeCategoryId = Convert.ToInt32(values[AGE_CATEGORY_ID]);
            }

            if (values.Contains(NURSERY_MEMBER_ID)) {
                model.NurseryMemberId = values[NURSERY_MEMBER_ID] != null ? Convert.ToInt32(values[NURSERY_MEMBER_ID]) : (int?)null;
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