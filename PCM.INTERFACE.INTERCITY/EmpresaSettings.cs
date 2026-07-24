namespace PCM.INTERFACE.INTERCITY
{
    /// <summary>
    /// Uma empresa processada pelo serviço. Vários itens podem ser cadastrados na
    /// seção "Empresas" para rodar N empresas dentro de UM único serviço.
    /// As conexões são opcionais: quando não informadas, o serviço usa as conexões
    /// compartilhadas de ConnectionStrings (DefaultConnection / ConnectionStringIntercity).
    /// </summary>
    public class EmpresaSettings
    {
        /// <summary>Nome/identificador amigável (usado nos logs).</summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>Quando false, a empresa é ignorada no ciclo.</summary>
        public bool Enabled { get; set; } = true;

        /// <summary>Empresa (PCM) a processar.</summary>
        public int CodigoEmpresa { get; set; }

        /// <summary>Conexão SQL (PCM) desta empresa. Vazio = usa a compartilhada.</summary>
        public string? DefaultConnection { get; set; }

        /// <summary>Conexão Oracle (Intercity) desta empresa. Vazio = usa a compartilhada.</summary>
        public string? ConnectionStringIntercity { get; set; }

        public string Label => string.IsNullOrWhiteSpace(Nome) ? CodigoEmpresa.ToString() : Nome;
    }
}
