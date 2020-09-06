using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class TypeAffiliation
    {
        public TypeAffiliation()
        {
            AspNetUsers = new HashSet<AspNetUser>();
        }

        [Key]
        public int TypeAffiliationId { get; set; }
        [StringLength(10)]
        public string Code { get; set; }
        [StringLength(50)]
        public string Libelle { get; set; }

        [InverseProperty(nameof(AspNetUser.TypeAffiliation))]
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
    }
}
