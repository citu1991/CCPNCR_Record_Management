
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace CCPNCR_Record_Management.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Required")]   
        public string UserName { get; set; }


        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


       
    }
}