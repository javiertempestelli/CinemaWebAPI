using System.ComponentModel.DataAnnotations;

namespace ProyectoCinema
{
    public class Genero
    {
        [Key]
        public int GeneroId { get; set; }
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        public Genero() { }
        public ICollection<Pelicula> Peliculas { get; set; }
    }
}
