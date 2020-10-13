using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class EntrepriseDomaine
    {
        public int EntrepriseId { get; set; }
        [ForeignKey(nameof(EntrepriseId))]
        public virtual Entreprise Entreprise { get; set; }

        public int TypeDomaineId { get; set; }
        [ForeignKey(nameof(TypeDomaineId))]
        public virtual TypeDomaine TypeDomaine { get; set; }
    }
}
