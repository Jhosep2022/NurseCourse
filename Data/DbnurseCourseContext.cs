using Microsoft.EntityFrameworkCore;
using NurseCourse.Models;

namespace NurseCourse.Data;

public partial class DbnurseCourseContext : DbContext
{
    public DbnurseCourseContext(DbContextOptions<DbnurseCourseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contenido> Contenidos { get; set; }
    public virtual DbSet<Curso> Cursos { get; set; }
    public virtual DbSet<Examene> Examenes { get; set; }
    public virtual DbSet<Modulo> Modulos { get; set; }
    public virtual DbSet<Progreso> Progreso { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<NotaExamen> NotasExamenes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("utf8mb4_0900_ai_ci").HasCharSet("utf8mb4");

        modelBuilder.Entity<Contenido>(entity =>
        {
            entity.HasKey(e => e.ContenidoId);
            entity.HasOne(d => d.Modulo)
                  .WithMany(p => p.Contenidos)
                  .HasForeignKey(d => d.ModuloId)
                  .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Curso>(entity =>
        {
            entity.HasKey(e => e.CursoId);
            entity.HasMany(d => d.Examenes)
                  .WithOne(p => p.Curso)
                  .HasForeignKey(d => d.CursoId);
            entity.HasMany(d => d.Modulos)
                  .WithOne(p => p.Curso)
                  .HasForeignKey(d => d.CursoId);
        });

        modelBuilder.Entity<Examene>(entity =>
        {
            entity.HasKey(e => e.ExamenId);
            entity.HasOne(d => d.Curso)
                  .WithMany(p => p.Examenes)
                  .HasForeignKey(d => d.CursoId);
            entity.HasMany(d => d.NotasExamenes)
                  .WithOne(n => n.Examen)
                  .HasForeignKey(n => n.ExamenId);
        });

        modelBuilder.Entity<Modulo>(entity =>
        {
            entity.HasKey(e => e.ModuloID);
            entity.HasOne(d => d.Curso)
                  .WithMany(p => p.Modulos)
                  .HasForeignKey(d => d.CursoId);
        });

        modelBuilder.Entity<Progreso>(entity =>
        {
            entity.HasKey(e => e.ProgresoId);
            entity.HasOne(d => d.Contenido)
                  .WithMany(p => p.Progresos)
                  .HasForeignKey(d => d.ContenidoId);
            entity.HasOne(d => d.Usuario)
                  .WithMany(p => p.Progresos)
                  .HasForeignKey(d => d.UsuarioId);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RolId);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId);
            entity.HasOne(d => d.Rol)
                  .WithMany(p => p.Usuarios)
                  .HasForeignKey(d => d.RolId);
            entity.HasMany(d => d.NotasExamenes)
                  .WithOne(n => n.Usuario)
                  .HasForeignKey(n => n.UsuarioId);
        });

        modelBuilder.Entity<NotaExamen>(entity =>
        {
            entity.HasKey(e => e.NotaExamenId);
            entity.HasOne(d => d.Usuario)
                  .WithMany(p => p.NotasExamenes)
                  .HasForeignKey(d => d.UsuarioId);
            entity.HasOne(d => d.Examen)
                  .WithMany(p => p.NotasExamenes)
                  .HasForeignKey(d => d.ExamenId);
        });
    }
}
