using System.Collections.Generic;

namespace PCM.WEB.MODELS
{

    #region ::: FIREBASE :::

    public class pwaToken
    {
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public int codigoUsuario { get; set; }
        public string token { get; set; }
    }

    #endregion

    #region ::: LOGIN :::

    public class pwaLogin
    {
        public int codigoEmpresa { get; set; }
        public string empresa { get; set; }
        public int codigoUnidade { get; set; }
        public int codigoUsuario { get; set; }
        public int codigoFuncionario { get; set; }
        public string unidade { get; set; }
        public string nome { get; set; }
        public int ativo { get; set; }
        public int apontamento { get; set; }
        public List<string> marca { get; set; }
        public List<pwaLocalizacao> localizacao { get; set; }
        public List<pwaForm> form { get; set; }
        public List<pwaUnidades> unidades { get; set; }
        public List<pwaStatus> statusGovernanca { get; set; }
        public List<pwaEndpoint> endpoint { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public string version { get; set; }
    }

    public class pwaEndpoint
    {
        public string descricao { get; set; }
        public string url { get; set; }
    }

    public class pwaUpdatePassword
    {
        public string email { get; set; }
        public string password { get; set; }
    }

    public class pwaForm
    {
        public int ordem { get; set; }
        public string tag { get; set; }
        public string descricao { get; set; }
        public string cssClass { get; set; }
        public int totalOk { get; set; }
        public int total { get; set; }
    }

    public class pwaDashboardOrdemServico
    {
        public int atrasado { get; set; }
        public int pendente { get; set; }
        public int vinculado { get; set; }
        public int emAndamento { get; set; }
    }

    public class pwaUnidades
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
        public string marca { get; set; }
        public string uf { get; set; }
        public string municipio { get; set; }
        public string urlLogo { get; set; }
    }

    public class pwaLocalizacao
    {
        public string uf { get; set; }
        public List<string> municipio { get; set; }
    }

    public class pwaDefaultResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
    }

    #endregion

    #region ::: GERAL :::

    public class pwaStatus
    {
        public string codigo { get; set; }
        public string descricao { get; set; }
        public string cssClass { get; set; }
    }

    public class pwaLista
    {
        public string codigo { get; set; }
        public string descricao { get; set; }
        public string cssClass { get; set; }
    }

    public class pwaImagem
    {
        public string url { get; set; }
        public string extensao { get; set; }
    }

    public class pwaArquivoInsert
    {
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public int codigoUsuario { get; set; }
        public long codigoDocumento { get; set; }
        public string tipo { get; set; }
        public string extensao { get; set; }
        public long codigoChecklist { get; set; }
        public int codigoItemChecklist { get; set; }
        public string arquivo { get; set; }
        public string guid { get; set; }
    }

    public class pwaApontamentoResponse
    {
        public long codigo { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
    }

    public class pwaCombo
    {
        public long codigo { get; set; }
        public string descricao { get; set; }
    }

    #endregion

    #region ::: PCM - ORDEM DE SERVIÇO :::

    public class pwaOrdemServicoList
    {
        public int page { get; set; }
        public List<pwaOrdemServico> results { get; set; }
        public List<pwaStatus> status { get; set; }
        public List<pwaStatus> statusHotel { get; set; }
        public List<pwaLista> prioridade { get; set; }
        public int totalResults { get; set; }
        public int totalPages { get; set; }
    }

    public class pwaOrdemServico
    {
        public int sequencia { get; set; }
        public long codigoOrdemServico { get; set; }
        public string ordemServico { get; set; }
        public string descricao { get; set; }
        public string dataAbertura { get; set; }
        public string dataNecessidade { get; set; }
        public string setor { get; set; }
        public int codigoSetor { get; set; }
        public int codigoLocal {get; set; }
        public int codigoPrioridade { get; set; }
        public string local { get; set; }
        public string solicitante { get; set; }
        public string prioridade { get; set; }
        public long codigoEquipamento { get; set; }
        public string equipamento { get; set; }
        public bool vinculado { get; set; }
        public pwaStatus status { get; set; }
        public pwaStatus statusOpera { get; set; }
        public List<pwaImagem> arquivo { get; set; }
    }

    public class pwaEstoqueList
    {
        public int page { get; set; }
        public List<pwaEstoque> results { get; set; }
        public int totalResults { get; set; }
        public int totalPages { get; set; }
    }

    public class pwaEstoque
    {
        public int sequencia { get; set; }
        public long codigoOrdemServico { get; set; }
        public long codigoProduto { get; set; }
        public string produto { get; set; }
        public string descricao { get; set; }
        public int quantidade { get; set; }
        public int saldoEstoque { get; set; }
    }

    public class pwaOrdemServicoInsert
    {
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public int codigoUsuario { get; set; }
        public string dataNecessidade { get; set; }
        public int codigoPrioridade { get; set; }
        public int codigoSetor { get; set; }
        public int codigoLocal { get; set; }
        public long codigoEquipamento { get; set; }
        public string descricao { get; set; }
    }

    public class pwaOrdemServicoStart
    {
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public int codigoUsuario { get; set; }
        public long codigoOrdemServico { get; set; }
    }

    public class pwaOrdemServicoResponse
    {
        public long codigo { get; set; }
        public bool success { get; set; }
        public string ordemServico { get; set; }
        public string message { get; set; }
    }

    public class pwaOrdemServicoApontamento
    {
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public int codigoUsuario { get; set; }
        public int codigoFuncionario { get; set; }
        public long codigoOrdemServico { get; set; }
        public string dataInicio { get; set; }
        public string dataTermino { get; set; }
        public string solucao { get; set; }
        public bool concluido { get; set; }
        public int codigoCategoria { get; set; }
        public int codigoJustificativa { get; set; }
    }

    #endregion

    #region ::: PCM - MANUTENÇÃO PROGRAMADA :::

    public class pwaManutencaoProgramadaList
    {
        public int page { get; set; }
        public List<pwaManutencaoProgramada> results { get; set; }
        public int totalResults { get; set; }
        public int totalPages { get; set; }
    }

    public class pwaManutencaoProgramada
    {
        public long codigoPCMProgramadaOrdemServico { get; set; }
        public long codigoPCMProgramada { get; set; }
        public long codigoChecklist { get; set; }
        public string descricao { get; set; }
        public pwaStatus status { get; set; }
        public string dataUltimaManutencao { get; set; }
        public string dataProximaManutencao { get; set; }
        public string setor { get; set; }
        public string familiaEquipamento { get; set; }
        public string equipamento { get; set; }
        public long codigoEquipamento { get; set; }
        public pwaChecklist checklist { get; set; }
    }

    public class pwaManutencaoProgramadaApontamento
    {
        public long codigoPCMProgramadaOrdemServico { get; set; }
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public int codigoUsuario { get; set; }
        public int codigoFuncionario { get; set; }
        public long codigoPCMProgramada { get; set; }
        public string dataInicio { get; set; }
        public string dataTermino { get; set; }
        public string solucao { get; set; }
        public bool concluido { get; set; }
        public string valor { get; set; }
        public int quantidadeEquipamento { get; set; }
        public int codigoJustificativa { get; set; }
        public string observacao { get; set; }
    }

    #endregion

    #region ::: TAREFA :::

    public class pwaTarefaList
    {
        public int page { get; set; }
        public List<pwaTarefa> results { get; set; }
        public int totalResults { get; set; }
        public int totalPages { get; set; }
    }

    public class pwaTarefa
    {
        public long codigoQATarefaOrdemServico { get; set; }
        public long codigoQATarefa { get; set; }
        public long codigoChecklist { get; set; }
        public string descricao { get; set; }
        public pwaStatus status { get; set; }
        public string dataUltimaTarefa { get; set; }
        public string dataProximaTarefa { get; set; }
        public pwaChecklist checklist { get; set; }
    }

    public class pwaTarefaApontamento
    {
        public long codigoQATarefaOrdemServico { get; set; }
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public int codigoUsuario { get; set; }
        public long codigoQATarefa { get; set; }
        public string dataInicio { get; set; }
        public string dataTermino { get; set; }
        public bool concluido { get; set; }
        public string observacao { get; set; }
    }

    #endregion

    #region ::: PMOC :::

    public class pwaPMOCList
    {
        public int page { get; set; }
        public List<pwaPMOC> results { get; set; }
        public int totalResults { get; set; }
        public int totalPages { get; set; }
    }

    public class pwaPMOC
    {
        public long codigoPMOCOrdemServico { get; set; }
        public long codigoArCondicionado { get; set; }
        public long codigoChecklist { get; set; }
        public string tag { get; set; }
        public string arCondicionado { get; set; }
        public string setor { get; set; }
        public string local { get; set; }
        public string tipoArCondicionado { get; set; }
        public string dataUltimoPMOC { get; set; }
        public string dataProximoPMOC { get; set; }
        public pwaStatus status { get; set; }
        public pwaChecklist checklist { get; set; }
    }

    public class pwaPMOCApontamento
    {
        public int codigoPMOCOrdemServico { get; set; }
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public int codigoUsuario { get; set; }
        public int codigoFuncionario { get; set; }
        public long codigoArCondicionado { get; set; }
        public string dataInicio { get; set; }
        public string dataTermino { get; set; }
        public string observacao { get; set; }
        public int intervalo { get; set; }
        public bool concluido { get; set; }
        public int codigoJustificativaApontamento { get; set; }
    }

    #endregion

    #region ::: UH EM DIA :::

    public class pwaUHDiaList
    {
        public int page { get; set; }
        public List<pwaUHDia> results { get; set; }
        public int totalResults { get; set; }
        public int totalPages { get; set; }
    }

    public class pwaUHDia
    {
        public long codigoApartamento { get; set; }
        public long codigoChecklist { get; set; }
        public string bloco { get; set; }
        public string andar { get; set; }
        public string uh { get; set; }
        public string dataUltimaUHDia { get; set; }
        public string dataProximaUHDia { get; set; }
        public pwaStatus status { get; set; }
        public pwaStatus statusOpera { get; set; }
        public pwaChecklist checklist { get; set; }
    }

    public class pwaUHDiaApontamento
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

    public class pwaUHStatusList
    {
        public int page { get; set; }
        public List<pwaUHStatus> results { get; set; }
        public List<pwaStatusUH> statusUH { get; set; }
        public List<pwaStatusGovernanca> statusGovernanca { get; set; }
        public int totalResults { get; set; }
        public int totalPages { get; set; }
    }

    public class pwaUHStatus
    {
        public long codigoApartamento { get; set; }
        public string bloco { get; set; }
        public string andar { get; set; }
        public string uh { get; set; }
        public string poolCondominio { get; set; }
        public int codigoTipoGovernanca { get; set; }
        public string tipoGovernanca { get; set; }
        public pwaStatus statusUH { get; set; }
        public pwaStatus statusGovernanca { get; set; }
    }

    public class pwaUHStatusUpdate
    {
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public int codigoUsuario { get; set; }
        public long codigoApartamento { get; set; }
        public string status { get; set; }
    }

    #endregion

    #region ::: GOVERNANÇA :::

    public class pwaGovernancaList
    {
        public int page { get; set; }
        public List<pwaGovernanca> results { get; set; }
        public List<pwaStatusUH> statusUH { get; set; }
        public List<pwaStatusGovernanca> statusGovernanca { get; set; }
        public bool apontaCamareira { get; set; }
        public bool alteraStatus { get; set; }
        public int totalResults { get; set; }
        public int totalPages { get; set; }
    }

    public class pwaGovernanca
    {
        public long codigoDocumento { get; set; }
        public long codigoApartamento { get; set; }
        public long codigoChecklist { get; set; }
        public int codigoTipoGovernanca { get; set; }
        public string bloco { get; set; }
        public string andar { get; set; }
        public string uh { get; set; }
        public string tipoGovernanca { get; set; }
        public bool naoPerturbe { get; set; }
        public string dataUltimaGovernaca { get; set; }
        public string poolCondominio { get; set; }
        public bool alertaCheckInOut { get; set; }
        public pwaStatus status { get; set; }
        public pwaStatus statusOpera { get; set; }
        public pwaStatus statusGovernanca { get; set; }
        public pwaStatus statusUH { get; set; }
        public pwaChecklist checklist { get; set; }
    }

    public class pwaStatusUH
    {
        public string codigo { get; set; }
        public string descricao { get; set; }
    }

    public class pwaStatusGovernanca
    {
        public string codigo { get; set; }
        public string descricao { get; set; }
    }

    public class pwaGovernancaApontamento
    {
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public int codigoUsuario { get; set; }
        public long codigoChecklist { get; set; }
        public int codigoFuncionario { get; set; }
        public int codigoCamareira { get; set; } = -1;
        public int codigoTipoGovernanca { get; set; }
        public long codigoApartamento { get; set; }
        public string dataInicio { get; set; }
        public string dataTermino { get; set; }
        public string observacao { get; set; }
        public bool naoPerturbe { get; set; }
        public bool concluido { get; set; }
        public string statusGovernanca { get; set; }
    }

    #endregion

    #region ::: GREEN PLANET :::

    public class pwaGreenPlanetList
    {
        public string data { get; set; }
        public int numeroHospede { get; set; }
        public int uhOcupada { get; set; }
        public List<pwaGreenPlanetItemMedicao> itemMedicao { get; set; }
    }

    public class pwaGreenPlanetItemMedicao
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
        public int numeroCasasDecimais { get; set; }
        public bool allowPicture { get; set; }
        public float valor { get; set; }
    }

    public class pwaGreenPlanetApontamento
    {
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public int codigoUsuario { get; set; }
        public string data { get; set; }
        public int numeroHospede { get; set; }
        public int uhOcupada { get; set; }
        public List<pwaGreenPlanetItemMedicao> itemMedicao { get; set; }
    }

    #endregion

    #region ::: DEDETIZAÇÃO :::

    public class pwaDedetizacaoList
    {
        public int page { get; set; }
        public List<pwaDedetizacao> results { get; set; }
        public int totalResults { get; set; }
        public int totalPages { get; set; }
    }

    public class pwaDedetizacao
    {
        public long codigoApartamento { get; set; }
        public string bloco { get; set; }
        public string andar { get; set; }
        public string uh { get; set; }
        public string dataUltimaDedetizacao { get; set; }
        public string dataProximaDedetizacao { get; set; }
        public pwaStatus status { get; set; }
        public pwaStatus statusOpera { get; set; }
    }

    public class pwaDedetizacaoApontamento
    {
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public int codigoUsuario { get; set; }
        public int codigoFuncionario { get; set; }
        public long codigoApartamento { get; set; }
        public string observacao { get; set; }
        public bool concluido { get; set; }
    }

    #endregion

    #region ::: MAPA MANUTENÇÃO :::

    public class pwaMapaList
    {
        public int page { get; set; }
        public List<pwaMapa> results { get; set; }
        public int totalResults { get; set; }
        public int totalPages { get; set; }
    }

    public class pwaMapa
    {
        public long codigo { get; set; }
        public string descricao { get; set; }
    }

    public class pwaMapaManutencaoList
    {
        public int page { get; set; }
        public List<pwaMapaManutencao> results { get; set; }
        public int totalResults { get; set; }
        public int totalPages { get; set; }
    }

    public class pwaMapaManutencao
    {
        public long codigoAtividade { get; set; }
        public long codigoApartamento { get; set; }
        public string bloco { get; set; }
        public string andar { get; set; }
        public string uh { get; set; }
        public string dataPrevisaoTermino { get; set; }
        public pwaStatus statusOpera { get; set; }
    }

    public class pwaMapaManutencaoApontamento
    {
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public int codigoUsuario { get; set; }
        public int codigoFuncionario { get; set; }
        public long codigoAtividade { get; set; }
        public long codigoApartamento { get; set; }
        public string observacao { get; set; }
        public bool concluido { get; set; }
    }

    #endregion

    #region ::: QUALIDADE - AUDITORIA :::


    public class pwaQualidadeAuditoriaList
    {
        public int page { get; set; }
        public List<pwaQualidadeAuditoria> results { get; set; }
        public int totalResults { get; set; }
        public int totalPages { get; set; }
    }

    public class pwaQualidadeAuditoria
    {
        public long codigo { get; set; }
        public long codigoAuditoriaInterna { get; set; }
        public long codigoChecklist { get; set; }
        public pwaStatus status { get; set; }
        public string dataUltimaAuditoria { get; set; }
        public string dataProximaAuditoria { get; set; }
        public string descricao { get; set; }
        public int pontosPossiveis { get; set; }
        public int pontosRealizados { get; set; }
        public int pontosConformes { get; set; }
        public int pontosNaoConformes { get; set; }
        public int naoRespondido { get; set; }
        public int naoAplicaveis { get; set; }
        public pwaChecklist checklist { get; set; }
    }

    public class pwaQualidadeAuditoriaApontamento
    {
        public int codigo { get; set; }
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public int codigoUsuario { get; set; }
        public int codigoFuncionario { get; set; }
        public long codigoAuditoriaInterna { get; set; }
        public string dataInicio { get; set; }
        public string dataTermino { get; set; }
        public string observacao { get; set; }
        public bool concluido { get; set; }
    }

    #endregion

    #region ::: AUDITORIA - CORPORATIVO :::


    public class pwaAuditoriaCorporativoList
    {
        public int page { get; set; }
        public List<pwaAuditoriaCorporativo> results { get; set; }
        public int totalResults { get; set; }
        public int totalPages { get; set; }
    }

    public class pwaAuditoriaCorporativo
    {
        public long codigo { get; set; }
        public long codigoAuditoriaInterna { get; set; }
        public long codigoChecklist { get; set; }
        public pwaStatus status { get; set; }
        public string dataUltimaAuditoria { get; set; }
        public string dataProximaAuditoria { get; set; }
        public string descricao { get; set; }
        public int pontosPossiveis { get; set; }
        public int pontosRealizados { get; set; }
        public int pontosConformes { get; set; }
        public int pontosNaoConformes { get; set; }
        public int naoRespondido { get; set; }
        public int naoAplicaveis { get; set; }
        public pwaChecklist checklist { get; set; }
    }

    public class pwaAuditoriaCorporativoApontamento
    {
        public int codigo { get; set; }
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public int codigoUsuario { get; set; }
        public int codigoFuncionario { get; set; }
        public long codigoAuditoriaInterna { get; set; }
        public string numeroDocumento { get; set; }
        public string dataInicio { get; set; }
        public string dataTermino { get; set; }
        public string observacao { get; set; }
        public bool concluido { get; set; }
    }

    #endregion

    #region ::: CHECKLIST :::

    public class pwaChecklist
    {
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public long codigoChecklist { get; set; }
        public List<pwaChecklistGrupo> grupo { get; set; }
    }

    public class pwaChecklistGrupo
    {
        public string descricao { get; set; }
        public int totalOk { get; set; }
        public int total { get; set; }
        public List<pwaChecklistSubGrupo> subgrupo { get; set; }
        public List<pwaChecklistItem> checklist { get; set; }
    }

    public class pwaChecklistSubGrupo
    {
        public string descricao { get; set; }
        public int totalOk { get; set; }
        public int total { get; set; }
        public List<pwaChecklistItem> checklist { get; set; }
    }

    public class pwaChecklistItem
    {
        public int codigoTipoChecklist { get; set; }
        public int codigo { get; set; }
        public string checklist { get; set; }
        public string descricao { get; set; }
        public int numeroDigitos { get; set; }
        public int allowPicture { get; set; }
        public string uom { get; set; }
        public string resultado { get; set; }
        public string observacao { get; set; }
        public bool ordemServico { get; set; }
        public string prazo { get; set; }
        public string color { get; set; }
        public int associarEquipamento { get; set; }
        public long codigoEquipamento { get; set; }
        public List<pwaImagem> arquivo { get; set; }
    }

    public class pwaChecklistApontamento
    {
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public int codigoUsuario { get; set; }
        public long codigoChecklist { get; set; }
        public int codigo { get; set; }
        public string resultado { get; set; }
        public string observacao { get; set; }
        public int intervalo { get; set; }
        public bool ordemServico { get; set; } = false;
        public string prazo { get; set; } = "";
    }

    #endregion

    #region ::: NOTIFICAÇÃO :::

    public class pwaNotificacao
    {
        public string message { get; set; }
        public bool success { get; set; }
        public string version { get; set; }
        public List<pwaListaNotificacao> results { get; set; }
    }

    public class pwaListaNotificacao
    {
        public long codigo { get; set; }
        public string cabecalho { get; set; }
        public string info { get; set; }
        public string modulo { get; set; }
        public string descricao { get; set; }
        public string autor { get; set; }
        public bool lido { get; set; }
    }

    public class pwaReadNotificacao
    {
        public long codigo { get; set; }
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public int codigoUsuario { get; set; }
    }

    #endregion

    #region ::: LOG BOOK :::

    public class pwaLogBook
    {
        public string message { get; set; }
        public bool success { get; set; }
        public List<pwaListaLogBook> results { get; set; }
    }

    public class pwaListaLogBook
    {
        public long codigo { get; set; }
        public string usuario { get; set; }
        public string data { get; set; }
        public string descricao { get; set; }
    }

    public class pwaInsertLogBook
    {
        public long codigo { get; set; }
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public int codigoUsuario { get; set; }
        public string descricao { get; set; }
    }

    #endregion

}
