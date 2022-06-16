namespace Projeto.Avaliacao.API.DTOs.Request
{
    public class FazendaRequestDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public object ToSetValuesModel()
        {
            return new
            {
                Name
            };
        }
    }
}
