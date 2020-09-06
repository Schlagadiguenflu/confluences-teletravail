using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class TypeDomaine
    {
        [Key]
        public int TypeDomaineId { get; set; }

        [StringLength(3)]
        public string Code { get; set; }

        [StringLength(60)]
        public string Libelle { get; set; }
    }
}
