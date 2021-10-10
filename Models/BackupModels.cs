using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCPNCR_Record_Management.Models
{
    public class BackupModels
    {
        public int DB_BK_ID {get;set;}
        public string BackUpFileName {get;set;}

        public DateTime? Createdate { get; set; }

        public List<BackupModels> listBackUp { get; set; }

    }
}