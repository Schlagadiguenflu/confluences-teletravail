using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class TypeDomaine
    {
        public TypeDomaine()
        {
            Entrepris = new HashSet<Entrepris>();
        }

        [Key]
        public int TypeDomaineId { get; set; }
        [StringLength(60)]
        public string Libelle { get; set; }

        [InverseProperty("TypeDomaine")]
        public virtual ICollection<Entrepris> Entrepris { get; set; }
    }
}
