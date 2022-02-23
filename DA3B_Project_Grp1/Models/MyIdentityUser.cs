using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DA3B_Project_Grp1.Models
{
    public class MyIdentityUser : IdentityUser<Guid>
    {

        [Display(Name = "Display Name")]
        [Required]
        [StringLength(60)]
        [MinLength(3)]
        public string DisplayName { get; set; }

        [Display(Name = "Date Of Birth")]
        [PersonalData]
        [DataType(DataType.Date)]
        //[Column(TypeName = "smalldatetime")]
        public DateTime DateOfBirth { get; set; }


    }
}
