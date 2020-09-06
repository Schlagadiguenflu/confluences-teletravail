using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class TypeOffre
    {
        public TypeOffre()
        {
            EntrepriseOffres = new HashSet<EntrepriseOffre>();
        }

        [Key]
        public int TypeOffreId { get; set; }
        [StringLength(30)]
        public string Libelle { get; set; }

        [InverseProperty(nameof(EntrepriseOffre.TypeOffre))]
        public virtual ICollection<EntrepriseOffre> EntrepriseOffres { get; set; }
    }
}
