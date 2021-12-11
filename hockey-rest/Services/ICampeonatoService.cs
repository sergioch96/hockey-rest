using hockey_rest.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hockey_rest.Services
{
    public interface ICampeonatoService
    {
        public List<TablaPosicionesDTO> GetTablaPosiciones();

        public List<JugadorPartidoDTO> GetTablaGoleadores();

        public List<JugadorPartidoDTO> GetTablaAcumulacionTarjetas();
    }
}
