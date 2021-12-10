using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hockey_rest.Models.Response
{
    public class UserResponse
    {
        public string Usuario { get; set; }
        public string Token { get; set; }
        public int IdTipoUsuario { get; set; }
    }
}
