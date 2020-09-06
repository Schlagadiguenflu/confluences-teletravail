using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.ModelsGestionStagiaire
{
    [Table("typestage")]
    public partial class Typestage
    {
        [Key]
        [Column("codetypestage")]
        public int Codetypestage { get; set; }
        [Required]
        [Column("nomtypestage")]
        [StringLength(50)]
        public string Nomtypestage { get; set; }
    }
}
