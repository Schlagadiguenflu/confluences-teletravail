using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            AppointmentStudents = new HashSet<AppointmentStudent>();
            Appointments = new HashSet<Appointment>();
            AspNetUserClaims = new HashSet<AspNetUserClaim>();
            AspNetUserLogins = new HashSet<AspNetUserLogin>();
            AspNetUserRoles = new HashSet<AspNetUserRole>();
            AspNetUserTokens = new HashSet<AspNetUserToken>();
            ContactCreateurs = new HashSet<Contact>();
            ContactModificateurs = new HashSet<Contact>();
            EntreprisCreateurs = new HashSet<Entrepris>();
            EntreprisFormateurIdDernierContactNavigations = new HashSet<Entrepris>();
            EntreprisStagiaireIdDernierContactNavigations = new HashSet<Entrepris>();
            Exercices = new HashSet<Exercice>();
            ExercicesAlones = new HashSet<ExercicesAlone>();
            Homework = new HashSet<Homework>();
            HomeworkStudents = new HashSet<HomeworkStudent>();
            HomeworkV2s = new HashSet<HomeworkV2s>();
            HomeworkV2studentExerciceAlones = new HashSet<HomeworkV2studentExerciceAlones>();
            HomeworkV2students = new HashSet<HomeworkV2students>();
            SessionStudents = new HashSet<SessionStudent>();
            SessionTeachers = new HashSet<SessionTeacher>();
            StageCreateurs = new HashSet<Stage>();
            StageStagiaires = new HashSet<Stage>();
            Theories = new HashSet<Theory>();
        }

        [Key]
        public string Id { get; set; }
        [StringLength(256)]
        public string UserName { get; set; }
        [StringLength(256)]
        public string NormalizedUserName { get; set; }
        [StringLength(256)]
        public string Email { get; set; }
        [StringLength(256)]
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        [Column(TypeName = "timestamp with time zone")]
        public DateTime? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public short GenderId { get; set; }
        public string PathImage { get; set; }
        public bool WantsMoreHomeworks { get; set; }
        public bool HasSeenHelpVideo { get; set; }
        public int? TypeAffiliationId { get; set; }

        [ForeignKey(nameof(GenderId))]
        [InverseProperty("AspNetUsers")]
        public virtual Gender Gender { get; set; }
        [ForeignKey(nameof(TypeAffiliationId))]
        [InverseProperty("AspNetUsers")]
        public virtual TypeAffiliation TypeAffiliation { get; set; }
        [InverseProperty(nameof(AppointmentStudent.Student))]
        public virtual ICollection<AppointmentStudent> AppointmentStudents { get; set; }
        [InverseProperty(nameof(Appointment.Teacher))]
        public virtual ICollection<Appointment> Appointments { get; set; }
        [InverseProperty(nameof(AspNetUserClaim.User))]
        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        [InverseProperty(nameof(AspNetUserLogin.User))]
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        [InverseProperty(nameof(AspNetUserRole.User))]
        public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
        [InverseProperty(nameof(AspNetUserToken.User))]
        public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; }
        [InverseProperty(nameof(Contact.Createur))]
        public virtual ICollection<Contact> ContactCreateurs { get; set; }
        [InverseProperty(nameof(Contact.Modificateur))]
        public virtual ICollection<Contact> ContactModificateurs { get; set; }
        [InverseProperty(nameof(Entrepris.Createur))]
        public virtual ICollection<Entrepris> EntreprisCreateurs { get; set; }
        [InverseProperty(nameof(Entrepris.FormateurIdDernierContactNavigation))]
        public virtual ICollection<Entrepris> EntreprisFormateurIdDernierContactNavigations { get; set; }
        [InverseProperty(nameof(Entrepris.StagiaireIdDernierContactNavigation))]
        public virtual ICollection<Entrepris> EntreprisStagiaireIdDernierContactNavigations { get; set; }
        [InverseProperty(nameof(Exercice.Teacher))]
        public virtual ICollection<Exercice> Exercices { get; set; }
        [InverseProperty(nameof(ExercicesAlone.Teacher))]
        public virtual ICollection<ExercicesAlone> ExercicesAlones { get; set; }
        [InverseProperty("Teacher")]
        public virtual ICollection<Homework> Homework { get; set; }
        [InverseProperty(nameof(HomeworkStudent.Student))]
        public virtual ICollection<HomeworkStudent> HomeworkStudents { get; set; }
        [InverseProperty("Teacher")]
        public virtual ICollection<HomeworkV2s> HomeworkV2s { get; set; }
        [InverseProperty("Student")]
        public virtual ICollection<HomeworkV2studentExerciceAlones> HomeworkV2studentExerciceAlones { get; set; }
        [InverseProperty("Student")]
        public virtual ICollection<HomeworkV2students> HomeworkV2students { get; set; }
        [InverseProperty(nameof(SessionStudent.Student))]
        public virtual ICollection<SessionStudent> SessionStudents { get; set; }
        [InverseProperty(nameof(SessionTeacher.Teacher))]
        public virtual ICollection<SessionTeacher> SessionTeachers { get; set; }
        [InverseProperty(nameof(Stage.Createur))]
        public virtual ICollection<Stage> StageCreateurs { get; set; }
        [InverseProperty(nameof(Stage.Stagiaire))]
        public virtual ICollection<Stage> StageStagiaires { get; set; }
        [InverseProperty(nameof(Theory.Teacher))]
        public virtual ICollection<Theory> Theories { get; set; }
    }
}
