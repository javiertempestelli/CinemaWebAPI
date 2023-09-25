using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaWebAPI
{
    public class Funcion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FuncionId { get; set; }
        [Required]
        public int SalaId { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        [Required]
        public DateTime Horario { get; set; }
        [Required]
        public Sala Sala { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
        public int PeliculaId { get; set; }
        public Pelicula Pelicula { get; set; }
    }
}
