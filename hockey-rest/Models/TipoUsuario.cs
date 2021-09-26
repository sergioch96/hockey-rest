using System;
using System.Collections.Generic;

#nullable disable

namespace hockey_rest.Models
{
    public partial class TipoUsuario
    {
        public TipoUsuario()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public int IdTipoUsuario { get; set; }
        public string TipoUser { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
