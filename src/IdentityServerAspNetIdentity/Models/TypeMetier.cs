﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class TypeMetier
    {
        [Key]
        public int TypeMetierId { get; set; }

        [StringLength(60)]
        public string Libelle { get; set; }

        [StringLength(300)]
        public string OldNames { get; set; }
    }
}
