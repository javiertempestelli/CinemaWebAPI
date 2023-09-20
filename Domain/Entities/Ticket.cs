using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoCinema
{
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TicketId { get; set; }
        [Required]
        [StringLength(50)]
        public string Usuario { get; set; }
        public int FuncionId { get; set; }
        public Funcion Funcion { get; set; }


    }
}
