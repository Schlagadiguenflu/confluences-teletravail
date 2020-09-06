using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class AppointmentStudent
    {
        [Key]
        public int AppointmentStudentId { get; set; }
        [DisplayName("Nom")]
        public int AppointmentId { get; set; }
        [DisplayName("Nom du participant-e")]
        [Required]
        public string StudentId { get; set; }
        [DisplayName("Nom")]
        [ForeignKey(nameof(AppointmentId))]
        [InverseProperty("AppointmentStudents")]
        public virtual Appointment Appointment { get; set; }
        [DisplayName("Nom du participant-e")]
        [ForeignKey(nameof(StudentId))]
        [InverseProperty(nameof(AspNetUser.AppointmentStudents))]
        public virtual AspNetUser Student { get; set; }
    }
}
