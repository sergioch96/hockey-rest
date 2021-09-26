using System;
using System.Collections.Generic;

#nullable disable

namespace hockey_rest.Models
{
    public partial class CampeonatoEquipo
    {
        public int IdCampeonato { get; set; }
        public int IdEquipo { get; set; }
        public byte? Puntos { get; set; }
        public byte? PartidosGanados { get; set; }
        public byte? PartidosEmpatados { get; set; }
        public byte? PartidosPerdidos { get; set; }
        public short? GolesFavor { get; set; }
        public short? GolesContra { get; set; }

        public virtual Campeonato IdCampeonatoNavigation { get; set; }
        public virtual Equipo IdEquipoNavigation { get; set; }
    }
}
