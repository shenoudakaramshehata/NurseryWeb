﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Nursery.Models
{
    public partial class Area
    {
        public Area()
        {
            NurseryMember = new HashSet<NurseryMember>();
        }
        public int AreaId { get; set; }
        public int CityId { get; set; }
        public string AreaTlAr { get; set; }
        public string AreaTlEn { get; set; }
        public bool? AreaIsActive { get; set; }
        public int? AreaOrderIndex { get; set; }
        [JsonIgnore]
        public virtual City City { get; set; }
        [JsonIgnore]
        public virtual ICollection<NurseryMember> NurseryMember { get; set; }
    }
}