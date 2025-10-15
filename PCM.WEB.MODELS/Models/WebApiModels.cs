using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCM.WEB.MODELS
{

    #region ::: LOGIN :::

    public class ApiLogin
    {
        public int message { get; set; }
        public int codigo_empresa { get; set; }
        public string unidade { get; set; }
        public string nome { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo_usuario { get; set; }
        public int codigo_funcionario { get; set; }
        public int ativo { get; set; }
        public List<string> perfil { get; set; }
    }

    #endregion

    #region ::: CADASTRO BÁSICO :::

    public class APIUnidade
    {
        public long codigo { get; set; }
        public string descricao { get; set; }
        public string image { get; set; }
        public int codigo_auditoria_checklist { get; set; }
    }

    public class APIComboBox
    {
        public long codigo { get; set; }
        public string descricao { get; set; }
    }

    public class APILocal
    {
        public long codigo { get; set; }
        public string descricao { get; set; }
    }

    #endregion

    #region ::: ORDEM SERVIÇO :::

    public class APIOrdemServico
    {
        public int page { get; set; }
        public List<ApiOrdemServicoList> results { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
    }

    public class ApiOrdemServicoList
    {
        public long codigo { get; set; }
        public string unidade { get; set; }
        public int codigo_unidade { get; set; }
        public string ordem_servico { get; set; }
        public string descricao { get; set; }
        public string data { get; set; }
        public string local { get; set; }
        public string solicitante { get; set; }
        public string prioridade { get; set; }
        public string descricao_status { get; set; }
        public int status { get; set; }
        public long codigo_equipamento { get; set; }
        public string equipamento { get; set; }
        public string codigo_funcionario { get; set; }
        public string status_opera { get; set; }
    }

    public class ApiOrdemServicoVincular
    {
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public long codigo_ordem_servico { get; set; }
        public int codigo_usuario { get; set; }
    }

    public class ApiOrdemServicoInput
    {
        public int codigo_empresa { get; set; }
        public int codigo_usuario { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo_setor { get; set; }
        public string data { get; set; }
        public int codigo_apartamento { get; set; }
        public long codigo_equipamento { get; set; }
        public int codigo_prioridade { get; set; }
        public string descricao { get; set; }
        public string ordem_servico { get; set; }
    }

    public class ApiOrdemServicoInputResponse
    {
        public long codigo { get; set; }
        public string message { get; set; }
        public string ordem_servico { get; set; }
    }

    public class ApiOrdemServicoApontamento
    {
        public int codigo_empresa { get; set; }
        public int codigo_usuario { get; set; }
        public long codigo_ordem_servico { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo_categoria { get; set; }
        public int codigo_funcionario { get; set; }
        public string data_inicio { get; set; }
        public string data_termino { get; set; }
        public string solucao { get; set; }
        public string imagem { get; set; }
        public int concluido { get; set; }
        public int codigo_justificativa_apontamento { get; set; }
        public long codigo_equipamento { get; set; }
    }

    public class ApiOrdemServicoApontamentoResponse
    {
        public long codigo { get; set; }
        public string message { get; set; }
    }

    public class ApiInfoEquipamentoResponse
    {
        public int codigo_setor { get; set; }
        public string setor { get; set; }
        public long codigo_apartamento { get; set; }
        public string apartamento { get; set; }
        public long codigo_equipamento { get; set; }
        public string equipamento { get; set; }
    }

    public class ApiStatusOrdemServico
    {
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public long codigo_ordem_servico { get; set; }
        public int codigo_usuario { get; set; }
        public int status { get; set; }
    }

    #endregion

    #region ::: PMOC :::

    public class APIPMOC    
    {
        public int page { get; set; }
        public List<ApiPMOCList> results { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
    }

    public class ApiPMOCList
    {
        public int codigo_empresa { get; set; }
        public string unidade { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo_equipamento { get; set; }
        public string tag { get; set; }
        public string descricao { get; set; }
        public string local { get; set; }
        public int codigo_tipo_ar_condicionado { get; set; }
        public string tipo_ar_condicionado { get; set; }
        public string data_proxima_manutencao { get; set; }
        public string data_ultima_manutencao { get; set; }
        public int status { get; set; }
        public string descricao_status { get; set; }
        public long codigo_checklist { get; set; }
        public int intervalo { get; set; }
    }

    public class APIPMOCChecklist
    {
        public int page { get; set; }
        public List<ApiPMOCChecklistList> results { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
    }

    public class ApiPMOCChecklistList
    {
        public int codigo_empresa { get; set; }
        public int codigo_tipo_ar_condicionado { get; set; }
        public int codigo { get; set; }
        public string grupo_checklist { get; set; }
        public string codigo_checklist { get; set; }
        public string descricao { get; set; }
        public int codigo_tipo_item_checklist { get; set; }
        public float valor_minimo { get; set; }
        public float valor_maximo { get; set; }
        public string resultado { get; set; }
        public string observacao { get; set; }
        public string unidade_medida { get; set; }
        public int codigo_unidade_medida { get; set; }
        public string data_manutencao { get; set; }
    }

    public class ApiPMOCApontamento
    {
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public long codigo_equipamento { get; set; }
        public int codigo_tipo_ar_condicionado { get; set; }
        public int codigo_usuario { get; set; }
        public string inicio { get; set; }
        public string termino { get; set; }
        public List<ApiPMOCApontamentoChecklist> checklist { get; set; }
    }

    public class ApiPMOCApontamentoChecklist
    {
        public int codigo { get; set; }
        public string resultado { get; set; }
        public string observacao { get; set; }
        public List<ApiPMOCApontamentoChecklistHoras> horas { get; set; }
    }

    public class ApiPMOCApontamentoChecklistHoras
    {
        public string inicio { get; set; }
        public string termino { get; set; }
    }

    #endregion

    #region ::: PROGRAMADA :::

    public class APIProgramada
    {
        public int page { get; set; }
        public List<APIProgramadaList> results { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
    }

    public class APIProgramadaList
    {
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public long codigo_pcm_programada { get; set; }
        public long codigo_checklist { get; set; }
        public string unidade { get; set; }
        public string data { get; set; }
        public string descricao { get; set; }
        public int status { get; set; }
        public string descricao_status { get; set; }
        public string data_ultima_manutencao { get; set; }
        public string data_validade { get; set; }
        public long codigo_equipamento { get; set; }
        public string equipamento { get; set; }
    }

    public class APIProgramadaChecklist
    {
        public int page { get; set; }
        public List<ApiProgramadaChecklistList> results { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
    }

    public class ApiProgramadaChecklistList
    {
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public long codigo_pcm_programada { get; set; }
        public int codigo { get; set; }
        public string descricao_programada { get; set; }
        public string grupo_checklist { get; set; }
        public string codigo_checklist { get; set; }
        public string descricao { get; set; }
        public int codigo_tipo_item_checklist { get; set; }
        public float valor_minimo { get; set; }
        public float valor_maximo { get; set; }
        public string resultado { get; set; }
        public string observacao { get; set; }
        public string unidade_medida { get; set; }
        public string data { get; set; }
        public string unidade { get; set; }
    }

    public class ApiProgramadaApontamento
    {
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public long codigo_pcm_programada { get; set; }
        public string inicio { get; set; }
        public string termino { get; set; }
        public int concluido { get; set; }
        public string solucao { get; set; }
        public string valor { get; set; }
        public string quantidade_equipamento { get; set; }
        public int codigo_usuario { get; set; }
        public int codigo_fornecedor { get; set; }
        public List<ApiProgramadaApontamentoChecklist> checklist { get; set; }
    }

    public class ApiProgramadaApontamentoChecklist
    {
        public int codigo { get; set; }
        public string resultado { get; set; }
        public string observacao { get; set; }
        public List<ApiProgramadaApontamentoChecklistHoras> horas { get; set; }
    }

    public class ApiProgramadaApontamentoChecklistHoras
    {
        public string inicio { get; set; }
        public string termino { get; set; }
    }

    #endregion

    #region ::: TAREFA :::

    public class APITarefa
    {
        public int page { get; set; }
        public List<APITarefaList> results { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
    }

    public class APITarefaList
    {
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public long codigo_qa_tarefa { get; set; }
        public long codigo_checklist { get; set; }
        public string unidade { get; set; }
        public string data { get; set; }
        public string descricao { get; set; }
        public int status { get; set; }
        public string descricao_status { get; set; }
        public string data_ultima_tarefa { get; set; }
    }

    #endregion

    #region ::: UH :::

    public class APIUH
    {
        public int page { get; set; }
        public List<APIUHList> results { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
    }

    public class APIUHList
    {
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public long codigo_apartamento { get; set; }
        public long codigo_checklist { get; set; }
        public long codigo { get; set; }
        public string unidade { get; set; }
        public int status { get; set; }
        public string data_ultima_vistoria { get; set; }
        public string data_proxima_vistoria { get; set; }
        public string bloco { get; set; }
        public string andar { get; set; }
        public string apartamento { get; set; }
        public string status_opera { get; set; }
        public int nova_vistoria { get; set; }
        public long codigo_apontamento_origem { get; set; }
    }

    public class APIUHChecklist
    {
        public int page { get; set; }
        public List<ApiUHChecklistList> results { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
    }

    public class ApiUHChecklistList
    {
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public long codigo_apartamento { get; set; }
        public int codigo { get; set; }
        public string apartamento { get; set; }
        public string grupo_checklist { get; set; }
        public string codigo_checklist { get; set; }
        public string descricao { get; set; }
        public int opcao { get; set; }
        public string observacao { get; set; }
        public int nova_vistoria { get; set; }
    }

    public class ApiUHApontamento
    {
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public long codigo_apartamento { get; set; }
        public string inicio { get; set; }
        public string termino { get; set; }
        public int codigo_usuario { get; set; }
        public int codigo_responsavel_unidade { get; set; }
        public List<ApiUHApontamentoChecklist> checklist { get; set; }
    }

    public class ApiUHApontamentoChecklist
    {
        public int codigo { get; set; }
        public int opcao { get; set; }
        public string observacao { get; set; }
        public List<ApiUHApontamentoChecklistHoras> horas { get; set; }
    }

    public class ApiUHApontamentoChecklistHoras
    {
        public string inicio { get; set; }
        public string termino { get; set; }
    }

    #endregion

    #region ::: GREEN PLANET :::

    public class APIGreenPlanet
    {
        public int page { get; set; }
        public List<APIGreenPlanetList> results { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
    }

    public class APIGreenPlanetList
    {
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public long codigo_item_medicao { get; set; }
        public string unidade { get; set; }
        public string data { get; set; }
        public string item_medicao { get; set; }
        public int quantidade_hospede { get; set; }
        public int ocupacao_quartos { get; set; }
        public string valor { get; set; }
    }

    public class APIGreenPlanetInput
    {
        public int codigo_empresa { get; set; }
        public int codigo_usuario { get; set; }
        public int codigo_unidade { get; set; }
        public long codigo_item_medicao { get; set; }
        public string quantidade_hospede { get; set; }
        public string ocupacao_quartos { get; set; }
        public string valor { get; set; }
    }

    public class APIGreenPlanetInputResponse
    {
        public string message { get; set; }
    }

    #endregion

    #region ::: FIREBASE :::

    public class APIFirebaseToken
    {
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public string token { get; set; }
    }

    public class APIFirebaseTokenNew
    {
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo_usuario { get; set; }
        public string token { get; set; }
    }

    #endregion

    #region ::: REQUISIÇÃO :::

    public class ApiRequisicaoInput
    {
        public int codigo_empresa { get; set; }
        public int codigo_usuario { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo_setor { get; set; }
        public int codigo_apartamento { get; set; }
        public long codigo_equipamento { get; set; }
        public int codigo_prioridade { get; set; }
        public string descricao { get; set; }
        public string imagem { get; set; }
        public string arquivo { get; set; }
        public string requisicao { get; set; }
    }

    public class ApiRequisicaoInputResponse
    {
        public string message { get; set; }
        public string requisicao { get; set; }
    }

    #endregion

    #region ::: AUDITORIA :::

    public class APIAuditoria
    {
        public int page { get; set; }
        public List<ApiAuditoriaList> results { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
    }

    public class ApiAuditoriaList
    {
        public long codigo { get; set; }
        public long codigo_checklist { get; set; }
        public string numero_documento { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string data { get; set; }
        public string executor { get; set; }
        public int status { get; set; }
    }

    #endregion

    #region ::: AUDITORIA = QUALIDADE :::

    public class APIAuditoriaQualidade
    {
        public int page { get; set; }
        public List<ApiAuditoriaQualidadeList> results { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
    }

    public class ApiAuditoriaQualidadeList
    {
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public long codigo_auditoria_interna { get; set; }
        public long codigo_checklist { get; set; }
        public long codigo { get; set; }
        public string unidade { get; set; }
        public int status { get; set; }
        public string data_ultima_auditora { get; set; }
        public string data_proxima_auditoria { get; set; }
        public string descricao { get; set; }
        public int pontos_possiveis { get; set; }
        public int pontos_realizados { get; set; }
        public int pontos_conformes { get; set; }
        public int pontos_nao_conformes { get; set; }
        public int nao_respondido { get; set; }
        public int nao_aplicaveis { get; set; }
    }

    #endregion

    #region ::: CHECKLIST :::

    public class APIGrupoChecklist
    {
        public string descricao { get; set; }
    }

    public class APIResponse
    {
        public string message { get; set; }
        public long codigo { get; set; }
    }

    public class APISubGrupoChecklist
    {
        public string grupo_checklist { get; set; }
        public string descricao { get; set; }
    }

    public class APICreateAuditoriaResponse
    {
        public long codigo { get; set; }
        public long codigo_checklist { get; set; }
        public List<APIChecklist> checklist { get; set; }        
    }

    public class APIChecklist
    {
        public long codigo_auditoria { get; set; }
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo_tipo_checklist { get; set; }
        public long codigo { get; set; }
        public int codigo_item_checklist { get; set; }
        public int codigo_usuario { get; set; }
        public string tipo { get; set; }
        public int codigo_checklist_item { get; set; }
        public string grupo_checklist { get; set; }
        public string sub_grupo_checklist { get; set; }
        public long codigo_checklist { get; set; }
        public string checklist { get; set; }
        public string descricao { get; set; }
        public int codigo_tipo_item_checklist { get; set; }
        public int numero_digitos { get; set; }
        public int allow_picture { get; set; }
        public string valor_minimo { get; set; }
        public string valor_maximo { get; set; }
        public string unidade_medida { get; set; }
        public string resultado { get; set; }
        public string observacao { get; set; }
        public int error_last { get; set; }
        public List<APIChecklistPictureWeb> picture { get; set; }
    }

    public class APIChecklistInsert
    {
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public long codigo_checklist { get; set; }
        public int codigo_usuario { get; set; }
        public long codigo { get; set; }
        public string tipo { get; set; }
        public string data_inicio { get; set; }
        public string data_termino { get; set; }
        public bool concluido { get; set; }
        public string solucao { get; set; }
        public float valor { get; set; }
        public int codigo_responsavel { get; set; }
        public int quantidade_equipamento { get; set; }
        public List<APIChecklistItem> checklistItems { get; set; }
    }

    public class APIChecklistItem
    {
        public int codigo_checklist_item { get; set; }
        public string resultado { get; set; }
        public string observacao { get; set; }
        public int prazo { get; set; }
        public List<APIChecklistPicture> picture { get; set; }
    }

    public class APIChecklistPictureWeb
    {
        public int codigo_checklist_item { get; set; }
        public string image { get; set; }
        public string observacao { get; set; }
    }

    public class APIChecklistPicture
    {
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public long codigo_checklist { get; set; }
        public int codigo_usuario { get; set; }
        public long codigo { get; set; }
        public int codigo_item_checklist { get; set; }
        public string foto { get; set; }
        public string observacao { get; set; }
    }

    public class APIPicture
    {
        public string tipo { get; set; }
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public long codigo { get; set; }
        public int codigo_item_checklist { get; set; }
        public string image { get; set; }
        public string base64 { get; set; }
    }

    public class APIPictureReturn
    {
        public string message { get; set; }
    }

    #endregion

}
