using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.ModelsGestionStagiaire
{
    [Table("typentreprise")]
    public partial class Typentreprise
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Required]
        [Column("catentnom")]
        [StringLength(50)]
        public string Catentnom { get; set; }
    }
}
