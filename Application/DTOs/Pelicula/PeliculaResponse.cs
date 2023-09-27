namespace CinemaWebAPI.Application.DTOs
{
    public class PeliculaResponse
    {
        public int PeliculaId { get; set; }
        public string Titulo { get; set; }
        public string Poster { get; set; }
        public string Trailer { get; set; }
        public string Sonopsis { get; set; }
        public GeneroResponse Genero { get; set; }
        public List<FuncionResponse> Funciones { get; set; }
    }
}