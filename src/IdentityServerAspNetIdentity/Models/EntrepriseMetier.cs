using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class EntrepriseMetier
    {
        public int EntrepriseId { get; set; }
        [ForeignKey(nameof(EntrepriseId))]
        public virtual Entreprise Entreprise { get; set; }

        public int TypeMetierId { get; set; }
        [ForeignKey(nameof(TypeMetierId))]
        public virtual TypeMetier TypeMetier { get; set; }
    }
}
