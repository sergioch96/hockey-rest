using hockey_rest.Models;
using hockey_rest.Models.Constants;
using hockey_rest.Models.Request;
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
    public class EquipoController : ControllerBase
    {
        private IEquipoService _equipoService;

        public EquipoController(IEquipoService equipoService)
        {
            _equipoService = equipoService;
        }

        [HttpPost, Authorize]
        public IActionResult AgregarEquipo(EquipoRequest model)
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                _equipoService.AgregarEquipo(model);
                respuesta.Exito = EstadoRespuesta.Ok;
                respuesta.Mensaje = "Equipo agregado exitosamente.";
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpGet]
        public IActionResult ObtenerEquipos()
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                using (hockeydbContext db = new Models.hockeydbContext())
                {
                    var lst = db.Equipos.ToList();
                    respuesta.Exito = EstadoRespuesta.Ok;
                    respuesta.Data = lst;
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
