namespace Projeto.Avaliacao.API.DTOs.Request
{
    public class DispositivoRequestDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long FazendaId { get; set; }

        public object ToSetValuesModel()
        {
            return new
            {
                Name,
                FazendaId
            };
        }
    }
}
