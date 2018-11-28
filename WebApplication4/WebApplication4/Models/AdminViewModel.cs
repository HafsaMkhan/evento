using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class AdminViewModel
    {
        [DataType(DataType.Text)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is Required")]
        public string username { get; set; }
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is Required")]
        public string password { get; set; }
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is missing!")]
        public string change_password { get; set; }
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is missing!")]
        public string confirm_password { get; set; }

    }
}