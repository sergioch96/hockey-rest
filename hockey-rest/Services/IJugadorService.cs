using hockey_rest.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hockey_rest.Services
{
    public interface IJugadorService
    {
        public void AgregarJugador(PersonaDTO jugador);
    }
}
