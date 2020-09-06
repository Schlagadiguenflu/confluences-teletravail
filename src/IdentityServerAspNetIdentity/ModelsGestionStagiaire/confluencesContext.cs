using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace IdentityServerAspNetIdentity.ModelsGestionStagiaire
{
    public partial class confluencesContext : DbContext
    {
        public confluencesContext()
        {
        }

        public confluencesContext(DbContextOptions<confluencesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Acteur> Acteurs { get; set; }
        public virtual DbSet<Annonce> Annonces { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Domaine> Domaines { get; set; }
        public virtual DbSet<Eleve> Eleves { get; set; }
        public virtual DbSet<Entremetier> Entremetiers { get; set; }
        public virtual DbSet<Entreprise> Entreprises { get; set; }
        public virtual DbSet<Metier> Metiers { get; set; }
        public virtual DbSet<Moyen> Moyens { get; set; }
        public virtual DbSet<Stage> Stages { get; set; }
        public virtual DbSet<Typentreprise> Typentreprises { get; set; }
        public virtual DbSet<Typestage> Typestages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Server=vs1.confluences.ch;Port=54347;Database=confluences;User Id=confluence_mgr;Password=ConfluencesTAAT47;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Acteur>(entity =>
            {
                entity.Property(e => e.Id).IsFixedLength();
            });

            modelBuilder.Entity<Annonce>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Codeaffiliation)
                    .HasName("Categories_pkey");

                entity.Property(e => e.Codeaffiliation).IsFixedLength();
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasKey(e => e.Idcontact)
                    .HasName("IdContact");

                entity.Property(e => e.Idcontact).HasIdentityOptions(100L, null, null, null, null, null);

                entity.Property(e => e.Codcreatcontact).IsFixedLength();

                entity.Property(e => e.Codmodifcontact).IsFixedLength();

                entity.HasOne(d => d.IdentNavigation)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.Ident)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("entid");
            });

            modelBuilder.Entity<Domaine>(entity =>
            {
                entity.HasKey(e => e.Codedomaine)
                    .HasName("domaine_pkey");

                entity.HasIndex(e => e.Codedomaine)
                    .HasName("PkcodeDomaine")
                    .IsUnique();

                entity.Property(e => e.Codedomaine).IsFixedLength();
            });

            modelBuilder.Entity<Eleve>(entity =>
            {
                entity.HasKey(e => e.Trgeleve)
                    .HasName("eleves_pkey");

                entity.Property(e => e.Trgeleve).IsFixedLength();
            });

            modelBuilder.Entity<Entremetier>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<Entreprise>(entity =>
            {
                entity.HasKey(e => e.Ident)
                    .HasName("IDENT");

                entity.HasIndex(e => e.Catent)
                    .HasName("CatIndexent");

                entity.HasIndex(e => e.Cpent)
                    .HasName("cpindexent");

                entity.Property(e => e.Ident).HasIdentityOptions(100L, null, null, null, null, null);

                entity.Property(e => e.CodeCreateur).IsFixedLength();

                entity.Property(e => e.CodederContact).IsFixedLength();

                entity.Property(e => e.Codedomaine).IsFixedLength();

                entity.Property(e => e.Moyen).IsFixedLength();

                entity.Property(e => e.Offre1)
                    .HasDefaultValueSql("false")
                    .HasComment("Stage");

                entity.Property(e => e.Offre2)
                    .HasDefaultValueSql("false")
                    .HasComment("Apprentissage");

                entity.Property(e => e.Offre3)
                    .HasDefaultValueSql("false")
                    .HasComment("Emploi Temporaire");

                entity.Property(e => e.Offre4)
                    .HasDefaultValueSql("false")
                    .HasComment("Emploi CDI");

                entity.Property(e => e.Pourqui).IsFixedLength();
            });

            modelBuilder.Entity<Metier>(entity =>
            {
                entity.HasKey(e => e.Codemetier)
                    .HasName("metier_pkey");
            });

            modelBuilder.Entity<Moyen>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Code).IsFixedLength();
            });

            modelBuilder.Entity<Stage>(entity =>
            {
                entity.Property(e => e.Stageid).HasIdentityOptions(null, null, 20L, 32000L, null, null);

                entity.Property(e => e.Attestation).HasDefaultValueSql("false");

                entity.Property(e => e.Eleve).IsFixedLength();

                entity.Property(e => e.HoraireApres).IsFixedLength();

                entity.Property(e => e.HoraireAvant).IsFixedLength();

                entity.Property(e => e.Rapport).HasDefaultValueSql("false");

                entity.HasOne(d => d.IdentNavigation)
                    .WithMany(p => p.Stages)
                    .HasForeignKey(d => d.Ident)
                    .HasConstraintName("entid");
            });

            modelBuilder.Entity<Typentreprise>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Typestage>(entity =>
            {
                entity.HasKey(e => e.Codetypestage)
                    .HasName("typestage_pkey");

                entity.Property(e => e.Codetypestage).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
