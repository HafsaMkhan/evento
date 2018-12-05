using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class AdminResetViewModel
    {
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings =false, ErrorMessage ="Previous Password is required.")]
        public string OldPassword { get; set; }
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "New Password is required.")]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirm Password is required.")]
        [Compare("NewPassword",ErrorMessage = "Password doesnot match!")]
        public string ConfirmPassword { get; set; }
    }
}