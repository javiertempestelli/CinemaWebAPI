namespace CinemaWebAPI.Application.DTOs.Pelicula
{
    public class PeliculaGET
    {
        public string Titulo { get; set; }
        public string Sonopsis { get; set; }
        public string Trailer { get; set; }
        public string Poster { get; set; }
        public int GeneroId { get; set; }
    }

}
