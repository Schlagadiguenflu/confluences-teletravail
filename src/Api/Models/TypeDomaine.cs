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
            EntrepriseDomaines = new HashSet<EntrepriseDomaine>();
        }

        [Key]
        public int TypeDomaineId { get; set; }
        [StringLength(60)]
        public string Libelle { get; set; }
        [StringLength(300)]
        public string OldNames { get; set; }

        [InverseProperty(nameof(EntrepriseDomaine.TypeDomaine))]
        public virtual ICollection<EntrepriseDomaine> EntrepriseDomaines { get; set; }
    }
}
