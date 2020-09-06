using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class TypeMoyen
    {
        [Key]
        public int TypeMoyenId { get; set; }

        [StringLength(3)]
        public string Code { get; set; }

        [Required]
        [StringLength(20)]
        public string Libelle { get; set; }
    }
}
