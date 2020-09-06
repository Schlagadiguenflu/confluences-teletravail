using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.ModelsGestionStagiaire
{
    public partial class Category
    {
        [Key]
        [Column("codeaffiliation")]
        [StringLength(10)]
        public string Codeaffiliation { get; set; }
        [Column("libelleaffiliation")]
        [StringLength(50)]
        public string Libelleaffiliation { get; set; }
    }
}
