using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.ModelsGestionStagiaire
{
    [Table("eleves")]
    public partial class Eleve
    {
        [Key]
        [Column("trgeleve")]
        [StringLength(3)]
        public string Trgeleve { get; set; }
        [Required]
        [Column("nomeleve")]
        [StringLength(50)]
        public string Nomeleve { get; set; }
        [Column("prenomeleve")]
        [StringLength(50)]
        public string Prenomeleve { get; set; }
        [Column("codeaffilisation")]
        [StringLength(10)]
        public string Codeaffilisation { get; set; }
        [StringLength(50)]
        public string NomStagiaire { get; set; }
    }
}
