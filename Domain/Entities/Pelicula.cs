using System.ComponentModel.DataAnnotations;

namespace ProyectoCinema
{
    public class Pelicula
    {
        [Key]
        public int PeliculaId { get; set; }
        [Required]
        [StringLength(50)]
        public string Titulo { get; set; }
        [Required]
        [StringLength(255)]
        public string Sonopsis { get; set; }
        [Required]
        [StringLength(100)]
        public string Poster { get; set; }
        [Required]
        [StringLength(100)]
        public string Trailer { get; set; }
        [Required]
        public int GeneroId { get; set; }
        public Genero Genero { get; set; }
        public ICollection<Funcion> Funciones { get; set; }
    }
}
