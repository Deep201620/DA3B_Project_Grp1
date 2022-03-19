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

        [Display(Name = "Gender")]
        [Required]
        [PersonalData]
        public string Gender { get; set; }


        [Display(Name = "Date of Birth")]
        [Required]
        [PersonalData]                                      // for GDPR Complaince
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

    }
}

