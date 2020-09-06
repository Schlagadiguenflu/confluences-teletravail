using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class HomeworkStudent
    {
        [Key]
        public int HomeworkStudentId { get; set; }
        public DateTime HomeworkStudentDate { get; set; }
        [Required]
        public string HomeworkFile { get; set; }
        public string HomeworkFileTeacher { get; set; }
        public int HomeworkId { get; set; }
        [Required]
        public string StudentId { get; set; }

        [ForeignKey(nameof(HomeworkId))]
        [InverseProperty("HomeworkStudents")]
        public virtual Homework Homework { get; set; }
        [ForeignKey(nameof(StudentId))]
        [InverseProperty(nameof(AspNetUser.HomeworkStudents))]
        public virtual AspNetUser Student { get; set; }
    }
}
