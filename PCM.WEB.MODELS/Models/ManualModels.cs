using System.Collections.Generic;

namespace PCM.WEB.MODELS
{

    #region ::: MANUAL :::

    /// <summary>
    /// Manual integrado (botão "?" do cabeçalho). O conteúdo mora no banco
    /// PostgreSQL do manual, e não no SQL Server do PCM. Uma classe serve a
    /// leitura e a manutenção: tipo 'S' é manual de tela (controller/action) e
    /// tipo 'P' é manual de processo — um manual avulso que as telas apontam
    /// pelo processo_codigo e que aparece como "ver também" no rodapé do painel.
    /// </summary>
    public class Manual
    {
        public int codigo { get; set; }
        /// <summary>S = tela, P = processo.</summary>
        public string tipo { get; set; }
        public string controller { get; set; }
        public string action { get; set; }
        /// <summary>Manual de processo ligado à tela, para o link do rodapé.</summary>
        public int processo_codigo { get; set; }
        public string processo_titulo { get; set; }
        public string titulo { get; set; }
        public string subtitulo { get; set; }
        public bool ativo { get; set; }
        public List<ManualItem> itens { get; set; } = new List<ManualItem>();
        /// <summary>
        /// Telas ALÉM da principal (controller/action acima) que este mesmo manual
        /// atende — telas irmãs que compartilham a ajuda em vez de duplicar o texto.
        /// </summary>
        public List<ManualTela> telas { get; set; } = new List<ManualTela>();
    }

    /// <summary>Uma tela adicional atendida pelo manual.</summary>
    public class ManualTela
    {
        public string controller { get; set; }
        /// <summary>Vazio = módulo inteiro daquele controller.</summary>
        public string action { get; set; }
    }

    /// <summary>Uma seção do manual.</summary>
    public class ManualItem
    {
        public int codigo { get; set; }
        public int sequencia { get; set; }
        public string titulo { get; set; }
        public string conteudo { get; set; }
        /// <summary>D = dica, A = aviso, vazio = sem destaque.</summary>
        public string tipo_nota { get; set; }
        public string nota { get; set; }
        public string imagem { get; set; }
        /// <summary>Hyperlink de vídeo (YouTube, Vimeo ou URL direta).</summary>
        public string video { get; set; }
    }

    /// <summary>Linha da grade de manutenção do manual.</summary>
    public class ManualGrid
    {
        public int codigo { get; set; }
        public string titulo { get; set; }
        public string subtitulo { get; set; }
        /// <summary>S = tela, P = processo.</summary>
        public string tipo { get; set; }
        /// <summary>A tela (controller/action) ou vazio quando processo.</summary>
        public string tela { get; set; }
        public int secoes { get; set; }
        public bool ativo { get; set; }
    }

    #endregion

}
