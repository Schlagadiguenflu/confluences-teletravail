using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class TypeMoyen
    {
        public TypeMoyen()
        {
            Entrepris = new HashSet<Entrepris>();
        }

        [Key]
        public int TypeMoyenId { get; set; }
        [Required]
        [StringLength(20)]
        public string Libelle { get; set; }

        [InverseProperty("TypeMoyen")]
        public virtual ICollection<Entrepris> Entrepris { get; set; }
    }
}
