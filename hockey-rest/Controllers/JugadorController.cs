using hockey_rest.Models;
using hockey_rest.Models.Common;
using hockey_rest.Models.Constants;
using hockey_rest.Models.Response;
using hockey_rest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

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

        [HttpPut, Authorize]
        public IActionResult EditarJugador(Persona jugador)
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                using (hockeydbContext db = new hockeydbContext())
                {
                    db.Personas.Update(jugador);
                    db.SaveChanges();
                    respuesta.Exito = EstadoRespuesta.Ok;
                    respuesta.Mensaje = "Jugador modificado exitosamente.";
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpGet("{idEquipo}")]
        public IActionResult GetJugadoresPorEquipo(int idEquipo)
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                var jugadores = _jugadorService.GetJugadoresPorEquipo(idEquipo);
                respuesta.Exito = EstadoRespuesta.Ok;
                respuesta.Data = jugadores;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpGet("jugadorPlanilla/{idEquipo}")]
        public IActionResult GetJugadoresCargarPlanilla(int idEquipo)
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                var jugadores = _jugadorService.GetJugadoresCargarPlanilla(idEquipo);
                respuesta.Exito = EstadoRespuesta.Ok;
                respuesta.Data = jugadores;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpGet("jugadorPartidoDisputado/{idEquipo}/{idPartido}")]
        public IActionResult GetJugadoresPartidoDisputado(int idEquipo, int idPartido)
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                var jugadores = _jugadorService.GetJugadoresPartidoDisputado(idEquipo, idPartido);
                respuesta.Exito = EstadoRespuesta.Ok;
                respuesta.Data = jugadores;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }
    }
}
