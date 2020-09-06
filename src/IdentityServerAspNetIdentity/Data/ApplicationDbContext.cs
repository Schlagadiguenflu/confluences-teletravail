using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentityServerAspNetIdentity.Models;

namespace IdentityServerAspNetIdentity.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<AppointmentStudent> AppointmentStudents { get; set; }
        public virtual DbSet<SchoolClassRoom> SchoolClassRooms { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<SessionStudent> SessionStudents { get; set; }
        public virtual DbSet<SessionTeacher> SessionTeachers { get; set; }
        public virtual DbSet<HomeworkType> HomeworkTypes { get; set; }
        public virtual DbSet<HomeworkStudent> HomeworkStudents { get; set; }
        public virtual DbSet<SchoolClassRoomExplanation> SchoolClassRoomExplanations { get; set; }
        public virtual DbSet<AnnouncePage> AnnouncePages { get; set; }
        public virtual DbSet<AnnouncePageLink> AnnouncePageLinks { get; set; }
        public virtual DbSet<SessionNumber> SessionNumbers { get; set; }
        public virtual DbSet<HomeworkV2> HomeworkV2s { get; set; }
        public virtual DbSet<Theory> Theories { get; set; }
        public virtual DbSet<Exercice> Exercices { get; set; }
        public virtual DbSet<HomeworkV2Student> HomeworkV2Students { get; set; }
        public virtual DbSet<ExerciceAlone> ExercicesAlone { get; set; }
        public virtual DbSet<HomeworkV2StudentExerciceAlone> HomeworkV2StudentExerciceAlones { get; set; }
        public virtual DbSet<TypeMoyen> TypeMoyens { get; set; }
        public virtual DbSet<TypeStage> TypeStages { get; set; }
        public virtual DbSet<TypeEntreprise> TypeEntreprises { get; set; }
        public virtual DbSet<TypeAffiliation> TypeAffiliations { get; set; }
        public virtual DbSet<TypeAnnonce> TypeAnnonces { get; set; }
        public virtual DbSet<TypeDomaine> TypeDomaines { get; set; }
        public virtual DbSet<TypeMetier> TypeMetiers { get; set; }
        public virtual DbSet<TypeOffre> TypeOffres { get; set; }
        public virtual DbSet<Entreprise> Entreprises { get; set; }
        public virtual DbSet<Stage> Stages { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<EntrepriseMetier> EntrepriseMetiers { get; set; }
        public virtual DbSet<EntrepriseOffre> EntrepriseOffres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=LoveMirroring;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<SessionTeacher>().HasKey(st => new { st.SessionId, st.TeacherId });
            builder.Entity<SessionStudent>().HasKey(st => new { st.SessionId, st.StudentId });
            builder.Entity<EntrepriseMetier>().HasKey(st => new { st.EntrepriseId, st.TypeMetierId });
            builder.Entity<EntrepriseOffre>().HasKey(st => new { st.EntrepriseId, st.TypeOffreId });
        }
    }
}
