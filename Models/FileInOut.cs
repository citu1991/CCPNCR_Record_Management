using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCPNCR_Record_Management.Models
{
    public class FileInOut
    {
        public int FileRecordId { get; set; }
        public string FileRecordCompleteName { get; set; }

        public string Rkremarks { get; set; }
        
        public int UserId { get; set; }       
        public string UserName { get; set; }
        

        public List<FileInOut> LIST_User { get; set; }
        public List<FileInOut> LIST_FileIn { get; set; }
        public List<FileInOut> LIST_FileOut { get; set; }
        
       
    }
}