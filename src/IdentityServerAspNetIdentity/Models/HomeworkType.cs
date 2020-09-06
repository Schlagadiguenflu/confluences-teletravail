using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class HomeworkType
    {
        [Key]
        public int HomeworkTypeId { get; set; }

        [Required]
        public string HomeworkTypeName { get; set; }

        public int HomeworkOrder { get; set; }
    }
}
