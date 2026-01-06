using System.Collections.Generic;

namespace PCM.WEB.MODELS
{

    public class dashboardGovernancaArrumadoxVistoriado
    {
        public string unidade { get; set; }
        public float quantidadeUHs { get; set; }
        public float quantidadeVistoriados { get; set; }
        public float quantidadeArrumados { get; set; }
    }

    public class dashboardGovernanca
    {
        public string quantidadeCamareira { get; set; }
        public string quantidadeVistoriador { get; set; }
        public string quantidadeUHs { get; set; }
        public string hhDisponivel { get; set; }
        public string hhUtilizado { get; set; }
        public string quantidateUHsGovernanca { get; set; }
        public string quantidadeOSGerada { get; set; }
    }

    public class dashboardGovernancaChartArrumacaoDia
    {
        public string camareira { get; set; }
        public string quantidade { get; set; }
        public string data { get; set; }
    }

    public class dashboardGovernancaChartVistoriaDia
    {
        public string vistoriador { get; set; }
        public string quantidade { get; set; }
        public string data { get; set; }
    }

    public class dashboardGovernancaChartProdutividade
    {
        public string unidade { get; set; }
        public string percentual { get; set; }
        public string quantidadePendente { get; set; }
        public string quantidadeOK { get; set; }
        public string total { get; set; }
    }

    public class dashboardGovernancaNCDia
    {
        public string unidade { get; set; }
        public string quantidadeNC { get; set; }
        public string data { get; set; }
    }

    public class dashboardGovernancaAtendimentoOS
    {
        public string unidade { get; set; }
        public int quantidadeProprio { get; set; }
        public int quantidadeTerceiro { get; set; }
        public string hhDisponivel { get; set; }
        public int quantidaeOSGerada { get; set; }
        public int quantidaeOSAtendida { get; set; }
        public int quantidaeOSPAX { get; set; }
        public int quantidadeOSPendente { get; set; }
        public string percentualAtendido { get; set; }
    }

    public class dashboardGovernancaChartUHxCamareira
    {
        public string camareira { get; set; }
        public string quantidade { get; set; }
        public string data { get; set; }
    }

    public class dashboardGovernancaNCDetalhado
    {
        public string ocorrencia { get; set; }
        public int quantidadeNC { get; set; }
        public int mediaMovel30Dias { get; set; }
        public string tendencia { get; set; }
    }

    public class dashboardRankingCamareira
    {
        public string camareira { get; set; }
        public string percentualNC { get; set; }
        public string percentualNCRetrabalho { get; set; }
        public int ranking { get; set; }
        public long qtdeNC { get; set; }
        public long qtdeNCRetrabalho { get; set; }
        public long pesoNC { get; set; }
        public long pesoNCRetrabalho { get; set; }
        public long qtdeUH { get; set; }
        public string cssClass { get; set; }
    }

    public class dashboardGovernancaNCCamareira
    {
        public string camareira { get; set; }
        public int quantidadeNC { get; set; }
        public int mediaMovel30Dias { get; set; }
        public string tendencia { get; set; }
    }

    public class dashboardGovernancaChartNaoConformidadeTipo
    {
        public string naoConformidadeTipo { get; set; }
        public string quantidade { get; set; }
    }

    public class dashboardGovernancaNaoConformidade
    {
        public string item { get; set; }
        public string descricao { get; set; }
        public string quantidade { get; set; }
    }

    public class Governanca
    {
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string bloco { get; set; }
        public string andar { get; set; }
        public string apartamento { get; set; }
        public int codigo_apartamento { get; set; }
        public string tipo_apartamento { get; set; }
        public int codigo_tipo_apartamento { get; set; }
        public long codigo_checklist { get; set; }
        public int status { get; set; }
        public string descricao_status { get; set; }
        public string data_proxima { get; set; }
        public string css_class { get; set; }
        public long codigo_vistoria { get; set; }
        public string room_status { get; set; }
        public string front_office_status { get; set; }
        public int codigo_tipo_governanca { get; set; }
        public bool nao_perturbe { get; set; }
    }

    public class GovernancaDados
    {
        public int codigo { get; set; }
        public string camareira { get; set; }
        public string apartamento { get; set; }
        public string codigo_funcionario { get; set; }
        public int codigo_funcionario_responsavel_vistoria { get; set; }
        public string data{ get; set; }
        public string hora_inicio { get; set; }
        public string hora_termino { get; set; }
        public int status { get; set; }
        public bool apontaCamareira { get; set; }
    }

    public class GovernancaApontamento
    {
        public string unidade { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo_apartamento { get; set; }
        public string apartamento { get; set; }
        public string funcionario_responsavel { get; set; }
        public string funcionario_responsavel_unidade { get; set; }
        public string data_inicio { get; set; }
        public string data_termino { get; set; }
        public bool aponta_horas { get; set; }
        public string hora_inicio { get; set; }
        public string hora_termino { get; set; }
        public float valor { get; set; }
        public long codigo_apontamento { get; set; }
    }

    public class GovernancaApontamentoResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
    }

    public class GovernancaApto
    {
        public List<GovernancaApontamentoChecklist> checklist { get; set; }
        public List<GovernancaApontamentoEnxoval> enxoval { get; set; }
    }
    
    public class GovernancaApontamentoChecklist
    {
        public int codigo { get; set; }
        public string grupo { get; set; }
        public string checklist { get; set; }
        public string descricao { get; set; }
        public int codigo_tipo_item_checklist { get; set; }
        public string resultado { get; set; }
        public string resultado_descricao { get; set; }
        public string observacao { get; set; }
        public bool nova_vistoria { get; set; }
    }

    public class GovernancaApontamentoEnxoval
    {
        public string unidade { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo_enxoval { get; set; }
        public string enxoval { get; set; }
        public int quantidade { get; set; }
        public long codigo_apontamento { get; set; }
    }

    public class GovernancaStatus
    {
        public int pendente { get; set; }
        public int concluido { get; set; }
        public int aguardandoLiberacao { get; set; }
        public int retrabalho { get; set; }
    }

    public class GovFuncionario
    {
        public int codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string nome { get; set; }
        public string cpf { get; set; }
        public int codigo_funcao { get; set; }
        public int codigo_usuario { get; set; }
        public string funcao { get; set; }
        public string telefone { get; set; }
        public string tipo_funcionario { get; set; }
        public int codigo_tipo_funcionario { get; set; }
        public float valor_hora { get; set; }
        public bool ativo { get; set; }
        public string texto_ativo { get; set; }
        public bool contabiliza_hora { get; set; }
    }

    public class GovernancaPlanejamento
    {
        public int codigoApartamento { get; set; }
        public string apartamento { get; set; }
        public string tipoApartamento { get; set; }
        public string tipoCama { get; set; }
        public string bloco { get; set; }
        public string andar { get; set; }
        public string quantidadeCama { get; set; }
        public string funcionario { get; set; }
        public string selecionado { get; set; }
        public string statusFrontOffice { get; set; }
        public string tipoGovernanca { get; set; }
        public string dataChegada { get; set; }
        public string dataSaida { get; set; }
        public string cssClassTipoGovernaca { get; set; }
        public string statusRoom { get; set; }
    }

    public class GovernancaPlanejamentoHistorico
    {
        public int codigoApartamento { get; set; }
        public string data { get; set; }
        public string tipoGovernanca { get; set; }
        public string apartamento { get; set; }
        public string tipoApartamento { get; set; }
        public string bloco { get; set; }
        public string andar { get; set; }
        public string quantidadeCama { get; set; }
        public string camareira { get; set; }
        public string executado { get; set; }
        public string quantidadeNC { get; set; }
        public string vistoriado { get; set; }

    }

    public class GovernancaApontamentoApartamento
    {
        public int codigoApartamento { get; set; }
        public string apartamento { get; set; }
        public string tipoApartamento { get; set; }
        public string tipoCama { get; set; }
        public string bloco { get; set; }
        public string andar { get; set; }
        public string quantidadeCama { get; set; }
        public string funcionario { get; set; }
        public string selecionado { get; set; }
        public string statusFrontOffice { get; set; }
        public string tipoGovernanca { get; set; }
        public string cssClassTipoGovernaca { get; set; }
        public string statusRoom { get; set; }
    }

    public class GovernancaHistorico
    {
        public long codigo { get; set; }
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public string unidade { get; set; }
        public string data { get; set; }
        public string apartamento { get; set; }
        public string camareira { get; set; }
        public string horaInicio { get; set; }
        public string horaTermino { get; set; }
        public string tempoGasto { get; set; }
        public string tipoGovernanca { get; set; }
        public int naoConformidade { get; set; }
        public string responsavelVistoria { get; set; }
        public string horaVistoria { get; set; }
        public string status { get; set; }
        public string cssClass { get; set; }
    }

    public class GovernancaHistoricoDetails
    {
        public List<GovernancaHistoricoChecklistGrupo> grupo { get; set; }
        public List<GovernancaHistoricoEnxoval> enxoval { get; set; }
    }

    public class GovernancaHistoricoChecklistGrupo
    {
        public string grupo { get; set; }
        public List<GovernancaHistoricoChecklistSubGrupo> subgrupo { get; set; }
    }

    public class GovernancaHistoricoChecklistSubGrupo
    {
        public string grupo { get; set; }
        public string subgrupo { get; set; }
        public List<GovernancaHistoricoChecklist> checklist { get; set; }
    }

    public class GovernancaHistoricoChecklist
    {
        public string checklist { get; set; }
        public string resposta { get; set; }
        public string observacao { get; set; }
        public bool foto { get; set; }
        public string cssClass { get; set; }
    }

    public class GovernancaHistoricoEnxoval
    {
        public string enxoval { get; set; }
        public string quantidade { get; set; }
        public string peso { get; set; }
        public string totalPeso { get; set; }
    }

    public class GovernancaStatusUH
    {
        public bool success { get; set; }
        public string message { get; set; }
        public List<GovernancaStatusUHDetalhe> result { get; set; }
        public List<GovernancaStatusUHDetalheError> resultError { get; set; }
    }

    public class GovernancaStatusUHDetalhe
    {
        public string bloco { get; set; }
        public string andar { get; set; }
        public string apartamento { get; set; }
        public string statusUH { get; set; }
        public string statusGov { get; set; }
        public string newStatusUH { get; set; }
        public string newStatusGov { get; set; }
    }

    public class GovernancaStatusUHDetalheError
    {
        public string apartamento { get; set; }
        public string erro { get; set; }
    }

    public class GovernancaLogAlteracaoStatusUH
    {
        public string unidade { get; set; }
        public string bloco { get; set; }
        public string andar { get; set; }
        public string apartamento { get; set; }
        public string status { get; set; }
        public string usuario { get; set; }
        public string dataAlteracao { get; set; }
    }


    public class GovernancaInventarioEnxoval
    {
        public long codigo { get; set; }
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public string unidade { get; set; }
        public string data { get; set; }
        public string contador { get; set; }
        public string status { get; set; }
        public string statusDescricao { get; set; }
        public string acuracidade { get; set; }
        public string cssClass { get; set; }
    }

}
