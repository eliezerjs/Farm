namespace Projeto.Avaliacao.API.DTOs.Request
{
    public class TelemetriaRequestDto
    {
        public long Id { get; set; }
        public long DispositivoId { get; set; }
        public double Temperatura { get; set; }
        public double Umidade { get; set; }
        public DateTime? Data { get; set; }
        public object ToSetValuesModel()
        {
            return new
            {
                DispositivoId,
                Temperatura,
                Umidade,
                Data
            };
        }
    }
}
