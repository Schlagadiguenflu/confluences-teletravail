using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.ModelsGestionStagiaire
{
    [Table("domaine")]
    public partial class Domaine
    {
        [Key]
        [Column("codedomaine")]
        [StringLength(3)]
        public string Codedomaine { get; set; }
        [Column("lbldomaine")]
        [StringLength(60)]
        public string Lbldomaine { get; set; }
    }
}
