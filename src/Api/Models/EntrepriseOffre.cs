using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class EntrepriseOffre
    {
        [Key]
        public int EntrepriseId { get; set; }
        [Key]
        public int TypeOffreId { get; set; }

        [ForeignKey(nameof(EntrepriseId))]
        [InverseProperty(nameof(Entrepris.EntrepriseOffres))]
        public virtual Entrepris Entreprise { get; set; }
        [ForeignKey(nameof(TypeOffreId))]
        [InverseProperty("EntrepriseOffres")]
        public virtual TypeOffre TypeOffre { get; set; }
    }
}
