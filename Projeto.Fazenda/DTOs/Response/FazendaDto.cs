namespace Projeto.Avaliacao.API.DTOs.Response
{
    public class FazendaResponseDto
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
