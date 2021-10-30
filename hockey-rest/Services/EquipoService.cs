using hockey_rest.Models;
using hockey_rest.Models.Common;
using hockey_rest.Models.Constants;
using hockey_rest.Models.Request;
using hockey_rest.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace hockey_rest.Services
{
    public class EquipoService : IEquipoService
    {
        #region querys

        /// <summary>
        /// recupera todos los equipos participantes del campeonato activo
        /// </summary>
        private const string QRY_EQUIPOS_PARTICIPANTES= "SELECT e.id_equipo, e.equipo FROM campeonato_equipo ce " +
                                                        "INNER JOIN equipo e on ce.id_equipo = e.id_equipo " +
                                                        "WHERE ce.id_campeonato = @id_campeonato";

        /// <summary>
        /// recupera todos los equipos existentes
        /// </summary>
        private const string QRY_TODOS_EQUIPOS = "SELECT e.id_equipo, e.equipo, p1.nombre_apellido as DT, p2.nombre_apellido as AT, p3.nombre_apellido as PF FROM equipo e " +
                                                    "INNER JOIN persona p1 on e.id_director_tecnico = p1.id_persona " +
                                                    "INNER JOIN persona p2 on e.id_asistente_tecnico = p2.id_persona " +
                                                    "INNER JOIN persona p3 on e.id_preparador_fisico = p3.id_persona ";

        #endregion

        /// <summary>
        /// Método que almacena un equipo y su cuerpo técnico
        /// </summary>
        /// <param name="model"></param>
        public void AgregarEquipo(EquipoRequest model)
        {
            using (hockeydbContext db = new hockeydbContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        int idDT = 0;
                        int idAT = 0;
                        int idPF = 0;

                        foreach (var miembro in model.CuerpoTecnico)
                        {
                            var persona = new Persona
                            {
                                NombreApellido = miembro.NombreApellido,
                                NumDocumento = miembro.NumDocumento,
                                FechaNacimiento = miembro.FechaNacimiento,
                                Telefono = miembro.Telefono,
                                Email = miembro.Email,
                                IdRol = miembro.IdRol
                            };

                            db.Personas.Add(persona);
                            db.SaveChanges();

                            switch (persona.IdRol)
                            {
                                case TipoRol.DirectorTecnico:
                                    idDT = persona.IdPersona;
                                    break;
                                case TipoRol.AsistenteTecnico:
                                    idAT = persona.IdPersona;
                                    break;
                                case TipoRol.PreparadorFisico:
                                    idPF = persona.IdPersona;
                                    break;
                                default:
                                    break;
                            }
                        }

                        var equipo = new Equipo
                        {
                            NombreEquipo = model.NombreEquipo,
                            IdDirectorTecnico = idDT,
                            IdAsistenteTecnico = idAT,
                            IdPreparadorFisico = idPF,
                            Escudo = string.IsNullOrEmpty(model.Escudo) ? null : model.Escudo
                        };

                        db.Equipos.Add(equipo);
                        db.SaveChanges();

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw new Exception("Ocurrió un error al intentar guardar el equipo.");
                    }
                }
            }
        }

        /// <summary>
        /// Obtiene los equipos participantes del torneo actual
        /// </summary>
        /// <returns></returns>
        public List<EquipoRequest> ObtenerEquiposParticipantes()
        {
            List<EquipoRequest> listaEquipos = new List<EquipoRequest>();

            try
            {
                string idCampeonato = HockeyUtil.ObtenerCampeonatoActivo();

                var result = SqlServerUtil.ExecuteQueryDataSet(QRY_EQUIPOS_PARTICIPANTES, SqlServerUtil.CreateParameter("id_campeonato", System.Data.SqlDbType.Int, idCampeonato));

                foreach (DataRow item in result)
                {
                    listaEquipos.Add(new EquipoRequest
                    {
                        IdEquipo = int.Parse(item[0].ToString()),
                        NombreEquipo = item[1].ToString()
                    });
                }

            }
            catch (Exception)
            {

                throw;
            }

            return listaEquipos;
        }

        /// <summary>
        /// Obtiene todos los equipos existentes
        /// </summary>
        /// <returns></returns>
        public List<ListaEquiposDTO> ObtenerTodosEquipos()
        {
            List<ListaEquiposDTO> listaEquipos = new List<ListaEquiposDTO>();

            try
            {
                var result = SqlServerUtil.ExecuteQueryDataSet(QRY_TODOS_EQUIPOS);

                foreach (DataRow item in result)
                {
                    listaEquipos.Add(new ListaEquiposDTO
                    {
                        IdEquipo = int.Parse(item[0].ToString()),
                        NombreEquipo = item[1].ToString(),
                        DirectorTecnico = item[2].ToString(),
                        AsistenteTecnico = item[3].ToString(),
                        PreparadorFisico = item[4].ToString(),
                        Estado = false
                    });
                }
            }
            catch (Exception)
            {

                throw;
            }

            return listaEquipos;
        }
    }
}
