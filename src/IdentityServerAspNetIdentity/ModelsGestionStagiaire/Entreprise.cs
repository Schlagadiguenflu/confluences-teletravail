using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.ModelsGestionStagiaire
{
    [Table("entreprise")]
    public partial class Entreprise
    {
        public Entreprise()
        {
            Contacts = new HashSet<Contact>();
            Stages = new HashSet<Stage>();
        }

        [Key]
        [Column("ident")]
        public short Ident { get; set; }
        [Required]
        [Column("noment")]
        [StringLength(50)]
        public string Noment { get; set; }
        [Required]
        [Column("vilent")]
        [StringLength(50)]
        public string Vilent { get; set; }
        [StringLength(13)]
        public string TelFixent { get; set; }
        [Column("telfaxent")]
        [StringLength(13)]
        public string Telfaxent { get; set; }
        [Column("adr1ent")]
        [StringLength(50)]
        public string Adr1ent { get; set; }
        [Column("adr2ent")]
        [StringLength(50)]
        public string Adr2ent { get; set; }
        [Column("cpent")]
        [StringLength(4)]
        public string Cpent { get; set; }
        [Column("emailent")]
        [StringLength(50)]
        public string Emailent { get; set; }
        [Column("catent")]
        public char? Catent { get; set; }
        [Column("remarqent")]
        [StringLength(10000)]
        public string Remarqent { get; set; }
        [Column("datecreatent", TypeName = "date")]
        public DateTime? Datecreatent { get; set; }
        [Column("datedercontactent", TypeName = "date")]
        public DateTime? Datedercontactent { get; set; }
        [StringLength(2)]
        public string CodeCreateur { get; set; }
        [StringLength(2)]
        public string CodederContact { get; set; }
        [Column("codemetier")]
        [StringLength(10)]
        public string Codemetier { get; set; }
        [Column("offre1")]
        public bool? Offre1 { get; set; }
        [Column("offre2")]
        public bool? Offre2 { get; set; }
        [Column("offre3")]
        public bool? Offre3 { get; set; }
        [Column("offre4")]
        public bool? Offre4 { get; set; }
        [Column("codedomaine")]
        [StringLength(3)]
        public string Codedomaine { get; set; }
        [Column("codemetier2")]
        [StringLength(10)]
        public string Codemetier2 { get; set; }
        [Column("codemetier3")]
        [StringLength(10)]
        public string Codemetier3 { get; set; }
        [Column("codemetier4")]
        [StringLength(10)]
        public string Codemetier4 { get; set; }
        [Column("codemetier5")]
        [StringLength(10)]
        public string Codemetier5 { get; set; }
        [Column("pourqui")]
        [StringLength(3)]
        public string Pourqui { get; set; }
        [Column("offre5")]
        public bool? Offre5 { get; set; }
        [Column("moyen")]
        [StringLength(3)]
        public string Moyen { get; set; }

        [InverseProperty(nameof(Contact.IdentNavigation))]
        public virtual ICollection<Contact> Contacts { get; set; }
        [InverseProperty(nameof(Stage.IdentNavigation))]
        public virtual ICollection<Stage> Stages { get; set; }
    }
}
