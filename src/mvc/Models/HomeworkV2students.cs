using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    [Table("HomeworkV2Students")]
    public partial class HomeworkV2students
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
        [InverseProperty("HomeworkV2students")]
        public virtual Exercice Exercice { get; set; }
        [ForeignKey(nameof(StudentId))]
        [InverseProperty(nameof(AspNetUser.HomeworkV2students))]
        public virtual AspNetUser Student { get; set; }

        [NotMapped]
        public List<IFormFile> Pictures { get; set; }
    }
}
