using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace hockey_rest.Models
{
    public partial class hockeydbContext : DbContext
    {
        public hockeydbContext()
        {
        }

        public hockeydbContext(DbContextOptions<hockeydbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Campeonato> Campeonatos { get; set; }
        public virtual DbSet<CampeonatoEquipo> CampeonatoEquipos { get; set; }
        public virtual DbSet<Equipo> Equipos { get; set; }
        public virtual DbSet<EquipoJugador> EquipoJugadors { get; set; }
        public virtual DbSet<EstadoPartido> EstadoPartidos { get; set; }
        public virtual DbSet<JugadorPartido> JugadorPartidos { get; set; }
        public virtual DbSet<Partido> Partidos { get; set; }
        public virtual DbSet<Persona> Personas { get; set; }
        public virtual DbSet<Rol> Rols { get; set; }
        public virtual DbSet<TipoCampeonato> TipoCampeonatos { get; set; }
        public virtual DbSet<TipoUsuario> TipoUsuarios { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DevConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Campeonato>(entity =>
            {
                entity.HasKey(e => e.IdCampeonato)
                    .HasName("PK__campeona__2AAD39BD11158C24");

                entity.ToTable("campeonato");

                entity.Property(e => e.IdCampeonato).HasColumnName("id_campeonato");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("activo")
                    .IsFixedLength(true);

                entity.Property(e => e.Anho).HasColumnName("anho");

                entity.Property(e => e.IdTipoCampeonato).HasColumnName("id_tipo_campeonato");

                entity.HasOne(d => d.IdTipoCampeonatoNavigation)
                    .WithMany(p => p.Campeonatos)
                    .HasForeignKey(d => d.IdTipoCampeonato)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__campeonat__id_ti__4222D4EF");
            });

            modelBuilder.Entity<CampeonatoEquipo>(entity =>
            {
                entity.HasKey(e => e.IdCampeonatoEquipo)
                    .HasName("PK__campeona__611C165567922531");

                entity.ToTable("campeonato_equipo");

                entity.Property(e => e.IdCampeonatoEquipo).HasColumnName("id_campeonato_equipo");

                entity.Property(e => e.GolesContra).HasColumnName("goles_contra");

                entity.Property(e => e.GolesFavor).HasColumnName("goles_favor");

                entity.Property(e => e.IdCampeonato).HasColumnName("id_campeonato");

                entity.Property(e => e.IdEquipo).HasColumnName("id_equipo");

                entity.Property(e => e.PartidosEmpatados).HasColumnName("partidos_empatados");

                entity.Property(e => e.PartidosGanados).HasColumnName("partidos_ganados");

                entity.Property(e => e.PartidosPerdidos).HasColumnName("partidos_perdidos");

                entity.Property(e => e.Puntos).HasColumnName("puntos");

                entity.HasOne(d => d.IdCampeonatoNavigation)
                    .WithMany(p => p.CampeonatoEquipos)
                    .HasForeignKey(d => d.IdCampeonato)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__campeonat__id_ca__31B762FC");

                entity.HasOne(d => d.IdEquipoNavigation)
                    .WithMany(p => p.CampeonatoEquipos)
                    .HasForeignKey(d => d.IdEquipo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__campeonat__id_eq__32AB8735");
            });

            modelBuilder.Entity<Equipo>(entity =>
            {
                entity.HasKey(e => e.IdEquipo)
                    .HasName("PK__equipo__EE01F88A174631B8");

                entity.ToTable("equipo");

                entity.Property(e => e.IdEquipo).HasColumnName("id_equipo");

                entity.Property(e => e.NombreEquipo)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("equipo");

                entity.Property(e => e.Escudo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("escudo");

                entity.Property(e => e.IdAsistenteTecnico).HasColumnName("id_asistente_tecnico");

                entity.Property(e => e.IdDirectorTecnico).HasColumnName("id_director_tecnico");

                entity.Property(e => e.IdPreparadorFisico).HasColumnName("id_preparador_fisico");

                entity.HasOne(d => d.IdAsistenteTecnicoNavigation)
                    .WithMany(p => p.EquipoIdAsistenteTecnicoNavigations)
                    .HasForeignKey(d => d.IdAsistenteTecnico)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__equipo__id_asist__3D5E1FD2");

                entity.HasOne(d => d.IdDirectorTecnicoNavigation)
                    .WithMany(p => p.EquipoIdDirectorTecnicoNavigations)
                    .HasForeignKey(d => d.IdDirectorTecnico)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__equipo__id_direc__3B75D760");

                entity.HasOne(d => d.IdPreparadorFisicoNavigation)
                    .WithMany(p => p.EquipoIdPreparadorFisicoNavigations)
                    .HasForeignKey(d => d.IdPreparadorFisico)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__equipo__id_prepa__3C69FB99");
            });

            modelBuilder.Entity<EquipoJugador>(entity =>
            {
                entity.HasKey(e => e.IdEquipoJugador)
                    .HasName("PK__equipo_j__A0E8AB799F30E7BA");

                entity.ToTable("equipo_jugador");

                entity.Property(e => e.IdEquipoJugador).HasColumnName("id_equipo_jugador");

                entity.Property(e => e.AcumulaTarjAmarilla).HasColumnName("acumula_tarj_amarilla");

                entity.Property(e => e.AcumulaTarjRoja).HasColumnName("acumula_tarj_roja");

                entity.Property(e => e.AcumulaTarjVerde).HasColumnName("acumula_tarj_verde");

                entity.Property(e => e.FechaFin)
                    .HasColumnType("date")
                    .HasColumnName("fecha_fin");

                entity.Property(e => e.FechaInicio)
                    .HasColumnType("date")
                    .HasColumnName("fecha_inicio");

                entity.Property(e => e.IdEquipo).HasColumnName("id_equipo");

                entity.Property(e => e.IdJugador).HasColumnName("id_jugador");

                entity.Property(e => e.PartidosSuspendidos).HasColumnName("partidos_suspendidos");

                entity.HasOne(d => d.IdEquipoNavigation)
                    .WithMany(p => p.EquipoJugadors)
                    .HasForeignKey(d => d.IdEquipo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__equipo_ju__id_eq__2A164134");

                entity.HasOne(d => d.IdJugadorNavigation)
                    .WithMany(p => p.EquipoJugadors)
                    .HasForeignKey(d => d.IdJugador)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__equipo_ju__id_ju__2B0A656D");
            });

            modelBuilder.Entity<EstadoPartido>(entity =>
            {
                entity.HasKey(e => e.IdEstadoPartido)
                    .HasName("PK__estado_p__267A0D3C92056D0A");

                entity.ToTable("estado_partido");

                entity.Property(e => e.IdEstadoPartido).HasColumnName("id_estado_partido");

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("estado_partido");
            });

            modelBuilder.Entity<JugadorPartido>(entity =>
            {
                entity.HasKey(e => e.IdJugadorPartido)
                    .HasName("PK__jugador___759D123B784679B5");

                entity.ToTable("jugador_partido");

                entity.Property(e => e.IdJugadorPartido).HasColumnName("id_jugador_partido");

                entity.Property(e => e.Goles).HasColumnName("goles");

                entity.Property(e => e.IdJugador).HasColumnName("id_jugador");

                entity.Property(e => e.IdPartido).HasColumnName("id_partido");

                entity.Property(e => e.NumCamiseta).HasColumnName("num_camiseta");

                entity.Property(e => e.TarjetasAmarillas).HasColumnName("tarjetas_amarillas");

                entity.Property(e => e.TarjetasRojas).HasColumnName("tarjetas_rojas");

                entity.Property(e => e.TarjetasVerdes).HasColumnName("tarjetas_verdes");

                entity.HasOne(d => d.IdJugadorNavigation)
                    .WithMany(p => p.JugadorPartidos)
                    .HasForeignKey(d => d.IdJugador)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__jugador_p__id_ju__2DE6D218");

                entity.HasOne(d => d.IdPartidoNavigation)
                    .WithMany(p => p.JugadorPartidos)
                    .HasForeignKey(d => d.IdPartido)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__jugador_p__id_pa__2EDAF651");
            });

            modelBuilder.Entity<Partido>(entity =>
            {
                entity.HasKey(e => e.IdPartido)
                    .HasName("PK__partido__42D83E44B8887781");

                entity.ToTable("partido");

                entity.Property(e => e.IdPartido).HasColumnName("id_partido");

                entity.Property(e => e.CapitanLocal)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("capitan_local");

                entity.Property(e => e.CapitanVisitante)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("capitan_visitante");

                entity.Property(e => e.Dia)
                    .HasColumnType("date")
                    .HasColumnName("dia");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.GolesLocal).HasColumnName("goles_local");

                entity.Property(e => e.GolesVisitante).HasColumnName("goles_visitante");

                entity.Property(e => e.Hora).HasColumnName("hora");

                entity.Property(e => e.IdArbitro1).HasColumnName("id_arbitro1");

                entity.Property(e => e.IdArbitro2).HasColumnName("id_arbitro2");

                entity.Property(e => e.IdCampeonato).HasColumnName("id_campeonato");

                entity.Property(e => e.IdEquipoLocal).HasColumnName("id_equipo_local");

                entity.Property(e => e.IdEquipoVisitante).HasColumnName("id_equipo_visitante");

                entity.Property(e => e.IdJuez).HasColumnName("id_juez");

                entity.Property(e => e.NumFecha)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("num_fecha");

                entity.HasOne(d => d.EstadoNavigation)
                    .WithMany(p => p.Partidos)
                    .HasForeignKey(d => d.Estado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__partido__estado__4CA06362");

                entity.HasOne(d => d.IdArbitro1Navigation)
                    .WithMany(p => p.PartidoIdArbitro1Navigations)
                    .HasForeignKey(d => d.IdArbitro1)
                    .HasConstraintName("FK__partido__id_arbi__49C3F6B7");

                entity.HasOne(d => d.IdArbitro2Navigation)
                    .WithMany(p => p.PartidoIdArbitro2Navigations)
                    .HasForeignKey(d => d.IdArbitro2)
                    .HasConstraintName("FK__partido__id_arbi__4AB81AF0");

                entity.HasOne(d => d.IdCampeonatoNavigation)
                    .WithMany(p => p.Partidos)
                    .HasForeignKey(d => d.IdCampeonato)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__partido__id_camp__46E78A0C");

                entity.HasOne(d => d.IdEquipoLocalNavigation)
                    .WithMany(p => p.PartidoIdEquipoLocalNavigations)
                    .HasForeignKey(d => d.IdEquipoLocal)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__partido__id_equi__47DBAE45");

                entity.HasOne(d => d.IdEquipoVisitanteNavigation)
                    .WithMany(p => p.PartidoIdEquipoVisitanteNavigations)
                    .HasForeignKey(d => d.IdEquipoVisitante)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__partido__id_equi__48CFD27E");

                entity.HasOne(d => d.IdJuezNavigation)
                    .WithMany(p => p.PartidoIdJuezNavigations)
                    .HasForeignKey(d => d.IdJuez)
                    .HasConstraintName("FK__partido__id_juez__4BAC3F29");
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(e => e.IdPersona)
                    .HasName("PK__persona__228148B069C1107A");

                entity.ToTable("persona");

                entity.HasIndex(e => e.NumDocumento, "UQ__persona__7BBF0F6ED7569CF5")
                    .IsUnique();

                entity.Property(e => e.IdPersona).HasColumnName("id_persona");

                entity.Property(e => e.Email)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FechaNacimiento)
                    .HasColumnType("date")
                    .HasColumnName("fecha_nacimiento");

                entity.Property(e => e.IdRol).HasColumnName("id_rol");

                entity.Property(e => e.NombreApellido)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("nombre_apellido");

                entity.Property(e => e.NumDocumento)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("num_documento");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("telefono");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Personas)
                    .HasForeignKey(d => d.IdRol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__persona__id_rol__38996AB5");
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.IdRol)
                    .HasName("PK__rol__6ABCB5E08E812BC9");

                entity.ToTable("rol");

                entity.Property(e => e.IdRol).HasColumnName("id_rol");

                entity.Property(e => e.NombreRol)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("rol");
            });

            modelBuilder.Entity<TipoCampeonato>(entity =>
            {
                entity.HasKey(e => e.IdTipoCampeonato)
                    .HasName("PK__tipo_cam__EBFDC9D9BB456FA7");

                entity.ToTable("tipo_campeonato");

                entity.Property(e => e.IdTipoCampeonato).HasColumnName("id_tipo_campeonato");

                entity.Property(e => e.TipoTorneo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("tipo_campeonato");
            });

            modelBuilder.Entity<TipoUsuario>(entity =>
            {
                entity.HasKey(e => e.IdTipoUsuario)
                    .HasName("PK__tipo_usu__B17D78C8F11434AE");

                entity.ToTable("tipo_usuario");

                entity.Property(e => e.IdTipoUsuario).HasColumnName("id_tipo_usuario");

                entity.Property(e => e.TipoUser)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("tipo_usuario");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__usuario__4E3E04AD63F58A6B");

                entity.ToTable("usuario");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.Activo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("activo")
                    .IsFixedLength(true);

                entity.Property(e => e.FechaAlta)
                    .HasColumnType("date")
                    .HasColumnName("fecha_alta");

                entity.Property(e => e.IdTipoUsuario).HasColumnName("id_tipo_usuario");

                entity.Property(e => e.Pass)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("pass");

                entity.Property(e => e.User)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("usuario");

                entity.HasOne(d => d.IdTipoUsuarioNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdTipoUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__usuario__id_tipo__17F790F9");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
