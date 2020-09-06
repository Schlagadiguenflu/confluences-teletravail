using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.ModelsGestionStagiaire
{
    [Table("stages")]
    public partial class Stage
    {
        [Required]
        [Column("nomstage")]
        [StringLength(30)]
        public string Nomstage { get; set; }
        [Key]
        [Column("stageid")]
        public int Stageid { get; set; }
        [Column("datefin", TypeName = "date")]
        public DateTime? Datefin { get; set; }
        [Column("datedebur", TypeName = "date")]
        public DateTime Datedebur { get; set; }
        [Column("bilanstage")]
        public string Bilanstage { get; set; }
        [Column("actionsuivi")]
        [StringLength(500)]
        public string Actionsuivi { get; set; }
        [Column("eleve")]
        [StringLength(3)]
        public string Eleve { get; set; }
        [Column("ident")]
        public short? Ident { get; set; }
        [Column("codemetier")]
        [StringLength(10)]
        public string Codemetier { get; set; }
        [Column("remarque")]
        [StringLength(500)]
        public string Remarque { get; set; }
        [Column("typestage")]
        public int? Typestage { get; set; }
        [Column("rapport")]
        public bool? Rapport { get; set; }
        [Column("attestation")]
        public bool? Attestation { get; set; }
        [Column("createur")]
        [StringLength(2)]
        public string Createur { get; set; }
        [Column("repas")]
        public bool? Repas { get; set; }
        [Column("trajets")]
        public bool? Trajets { get; set; }
        [Column("horaire-avant")]
        [StringLength(11)]
        public string HoraireAvant { get; set; }
        [Column("horaire-apres")]
        [StringLength(11)]
        public string HoraireApres { get; set; }
        [Column("statut-stage")]
        public char? StatutStage { get; set; }

        [ForeignKey(nameof(Ident))]
        [InverseProperty(nameof(Entreprise.Stages))]
        public virtual Entreprise IdentNavigation { get; set; }
    }
}
