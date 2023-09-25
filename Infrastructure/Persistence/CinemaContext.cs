using Microsoft.EntityFrameworkCore;
namespace CinemaWebAPI;



public class CinemaContext : DbContext
{
    public CinemaContext(DbContextOptions<CinemaContext> options) : base(options)
    {
    }

    public DbSet<Genero> Generos { get; set; }
    public DbSet<Pelicula> Peliculas { get; set; }
    public DbSet<Sala> Salas { get; set; }
    public DbSet<Funcion> Funciones { get; set; }
    public DbSet<Ticket> Tickets { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //// Configuraciones de modelos y relaciones
        modelBuilder.Entity<Genero>()
          .HasMany(g => g.Peliculas)
          .WithOne(p => p.Genero)
          .HasForeignKey(p => p.GeneroId);

        //// Relación entre Sala y Funcion (uno a muchos)
        modelBuilder.Entity<Sala>()
            .HasMany(s => s.Funciones)
            .WithOne(f => f.Sala)
            .HasForeignKey(f => f.SalaId);

        //// Relación entre Funcion y Pelicula (muchos a uno)
        modelBuilder.Entity<Funcion>()
            .HasOne(f => f.Pelicula)
            .WithMany(p => p.Funciones)
            .HasForeignKey(f => f.PeliculaId);

        // Relación entre Funcion y Ticket (uno a muchos)
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Funcion)
            .WithMany(f => f.Tickets)
            .HasForeignKey(t => t.FuncionId);

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=CineDB;Trusted_Connection=True;TrustServerCertificate=True");
    }

}

