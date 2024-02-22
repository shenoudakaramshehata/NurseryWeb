using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nursery.Entities
{
    public class RegistrationModel
    {
        
        public int NurseryMemberId { get; set; }
        public string NurseryTlAr { get; set; }
        public string NurseryTlEn { get; set; }
        public string NurseryDescAr { get; set; }
        public string NurseryDescEn { get; set; }
        public string Email { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string WorkTimeFrom { get; set; }
        public string WorkTimeTo { get; set; }
        public int? AgeCategoryId { get; set; }
        public bool? TransportationService { get; set; }
        public bool? SpecialNeeds { get; set; }
        public int? Language { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public int? AreaId { get; set; }
        public string Address { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Logo { get; set; }
        public string Banner { get; set; }
        public string Password { get; set; }
        public int AvailableSeats { get; set; }
        public int PaymentMethodId { get; set; }
        public int PlanId { get; set; }
        public int PlanTypeId { get; set; }
        public List<string> NurseryImage{ get; set; }
        public List<int> StudyLanguages{ get; set; }

    }
}
