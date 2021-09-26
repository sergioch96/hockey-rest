using System;
using System.Collections.Generic;

#nullable disable

namespace hockey_rest.Models
{
    public partial class Usuario
    {
        public int IdUsuario { get; set; }
        public string User { get; set; }
        public string Pass { get; set; }
        public DateTime FechaAlta { get; set; }
        public string Activo { get; set; }
        public int IdTipoUsuario { get; set; }

        public virtual TipoUsuario IdTipoUsuarioNavigation { get; set; }
    }
}
