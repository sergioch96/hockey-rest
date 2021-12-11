using hockey_rest.Models;
using hockey_rest.Models.Common;
using hockey_rest.Models.Constants;
using hockey_rest.Util;
using Microsoft.Data.SqlClient;
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
        /// <summary>
        /// Obtiene jugadores para cargar en planilla
        /// </summary>
        private const string QRY_GET_JUGADORES_CARGAR_PLANILLA = "SELECT j.id_persona, j.nombre_apellido, eq.partidos_suspendidos from persona j " +
                                                                 "INNER JOIN equipo_jugador eq on j.id_persona = eq.id_jugador " +
                                                                 "INNER JOIN equipo e on eq.id_equipo = e.id_equipo " +
                                                                 "WHERE e.id_equipo = @id_equipo";

        /// <summary>
        /// Recupera los jugadores de un equipo que disputaron un partido especifico
        /// </summary>
        private const string QRY_GET_JUGADORES_PARTIDO_DISPUTADO = "SELECT jp.num_camiseta, pe.nombre_apellido, jp.goles, " +
                                                                   "jp.tarjetas_verdes, jp.tarjetas_amarillas, jp.tarjetas_rojas, eq.id_equipo " +
                                                                   "FROM jugador_partido jp " +
                                                                   "INNER JOIN persona pe on jp.id_jugador = pe.id_persona " +
                                                                   "INNER JOIN equipo_jugador ej on ej.id_jugador = pe.id_persona " +
                                                                   "INNER JOIN equipo eq on eq.id_equipo = ej.id_equipo " +
                                                                   "WHERE eq.id_equipo = @id_equipo AND jp.id_partido = @id_partido";

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

        /// <summary>
        /// Obtiene lista de jugadores de un equipo para cargar planilla
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <returns>Lista de jugadores</returns>
        public List<JugadorPartidoDTO> GetJugadoresCargarPlanilla(int idEquipo)
        {
            List<JugadorPartidoDTO> jugadores = new List<JugadorPartidoDTO>();

            try
            {
                var result = SqlServerUtil.ExecuteQueryDataSet(QRY_GET_JUGADORES_CARGAR_PLANILLA, SqlServerUtil.CreateParameter("id_equipo", SqlDbType.Int, idEquipo));

                if (result != null)
                {
                    foreach (DataRow item in result)
                    {
                        jugadores.Add(new JugadorPartidoDTO
                        {
                            IdPersona = int.Parse(item[0].ToString()),
                            NombreApellido = item[1].ToString(),
                            PartidosSuspendidos = !string.IsNullOrEmpty(item[2].ToString()) ? int.Parse(item[2].ToString()) : 0
                        });
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Ocurrió un error al recuperar jugadores de un equipo para cargar la planilla.");
            }

            return jugadores;
        }

        /// <summary>
        /// Obtiene lista de jugadores de un equipo que disputaron un partido especifico
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <returns>Lista de jugadores</returns>
        public List<JugadorPartidoDTO> GetJugadoresPartidoDisputado(int idEquipo, int idPartido)
        {
            List<JugadorPartidoDTO> jugadores = new List<JugadorPartidoDTO>();

            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>();

                parametros.Add(SqlServerUtil.CreateParameter("id_equipo", SqlDbType.Int, idEquipo));
                parametros.Add(SqlServerUtil.CreateParameter("id_partido", SqlDbType.Int, idPartido));

                var result = SqlServerUtil.ExecuteQueryDataSet(QRY_GET_JUGADORES_PARTIDO_DISPUTADO, parametros.ToArray());

                if (result != null)
                {
                    foreach (DataRow item in result)
                    {
                        jugadores.Add(new JugadorPartidoDTO
                        {
                            NumeroCamiseta = int.Parse(item[0].ToString()),
                            NombreApellido = item[1].ToString(),
                            Goles = !string.IsNullOrEmpty(item[2].ToString()) ? int.Parse(item[2].ToString()) : 0,
                            TarjetasVerdes = !string.IsNullOrEmpty(item[3].ToString()) ? int.Parse(item[3].ToString()) : 0,
                            TarjetasAmarillas = !string.IsNullOrEmpty(item[4].ToString()) ? int.Parse(item[4].ToString()) : 0,
                            TarjetasRojas = !string.IsNullOrEmpty(item[5].ToString()) ? int.Parse(item[5].ToString()) : 0,
                            IdEquipo = int.Parse(item[0].ToString())
                        });
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Ocurrió un error al recuperar jugadores que disputaron un partido.");
            }

            return jugadores;
        }
    }
}
