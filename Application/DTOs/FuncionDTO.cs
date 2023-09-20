using ProyectoCinema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class FuncionDTO
    {
        [JsonIgnore]
        public int FuncionId { get; set; }
        public int SalaId { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Horario { get; set; }
        public int PeliculaId { get; set; }
    }
}
