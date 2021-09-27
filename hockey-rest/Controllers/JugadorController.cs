using hockey_rest.Models.Common;
using hockey_rest.Models.Constants;
using hockey_rest.Models.Response;
using hockey_rest.Services;
using Microsoft.AspNetCore.Authorization;
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
    public class JugadorController : ControllerBase
    {
        private IJugadorService _jugadorService;

        public JugadorController(IJugadorService jugadorService)
        {
            _jugadorService = jugadorService;
        }

        [HttpPost, Authorize]
        public IActionResult AgregarJugador(PersonaDTO jugador)
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                _jugadorService.AgregarJugador(jugador);
                respuesta.Exito = EstadoRespuesta.Ok;
                respuesta.Mensaje = "Jugador agregado exitosamente.";
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }
    }
}
