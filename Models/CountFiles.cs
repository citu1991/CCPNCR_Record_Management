using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCPNCR_Record_Management.Models
{
    public class CountFiles
    {
       public int CountTotal_File { get; set; }
      public int CountTotal_Inside { get; set; }
      public int CountTotal_Outside { get; set; }
      public int CountTotal_Pending { get; set; }
        public int CountTotal_Userwise { get; set; }
        public List<CountFiles> List_CountFiles { get; set; }
    }
}