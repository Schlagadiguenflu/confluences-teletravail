using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class Entreprise
    {
        [Key]
        public int EntrepriseId { get; set; }

        [Required]
        [StringLength(50)]
        public string Nom { get; set; }

        [Required]
        [StringLength(50)]
        public string Ville { get; set; }

        [StringLength(13)]
        public string TelNatel { get; set; }

        [StringLength(13)]
        public string TelFax { get; set; }

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
        [ForeignKey(nameof(TypeEntrepriseId))]
        public virtual TypeEntreprise TypeEntreprise { get; set; }

        public int? TypeDomaineId { get; set; }
        [ForeignKey(nameof(TypeDomaineId))]
        public virtual TypeDomaine TypeDomaine { get; set; }

        public int? TypeMoyenId { get; set; }
        [ForeignKey(nameof(TypeMoyenId))]
        public virtual TypeMoyen TypeMoyen { get; set; }

        public DateTime? DateDernierContact { get; set; }

        // OK
        public string CreateurId { get; set; }
        [ForeignKey(nameof(CreateurId))]
        public virtual ApplicationUser Createur { get; set; }

        // Ok
        public string FormateurIdDernierContact { get; set; }
        [ForeignKey(nameof(FormateurIdDernierContact))]
        public virtual ApplicationUser FormateurDernierContact { get; set; }

        // ok
        public string StagiaireIdDernierContact { get; set; }
        [ForeignKey(nameof(StagiaireIdDernierContact))]
        public virtual ApplicationUser StagiaireDernier { get; set; }


    }
}