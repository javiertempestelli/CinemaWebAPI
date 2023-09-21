using System.Text.Json.Serialization;

namespace Application.DTOs
{
    public class FuncionDTO
    {
        [JsonIgnore]
        public int FuncionId { get; set; }

        [JsonIgnore]
        public int SalaId { get; set; }
        public string SalaNombre { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Horario { get; set; }

        [JsonIgnore]
        public int PeliculaId { get; set; }
        public string PeliculaTitulo { get; set; }
    }
}
