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
    public class NurseryMembersController : Controller
    {
        private NurseryContext _context;

        public NurseryMembersController(NurseryContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var nurserymember = _context.NurseryMember.Select(i => new {
                i.NurseryMemberId,
                i.NurseryTlAr,
                i.NurseryTlEn,
                i.NurseryDescAr,
                i.NurseryDescEn,
                i.Email,
                i.Phone1,
                i.Phone2,
                i.Fax,
                i.Mobile,
                i.Facebook,
                i.Twitter,
                i.Instagram,
                i.WorkTimeFrom,
                i.WorkTimeTo,
                i.AgeCategoryId,
                i.TransportationService,
                i.SpecialNeeds,
                i.Language,
                i.CountryId,
                i.CityId,
                i.AreaId,
                i.Address,
                i.Lat,
                i.Lng,
                i.Logo,
                i.Banner,
                i.Area.City.Country,
                i.IsSlider
            });

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "NurseryMemberId" };
            // loadOptions.PaginateViaPrimaryKey = true;


            return Json(await DataSourceLoader.LoadAsync(nurserymember, loadOptions));
        }
        [HttpGet]
        public async Task<object> GetImagesForNursery([FromQuery] int id)
        {
            var Nurseryimages = _context.NurseryImage.Where(p => p.NurseryId == id).Select(i => new {
                i.NurseryImageId,
                i.Pic,
                i.NurseryId
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ImageId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Nurseryimages;
        }
        [HttpPost]
        public async Task<int> RemoveImageById([FromQuery] int id)
        {
            var nurseryPic = _context.NurseryImage.FirstOrDefault(p => p.NurseryImageId == id);
            _context.NurseryImage.Remove(nurseryPic);
            _context.SaveChanges();

            return id;
        }
        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new NurseryMember();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.NurseryMember.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.NurseryMemberId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.NurseryMember.FirstOrDefaultAsync(item => item.NurseryMemberId == key);
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
            var model = await _context.NurseryMember.FirstOrDefaultAsync(item => item.NurseryMemberId == key);

            _context.NurseryMember.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> AgeCategoryLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.AgeCategory where i.IsActive==true
                         orderby i.AgeCategoryTlAr
                         select new {
                             Value = i.AgeCategoryId,
                             Text = i.AgeCategoryTlAr
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> AreaLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Area
                         orderby i.AreaTlAr
                        where i.AreaIsActive==true
                              && i.City.CityIsActive == true
                              && i.City.Country.CountryIsActive == true
                         select new {
                             Value = i.AreaId,
                             Text = i.AreaTlAr
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
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

        private void PopulateModel(NurseryMember model, IDictionary values) {
            string NURSERY_MEMBER_ID = nameof(NurseryMember.NurseryMemberId);
            string NURSERY_TL_AR = nameof(NurseryMember.NurseryTlAr);
            string NURSERY_TL_EN = nameof(NurseryMember.NurseryTlEn);
            string NURSERY_DESC_AR = nameof(NurseryMember.NurseryDescAr);
            string NURSERY_DESC_EN = nameof(NurseryMember.NurseryDescEn);
            string EMAIL = nameof(NurseryMember.Email);
            string PHONE1 = nameof(NurseryMember.Phone1);
            string PHONE2 = nameof(NurseryMember.Phone2);
            string FAX = nameof(NurseryMember.Fax);
            string MOBILE = nameof(NurseryMember.Mobile);
            string FACEBOOK = nameof(NurseryMember.Facebook);
            string TWITTER = nameof(NurseryMember.Twitter);
            string INSTAGRAM = nameof(NurseryMember.Instagram);
            string WORK_TIME_FROM = nameof(NurseryMember.WorkTimeFrom);
            string WORK_TIME_TO = nameof(NurseryMember.WorkTimeTo);
            string AGE_CATEGORY_ID = nameof(NurseryMember.AgeCategoryId);
            string TRANSPORTATION_SERVICE = nameof(NurseryMember.TransportationService);
            string SPECIAL_NEEDS = nameof(NurseryMember.SpecialNeeds);
            string LANGUAGE = nameof(NurseryMember.Language);
            string COUNTRY_ID = nameof(NurseryMember.CountryId);
            string CITY_ID = nameof(NurseryMember.CityId);
            string AREA_ID = nameof(NurseryMember.AreaId);
            string ADDRESS = nameof(NurseryMember.Address);
            string LAT = nameof(NurseryMember.Lat);
            string LNG = nameof(NurseryMember.Lng);
            string LOGO = nameof(NurseryMember.Logo);
            string BANNER = nameof(NurseryMember.Banner);
            string IS_SLIDER = nameof(NurseryMember.IsSlider);

            if(values.Contains(NURSERY_MEMBER_ID)) {
                model.NurseryMemberId = Convert.ToInt32(values[NURSERY_MEMBER_ID]);
            }

            if(values.Contains(NURSERY_TL_AR)) {
                model.NurseryTlAr = Convert.ToString(values[NURSERY_TL_AR]);
            }

            if(values.Contains(NURSERY_TL_EN)) {
                model.NurseryTlEn = Convert.ToString(values[NURSERY_TL_EN]);
            }

            if(values.Contains(NURSERY_DESC_AR)) {
                model.NurseryDescAr = Convert.ToString(values[NURSERY_DESC_AR]);
            }

            if(values.Contains(NURSERY_DESC_EN)) {
                model.NurseryDescEn = Convert.ToString(values[NURSERY_DESC_EN]);
            }

            if(values.Contains(EMAIL)) {
                model.Email = Convert.ToString(values[EMAIL]);
            }

            if(values.Contains(PHONE1)) {
                model.Phone1 = Convert.ToString(values[PHONE1]);
            }

            if(values.Contains(PHONE2)) {
                model.Phone2 = Convert.ToString(values[PHONE2]);
            }

            if(values.Contains(FAX)) {
                model.Fax = Convert.ToString(values[FAX]);
            }

            if(values.Contains(MOBILE)) {
                model.Mobile = Convert.ToString(values[MOBILE]);
            }

            if(values.Contains(FACEBOOK)) {
                model.Facebook = Convert.ToString(values[FACEBOOK]);
            }

            if(values.Contains(TWITTER)) {
                model.Twitter = Convert.ToString(values[TWITTER]);
            }

            if(values.Contains(INSTAGRAM)) {
                model.Instagram = Convert.ToString(values[INSTAGRAM]);
            }

            if(values.Contains(WORK_TIME_FROM)) {
                model.WorkTimeFrom = Convert.ToString(values[WORK_TIME_FROM]);
            }

            if(values.Contains(WORK_TIME_TO)) {
                model.WorkTimeTo = Convert.ToString(values[WORK_TIME_TO]);
            }

            if(values.Contains(AGE_CATEGORY_ID)) {
                model.AgeCategoryId = values[AGE_CATEGORY_ID] != null ? Convert.ToInt32(values[AGE_CATEGORY_ID]) : (int?)null;
            }

            if(values.Contains(TRANSPORTATION_SERVICE)) {
                model.TransportationService = values[TRANSPORTATION_SERVICE] != null ? Convert.ToBoolean(values[TRANSPORTATION_SERVICE]) : (bool?)null;
            }

            if(values.Contains(SPECIAL_NEEDS)) {
                model.SpecialNeeds = values[SPECIAL_NEEDS] != null ? Convert.ToBoolean(values[SPECIAL_NEEDS]) : (bool?)null;
            }

            if(values.Contains(LANGUAGE)) {
                model.Language = values[LANGUAGE] != null ? Convert.ToInt32(values[LANGUAGE]) : (int?)null;
            }

            if(values.Contains(COUNTRY_ID)) {
                model.CountryId = values[COUNTRY_ID] != null ? Convert.ToInt32(values[COUNTRY_ID]) : (int?)null;
            }

            if(values.Contains(CITY_ID)) {
                model.CityId = values[CITY_ID] != null ? Convert.ToInt32(values[CITY_ID]) : (int?)null;
            }

            if(values.Contains(AREA_ID)) {
                model.AreaId = values[AREA_ID] != null ? Convert.ToInt32(values[AREA_ID]) : (int?)null;
            }

            if(values.Contains(ADDRESS)) {
                model.Address = Convert.ToString(values[ADDRESS]);
            }

            if(values.Contains(LAT)) {
                model.Lat = Convert.ToString(values[LAT]);
            }

            if(values.Contains(LNG)) {
                model.Lng = Convert.ToString(values[LNG]);
            }

            if(values.Contains(LOGO)) {
                model.Logo = Convert.ToString(values[LOGO]);
            }

            if(values.Contains(BANNER)) {
                model.Banner = Convert.ToString(values[BANNER]);
            }
            if (values.Contains(IS_SLIDER))
            {
                model.IsSlider =  Convert.ToBoolean(values[IS_SLIDER]);

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