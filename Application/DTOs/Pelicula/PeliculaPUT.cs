using System.Text.Json.Serialization;

namespace CinemaWebAPI.Application.DTOs.Pelicula
{
    public class PeliculaPUT
    {
        [JsonIgnore]
        public int PeliculaId { get; set; }
        public string Titulo { get; set; }
        public string Poster { get; set; }
        public string Trailer { get; set; } // El nuevo tráiler de la película
        public string Sonopsis { get; set; } // La nueva sinopsis de la película
        public int Genero { get; set; }
    }

}