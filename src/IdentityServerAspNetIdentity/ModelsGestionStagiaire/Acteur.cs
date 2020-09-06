using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.ModelsGestionStagiaire
{
    [Table("acteur")]
    public partial class Acteur
    {
        [Key]
        [Column("ID")]
        [StringLength(2)]
        public string Id { get; set; }
        [Column("LIB")]
        [StringLength(20)]
        public string Lib { get; set; }
    }
}
