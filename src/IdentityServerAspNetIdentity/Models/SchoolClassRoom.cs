using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class SchoolClassRoom
    {
        [Key]
        public int SchoolClassRoomId { get; set; }

        [DisplayName("Nom")]
        [Required]
        public string SchoolClassRoomName { get; set; }

        [DisplayName("Lien du tuto")]
        public string ExplanationVideoLink { get; set; }

    }
}
