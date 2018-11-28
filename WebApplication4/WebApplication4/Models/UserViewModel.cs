using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication4.Models
{
    public class UserViewModel
    {
        [DataType(DataType.EmailAddress)]
        [Required(AllowEmptyStrings =false,ErrorMessage ="Email is Required")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is Required")]
        public string Name { get; set; }
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is Required")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirm Password is Required")]
        public string C_Password { get; set; }
        [DataType(DataType.PhoneNumber)]
        public Nullable<int> Contact { get; set; }
        public string PackageId { get; set; }
    }
}