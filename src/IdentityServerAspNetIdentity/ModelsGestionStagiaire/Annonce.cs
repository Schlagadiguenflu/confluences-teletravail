using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.ModelsGestionStagiaire
{
    [Table("annonce")]
    public partial class Annonce
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("lib-annonce")]
        [StringLength(30)]
        public string LibAnnonce { get; set; }
    }
}
