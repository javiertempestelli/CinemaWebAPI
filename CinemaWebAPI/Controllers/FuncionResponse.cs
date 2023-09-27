namespace CinemaWebAPI.Controllers
{
    public class FuncionResponse
    {
        public int FuncionId { get; set; }
        public DateTime Fecha { get; set; }
        public string Horario { get; set; }
        public PeliculaResponse Pelicula { get; internal set; }
        public SalaResponse Sala { get; internal set; }
    }

}