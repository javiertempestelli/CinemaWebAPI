using System.ComponentModel.DataAnnotations;

namespace ProyectoCinema
{
    public class Sala
    {
        [Key]
        public int SalaId { get; set; }
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        [Required]
        public int Capacidad { get; set; }
        public ICollection<Funcion> Funciones { get; set; }
    }
}
