using System.ComponentModel.DataAnnotations;

namespace Projeto.Avaliacao.API.Models
{
    /// <summary>
    /// Model of Devices. 
    /// </summary>
    public class Dispositivo : ModelBase
    {
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(255)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Farm is required.")]
        public long FazendaId { get; set; }
        public virtual Fazenda Fazenda { get; set; }
    }
}
