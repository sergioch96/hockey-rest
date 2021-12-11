using hockey_rest.Models.Common;
using hockey_rest.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace hockey_rest.Services
{
    public class CampeonatoService : ICampeonatoService
    {
        #region querys

        /// <summary>
        /// obtiene la tabla de posiciones del campeonato actual
        /// </summary>
        private const string QRY_GET_TABLA_POSICIONES = "SELECT eq.id_equipo, eq.equipo, ce.puntos, (ce.partidos_ganados + ce.partidos_perdidos + ce.partidos_empatados) as partidos_jugados, " + 
                                                               "ce.partidos_ganados, ce.partidos_empatados, ce.partidos_perdidos, ce.goles_favor, " +
                                                               "ce.goles_contra, (ce.goles_favor - ce.goles_contra) as diferencia_goles " +
                                                        "FROM campeonato_equipo ce " +
                                                        "INNER JOIN equipo eq on ce.id_equipo = eq.id_equipo " +
                                                        "WHERE ce.id_campeonato = @id_campeonato " +
                                                        "ORDER BY puntos desc, diferencia_goles desc, goles_favor desc";
        /// <summary>
        /// obtiene la tabla de goleadores del campeonato actual
        /// </summary>
        private const string QRY_GET_TABLA_GOLEADORES = "SELECT pe.nombre_apellido, eq.equipo, sum(jp.goles) as goles " +
                                                        "FROM equipo eq " +
                                                        "INNER JOIN equipo_jugador ej on eq.id_equipo = ej.id_equipo " +
                                                        "INNER JOIN persona pe on ej.id_jugador = pe.id_persona " +
                                                        "INNER JOIN jugador_partido jp on pe.id_persona = jp.id_jugador " +
                                                        "INNER JOIN partido pa on jp.id_partido = pa.id_partido " +
                                                        "WHERE pa.id_campeonato = @id_campeonato and goles > 0 " +
                                                        "GROUP BY pe.nombre_apellido, eq.equipo " +
                                                        "ORDER BY goles desc";

        /// <summary>
        /// obtiene la tabla de acumulacion de tarjetas del campeonato actual
        /// </summary>
        private const string QRY_GET_TABLA_ACUMULACION_TARJETAS = "SELECT pe.nombre_apellido, eq.equipo, sum(ej.acumula_tarj_verde) as tarjetas_verdes, " +
                                                                         "sum(ej.acumula_tarj_amarilla) as tarjetas_amarillas, sum(ej.acumula_tarj_roja) as tarjetas_rojas " +
                                                                  "FROM persona pe " +
                                                                  "INNER JOIN equipo_jugador ej on ej.id_jugador = pe.id_persona " +
                                                                  "INNER JOIN equipo eq on eq.id_equipo = ej.id_equipo " +
                                                                  "INNER JOIN campeonato_equipo ce on ce.id_equipo = eq.id_equipo " +
                                                                  "WHERE ce.id_campeonato =  @id_campeonato and (ej.acumula_tarj_verde > 0 or ej.acumula_tarj_amarilla > 0 or ej.acumula_tarj_roja > 0) " +
                                                                  "GROUP BY pe.nombre_apellido, eq.equipo " +
                                                                  "ORDER BY tarjetas_rojas desc, tarjetas_amarillas desc, tarjetas_verdes desc;";

        #endregion

        /// <summary>
        /// Recupera la tabla de posiciones del campeonato actual
        /// </summary>
        /// <returns></returns>
        public List<TablaPosicionesDTO> GetTablaPosiciones()
        {
            List<TablaPosicionesDTO> tablaPosiciones = new List<TablaPosicionesDTO>();

            try
            {
                string idCampeonato = HockeyUtil.ObtenerCampeonatoActivo();

                var result = SqlServerUtil.ExecuteQueryDataSet(QRY_GET_TABLA_POSICIONES, SqlServerUtil.CreateParameter("id_campeonato", SqlDbType.Int, idCampeonato));

                if (result != null)
                {
                    foreach (DataRow item in result)
                    {
                        tablaPosiciones.Add(new TablaPosicionesDTO
                        {
                            IdEquipo = int.Parse(item[0].ToString()),
                            Equipo = item[1].ToString(),
                            Puntos = int.Parse(item[2].ToString()),
                            PartidosJugados = int.Parse(item[3].ToString()),
                            PartidosGanados = int.Parse(item[4].ToString()),
                            PartidosEmpatados = int.Parse(item[5].ToString()),
                            PartidosPerdidos = int.Parse(item[6].ToString()),
                            GolesFavor = int.Parse(item[7].ToString()),
                            GolesContra = int.Parse(item[8].ToString()),
                            DiferenciaGoles = int.Parse(item[9].ToString())
                        });
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Ocurrió un error al intentar obtener la tabla de posiciones.");
            }

            return tablaPosiciones;
        }

        /// <summary>
        /// Recupera la tabla de goleadores del campeonato actual
        /// </summary>
        /// <returns></returns>
        public List<JugadorPartidoDTO> GetTablaGoleadores()
        {
            List<JugadorPartidoDTO> tablaGoleadores = new List<JugadorPartidoDTO>();

            try
            {
                string idCampeonato = HockeyUtil.ObtenerCampeonatoActivo();

                var result = SqlServerUtil.ExecuteQueryDataSet(QRY_GET_TABLA_GOLEADORES, SqlServerUtil.CreateParameter("id_campeonato", SqlDbType.Int, idCampeonato));

                if (result != null)
                {
                    foreach (DataRow item in result)
                    {
                        tablaGoleadores.Add(new JugadorPartidoDTO
                        {
                            NombreApellido = item[0].ToString(),
                            Equipo = item[1].ToString(),
                            Goles = int.Parse(item[2].ToString())
                        });
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Ocurrió un error al intentar obtener la tabla de goleadores.");
            }

            return tablaGoleadores;
        }

        /// <summary>
        /// Recupera la tabla de acumulacion de tarjetas del campeonato actual
        /// </summary>
        /// <returns></returns>
        public List<JugadorPartidoDTO> GetTablaAcumulacionTarjetas()
        {
            List<JugadorPartidoDTO> tablaTarjetas = new List<JugadorPartidoDTO>();

            try
            {
                string idCampeonato = HockeyUtil.ObtenerCampeonatoActivo();

                var result = SqlServerUtil.ExecuteQueryDataSet(QRY_GET_TABLA_ACUMULACION_TARJETAS, SqlServerUtil.CreateParameter("id_campeonato", SqlDbType.Int, idCampeonato));

                if (result != null)
                {
                    foreach (DataRow item in result)
                    {
                        tablaTarjetas.Add(new JugadorPartidoDTO
                        {
                            NombreApellido = item[0].ToString(),
                            Equipo = item[1].ToString(),
                            TarjetasVerdes = int.Parse(item[2].ToString()),
                            TarjetasAmarillas = int.Parse(item[3].ToString()),
                            TarjetasRojas = int.Parse(item[4].ToString())
                        });
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Ocurrió un error al intentar obtener la tabla de goleadores.");
            }

            return tablaTarjetas;
        }
    }
}
