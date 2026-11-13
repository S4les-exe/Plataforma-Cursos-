using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PlataformaCursos.Domains;

namespace PlataformaCursos.Contexts;

public partial class PlataformaCursosDBContext : DbContext
{
    public PlataformaCursosDBContext()
    {
    }

    public PlataformaCursosDBContext(DbContextOptions<PlataformaCursosDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aluno> Aluno { get; set; }

    public virtual DbSet<Curso> Curso { get; set; }

    public virtual DbSet<Instrutor> Instrutor { get; set; }

    public virtual DbSet<Matricula> Matricula { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=PlataformaCursosDB;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aluno>(entity =>
        {
            entity.HasKey(e => e.IdAluno).HasName("PK__Aluno__8092FCB3864780AF");

            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Nome).HasMaxLength(150);
        });

        modelBuilder.Entity<Curso>(entity =>
        {
            entity.HasKey(e => e.IdCurso).HasName("PK__Curso__085F27D68E6A8E01");

            entity.Property(e => e.Disponivel).HasDefaultValue(true);
            entity.Property(e => e.Nome).HasMaxLength(150);

            entity.HasOne(d => d.IdInstrutorNavigation).WithMany(p => p.Curso)
                .HasForeignKey(d => d.IdInstrutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Curso_Instrutor");
        });

        modelBuilder.Entity<Instrutor>(entity =>
        {
            entity.HasKey(e => e.IdInstrutor).HasName("PK__Instruto__5E3B6019872B3E7E");

            entity.Property(e => e.AreaEspecializacao).HasMaxLength(150);
            entity.Property(e => e.Nome).HasMaxLength(150);
        });

        modelBuilder.Entity<Matricula>(entity =>
        {
            entity.HasKey(e => e.IdMatricula).HasName("PK__Matricul__AD06C20FB38A2F87");

            entity.HasIndex(e => new { e.IdAluno, e.IdCurso }, "UQ_Aluno_Curso").IsUnique();

            entity.Property(e => e.DataMatricula)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdAlunoNavigation).WithMany(p => p.Matricula)
                .HasForeignKey(d => d.IdAluno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Matricula_Aluno");

            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.Matricula)
                .HasForeignKey(d => d.IdCurso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Matricula_Curso");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
