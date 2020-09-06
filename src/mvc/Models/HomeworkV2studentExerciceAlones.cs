using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.Models
{
    [Table("HomeworkV2StudentExerciceAlones")]
    public partial class HomeworkV2studentExerciceAlones
    {
        [Key]
        [Column("HomeworkV2StudentId")]
        public int HomeworkV2studentId { get; set; }
        public DateTime HomeworkStudentDate { get; set; }
        [Required]
        public string HomeworkFile { get; set; }
        public string HomeworkFileTeacher { get; set; }
        public string HomeworkCommentaryTeacher { get; set; }
        public int ExerciceId { get; set; }
        [Required]
        public string StudentId { get; set; }

        [ForeignKey(nameof(ExerciceId))]
        [InverseProperty(nameof(ExercicesAlone.HomeworkV2studentExerciceAlones))]
        public virtual ExercicesAlone Exercice { get; set; }
        [ForeignKey(nameof(StudentId))]
        [InverseProperty(nameof(AspNetUser.HomeworkV2studentExerciceAlones))]
        public virtual AspNetUser Student { get; set; }
    }
}
