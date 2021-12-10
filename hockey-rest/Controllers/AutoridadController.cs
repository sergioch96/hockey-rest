using hockey_rest.Models;
using hockey_rest.Models.Constants;
using hockey_rest.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hockey_rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoridadController : ControllerBase
    {
        [HttpGet]
        public IActionResult ObtenerAutoridades()
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                using (hockeydbContext db = new hockeydbContext())
                {
                    var autoridades = db.Personas.Where(x => x.IdRol.Equals(TipoRol.Arbitro) || x.IdRol.Equals(TipoRol.JuezDeMesa)).ToList();
                    respuesta.Exito = EstadoRespuesta.Ok;
                    respuesta.Data = autoridades;
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }
    }
}
