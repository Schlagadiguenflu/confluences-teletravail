using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class HomeworkV2Student
    {
        [Key]
        public int HomeworkV2StudentId { get; set; }

        [Required]
        public DateTime HomeworkStudentDate { get; set; }

        [Required]
        public string HomeworkFile { get; set; }

        public string HomeworkFileTeacher { get; set; }
        public string HomeworkCommentaryTeacher { get; set; }

        [Required]
        public int ExerciceId { get; set; }
        [ForeignKey(nameof(ExerciceId))]
        public virtual Exercice Exercice { get; set; }

        [Required]
        public string StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual ApplicationUser Student { get; set; }
    }
}
