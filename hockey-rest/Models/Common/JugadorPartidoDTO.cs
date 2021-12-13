using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hockey_rest.Models.Common
{
    public class JugadorPartidoDTO
    {
        public int IdPersona { get; set; }
        public string NombreApellido { get; set; }
        public int NumeroCamiseta { get; set; }
        public int Goles { get; set; }
        public int TarjetasVerdes { get; set; }
        public int TarjetasAmarillas { get; set; }
        public int TarjetasRojas { get; set; }
        public int PartidosSuspendidos { get; set; }
        public int PartidosJugados{ get; set; }
        public int IdEquipo { get; set; }
        public string Equipo { get; set; }
    }
}
