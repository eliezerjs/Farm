using System.ComponentModel.DataAnnotations;

namespace Projeto.Avaliacao.API.Models
{
    public class Fazenda : ModelBase
    {
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(255)]
        public string? Name { get; set; }   
    }
}
