using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class SessionStudent
    {
        [Key]
        public int SessionId { get; set; }
        [Key]
        public string StudentId { get; set; }

        [ForeignKey(nameof(SessionId))]
        [InverseProperty("SessionStudents")]
        public virtual Session Session { get; set; }
        [ForeignKey(nameof(StudentId))]
        [InverseProperty(nameof(AspNetUser.SessionStudents))]
        public virtual AspNetUser Student { get; set; }
    }
}
