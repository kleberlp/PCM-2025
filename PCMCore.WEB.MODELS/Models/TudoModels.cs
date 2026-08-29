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

    // KPIs da agenda de vencimentos (Fase 5)
    public class TudoAgendaKpi
    {
        public int planejados { get; set; }
        public int atrasados { get; set; }
        public int hoje { get; set; }
        public int concluidos { get; set; }
    }

    // Evento do calendário (Fase 5.1)
    public class TudoAgendaEvento
    {
        public long codigo { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo_apartamento { get; set; }
        public string data { get; set; }          // yyyy-MM-dd
        public string local { get; set; }
        public string setor { get; set; }
        public string responsavel { get; set; }
        public int status_codigo { get; set; }     // 0 planejado, 1 concluído, 2 cancelado
        public string situacao { get; set; }
        public bool atrasado { get; set; }
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
        public int codigo_tipo_item_checklist { get; set; } // 1 Sim/Não, 2 Numérico, 3 Texto, 4 Data, 5 Hora, 8 Sim/Não/N.A.
        public string checklist { get; set; }
        public string descricao { get; set; }
        public string opcao { get; set; }            // SIM / NÃO / N/A (tipos 1 e 8)
        public string resposta { get; set; }         // valor livre (texto/numérico/data/hora)
        public string observacao { get; set; }
        public bool allow_picture { get; set; }      // tb_chk_checklist_item.allow_picture
        public decimal valor_minimo { get; set; }
        public decimal valor_maximo { get; set; }
        public string unidade_medida { get; set; }
        public bool abre_os { get; set; }            // usuário optou por abrir OS neste item
        public bool nova_vistoria { get; set; }
        public long codigo_ordem_servico { get; set; }
    }

    // Cabeçalho do apontamento (execução) + contexto do local — Fase 2
    public class TudoApontamento
    {
        public long codigo { get; set; }
        public int codigo_apartamento { get; set; }
        public string local { get; set; }
        public string setor { get; set; }
        public long codigo_checklist { get; set; }
        public string checklist { get; set; }
        public string periodicidade { get; set; }
        public int intervalo { get; set; }
        public string data_proxima { get; set; }
        public int codigo_funcionario_responsavel { get; set; }
        public string data_inicio { get; set; }
        public string data_termino { get; set; }
        public string hora_inicio { get; set; }
        public string hora_termino { get; set; }
        public bool nova_vistoria { get; set; }
        public bool finalizado { get; set; }
    }

    // ===== PWA (app) — Fase 4 =====
    public class pwaTudoDiaList
    {
        public int page { get; set; }
        public List<pwaTudoDia> results { get; set; }
        public int totalResults { get; set; }
        public int totalPages { get; set; }
    }

    public class pwaTudoDia
    {
        public long codigoApartamento { get; set; }
        public long codigoChecklist { get; set; }
        public string local { get; set; }
        public string setor { get; set; }
        public string dataUltimoTudo { get; set; }
        public string dataProximoTudo { get; set; }
        public pwaStatus status { get; set; }
    }

    public class pwaTudoDiaApontamento
    {
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public int codigoUsuario { get; set; }
        public int codigoFuncionario { get; set; }
        public long codigoApartamento { get; set; }
        public string dataInicio { get; set; }
        public string dataTermino { get; set; }
        public string observacao { get; set; }
        public bool concluido { get; set; }
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
