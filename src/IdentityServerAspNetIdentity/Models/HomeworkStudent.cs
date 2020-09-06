using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class HomeworkStudent
    {
        [Key]
        public int HomeworkStudentId { get; set; }

        [Required]
        public DateTime HomeworkStudentDate { get; set; }

        [Required]
        public string HomeworkFile { get; set; }

        public string HomeworkFileTeacher { get; set; }

        [Required]
        public int HomeworkId { get; set; }
        [ForeignKey(nameof(HomeworkId))]
        public virtual Homework Homework { get; set; }

        [Required]
        public string StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual ApplicationUser Student { get; set; }

    }
}
