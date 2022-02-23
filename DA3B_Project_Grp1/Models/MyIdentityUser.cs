using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DA3B_Project_Grp1.Models
{
    public class MyIdentityUser : IdentityUser<Guid>
    {

        [Display(Name = "Display Name")]
        [StringLength(60)]
        [Required]
        [MinLength(3)]
        public string DisplayName { get; set; }

        [Display(Name = "Date Of Birth")]
        [PersonalData]
        [Required]
        [Column(TypeName = "smalldatetime")]
        public DateTime DateOfBirth { get; set; }


    }
}
