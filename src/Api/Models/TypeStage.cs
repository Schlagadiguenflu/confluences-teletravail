using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class TypeStage
    {
        public TypeStage()
        {
            Stages = new HashSet<Stage>();
        }

        [Key]
        public int TypeStageId { get; set; }
        [Required]
        [StringLength(50)]
        public string Nom { get; set; }

        [InverseProperty(nameof(Stage.TypeStage))]
        public virtual ICollection<Stage> Stages { get; set; }
    }
}
