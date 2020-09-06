using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class SessionTeacher
    {
        [DisplayName("Session")]
        // Is Key aswell go look in context
        public int SessionId { get; set; }
        [ForeignKey(nameof(SessionId))]
        public virtual Session Session { get; set; }

        [DisplayName("Formateur-trice")]
        // Is Key aswell go look in context
        public string TeacherId { get; set; }
        [ForeignKey(nameof(TeacherId))]
        public virtual ApplicationUser Teacher { get; set; }
    }
}
