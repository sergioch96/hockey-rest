using System;
using System.Collections.Generic;

#nullable disable

namespace hockey_rest.Models
{
    public partial class Equipo
    {
        public Equipo()
        {
            PartidoIdEquipoLocalNavigations = new HashSet<Partido>();
            PartidoIdEquipoVisitanteNavigations = new HashSet<Partido>();
        }

        public int IdEquipo { get; set; }
        public string NombreEquipo { get; set; }
        public int IdDirectorTecnico { get; set; }
        public int IdPreparadorFisico { get; set; }
        public int IdAsistenteTecnico { get; set; }
        public string Escudo { get; set; }

        public virtual Persona IdAsistenteTecnicoNavigation { get; set; }
        public virtual Persona IdDirectorTecnicoNavigation { get; set; }
        public virtual Persona IdPreparadorFisicoNavigation { get; set; }
        public virtual ICollection<Partido> PartidoIdEquipoLocalNavigations { get; set; }
        public virtual ICollection<Partido> PartidoIdEquipoVisitanteNavigations { get; set; }
    }
}
