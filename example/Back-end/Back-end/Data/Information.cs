using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace Back_end.Data
{
    public class Information
    {
        public int Id { get; set; }
        [Display(Name = "First name")]
        public string Firstname { get; set; }
        [Display(Name = "Last name")]
        public string Lastname { get; set; }
        public string Address { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get;set; }
        [DataType(DataType.DateTime)]
        public DateTime time { get; set; }
    }
}
