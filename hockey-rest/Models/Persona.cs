using System;
using System.Collections.Generic;

#nullable disable

namespace hockey_rest.Models
{
    public partial class Persona
    {
        public Persona()
        {
            EquipoIdAsistenteTecnicoNavigations = new HashSet<Equipo>();
            EquipoIdDirectorTecnicoNavigations = new HashSet<Equipo>();
            EquipoIdPreparadorFisicoNavigations = new HashSet<Equipo>();
            EquipoJugadors = new HashSet<EquipoJugador>();
            JugadorPartidos = new HashSet<JugadorPartido>();
            PartidoIdArbitro1Navigations = new HashSet<Partido>();
            PartidoIdArbitro2Navigations = new HashSet<Partido>();
            PartidoIdJuezNavigations = new HashSet<Partido>();
        }

        public int IdPersona { get; set; }
        public string NombreApellido { get; set; }
        public string NumDocumento { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public int IdRol { get; set; }

        public virtual Rol IdRolNavigation { get; set; }
        public virtual ICollection<Equipo> EquipoIdAsistenteTecnicoNavigations { get; set; }
        public virtual ICollection<Equipo> EquipoIdDirectorTecnicoNavigations { get; set; }
        public virtual ICollection<Equipo> EquipoIdPreparadorFisicoNavigations { get; set; }
        public virtual ICollection<EquipoJugador> EquipoJugadors { get; set; }
        public virtual ICollection<JugadorPartido> JugadorPartidos { get; set; }
        public virtual ICollection<Partido> PartidoIdArbitro1Navigations { get; set; }
        public virtual ICollection<Partido> PartidoIdArbitro2Navigations { get; set; }
        public virtual ICollection<Partido> PartidoIdJuezNavigations { get; set; }
    }
}
