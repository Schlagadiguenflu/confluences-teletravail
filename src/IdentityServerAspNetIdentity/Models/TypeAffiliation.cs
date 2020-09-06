using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class TypeAffiliation
    {
        [Key]
        public int TypeAffiliationId { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(50)]
        public string Libelle { get; set; }
    }
}
