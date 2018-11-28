using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class CompanyViewModel
    {
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is Required")]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Contact is Required")]
        public Nullable<int> Contact { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "CEO Name is Required")]
        public string Ceo { get; set; }
        [Required(AllowEmptyStrings =false, ErrorMessage ="Address is missing!")]
        public string Address { get; set; }
        public Nullable<bool> Isapproved { get; set; }
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is Required")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirm Password is missing!")]
        public string C_Password { get; set; }
    }
}