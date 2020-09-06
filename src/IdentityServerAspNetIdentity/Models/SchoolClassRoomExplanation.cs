using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class SchoolClassRoomExplanation
    {
        [Key]
        public int SchoolClassRoomExplanationId { get; set; }

        [Required]
        public string LanguageName { get; set; }

        [Required]
        public string AudioLink { get; set; }

        [Required]
        public int SchoolClassRoomId { get; set; }
        [ForeignKey(nameof(SchoolClassRoomId))]
        public virtual SchoolClassRoom SchoolClassRoom { get; set; }
    }
}
