using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class Homework
    {
        public Homework()
        {
            HomeworkStudents = new HashSet<HomeworkStudent>();
        }

        [Key]
        public int HomeworkId { get; set; }
        public DateTime HomeworkDate { get; set; }
        public string ClassName { get; set; }
        public string ClassLink { get; set; }
        public string ExerciceName { get; set; }
        public string ExerciceLink { get; set; }
        public int HomeworkTypeId { get; set; }
        public int SessionId { get; set; }
        [Required]
        public string TeacherId { get; set; }
        public bool IsHomeworkAdditionnal { get; set; }

        [ForeignKey(nameof(HomeworkTypeId))]
        [InverseProperty("Homework")]
        public virtual HomeworkType HomeworkType { get; set; }
        [ForeignKey(nameof(SessionId))]
        [InverseProperty("Homework")]
        public virtual Session Session { get; set; }
        [ForeignKey(nameof(TeacherId))]
        [InverseProperty(nameof(AspNetUser.Homework))]
        public virtual AspNetUser Teacher { get; set; }
        [InverseProperty(nameof(HomeworkStudent.Homework))]
        public virtual ICollection<HomeworkStudent> HomeworkStudents { get; set; }
    }
}
