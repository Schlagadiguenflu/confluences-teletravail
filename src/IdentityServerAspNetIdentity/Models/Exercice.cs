using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class Exercice
    {
        [Key]
        public int ExerciceId { get; set; }

        [Required]
        public DateTime ExerciceDate { get; set; }
        public string ExerciceName { get; set; }
        public string ExerciceLink { get; set; }

        public string AudioLink { get; set; }
        public string VideoLink { get; set; }

        public DateTime CorrectionDate { get; set; }
        public string CorrectionLink { get; set; }

        [DisplayName("Est un exercice supplémentaire")]
        public bool IsHomeworkAdditionnal { get; set; }

        [Required]
        public string TeacherId { get; set; }
        [ForeignKey(nameof(TeacherId))]
        public virtual ApplicationUser Teacher { get; set; }

        [Required]
        public int TheoryId { get; set; }
        [ForeignKey(nameof(TheoryId))]
        public virtual Theory Theory { get; set; }

    }
}
