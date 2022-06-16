using System.ComponentModel.DataAnnotations;

namespace Projeto.Avaliacao.API.Models
{
    public class Telemetria : ModelBase
    {
        [Required(ErrorMessage = "Dispositivo is required.")]
        public long DispositivoId { get; set; }
        public virtual Dispositivo Dispositivo { get; set; }
        public double Temperatura { get; set; }
        public double Umidade { get; set; }
        public DateTime? Data { get; set; }
    }
}
