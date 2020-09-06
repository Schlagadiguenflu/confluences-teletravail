using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class EntrepriseMetier
    {
        [Key]
        public int EntrepriseId { get; set; }
        [Key]
        public int TypeMetierId { get; set; }

        [ForeignKey(nameof(EntrepriseId))]
        [InverseProperty(nameof(Entrepris.EntrepriseMetiers))]
        public virtual Entrepris Entreprise { get; set; }
        [ForeignKey(nameof(TypeMetierId))]
        [InverseProperty("EntrepriseMetiers")]
        public virtual TypeMetier TypeMetier { get; set; }
    }
}
