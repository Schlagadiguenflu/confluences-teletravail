using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class SessionTeacher
    {
        [Key]
        public int SessionId { get; set; }
        [Key]
        public string TeacherId { get; set; }
        public int Order { get; set; }

        [ForeignKey(nameof(SessionId))]
        [InverseProperty("SessionTeachers")]
        public virtual Session Session { get; set; }
        [ForeignKey(nameof(TeacherId))]
        [InverseProperty(nameof(AspNetUser.SessionTeachers))]
        public virtual AspNetUser Teacher { get; set; }
    }
}
