using System.Text.Json.Serialization;

namespace Application.DTOs.Funcion
{
    public class FuncionDTO
    {
        [JsonIgnore]
        public int FuncionId { get; set; }

        public int SalaId { get; set; }
        //        [JsonIgnore]

        //        public string SalaNombre { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Horario { get; set; }


        public int PeliculaId { get; set; }
        //        [JsonIgnore]
        //        public string PeliculaTitulo { get; set; }
    }
}
