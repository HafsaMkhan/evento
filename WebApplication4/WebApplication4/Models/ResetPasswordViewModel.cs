using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class ResetPasswordViewModel
    {
        [Required(AllowEmptyStrings =false,ErrorMessage ="New Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage ="At least 6 characters")]
        public string NewPassword { get; set; }
        [Compare("NewPassword", ErrorMessage = "Password doesnot match")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "At least 6 characters")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string ResetCode { get; set; }
    }
}