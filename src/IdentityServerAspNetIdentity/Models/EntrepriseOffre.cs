using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class EntrepriseOffre
    {
        public int EntrepriseId { get; set; }
        [ForeignKey(nameof(EntrepriseId))]
        public virtual Entreprise Entreprise { get; set; }

        public int TypeOffreId { get; set; }
        [ForeignKey(nameof(TypeOffreId))]
        public virtual TypeOffre TypeOffre { get; set; }
    }
}
