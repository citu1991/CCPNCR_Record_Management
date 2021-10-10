using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCPNCR_Record_Management.Models
{
    public class FileVolume
    {
        public int FileVolumeId { get; set; }
        public string FileVolumeName { get; set; }

        public List<FileVolume> ListFileVolume { get; set; }
    }
}