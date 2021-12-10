using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hockey_rest.Models.Common
{
    public class PartidoDTO
    {
        public int IdPartido { get; set; }
        public string FechaTorneo { get; set; }
        public int IdEstado { get; set; }
        public string Estado { get; set; }
        public string Dia { get; set; }
        public string Hora { get; set; }
        public string EquipoLocal { get; set; }
        public int IdEquipoLocal { get; set; }
        public string EquipoVisitante { get; set; }
        public int IdEquipoVisitante { get; set; }
        public int GolesLocal { get; set; }
        public int GolesVisitante { get; set; }
        public string Arbitro1 { get; set; }
        public int IdArbitro1 { get; set; }
        public string Arbitro2 { get; set; }
        public int IdArbitro2 { get; set; }
        public string Juez { get; set; }
        public int IdJuez { get; set; }
        public string CapitanLocal { get; set; }
        public string CapitanVisitante { get; set; }
    }
}
