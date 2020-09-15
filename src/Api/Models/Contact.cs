using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class Contact
    {
        [Key]
        public int ContactId { get; set; }
        [Required]
        [StringLength(50)]
        public string Nom { get; set; }
        [StringLength(50)]
        public string Prenom { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(13)]
        public string TelFix { get; set; }
        [StringLength(13)]
        public string Natel { get; set; }
        public DateTime? DateCreation { get; set; }
        public DateTime? DateModification { get; set; }
        public string CreateurId { get; set; }
        public string ModificateurId { get; set; }
        public int EntrepriseId { get; set; }
        [StringLength(50)]
        public string Fonction { get; set; }

        [ForeignKey(nameof(CreateurId))]
        [InverseProperty(nameof(AspNetUser.ContactCreateurs))]
        public virtual AspNetUser Createur { get; set; }
        [ForeignKey(nameof(EntrepriseId))]
        [InverseProperty(nameof(Entrepris.Contacts))]
        public virtual Entrepris Entreprise { get; set; }
        [ForeignKey(nameof(ModificateurId))]
        [InverseProperty(nameof(AspNetUser.ContactModificateurs))]
        public virtual AspNetUser Modificateur { get; set; }
    }
}
