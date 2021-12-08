using hockey_rest.Models;
using hockey_rest.Models.Common;
using hockey_rest.Models.Constants;
using hockey_rest.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace hockey_rest.Services
{
    public class JugadorService : IJugadorService
    {
        #region querys

        /// <summary>
        /// obtiene jugadores por un id_equipo
        /// </summary>
        private const string QRY_GET_JUGADORES_POR_EQUIPO = "SELECT j.id_persona, j.nombre_apellido, j.num_documento, j.fecha_nacimiento, j.telefono, j.email FROM persona j " +
                                                            "INNER JOIN equipo_jugador eq on j.id_persona = eq.id_jugador " +
                                                            "INNER JOIN equipo e on eq.id_equipo = e.id_equipo " +
                                                            "WHERE e.id_equipo = @id_equipo";

        #endregion

        /// <summary>
        /// Método que agrega un jugador a un equipo
        /// </summary>
        /// <param name="jugador"></param>
        public void AgregarJugador(PersonaDTO jugador)
        {
            using (hockeydbContext db = new hockeydbContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var persona = new Persona
                        {
                            NombreApellido = jugador.NombreApellido,
                            NumDocumento = jugador.NumDocumento,
                            FechaNacimiento = jugador.FechaNacimiento,
                            Telefono = jugador.Telefono,
                            Email = jugador.Email,
                            IdRol = TipoRol.Jugador
                        };

                        db.Personas.Add(persona);
                        db.SaveChanges();

                        var equipoJugador = new EquipoJugador
                        {
                            IdEquipo = jugador.IdEquipo,
                            IdJugador = persona.IdPersona,
                            FechaInicio = DateTime.Now.Date,
                            AcumulaTarjVerde = 0,
                            AcumulaTarjAmarilla = 0,
                            AcumulaTarjRoja = 0,
                            PartidosSuspendidos = 0
                        };

                        db.EquipoJugadors.Add(equipoJugador);
                        db.SaveChanges();

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw new Exception("Ocurrió un error al intentar agregar el jugador a un equipo.");
                    }
                }
            }
        }

        /// <summary>
        /// Método que obtiene los jugadores de un equipo
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <returns>Lista de jugadores</returns>
        public List<PersonaDTO> GetJugadoresPorEquipo(int idEquipo)
        {
            List<PersonaDTO> jugadores = new List<PersonaDTO>();

            try
            {
                var result = SqlServerUtil.ExecuteQueryDataSet(QRY_GET_JUGADORES_POR_EQUIPO, SqlServerUtil.CreateParameter("id_equipo", SqlDbType.Int, idEquipo));

                if (result != null)
                {
                    foreach (DataRow item in result)
                    {
                        jugadores.Add(new PersonaDTO
                        {
                            IdPersona = int.Parse(item[0].ToString()),
                            NombreApellido = item[1].ToString(),
                            NumDocumento = item[2].ToString(),
                            FechaNacimiento = DateTime.Parse(item[3].ToString()),
                            Telefono = item[4].ToString(),
                            Email = item[5].ToString()
                        });
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Ocurrió un error al intentar obtener los jugadores del equipo.");
            }

            return jugadores;
        }
    }
}
