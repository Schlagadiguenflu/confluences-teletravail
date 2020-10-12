using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class Exercice
    {
        public Exercice()
        {
            HomeworkV2students = new HashSet<HomeworkV2students>();
        }

        [Key]
        public int ExerciceId { get; set; }
        [DisplayName("Date de l'exercice")]
        public DateTime ExerciceDate { get; set; }
        [DisplayName("Nom de l'exercice")]
        public string ExerciceName { get; set; }
        [DisplayName("Lien de l'exercice")]
        public string ExerciceLink { get; set; }
        [DisplayName("Lien audio")]
        public string AudioLink { get; set; }
        [DisplayName("Lien vidéo")]
        public string VideoLink { get; set; }

        [DisplayName("Date de la correction (Quand cela doit s'afficher)")]
        public DateTime CorrectionDate { get; set; }
        [DisplayName("Lien de la correction")]
        public string CorrectionLink { get; set; }


        [DisplayName("Est un exercice supplémentaire")]
        public bool IsHomeworkAdditionnal { get; set; }
        [DisplayName("Formateur-trice")]
        [Required]
        public string TeacherId { get; set; }
        [DisplayName("Concerne le cours de")]
        public int TheoryId { get; set; }
        [DisplayName("Formateur-trice")]
        [ForeignKey(nameof(TeacherId))]
        [InverseProperty(nameof(AspNetUser.Exercices))]
        public virtual AspNetUser Teacher { get; set; }
        [DisplayName("Concerne le cours de")]
        [ForeignKey(nameof(TheoryId))]
        [InverseProperty("Exercices")]
        public virtual Theory Theory { get; set; }
        [InverseProperty("Exercice")]
        public virtual ICollection<HomeworkV2students> HomeworkV2students { get; set; }
    }
}
