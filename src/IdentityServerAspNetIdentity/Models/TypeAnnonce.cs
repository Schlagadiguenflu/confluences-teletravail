using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class TypeAnnonce
    {
        [Key]
        public int TypeAnnonceId { get; set; }

        [StringLength(30)]
        public string Libelle { get; set; }
    }
}
