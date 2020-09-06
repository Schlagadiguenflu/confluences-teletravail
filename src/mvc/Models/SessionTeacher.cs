using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class SessionTeacher
    {
        [DisplayName("Session")]
        [Key]
        public int SessionId { get; set; }
        [DisplayName("Formateur-trice")]
        [Key]
        public string TeacherId { get; set; }

        [ForeignKey(nameof(SessionId))]
        [InverseProperty("SessionTeachers")]
        public virtual Session Session { get; set; }
        [ForeignKey(nameof(TeacherId))]
        [InverseProperty(nameof(AspNetUser.SessionTeachers))]
        public virtual AspNetUser Teacher { get; set; }
    }
}
