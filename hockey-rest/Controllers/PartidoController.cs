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
    public class PartidoController : ControllerBase
    {
        private IPartidoService _partidoService;

        public PartidoController(IPartidoService partidoService)
        {
            _partidoService = partidoService;
        }

        [HttpGet("{id}")]
        public IActionResult GetPartido(int id)
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                var partido = _partidoService.GetPartido(id);

                respuesta.Exito = EstadoRespuesta.Ok;
                respuesta.Data = partido;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpGet]
        public IActionResult ObtenerPartidos()
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                var partidos = _partidoService.ObtenerPartidos();
                respuesta.Exito = EstadoRespuesta.Ok;
                respuesta.Data = partidos;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpPut, Authorize]
        public IActionResult ProgramarPartido(PartidoDTO partido)
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                var partidos = _partidoService.ProgramarPartido(partido);
                respuesta.Exito = EstadoRespuesta.Ok;
                respuesta.Mensaje = "Partido programado correctamente";
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }
    }
}
