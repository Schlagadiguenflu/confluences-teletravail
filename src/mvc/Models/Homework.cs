using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class Homework
    {
        public Homework()
        {
            HomeworkStudents = new HashSet<HomeworkStudent>();
        }

        [Key]
        public int HomeworkId { get; set; }
        [DisplayName("Date")]
        public DateTime HomeworkDate { get; set; }
        [DisplayName("Nom de la théorie")]
        public string ClassName { get; set; }
        [DisplayName("Lien vers la théorie")]
        public string ClassLink { get; set; }
        [DisplayName("Nom de l'exercice")]
        public string ExerciceName { get; set; }
        [DisplayName("Lien vers l'exercice")]
        public string ExerciceLink { get; set; }
        [DisplayName("Type")]
        public int HomeworkTypeId { get; set; }
        [DisplayName("Classe")]
        public int SessionId { get; set; }
        [DisplayName("Formateur-trice")]
        [Required]
        public string TeacherId { get; set; }
        [DisplayName("Est un devoir supplémentaire")]
        public bool IsHomeworkAdditionnal { get; set; }

        [DisplayName("Type")]
        [ForeignKey(nameof(HomeworkTypeId))]
        [InverseProperty("Homework")]
        public virtual HomeworkType HomeworkType { get; set; }
        [DisplayName("Classe")]
        [ForeignKey(nameof(SessionId))]
        [InverseProperty("Homework")]
        public virtual Session Session { get; set; }
        [DisplayName("Formateur-trice")]
        [ForeignKey(nameof(TeacherId))]
        [InverseProperty(nameof(AspNetUser.Homework))]
        public virtual AspNetUser Teacher { get; set; }
        [InverseProperty(nameof(HomeworkStudent.Homework))]
        public virtual ICollection<HomeworkStudent> HomeworkStudents { get; set; }
    }
}
