using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class TypeEntreprise
    {
        [Key]
        public int TypeEntrepriseId { get; set; }

        [Required]
        [StringLength(50)]
        public string Nom { get; set; }
    }
}
