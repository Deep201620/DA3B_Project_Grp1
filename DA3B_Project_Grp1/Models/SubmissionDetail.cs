using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DA3B_Project_Grp1.Models
{
    [Table("SubmissionDetails")]
    public class SubmissionDetails
    {
        [Display(Name = "User ID")]
        //[Key]
        [ForeignKey(nameof(SubmissionDetails.User))]
        public Guid UserId { get; set; }

        public MyIdentityUser User { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Submission Id")]
        public int SubmissionId { get; set; }


        [Display(Name = "Project Id")]
        [ForeignKey(nameof(SubmissionDetails.project))]
        public int ProjectId { get; set; }
        public Project project { get; set; }


        [Display(Name = "Submission Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "dd-mm-yyyy hh:mm:ss")]
        public DateTime SubmissionDate { get; set; }

        [Display(Name = "Submission File")]
        [Required]
        [NotMapped]
        public IFormFile SubmissionFile { get; set; }

        [Display(Name = "FileName")]
        public string SubmittedFileName { get; set; }

        [Display(Name = "Approval Status")]
        //[Required]
        public bool ApprovalStatus { get; set; }

        [Display(Name = "Reviewed By")]
        public string ReviewedBy { get; set; }

        public string Remarks { get; set; }
    }
}