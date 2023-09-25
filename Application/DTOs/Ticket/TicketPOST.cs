using System.Text.Json.Serialization;

namespace Application.DTOs.Ticket
{
    public class TicketPOST
    {
        public Guid TicketId { get; set; }
        public int FuncionId { get; set; }
        public string Usuario { get; set; }
    }

}
