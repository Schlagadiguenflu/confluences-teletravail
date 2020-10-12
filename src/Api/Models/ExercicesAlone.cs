using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
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
        public DateTime ExerciceDate { get; set; }
        public string ExerciceName { get; set; }
        public string ExerciceLink { get; set; }
        public string AudioLink { get; set; }
        public DateTime CorrectionDate { get; set; }
        public string CorrectionLink { get; set; }
        public bool IsHomeworkAdditionnal { get; set; }
        [Required]
        public string TeacherId { get; set; }
        [Column("HomeworkV2Id")]
        public int HomeworkV2id { get; set; }
        public string VideoLink { get; set; }

        [ForeignKey(nameof(HomeworkV2id))]
        [InverseProperty(nameof(HomeworkV2s.ExercicesAlones))]
        public virtual HomeworkV2s HomeworkV2 { get; set; }
        [ForeignKey(nameof(TeacherId))]
        [InverseProperty(nameof(AspNetUser.ExercicesAlones))]
        public virtual AspNetUser Teacher { get; set; }
        [InverseProperty("Exercice")]
        public virtual ICollection<HomeworkV2studentExerciceAlones> HomeworkV2studentExerciceAlones { get; set; }
    }
}
