using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class TypeEntrepris
    {
        public TypeEntrepris()
        {
            Entrepris = new HashSet<Entrepris>();
        }

        [Key]
        public int TypeEntrepriseId { get; set; }
        [Required]
        [StringLength(50)]
        public string Nom { get; set; }

        [InverseProperty("TypeEntreprise")]
        public virtual ICollection<Entrepris> Entrepris { get; set; }
    }
}
