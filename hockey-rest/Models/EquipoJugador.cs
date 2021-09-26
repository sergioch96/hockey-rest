using System;
using System.Collections.Generic;

#nullable disable

namespace hockey_rest.Models
{
    public partial class EquipoJugador
    {
        public int IdEquipo { get; set; }
        public int IdJugador { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public byte? AcumulaTarjVerde { get; set; }
        public byte? AcumulaTarjAmarilla { get; set; }
        public byte? AcumulaTarjRoja { get; set; }
        public byte? PartidosSuspendidos { get; set; }

        public virtual Equipo IdEquipoNavigation { get; set; }
        public virtual Persona IdJugadorNavigation { get; set; }
    }
}
