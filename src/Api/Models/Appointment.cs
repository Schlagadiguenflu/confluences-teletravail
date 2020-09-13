using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class Appointment
    {
        public Appointment()
        {
            AppointmentStudents = new HashSet<AppointmentStudent>();
        }

        [Key]
        public int AppointmentId { get; set; }
        [Required]
        public string AppointmentName { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        [Required]
        public string TeacherId { get; set; }
        public bool IsWeekly { get; set; }

        [ForeignKey(nameof(TeacherId))]
        [InverseProperty(nameof(AspNetUser.Appointments))]
        public virtual AspNetUser Teacher { get; set; }
        [InverseProperty(nameof(AppointmentStudent.Appointment))]
        public virtual ICollection<AppointmentStudent> AppointmentStudents { get; set; }
    }
}
