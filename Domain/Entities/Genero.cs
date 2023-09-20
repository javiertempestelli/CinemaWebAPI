using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoCinema
{
    public class Genero
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GeneroId { get; set; }
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        public Genero() { }
        public ICollection<Pelicula> Peliculas { get; set; }
    }
}
