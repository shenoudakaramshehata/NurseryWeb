﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Nursery.Models
{
    public partial class NurseryMember
    {
        public NurseryMember()
        {
            NurseryImage = new HashSet<NurseryImage>();
            NurserySubscription = new HashSet<NurserySubscription>();
            Child = new HashSet<Child>();
        }
        public int NurseryMemberId { get; set; }
        public string NurseryTlAr { get; set; }
        public string NurseryTlEn { get; set; }
        public string NurseryDescAr { get; set; }
        public string NurseryDescEn { get; set; }
        public string StudyLanguage { get; set; }
        public string Password { get; set; }
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
        public int AvailableSeats { get; set; }
        [System.ComponentModel.DefaultValue(true)]
        public bool IsSlider { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool IsActive { get; set; }
        public virtual AgeCategory AgeCategory { get; set; }
        public virtual Area Area { get; set; }
        public virtual ICollection<NurseryImage> NurseryImage { get; set; }
        public virtual ICollection<NurserySubscription> NurserySubscription { get; set; }
        public virtual ICollection<MacsAccount> MacsAccounts { get; set; }
        public virtual ICollection<NurseryStudyLanguage> NurseryStudyLanguages { get; set; }
        public virtual ICollection<Child> Child { get; set; }
    }
}
