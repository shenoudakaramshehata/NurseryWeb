using System.ComponentModel.DataAnnotations;

namespace Nursery.Entities
{

    public class NurseryRegistrationVm
    {

        public int NurseryMemberId { get; set; }
        [Required(ErrorMessage = "Nursery Name In Arabic is Required")]
        public string NurseryTlAr { get; set; }
        [Required(ErrorMessage = "Nursery Name In English is Required")]
        public string NurseryTlEn { get; set; }
    
        public string NurseryDescAr { get; set; }
        public string NurseryDescEn { get; set; }
        [Required(ErrorMessage = "Email is Required"), RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Not Valid")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone is Required")]
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Fax { get; set; }
        [Required(ErrorMessage = "Mobile is Required")]
        public string Mobile { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        [Required(ErrorMessage = "Work Time From is Required")]
        public string WorkTimeFrom { get; set; }
        [Required(ErrorMessage = "Work Time To is Required")]
        public string WorkTimeTo { get; set; }
        public int? AgeCategoryId { get; set; }
        public bool TransportationService { get; set; }
        public bool SpecialNeeds { get; set; }
        public int? Language { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public int? AreaId { get; set; }
        [Required(ErrorMessage = "Address is Required")]
        public string Address { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Logo { get; set; }
        public string Banner { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Password is Required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is Required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The Password and Confirmation Password do not match")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "No.Available Seats is Required")]
        public int AvailableSeats { get; set; }
        public int PaymentMethodId { get; set; }
        public int PlanId { get; set; }
        public int PlanTypeId { get; set; }

   

    }
}

