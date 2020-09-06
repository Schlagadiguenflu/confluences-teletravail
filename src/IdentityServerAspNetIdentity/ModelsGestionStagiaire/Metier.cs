using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.ModelsGestionStagiaire
{
    [Table("metier")]
    public partial class Metier
    {
        [Key]
        [Column("codemetier")]
        [StringLength(10)]
        public string Codemetier { get; set; }
        [Column("libelemetier")]
        [StringLength(60)]
        public string Libelemetier { get; set; }
    }
}
