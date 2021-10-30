using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hockey_rest.Models.Common
{
    public class ListaEquiposDTO
    {
        public int IdEquipo { get; set; }
        public string NombreEquipo { get; set; }
        public string DirectorTecnico { get; set; }
        public string AsistenteTecnico { get; set; }
        public string PreparadorFisico { get; set; }
        public bool Estado { get; set; }
    }
}
