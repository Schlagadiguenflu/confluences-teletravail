using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class Stage
    {
        [Key]
        public int StageId { get; set; }

        [Required]
        [StringLength(30)]
        public string Nom { get; set; }
        
        public DateTime Debut { get; set; }

        public DateTime? Fin { get; set; }

        public string Bilan { get; set; }

        [StringLength(500)]
        public string ActionSuivi { get; set; }
      
        [StringLength(500)]
        public string Remarque { get; set; }

        public bool? Rapport { get; set; }
        
        public bool? Attestation { get; set; }

        public bool? Repas { get; set; }

        public bool? Trajets { get; set; }

        // FK
        [Required]
        public string StagiaireId { get; set; }
        [ForeignKey(nameof(StagiaireId))]
        public virtual ApplicationUser Stagiaire { get; set; }

        public int? EntrepriseId { get; set; }
        [ForeignKey(nameof(EntrepriseId))]
        public virtual Entreprise Entreprise { get; set; }

        public string CreateurId { get; set; }
        [ForeignKey(nameof(CreateurId))]
        public virtual ApplicationUser Createur { get; set; }

        [Required]
        public int TypeMetierId { get; set; }
        [ForeignKey(nameof(TypeMetierId))]
        public virtual TypeMetier TypeMetier { get; set; }

        public int? TypeStageId { get; set; }
        [ForeignKey(nameof(TypeStageId))]
        public virtual TypeStage TypeStage { get; set; }

        [StringLength(11)]
        public string HoraireMatin { get; set; }

        [StringLength(11)]
        public string HoraireApresMidi { get; set; }

        public int? TypeAnnonceId { get; set; }
        [ForeignKey(nameof(TypeAnnonceId))]
        public virtual TypeAnnonce TypeAnnonce { get; set; }
    }
}
