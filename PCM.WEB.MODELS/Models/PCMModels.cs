using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCM.WEB.MODELS
{
    public class PCMOrdemServico
    {
        public long codigo { get; set; }
        public long codigo_pcm_ordem_servico { get; set; }
        public long codigo_pcm_programada { get; set; }
        public int item { get; set; }
        public string ordem_servico { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string setor { get; set; }
        public string equipamento { get; set; }
        public string apartamento { get; set; }
        public string local { get; set; }
        public string prioridade { get; set; }
        public string periodicidade { get; set; }
        public string intervalo { get; set; }
        public string servico { get; set; }
        public string status { get; set; }
        public int codigo_status { get; set; }
        public int codigo_tipo_ordem_servico { get; set; }
        public string data { get; set; }
        public string solicitante { get; set; }
    }

    public class PCMProgramadaOrdemServico
    {
        public long codigo { get; set; }
        public long codigo_pcm_programada { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public int codigo_setor { get; set; }
        public string setor { get; set; }
        public long codigo_equipamento { get; set; }
        public string equipamento { get; set; }
        public string categoria { get; set; }
        public int codigo_tipo_servico { get; set; }
        public string tipo_servico { get; set; }
        public string tipo_ordem_servico { get; set; }
        public string servico { get; set; }
        public string data { get; set; }
        public string descricao_solucao { get; set; }
        public string status { get; set; }
        public float valor { get; set; }
        public string valor_texto { get; set; }
        public int quantidade_equipamento { get; set; }
        public bool aponta_horas { get; set; }
        public List<PCMProgramadaOrdemServicoApontamento> apontamento { get; set; }
        public List<PCMProgramadaOrdemServicoChecklist> checklist { get; set; }
    }

    public class PCMProgramadaOrdemServicoApontamento
    {
        public long codigo_pcm_programada_ordem_servico { get; set; }
        public long codigo { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo_executor { get; set; }
        public string executor { get; set; }
        public string data_inicio { get; set; }
        public string data_termino { get; set; }
    }

    public class PCMProgramadaOrdemServicoChecklist
    {
        public long codigo_pcm_programada_ordem_servico { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo { get; set; }
        public int codigo_tipo_item_checklist { get; set; }
        public string tipo_checklist { get; set; }
        public string grupo { get; set; }
        public string subgrupo { get; set; }
        public string checklist { get; set; }
        public string descricao { get; set; }
        public bool allow_picture { get; set; }
        public bool possui_foto { get; set; }
        public float valor_minimo { get; set; }
        public float valor_maximo { get; set; }
        public string unidade_medida { get; set; }
        public string observacao { get; set; }
        public string texto { get; set; }
        public string numero { get; set; }
        public string data { get; set; }
        public string hora { get; set; }
        public bool sim_nao { get; set; }
        public bool sim_nao_na { get; set; }
        public string resultado { get; set; }
    }

    public class PCMOrdemServicoApontamento
    {
        public long codigo { get; set; }
        public string executor { get; set; }
        public string descricao_solucao { get; set; }
        public string data_inicio { get; set; }
        public string hora_inicio { get; set; }
        public string data_termino { get; set; }
        public string hora_termino { get; set; }
    }

    public class AgendaOS
    {
        public PCMOrdemServico ordem_servico { get; set; }
        public List<PCMOrdemServicoApontamento> apontamento { get; set; }
    }

    public class Apontamento
    {
        public string unidade { get; set; }
        public int codigo_unidade { get; set; }
        public string ordem_servico { get; set; }
        public string setor { get; set; }
        public string equipamento { get; set; }
        public string apartamento { get; set; }
        public string servico { get; set; }
        public string descricao_solucao { get; set; }
        public string categoria { get; set; }
        public string tipo_servico { get; set; }
        public string tipo_ordem_servico { get; set; }
        public int codigo_categoria { get; set; }
        public int codigo_departamento { get; set; }
        public int codigo_tipo_servico { get; set; }
        public int codigo_tipo_ordem_servico { get; set; }
        public int codigo_fornecedor { get; set; }
        public int codigo_funcionario { get; set; }
        public string fornecedor { get; set; }
        public string funcionario { get; set; }
        public string data_inicio { get; set; }
        public string data_termino { get; set; }
        public bool aponta_horas { get; set; }
        public bool aponta_horas_qualidade { get; set; }
        public string hora_inicio { get; set; }
        public string hora_termino { get; set; }
        public float valor { get; set; }
        public int quantidade_equipamento { get; set; }
        public bool exige_laudo { get; set; }
        public long codigo_apontamento { get; set; }
        public long codigo_pcm_programada { get; set; }
        public long codigo_pcm_programada_ordem_servico { get; set; }
        public long codigo { get; set; }
        public long codigo_pcm_ordem_servico { get; set; }
        public bool ar_condicionado { get; set; }
        public List<string> fotos { get; set; }
    }

    public class OrdemServicoApontamento
    {
        public int item { get; set; }
        public string executor { get; set; }
        public string solucao { get; set; }
        public string justificativa { get; set; }
        public string data_inicio { get; set; }
        public string data_termino { get; set; }
        public string horas { get; set; }
        public string imagem_ordem_servico { get; set; }
        public string imagem_apontamento { get; set; }
        public string usuario { get; set; }
        public List<PictureModels> fotos { get; set; }
    }

    public class ManutencaoApontamento
    {
        public string unidade { get; set; }
        public string executor { get; set; }
        public string ordem_servico { get; set; }
        public string local { get; set; }
        public string descricao { get; set; }
        public string data_inicio { get; set; }
        public string data_termino { get; set; }
        public string horas { get; set; }
        public string custo { get; set; }
        public bool erro { get; set; }
        public int codigo_unidade { get; set; }
        public long codigo { get; set; }
    }

    public class ApontamentoChecklist
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
        public string unidade_medida { get; set; }
        public int codigo_tipo_item_checklist { get; set; }
        public bool resultado { get; set; }
        public float valor { get; set; }
        public float valor_minimo { get; set; }
        public float valor_maximo { get; set; }
        public string observacao { get; set; }
    }

    public class OrdemServico
    {
        public Int64 codigo { get; set; }
        public int codigo_categoria { get; set; }
        public string categoria { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public int codigo_setor { get; set; }
        public string setor { get; set; }
        public long codigo_equipamento { get; set; }
        public string equipamento { get; set; }
        public string departamento { get; set; }
        public string local { get; set; }
        public string solicitante { get; set; }
        public string executor { get; set; }
        public string data_execucao { get; set; }
        public int codigo_apartamento { get; set; }
        public string apartamento { get; set; }
        public string numero_documento { get; set; }
        public string ordem_servico_cliente { get; set; }
        public string data { get; set; }
        public string data_necessidade { get; set; }
        public string data_necessidade_inicial { get; set; }
        public string dias { get; set; }
        public string css_class { get; set; }
        public string descricao { get; set; }
        public int codigo_prioridade { get; set; }
        public string prioridade { get; set; }
        public int codigo_tipo_servico { get; set; }
        public string tipo_servico { get; set; }
        public int codigo_tipo_ordem_servico { get; set; }
        public string tipo_ordem_servico { get; set; }
        public string arquivo { get; set; }
        public int status { get; set; }
        public string status_descricao { get; set; }
        public string horas { get; set; }
        public string justificativa_apontamento { get; set; }
        public string justificativa_cancelamento { get; set; }
        public string imagem { get; set; }
        public string auditoria { get; set; }
        public List<PictureModels> fotos { get; set; }
    }

    public class ManutencaoProgramada
    {
        public string manutencao { get; set; }
        public int janeiro { get; set; }
        public int fevereiro { get; set; }
        public int marco { get; set; }
        public int abril { get; set; }
        public int maio { get; set; }
        public int junho { get; set; }
        public int julho { get; set; }
        public int agosto { get; set; }
        public int setembro { get; set; }
        public int outubro { get; set; }
        public int novembro { get; set; }
        public int dezembro { get; set; }
        public int manutencao_janeiro { get; set; }
        public int manutencao_fevereiro { get; set; }
        public int manutencao_marco { get; set; }
        public int manutencao_abril { get; set; }
        public int manutencao_maio { get; set; }
        public int manutencao_junho { get; set; }
        public int manutencao_julho { get; set; }
        public int manutencao_agosto { get; set; }
        public int manutencao_setembro { get; set; }
        public int manutencao_outubro { get; set; }
        public int manutencao_novembro { get; set; }
        public int manutencao_dezembro { get; set; }
        public long codigo_pcm_programada { get; set; }
        public int codigo_unidade { get; set; }
    }

    public class CronogramaManutencaoPreventiva
    {
        public string nome_fantasia { get; set; }
        public string manutencao { get; set; }
        public string data_ultima_manutencao { get; set; }
        public int mes_1 { get; set; }
        public int mes_2 { get; set; }
        public int mes_3 { get; set; }
        public int mes_4 { get; set; }
        public int mes_5 { get; set; }
        public int mes_6 { get; set; }
        public int mes_7 { get; set; }
        public int mes_8 { get; set; }
        public int mes_9 { get; set; }
        public int mes_10 { get; set; }
        public int mes_11 { get; set; }
        public int mes_12 { get; set; }
        public long codigo_pcm_programada { get; set; }
        public int codigo_unidade { get; set; }
        public long codigo_pcm_programada_ordem_servico { get; set; }
    }

    public class ManutencaoPreventivaStatus
    {
        public int pendente { get; set; }
        public int atrasado { get; set; }
        public int concluido { get; set; }
        public int emAndamento { get; set; }
    }

    public class ManutencaoPreventiva
    {
        public int codigoUnidade { get; set; }
        public long codigoPCMProgramada { get; set; }
        public long codigoPCMOrdemServicoProgramada { get; set; }
        public string unidade { get; set; }
        public string preventiva { get; set; }
        public string equipamento { get; set; }
        public string dataUltimaManutencao { get; set; }
        public string dataProximaManutencao { get; set; }
        public string periodicidade { get; set; }
        public string intervalo { get; set; }
        public string status { get; set; }
        public int statusID { get; set; }
        public string cssClass { get; set; }
    }

    public class ManutencaoRotina
    {
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public long codigo_pcm_programada { get; set; }
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

    public class ManutencaoLaudo
    {
        public string nome_fantasia { get; set; }
        public string manutencao { get; set; }
        public string equipamento { get; set; }
        public string data_validade { get; set; }
        public int mes_1 { get; set; }
        public int mes_2 { get; set; }
        public int mes_3 { get; set; }
        public int mes_4 { get; set; }
        public int mes_5 { get; set; }
        public int mes_6 { get; set; }
        public int mes_7 { get; set; }
        public int mes_8 { get; set; }
        public int mes_9 { get; set; }
        public int mes_10 { get; set; }
        public int mes_11 { get; set; }
        public int mes_12 { get; set; }
        public long codigo_pcm_programada { get; set; }
        public int codigo_unidade { get; set; }
        public long codigo_pcm_programada_ordem_servico { get; set; }
    }

    public class ManutencaoProgramadaAgenda
    {
        public string nome { get; set; }
        public string data { get; set; }
        public string color { get; set; }
        public long codigo_pcm_programada { get; set; }
        public int codigo_unidade { get; set; }
    }

    public class PCMManutencaoProgramada
    {
        public string manutencao { get; set; }
        public long codigo_pcm_programada { get; set; }
        public int codigo_unidade { get; set; }
        public List<PCMManutencaoProgramadaMes> mes { get; set; }
    }

    public class PCMManutencaoProgramadaMes
    {
        public int mes { get; set; }
        public int ano { get; set; }
        public string manutencao { get; set; }
    }

    public class PCMRotinaStatus
    {
        public int atrasado { get; set; }
        public int pendente { get; set; }
        public int concluido { get; set; }
        public int andamento { get; set; }
    }

    public class PCMRotina
    {
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public List<PCMRotinaDia> rotina { get; set; }
    }

    public class PCMRotinaDia
    {
        public long codigo_pcm_programada { get; set; }
        public long codigo_pcm_programada_ordem_servico { get; set; }
        public string descricao { get; set; }
        public int status { get; set; }
        public string descricao_status { get; set; }
        public string cor { get; set; }
        public string data { get; set; }
        public string dataUltimaManutencao { get; set; }
        public string intervalo { get; set; }
        public string periodicidade { get; set; }
    }

    public class PCMProgramadaHistorico
    {
        public string unidade { get; set; }
        public string data { get; set; }
        public string descricao { get; set; }
        public string tipo_servico { get; set; }
        public string tipo_ordem_servico { get; set; }
        public string quantidade_equipamento { get; set; }
        public string valor { get; set; }
        public long codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string formulario { get; set; }
    }

    public class PCMFalta
    {
        public long codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public int codigo_funcionario { get; set; }
        public string funcionario { get; set; }
        public int codigo_justificativa_falta { get; set; }
        public string justificativa_falta { get; set; }
        public string data_inicio { get; set; }
        public string hora_inicio { get; set; }
        public string data_termino { get; set; }
        public string hora_termino { get; set; }
        public string observacao { get; set; }
        public string path_arquivo { get; set; }
    }

    public class RequisicaoAprovarReprovar
    {
        public Int64 codigo { get; set; }
        public int codigo_categoria { get; set; }
        public string categoria { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public int codigo_setor { get; set; }
        public string setor { get; set; }
        public long codigo_equipamento { get; set; }
        public string equipamento { get; set; }
        public string local { get; set; }
        public string solicitante { get; set; }
        public int codigo_apartamento { get; set; }
        public string apartamento { get; set; }
        public string numero_requisicao { get; set; }
        public string data { get; set; }
        public string descricao { get; set; }
        public int codigo_prioridade { get; set; }
        public string prioridade { get; set; }
        public string arquivo { get; set; }
        public int status { get; set; }
        public string status_descricao { get; set; }
        public string status_css { get; set; }
        public string imagem { get; set; }
    }

    public class PCMCronogramaSemana
    {
        public long codigo_pcm_programada { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public int codigo { get; set; }
        public string descricao { get; set; }
        public int codigo_setor { get; set; }
        public string setor { get; set; }
        public long codigo_equipamento { get; set; }
        public string equipamento { get; set; }
        public string tempo_estimado { get; set; }
        public string prioridade { get; set; }
        public string periodicidade { get; set; }
        public int intervalo { get; set; }
        public string semana1 { get; set; }
        public string semana2 { get; set; }
        public string semana3 { get; set; }
        public string semana4 { get; set; }
        public string semana5 { get; set; }
        public string semana6 { get; set; }
        public string semana7 { get; set; }
        public string semana8 { get; set; }
        public string semana9 { get; set; }
        public string semana10 { get; set; }
        public string semana11 { get; set; }
        public string semana12 { get; set; }
        public string semana13 { get; set; }
        public string semana14 { get; set; }
        public string semana15 { get; set; }
        public string semana16 { get; set; }
        public string semana17 { get; set; }
        public string semana18 { get; set; }
        public string semana19 { get; set; }
        public string semana20 { get; set; }
        public string semana21 { get; set; }
        public string semana22 { get; set; }
        public string semana23 { get; set; }
        public string semana24 { get; set; }
        public string semana25 { get; set; }
        public string semana26 { get; set; }
        public string semana27 { get; set; }
        public string semana28 { get; set; }
        public string semana29 { get; set; }
        public string semana30 { get; set; }
        public string semana31 { get; set; }
        public string semana32 { get; set; }
        public string semana33 { get; set; }
        public string semana34 { get; set; }
        public string semana35 { get; set; }
        public string semana36 { get; set; }
        public string semana37 { get; set; }
        public string semana38 { get; set; }
        public string semana39 { get; set; }
        public string semana40 { get; set; }
        public string semana41 { get; set; }
        public string semana42 { get; set; }
        public string semana43 { get; set; }
        public string semana44 { get; set; }
        public string semana45 { get; set; }
        public string semana46 { get; set; }
        public string semana47 { get; set; }
        public string semana48 { get; set; }
        public string semana49 { get; set; }
        public string semana50 { get; set; }
        public string semana51 { get; set; }
        public string semana52 { get; set; }
        public string total_semana1 { get; set; }
        public string total_semana2 { get; set; }
        public string total_semana3 { get; set; }
        public string total_semana4 { get; set; }
        public string total_semana5 { get; set; }
        public string total_semana6 { get; set; }
        public string total_semana7 { get; set; }
        public string total_semana8 { get; set; }
        public string total_semana9 { get; set; }
        public string total_semana10 { get; set; }
        public string total_semana11 { get; set; }
        public string total_semana12 { get; set; }
        public string total_semana13 { get; set; }
        public string total_semana14 { get; set; }
        public string total_semana15 { get; set; }
        public string total_semana16 { get; set; }
        public string total_semana17 { get; set; }
        public string total_semana18 { get; set; }
        public string total_semana19 { get; set; }
        public string total_semana20 { get; set; }
        public string total_semana21 { get; set; }
        public string total_semana22 { get; set; }
        public string total_semana23 { get; set; }
        public string total_semana24 { get; set; }
        public string total_semana25 { get; set; }
        public string total_semana26 { get; set; }
        public string total_semana27 { get; set; }
        public string total_semana28 { get; set; }
        public string total_semana29 { get; set; }
        public string total_semana30 { get; set; }
        public string total_semana31 { get; set; }
        public string total_semana32 { get; set; }
        public string total_semana33 { get; set; }
        public string total_semana34 { get; set; }
        public string total_semana35 { get; set; }
        public string total_semana36 { get; set; }
        public string total_semana37 { get; set; }
        public string total_semana38 { get; set; }
        public string total_semana39 { get; set; }
        public string total_semana40 { get; set; }
        public string total_semana41 { get; set; }
        public string total_semana42 { get; set; }
        public string total_semana43 { get; set; }
        public string total_semana44 { get; set; }
        public string total_semana45 { get; set; }
        public string total_semana46 { get; set; }
        public string total_semana47 { get; set; }
        public string total_semana48 { get; set; }
        public string total_semana49 { get; set; }
        public string total_semana50 { get; set; }
        public string total_semana51 { get; set; }
        public string total_semana52 { get; set; }
    }

    public class PlanoAcao
    {
        public long codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string numero_documento { get; set; }
        public string data_abertura { get; set; }
        public string codigo_usuario_solicitante { get; set; }
        public string solicitante { get; set; }
        public int codigo_departamento { get; set; }
        public string departamento { get; set; }
        public int codigo_responsavel { get; set; }
        public string responsavel { get; set; }
        public string data_necessidade { get; set; }
        public string descricao { get; set; }
        public int codigo_prioridade { get; set; }
        public string prioridade { get; set; }
        public string data_execucao { get; set; }
        public int status { get; set; }
        public string status_descricao { get; set; }
        public string percentual { get; set; }
        public string css_class { get; set; }
        public string dias { get; set; }
        public List<PictureModels> fotos { get; set; }
    }

    public class PlanoAcaoStatus
    {
        public string pendente { get; set; }
        public string concluido { get; set; }
        public string atrasado { get; set; }
        public string em_andamento { get; set; }
    }

    public class PlanoAcaoApontamento
    {
        public string item { get; set; }
        public long codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string plano_acao { get; set; }
        public string prioridade { get; set; }
        public string departamento { get; set; }
        public string responsavel { get; set; }
        public string descricao { get; set; }
        public string observacao { get; set; }
        public string executor { get; set; }
        public string data { get; set; }
        public string percentual { get; set; }
        public int codigo_justificativa_apontamento { get; set; }
        public string justificativa_apontamento { get; set; }
        public string valor { get; set; }
        public List<PictureModels> fotos { get; set; }
    }

    public class defaultResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string documentNumber { get; set; }
    }

}
