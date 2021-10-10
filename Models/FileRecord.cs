using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCPNCR_Record_Management.Models
{
    public class FileRecord
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int FileIn_OutRecordId { get; set; }
        public int FileRecordId { get; set; }
        public int FileVolumeId { get; set; }
        public int FileTypeId { get; set; }

        public int IsApprovalSent { get; set; }
        public int IsApproved { get; set; }
        public int IsFileInRecord { get; set; }

        public string FileTypeName { get; set; }
        public string FileVolumeName { get; set; }        
        public string FileRecordName { get; set; }        
        public string FileRecordNumber { get; set; }               
        public int NPStart { get; set; }       
        public int NPEnd { get; set; }
        public DateTime DateonlastNP { get; set; }
        public string SignedByonlastNP { get; set; }       
        public int CPStart { get; set; }      
        public int CPEnd { get; set; }

        public string FileCalledByUser { get; set; }
        public string FileCalledDate { get; set; }
        public string ApprovalSentDate { get; set; }

        public FileRecord File_Record { get; set; }
        public List<FileRecord> List_File_Record { get; set; }
    }

    public class FileLastStatus
    {
        public int FileRecordId { get; set; }
        public string Message { get; set; }
        public string FCReturnRemarksByUser { get; set; }
        public string FCApprovalRemarksByUser { get; set; }
        public string FCRejectionRemarksByUser { get; set; }
        public string FCRemarksByRK { get; set; }        
        public string UserFullName { get; set; }
        public string FCApprovalDate { get; set; }
        public string FCRejectionDate { get; set; }

        public string FCReturnDate { get; set; }
        public string Error { get; set; }

        public FileLastStatus File_LastStatus { get; set; }
        public List<FileLastStatus> List_FileLastStatus { get; set; }
    }
}