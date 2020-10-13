using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class EntrepriseDomaine
    {
        [Key]
        public int EntrepriseId { get; set; }
        [Key]
        public int TypeDomaineId { get; set; }

        [ForeignKey(nameof(EntrepriseId))]
        [InverseProperty(nameof(Entrepris.EntrepriseDomaines))]
        public virtual Entrepris Entreprise { get; set; }
        [ForeignKey(nameof(TypeDomaineId))]
        [InverseProperty("EntrepriseDomaines")]
        public virtual TypeDomaine TypeDomaine { get; set; }
    }
}
