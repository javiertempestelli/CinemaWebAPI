using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoCinema
{
    public class Pelicula
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
