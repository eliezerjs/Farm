using Microsoft.EntityFrameworkCore;
using Projeto.Avaliacao.API.Models;

namespace Projeto.Avaliacao.API.Repository
{
    /// <summary>
    ///  Context of application.
    /// </summary>
    public partial class DefaultContext : DbContext
    {
        public DefaultContext(DbContextOptions<DefaultContext> options)
            : base(options)
        {

        }
        
        /// <summary>
        /// Entity Farm.
        /// </summary>
        public DbSet<Fazenda> Fazendas { get; set; }
        
        /// <summary>
        /// Entity Devices.
        /// </summary>
        public DbSet<Dispositivo> Dispositivos { get; set; }
        
        /// <summary>
        /// Entity Telemetry.
        /// </summary>
        public DbSet<Telemetria> Telemetrias { get; set; }
    }
}
