using hockey_rest.Models;
using hockey_rest.Models.Common;
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
                var idEquipo = _equipoService.AgregarEquipo(model);
                respuesta.Exito = EstadoRespuesta.Ok;
                respuesta.Mensaje = "Equipo agregado exitosamente.";
                respuesta.Data = idEquipo;
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
            List<ListaEquiposDTO> listaEquipos = new List<ListaEquiposDTO>();
            List<EquipoRequest> equiposParticipantes = new List<EquipoRequest>();

            try
            {
                listaEquipos = _equipoService.ObtenerTodosEquipos();
                equiposParticipantes = _equipoService.ObtenerEquiposParticipantes();

                foreach (var equipo in listaEquipos)
                {
                    var result = equiposParticipantes.Where(x => x.IdEquipo.Equals(equipo.IdEquipo)).FirstOrDefault();

                    if (result != null)
                        equipo.Estado = true;
                }

                respuesta.Exito = EstadoRespuesta.Ok;
                respuesta.Data = listaEquipos;

                //using (hockeydbContext db = new Models.hockeydbContext())
                //{
                //    var lst = db.Equipos.ToList();
                //    respuesta.Exito = EstadoRespuesta.Ok;
                //    respuesta.Data = lst;
                //}
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpGet("{id}")]
        public IActionResult GetEquipo(int id)
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                var equipo = _equipoService.GetEquipo(id);

                respuesta.Exito = EstadoRespuesta.Ok;
                respuesta.Data = equipo;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpGet("planillaJugadores/{idEquipo}")]
        public IActionResult GetJugadoresPartidoDisputado(int idEquipo)
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                var jugadores = _equipoService.ObtenerPlanillaJugadores(idEquipo);
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
