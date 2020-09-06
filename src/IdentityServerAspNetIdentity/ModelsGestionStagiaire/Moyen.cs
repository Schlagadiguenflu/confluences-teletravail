using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.ModelsGestionStagiaire
{
    [Table("moyens")]
    public partial class Moyen
    {
        [Column("libelle")]
        [StringLength(20)]
        public string Libelle { get; set; }
        [Column("code")]
        [StringLength(3)]
        public string Code { get; set; }
    }
}
