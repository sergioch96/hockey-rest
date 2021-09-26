using System;
using System.Collections.Generic;

#nullable disable

namespace hockey_rest.Models
{
    public partial class Rol
    {
        public Rol()
        {
            Personas = new HashSet<Persona>();
        }

        public int IdRol { get; set; }
        public string NombreRol { get; set; }

        public virtual ICollection<Persona> Personas { get; set; }
    }
}
