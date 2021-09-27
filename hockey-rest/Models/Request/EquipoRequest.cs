using hockey_rest.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hockey_rest.Models.Request
{
    public class EquipoRequest
    {
        public int IdEquipo { get; set; }
        public string NombreEquipo { get; set; }
        public List<PersonaDTO> CuerpoTecnico { get; set; }
        public string Escudo { get; set; }
    }
}
