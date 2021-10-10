using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCPNCR_Record_Management.Models
{
    public class FileMasterRecordVM
    {      
        public int FileRecordId { get; set; }
        public int FileTypeId { get; set; }
        public int FileVolumeId { get; set; }

        [Required(ErrorMessage = "File Name is Required")]
        public string FileRecordName { get; set; }

        [Required(ErrorMessage ="File No is Required")]
        public string FileRecordNumber { get; set; }

        [Required(ErrorMessage = "Noting First Page is Required")]
        [Range(1,5000)]
        public int NPStart { get; set; }

        [Required(ErrorMessage = "Noting Last Page is Required")]
        [Range(1, 5000)]
        public int  NPEnd { get; set; }

        [Required(ErrorMessage = "Last Date on Noting is Required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime DateonlastNP { get; set; }

        [Required(ErrorMessage = "Noting Signed by is Required")]
        public string SignedByonlastNP { get; set; }

        [Required(ErrorMessage = "Chain Last Page is Required")]
        [Range(1, 5000)]
        public int  CPStart { get; set; }

        [Required(ErrorMessage = "Chain Last Page is Required")]
        [Range(1, 5000)]
        public int CPEnd { get; set; }

        //public FileType FileType { get; set; }
        //public FileVolume FileVolume { get; set; }

        public List<FileType> LIST_FileType { get; set; }
        public List<FileVolume> LIST_FileVolume { get; set; }
        public List<FileRecord> LIST_FileRecord { get; set; }
        public List<FileMasterRecordVM> LIST_FMRecord { get; set; }
    }
}