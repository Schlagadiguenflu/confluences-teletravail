using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class TypeMetier
    {
        public TypeMetier()
        {
            EntrepriseMetiers = new HashSet<EntrepriseMetier>();
            Stages = new HashSet<Stage>();
        }

        [Key]
        public int TypeMetierId { get; set; }
        [StringLength(10)]
        public string Code { get; set; }
        [StringLength(60)]
        public string Libelle { get; set; }

        [InverseProperty(nameof(EntrepriseMetier.TypeMetier))]
        public virtual ICollection<EntrepriseMetier> EntrepriseMetiers { get; set; }
        [InverseProperty(nameof(Stage.TypeMetier))]
        public virtual ICollection<Stage> Stages { get; set; }
    }
}
