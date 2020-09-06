using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class Gender
    {

        [Key]
        public short GenderId { get; set; }
        [Required]
        [StringLength(10)]
        public string GenderName { get; set; }

    }
}
