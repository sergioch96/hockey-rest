using System;
using System.Collections.Generic;

#nullable disable

namespace hockey_rest.Models
{
    public partial class TipoCampeonato
    {
        public TipoCampeonato()
        {
            Campeonatos = new HashSet<Campeonato>();
        }

        public int IdTipoCampeonato { get; set; }
        public string TipoTorneo { get; set; }

        public virtual ICollection<Campeonato> Campeonatos { get; set; }
    }
}
