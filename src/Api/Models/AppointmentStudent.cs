using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class AppointmentStudent
    {
        [Key]
        public int AppointmentStudentId { get; set; }
        public int AppointmentId { get; set; }
        [Required]
        public string StudentId { get; set; }

        [ForeignKey(nameof(AppointmentId))]
        [InverseProperty("AppointmentStudents")]
        public virtual Appointment Appointment { get; set; }
        [ForeignKey(nameof(StudentId))]
        [InverseProperty(nameof(AspNetUser.AppointmentStudents))]
        public virtual AspNetUser Student { get; set; }
    }
}
