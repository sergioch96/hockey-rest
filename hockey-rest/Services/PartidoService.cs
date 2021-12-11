using hockey_rest.Models.Common;
using hockey_rest.Util;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace hockey_rest.Services
{
    public class PartidoService : IPartidoService
    {
        #region querys

        /// <summary>
        /// obtiene los datos de un partido en particular
        /// </summary>
        public const string QRY_GET_PARTIDO = "SELECT p.id_partido, p.id_equipo_local, e1.equipo, p.id_equipo_visitante, e2.equipo, p.num_fecha, p.dia, p.hora, " +
                                                     "p.id_arbitro1, p1.nombre_apellido, p.id_arbitro2, p2.nombre_apellido, p.id_juez, p3.nombre_apellido, " +
                                                     "p.goles_local, p.goles_visitante, p.capitan_local, p.capitan_visitante, p.estado, es.estado_partido " +
                                              "FROM partido p " +
                                              "INNER JOIN equipo e1 on p.id_equipo_local = e1.id_equipo " +
                                              "INNER JOIN equipo e2 on p.id_equipo_visitante = e2.id_equipo " +
                                              "INNER JOIN estado_partido es on p.estado = es.id_estado_partido " +
                                              "LEFT JOIN persona p1 on p.id_arbitro1 = p1.id_persona " +
                                              "LEFT JOIN persona p2 on p.id_arbitro2 = p2.id_persona " +
                                              "LEFT JOIN persona p3 on p.id_juez = p3.id_persona " +
                                              "WHERE p.id_partido = @id_partido";

        /// <summary>
        /// obtiene todos los partidos de un campeonato
        /// </summary>
        private const string QRY_OBTENER_PARTIDOS = "SELECT p.id_partido, p.id_equipo_local, e1.equipo, p.id_equipo_visitante, e2.equipo, p.num_fecha, p.dia, p.hora, " +
                                                        "p.id_arbitro1, p1.nombre_apellido, p.id_arbitro2, p2.nombre_apellido, p.id_juez, p3.nombre_apellido, " +
                                                        "p.goles_local, p.goles_visitante, p.capitan_local, p.capitan_visitante, p.estado, es.estado_partido " +
                                                    "FROM partido p " +
                                                    "INNER JOIN equipo e1 on p.id_equipo_local = e1.id_equipo " +
                                                    "INNER JOIN equipo e2 on p.id_equipo_visitante = e2.id_equipo " +
                                                    "INNER JOIN estado_partido es on p.estado = es.id_estado_partido " +
                                                    "LEFT JOIN persona p1 on p.id_arbitro1 = p1.id_persona " +
                                                    "LEFT JOIN persona p2 on p.id_arbitro2 = p2.id_persona " +
                                                    "LEFT JOIN persona p3 on p.id_juez = p3.id_persona " +
                                                    "WHERE p.id_campeonato = @id_campeonato";

        /// <summary>
        /// Actualiza un partido a estado programado
        /// </summary>
        private const string QRY_PROGRAMAR_PARTIDO = "UPDATE partido " +
                                                     "SET id_arbitro1 = @id_arbitro1, id_arbitro2 = @id_arbitro2, " +
                                                         "id_juez = @id_juez, dia = @dia, hora = @hora, estado = 1 " +
                                                     "WHERE id_partido = @id_partido";

        /// <summary>
        /// Actualiza un partido a estado finalizado
        /// </summary>
        private const string QRY_CARGAR_PARTIDO = "UPDATE partido " +
                                                  "SET goles_local = @goles_local, goles_visitante = @goles_visitante, " +
                                                      "capitan_local = @capitan_local, capitan_visitante = @capitan_visitante, estado = 2 " +
                                                  "WHERE id_partido = @id_partido";

        /// <summary>
        /// Carga datos y estadisticas de un jugador en un partido
        /// </summary>
        private const string QRY_CARGAR_JUGADOR_PARTIDO = "INSERT INTO jugador_partido (id_jugador, id_partido, goles, tarjetas_verdes, tarjetas_amarillas, tarjetas_rojas, num_camiseta) " +
                                                          "VALUES (@id_jugador, @id_partido, @goles, @tarjetas_verdes, @tarjetas_amarillas, @tarjetas_rojas, @num_camiseta)";

        /// <summary>
        /// Recupera datos de un partido
        /// </summary>
        /// <param name="idPartido">Identificador del partido</param>
        /// <returns></returns>
        public PartidoDTO GetPartido(int idPartido)
        {
            PartidoDTO partido = new PartidoDTO();

            try
            {
                var result = SqlServerUtil.ExecuteQueryDataSet(QRY_GET_PARTIDO, SqlServerUtil.CreateParameter("id_partido", SqlDbType.Int, idPartido));

                if (result != null)
                {
                    foreach (DataRow item in result)
                    {
                        partido.IdPartido = int.Parse(item[0].ToString());
                        partido.IdEquipoLocal = int.Parse(item[1].ToString());
                        partido.EquipoLocal = item[2].ToString();
                        partido.IdEquipoVisitante = int.Parse(item[3].ToString());
                        partido.EquipoVisitante = item[4].ToString();
                        partido.FechaTorneo = item[5].ToString();
                        partido.Dia = !string.IsNullOrEmpty(item[6].ToString()) ? DateTime.Parse(item[6].ToString()).ToString("dd/MM/yyyy") : "";
                        partido.Hora = !string.IsNullOrEmpty(item[7].ToString()) ? item[7].ToString().Substring(0, 5) : "";
                        partido.IdArbitro1 = !string.IsNullOrEmpty(item[8].ToString()) ? int.Parse(item[8].ToString()) : 0;
                        partido.Arbitro1 = item[9].ToString();
                        partido.IdArbitro2 = !string.IsNullOrEmpty(item[10].ToString()) ? int.Parse(item[10].ToString()) : 0;
                        partido.Arbitro2 = item[11].ToString();
                        partido.IdJuez = !string.IsNullOrEmpty(item[12].ToString()) ? int.Parse(item[12].ToString()) : 0;
                        partido.Juez = item[13].ToString();
                        partido.GolesLocal = !string.IsNullOrEmpty(item[14].ToString()) ? int.Parse(item[14].ToString()) : 0;
                        partido.GolesVisitante = !string.IsNullOrEmpty(item[15].ToString()) ? int.Parse(item[15].ToString()) : 0;
                        partido.CapitanLocal = item[16].ToString();
                        partido.CapitanVisitante = item[17].ToString();
                        partido.IdEstado = int.Parse(item[18].ToString());
                        partido.Estado = item[19].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Ocurrió un error al intentar obtener datos del partido.");
            }

            return partido;
        }

        #endregion
        /// <summary>
        /// Obtiene todos los partidos del campeonato actual
        /// </summary>
        /// <returns></returns>
        public List<PartidoDTO> ObtenerPartidos()
        {
            List<PartidoDTO> partidos = new List<PartidoDTO>();

            try
            {
                string idCampeonato = HockeyUtil.ObtenerCampeonatoActivo();

                var result = SqlServerUtil.ExecuteQueryDataSet(QRY_OBTENER_PARTIDOS, SqlServerUtil.CreateParameter("id_campeonato", SqlDbType.Int, idCampeonato));

                foreach (DataRow item in result)
                {
                    partidos.Add(new PartidoDTO
                    {
                        IdPartido = int.Parse(item[0].ToString()),
                        IdEquipoLocal = int.Parse(item[1].ToString()),
                        EquipoLocal = item[2].ToString(),
                        IdEquipoVisitante = int.Parse(item[3].ToString()),
                        EquipoVisitante = item[4].ToString(),
                        FechaTorneo = item[5].ToString(),
                        Dia = !string.IsNullOrEmpty(item[6].ToString()) ? DateTime.Parse(item[6].ToString()).ToString("dd/MM/yyyy") : "",
                        Hora = !string.IsNullOrEmpty(item[7].ToString()) ? item[7].ToString().Substring(0, 5) : "",
                        IdArbitro1 = !string.IsNullOrEmpty(item[8].ToString()) ? int.Parse(item[8].ToString()) : 0,
                        Arbitro1 = item[9].ToString(),
                        IdArbitro2 = !string.IsNullOrEmpty(item[10].ToString()) ? int.Parse(item[10].ToString()) : 0,
                        Arbitro2 = item[11].ToString(),
                        IdJuez = !string.IsNullOrEmpty(item[12].ToString()) ? int.Parse(item[12].ToString()) : 0,
                        Juez = item[13].ToString(),
                        GolesLocal = !string.IsNullOrEmpty(item[14].ToString()) ? int.Parse(item[14].ToString()) : 0,
                        GolesVisitante = !string.IsNullOrEmpty(item[15].ToString()) ? int.Parse(item[15].ToString()) : 0,
                        CapitanLocal = item[16].ToString(),
                        CapitanVisitante = item[17].ToString(),
                        IdEstado = int.Parse(item[18].ToString()),
                        Estado = item[19].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al intentar obtener todos los partidos del campeonato.");
            }

            return partidos;
        }

        /// <summary>
        /// Método que actualiza un partido a estado programado
        /// </summary>
        public int ProgramarPartido(PartidoDTO partido)
        {
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>();

                parametros.Add(SqlServerUtil.CreateParameter("id_arbitro1", SqlDbType.Int, partido.IdArbitro1));
                parametros.Add(SqlServerUtil.CreateParameter("id_arbitro2", SqlDbType.Int, partido.IdArbitro2));
                parametros.Add(SqlServerUtil.CreateParameter("id_juez", SqlDbType.Int, partido.IdJuez));
                parametros.Add(SqlServerUtil.CreateParameter("dia", SqlDbType.Date, partido.Dia));
                parametros.Add(SqlServerUtil.CreateParameter("hora", SqlDbType.Time, partido.Hora));
                parametros.Add(SqlServerUtil.CreateParameter("id_partido", SqlDbType.Int, partido.IdPartido));

                var rowsAffected = SqlServerUtil.ExecuteNonQuery(QRY_PROGRAMAR_PARTIDO, parametros.ToArray());

                return rowsAffected;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al intentar actualizar partido a programado.");
            }
        }

        /// <summary>
        /// Método que finaliza un partido y carga los datos de los jugadores de cada equipo participante
        /// </summary>
        public int FinalizarPartido(PartidoDTO partido)
        {
            int rowsAffected = 0;
            string connString = SqlServerUtil.GetConnectionString();

            using (SqlConnection conn = new(connString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                
                try
                {
                    // actualiza el partido a estado finalizado
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Transaction = transaction;
                        cmd.CommandText = QRY_CARGAR_PARTIDO;
                        cmd.Parameters.Add(SqlServerUtil.CreateParameter("goles_local", SqlDbType.TinyInt, partido.GolesLocal));
                        cmd.Parameters.Add(SqlServerUtil.CreateParameter("goles_visitante", SqlDbType.TinyInt, partido.GolesVisitante));
                        cmd.Parameters.Add(SqlServerUtil.CreateParameter("capitan_local", SqlDbType.VarChar, partido.CapitanLocal));
                        cmd.Parameters.Add(SqlServerUtil.CreateParameter("capitan_visitante", SqlDbType.VarChar, partido.CapitanVisitante));
                        cmd.Parameters.Add(SqlServerUtil.CreateParameter("id_partido", SqlDbType.Int, partido.IdPartido));

                        rowsAffected = cmd.ExecuteNonQuery();
                    }

                    // carga estadisticas de jugadores participantes equipo local
                    foreach (var item in partido.JugadoresLocal)
                    {
                        using SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.Transaction = transaction;
                        cmd.CommandText = QRY_CARGAR_JUGADOR_PARTIDO;
                        cmd.Parameters.Add(SqlServerUtil.CreateParameter("id_jugador", SqlDbType.Int, item.IdPersona));
                        cmd.Parameters.Add(SqlServerUtil.CreateParameter("id_partido", SqlDbType.Int, partido.IdPartido));
                        cmd.Parameters.Add(SqlServerUtil.CreateParameter("goles", SqlDbType.TinyInt, item.Goles));
                        cmd.Parameters.Add(SqlServerUtil.CreateParameter("tarjetas_verdes", SqlDbType.TinyInt, item.TarjetasVerdes));
                        cmd.Parameters.Add(SqlServerUtil.CreateParameter("tarjetas_amarillas", SqlDbType.TinyInt, item.TarjetasAmarillas));
                        cmd.Parameters.Add(SqlServerUtil.CreateParameter("tarjetas_rojas", SqlDbType.TinyInt, item.TarjetasRojas));
                        cmd.Parameters.Add(SqlServerUtil.CreateParameter("num_camiseta", SqlDbType.TinyInt, item.NumeroCamiseta));

                        rowsAffected += cmd.ExecuteNonQuery();
                    }

                    // carga estadisticas de jugadores participantes equipo visitante
                    foreach (var item in partido.JugadoresVisitante)
                    {
                        using SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.Transaction = transaction;
                        cmd.CommandText = QRY_CARGAR_JUGADOR_PARTIDO;
                        cmd.Parameters.Add(SqlServerUtil.CreateParameter("id_jugador", SqlDbType.Int, item.IdPersona));
                        cmd.Parameters.Add(SqlServerUtil.CreateParameter("id_partido", SqlDbType.Int, partido.IdPartido));
                        cmd.Parameters.Add(SqlServerUtil.CreateParameter("goles", SqlDbType.TinyInt, item.Goles));
                        cmd.Parameters.Add(SqlServerUtil.CreateParameter("tarjetas_verdes", SqlDbType.TinyInt, item.TarjetasVerdes));
                        cmd.Parameters.Add(SqlServerUtil.CreateParameter("tarjetas_amarillas", SqlDbType.TinyInt, item.TarjetasAmarillas));
                        cmd.Parameters.Add(SqlServerUtil.CreateParameter("tarjetas_rojas", SqlDbType.TinyInt, item.TarjetasRojas));
                        cmd.Parameters.Add(SqlServerUtil.CreateParameter("num_camiseta", SqlDbType.TinyInt, item.NumeroCamiseta));

                        rowsAffected += cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Ocurrió un error al intentar finalizar un partido.");
                }
            }

            return rowsAffected;
        }
    }
}
