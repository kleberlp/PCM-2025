using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCM.WEB.MODELS
{
    public class login
    {
        public string email { get; set; }
        public string senha { get; set; }
    }

    public class imagemLogin
    {
        public string imagem_login { get; set; }
        public string imagem_background { get; set; }
    }
    
    public class FormularioVisualizar
    {
        public bool qualidade { get; set; }
        public bool adm_cliente { get; set; }
        public bool adm_empresa { get; set; }
        public bool adm_perfil { get; set; }
        public bool adm_perfil_hierarquia { get; set; }        
        public bool adm_usuario { get; set; }
        public bool audit_laudo { get; set; }
        public bool audit_normas_procedimentos { get; set; }
        public bool audit_relatorio { get; set; }
        public bool audit_relatorio_mensal_pcm { get; set; }
        public bool audit_externa { get; set; }
        public bool audit_corporativo { get; set; }
        public bool audit_corporativo_historico { get; set; }
        public bool cad_apartamento { get; set; }
        public bool cad_ar_condicionado { get; set; }
        public bool cad_atividade { get; set; }
        public bool cad_auditoria_qualidade { get; set; }
        public bool cad_auditoria_corporativo { get; set; }
        public bool cad_categoria { get; set; }
        public bool cad_checklist { get; set; }
        public bool cad_cliente { get; set; }
        public bool cad_cliente_acordo_comercial { get; set; }
        public bool cad_departamento { get; set; }
        public bool cad_departamento_gestor { get; set; }
        public bool cad_equipamento { get; set; }
        public bool cad_enxoval { get; set; }
        public bool cad_familia_equipamento { get; set; }
        public bool cad_fornecedor { get; set; }
        public bool cad_funcao { get; set; }
        public bool cad_funcionario { get; set; }
        public bool cad_dedetizacao { get; set; }
        public bool cad_grupo_item_medicao { get; set; }
        public bool cad_grupo_produto { get; set; }
        public bool cad_item_medicao { get; set; }
        public bool cad_itens_gerais { get; set; }
        public bool cad_justificativa_apontamento { get; set; }
        public bool cad_justificativa_falta { get; set; }
        public bool cad_justificativa_cancelar_ordem_servico { get; set; }
        public bool cad_laudo { get; set; }
        public bool adm_configuracao_desempenho_unidade { get; set; }
        public bool cad_preventiva { get; set; }
        public bool cad_prioridade { get; set; }
        public bool cad_produto { get; set; }        
        public bool cad_rotina { get; set; }
        public bool cad_setor { get; set; }
        public bool cad_tarefa { get; set; }
        public bool cad_tipo_apartamento { get; set; }
        public bool cad_tipo_ar_condicionado { get; set; }
        public bool cad_tipo_cama { get; set; }
        public bool cad_tipo_despesa { get; set; }
        public bool cad_treinamento { get; set; }
        public bool cad_unidade { get; set; }
        public bool cad_unidade_medida { get; set; }
        public bool cad_relatorio_itens_auditaveis { get; set; }
        public bool cfg_opera { get; set; }
        public bool fin_contrato { get; set; }
        public bool fin_controle_gasto { get; set; }
        public bool fin_input_despesa { get; set; }

        public bool governanca { get; set; }
        public bool gov_funcionario { get; set; }
        public bool gov_planejamento { get; set; }
        public bool gov_planejamento_historico { get; set; }
        public bool gov_historico { get; set; }
        public bool gov_dashboard { get; set; }
        public bool gov_apontamento { get; set; }
        public bool gov_status_uh { get; set; }
        public bool gov_lavanderia { get; set; }
        public bool gov_log_alteracao_status_gov { get; set; }

        public bool green_lancamento { get; set; }
        public bool log_book { get; set; }
        public bool ordem_servico { get; set; }
        public bool ordem_servico_atribuir { get; set; }
        public bool uh_atividade { get; set; }
        public bool pcm_apontamento_os_edit { get; set; }
        public bool pcm_apontamento_laudo { get; set; }
        public bool pcm_apontamento_preventiva { get; set; }
        public bool pcm_apontamento_os { get; set; }
        public bool pcm_apontamento_rotina { get; set; }
        public bool pcm_cronograma_semanal { get; set; }
        public bool pcm_falta { get; set; }
        public bool pcm_historico_programada { get; set; }
        public bool pcm_manutencao_laudo { get; set; }
        public bool pcm_manutencao_preventiva { get; set; }
        public bool pcm_manutencao_rotina { get; set; }
        public bool pcm_plano_acao { get; set; }
        public bool pcm_requisicao { get; set; }
        public bool pcm_requisicao_aprovar_reprovar { get; set; }
        public bool pmoc_apontamento { get; set; }
        public bool pmoc_historico { get; set; }
        public bool pmoc { get; set; }
        public bool pmoc_andar { get; set; }
        public bool pmoc_bup { get; set; }
        public bool pmoc_cronograma { get; set; }
        public bool pmoc_cronograma_bup { get; set; }
        public bool rel_atendimento { get; set; }
        public bool rel_custo_horas_trabalhadas { get; set; }
        public bool rel_funcionario_horas_trabalhadas { get; set; }
        public bool rel_funcionario_ociosidade { get; set; }
        public bool rel_green_planet { get; set; }
        public bool rel_manutencao_aberto_concluido { get; set; }
        public bool rel_manutencao { get; set; }
        public bool rel_manutencao_categoria { get; set; }
        public bool rel_manutencao_equipamento { get; set; }
        public bool rel_manutencao_executor { get; set; }
        public bool rel_manutencao_setor { get; set; }
        public bool rel_manutencao_solicitante { get; set; }
        public bool rel_manutencao_tempo_medio_atendimento { get; set; }
        public bool rel_manutencao_tipo_ordem_servico { get; set; }
        public bool rel_pmoc_mes { get; set; }
        public bool rel_pmoc_bimestral { get; set; }
        public bool rel_log_book { get; set; }
        public bool rel_nao_conformidade { get; set; }
        public bool rel_preventiva_mes { get; set; }
        public bool rel_dinamico { get; set; }
        public bool rel_consumo_enxoval { get; set; }
        public bool rel_horas_trabalhadas_governanca { get; set; }
        public bool rel_camareira_uh { get; set; }
        public bool rel_camareira_nc { get; set; }
        public bool rel_responsavel_vistoria_uh { get; set; }
        public bool uh_checklist { get; set; }
        public bool uh_checklist_historico { get; set; }
        public bool uh_dedetizacao { get; set; }
        public bool excel_ordem_servico { get; set; }
        public bool excel_plano_acao_qa { get; set; }

        public bool est_requisicao_compra { get; set; }
        public bool est_ordem_compra { get; set; }
        public bool est_entrada { get; set; }
        public bool est_saida { get; set; }
        public bool est_inventario { get; set; }
        public bool est_listagem { get; set; }

        public bool aeb_contrato { get; set; }
        public bool aeb_laudo { get; set; }
        public bool aeb_normas_procedimentos { get; set; }
        public bool aeb_auditoria_externa { get; set; }

        public bool qa_auditoria { get; set; }
        public bool qa_auditoria_historico { get; set; }
        public bool qa_auditoria_cronograma { get; set; }
        public bool qa_plano_acao { get; set; }
        public bool dash_desempenho { get; set; }
        public bool dash_desempenho_qa { get; set; }
        public bool qa_tarefa { get; set; }
        public bool qa_tarefa_historico { get; set; }

        public bool cad_area_comum { get; set; }
        public bool agenda_area_comum { get; set; }
        public bool agenda_entrada { get; set; }
        public bool agenda_saida { get; set; }

        public bool upload_excel { get; set; }
        public bool upload_pmoc { get; set; }

        public bool lav_apontamento { get; set; }
        public bool lav_historico { get; set; }
        public bool rel_lav_controle { get; set; }

        public bool cad_item_os_hospede { get; set; }

    }
}
