using System.Collections.Generic;

namespace PCM.WEB.MODELS
{
    // ===== Tudo em Dia — checklist periódico de manutenção por LOCAL =====

    // Um local na lista de execução (tela ChecklistTudo)
    public class TudoLocal
    {
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public int codigo_setor { get; set; }
        public string setor { get; set; }
        public int codigo { get; set; }              // codigo do local (tb_cad_apartamento)
        public string local { get; set; }
        public long codigo_checklist { get; set; }
        public string checklist { get; set; }
        public int codigo_periodicidade { get; set; }
        public string periodicidade { get; set; }
        public int intervalo { get; set; }
        public string data_proxima { get; set; }     // dd/MM/yyyy
        public int status { get; set; }              // 1..5 (tb_stc_status_uh_dia)
        public string css_class { get; set; }
        public string color { get; set; }
        public string bg_color { get; set; }
        public long codigo_apontamento { get; set; }
    }

    // Contadores da torre de status
    public class TudoStatus
    {
        public int atrasado { get; set; }
        public int pendente { get; set; }
        public int manutencao { get; set; }
        public int nova_vistoria { get; set; }
        public int realizada { get; set; }
    }

    // Item do checklist na execução (tela ChecklistTudoApontamento) — Fase 2
    public class TudoApontamentoChecklist
    {
        public string grupo { get; set; }
        public int codigo { get; set; }              // codigo do item do checklist
        public string checklist { get; set; }
        public string descricao { get; set; }
        public string opcao { get; set; }            // SIM / NÃO / N/A
        public string observacao { get; set; }
        public bool nova_vistoria { get; set; }
    }

    // Cabeçalho do apontamento (execução) — Fase 2
    public class TudoApontamento
    {
        public long codigo { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo_apartamento { get; set; }
        public long codigo_checklist { get; set; }
        public int codigo_funcionario_responsavel { get; set; }
        public string data_inicio { get; set; }
        public string data_termino { get; set; }
        public string hora_inicio { get; set; }
        public string hora_termino { get; set; }
        public bool nova_vistoria { get; set; }
    }

    // Linha do histórico — Fase 3
    public class TudoChecklistHistorico
    {
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string setor { get; set; }
        public int codigo_apartamento { get; set; }
        public string local { get; set; }
        public long codigo { get; set; }
        public string checklist { get; set; }
        public string responsavel { get; set; }
        public string data_inicio { get; set; }
        public string data_termino { get; set; }
        public string tempo { get; set; }
    }
}
