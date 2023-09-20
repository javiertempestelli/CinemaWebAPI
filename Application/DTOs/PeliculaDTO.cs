using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PeliculaDTO
    {
        [JsonIgnore]
        public int PeliculaId { get; set; }
        public string Sonopsis { get; set; }
        public string Titulo { get; set; }
        public string Trailer { get; set; }
        public string Poster {  get; set; }
        [JsonIgnore]
        public int GeneroId { get; set; }
    }

}
