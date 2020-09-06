using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.Models
{
    [Table("ExercicesAlone")]
    public partial class ExercicesAlone
    {
        public ExercicesAlone()
        {
            HomeworkV2studentExerciceAlones = new HashSet<HomeworkV2studentExerciceAlones>();
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
        [DisplayName("Date de la correction (Quand cela doit s'afficher)")]
        public DateTime CorrectionDate { get; set; }
        [DisplayName("Lien de la correction")]
        public string CorrectionLink { get; set; }
        [DisplayName("Est un exercice supplémentaire")]
        public bool IsHomeworkAdditionnal { get; set; }
        [Required]
        [DisplayName("Formateur-trice")]
        public string TeacherId { get; set; }
        [Column("HomeworkV2Id")]
        [DisplayName("Devoir")]
        public int HomeworkV2id { get; set; }
        [DisplayName("Devoir")]
        [ForeignKey(nameof(HomeworkV2id))]
        [InverseProperty(nameof(HomeworkV2s.ExercicesAlones))]
        public virtual HomeworkV2s HomeworkV2 { get; set; }
        [DisplayName("Formateur-trice")]
        [ForeignKey(nameof(TeacherId))]
        [InverseProperty(nameof(AspNetUser.ExercicesAlones))]
        public virtual AspNetUser Teacher { get; set; }
        [InverseProperty("Exercice")]
        public virtual ICollection<HomeworkV2studentExerciceAlones> HomeworkV2studentExerciceAlones { get; set; }
    }
}
