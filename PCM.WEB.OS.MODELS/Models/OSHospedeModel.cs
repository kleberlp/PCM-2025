namespace PCM.WEB.OS.MODELS
{
    public class OSHospedeApartamento
    {
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public long codigoApartamento { get; set; }
        public string? unidade { get; set; }
        public string? apartamento { get; set; }
        public string? bloco { get; set; }
        public string? andar { get; set; }
    }

    public class PerguntasAvaliacao
    {
        public int codigo { get; set; }
        public string? descricao { get; set; }
    }
    
    public class RatingAnswer
    {
        public int QuestionId { get; set; }
        public int Value { get; set; }
    }

    public class RatingForm
    {
        public string uniqueId { get; set; }
        public List<RatingAnswer> Ratings { get; set; }
    }
}
