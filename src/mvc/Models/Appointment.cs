using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class Appointment
    {
        public Appointment()
        {
            AppointmentStudents = new HashSet<AppointmentStudent>();
        }

        [Key]
        public int AppointmentId { get; set; }
        [DisplayName("Nom")]
        [Required]
        public string AppointmentName { get; set; }
        [DisplayName("Début")]
        public DateTime DateStart { get; set; }
        [DisplayName("Fin")]
        public DateTime DateEnd { get; set; }
        [DisplayName("Hebdomadaire")]
        public bool IsWeekly { get; set; }
        [DisplayName("Formateur-trice")]
        [Required]
        public string TeacherId { get; set; }
        [DisplayName("Formateur-trice")]
        [ForeignKey(nameof(TeacherId))]
        [InverseProperty(nameof(AspNetUser.Appointments))]
        public virtual AspNetUser Teacher { get; set; }
        [InverseProperty(nameof(AppointmentStudent.Appointment))]
        public virtual ICollection<AppointmentStudent> AppointmentStudents { get; set; }
    }
}
