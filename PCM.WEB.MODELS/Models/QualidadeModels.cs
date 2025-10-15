using System.Collections.Generic;

namespace PCM.WEB.MODELS
{
    public class QAAuditoriaStatus
    {
        public int atrasado { get; set; }
        public int pendente { get; set; }
        public int concluido { get; set; }
        public int em_andamento { get; set; }
    }

    public class QAAuditoria
    {
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public List<QAAuditoriaDia> auditoria { get; set; }
    }

    public class QAAuditoriaDia
    {
        public int codigo_auditoria_interna { get; set; }
        public long codigo_auditoria { get; set; }
        public long codigo_checklist { get; set; }
        public string descricao { get; set; }
        public int status { get; set; }
        public string descricao_status { get; set; }
        public string css_class { get; set; }
        public string data { get; set; }
        public string usuario { get; set; }
        public int pontos_possiveis { get; set; }
        public int pontos_realizados { get; set; }
        public int conforme { get; set; }
        public int nao_conforme { get; set; }
        public int nao_respondido { get; set; }
        public int nao_aplicavel { get; set; }
        public string nota { get; set; }
    }

    public class QAAuditoriaHistorico
    {
        public int codigo_auditoria_interna { get; set; }
        public long codigo_auditoria { get; set; }
        public long codigo_checklist { get; set; }
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string descricao { get; set; }
        public string usuario { get; set; }
        public int status { get; set; }
        public string descricao_status { get; set; }
        public string css_class { get; set; }
        public string data { get; set; }
        public int pontos_possiveis { get; set; }
        public int pontos_realizados { get; set; }
        public int conforme { get; set; }
        public int nao_conforme { get; set; }
        public int nao_respondido { get; set; }
        public int nao_aplicavel { get; set; }
        public string nota { get; set; }
    }

    public class QAApontamento
    {
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public int codigo_auditoria_interna { get; set; }
        public long codigo_auditoria { get; set; }
        public int codigo_departamento { get; set; }
        public long codigo_checklist { get; set; }
        public string descricao { get; set; }
        public string usuario { get; set; }
        public string numero_documento { get; set; }
        public int status { get; set; }
        public string data_inicio { get; set; }
        public string data_termino { get; set; }
        public string nota { get; set; }
        public List<QAApontamentoChecklist> checklist { get; set; }
    }

    public class QAApontamentoChecklist
    {
        public long codigo_checklist { get; set; }
        public int codigo_item_checklist { get; set; }
        public string checklist { get; set; }
        public int codigo_tipo_item_checklist { get; set; }
        public string grupo { get; set; }
        public string subgrupo { get; set; }
        public string descricao { get; set; }
        public int numero_digitos { get; set; }
        public bool allow_picture { get; set; }
        public string unidade_medida { get; set; }
        public string resultado { get; set; }
        public string observacao { get; set; }
        public string responsavel { get; set; }
        public string prazo { get; set; }
        public int quantidade_ordem_servico { get; set; }
        public bool possui_foto { get; set; }
    }

    public class QAApontamentoArquivo
    {
        public long codigo_auditoria { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo_item_checklist { get; set; }
        public List<string> arquivo { get; set; }        
    }


    public class QAAuditoriaCronograma
    {
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public long codigo_auditoria { get; set; }
        public long codigo_auditoria_interna { get; set; }
        public string descricao { get; set; }
        public int codigo_periodicidade { get; set; }
        public string periodicidade { get; set; }
        public int intervalo { get; set; }
        public string data_ultima_auditoria { get; set; }
        public string data_proxima_auditoria { get; set; }
        public string data_inicio { get; set; }
        public string data_termino { get; set; }
        public int total { get; set; }
        public int total_realizado { get; set; }
        public float percentual { get; set; }
        public int dia_1 { get; set; }
        public int dia_2 { get; set; }
        public int dia_3 { get; set; }
        public int dia_4 { get; set; }
        public int dia_5 { get; set; }
        public int dia_6 { get; set; }
        public int dia_7 { get; set; }
        public int dia_8 { get; set; }
        public int dia_9 { get; set; }
        public int dia_10 { get; set; }
        public int dia_11 { get; set; }
        public int dia_12 { get; set; }
        public int dia_13 { get; set; }
        public int dia_14 { get; set; }
        public int dia_15 { get; set; }
        public int dia_16 { get; set; }
        public int dia_17 { get; set; }
        public int dia_18 { get; set; }
        public int dia_19 { get; set; }
        public int dia_20 { get; set; }
        public int dia_21 { get; set; }
        public int dia_22 { get; set; }
        public int dia_23 { get; set; }
        public int dia_24 { get; set; }
        public int dia_25 { get; set; }
        public int dia_26 { get; set; }
        public int dia_27 { get; set; }
        public int dia_28 { get; set; }
        public int dia_29 { get; set; }
        public int dia_30 { get; set; }
        public int dia_31 { get; set; }
    }

    public class QATarefaStatus
    {
        public int atrasado { get; set; }
        public int pendente { get; set; }
        public int concluido { get; set; }
        public int em_andamento { get; set; }
    }

    public class QATarefa
    {
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public List<QATarefaDia> tarefa { get; set; }
    }

    public class QATarefaDados
    {
        public long codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string descricao { get; set; }
        public string descricao_solucao { get; set; }
        public string data_inicio { get; set; }
        public string data_termino { get; set; }
        public string hora_inicio { get; set; }
        public string hora_termino { get; set; }
    }

    public class QATarefaDia
    {
        public long codigo_qa_tarefa { get; set; }
        public long codigo_qa_tarefa_ordem_servico { get; set; }
        public string descricao { get; set; }
        public int status { get; set; }
        public string descricao_status { get; set; }
        public string cor { get; set; }
        public string data { get; set; }
    }

    public class QualidadeTarefa
    {
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public long codigo_qa_tarefa { get; set; }
        public string descricao { get; set; }
        public int codigo_periodicidade { get; set; }
        public string periodicidade { get; set; }
        public int intervalo { get; set; }
        public string data_ultima_os { get; set; }
        public string data_proxima_os { get; set; }
        public string data_inicio { get; set; }
        public string data_termino { get; set; }
        public int total { get; set; }
        public int total_realizado { get; set; }
        public float percentual { get; set; }
        public int dia_1 { get; set; }
        public int dia_2 { get; set; }
        public int dia_3 { get; set; }
        public int dia_4 { get; set; }
        public int dia_5 { get; set; }
        public int dia_6 { get; set; }
        public int dia_7 { get; set; }
        public int dia_8 { get; set; }
        public int dia_9 { get; set; }
        public int dia_10 { get; set; }
        public int dia_11 { get; set; }
        public int dia_12 { get; set; }
        public int dia_13 { get; set; }
        public int dia_14 { get; set; }
        public int dia_15 { get; set; }
        public int dia_16 { get; set; }
        public int dia_17 { get; set; }
        public int dia_18 { get; set; }
        public int dia_19 { get; set; }
        public int dia_20 { get; set; }
        public int dia_21 { get; set; }
        public int dia_22 { get; set; }
        public int dia_23 { get; set; }
        public int dia_24 { get; set; }
        public int dia_25 { get; set; }
        public int dia_26 { get; set; }
        public int dia_27 { get; set; }
        public int dia_28 { get; set; }
        public int dia_29 { get; set; }
        public int dia_30 { get; set; }
        public int dia_31 { get; set; }
    }

    public class QATarefaOrdemServico
    {
        public long codigo { get; set; }
        public int codigo_unidade { get; set; }
        public long codigo_qa_tarefa { get; set; }
        public string unidade { get; set; }
        public string servico { get; set; }
        public string data { get; set; }
        public string descricao_solucao { get; set; }
        public string status { get; set; }
        public List<QATarefaApontamento> apontamento { get; set; }
        public List<QATarefaApontamentoChecklist> checklist { get; set; }
    }

    public class QATarefaApontamento
    {
        public long codigo { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo_funcionario { get; set; }
        public string funcionario { get; set; }
        public string descricao_solucao { get; set; }
        public string data_inicio { get; set; }
        public string data_termino { get; set; }
        public string hora_inicio { get; set; }
        public string hora_termino { get; set; }
        public List<string> fotos { get; set; }
    }

    public class QATarefaApontamentoChecklist
    {
        public long codigo_qa_tarefa_ordem_servico { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo { get; set; }
        public int codigo_tipo_item_checklist { get; set; }
        public string tipo_checklist { get; set; }
        public string grupo { get; set; }
        public string subgrupo { get; set; }
        public string checklist { get; set; }
        public string descricao { get; set; }
        public float valor_minimo { get; set; }        
        public float valor_maximo { get; set; }
        public string resultado { get; set; }
        public string unidade_medida { get; set; }
        public string observacao { get; set; }
        public bool allow_picture { get; set; }
    }

    public class QATarefaOrdemServicoHistorico
    {
        public long codigo { get; set; }
        public int codigo_unidade { get; set; }
        public long codigo_qa_tarefa { get; set; }
        public string unidade { get; set; }
        public string tarefa { get; set; }
        public string data_execucao { get; set; }
    }

}
