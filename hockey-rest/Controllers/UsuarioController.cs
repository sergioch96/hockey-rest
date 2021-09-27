using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hockey_rest.Models;
using hockey_rest.Services;
using hockey_rest.Models.Request;
using hockey_rest.Models.Response;
using hockey_rest.Models.Constants;

namespace hockey_rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private IUserService _userService;

        public UsuarioController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Autentificar([FromBody] AuthRequest model)
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                var userResponse = _userService.Auth(model);

                if (userResponse == null)
                {
                    respuesta.Mensaje = "Usuario o contraseña incorrecta";
                    return Ok(respuesta);
                }

                respuesta.Exito = EstadoRespuesta.Ok;
                respuesta.Data = userResponse;

            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }
    }
}
