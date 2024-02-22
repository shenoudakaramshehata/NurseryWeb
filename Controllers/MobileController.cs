using Nursery.Data;
using Nursery.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nursery.Entities;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Http.Headers;
using MimeKit;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace Nursery.Controllers
{
    [ApiController]
    [Route("api/[Controller]/[action]")]
    public class MobileController : Controller
    {

        private readonly NurseryContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IEmailSender _emailSender;
        static string token = "rLtt6JWvbUHDDhsZnfpAhpYk4dxYDQkbcPTyGaKp2TYqQgG7FGZ5Th_WD53Oq8Ebz6A53njUoo1w3pjU1D4vs_ZMqFiz_j0urb_BH9Oq9VZoKFoJEDAbRZepGcQanImyYrry7Kt6MnMdgfG5jn4HngWoRdKduNNyP4kzcp3mRv7x00ahkm9LAK7ZRieg7k1PDAnBIOG3EyVSJ5kK4WLMvYr7sCwHbHcu4A5WwelxYK0GMJy37bNAarSJDFQsJ2ZvJjvMDmfWwDVFEVe_5tOomfVNt6bOg9mexbGjMrnHBnKnZR1vQbBtQieDlQepzTZMuQrSuKn-t5XZM7V6fCW7oP-uXGX-sMOajeX65JOf6XVpk29DP6ro8WTAflCDANC193yof8-f5_EYY-3hXhJj7RBXmizDpneEQDSaSz5sFk0sV5qPcARJ9zGG73vuGFyenjPPmtDtXtpx35A-BVcOSBYVIWe9kndG3nclfefjKEuZ3m4jL9Gg1h2JBvmXSMYiZtp9MR5I6pvbvylU_PP5xJFSjVTIz7IQSjcVGO41npnwIxRXNRxFOdIUHn0tjQ-7LwvEcTXyPsHXcMD8WtgBh-wxR8aKX7WPSsT1O8d8reb2aR7K3rkV3K82K_0OgawImEpwSvp9MNKynEAJQS6ZHe_J_l77652xwPNxMRTMASk1ZsJL";
        public HttpClient httpClient { get; set; }
        private readonly IConfiguration _configuration;

        public MobileController(NurseryContext context, IEmailSender emailSender, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
            ApplicationDbContext db, RoleManager<IdentityRole> roleManager, IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _db = db;
            _roleManager = roleManager;
            _hostEnvironment = hostEnvironment;
            httpClient = new HttpClient();
            _emailSender = emailSender;
            _configuration = configuration;
        }



        [HttpGet]
        public IActionResult GetNurseryById([FromQuery] int NurseryMemberId)
        {
            try
            {
                var nurserymember = _context.NurseryMember.Where(c => c.NurseryMemberId == NurseryMemberId).Select(i => new
                {
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
                    Area = i.Area,
                    City = i.Area.City,
                    Country = i.Area.City.Country,
                    i.AgeCategory,
                    i.Address,
                    i.Lat,
                    i.Lng,
                    i.Logo,
                    i.Banner,
                    i.NurseryImage,
                    i.AvailableSeats,
                    // i.PaymentMethod,
                    NurserySubscription = i.NurserySubscription.OrderByDescending(e => e.NurserySubscriptionId).FirstOrDefault(),
                    i.IsSlider,
                    i.Child,
                    i.NurseryStudyLanguages


                }).FirstOrDefault();
                if (nurserymember != null)
                {
                    return Ok(new { status = true, nurserymember });
                }
                return Ok(new { status = false, message = "Something went wrong" });


            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }


        }
        [HttpGet]
        public IActionResult GetParentById([FromQuery] int ParentId)
        {
            try
            {
                var parent = _context.Parent.Select(i => new
                {
                    i.ParentId,
                    i.ParentName,
                    i.ParentAddress,
                    i.ParentPhone,
                    i.ParentEmail,
                    Child = i.Child.Select(j => new
                    {
                        j.ChildId,
                        j.ChildName,
                        j.ChildPhone,
                        j.ChildEmail,
                        j.ChildImage,
                        j.ParentId,
                        j.NurseryMemberId,
                        j.NurseryMember
                    })
                }).FirstOrDefault(c => c.ParentId == ParentId);

                return Ok(new { status = true, parent }
                );

            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }


        }
        [HttpGet]
        public async Task<ActionResult<ApplicationUser>> Login([FromQuery] string Email, [FromQuery] string Password)/*, [FromQuery] string deviceMac, [FromQuery] string deviceId*/
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(Email);

                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, Password, true);
                    if (result.Succeeded)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        var EntityName = user.EntityName;
                        if (roles != null && roles.FirstOrDefault() == "Nursery")
                        {
                            var Nursery = await _context.NurseryMember.FindAsync(user.EntityId);
                            //var macAddExist = _context.MacsAccounts.Where(e => e.MacDevice == deviceMac).FirstOrDefault();
                            //if (macAddExist == null)
                            //{
                            //    var MacObj = new MacsAccount()
                            //    {
                            //        DeviceId = deviceId,
                            //        MacDevice = deviceMac,
                            //    };
                            //}
                            //else
                            //{
                            //    macAddExist.DeviceId = deviceMac;
                            //    _context.Attach(macAddExist).State = EntityState.Modified;
                            //}
                            //_context.SaveChanges();
                            return Ok(new { status = true, Message = "Nursery Login successfully!", user, Nursery, EntityName });
                        }

                        if (roles != null && roles.FirstOrDefault() == "Parent")
                        {
                            var Parent = await _context.Parent.FindAsync(user.EntityId);
                            return Ok(new { status = true, Message = "Parent Login successfully!", user, Parent, EntityName });
                        }


                    }
                }

                return Ok(new { status = false, message = "User Not Found" });

            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }


        }
        //[HttpPost]
        //public async Task<IActionResult> NurseryRegister([FromBody] RegistrationModel registrationModel)
        //{
        //    try
        //    {
        //        var userExists = await _userManager.FindByEmailAsync(registrationModel.Email);
        //        if (userExists != null)
        //            return Ok(new { status = false, Message = "User already exists!" });
        //        var plan = _context.Plan.Find(registrationModel.PlanId);

        //        if (plan == null)
        //        {

        //            return Ok(new { status = false, Message = "Nursery creation failed! Please Select Plan and try again." });

        //        }
        //        var planType = _context.PlanTypes.Find(registrationModel.PlanTypeId);
        //        if (planType == null)
        //        {

        //            return Ok(new { status = false, Message = "Nursery creation failed! Please Select PlanType and try again." });

        //        }
        //        var area = _context.Area.Find(registrationModel.AreaId);
        //        if (area == null)
        //        {
        //            return Ok(new { status = false, Message = "Nursery creation failed! Please Select  and try again." });
        //        }

        //        if (registrationModel.PaymentMethodId == 0)
        //        {
        //            return Ok(new { status = false, Message = "Nursery creation failed! Please Select Payment Method and try again." });
        //        }
        //        var nurseryMember = new NurseryMember();

        //        if (registrationModel.Logo != null&&registrationModel.Logo!="")
        //        {
        //            string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/NurseryMember");
        //            var bytes = Convert.FromBase64String(registrationModel.Logo);
        //            string uniqePictureName = Guid.NewGuid() + ".jpeg";
        //            string uploadedImagePath = Path.Combine(uploadFolder, uniqePictureName);
        //            using (var imageFile = new FileStream(uploadedImagePath, FileMode.Create))
        //            {
        //                imageFile.Write(bytes, 0, bytes.Length);
        //                imageFile.Flush();
        //            }
        //            nurseryMember.Logo = uniqePictureName;
        //        }
        //        if (registrationModel.Banner != null && registrationModel.Banner != "")
        //        {
        //            var bytes = Convert.FromBase64String(registrationModel.Banner);
        //            string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/NurseryMember");
        //            string uniqePictureName = Guid.NewGuid() + ".jpeg";
        //            string uploadedImagePath = Path.Combine(uploadFolder, uniqePictureName);
        //            using (var imageFile = new FileStream(uploadedImagePath, FileMode.Create))
        //            {
        //                imageFile.Write(bytes, 0, bytes.Length);
        //                imageFile.Flush();
        //            }
        //            nurseryMember.Banner = uniqePictureName;
        //        }
        //        nurseryMember.NurseryTlAr = registrationModel.NurseryTlAr;
        //        nurseryMember.NurseryTlEn = registrationModel.NurseryTlEn;
        //        nurseryMember.NurseryDescAr = registrationModel.NurseryDescAr;
        //        nurseryMember.NurseryDescEn = registrationModel.NurseryDescEn;
        //        nurseryMember.Email = registrationModel.Email;
        //        nurseryMember.Phone1 = registrationModel.Phone1;
        //        nurseryMember.Phone2 = registrationModel.Phone2;
        //        nurseryMember.Fax = registrationModel.Fax;
        //        nurseryMember.Mobile = registrationModel.Mobile;
        //        nurseryMember.Facebook = registrationModel.Facebook;
        //        nurseryMember.Twitter = registrationModel.Twitter;
        //        nurseryMember.Instagram = registrationModel.Instagram;
        //        nurseryMember.WorkTimeFrom = registrationModel.WorkTimeFrom;
        //        nurseryMember.WorkTimeTo = registrationModel.WorkTimeTo;
        //        nurseryMember.AgeCategoryId = registrationModel.AgeCategoryId;
        //        nurseryMember.TransportationService = registrationModel.TransportationService;
        //        nurseryMember.SpecialNeeds = registrationModel.SpecialNeeds;
        //        nurseryMember.Language = registrationModel.Language;
        //        nurseryMember.CountryId = _context.Area.Include(c => c.City).FirstOrDefault(c => c.AreaId == registrationModel.AreaId)?.City.CountryId;
        //        nurseryMember.CityId = _context.Area.FirstOrDefault(c => c.AreaId == registrationModel.AreaId)?.CityId;
        //        nurseryMember.AreaId = registrationModel.AreaId;
        //        nurseryMember.Address = registrationModel.Address;
        //        nurseryMember.Lat = registrationModel.Lat;
        //        nurseryMember.Lng = registrationModel.Lng;
        //        nurseryMember.AvailableSeats = registrationModel.AvailableSeats;
        //        nurseryMember.PaymentMethodId = registrationModel.PaymentMethodId;
        //        nurseryMember.IsActive = false;
        //        _context.NurseryMember.Add(nurseryMember);
        //        _context.SaveChanges();


        //        if (!(nurseryMember.NurseryMemberId > 0))
        //        {
        //            return Ok(new { status = false, Message = "Nursery creation failed! Please try again." });

        //        }

        //        if (registrationModel.NurseryImage != null&& registrationModel.NurseryImage.Count != 0)
        //        {
        //            var ListImage = new List<NurseryImage>();
        //            foreach (var item in registrationModel.NurseryImage)
        //            {
        //                var ins = new NurseryImage();
        //                string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/NurseryMember");
        //                var bytes = Convert.FromBase64String(item);
        //                string uniqePictureName = Guid.NewGuid() + ".jpeg";
        //                string uploadedImagePath = Path.Combine(uploadFolder, uniqePictureName);
        //                using (var imageFile = new FileStream(uploadedImagePath, FileMode.Create))
        //                {
        //                    imageFile.Write(bytes, 0, bytes.Length);
        //                    imageFile.Flush();
        //                }
        //                ins.Pic = uniqePictureName;
        //                ins.NurseryId = nurseryMember.NurseryMemberId;
        //                ListImage.Add(ins);
        //            }
        //            _context.NurseryImage.AddRange(ListImage);
        //            _context.SaveChanges();
        //        }
        //        double totalCost = plan.Price.Value + planType.Cost;
        //        var subscription = new NurserySubscription();
        //        subscription.PlanId = registrationModel.PlanId;
        //        subscription.NurseryId = nurseryMember.NurseryMemberId;
        //        subscription.Price = plan.Price;
        //        subscription.StartDate = DateTime.Now;
        //        subscription.EndDate = DateTime.Now.AddMonths(plan.DurationInMonth.Value);
        //        _context.NurserySubscription.Add(subscription);
        //        _context.SaveChanges();

        //        var user = new ApplicationUser
        //        {
        //            UserName = registrationModel.Email,
        //            Email = registrationModel.Email,
        //            PhoneNumber = registrationModel.Mobile,
        //            EntityId = nurseryMember.NurseryMemberId,
        //            EntityName = 2

        //        };

        //        var result = await _userManager.CreateAsync(user, registrationModel.Password);

        //        if (!result.Succeeded)
        //        {
        //            _context.NurseryMember.Remove(nurseryMember);
        //            _context.SaveChanges();
        //            return Ok(new { status = false, Message = "User creation failed! Please check user details and try again." });

        //        }
        //        await _userManager.AddToRoleAsync(user, "Nursery");
        //        if (nurseryMember.PaymentMethodId == 1)
        //        {
        //            var requesturl = "https://api.upayments.com/test-payment";
        //            var fields = new
        //            {
        //                merchant_id = "1201",
        //                username = "test",
        //                password = "test",
        //                order_id = nurseryMember.NurseryMemberId,
        //                total_price = totalCost,
        //                test_mode = 0,
        //                CstFName = nurseryMember.NurseryTlEn,
        //                CstEmail = nurseryMember.Email,
        //                CstMobile = nurseryMember.Phone1,
        //                api_key = "jtest123",
        //                //success_url = "https://localhost:44354/success",
        //                //error_url = "https://localhost:44354/failed"
        //                success_url = "http://alhadhanah.com/success",
        //                error_url = "http://alhadhanah.com/failed"

        //            };
        //            var content = new StringContent(JsonConvert.SerializeObject(fields), Encoding.UTF8, "application/json");
        //            var task = httpClient.PostAsync(requesturl, content);
        //            var res = await task.Result.Content.ReadAsStringAsync();
        //            var paymenturl = JsonConvert.DeserializeObject<paymenturl>(res);
        //            if (paymenturl.status == "success")

        //            {
        //                return Ok(new { status = true, Message = "Nursery creation successfully!", user, nurseryMember, nurseryMember.NurseryImage, EntityName = 2 ,paymentUrl=paymenturl.paymentURL});
        //            }
        //            else
        //            {
        //                _context.NurserySubscription.Remove(subscription);
        //                _context.NurseryMember.Remove(nurseryMember);
        //                await  _userManager.DeleteAsync(user);
        //                return Ok(new { status = false, Message = "SomeThing went Error while Rquesting Payment Gateway !" });
        //            }

        //        }
        //        else if (nurseryMember.PaymentMethodId == 3)
        //        {

        //            var sendPaymentRequest = new
        //            {

        //                CustomerName = nurseryMember.NurseryTlEn,
        //                NotificationOption = "LNK",
        //                InvoiceValue = totalCost,
        //                CallBackUrl = "http://alhadhanah.com/FattorahSuccess",
        //                ErrorUrl = "http://alhadhanah.com/FattorahError",
        //                UserDefinedField = nurseryMember.NurseryMemberId,
        //                CustomerEmail = nurseryMember.Email
        //            };
        //            var sendPaymentRequestJSON = JsonConvert.SerializeObject(sendPaymentRequest);

        //            string url = "https://apitest.myfatoorah.com/v2/SendPayment";
        //            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //            var httpContent = new StringContent(sendPaymentRequestJSON, Encoding.UTF8, "application/json");
        //            var responseMessage = httpClient.PostAsync(url, httpContent);
        //            var res = await responseMessage.Result.Content.ReadAsStringAsync();
        //            var FattoraRes = JsonConvert.DeserializeObject<FattorhResult>(res);


        //            if (FattoraRes.IsSuccess == true)
        //            {
        //                Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(res);
        //                var InvoiceRes = jObject["Data"].ToObject<InvoiceData>();
        //                return Ok(new { status = true, Message = "Nursery creation successfully!", user, nurseryMember, nurseryMember.NurseryImage, EntityName = 2, paymentUrl = InvoiceRes.InvoiceURL });


        //            }
        //            else
        //            {

        //                _context.NurserySubscription.Remove(subscription);
        //                _context.NurseryMember.Remove(nurseryMember);
        //                await _userManager.DeleteAsync(user);
        //                return Ok(new { status = false, Message = "SomeThing went Error while Rquesting Payment Gateway !" });


        //            }


        //        }
        //        else 
        //        {
        //            await _emailSender.SendEmailAsync(
        //          registrationModel.Email,
        //           $"Welcome To {registrationModel.NurseryTlEn} ...",
        //            $"Thank You For Registration");
        //            return Ok(new { status = true, Message = "Nursery creation successfully!", user, nurseryMember, nurseryMember.NurseryImage, EntityName = 2 });

        //        }




        //    }
        //    catch (Exception)
        //    {

        //        return Ok(new { status = false, Message = "Something went wrong." });
        //    }
        //}
        [HttpPost]
        public async Task<IActionResult> NurseryRegister([FromBody] RegistrationModel registrationModel)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(registrationModel.Email);
                if (userExists != null)
                    return Ok(new { status = false, Message = "User already exists!" });
                var plan = _context.Plan.Find(registrationModel.PlanId);

                if (plan == null)
                {

                    return Ok(new { status = false, Message = "Nursery creation failed! Please Select Plan and try again." });

                }
                var planType = _context.PlanTypes.Find(registrationModel.PlanTypeId);
                if (planType == null)
                {

                    return Ok(new { status = false, Message = "Nursery creation failed! Please Select PlanType and try again." });

                }
                if (registrationModel.StudyLanguages.Count == 0)
                {

                    return Ok(new { status = false, Message = "You Must Select AtLeast One Study Language" });

                }
                var area = _context.Area.Find(registrationModel.AreaId);
                if (area == null)
                {
                    return Ok(new { status = false, Message = "Nursery creation failed! Please Select  and try again." });
                }

                if (registrationModel.PaymentMethodId == 0)
                {
                    return Ok(new { status = false, Message = "Nursery creation failed! Please Select Payment Method and try again." });
                }
                var nurseryMember = new NurseryMember();

                if (registrationModel.Logo != null && registrationModel.Logo != "")
                {
                    string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/NurseryMember");
                    var bytes = Convert.FromBase64String(registrationModel.Logo);
                    string uniqePictureName = Guid.NewGuid() + ".jpeg";
                    string uploadedImagePath = Path.Combine(uploadFolder, uniqePictureName);
                    using (var imageFile = new FileStream(uploadedImagePath, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }
                    nurseryMember.Logo = uniqePictureName;
                }
                if (registrationModel.Banner != null && registrationModel.Banner != "")
                {
                    var bytes = Convert.FromBase64String(registrationModel.Banner);
                    string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/NurseryMember");
                    string uniqePictureName = Guid.NewGuid() + ".jpeg";
                    string uploadedImagePath = Path.Combine(uploadFolder, uniqePictureName);
                    using (var imageFile = new FileStream(uploadedImagePath, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }
                    nurseryMember.Banner = uniqePictureName;
                }
                List<NurseryStudyLanguage> nurseryStudyLanguages = new List<NurseryStudyLanguage>();

                for (int item = 0; item < registrationModel.StudyLanguages.Count; item++)
                {
                    NurseryStudyLanguage nurseryStudy = new NurseryStudyLanguage()
                    {

                        LanguageId = registrationModel.StudyLanguages[item]
                    };
                    nurseryStudyLanguages.Add(nurseryStudy);


                }

                nurseryMember.NurseryTlAr = registrationModel.NurseryTlAr;
                nurseryMember.NurseryTlEn = registrationModel.NurseryTlEn;
                nurseryMember.NurseryDescAr = registrationModel.NurseryDescAr;
                nurseryMember.NurseryDescEn = registrationModel.NurseryDescEn;
                nurseryMember.Email = registrationModel.Email;
                nurseryMember.Phone1 = registrationModel.Phone1;
                nurseryMember.Phone2 = registrationModel.Phone2;
                nurseryMember.Fax = registrationModel.Fax;
                nurseryMember.Mobile = registrationModel.Mobile;
                nurseryMember.Facebook = registrationModel.Facebook;
                nurseryMember.Twitter = registrationModel.Twitter;
                nurseryMember.Instagram = registrationModel.Instagram;
                nurseryMember.WorkTimeFrom = registrationModel.WorkTimeFrom;
                nurseryMember.WorkTimeTo = registrationModel.WorkTimeTo;
                nurseryMember.AgeCategoryId = registrationModel.AgeCategoryId;
                nurseryMember.TransportationService = registrationModel.TransportationService;
                nurseryMember.SpecialNeeds = registrationModel.SpecialNeeds;
                nurseryMember.Language = registrationModel.Language;
                nurseryMember.CountryId = _context.Area.Include(c => c.City).FirstOrDefault(c => c.AreaId == registrationModel.AreaId)?.City.CountryId;
                nurseryMember.CityId = _context.Area.FirstOrDefault(c => c.AreaId == registrationModel.AreaId)?.CityId;
                nurseryMember.AreaId = registrationModel.AreaId;
                nurseryMember.Address = registrationModel.Address;
                nurseryMember.Lat = registrationModel.Lat;
                nurseryMember.Lng = registrationModel.Lng;
                nurseryMember.AvailableSeats = registrationModel.AvailableSeats;
                nurseryMember.NurseryStudyLanguages = nurseryStudyLanguages;
                nurseryMember.Password = registrationModel.Password;

                _context.NurseryMember.Add(nurseryMember);
                _context.SaveChanges();
                if (!(nurseryMember.NurseryMemberId > 0))
                {
                    return Ok(new { status = false, Message = "Nursery creation failed! Please try again." });
                }

                if (registrationModel.NurseryImage != null && registrationModel.NurseryImage.Count != 0)
                {
                    var ListImage = new List<NurseryImage>();
                    foreach (var item in registrationModel.NurseryImage)
                    {
                        var ins = new NurseryImage();
                        string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/NurseryMember");
                        var bytes = Convert.FromBase64String(item);
                        string uniqePictureName = Guid.NewGuid() + ".jpeg";
                        string uploadedImagePath = Path.Combine(uploadFolder, uniqePictureName);
                        using (var imageFile = new FileStream(uploadedImagePath, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                        }
                        ins.Pic = uniqePictureName;
                        ins.NurseryId = nurseryMember.NurseryMemberId;
                        ListImage.Add(ins);
                    }
                    _context.NurseryImage.AddRange(ListImage);
                    _context.SaveChanges();
                }
                double TotalCost = plan.Price.Value + planType.Cost;
                var subscription = new NurserySubscription();
                subscription.PlanId = registrationModel.PlanId;
                subscription.NurseryId = nurseryMember.NurseryMemberId;
                subscription.Price = TotalCost;
                subscription.PlanTypeId = registrationModel.PlanTypeId;
                subscription.StartDate = DateTime.Now;
                subscription.PaymentMethodId = registrationModel.PaymentMethodId;
                subscription.EndDate = DateTime.Now.AddMonths(plan.DurationInMonth.Value);
                _context.NurserySubscription.Add(subscription);
                _context.SaveChanges();

                var user = new ApplicationUser
                {
                    UserName = registrationModel.Email,
                    Email = registrationModel.Email,
                    PhoneNumber = registrationModel.Mobile,
                    EntityId = nurseryMember.NurseryMemberId,
                    EntityName = 2

                };

                var result = await _userManager.CreateAsync(user, registrationModel.Password);

                if (!result.Succeeded)
                {
                    _context.NurseryMember.Remove(nurseryMember);
                    _context.SaveChanges();
                    return Ok(new { status = false, Message = "User creation failed! Please check user details and try again." });

                }
                await _userManager.AddToRoleAsync(user, "Nursery");
                if (registrationModel.PaymentMethodId == 1)
                {
                    var requesturl = "https://api.upayments.com/test-payment";
                    var fields = new
                    {
                        merchant_id = "1201",
                        username = "test",
                        password = "test",
                        order_id = subscription.NurserySubscriptionId,
                        total_price = TotalCost,
                        test_mode = 0,
                        CstFName = nurseryMember.NurseryTlEn,
                        CstEmail = nurseryMember.Email,
                        CstMobile = nurseryMember.Phone1,
                        api_key = "jtest123",
                        //success_url = "https://localhost:44354/success",
                        //error_url = "https://localhost:44354/failed"
                        success_url = "http://alhadhanah.com/success",
                        error_url = "http://alhadhanah.com/failed"

                    };
                    var content = new StringContent(JsonConvert.SerializeObject(fields), Encoding.UTF8, "application/json");
                    var task = httpClient.PostAsync(requesturl, content);
                    var res = await task.Result.Content.ReadAsStringAsync();
                    var paymenturl = JsonConvert.DeserializeObject<paymenturl>(res);
                    if (paymenturl.status == "success")

                    {
                        return Ok(new { status = true, Message = "Nursery creation successfully!", user, nurseryMember, nurseryMember.NurseryImage, EntityName = 2, paymentUrl = paymenturl.paymentURL });
                    }
                    else
                    {
                        _context.NurserySubscription.Remove(subscription);
                        _context.NurseryMember.Remove(nurseryMember);
                        await _userManager.DeleteAsync(user);
                        return Ok(new { status = false, Message = "SomeThing went Error while Rquesting Payment Gateway !" });
                    }

                }
                 else if (registrationModel.PaymentMethodId == 3)
                {
                    bool IsProd = bool.Parse(_configuration["IsProd"]);
                    var TestToken = _configuration["Secret_API_Key_Test"];
                    var LiveToken = _configuration["Secret_API_Key_Live"];


                    var TapMessage = new
                    {
                        amount = TotalCost,
                        currency = "KWD",
                        threeDSecure = true,
                        save_card = false,
                        description = "Nursery Fees",
                        statement_descriptor = "Sample",
                        metadata = new
                        {
                            udf1 = user.Id,
                            CstFName = nurseryMember.NurseryTlEn,
                            CstEmail = nurseryMember.Email,
                            CstMobile = nurseryMember.Phone1,
                            order_id = subscription.NurserySubscriptionId,
                        },
                        reference = new
                        {
                            transaction = "txn_0001",
                        },
                        receipt = new
                        {
                            email = false,
                            sms = true
                        },
                        customer = new
                        {
                            first_name = user.UserName,
                            middle_name = "test",
                            last_name = "test",
                            email = user.UserName,
                            phone = new
                            {
                                country_code = "965",
                                number = "50143413"
                            }
                        },
                        merchant = new { id = "21900800" },
                        source = new { id = "src_kw.knet" },
                        //redirect = new { url = "https://localhost:7203/CallBack" }
                        redirect = new { url = "https://localhost:5001/CallBack" }
                    };


                    var sendPaymentRequestJSON1 = JsonConvert.SerializeObject(TapMessage);
                    var client = new RestClient("https://api.tap.company/v2/charges");
                    var request = new RestRequest();
                    request.AddHeader("content-type", "application/json");


                    if (IsProd) // fattorah live
                    {

                        request.AddHeader("authorization", LiveToken);
                        //end of TaP
                 
                    }
                    else // fattorah test
                    {

                        request.AddHeader("authorization", TestToken);

                    }
                    request.AddParameter("application/json", sendPaymentRequestJSON1, ParameterType.RequestBody);
                    RestResponse response = await client.PostAsync(request);
                    var DeserializeObjectResopnse = JsonConvert.DeserializeObject<JObject>(response.Content);

                    var Transaction = DeserializeObjectResopnse.GetValue("transaction");

                    var Url = Transaction["url"].ToString();

                    return Redirect(Url);

                }
                else
                {
                    var webRoot = _hostEnvironment.WebRootPath;

                    var pathToFile = _hostEnvironment.WebRootPath
                           + Path.DirectorySeparatorChar.ToString()
                           + "Templates"
                           + Path.DirectorySeparatorChar.ToString()
                           + "EmailTemplate"
                           + Path.DirectorySeparatorChar.ToString()
                           + "Email.html";
                    var builder = new BodyBuilder();
                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {

                        builder.HtmlBody = SourceReader.ReadToEnd();

                    }
                    string messageBody = string.Format(builder.HtmlBody,
                       subscription.StartDate.Value.ToShortDateString(),
                       subscription.EndDate.Value.ToShortDateString(),
                       TotalCost,
                       string.Format("{0:dddd, d MMMM yyyy}", DateTime.Now)
                       );
                    await _emailSender.SendEmailAsync(nurseryMember.Email, "Nursery Subscription", messageBody);
                    return Ok(new { status = true, Message = "Nursery creation successfully!", user, nurseryMember, nurseryMember.NurseryImage, EntityName = 2, paymentUrl = "http://alhadhanah.com/Thankyou" });


                }



            }
            catch (Exception)
            {

                return Ok(new { status = false, Message = "Something went wrong." });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddSubscription([FromBody] SubscriptionVM subscriptionVM)
        {
            try
            {
                var NurseryExists = _context.NurseryMember.Find(subscriptionVM.NurseryId);
                if (NurseryExists == null)
                    return Ok(new { status = false, Message = "User Not exists!" });
                var plan = _context.Plan.Find(subscriptionVM.PlanId);

                if (plan == null)
                {

                    return Ok(new { status = false, Message = " Please Select Plan and try again." });

                }
                var planType = _context.PlanTypes.Find(subscriptionVM.PlanTypeId);
                if (planType == null)
                {

                    return Ok(new { status = false, Message = " Please Select PlanType and try again." });

                }
                var userSubscription = _context.NurserySubscription.Where(e => e.NurseryId == subscriptionVM.NurseryId).OrderByDescending(e => e.NurserySubscriptionId).FirstOrDefault();

                if (userSubscription != null)
                {

                    if (userSubscription.EndDate > DateTime.Now)
                    {
                        return Ok(new { status = false, Message = "Nursery Aleardy Subscriped In Plan" });

                    }
                }
                var subscriptionObj = new NurserySubscription()
                {
                    NurseryId = subscriptionVM.NurseryId,
                    PlanId = subscriptionVM.PlanId,
                    PlanTypeId = subscriptionVM.PlanTypeId,
                    PaymentMethodId = subscriptionVM.PaymentMethodId,
                    Price = plan.Price + planType.Cost,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(plan.DurationInMonth.Value)

                };
                _context.NurserySubscription.Add(subscriptionObj);
                _context.SaveChanges();


                if (subscriptionVM.PaymentMethodId == 1)
                {
                    var requesturl = "https://api.upayments.com/test-payment";
                    var fields = new
                    {
                        merchant_id = "1201",
                        username = "test",
                        password = "test",
                        order_id = subscriptionObj.NurserySubscriptionId,
                        total_price = subscriptionObj.Price,
                        test_mode = 0,
                        CstFName = NurseryExists.NurseryTlEn,
                        CstEmail = NurseryExists.Email,
                        CstMobile = NurseryExists.Phone1,
                        api_key = "jtest123",
                        //success_url = "https://localhost:44354/success",
                        //error_url = "https://localhost:44354/SubPayFail"
                        success_url = "http://alhadhanah.com/success",
                        error_url = "http://alhadhanah.com/SubPayFail"

                    };
                    var content = new StringContent(JsonConvert.SerializeObject(fields), Encoding.UTF8, "application/json");
                    var task = httpClient.PostAsync(requesturl, content);
                    var res = await task.Result.Content.ReadAsStringAsync();
                    var paymenturl = JsonConvert.DeserializeObject<paymenturl>(res);
                    if (paymenturl.status == "success")

                    {
                        return Ok(new { status = true, Message = "Nursery Subscription successfully!", paymentUrl = paymenturl.paymentURL, Subscription = subscriptionObj });
                    }
                    else
                    {
                        _context.NurserySubscription.Remove(subscriptionObj);

                        return Ok(new { status = false, Message = "SomeThing went Error while Rquesting Payment Gateway !" });
                    }

                }
                else if (subscriptionVM.PaymentMethodId == 3)
                {

                    bool Fattorahstatus = bool.Parse(_configuration["FattorahStatus"]);
                    var TestToken = _configuration["TestToken"];
                    var LiveToken = _configuration["LiveToken"];
                    if (Fattorahstatus) // fattorah live
                    {
                        var sendPaymentRequest = new
                        {

                            CustomerName = NurseryExists.NurseryTlEn,
                            NotificationOption = "LNK",
                            InvoiceValue = subscriptionObj.Price,
                            CallBackUrl = "http://alhadhanah.com/FattorahSuccess",
                            ErrorUrl = "http://alhadhanah.com/FattorahSubPayFail",
                            UserDefinedField = subscriptionObj.NurserySubscriptionId,
                            CustomerEmail = NurseryExists.Email
                        };
                        var sendPaymentRequestJSON = JsonConvert.SerializeObject(sendPaymentRequest);

                        string url = "https://api.myfatoorah.com/v2/SendPayment";
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LiveToken);
                        var httpContent = new StringContent(sendPaymentRequestJSON, Encoding.UTF8, "application/json");
                        var responseMessage = httpClient.PostAsync(url, httpContent);
                        var res = await responseMessage.Result.Content.ReadAsStringAsync();
                        var FattoraRes = JsonConvert.DeserializeObject<FattorhResult>(res);


                        if (FattoraRes.IsSuccess == true)
                        {
                            Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(res);
                            var InvoiceRes = jObject["Data"].ToObject<InvoiceData>();
                            return Ok(new { status = true, Message = "Nursery Subscription successfully!", paymentUrl = InvoiceRes.InvoiceURL, Subscription = subscriptionObj });



                        }
                        else
                        {

                            _context.NurserySubscription.Remove(subscriptionObj);

                            return Ok(new { status = false, Message = "SomeThing went Error while Rquesting Payment Gateway !" });
                        }
                    }
                    else               //fattorah test
                    {
                        var sendPaymentRequest = new
                        {

                            CustomerName = NurseryExists.NurseryTlEn,
                            NotificationOption = "LNK",
                            InvoiceValue = subscriptionObj.Price,
                            CallBackUrl = "http://alhadhanah.com/FattorahSuccess",
                            ErrorUrl = "http://alhadhanah.com/FattorahSubPayFail",
                            UserDefinedField = subscriptionObj.NurserySubscriptionId,
                            CustomerEmail = NurseryExists.Email
                        };
                        var sendPaymentRequestJSON = JsonConvert.SerializeObject(sendPaymentRequest);

                        string url = "https://apitest.myfatoorah.com/v2/SendPayment";
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TestToken);
                        var httpContent = new StringContent(sendPaymentRequestJSON, Encoding.UTF8, "application/json");
                        var responseMessage = httpClient.PostAsync(url, httpContent);
                        var res = await responseMessage.Result.Content.ReadAsStringAsync();
                        var FattoraRes = JsonConvert.DeserializeObject<FattorhResult>(res);


                        if (FattoraRes.IsSuccess == true)
                        {
                            Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(res);
                            var InvoiceRes = jObject["Data"].ToObject<InvoiceData>();
                            return Ok(new { status = true, Message = "Nursery Subscription successfully!", paymentUrl = InvoiceRes.InvoiceURL, Subscription = subscriptionObj });



                        }
                        else
                        {

                            _context.NurserySubscription.Remove(subscriptionObj);

                            return Ok(new { status = false, Message = "SomeThing went Error while Rquesting Payment Gateway !" });
                        }
                    }



                }
                else
                {
                    var webRoot = _hostEnvironment.WebRootPath;

                    var pathToFile = _hostEnvironment.WebRootPath
                           + Path.DirectorySeparatorChar.ToString()
                           + "Templates"
                           + Path.DirectorySeparatorChar.ToString()
                           + "EmailTemplate"
                           + Path.DirectorySeparatorChar.ToString()
                           + "Email.html";
                    var builder = new BodyBuilder();
                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {

                        builder.HtmlBody = SourceReader.ReadToEnd();

                    }
                    string messageBody = string.Format(builder.HtmlBody,
                       subscriptionObj.StartDate.Value.ToShortDateString(),
                       subscriptionObj.EndDate.Value.ToShortDateString(),
                       plan.Price + planType.Cost,
                       string.Format("{0:dddd, d MMMM yyyy}", DateTime.Now)
                       );
                    await _emailSender.SendEmailAsync(NurseryExists.Email, "Nursery Subscription", messageBody);
                    return Ok(new { status = true, Message = "Nursery Subscription successfully!", paymentUrl = "http://alhadhanah.com/Thankyou", Subscription = subscriptionObj });

                }
            }
            catch (Exception e)
            {

                return Ok(new { status = false, Message = e.Message });
            }

        }
        [HttpGet]
        public IActionResult GetAreasList()
        {
            try
            {
                var list = _context.Area.Where(c => c.AreaIsActive == true && c.City.CityIsActive == true && c.City.Country.CountryIsActive == true).OrderBy(c => c.AreaOrderIndex).ToList();
                return Ok(new { list });
            }
            catch (Exception)
            {
                return Ok(new { status = false, message = "Something went wrong" });
            }

        }
        [HttpGet]
        public IActionResult GetCitiesList()
        {
            try
            {
                var list = _context.City.Include(c => c.Area.Where(c => c.AreaIsActive == true)).Where(c => c.CityIsActive == true && c.Country.CountryIsActive == true).OrderBy(c => c.CityOrderIndex).ToList();
                return Ok(new { list });
            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }

        }
        [HttpGet]
        public IActionResult GetPlansList([FromQuery] int CountryId)
        {
            try
            {
                var list = _context.Plan.Where(c => c.CountryId == CountryId && c.IsActive != false && c.IsActive != null && c.Country.CountryIsActive == true).ToList();
                return Ok(new { list });
            }
            catch (Exception)
            {
                return Ok(new { status = false, message = "Something went wrong" });
            }

        }
        [HttpPost]
        public async Task<IActionResult> ParentRegister([FromBody] ParentModel Model)

        {

            try
            {
                var userExists = await _userManager.FindByEmailAsync(Model.ParentEmail);
                if (userExists != null)
                    return Ok(new { status = false, Message = "User creation failed! Please check user details and try again." });

                var parent = new Parent();
                parent.ParentAddress = Model.ParentAddress;
                parent.ParentName = Model.ParentName;
                parent.ParentPhone = Model.ParentPhone;
                parent.ParentEmail = Model.ParentEmail;
                _context.Parent.Add(parent);
                _context.SaveChanges();
                if (!(parent.ParentId > 0))
                {
                    return Ok(new { status = false, Message = "User creation failed! Please check user details and try again." });

                }
                var user = new ApplicationUser
                {
                    UserName = Model.ParentEmail,
                    Email = Model.ParentEmail,
                    PhoneNumber = Model.ParentPhone,
                    EntityId = parent.ParentId,
                    EntityName = 3

                };
                var result = await _userManager.CreateAsync(user, Model.Password);
                if (!result.Succeeded)
                {
                    _context.Parent.Remove(parent);
                    _context.SaveChanges();
                    return Ok(new { status = false, Message = "User creation failed! Please check user details and try again." });
                }
                await _userManager.AddToRoleAsync(user, "Parent");
                var webRoot = _hostEnvironment.WebRootPath;

                var pathToFile = _hostEnvironment.WebRootPath
                       + Path.DirectorySeparatorChar.ToString()
                       + "Templates"
                       + Path.DirectorySeparatorChar.ToString()
                       + "EmailTemplate"
                       + Path.DirectorySeparatorChar.ToString()
                       + "ParentRegister.html";
                var builder = new BodyBuilder();
                using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                {

                    builder.HtmlBody = SourceReader.ReadToEnd();

                }
                string messageBody = string.Format(builder.HtmlBody,
                  parent.ParentName,
                   string.Format("{0:dddd, d MMMM yyyy}", DateTime.Now)
                   );
                await _emailSender.SendEmailAsync(parent.ParentEmail, "Nursery Subscription", messageBody);
                return Ok(new { status = true, Message = "User created successfully!", user, parent, EntityName = 3 });

            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }
        }
        [HttpGet]
        public async Task<IActionResult> userExists([FromQuery] string Email)

        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(Email);
                if (userExists != null)
                    return Ok(new { status = false, Message = "User already exists!" });
                else
                    return Ok(new { status = true, Message = "User not exists!" });

            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }

        }
        [HttpGet]
        public ActionResult GetPaymentMethodList()
        {
            /////////////

            try
            {
                var list = _context.PaymentMethod.Where(e => e.IsActive).ToList();
                return Ok(new { list });
            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }
        }
        [HttpGet]
        public ActionResult GetAgeCategoryList()
        {
            try
            {
                var list = _context.AgeCategory.Where(e => e.IsActive == true).ToList();
                return Ok(new { list });
            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }

        }
        [HttpPost]
        public IActionResult ContactUs([FromBody] ContactUs Model)
        {
            try
            {
                Model.TransDate = DateTime.Now;
                _context.ContactUs.Add(Model);
                _context.SaveChanges();
                return Ok(new { status = true, Message = "Message Sent Successfully" });

            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }

        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new { status = false, Message = "model not Valid" });


                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return Ok(new { status = false, Message = "User not found" });

                }
                var Result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!Result.Succeeded)
                {
                    foreach (var error in Result.Errors)
                    {
                        ModelState.TryAddModelError(error.Code, error.Description);
                    }
                    return Ok(new { status = false, message = ModelState });

                }

                return Ok(new { status = true, Message = "Password Changed" });

            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }


        }
        [HttpPost]
        public IActionResult EditParent([FromBody] Parent model)
        {
            try
            {
                Parent parent = _context.Parent.FirstOrDefault(e => e.ParentId == model.ParentId);
                if (parent == null)
                    return Ok(new { status = false, Message = "Parent Not Found" });


                parent.ParentAddress = model.ParentAddress;
                parent.ParentName = model.ParentName;
                parent.ParentPhone = model.ParentPhone;
                parent.ParentAddress = model.ParentAddress;
                _context.Attach(parent).State = EntityState.Modified;
                _context.SaveChangesAsync();

                return Ok(new { status = true, Message = "successfully Edit Parent !", parent });

            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });
            }


        }
        [HttpPost]
        public IActionResult EditNursery([FromBody] RegistrationModel model)
        {
            try
            {
                NurseryMember nurseryMember = _context.NurseryMember.FirstOrDefault(e => e.NurseryMemberId == model.NurseryMemberId);
                if (nurseryMember == null)
                    return Ok(new { status = false, Message = "Nursery Not Found" });
                string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/NurseryMember");
                string ImagePath = "";

                if (model.Logo != null && model.Logo != "" && nurseryMember.Logo != model.Logo)
                {

                    var bytes = Convert.FromBase64String(model.Logo);
                    string uniqePictureName = Guid.NewGuid() + ".jpeg";
                    string uploadedImagePath = Path.Combine(uploadFolder, uniqePictureName);
                    using (var imageFile = new FileStream(uploadedImagePath, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }
                    ImagePath = uploadFolder + "/" + nurseryMember.Logo;
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                    nurseryMember.Logo = uniqePictureName;
                }

                if (model.Banner != null && model.Banner != "" && nurseryMember.Banner != model.Banner)
                {
                    var bytes = Convert.FromBase64String(model.Banner);
                    string uniqePictureName = Guid.NewGuid() + ".jpeg";
                    string uploadedImagePath = Path.Combine(uploadFolder, uniqePictureName);
                    using (var imageFile = new FileStream(uploadedImagePath, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }
                    ImagePath = uploadFolder + "/" + nurseryMember.Banner;
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                    nurseryMember.Banner = uniqePictureName;
                }
                List<NurseryStudyLanguage> nurseryStudyLanguages = new List<NurseryStudyLanguage>();

                for (int item = 0; item < model.StudyLanguages.Count; item++)
                {
                    NurseryStudyLanguage nurseryStudy = new NurseryStudyLanguage()
                    {

                        LanguageId = model.StudyLanguages[item],
                        NurseryId = nurseryMember.NurseryMemberId
                    };
                    nurseryStudyLanguages.Add(nurseryStudy);



                }
                var list = _context.NurseryStudyLanguages.Where(e => e.NurseryId == nurseryMember.NurseryMemberId);
                _context.NurseryStudyLanguages.RemoveRange(list);
                _context.NurseryStudyLanguages.AddRange(nurseryStudyLanguages);
                nurseryMember.NurseryTlAr = model.NurseryTlAr;
                nurseryMember.NurseryTlEn = model.NurseryTlEn;
                nurseryMember.NurseryDescAr = model.NurseryDescAr;
                nurseryMember.NurseryDescEn = model.NurseryDescEn;
                nurseryMember.Email = model.Email;
                nurseryMember.Phone1 = model.Phone1;
                nurseryMember.Phone2 = model.Phone2;
                nurseryMember.Fax = model.Fax;
                nurseryMember.Mobile = model.Mobile;
                nurseryMember.Facebook = model.Facebook;
                nurseryMember.Twitter = model.Twitter;
                nurseryMember.Instagram = model.Instagram;
                nurseryMember.WorkTimeFrom = model.WorkTimeFrom;
                nurseryMember.WorkTimeTo = model.WorkTimeTo;
                nurseryMember.AgeCategoryId = model.AgeCategoryId;
                nurseryMember.TransportationService = model.TransportationService;
                nurseryMember.SpecialNeeds = model.SpecialNeeds;
                nurseryMember.Language = model.Language;
                nurseryMember.CountryId = _context.Area.Include(c => c.City).FirstOrDefault(c => c.AreaId == nurseryMember.AreaId)?.City.CountryId;
                nurseryMember.CityId = _context.Area.FirstOrDefault(c => c.AreaId == nurseryMember.AreaId)?.CityId;
                nurseryMember.AreaId = model.AreaId;
                nurseryMember.Address = model.Address;
                nurseryMember.Lat = model.Lat;
                nurseryMember.Lng = model.Lng;
                nurseryMember.AvailableSeats = model.AvailableSeats;
                //nurseryMember.PaymentMethodId = model.PaymentMethodId;


                _context.Attach(nurseryMember).State = EntityState.Modified;
                _context.SaveChanges();
                var newListImage = new List<NurseryImage>();
                foreach (var item in model.NurseryImage)
                {
                    var ins = new NurseryImage();
                    var bytes = Convert.FromBase64String(item);
                    string uniqePictureName = Guid.NewGuid() + ".jpeg";
                    string uploadedImagePath = Path.Combine(uploadFolder, uniqePictureName);
                    using (var imageFile = new FileStream(uploadedImagePath, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }
                    ins.Pic = uniqePictureName;
                    ins.NurseryId = nurseryMember.NurseryMemberId;
                    newListImage.Add(ins);
                }
                _context.NurseryImage.AddRange(newListImage);
                _context.SaveChanges();
                return Ok(new { status = true, Message = "successfully Edit Nursery !", nurseryMember, newListImage });

            }
            catch (Exception ex)
            {

                return Ok(new { status = false, message = "Something went wrong", ex.InnerException });
            }


        }
        [HttpGet]
        public IActionResult GetSystemConfiguration()

        {
            try
            {
                var systemConfiguration = _context.Configuration.FirstOrDefault();
                return Ok(new { systemConfiguration });

            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }

        }
        [HttpGet]
        public IActionResult GetFAQList()
        {
            try
            {
                var list = _context.FAQ.ToList();
                return Ok(new { list });

            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }

        }
        [HttpGet]
        public IActionResult GetPageContent([FromQuery] int PageContentId)
        {

            try
            {
                var list = _context.PageContent.FirstOrDefault(c => c.PageContentId == PageContentId);
                return Ok(new { list });

            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }
        }

        //[HttpPost]
        //public IActionResult AddNurserySubscription([FromBody] NurserySubscription model)

        //{
        //    try
        //    {
        //        var plan = _context.Plan.Find(model.PlanId);

        //        if (plan == null)
        //        {

        //            return Ok(new { status = false, message = "Plan not Found" });

        //        }
        //        var Nursery = _context.NurseryMember.Find(model.NurseryId);
        //        if (Nursery == null)
        //        {

        //            return Ok(new { status = false, message = "Nursery not Found" });

        //        }
        //        if (_context.NurserySubscription.Any(e => e.PlanId == model.PlanId && e.NurseryId == model.NurseryId))
        //        {
        //            return Ok(new { status = false, message = "Nursery already has subscribed in this paln" });
        //        }
        //        model.Price = plan.Price;
        //        model.StartDate = DateTime.Now;
        //        model.EndDate = DateTime.Now.AddMonths(plan.DurationInMonth.Value);
        //        _context.NurserySubscription.Add(model);
        //        _context.SaveChanges();
        //        return Ok(new { status = true, Message = "Subscription Added" });
        //    }
        //    catch (Exception)
        //    {

        //        return Ok(new { status = false, message = "Something went wrong" });
        //    }


        //}
        [HttpPost]
        public IActionResult DeleteNurseryImage(int ImageId)
        {
            try
            {
                var model = _context.NurseryImage.FirstOrDefault(item => item.NurseryImageId == ImageId);
                if (model != null)
                {
                    string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/NurseryMember");
                    string ImagePath = "";
                    ImagePath = uploadFolder + "/" + model.Pic;
                    _context.NurseryImage.Remove(model);
                    _context.SaveChangesAsync();

                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }

                    return Ok(new { status = true, Message = "Image Deleted Successfully" });

                }
                return Ok(new { status = false, Message = "Image not Found " });
            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });
            }


        }
        [HttpGet]
        public IActionResult GetCountriesList()
        {
            try
            {
                var list = _context.Country.Where(c => c.CountryIsActive == true).Select(i => new
                {
                    i.CountryId,
                    i.CountryTlAr,
                    i.CountryTlEn,
                    cities = i.City.Where(c => c.CityIsActive == true).Select(j => new
                    {
                        j.CityId,
                        j.CityTlAr,
                        j.CityTlEn,
                        j.CityIsActive,
                        j.CityOrderIndex,
                        areas = j.Area.Where(c => c.AreaIsActive == true).ToList(),


                    }),

                    i.CountryPic,
                    i.CountryIsActive,
                    i.CountryOrderIndex
                });

                return Ok(new { list });
            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }

        }
        [HttpGet]
        public IActionResult GetAdzList([FromQuery] int CountryId)
        {
            try
            {
                var list = _context.Adz.Where(c => c.CountryId == CountryId && c.AdzIsActive == true && c.Country.CountryIsActive == true).ToList();
                return Ok(new { list });
            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }

        }
        [HttpGet]
        public IActionResult Search([FromQuery] string SearchText, [FromQuery] int CountryId)

        {
            try
            {
                var list = _context.NurseryMember.Where(c => c.CountryId == CountryId && c.NurserySubscription.OrderByDescending(e => e.NurserySubscriptionId).FirstOrDefault().IsActive
                && c.Area.AreaIsActive == true && c.Area.City.CityIsActive == true &&
                c.Area.City.Country.CountryIsActive == true && c.IsActive == true &&
                (c.Address.Contains(SearchText) || c.NurseryTlAr.Contains(SearchText)
               || c.NurseryTlEn.Contains(SearchText) || c.NurseryDescAr.Contains(SearchText)
               || c.NurseryDescEn.Contains(SearchText) || c.Area.AreaTlEn.Contains(SearchText)
               || c.Area.AreaTlEn.Contains(SearchText) || c.Area.City.CityTlAr.Contains(SearchText) ||
               c.Area.City.CityTlEn.Contains(SearchText))
                ).Select(i => new
                {
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

                    Area = i.Area,
                    City = i.Area.City,
                    Country = i.Area.City.Country,

                    i.AgeCategory,
                    i.Address,
                    i.Lat,
                    i.Lng,
                    i.Logo,
                    i.Banner,
                    i.NurseryImage,
                    i.AvailableSeats,
                    i.NurseryStudyLanguages,
                    i.IsSlider
                });

                return Ok(new { list });
            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }
        }
        [HttpGet]
        public IActionResult GetNurseryByCountryId(int CountryId)
        {
            try
            {

                var list = _context.NurseryMember.Where(c => c.CountryId == CountryId &&
                c.NurserySubscription.OrderByDescending(e => e.NurserySubscriptionId).FirstOrDefault().IsActive && c.NurserySubscription.OrderByDescending(e => e.NurserySubscriptionId).FirstOrDefault().EndDate >= DateTime.Now && c.Area.AreaIsActive == true
                && c.Area.City.CityIsActive == true &&
                c.IsActive == true
                && c.Area.City.Country.CountryIsActive == true).Select(i => new
                {
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
                    Area = i.Area,
                    City = i.Area.City,
                    Country = i.Area.City.Country,
                    i.AgeCategory,
                    i.Address,
                    i.Lat,
                    i.Lng,
                    i.Logo,
                    i.Banner,
                    i.NurseryImage,
                    i.AvailableSeats,
                    NurserySubscription = i.NurserySubscription.OrderByDescending(e => e.NurserySubscriptionId).FirstOrDefault(),
                    i.IsSlider,
                    i.Child,
                    i.NurseryStudyLanguages
                });

                return Ok(new { list });
            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }

        }
        [HttpGet]
        public IActionResult GetBannerList(int countryId)
        {
            try
            {
                //var NurseryList = _context.NurseryMember.Include(e => e.NurserySubscription).Where(c => c.CountryId == countryId && c.NurserySubscription.Count > 0).ToList();
                var list = _context.NurseryMember.Where(c => c.CountryId == countryId && c.IsSlider == true && c.NurserySubscription.OrderByDescending(e => e.NurserySubscriptionId).FirstOrDefault().IsActive && c.NurserySubscription.OrderByDescending(e => e.NurserySubscriptionId).FirstOrDefault().EndDate >= DateTime.Now &&
                c.IsActive == true &&
                c.Area.AreaIsActive == true
                && c.Area.City.CityIsActive == true
                && c.Area.City.Country.CountryIsActive == true).Select(i => new
                {
                    i.NurseryMemberId,
                    i.NurseryTlAr,
                    i.NurseryTlEn,
                    Area = i.Area,
                    City = i.Area.City,
                    Country = i.Area.City.Country,
                    i.Logo,
                    i.Banner,
                    NurserySubscription = i.NurserySubscription.OrderByDescending(e => e.NurserySubscriptionId).FirstOrDefault(),

                });

                return Ok(new { list });
            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }

        }

        [HttpGet]
        public IActionResult GetChildrenbyParentId([FromQuery] int ParentId)
        {
            try
            {
                var list = _context.Child.Where(c => c.ParentId == ParentId).Select(i => new
                {
                    i.ChildId,
                    i.ChildName,
                    i.ChildPhone,
                    i.ChildEmail,
                    i.ChildImage,
                    i.ParentId,
                    i.NurseryMemberId,
                    //i.BirthDate,
                    parent = i.Parent
                });

                return Ok(new { list });
            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }

        }
        [HttpPost]
        public IActionResult AddChild([FromBody] Child Model)

        {
            try
            {
                var parentExists = _context.Parent.Where(c => c.ParentId == Model.ParentId).Any();
                if (!parentExists)
                {
                    return Ok(new { status = false, Message = "parent Not Found" });

                }
                if (ModelState.IsValid)
                {
                    if (Model.ChildImage != null)
                    {
                        string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/Child");
                        var bytes = Convert.FromBase64String(Model.ChildImage);
                        string uniqePictureName = Guid.NewGuid() + ".jpeg";
                        string uploadedImagePath = Path.Combine(uploadFolder, uniqePictureName);
                        using (var imageFile = new FileStream(uploadedImagePath, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                        }
                        Model.ChildImage = uniqePictureName;
                    }
                    _context.Child.Add(Model);
                    _context.SaveChanges();
                    return Ok(new { status = true, Message = "Child Added Successfully" });
                }

                return Ok(new { status = false, Message = "Child Not Add please Try Again" });

            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });
            }

        }

        [HttpGet]
        public IActionResult AddChildToNursery([FromQuery] int ChildId, int NurseryId)

        {
            try
            {
                var child = _context.Child.Find(ChildId);
                if (child == null)
                {
                    return Ok(new { status = false, Message = "child Not Found" });

                }
                var nursery = _context.NurseryMember.Include(e => e.AgeCategory).FirstOrDefault(e => e.NurseryMemberId == NurseryId);
                if (nursery == null)
                {
                    return Ok(new { status = false, Message = "nursery Not Found" });

                }

                if (child.NurseryMemberId != null && child.NurseryMemberId == NurseryId)
                {
                    return Ok(new { status = false, Message = "Child is already Added to this Nursery" });
                }
                if (child.NurseryMemberId != null && child.NurseryMemberId != NurseryId)
                {
                    var CurrentNursery = _context.NurseryMember.Include(e => e.AgeCategory).FirstOrDefault(e => e.NurseryMemberId == child.NurseryMemberId);
                    CurrentNursery.AvailableSeats = CurrentNursery.AvailableSeats + 1;
                    _context.NurseryMember.Update(CurrentNursery);
                }
                if (nursery.AvailableSeats <= 0)
                {
                    return Ok(new { status = false, Message = "No avalible Seats" });
                }

                if (child.AgeCategoryId != nursery.AgeCategoryId)
                {
                    return Ok(new { status = false, Message = "Child age is not suitable" });
                }

                child.NurseryMemberId = NurseryId;
                nursery.AvailableSeats = nursery.AvailableSeats - 1;
                _context.Child.Update(child);
                _context.NurseryMember.Update(nursery);
                _context.SaveChanges();

                return Ok(new { status = true, Message = "Child Added to nursery Successfully" });


            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });
            }

        }
        [HttpGet]
        public IActionResult DeleteChildToNursery([FromQuery] int ChildId)

        {
            try
            {
                var child = _context.Child.Find(ChildId);
                if (child == null)
                {
                    return Ok(new { status = false, Message = "child Not Found" });

                }

                _context.Child.Remove(child);
                _context.SaveChanges();
                return Ok(new { status = true, Message = "Child Deleted Successfully" });


            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });
            }

        }
        [HttpPost]
        public IActionResult DeleteChildFromNursery(int ChildId, int NurseryId)
        {
            try
            {
                var child = _context.Child.Find(ChildId);
                if (child == null)
                {
                    return Ok(new { status = false, Message = "child Not Found" });

                }
                var nursery = _context.NurseryMember.Include(e => e.AgeCategory).FirstOrDefault(e => e.NurseryMemberId == NurseryId);
                if (nursery == null)
                {
                    return Ok(new { status = false, Message = "nursery Not Found" });
                }

                if (child.NurseryMemberId != null && child.NurseryMemberId == NurseryId)
                {
                    child.NurseryMemberId = null;
                    nursery.AvailableSeats = nursery.AvailableSeats + 1;
                    _context.Child.Update(child);
                    _context.NurseryMember.Update(nursery);
                    _context.SaveChanges();

                    return Ok(new { status = true, Message = "Child Deleted from nursery Successfully" });
                }
                else if (child.NurseryMemberId != null && child.NurseryMemberId != NurseryId)
                {
                    return Ok(new { status = false, message = "Child Isn't registered in this Nursery" });
                }
                else
                {
                    return Ok(new { status = false, message = "Something went wrong" });
                }
            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });
            }

        }

        [HttpGet]
        public IActionResult MakeNotificationIsRead([FromQuery] int PublicNotificationDeviceId)
        {

            try
            {
                var model = _context.PublicNotificationDevice.Find(PublicNotificationDeviceId);
                if (model == null)
                {
                    return Ok(new { status = false, message = "Notification not Found" });


                }
                model.IsRead = true;
                _context.PublicNotificationDevice.Update(model);
                _context.SaveChanges();

                return Ok(new { status = true, message = "deviceId Added To trainer " });

            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }
        }
        [HttpPost]
        public IActionResult AddPublicDevice([FromBody] PublicDevice model)
        {

            try
            {
                var publicDevice = _context.PublicDevice.FirstOrDefault(c => c.DeviceId == model.DeviceId);
                if (publicDevice != null)
                {
                    publicDevice.CountryId = model.CountryId;
                    publicDevice.IsAndroiodDevice = model.IsAndroiodDevice;

                    _context.PublicDevice.Update(publicDevice);
                    _context.SaveChanges();
                    return Ok(new { status = true, message = "deviceId edited" });
                }
                _context.PublicDevice.Add(model);
                _context.SaveChanges();
                return Ok(new { status = true, message = "deviceId Added" });

            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex });
            }
        }

        [HttpGet]
        public IActionResult GetPublicNotificationByDeviceId([FromQuery] string deviceId)
        {
            try
            {
                var publicDevice = _context.PublicDevice.FirstOrDefault(c => c.DeviceId == deviceId);
                var List = _context.PublicNotificationDevice.Include(c => c.PublicNotification).Where(c => c.PublicDeviceId == publicDevice.PublicDeviceId);


                return Ok(new { status = true, List });
            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }


        }

        [HttpGet]
        public IActionResult DeletePublicNotification([FromQuery] int publicNotificationDeviceId)
        {
            try
            {
                var model = _context.PublicNotificationDevice.FirstOrDefault(c => c.PublicNotificationDeviceId == publicNotificationDeviceId);
                _context.Remove(model);
                _context.SaveChanges();
                return Ok(new { status = true });
            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }


        }

        [HttpGet]
        public IActionResult GetAllPlansType()
        {
            try
            {
                var PlansTypeList = _context.PlanTypes.ToList();
                return Ok(new { Status = true, PlansTypeList = PlansTypeList });
            }
            catch (Exception ex)
            {

                return Ok(new { status = false, message = ex.Message });

            }

        }

        //[HttpGet]
        //public IActionResult test()
        //{
        //    try
        //    {
        //       var CountryId = _context.Area.Include(c=>c.City).FirstOrDefault(c => c.AreaId ==13)?.City.CountryId;
        //        var CityId = _context.Area.FirstOrDefault(c => c.AreaId == 13)?.CityId;


        //        return Ok(new { status = true, CityId, CountryId });
        //    }
        //    catch (Exception)
        //    {

        //        return Ok(new { status = false, message = "Something went wrong" });

        //    }


        //}

        [HttpGet]
        public IActionResult GetChildInfo(int id)
        {
            try
            {
                var Child = _context.Child.Select(e => new
                {
                    e.ChildId,
                    e.ChildImage,
                    e.ChildName,
                    AgeCategoryAr = e.AgeCategory.AgeCategoryTlAr,
                    AgeCategoryEn = e.AgeCategory.AgeCategoryTlEn,
                    e.AgeCategoryId,
                    e.ParentId,
                    e.ChildEmail,
                    e.ChildPhone,
                    ParentName = e.Parent.ParentName,
                    ParentPhone = e.Parent.ParentPhone,
                    ParentEmail = e.Parent.ParentEmail,
                    ParentAddress = e.Parent.ParentAddress
                }).FirstOrDefault(e => e.ChildId == id);
                return Ok(new { Status = true, Child = Child });
            }
            catch (Exception ex)
            {

                return Ok(new { status = false, message = ex.Message });

            }

        }

        [HttpPost]
        public async Task<IActionResult> ForgetPasswordAsync(string Email)
        {
            try
            {
                if (Email != null)
                {
                    var user = await _userManager.FindByEmailAsync(Email);
                    if (user == null)
                    {
                        // Don't reveal that the user does not exist or is not confirmed
                        return Ok(new { status = false, message = "User isn't Exist" });
                        //_toastNotififcation.AddSuccessToastMessage(" Please check your email to reset your password. ");
                    }

                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var webRoot = _hostEnvironment.WebRootPath;

                    var pathToFile = _hostEnvironment.WebRootPath
                           + Path.DirectorySeparatorChar.ToString()
                           + "Templates"
                           + Path.DirectorySeparatorChar.ToString()
                           + "EmailTemplate"
                           + Path.DirectorySeparatorChar.ToString()
                           + "ResetPassword.html";
                    var builder = new BodyBuilder();
                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {

                        builder.HtmlBody = SourceReader.ReadToEnd();

                    }
                    string messageBody = string.Format(builder.HtmlBody,
                     user.UserName,
                     code,
                       string.Format("{0:dddd, d MMMM yyyy}", DateTime.Now)
                       );
                    await _emailSender.SendEmailAsync(
                        user.Email,
                        "Reset Password",
                       messageBody);

                    return Ok(new { status = true, message = "Please check your email to reset your password." });

                }
                else
                {
                    return Ok(new { status = false, message = "Must send Email" });
                }
            }
            catch (Exception ex)
            {

                return Ok(new { status = false, message = ex.Message });

            }

        }
        [HttpDelete]
        public IActionResult DeleteUserAccount(int userId)
        {
            var user = _userManager.Users.Where(e => e.EntityId == userId).FirstOrDefault();
            try
            {
                if (user == null)
                {
                    return Ok(new { Status = false, Message = "User Not Found" });
                }

                _db.Users.Remove(user);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                return Ok(new { Status = false, Message = e.Message });

            }
            return Ok(new { Status = true, Message = "User Account Deleted Successfully" });
        }

        [HttpGet]
        public IActionResult GetAllLanguages()
        {
            try
            {
                var Languages = _context.Languages.Select(e => new
                {
                    e.LanguageId,
                    e.Title,

                }).ToList();
                return Ok(new { Status = true, Languages = Languages });
            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, message = ex.Message });

            }

        }

        [HttpGet]
        public IActionResult GetLanguageById(int langId)
        {
            try
            {
                var Language = _context.Languages.Where(e => e.LanguageId == langId).FirstOrDefault();
                if (Language == null)
                {
                    return Ok(new { Status = false, message = "Language Obj Not Found" });
                }
                return Ok(new { Status = true, Languages = Language });
            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, message = ex.Message });

            }

        }
        [HttpGet]
        public IActionResult GetNurseryShareUrl(int nurseryId)
        {
            try
            {
                var NurseryMember = _context.NurseryImage.Where(e => e.NurseryId == nurseryId).FirstOrDefault();
                if (NurseryMember == null)
                {
                    return Ok(new { Status = false, message = "Nursery Not Found" });
                }
                //http://techetime-001-site2.atempurl.com
                return Ok(new { Status = true, Url = $"http://alhadhanah.com/nurseryDetails?id={NurseryMember.NurseryId}" });
            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, message = ex.Message });

            }

        }
        [HttpGet]
        #region Nursery V2
        #region NurserAdminArea
        //public IActionResult GetAllNurseryPlanType()
        //{
        //    try
        //    {
        //        var NurseryPlanTypes = _context.NurseryPlanTypes.Select(e => new
        //        {
        //            NurseryPlanTypeId = e.NurseryPlanTypeId,
        //            NurseryPlanTypeTitleAr = e.NurseryPlanTypeTitleAr,
        //            NurseryPlanTypeTitleEn = e.NurseryPlanTypeTitleEn,

        //        }).ToList();
        //        return Ok(new { Status = true, NurseryPlanTypes = NurseryPlanTypes });
        //    }
        //    catch (Exception ex)
        //    {

        //        return Ok(new { Status = false, message = ex.Message });

        //    }

        //}
        //[HttpGet]
        //public IActionResult GetAllDocumentTypes()
        //{
        //    try
        //    {
        //        var DocumentTypes = _context.DocumentTypes.Select(e => new
        //        {
        //            DocumentTypeId = e.DocumentTypeId,
        //            DocumentTypeEn = e.DocumentTypeEn,
        //            DocumentTypeAr = e.DocumentTypeAr,

        //        }).ToList();
        //        return Ok(new { Status = true, DocumentTypes = DocumentTypes });
        //    }
        //    catch (Exception ex)
        //    {

        //        return Ok(new { Status = false, message = ex.Message });

        //    }

        //}
        //[HttpPost]
        //public IActionResult AddNurseryPlan([FromBody] NurseryPlanVM nurseryPlanVM)
        //{

        //    try
        //    {
        //        var Nursey = _context.NurseryMember.Where(e => e.NurseryMemberId == nurseryPlanVM.NurseryMemberId).FirstOrDefault();
        //        if (Nursey == null)
        //        {
        //            return Ok(new { Status = false, Message = "Nursey Not Found" });

        //        }
        //        var NurseryPlanType = _context.NurseryPlanTypes.Where(e => e.NurseryPlanTypeId == nurseryPlanVM.NurseryPlanTypeId).FirstOrDefault();
        //        if (NurseryPlanType == null)
        //        {
        //            return Ok(new { Status = false, Message = "Nursery Plan Type Not Found" });
        //        }
        //        var NurseryPlan = new NurseryPlan()
        //        {
        //            IsActive = nurseryPlanVM.IsActive,
        //            PlanTitleAr = nurseryPlanVM.PlanTitleAr,
        //            PlanTitleEn = nurseryPlanVM.PlanTitleEn,
        //            Price = nurseryPlanVM.Price,
        //            NurseryMemberId = nurseryPlanVM.NurseryMemberId,
        //            NurseryPlanTypeId = nurseryPlanVM.NurseryPlanTypeId,
        //        };
        //        _context.NurseryPlans.Add(NurseryPlan);
        //        _context.SaveChanges();
        //        return Ok(new { Status = true, message = "Nursery Plan Added Successfully" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { Status = false, message = ex.Message });
        //    }
        //}
        //[HttpPut]
        //public IActionResult EditNurseryPlan([FromBody] EditNurseryPlanVM editNurseryPlanVM)
        //{

        //    try
        //    {

        //        var NurseryPlanType = _context.NurseryPlanTypes.Where(e => e.NurseryPlanTypeId == editNurseryPlanVM.NurseryPlanTypeId).FirstOrDefault();
        //        if (NurseryPlanType == null)
        //        {
        //            return Ok(new { Status = false, Message = "Nursery Plan Type Not Found" });
        //        }
        //        var NurseryPlanObj = _context.NurseryPlans.Where(e => e.NurseryPlanId == editNurseryPlanVM.NurseryPlanId).FirstOrDefault();
        //        if (NurseryPlanObj == null)
        //        {
        //            return Ok(new { Status = false, Message = "Nursery Plan Not Found" });

        //        }
        //        NurseryPlanObj.IsActive = editNurseryPlanVM.IsActive;
        //        NurseryPlanObj.NurseryPlanTypeId = editNurseryPlanVM.NurseryPlanTypeId;
        //        NurseryPlanObj.PlanTitleAr = editNurseryPlanVM.PlanTitleAr;
        //        NurseryPlanObj.PlanTitleEn = editNurseryPlanVM.PlanTitleEn;
        //        NurseryPlanObj.Price = editNurseryPlanVM.Price;
        //        _context.Attach(NurseryPlanObj).State = EntityState.Modified;
        //        _context.SaveChangesAsync();

        //        return Ok(new { Status = true, message = "Nursery Plan Edited Successfully" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { Status = false, message = ex.Message });
        //    }
        //}
        //[HttpDelete]
        //public IActionResult DeleteNurseryPlan(int NurseryId, int PlanId)
        //{
        //    var NurseryPlan = _context.NurseryPlans.Where(e => e.NurseryPlanId == PlanId && e.NurseryMemberId == NurseryId).FirstOrDefault();
        //    try
        //    {
        //        if (NurseryPlan == null)
        //        {
        //            return Ok(new { Status = false, Message = "Nursery Plan Not Found" });
        //        }
        //        _context.NurseryPlans.Remove(NurseryPlan);
        //        _context.SaveChanges();
        //        return Ok(new { Status = true, Message = "Nursery Plan Deleted Successfully" });
        //    }
        //    catch (Exception e)
        //    {
        //        return Ok(new { Status = false, Message = e.Message });

        //    }
        //}
        //[HttpGet]
        //public IActionResult GetAllNurseryPlanByNurseryId(int NurseryId)
        //{
        //    try
        //    {
        //        var ActiveNurseryPlan = _context.NurseryPlans.Where(e => e.NurseryMemberId == NurseryId && e.IsActive == true).Select(e => new
        //        {
        //            NurseryPlanId = e.NurseryPlanId,
        //            NurseryMemberId = e.NurseryMemberId,
        //            NurseryPlanTypeId = e.NurseryPlanTypeId,
        //            PlanTitleAr = e.PlanTitleAr,
        //            PlanTitleEn = e.PlanTitleEn,
        //            Price = e.Price,

        //        }).ToList();
        //        return Ok(new { Status = true, ActiveNurseryPlan = ActiveNurseryPlan });
        //    }
        //    catch (Exception ex)
        //    {

        //        return Ok(new { Status = false, message = ex.Message });

        //    }

        //}
        //[HttpPost]
        //public IActionResult AddAcademyYear([FromBody] AcademyYearVM academyYearVM)
        //{

        //    try
        //    {
        //        var Nursey = _context.NurseryMember.Where(e => e.NurseryMemberId == academyYearVM.NurseryMemberId).FirstOrDefault();
        //        if (Nursey == null)
        //        {
        //            return Ok(new { Status = false, Message = "Nursey Not Found" });

        //        }

        //        var academyYear = new AcademyYear()
        //        {
        //            IsActive = academyYearVM.IsActive,
        //            TitleAr = academyYearVM.TitleAr,
        //            TitleEn = academyYearVM.TitleEn,
        //            StartDate = academyYearVM.StartDate,
        //            EndDate = academyYearVM.EndDate,
        //            NurseryMemberId = academyYearVM.NurseryMemberId,

        //        };
        //        _context.AcademyYears.Add(academyYear);
        //        _context.SaveChanges();
        //        return Ok(new { Status = true, message = "Academy Year Added Successfully" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { Status = false, message = ex.Message });
        //    }
        //}
        //[HttpPut]
        //public IActionResult EditAcademyYear([FromBody] EditAcademyYearVM editAcademyYearVM)
        //{

        //    try
        //    {


        //        var AcademyYearObj = _context.AcademyYears.Where(e => e.AcademyYearId == editAcademyYearVM.AcademyYearId).FirstOrDefault();
        //        if (AcademyYearObj == null)
        //        {
        //            return Ok(new { Status = false, Message = "Academy Year Not Found" });

        //        }
        //        AcademyYearObj.IsActive = editAcademyYearVM.IsActive;
        //        AcademyYearObj.EndDate = editAcademyYearVM.EndDate;
        //        AcademyYearObj.StartDate = editAcademyYearVM.StartDate;
        //        AcademyYearObj.TitleAr = editAcademyYearVM.TitleAr;
        //        AcademyYearObj.TitleEn = editAcademyYearVM.TitleEn;
        //        _context.Attach(AcademyYearObj).State = EntityState.Modified;
        //        _context.SaveChangesAsync();
        //        return Ok(new { Status = true, message = "Academy Year Edited Successfully" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { Status = false, message = ex.Message });
        //    }
        //}
        //[HttpDelete]
        //public IActionResult DeleteAcademyYear(int AcademyYearId, int NurseryId)
        //{
        //    try
        //    {

        //        var AcademyYearObj = _context.AcademyYears.Where(e => e.AcademyYearId == AcademyYearId && e.NurseryMemberId == NurseryId).FirstOrDefault();
        //        if (AcademyYearObj == null)
        //        {
        //            return Ok(new { Status = false, Message = "Academy Year Not Found" });
        //        }
        //        _context.AcademyYears.Remove(AcademyYearObj);
        //        _context.SaveChanges();
        //        return Ok(new { Status = true, Message = "Academy Year Deleted Successfully" });
        //    }
        //    catch (Exception e)
        //    {
        //        return Ok(new { Status = false, Message = e.Message });

        //    }
        //}
        //[HttpGet]
        //public IActionResult GetAllAcademyYearByNurseryId(int NurseryId)
        //{
        //    try
        //    {
        //        var ActiveAcademyYear = _context.AcademyYears.Where(e => e.NurseryMemberId == NurseryId && e.IsActive == true).Select(e => new
        //        {
        //            AcademyYearId = e.AcademyYearId,
        //            TitleAr = e.TitleAr,
        //            TitleEn = e.TitleEn,
        //            NurseryMemberId = e.NurseryMemberId,
        //            StartDate = e.StartDate,
        //            EndDate = e.EndDate,

        //        }).ToList();
        //        return Ok(new { Status = true, ActiveAcademyYear = ActiveAcademyYear });
        //    }
        //    catch (Exception ex)
        //    {

        //        return Ok(new { Status = false, Message = ex.Message });

        //    }

        //}
        //[HttpPost]
        //public IActionResult AddGroup([FromBody] GroupVM groupVM, IFormFile groupImage)
        //{

        //    try
        //    {
        //        var Nursey = _context.NurseryMember.Where(e => e.NurseryMemberId == groupVM.NurseryMemberId).FirstOrDefault();
        //        if (Nursey == null)
        //        {
        //            return Ok(new { Status = false, Message = "Nursey Not Found" });

        //        }
        //        var groupObj = new Group()
        //        {
        //            NurseryMemberId = groupVM.NurseryMemberId,
        //            NameAr = groupVM.NameAr,
        //            NameEn = groupVM.NameEn,
        //            Description = groupVM.Description,
        //            CreatedDate=DateTime.Now,

        //        };
        //        if (groupImage!= null)
        //        {
        //            string folder = "Images/Group/";
        //            groupObj.Pic = UploadImage(folder, groupImage);
                   
        //        }

                
        //        _context.Groups.Add(groupObj);
        //        _context.SaveChanges();
        //        return Ok(new { Status = true, Message = "Group Added Successfully" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { Status = false, Message = ex.Message });
        //    }
        //}
        //[HttpPut]
        //public IActionResult EditGroup([FromBody] EditGroupVM editGroupVM, IFormFile groupImage)
        //{

        //    try
        //    {


        //        var groupObj = _context.Groups.Where(e => e.GroupId == editGroupVM.GroupId).FirstOrDefault();
        //        if (groupObj == null)
        //        {
        //            return Ok(new { Status = false, Message = "Group Not Found" });

        //        }
        //        groupObj.NameAr = editGroupVM.NameAr;
        //        groupObj.NameEn = editGroupVM.NameEn;
        //        groupObj.Description = editGroupVM.Description;
        //        if (groupImage != null)
        //        {
        //            string folder = "Images/Group/";
        //            groupObj.Pic = UploadImage(folder, groupImage);

        //        }
        //        _context.Attach(groupObj).State = EntityState.Modified;
        //        _context.SaveChangesAsync();
        //        return Ok(new { Status = true, Message = "Group Edited Successfully" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { Status = false, Message = ex.Message });
        //    }
        //}
        //[HttpDelete]
        //public IActionResult DeleteGroup(int GroupId, int NurseryId)
        //{
        //    try
        //    {

        //        var groupObj = _context.Groups.Where(e => e.GroupId == GroupId && e.NurseryMemberId == NurseryId).FirstOrDefault();
        //        if (groupObj == null)
        //        {
        //            return Ok(new { Status = false, Message = "Group Not Found" });
        //        }
        //        _context.Groups.Remove(groupObj);
        //        _context.SaveChanges();
        //        return Ok(new { Status = true, Message = "Group Deleted Successfully" });
        //    }
        //    catch (Exception e)
        //    {
        //        return Ok(new { Status = false, Message = e.Message });

        //    }
        //}
        //[HttpGet]
        //public IActionResult GetAllGroupsByNurseryId(int NurseryId)
        //{
        //    try
        //    {
        //        var GroupList = _context.Groups.Include(e=>e.StudentGroups).Where(e => e.NurseryMemberId == NurseryId).Select(e => new
        //        {
        //            NameEn = e.NameEn,
        //            NameAr = e.NameAr,
        //            Description = e.Description,
        //            NurseryMemberId = e.NurseryMemberId,
        //            Pic = e.Pic,
        //            StudentGroups = e.StudentGroups,

        //        }).ToList();
        //        return Ok(new { Status = true, GroupList = GroupList });
        //    }
        //    catch (Exception ex)
        //    {

        //        return Ok(new { Status = false, Message = ex.Message });

        //    }

        //}
        //[HttpPost]
        //public IActionResult AddStudent([FromBody] StudentVM studentVM,IFormFile StudentImage)
        //{

        //    try
        //    {
        //        var Nursey = _context.NurseryMember.Where(e => e.NurseryMemberId == studentVM.NurseryMemberId).FirstOrDefault();
        //        if (Nursey == null)
        //        {
        //            return Ok(new { Status = false, Message = "Nursey Not Found" });

        //        }
        //        var NurseryPlan = _context.NurseryPlans.Where(e => e.NurseryPlanId == studentVM.NurseryPlanId).FirstOrDefault();
        //        if (NurseryPlan == null)
        //        {
        //            return Ok(new { Status = false, Message = "Nursery Plan Not Found" });
        //        }
        //        var StudentObj = new Student()
        //        {
        //            NameAr = studentVM.NameAr,
        //            NameEn = studentVM.NameEn,
        //            Birthdate = studentVM.Birthdate,
        //            Age = studentVM.Age,
        //            NurseryMemberId = studentVM.NurseryMemberId,
        //            NurseryPlanId = studentVM.NurseryPlanId,
                    
        //        };
        //        if (StudentImage != null)
        //        {
        //            string folder = "Images/Student/";
        //            StudentObj.Pic = UploadImage(folder, StudentImage);

        //        }
                
        //        _context.Students.Add(StudentObj);
        //        _context.SaveChanges();
        //        //////////////////////Fattorahhhhhhhhhhhhhhhhhhhhhhhhhhhh///
        //        return Ok(new { Status = true, message = "Student Added Successfully" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { Status = false, message = ex.Message });
        //    }
        //}
        //[HttpPost]
        //public IActionResult AddAttachmentForStudent([FromBody] StudentAttachmentVM studentAttachmentVM, IFormFile file)
        //{

        //    try
        //    {
        //        var Student = _context.StudentAttachments.Where(e => e.StudentId == studentAttachmentVM.StudentId).FirstOrDefault();
        //        if (Student == null)
        //        {
        //            return Ok(new { Status = false, Message = "Student Not Found" });

        //        }
        //        var DocumentType = _context.DocumentTypes.Where(e => e.DocumentTypeId == studentAttachmentVM.DocumentTypeId).FirstOrDefault();
        //        if (DocumentType == null)
        //        {
        //            return Ok(new { Status = false, Message = "Document Type Not Found" });

        //        }
        //        if (file == null)
        //        {
        //            return Ok(new { Status = false, Message = "Please...Upload File" });

        //        }
        //        var studentAttachmentObj = new StudentAttachment()
        //        {
        //            DocumentTypeId = studentAttachmentVM.DocumentTypeId,
        //            StudentId = studentAttachmentVM.StudentId,
        //            Description = studentAttachmentVM.Description,
        //            AttachmentDate = DateTime.Now,
                   

        //        };
        //        if (file != null)
        //        {
        //            string folder = "Images/StudAttachment/";
        //            studentAttachmentObj.FileUrl = UploadImage(folder, file);

        //        }


        //        _context.StudentAttachments.Add(studentAttachmentObj);
        //        _context.SaveChanges();
        //        return Ok(new { Status = true, Message = "Attachment Added Successfully" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { Status = false, Message = ex.Message });
        //    }
        //}
        //[HttpDelete]
        //public IActionResult DeleteAttachmentForStudent(int AttachmentId, int StudentId)
        //{
        //    try
        //    {

        //        var AttachmentObj = _context.StudentAttachments.Where(e => e.StudentAttachmentId == AttachmentId && e.StudentId == StudentId).FirstOrDefault();
        //        if (AttachmentObj == null)
        //        {
        //            return Ok(new { Status = false, Message = "Attachment Not Found" });
        //        }
        //        _context.StudentAttachments.Remove(AttachmentObj);
        //        _context.SaveChanges();
        //        return Ok(new { Status = true, Message = "Attachment Deleted Successfully" });
        //    }
        //    catch (Exception e)
        //    {
        //        return Ok(new { Status = false, Message = e.Message });

        //    }
        //}
        //[HttpPost]
        //public IActionResult AddStudentToGroup([FromBody] StudentGroupVM studentGroupVM)
        //{

        //    try
        //    {
        //        var StudentObj = _context.Students.Where(e => e.StudentId == studentGroupVM.StudentId).FirstOrDefault();
        //        if (StudentObj == null)
        //        {
        //            return Ok(new { Status = false, Message = "Student Not Found" });

        //        }
        //        var GroupObj = _context.Groups.Where(e => e.GroupId == studentGroupVM.GroupId).FirstOrDefault();
        //        if (GroupObj == null)
        //        {
        //            return Ok(new { Status = false, Message = "Group Not Found" });
        //        }
        //        var StudentExistInGroup = _context.StudentGroups.Where(e => e.GroupId == studentGroupVM.GroupId&&e.StudentId==studentGroupVM.StudentId).FirstOrDefault();
        //        if (StudentExistInGroup != null)
        //        {
        //            return Ok(new { Status = false, Message = "Student Aleady In This Group" });
        //        }

        //        var studentGroup = new StudentGroup()
        //        {
        //            GroupId = studentGroupVM.GroupId,
        //            StudentId = studentGroupVM.StudentId,
        //            StartDate = DateTime.Now,
                    

        //        };
                
        //        _context.StudentGroups.Add(studentGroup);
        //        _context.SaveChanges();
              
        //        return Ok(new { Status = true, message = "Student Added Successfully" });

        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { Status = false, message = ex.Message });
        //    }
        //}
        //[HttpDelete]
        //public IActionResult DeleteStudentFromGroup(int studentId, int GroupId)
        //{
        //    try
        //    {

        //        var StudentExistInGroupObj = _context.StudentGroups.Where(e => e.StudentId == studentId && e.GroupId == GroupId).FirstOrDefault();
        //        if (StudentExistInGroupObj == null)
        //        {
        //            return Ok(new { Status = false, Message = "Student Not Exist In This Group" });
        //        }
        //        _context.StudentGroups.Remove(StudentExistInGroupObj);
        //        _context.SaveChanges();
        //        return Ok(new { Status = true, Message = "Student Remove Successfully From This Group" });
        //    }
        //    catch (Exception e)
        //    {
        //        return Ok(new { Status = false, Message = e.Message });

        //    }
        //}
        //[HttpGet]
        //public IActionResult GetAllStudentHaveNotGroupByNurseryId(int NurseryId)
        //{
        //    try
        //    {


        //        var StudentWithOutGroups = _context.Students.Include(e=>e.StudentGroups).Where(e => e.StudentGroups.Count == 0).ToList();
        //        return Ok(new { Status = true, StudentWithOutGroups = StudentWithOutGroups });
        //    }
        //    catch (Exception ex)
        //    {

        //        return Ok(new { Status = false, Message = ex.Message });

        //    }

        //}
        #endregion
        #endregion
        [ApiExplorerSettings(IgnoreApi = true)]
        private string UploadImage(string folderPath, IFormFile file)
        {

            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_hostEnvironment.WebRootPath, folderPath);

            file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return folderPath;
        }
    }
}


