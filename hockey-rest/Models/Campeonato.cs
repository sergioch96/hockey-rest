using System;
using System.Collections.Generic;

#nullable disable

namespace hockey_rest.Models
{
    public partial class Campeonato
    {
        public Campeonato()
        {
            Partidos = new HashSet<Partido>();
        }

        public int IdCampeonato { get; set; }
        public int IdTipoCampeonato { get; set; }
        public int Anho { get; set; }
        public string Activo { get; set; }

        public virtual TipoCampeonato IdTipoCampeonatoNavigation { get; set; }
        public virtual ICollection<Partido> Partidos { get; set; }
    }
}
