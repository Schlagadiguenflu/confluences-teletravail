using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class Stage
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
        [Required]
        public string StagiaireId { get; set; }
        public int? EntrepriseId { get; set; }
        public string CreateurId { get; set; }
        public int TypeMetierId { get; set; }
        public int? TypeStageId { get; set; }
        [StringLength(11)]
        public string HoraireMatin { get; set; }
        [StringLength(11)]
        public string HoraireApresMidi { get; set; }
        public int? TypeAnnonceId { get; set; }

        [ForeignKey(nameof(CreateurId))]
        [InverseProperty(nameof(AspNetUser.StageCreateurs))]
        public virtual AspNetUser Createur { get; set; }
        [ForeignKey(nameof(EntrepriseId))]
        [InverseProperty(nameof(Entrepris.Stages))]
        public virtual Entrepris Entreprise { get; set; }
        [ForeignKey(nameof(StagiaireId))]
        [InverseProperty(nameof(AspNetUser.StageStagiaires))]
        public virtual AspNetUser Stagiaire { get; set; }
        [ForeignKey(nameof(TypeAnnonceId))]
        [InverseProperty("Stages")]
        public virtual TypeAnnonce TypeAnnonce { get; set; }
        [ForeignKey(nameof(TypeMetierId))]
        [InverseProperty("Stages")]
        public virtual TypeMetier TypeMetier { get; set; }
        [ForeignKey(nameof(TypeStageId))]
        [InverseProperty("Stages")]
        public virtual TypeStage TypeStage { get; set; }
    }
}
