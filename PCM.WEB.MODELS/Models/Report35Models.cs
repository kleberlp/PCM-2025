using System;
using System.Collections.Generic;
using System.Linq;

namespace PCM.WEB.MODELS
{
    public class Report35ViewModel
    {
        public short CodigoEmpresa { get; set; }
        public int CodigoUnidade { get; set; }
        public long CodigoOrdemServico { get; set; }

        // Nome fantasia da empresa (tb_cad_empresa.nome_fantasia) — exibido no lugar do código
        // interno; código nunca aparece em telas/relatórios voltados ao usuário.
        public string EmpresaDescricao { get; set; }

        public string TipoOrdemServico { get; set; }
        public string Unidade { get; set; }
        public string Data { get; set; }
        public string Servico { get; set; }
        public string Equipamento { get; set; }
        public string Categoria { get; set; }
        public string TipoServico { get; set; }
        public string Solucao { get; set; }

        // Logo do cliente exibido no cabeçalho do relatório (caminho virtual, ex.: "~/Content/...").
        // Montado pelo controller a partir de CodigoEmpresa (pasta ~/Content/img/Cliente/Icons/{codigo_empresa}/).
        public string ClienteLogoUrl { get; set; }

        // Nome do usuário logado que gerou o relatório (Session["nome"], preenchido no login).
        public string EmitidoPor { get; set; }

        public List<Report35ApontamentoViewModel> Apontamentos { get; set; } = new List<Report35ApontamentoViewModel>();
        public List<Report35GrupoViewModel> Grupos { get; set; } = new List<Report35GrupoViewModel>();

        public IEnumerable<Report35ItemViewModel> TodosItens =>
            Grupos.SelectMany(g => g.SubGrupos).SelectMany(s => s.Itens);

        public int TotalItensAvaliaveis => TodosItens.Count(i => i.EhAvaliavel);

        public int TotalConformes => TodosItens.Count(i => i.EhAvaliavel && i.EhConforme);

        public int TotalNaoConformes => TodosItens.Count(i => i.EhAvaliavel && i.EhNaoConforme);

        public int TotalFotos => TodosItens.Sum(i => i.Fotos.Count);

        public decimal PercentualConformidade => TotalItensAvaliaveis == 0
            ? 0m
            : Math.Round(TotalConformes * 100m / TotalItensAvaliaveis, 1);
    }

    public class Report35ApontamentoViewModel
    {
        public string Executor { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataTermino { get; set; }
        public int? TempoGastoMinutos { get; set; }
        public string DescricaoSolucao { get; set; }
    }

    public class Report35GrupoViewModel
    {
        public string Nome { get; set; }
        public List<Report35SubGrupoViewModel> SubGrupos { get; set; } = new List<Report35SubGrupoViewModel>();

        private IEnumerable<Report35ItemViewModel> TodosItens => SubGrupos.SelectMany(s => s.Itens);

        public int TotalAvaliaveis => TodosItens.Count(i => i.EhAvaliavel);

        public int TotalConformes => TodosItens.Count(i => i.EhAvaliavel && i.EhConforme);

        // Calculado a partir dos próprios itens (não do campo "media_grupo" do SP legado,
        // que depende de "peso" e retorna 0 quando o checklist não usa peso — caso deste relatório).
        public decimal? PercentualConformidade => TotalAvaliaveis == 0
            ? (decimal?)null
            : Math.Round(TotalConformes * 100m / TotalAvaliaveis, 1);
    }

    public class Report35SubGrupoViewModel
    {
        public string Nome { get; set; }
        public List<Report35ItemViewModel> Itens { get; set; } = new List<Report35ItemViewModel>();

        public int TotalAvaliaveis => Itens.Count(i => i.EhAvaliavel);

        public int TotalConformes => Itens.Count(i => i.EhAvaliavel && i.EhConforme);

        public decimal? PercentualConformidade => TotalAvaliaveis == 0
            ? (decimal?)null
            : Math.Round(TotalConformes * 100m / TotalAvaliaveis, 1);
    }

    public class Report35ItemViewModel
    {
        public int Codigo { get; set; }
        public short CodigoTipoItemChecklist { get; set; }
        public string Checklist { get; set; }
        public string Descricao { get; set; }
        public string Resultado { get; set; }
        public string Observacao { get; set; }
        public decimal? Peso { get; set; }
        public List<Report35FotoViewModel> Fotos { get; set; } = new List<Report35FotoViewModel>();

        // Tipos 1 (SIM/NÃO) e 8 (SIM/NÃO/N.A.) entram no cálculo de conformidade.
        // Tipos 2 (NUMÉRICO), 3 (TEXTO), 4 (DATA), 5 (HORA), 6, 7 são respostas livres, sem julgamento binário.
        public bool EhBooleano => CodigoTipoItemChecklist == 1 || CodigoTipoItemChecklist == 8;

        public string ResultadoNormalizado => (Resultado ?? string.Empty).Trim().ToUpperInvariant();

        public bool EhAvaliavel => EhBooleano && (ResultadoNormalizado == "SIM" || ResultadoNormalizado == "NÃO" || ResultadoNormalizado == "NAO");

        public bool EhConforme => EhBooleano && ResultadoNormalizado == "SIM";

        public bool EhNaoConforme => EhBooleano && (ResultadoNormalizado == "NÃO" || ResultadoNormalizado == "NAO");

        public bool EhNaoAplicavel => EhBooleano && (ResultadoNormalizado == "N.A." || ResultadoNormalizado == "NA");
    }

    public class Report35FotoViewModel
    {
        public string CaminhoArquivo { get; set; }
        public string Observacao { get; set; }
    }
}
