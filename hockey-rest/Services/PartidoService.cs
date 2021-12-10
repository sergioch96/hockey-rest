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
        private const string QRY_PROGRAMAR_PARTIDOS = "UPDATE partido " +
                                                      "SET id_arbitro1 = @id_arbitro1, id_arbitro2 = @id_arbitro2, " +
                                                            "id_juez = @id_juez, dia = @dia, hora = @hora, estado = 1 " +
                                                      "WHERE id_partido = @id_partido";

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

                var rowsAffected = SqlServerUtil.ExecuteNonQuery(QRY_PROGRAMAR_PARTIDOS, parametros.ToArray());

                return rowsAffected;
            }
            catch (Exception)
            {
                throw new Exception("Ocurrió un error al intentar actualizar partido a programado.");
            }
        }
    }
}
