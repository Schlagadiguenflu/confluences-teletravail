using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class Session
    {
        [Key]
        public int SessionId { get; set; }

        [DisplayName("Date de début")]
        [Required]
        public DateTime DateStart { get; set; }

        [DisplayName("Date de fin")]
        [Required]
        public DateTime DateEnd { get; set; }

        [DisplayName("Classe")]
        [Required]
        public int SchoolClassRoomId { get; set; }
        [ForeignKey(nameof(SchoolClassRoomId))]
        public virtual SchoolClassRoom SchoolClassRoom { get; set; }

        [DisplayName("Numéro de la session")]
        [Required]
        public int SessionNumberId { get; set; }
        [ForeignKey(nameof(SessionNumberId))]
        public virtual SessionNumber SessionNumber { get; set; }

    }
}
