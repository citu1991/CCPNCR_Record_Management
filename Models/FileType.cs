using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCPNCR_Record_Management.Models
{
    public class FileType
    {
        public int FileTypeId { get; set; }
        public string FileTypeName { get; set; }

        public List<FileType> ListFileType { get; set; }
    }
}