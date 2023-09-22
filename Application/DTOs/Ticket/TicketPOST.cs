﻿using System.Text.Json.Serialization;

namespace Application.DTOs.Ticket
{
    public class TicketPOST
    {
        public Guid TicketId { get; set; }
        [JsonIgnore]
        public int FuncionId { get; set; }
        public string Usuario { get; set; }
    }

}
