using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class Entrepris
    {
        public Entrepris()
        {
            Contacts = new HashSet<Contact>();
            EntrepriseMetiers = new HashSet<EntrepriseMetier>();
            EntrepriseOffres = new HashSet<EntrepriseOffre>();
            Stages = new HashSet<Stage>();
        }

        [Key]
        public int EntrepriseId { get; set; }
        [Required]
        [StringLength(50)]
        public string Nom { get; set; }
        [Required]
        [StringLength(50)]
        public string Ville { get; set; }
        [StringLength(13)]
        public string TelFix { get; set; }
        [StringLength(13)]
        public string TelNatel { get; set; }
        [StringLength(50)]
        public string Adr1 { get; set; }
        [StringLength(50)]
        public string Adr2 { get; set; }
        [StringLength(4)]
        public string CodePostal { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(10000)]
        public string Remarque { get; set; }
        public DateTime? DateCreation { get; set; }
        public int? TypeEntrepriseId { get; set; }
        public int? TypeDomaineId { get; set; }
        public int? TypeMoyenId { get; set; }
        public DateTime? DateDernierContact { get; set; }
        public string CreateurId { get; set; }
        public string FormateurIdDernierContact { get; set; }
        public string StagiaireIdDernierContact { get; set; }

        [ForeignKey(nameof(CreateurId))]
        [InverseProperty(nameof(AspNetUser.EntreprisCreateurs))]
        public virtual AspNetUser Createur { get; set; }
        [ForeignKey(nameof(FormateurIdDernierContact))]
        [InverseProperty(nameof(AspNetUser.EntreprisFormateurIdDernierContactNavigations))]
        public virtual AspNetUser FormateurIdDernierContactNavigation { get; set; }
        [ForeignKey(nameof(StagiaireIdDernierContact))]
        [InverseProperty(nameof(AspNetUser.EntreprisStagiaireIdDernierContactNavigations))]
        public virtual AspNetUser StagiaireIdDernierContactNavigation { get; set; }
        [ForeignKey(nameof(TypeDomaineId))]
        [InverseProperty("Entrepris")]
        public virtual TypeDomaine TypeDomaine { get; set; }
        [ForeignKey(nameof(TypeEntrepriseId))]
        [InverseProperty(nameof(TypeEntrepris.Entrepris))]
        public virtual TypeEntrepris TypeEntreprise { get; set; }
        [ForeignKey(nameof(TypeMoyenId))]
        [InverseProperty("Entrepris")]
        public virtual TypeMoyen TypeMoyen { get; set; }
        [InverseProperty(nameof(Contact.Entreprise))]
        public virtual ICollection<Contact> Contacts { get; set; }
        [InverseProperty(nameof(EntrepriseMetier.Entreprise))]
        public virtual ICollection<EntrepriseMetier> EntrepriseMetiers { get; set; }
        [InverseProperty(nameof(EntrepriseOffre.Entreprise))]
        public virtual ICollection<EntrepriseOffre> EntrepriseOffres { get; set; }
        [InverseProperty(nameof(Stage.Entreprise))]
        public virtual ICollection<Stage> Stages { get; set; }
    }
}
