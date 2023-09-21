using System.Text.Json.Serialization;

namespace Application.DTOs.Pelicula
{
    public class PeliculaGET
    {
        [JsonIgnore]
        public int PeliculaId { get; set; }
        public string Titulo { get; set; }
        public string Sonopsis { get; set; }
        public string Trailer { get; set; }
        public string Poster { get; set; }
        [JsonIgnore]
        public int GeneroId { get; set; }
        public string GeneroNombre { get; set; }
    }

}
