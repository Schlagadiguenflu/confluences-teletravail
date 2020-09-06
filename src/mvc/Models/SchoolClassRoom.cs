using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
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
        [DisplayName("Nom")]
        [Required]
        public string SchoolClassRoomName { get; set; }
        [DisplayName("Lien du tuto")]
        public string ExplanationVideoLink { get; set; }

        [InverseProperty(nameof(SchoolClassRoomExplanation.SchoolClassRoom))]
        public virtual ICollection<SchoolClassRoomExplanation> SchoolClassRoomExplanations { get; set; }
        [InverseProperty(nameof(Session.SchoolClassRoom))]
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
