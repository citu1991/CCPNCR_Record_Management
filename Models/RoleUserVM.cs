using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCPNCR_Record_Management.Models
{
    public class RoleUserVM
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<RoleUserVM> RoleList { get; set; }
        public List<RoleUserVM> UserList { get; set; }
    }
}