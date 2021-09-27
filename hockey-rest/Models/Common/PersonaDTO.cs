using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hockey_rest.Models.Common
{
    public class PersonaDTO
    {
        public int IdPersona { get; set; }
        public string NombreApellido { get; set; }
        public string NumDocumento { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public int IdRol { get; set; }
        public int IdEquipo { get; set; }
    }
}
