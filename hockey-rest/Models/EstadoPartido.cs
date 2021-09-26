using System;
using System.Collections.Generic;

#nullable disable

namespace hockey_rest.Models
{
    public partial class EstadoPartido
    {
        public EstadoPartido()
        {
            Partidos = new HashSet<Partido>();
        }

        public int IdEstadoPartido { get; set; }
        public string Estado { get; set; }

        public virtual ICollection<Partido> Partidos { get; set; }
    }
}
