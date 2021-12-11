using hockey_rest.Models.Constants;
using hockey_rest.Models.Response;
using hockey_rest.Services;
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
    public class CampeonatoController : ControllerBase
    {
        private ICampeonatoService _campeonatoService;

        public CampeonatoController(ICampeonatoService campeonatoService)
        {
            _campeonatoService = campeonatoService;
        }

        [HttpGet("tablaPosiciones")]
        public IActionResult ObtenerTablaPosiciones()
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                var tabla = _campeonatoService.GetTablaPosiciones();
                respuesta.Exito = EstadoRespuesta.Ok;
                respuesta.Data = tabla;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpGet("tablaGoleadores")]
        public IActionResult ObtenerTablaGoleadores()
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                var tabla = _campeonatoService.GetTablaGoleadores();
                respuesta.Exito = EstadoRespuesta.Ok;
                respuesta.Data = tabla;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpGet("tablaTarjetas")]
        public IActionResult ObtenerTablaAcumulacionTarjetas()
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                var tabla = _campeonatoService.GetTablaAcumulacionTarjetas();
                respuesta.Exito = EstadoRespuesta.Ok;
                respuesta.Data = tabla;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }
    }
}
