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
    public virtual DbSet<OpcionesDeRespuesta> OpcionesDeRespuestas { get; set; }
    public virtual DbSet<Pregunta> Preguntas { get; set; }
    public virtual DbSet<Progreso> Progreso { get; set; }
    public virtual DbSet<RespuestasUsuario> RespuestasUsuarios { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            throw new InvalidOperationException("La configuración de la base de datos no está configurada correctamente.");
        }
    }

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
        });

        modelBuilder.Entity<Modulo>(entity =>
        {
            entity.HasKey(e => e.ModuloId);
            entity.HasOne(d => d.Curso)
                  .WithMany(p => p.Modulos)
                  .HasForeignKey(d => d.CursoId);
        });

        modelBuilder.Entity<OpcionesDeRespuesta>(entity =>
        {
            entity.HasKey(e => e.OpcionId);
            entity.HasOne(d => d.Pregunta)
                  .WithMany(p => p.OpcionesDeRespuesta)
                  .HasForeignKey(d => d.PreguntaId);
        });

        modelBuilder.Entity<Pregunta>(entity =>
        {
            entity.HasKey(e => e.PreguntaId);
            entity.HasOne(d => d.Examen)
                  .WithMany(p => p.Pregunta)
                  .HasForeignKey(d => d.ExamenId);
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

        modelBuilder.Entity<RespuestasUsuario>(entity =>
        {
            entity.ToTable("RespuestasUsuarios"); // Asegúrate de que esto coincida exactamente con el nombre en la base de datos
            entity.HasKey(e => e.RespuestaUsuarioId);
            entity.HasOne(d => d.Opcion)
                .WithMany(p => p.RespuestasUsuarios)
                .HasForeignKey(d => d.OpcionId);
            entity.HasOne(d => d.Pregunta)
                .WithMany(p => p.RespuestasUsuarios)
                .HasForeignKey(d => d.PreguntaId);
            entity.HasOne(d => d.Usuario)
                .WithMany(p => p.RespuestasUsuarios)
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
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
