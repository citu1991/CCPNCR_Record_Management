using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCPNCR_Record_Management.Models
{
    public class SignUp
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }       

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$",
        ErrorMessage = "Please Enter Correct Email Address")]
        public string UserEmail { get; set; }



        [StringLength(30, ErrorMessage = "Password must be minimum 6 characters long which contain at least one lowercase letter, one uppercase letter, one numeric digit, and one special character", MinimumLength = 6)]
        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Password)]
        public string UserPwd { get; set; }


        [Required(ErrorMessage = "Required")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Required")]
        public string UserFName { get; set; }



        [Required(ErrorMessage = "Required")]
        public string UserLName { get; set; }



        [Required(ErrorMessage = "Required")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, ErrorMessage = "The Mobile No must contains only 10 Digit without Prefix 0", MinimumLength = 10)]
        public string UserMobile { get; set; }

        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsActive { get; set; }

        public List<SignUp> User_List { get; set; } 

    }
}