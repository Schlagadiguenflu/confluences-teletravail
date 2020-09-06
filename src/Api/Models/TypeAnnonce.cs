using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class TypeAnnonce
    {
        public TypeAnnonce()
        {
            Stages = new HashSet<Stage>();
        }

        [Key]
        public int TypeAnnonceId { get; set; }
        [StringLength(30)]
        public string Libelle { get; set; }

        [InverseProperty(nameof(Stage.TypeAnnonce))]
        public virtual ICollection<Stage> Stages { get; set; }
    }
}
