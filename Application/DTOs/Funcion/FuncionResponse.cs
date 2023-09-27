namespace CinemaWebAPI.Application.DTOs
{
    public class FuncionResponse
    {
        public int FuncionId { get; set; }
        public DateTime Fecha { get; set; }
        public string Horario { get; set; }
        public PeliculaResponse Pelicula { get;  set; }
        public SalaResponse Sala { get;  set; }
    }

}