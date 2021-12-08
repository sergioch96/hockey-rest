using hockey_rest.Models;
using hockey_rest.Models.Common;
using hockey_rest.Models.Request;
using hockey_rest.Models.Response;
using hockey_rest.Util;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace hockey_rest.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        // Método de autenticación de usuario
        public UserResponse Auth(AuthRequest model)
        {
            try
            {
                UserResponse userResponse = new UserResponse();
                string sPassword = Encrypt.GetSHA256(model.Pass);

                using (var db = new hockeydbContext())
                {
                    var usuario = db.Usuarios.Where(d => d.User.Equals(model.User) && d.Pass.Equals(sPassword)).FirstOrDefault();

                    if (usuario == null) return null;

                    userResponse.Usuario = usuario.User;
                    userResponse.Token = GetToken(usuario);
                }

                return userResponse;
            }
            catch (Exception)
            {
                throw new Exception("Ocurrió un error al autenticar credenciales del usuario.");
            }
        }

        // Método que genera token para un usuario
        private string GetToken(Usuario usuario)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

                var llave = Encoding.ASCII.GetBytes(_appSettings.Secreto);
                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                        new Claim(ClaimTypes.Role, usuario.IdTipoUsuario.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(llave), 
                        SecurityAlgorithms.HmacSha256Signature
                    )
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            catch (Exception)
            {
                throw new Exception("Ocurrió un error al intentar generar el token para el usuario.");
            }
        }
    }
}
