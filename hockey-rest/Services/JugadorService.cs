using hockey_rest.Models;
using hockey_rest.Models.Common;
using hockey_rest.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hockey_rest.Services
{
    public class JugadorService : IJugadorService
    {
        // Método que agrega un jugador nuevo a un equipo
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
    }
}
