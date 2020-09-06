using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class AppointmentStudent
    {
        [Key]
        public int AppointmentStudentId { get; set; }

        [Required]
        public int AppointmentId { get; set; }
        [ForeignKey(nameof(AppointmentId))]
        public virtual Appointment Appointment { get; set; }

        [Required]
        public string StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual ApplicationUser Student { get; set; }
    }
}
