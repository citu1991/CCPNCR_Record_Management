using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCPNCR_Record_Management.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }    

        public List<Role> Role_List { get; set; }
    }
}