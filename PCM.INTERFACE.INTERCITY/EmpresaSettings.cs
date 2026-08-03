namespace PCM.INTERFACE.INTERCITY
{
    public class EmpresaSettings
    {
        public string Nome { get; set; } = string.Empty;
        public bool Enabled { get; set; } = true;
        public int CodigoEmpresa { get; set; }
        public string? DefaultConnection { get; set; }
        public string? ConnectionStringIntercity { get; set; }
        public string Label => string.IsNullOrWhiteSpace(Nome) ? CodigoEmpresa.ToString() : Nome;
    }
}
