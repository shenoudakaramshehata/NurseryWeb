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
    public class PublicNotificationsController : Controller
    {
        private NurseryContext _context;

        public PublicNotificationsController(NurseryContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var publicnotification = _context.PublicNotification.Select(i => new {
                i.PublicNotificationId,
                i.Title,
                i.Body,
                i.Date,
                i.EntityTypeNotifyId,
                i.EntityId,
                i.CountryId
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "PublicNotificationId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(publicnotification, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new PublicNotification();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.PublicNotification.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.PublicNotificationId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.PublicNotification.FirstOrDefaultAsync(item => item.PublicNotificationId == key);
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
            var model = await _context.PublicNotification.FirstOrDefaultAsync(item => item.PublicNotificationId == key);

            _context.PublicNotification.Remove(model);
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
                               where  i.CountryIsActive==true
                               select new
                               {
                                   Value = i.CountryId,
                                   Text = i.CountryTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.Country
                           orderby i.CountryTlAr
                           where i.CountryIsActive == true
                           select new
                           {
                               Value = i.CountryId,
                               Text = i.CountryTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));

        }
       
       [HttpGet]
        public async Task<IActionResult> NurseryLookup(DataSourceLoadOptions loadOptions,int? CountryId)
        {

            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                if (CountryId == 0)
                {
                    var lookupEn = from i in _context.NurseryMember
                                   orderby i.NurseryTlEn
                                   where i.CountryId==CountryId&& i.NurserySubscription.OrderByDescending(e => e.NurserySubscriptionId).FirstOrDefault().IsActive
                                   select new
                                   {
                                       Value = i.NurseryMemberId,
                                       Text = i.NurseryTlEn
                                   };
                    return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
                }
                else
                {
                    var lookupEn = from i in _context.NurseryMember
                                   orderby i.NurseryTlEn
                                   where i.NurserySubscription.OrderByDescending(e => e.NurserySubscriptionId).FirstOrDefault().IsActive
                                   select new
                                   {
                                       Value = i.NurseryMemberId,
                                       Text = i.NurseryTlEn
                                   };
                    return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
                }
             
            }
            if (CountryId == 0)
            {
                var lookupAr = from i in _context.NurseryMember
                               orderby i.NurseryTlAr
                               where i.CountryId == CountryId && i.NurserySubscription.OrderByDescending(e => e.NurserySubscriptionId).FirstOrDefault().IsActive
                               && i.Area.AreaIsActive==true
                               &&i.Area.City.CityIsActive==true
                               &&i.Area.City.Country.CountryIsActive==true
                               select new
                               {
                                   Value = i.NurseryMemberId,
                                   Text = i.NurseryTlAr
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));
            }
            else
            {
                var lookupAr = from i in _context.NurseryMember
                               orderby i.NurseryTlAr
                               where i.NurserySubscription.OrderByDescending(e=>e.NurserySubscriptionId).FirstOrDefault().IsActive
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
         

        }

        [HttpGet]
        public async Task<IActionResult> EntityTypeLookup(DataSourceLoadOptions loadOptions)
        {
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
        public async Task<IActionResult> SliderTypeLookup(DataSourceLoadOptions loadOptions)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.EntityTypeNotifies
                               orderby i.EntityTypeNotifyId

                               select new
                               {
                                   Value = i.EntityTypeNotifyId
                                   ,
                                   Text = i.TitleEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.EntityTypeNotifies
                           orderby i.EntityTypeNotifyId

                           select new
                           {
                               Value = i.EntityTypeNotifyId
                               ,
                               Text = i.TitleAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));

        }
        private void PopulateModel(PublicNotification model, IDictionary values) {
            string PUBLIC_NOTIFICATION_ID = nameof(PublicNotification.PublicNotificationId);
            string TITLE = nameof(PublicNotification.Title);
            string BODY = nameof(PublicNotification.Body);
            string DATE = nameof(PublicNotification.Date);
            //string ENTITY_TYPE_ID = nameof(PublicNotification.EntityTypeId);
            string ENTITY_TYPE_NOTIFY_ID = nameof(PublicNotification.EntityTypeNotifyId);
            string ENTITY_ID = nameof(PublicNotification.EntityId);
            string COUNTRY_ID = nameof(PublicNotification.CountryId);

            if(values.Contains(PUBLIC_NOTIFICATION_ID)) {
                model.PublicNotificationId = Convert.ToInt32(values[PUBLIC_NOTIFICATION_ID]);
            }

            if(values.Contains(TITLE)) {
                model.Title = Convert.ToString(values[TITLE]);
            }

            if(values.Contains(BODY)) {
                model.Body = Convert.ToString(values[BODY]);
            }

            if(values.Contains(DATE)) {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            //if(values.Contains(ENTITY_TYPE_ID)) {
            //    model.EntityTypeId = values[ENTITY_TYPE_ID] != null ? Convert.ToInt32(values[ENTITY_TYPE_ID]) : (int?)null;
            //}
            if (values.Contains(ENTITY_TYPE_NOTIFY_ID))
            {
                model.EntityTypeNotifyId = values[ENTITY_TYPE_NOTIFY_ID] != null ? Convert.ToInt32(values[ENTITY_TYPE_NOTIFY_ID]) : (int?)null;
            }

            if (values.Contains(ENTITY_ID)) {
                model.EntityId = values[ENTITY_ID] != null ? Convert.ToInt32(values[ENTITY_ID]) : (int?)null;
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