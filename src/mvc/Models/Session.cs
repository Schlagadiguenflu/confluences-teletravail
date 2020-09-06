using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class Session
    {
        public Session()
        {
            Homework = new HashSet<Homework>();
            HomeworkV2s = new HashSet<HomeworkV2s>();
            SessionStudents = new HashSet<SessionStudent>();
            SessionTeachers = new HashSet<SessionTeacher>();
        }

        [Key]
        public int SessionId { get; set; }
        [DisplayName("Date de début")]
        public DateTime DateStart { get; set; }
        [DisplayName("Date de fin")]
        public DateTime DateEnd { get; set; }
        [DisplayName("Classe")]
        public int SchoolClassRoomId { get; set; }
        [DisplayName("Numéro de session")]
        public int SessionNumberId { get; set; }

        [DisplayName("Classe")]
        [ForeignKey(nameof(SchoolClassRoomId))]
        [InverseProperty("Sessions")]
        public virtual SchoolClassRoom SchoolClassRoom { get; set; }
        [ForeignKey(nameof(SessionNumberId))]
        [InverseProperty("Sessions")]
        public virtual SessionNumber SessionNumber { get; set; }
        [InverseProperty("Session")]
        public virtual ICollection<Homework> Homework { get; set; }
        [InverseProperty("Session")]
        public virtual ICollection<HomeworkV2s> HomeworkV2s { get; set; }
        [InverseProperty(nameof(SessionStudent.Session))]
        public virtual ICollection<SessionStudent> SessionStudents { get; set; }
        [InverseProperty(nameof(SessionTeacher.Session))]
        public virtual ICollection<SessionTeacher> SessionTeachers { get; set; }
    }
}
