using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class TypeStage
    {
        [Key]
        public int TypeStageId { get; set; }
        [Required]
        [StringLength(50)]
        public string Nom { get; set; }
    }
}
