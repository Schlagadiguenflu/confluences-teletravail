using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.ModelsGestionStagiaire
{
    [Table("entremetier")]
    public partial class Entremetier
    {
        [Column("ident")]
        public short? Ident { get; set; }
        [Column("codemetier")]
        [StringLength(10)]
        public string Codemetier { get; set; }
    }
}
