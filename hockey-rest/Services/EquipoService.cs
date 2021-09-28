using hockey_rest.Models;
using hockey_rest.Models.Constants;
using hockey_rest.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hockey_rest.Services
{
    public class EquipoService : IEquipoService
    {
        // Método que almacena un equipo y su cuerpo técnico
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
    }
}
