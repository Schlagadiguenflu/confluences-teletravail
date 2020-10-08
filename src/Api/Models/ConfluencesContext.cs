using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Api.Models
{
    public partial class ConfluencesContext : DbContext
    {
        public ConfluencesContext()
        {
        }

        public ConfluencesContext(DbContextOptions<ConfluencesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AnnouncePage> AnnouncePages { get; set; }
        public virtual DbSet<AnnouncePageLink> AnnouncePageLinks { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<AppointmentStudent> AppointmentStudents { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Entrepris> Entreprises { get; set; }
        public virtual DbSet<EntrepriseMetier> EntrepriseMetiers { get; set; }
        public virtual DbSet<EntrepriseOffre> EntrepriseOffres { get; set; }
        public virtual DbSet<Exercice> Exercices { get; set; }
        public virtual DbSet<ExercicesAlone> ExercicesAlones { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<Homework> Homework { get; set; }
        public virtual DbSet<HomeworkStudent> HomeworkStudents { get; set; }
        public virtual DbSet<HomeworkType> HomeworkTypes { get; set; }
        public virtual DbSet<HomeworkV2s> HomeworkV2s { get; set; }
        public virtual DbSet<HomeworkV2studentExerciceAlones> HomeworkV2studentExerciceAlones { get; set; }
        public virtual DbSet<HomeworkV2students> HomeworkV2students { get; set; }
        public virtual DbSet<SchoolClassRoom> SchoolClassRooms { get; set; }
        public virtual DbSet<SchoolClassRoomExplanation> SchoolClassRoomExplanations { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<SessionNumber> SessionNumbers { get; set; }
        public virtual DbSet<SessionStudent> SessionStudents { get; set; }
        public virtual DbSet<SessionTeacher> SessionTeachers { get; set; }
        public virtual DbSet<Stage> Stages { get; set; }
        public virtual DbSet<Theory> Theories { get; set; }
        public virtual DbSet<TypeAffiliation> TypeAffiliations { get; set; }
        public virtual DbSet<TypeAnnonce> TypeAnnonces { get; set; }
        public virtual DbSet<TypeDomaine> TypeDomaines { get; set; }
        public virtual DbSet<TypeEntrepris> TypeEntreprises { get; set; }
        public virtual DbSet<TypeMetier> TypeMetiers { get; set; }
        public virtual DbSet<TypeMoyen> TypeMoyens { get; set; }
        public virtual DbSet<TypeOffre> TypeOffres { get; set; }
        public virtual DbSet<TypeStage> TypeStages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnnouncePageLink>(entity =>
            {
                entity.HasIndex(e => e.AnnouncePageId);
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasIndex(e => e.TeacherId);
            });

            modelBuilder.Entity<AppointmentStudent>(entity =>
            {
                entity.HasIndex(e => e.AppointmentId);

                entity.HasIndex(e => e.StudentId);
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique();
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.GenderId);

                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique();

                entity.HasIndex(e => e.TypeAffiliationId);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.EntrepriseId);

                entity.HasIndex(e => e.ModificateurId);
            });

            modelBuilder.Entity<Entrepris>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.FormateurIdDernierContact);

                entity.HasIndex(e => e.StagiaireIdDernierContact);

                entity.HasIndex(e => e.TypeDomaineId);

                entity.HasIndex(e => e.TypeEntrepriseId);

                entity.HasIndex(e => e.TypeMoyenId);
            });

            modelBuilder.Entity<EntrepriseMetier>(entity =>
            {
                entity.HasKey(e => new { e.EntrepriseId, e.TypeMetierId });

                entity.HasIndex(e => e.TypeMetierId);
            });

            modelBuilder.Entity<EntrepriseOffre>(entity =>
            {
                entity.HasKey(e => new { e.EntrepriseId, e.TypeOffreId });

                entity.HasIndex(e => e.TypeOffreId);
            });

            modelBuilder.Entity<Exercice>(entity =>
            {
                entity.HasIndex(e => e.TeacherId);

                entity.HasIndex(e => e.TheoryId);

                entity.Property(e => e.CorrectionDate).HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");
            });

            modelBuilder.Entity<ExercicesAlone>(entity =>
            {
                entity.HasIndex(e => e.HomeworkV2id);

                entity.HasIndex(e => e.TeacherId);
            });

            modelBuilder.Entity<Homework>(entity =>
            {
                entity.HasIndex(e => e.HomeworkTypeId);

                entity.HasIndex(e => e.SessionId);

                entity.HasIndex(e => e.TeacherId);
            });

            modelBuilder.Entity<HomeworkStudent>(entity =>
            {
                entity.HasIndex(e => e.HomeworkId);

                entity.HasIndex(e => e.StudentId);
            });

            modelBuilder.Entity<HomeworkV2s>(entity =>
            {
                entity.HasIndex(e => e.HomeworkTypeId);

                entity.HasIndex(e => e.SessionId);

                entity.HasIndex(e => e.TeacherId);
            });

            modelBuilder.Entity<HomeworkV2studentExerciceAlones>(entity =>
            {
                entity.HasIndex(e => e.ExerciceId);

                entity.HasIndex(e => e.StudentId);
            });

            modelBuilder.Entity<HomeworkV2students>(entity =>
            {
                entity.HasIndex(e => e.ExerciceId);

                entity.HasIndex(e => e.StudentId);
            });

            modelBuilder.Entity<SchoolClassRoomExplanation>(entity =>
            {
                entity.HasIndex(e => e.SchoolClassRoomId);

                entity.HasOne(d => d.SchoolClassRoom)
                    .WithMany(p => p.SchoolClassRoomExplanations)
                    .HasForeignKey(d => d.SchoolClassRoomId)
                    .HasConstraintName("FK_SchoolClassRoomExplanations_SchoolClassRooms_SchoolClassRoo~");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasIndex(e => e.SchoolClassRoomId);

                entity.HasIndex(e => e.SessionNumberId);
            });

            modelBuilder.Entity<SessionStudent>(entity =>
            {
                entity.HasKey(e => new { e.SessionId, e.StudentId });

                entity.HasIndex(e => e.StudentId);
            });

            modelBuilder.Entity<SessionTeacher>(entity =>
            {
                entity.HasKey(e => new { e.SessionId, e.TeacherId });

                entity.HasIndex(e => e.TeacherId);
            });

            modelBuilder.Entity<Stage>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.EntrepriseId);

                entity.HasIndex(e => e.StagiaireId);

                entity.HasIndex(e => e.TypeAnnonceId);

                entity.HasIndex(e => e.TypeMetierId);

                entity.HasIndex(e => e.TypeStageId);
            });

            modelBuilder.Entity<Theory>(entity =>
            {
                entity.HasIndex(e => e.HomeworkV2id);

                entity.HasIndex(e => e.TeacherId);
            });

            modelBuilder.Entity<TypeAffiliation>(entity =>
            {
                entity.HasIndex(e => e.Code)
                    .IsUnique();

                entity.HasIndex(e => e.Libelle)
                    .IsUnique();
            });

            modelBuilder.Entity<TypeAnnonce>(entity =>
            {
                entity.HasIndex(e => e.Libelle)
                    .IsUnique();
            });

            modelBuilder.Entity<TypeDomaine>(entity =>
            {
                entity.HasIndex(e => e.Code)
                    .IsUnique();

                entity.HasIndex(e => e.Libelle)
                    .IsUnique();
            });

            modelBuilder.Entity<TypeEntrepris>(entity =>
            {
                entity.HasIndex(e => e.Nom)
                    .IsUnique();
            });

            modelBuilder.Entity<TypeMetier>(entity =>
            {
                entity.HasIndex(e => e.Code)
                    .IsUnique();

                entity.HasIndex(e => e.Libelle)
                    .IsUnique();
            });

            modelBuilder.Entity<TypeMoyen>(entity =>
            {
                entity.HasIndex(e => e.Code)
                    .IsUnique();

                entity.HasIndex(e => e.Libelle)
                    .IsUnique();
            });

            modelBuilder.Entity<TypeOffre>(entity =>
            {
                entity.HasIndex(e => e.Libelle)
                    .IsUnique();
            });

            modelBuilder.Entity<TypeStage>(entity =>
            {
                entity.HasIndex(e => e.Nom)
                    .IsUnique();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
