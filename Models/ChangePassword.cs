using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCPNCR_Record_Management.Models
{
    public class ChangePassword
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}