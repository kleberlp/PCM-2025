using System.Collections.Generic;

namespace PCM.WEB.MODELS
{

    #region ::: AVISOS AOS CLIENTES :::

    /// <summary>
    /// Aviso exibido como popup no login: período de vigência, alvo por
    /// empresa/unidade (-1 = todas), auditoria e avaliação opcionais e as
    /// seções 1:N em HTML — cada seção é um passo do carrossel.
    /// </summary>
    public class Aviso
    {
        public int codigo { get; set; }
        public string titulo { get; set; }
        /// <summary>yyyy-MM-dd no cadastro; dd/MM/yyyy quando exibido.</summary>
        public string data_inicio { get; set; }
        public string data_termino { get; set; }
        /// <summary>-1 = todas as empresas.</summary>
        public int codigo_empresa { get; set; } = -1;
        /// <summary>-1 = todas as unidades.</summary>
        public int codigo_unidade { get; set; } = -1;
        public bool auditado { get; set; }
        public bool avaliado { get; set; }
        public bool ativo { get; set; } = true;
        /// <summary>No payload do login: avaliação já dada pelo usuário (0 = nenhuma).</summary>
        public int avaliacao { get; set; }
        public List<AvisoSecao> secoes { get; set; } = new List<AvisoSecao>();
    }

    /// <summary>Uma seção do aviso (um passo do carrossel).</summary>
    public class AvisoSecao
    {
        public int codigo { get; set; }
        public int sequencia { get; set; }
        public string titulo { get; set; }
        /// <summary>HTML, sanitizado na gravação.</summary>
        public string conteudo { get; set; }
    }

    /// <summary>Linha da grade de manutenção de avisos.</summary>
    public class AvisoGrid
    {
        public int codigo { get; set; }
        public string titulo { get; set; }
        public string data_inicio { get; set; }
        public string data_termino { get; set; }
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public bool auditado { get; set; }
        public bool avaliado { get; set; }
        public bool ativo { get; set; }
        /// <summary>0 desativado · 1 agendado · 2 ativo · 3 encerrado.</summary>
        public int situacao { get; set; }
        public int visualizacoes { get; set; }
        public int avaliacoes { get; set; }
        public decimal media_avaliacao { get; set; }
    }

    /// <summary>Resumo da auditoria de um aviso.</summary>
    public class AvisoAuditoriaResumo
    {
        public string titulo { get; set; }
        public int visualizacoes { get; set; }
        public int usuarios { get; set; }
        public int dispensaram { get; set; }
        public int avaliaram { get; set; }
        public decimal media_avaliacao { get; set; }
        public List<AvisoAuditoriaLinha> linhas { get; set; } = new List<AvisoAuditoriaLinha>();
    }

    /// <summary>Uma linha do log de auditoria (um usuário).</summary>
    public class AvisoAuditoriaLinha
    {
        public string usuario { get; set; }
        public string ultima_visualizacao { get; set; }
        public int exibicoes { get; set; }
        /// <summary>0 = não avaliou.</summary>
        public int avaliacao { get; set; }
        public bool nao_ver_mais { get; set; }
        public string data_dispensa { get; set; }
    }

    #endregion

}
