namespace Projeto.Avaliacao.API.DTOs.Response
{
    public class DispositivoResponseDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long FazendaId { get; set; }
        public FazendaResponseDto Fazenda { get; set; }
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
