using System.ComponentModel.DataAnnotations;

namespace Projeto.Avaliacao.API.Models
{
    public abstract class ModelBase
    {
        [Key]
        public long Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
