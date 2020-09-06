using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class SessionStudent
    {
        [DisplayName("Session")]
        [Key]
        public int SessionId { get; set; }
        [DisplayName("Participant-e")]
        [Key]
        public string StudentId { get; set; }
        [DisplayName("Session")]
        [ForeignKey(nameof(SessionId))]
        [InverseProperty("SessionStudents")]
        public virtual Session Session { get; set; }
        [DisplayName("Participant-e")]
        [ForeignKey(nameof(StudentId))]
        [InverseProperty(nameof(AspNetUser.SessionStudents))]
        public virtual AspNetUser Student { get; set; }
    }
}
