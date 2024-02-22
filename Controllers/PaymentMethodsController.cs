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
    public class PaymentMethodsController : Controller
    {
        private NurseryContext _context;

        public PaymentMethodsController(NurseryContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var paymentmethod = _context.PaymentMethod.Select(i => new {
                i.PaymentMethodId,
                i.PaymentMethodTlAr,
                i.PaymentMethodTlEn,
                i.IsActive
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "PaymentMethodId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(paymentmethod, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new PaymentMethod();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.PaymentMethod.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.PaymentMethodId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.PaymentMethod.FirstOrDefaultAsync(item => item.PaymentMethodId == key);
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
            var model = await _context.PaymentMethod.FirstOrDefaultAsync(item => item.PaymentMethodId == key);

            _context.PaymentMethod.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(PaymentMethod model, IDictionary values) {
            string PAYMENT_METHOD_ID = nameof(PaymentMethod.PaymentMethodId);
            string PAYMENT_METHOD_TL_AR = nameof(PaymentMethod.PaymentMethodTlAr);
            string PAYMENT_METHOD_TL_EN = nameof(PaymentMethod.PaymentMethodTlEn);
            string IS_ACTIVE = nameof(PaymentMethod.IsActive);

            if (values.Contains(PAYMENT_METHOD_ID)) {
                model.PaymentMethodId = Convert.ToInt32(values[PAYMENT_METHOD_ID]);
            }

            if(values.Contains(PAYMENT_METHOD_TL_AR)) {
                model.PaymentMethodTlAr = Convert.ToString(values[PAYMENT_METHOD_TL_AR]);
            }

            if(values.Contains(PAYMENT_METHOD_TL_EN)) {
                model.PaymentMethodTlEn = Convert.ToString(values[PAYMENT_METHOD_TL_EN]);
            }
            if (values.Contains(IS_ACTIVE))
            {
                model.IsActive = Convert.ToBoolean(values[IS_ACTIVE]) ;
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