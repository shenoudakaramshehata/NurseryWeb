﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nursery.Models
{
    public partial class ContactUs
    {
        [Key]
        public int ContactId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string Msg { get; set; }
        public DateTime? TransDate { get; set; }
    }
}