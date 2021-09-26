using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hockey_rest.Models.Request
{
    public class AuthRequest
    {
        [Required]
        public string User { get; set; }
        [Required]
        public string Pass { get; set; }
    }
}
