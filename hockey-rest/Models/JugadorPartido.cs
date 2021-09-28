using System;
using System.Collections.Generic;

#nullable disable

namespace hockey_rest.Models
{
    public partial class JugadorPartido
    {
        public int IdJugadorPartido { get; set; }
        public int IdJugador { get; set; }
        public int IdPartido { get; set; }
        public byte? Goles { get; set; }
        public byte? TarjetasVerdes { get; set; }
        public byte? TarjetasAmarillas { get; set; }
        public byte? TarjetasRojas { get; set; }
        public byte NumCamiseta { get; set; }

        public virtual Persona IdJugadorNavigation { get; set; }
        public virtual Partido IdPartidoNavigation { get; set; }
    }
}
