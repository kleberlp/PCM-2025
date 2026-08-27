namespace TAG.SERVICE.Models
{
    /// <summary>Linha pendente de TAG de equipamento/rotina (chave composta).</summary>
    public class TagPendente
    {
        public short CodigoEmpresa { get; set; }
        public int CodigoUnidade { get; set; }
        public long Codigo { get; set; }
        /// <summary>URL da imagem do QR code a baixar.</summary>
        public string? Code { get; set; }
    }

    /// <summary>Linha pendente de TAG de apartamento (chave por uniqueId).</summary>
    public class TagApartamentoPendente
    {
        public string UniqueId { get; set; } = string.Empty;
        public string? Code { get; set; }
    }
}
