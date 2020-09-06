using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class SessionStudent
    {
        [DisplayName("Session")]
        public int SessionId { get; set; }
        [ForeignKey(nameof(SessionId))]
        public virtual Session Session { get; set; }

        [DisplayName("Participant-e")]
        public string StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual ApplicationUser Student { get; set; }
    }
}
