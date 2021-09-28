using System;
using System.Collections.Generic;

#nullable disable

namespace hockey_rest.Models
{
    public partial class Partido
    {
        public Partido()
        {
            JugadorPartidos = new HashSet<JugadorPartido>();
        }

        public int IdPartido { get; set; }
        public int IdCampeonato { get; set; }
        public int IdEquipoLocal { get; set; }
        public int IdEquipoVisitante { get; set; }
        public string NumFecha { get; set; }
        public DateTime? Dia { get; set; }
        public TimeSpan? Hora { get; set; }
        public int? IdArbitro1 { get; set; }
        public int? IdArbitro2 { get; set; }
        public int? IdJuez { get; set; }
        public byte? GolesLocal { get; set; }
        public byte? GolesVisitante { get; set; }
        public string CapitanLocal { get; set; }
        public string CapitanVisitante { get; set; }
        public int Estado { get; set; }

        public virtual EstadoPartido EstadoNavigation { get; set; }
        public virtual Persona IdArbitro1Navigation { get; set; }
        public virtual Persona IdArbitro2Navigation { get; set; }
        public virtual Campeonato IdCampeonatoNavigation { get; set; }
        public virtual Equipo IdEquipoLocalNavigation { get; set; }
        public virtual Equipo IdEquipoVisitanteNavigation { get; set; }
        public virtual Persona IdJuezNavigation { get; set; }
        public virtual ICollection<JugadorPartido> JugadorPartidos { get; set; }
    }
}
