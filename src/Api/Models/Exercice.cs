using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class Exercice
    {
        public Exercice()
        {
            HomeworkV2students = new HashSet<HomeworkV2students>();
        }

        [Key]
        public int ExerciceId { get; set; }
        public DateTime ExerciceDate { get; set; }
        public string ExerciceName { get; set; }
        public string ExerciceLink { get; set; }
        public bool IsHomeworkAdditionnal { get; set; }
        [Required]
        public string TeacherId { get; set; }
        public int TheoryId { get; set; }
        public DateTime CorrectionDate { get; set; }
        public string CorrectionLink { get; set; }
        public string AudioLink { get; set; }
        public string VideoLink { get; set; }

        [ForeignKey(nameof(TeacherId))]
        [InverseProperty(nameof(AspNetUser.Exercices))]
        public virtual AspNetUser Teacher { get; set; }
        [ForeignKey(nameof(TheoryId))]
        [InverseProperty("Exercices")]
        public virtual Theory Theory { get; set; }
        [InverseProperty("Exercice")]
        public virtual ICollection<HomeworkV2students> HomeworkV2students { get; set; }
    }
}
