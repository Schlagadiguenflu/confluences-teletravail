using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class SessionNumber
    {
        public SessionNumber()
        {
            Sessions = new HashSet<Session>();
        }

        [Key]
        public int SessionNumberId { get; set; }

        [InverseProperty(nameof(Session.SessionNumber))]
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
