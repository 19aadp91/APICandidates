using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<TblCandidate> Candidates { get; set; }
        public DbSet<TblCandidateExperience> CandidatesExperiences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TblCandidate>(entity =>
            {
                entity.HasKey(e => e.IdCandidate).HasName("PK_Candidates");
                entity.ToTable("Candidates");
                entity.Property(e => e.IdCandidate).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Surname).HasMaxLength(150).IsRequired();
                entity.Property(e => e.Birthdate).HasColumnType("datetime").IsRequired();
                entity.Property(e => e.Email).HasMaxLength(250).IsRequired();
                entity.Property(e => e.InsertDate).HasColumnType("datetime").IsRequired();
                entity.Property(e => e.ModifyDate).HasColumnType("datetime").IsRequired(false);
                entity.HasIndex(e => e.Email).IsUnique();
            });

            modelBuilder.Entity<TblCandidateExperience>(entity =>
            {
                entity.HasKey(e => e.IdCandidatesExperiences).HasName("PK_CandidatesExperiences");
                entity.ToTable("CandidatesExperiences");
                entity.Property(e => e.IdCandidatesExperiences).ValueGeneratedOnAdd();
                entity.Property(e => e.Company).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Job).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(4000).IsRequired();
                entity.Property(e => e.Salary).HasColumnType("numeric(8,2)").IsRequired();
                entity.Property(e => e.BeginDate).HasColumnType("datetime").IsRequired();
                entity.Property(e => e.EndDate).HasColumnType("datetime").IsRequired(false);
                entity.Property(e => e.InsertDate).HasColumnType("datetime").IsRequired();
                entity.Property(e => e.ModifyDate).HasColumnType("datetime").IsRequired(false);

                entity.HasOne(d => d.Candidate)
                    .WithMany(p => p.Experiences)
                    .HasForeignKey(d => d.IdCandidate)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CandidatesExperiences_Candidates");
            });
        }
    }
}
