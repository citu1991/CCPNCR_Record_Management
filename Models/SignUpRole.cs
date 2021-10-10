using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCPNCR_Record_Management.Models
{
    public class SignUpRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }

        public List<Role> Roles { get; set; }
        public List<SignUp> Signups { get; set; }
        public List<SignUpRole> SignUps_Roles { get; set; }

    }
}