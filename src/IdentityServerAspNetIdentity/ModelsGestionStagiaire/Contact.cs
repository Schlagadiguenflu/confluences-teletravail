using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.ModelsGestionStagiaire
{
    [Table("contact")]
    public partial class Contact
    {
        [Key]
        [Column("idcontact")]
        public short Idcontact { get; set; }
        [Required]
        [Column("nomcontact")]
        [StringLength(50)]
        public string Nomcontact { get; set; }
        [Column("prenomcontact")]
        [StringLength(50)]
        public string Prenomcontact { get; set; }
        [Column("emailcontact")]
        [StringLength(32)]
        public string Emailcontact { get; set; }
        [Column("telfixcontact")]
        [StringLength(13)]
        public string Telfixcontact { get; set; }
        [Column("natelcontact")]
        [StringLength(13)]
        public string Natelcontact { get; set; }
        [Column("faxcontact")]
        [StringLength(13)]
        public string Faxcontact { get; set; }
        [Column("datecreatcontact", TypeName = "date")]
        public DateTime? Datecreatcontact { get; set; }
        [Column("codcreatcontact")]
        [StringLength(2)]
        public string Codcreatcontact { get; set; }
        [Column("datemodifcontact", TypeName = "date")]
        public DateTime? Datemodifcontact { get; set; }
        [Column("codmodifcontact")]
        [StringLength(2)]
        public string Codmodifcontact { get; set; }
        [Column("ident")]
        public short Ident { get; set; }

        [ForeignKey(nameof(Ident))]
        [InverseProperty(nameof(Entreprise.Contacts))]
        public virtual Entreprise IdentNavigation { get; set; }
    }
}
