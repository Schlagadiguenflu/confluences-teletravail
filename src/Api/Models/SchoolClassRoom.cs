using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class SchoolClassRoom
    {
        public SchoolClassRoom()
        {
            SchoolClassRoomExplanations = new HashSet<SchoolClassRoomExplanation>();
            Sessions = new HashSet<Session>();
        }

        [Key]
        public int SchoolClassRoomId { get; set; }
        [Required]
        public string SchoolClassRoomName { get; set; }
        public string ExplanationVideoLink { get; set; }
        public bool IsArchived { get; set; }

        [InverseProperty(nameof(SchoolClassRoomExplanation.SchoolClassRoom))]
        public virtual ICollection<SchoolClassRoomExplanation> SchoolClassRoomExplanations { get; set; }
        [InverseProperty(nameof(Session.SchoolClassRoom))]
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
