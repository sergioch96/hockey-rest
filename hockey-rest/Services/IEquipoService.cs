using hockey_rest.Models.Common;
using hockey_rest.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hockey_rest.Services
{
    public interface IEquipoService
    {
        public void AgregarEquipo(EquipoRequest model);

        public List<EquipoRequest> ObtenerEquiposParticipantes();

        public List<ListaEquiposDTO> ObtenerTodosEquipos();
    }
}
