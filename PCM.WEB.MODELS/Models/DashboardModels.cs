using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCM.WEB.MODELS
{
    public class chartLinhas
    {
        public string serie { get; set; }
        public List<string> valor { get; set; }
    }

    public class DashboardInfo
    {
        public string unidade { get; set; }
        public string data_implantacao { get; set; }
        public string quantidade_usuario { get; set; }
        public string quantidade_equipamento { get; set; }
        public string equipe_manutencao { get; set; }
        public string quantidade_os { get; set; }
        public string hh_apontado { get; set; }
        public string hh_corretiva { get; set; }
        public string hh_preventiva { get; set; }
        public string corretiva_preventiva { get; set; }
        public string rotinas_implantadas { get; set; }
        public string preventivas_implantadas { get; set; }
        public string laudo { get; set; }
        public string preventiva { get; set; }
        public string rotina { get; set; }
        public string pmoc { get; set; }
        public string uh_dia { get; set; }
        public string os_atendimento_dia { get; set; }
        public string os_atendimento_um_dia { get; set; }
        public string os_atendimento_tres_dias { get; set; }
        public string os_atendimento_cinco_dias { get; set; }
        public string os_atendimento_acima_cinco_dias { get; set; }
        public string os_nao_atendido { get; set; }
        public string quantidade_colaborador_proprio { get; set; }
        public string quantidade_colaborador_terceiro { get; set; }
        public string hh_disponivel { get; set; }
        public string faltas_justificadas { get; set; }
        public string faltas_nao_justificadas { get; set; }
        public string horas_corretivas { get; set; }
        public string percentual_corretiva { get; set; }
        public string horas_preventivas { get; set; }
        public string percentual_preventiva { get; set; }
        public string horas_pmoc { get; set; }
        public string percentual_pmoc { get; set; }
        public string horas_uh { get; set; }
        public string percentual_uh { get; set; }
        public string horas_mapa_manutencao { get; set; }
        public string percentual_mapa_manutencao { get; set; }
        public string horas_ociosas { get; set; }
        public string percentual_horas_ociosas { get; set; }
        public string quantidade_os_gerada { get; set; }
        public string quantidade_os_atendida { get; set; }
        public string quantidade_os_pendente { get; set; }
        public string ranking { get; set; }
    }

    public class RankingUnidades
    {
        public int ranking{ get; set; }
        public string unidade { get; set; }
        public string nota { get; set; }
    }

    public class NotasUnidades
    {
        public string unidade { get; set; }
        public string nota_final { get; set; }
        public string laudo { get; set; }
        public string laudo_peso { get; set; }
        public string preventiva { get; set; }
        public string preventiva_peso { get; set; }
        public string rotina { get; set; }
        public string rotina_peso { get; set; }
        public string pmoc { get; set; }
        public string pmoc_peso { get; set; }
        public string os_atendimento_dia { get; set; }
        public string os_atendimento_dia_peso { get; set; }
        public string uh_dia { get; set; }
        public string uh_dia_peso { get; set; }
        public string hh_nao_apontado { get; set; }
        public string hh_nao_apontado_peso { get; set; }
        public string os_pendente { get; set; }
        public string os_pendente_peso { get; set; }
        public string hh_extra { get; set; }
        public string hh_extra_peso { get; set; }
        public string preventiva_corretiva { get; set; }
        public string preventiva_corretiva_peso { get; set; }
        public string green_planet { get; set; }
        public string green_planet_peso { get; set; }
        public string atend_ok { get; set; }
        public string atend_pend { get; set; }
        public string atend_total { get; set; }
        public string gp_ok { get; set; }
        public string gp_pend { get; set; }
        public string gp_total { get; set; }
        public string hh_nao_apontado_realizado { get; set; }
        public string os_pendente_realizado { get; set; }
        public string hh_extra_realizado { get; set; }
        public string prev_corretiva_realizado { get; set; }
    }

    public class PercentualNota
    {
        public string laudo { get; set; }
        public string preventiva { get; set; }
        public string rotina { get; set; }
        public string pmoc { get; set; }
        public string uh_dia { get; set; }
        public string os_atendimento_dia { get; set; }
        public string hh_nao_apontado { get; set; }
        public string os_pendente { get; set; }
        public string hh_extra { get; set; }
        public string preventiva_corretiva { get; set; }
        public string green_planet { get; set; }
    }

    public class MetricaUnidades
    {
        public string unidade { get; set; }
        public int quantidade_ok { get; set; }
        public int quantidade_pendente { get; set; }
        public int total { get; set; }
        public string percentual_atendido { get; set; }
    }

    public class DashboardAtualData
    {
        public List<MetricaUnidades> metrica_laudo      { get; set; } = new List<MetricaUnidades>();
        public List<MetricaUnidades> metrica_preventiva { get; set; } = new List<MetricaUnidades>();
        public List<MetricaUnidades> metrica_pmoc       { get; set; } = new List<MetricaUnidades>();
        public List<MetricaUnidades> metrica_rotina     { get; set; } = new List<MetricaUnidades>();
        public List<MetricaUnidades> metrica_uh         { get; set; } = new List<MetricaUnidades>();
        public List<NotasUnidades>   notas_unidades     { get; set; } = new List<NotasUnidades>();
        public PercentualNota        percentual_nota     { get; set; } = new PercentualNota();
    }

    public class AtendimentoOrdemServico
    {
        public string unidade { get; set; }
        public string quantidade_colaborador_proprio { get; set; }
        public string quantidade_colaborador_terceiro { get; set; }
        public string hh_disponivel { get; set; }
        public string quantidade_os_gerada { get; set; }
        public string quantidade_os_atendida { get; set; }
        public string quantidade_os_por_funcionario { get; set; }
        public string quantidade_os_pendente { get; set; }
        public string percentual_os_gerada { get; set; }
        public string os_atendimento_dia { get; set; }
        public string os_atendimento_um_dia { get; set; }
        public string os_atendimento_tres_dias { get; set; }
        public string os_atendimento_cinco_dias { get; set; }
        public string os_atendimento_acima_cinco_dias { get; set; }
        public string os_nao_atendido { get; set; }
    }

    public class ApontamentoHoras
    {
        public string unidade { get; set; }
        public string quantidade_colaborador_proprio { get; set; }
        public string quantidade_colaborador_terceiro { get; set; }
        public string hh_disponivel { get; set; }
        public string faltas_justificadas { get; set; }
        public string faltas_nao_justificadas { get; set; }
        public string horas_corretivas { get; set; }
        public string percentual_horas_corretivas { get; set; }
        public string horas_preventivas { get; set; }
        public string percentual_horas_preventivas { get; set; }
        public string horas_pmoc { get; set; }
        public string percentual_horas_pmoc { get; set; }
        public string horas_uh_dia { get; set; }
        public string percentual_horas_uh_dia { get; set; }
        public string total_apontamento { get; set; }
        public string saldo_hh_disponivel { get; set; }
        public string css_saldo_hh_disponivel { get; set; }
        public string percentual_ociosidade { get; set; }
        public string hora_extra { get; set; }
        public string css_row { get; set; }
    }

    public class QualidadeAuditoria
    {
        public string unidade { get; set; }
        public string quantidade_auditoria { get; set; }
        public string quantidade_concluida { get; set; }
        public string quantidade_atrasada { get; set; }
        public string quantidade_em_andamento { get; set; }
        public string percentual_execucao { get; set; }
    }

    public class QualidadeNotasUnidade
    {
        public int codigo_auditoria_interna { get; set; }
        public string auditoria { get; set; }
        public string quantidade_auditoria { get; set; }
        public string quantidade_concluida { get; set; }
        public string total_perguntas { get; set; }
        public string total_conformes { get; set; }
        public string total_nao_conformes { get; set; }
        public string total_na { get; set; }
        public string atrasada { get; set; }
        public string em_andamento { get; set; }
        public string media_nota { get; set; }
    }

    public class QualidadeNotas
    {
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string quantidade_auditoria { get; set; }
        public string quantidade_concluida { get; set; }
        public string total_perguntas { get; set; }
        public string total_conformes { get; set; }
        public string total_nao_conformes { get; set; }
        public string total_na { get; set; }
        public string atrasada { get; set; }
        public string em_andamento { get; set; }
        public string media_nota { get; set; }
    }

    public class QualidadePlanoAcao
    {
        public string unidade { get; set; }
        public string quantidade_plano_acao { get; set; }
        public string quantidade_concluido { get; set; }
        public string quantidade_pendente { get; set; }
        public string quantidade_atrasada { get; set; }
        public string quantidade_em_andamento { get; set; }
        public string tempo_medio_em_andamento { get; set; }
    }

    public class QualidadePlanoAcaoUnidade
    {
        public string departamento_responsavel { get; set; }
        public string quantidade_plano_acao { get; set; }
        public string quantidade_concluido { get; set; }
        public string quantidade_pendente { get; set; }
        public string quantidade_atrasada { get; set; }
        public string tempo_medio_atraso { get; set; }
        public string quantidade_em_andamento { get; set; }
        public string tempo_medio_em_andamento { get; set; }
    }

    public class QualidadePlanoAcaoTodasUnidades
    {
        public string unidade { get; set; }
        public string departamento_responsavel { get; set; }
        public string quantidade_plano_acao { get; set; }
        public string quantidade_concluido { get; set; }
        public string quantidade_pendente { get; set; }
        public string quantidade_atrasada { get; set; }
        public string tempo_medio_atraso { get; set; }
        public string quantidade_em_andamento { get; set; }
        public string tempo_medio_em_andamento { get; set; }
    }

    public class QualidadePlanoAcaoJustificativaTodasUnidades
    {
        public string unidade { get; set; }
        public string justificativa { get; set; }
        public string quantidade { get; set; }
        public string departamento_responsavel { get; set; }
        public string quantidade_concluida { get; set; }
        public string quantidade_atrasada { get; set; }
        public string tempo_medio_em_andamento { get; set; }
    }

    public class QualidadePlanoAcaoJustificativaUnidade
    {
        public string justificativa { get; set; }
        public string quantidade { get; set; }
        public string departamento_responsavel { get; set; }
        public string quantidade_concluida { get; set; }
        public string quantidade_atrasada { get; set; }
        public string tempo_medio_em_andamento { get; set; }
    }

    public class QualidadePlanoAcaoRecorrenciaUnidade
    {
        public string quantidade { get; set; }
        public string item_plano_acao { get; set; }
        public string departamento { get; set; }
        public string tempo_gasto { get; set; }
        public string valor_gasto { get; set; }
    }



    public class DashboardGovernancaInfo
    {
        public string unidade { get; set; }
        public string quantidadeCamareiras { get; set; }
        public string quantidadeSupervisores { get; set; }
        public string uhsArrumadas { get; set; }
        public string uhsPermanencia { get; set; }
        public string uhsSaida { get; set; }
        public string uhsVistoriadas { get; set; }
        public string percentualVistoria { get; set; }
        public string quantidadeOSManutencao { get; set; }
        public string quantidadeNC { get; set; }
        public string quantidadeRetrabalho { get; set; }
        public string quantidadeAlteracaoStatus { get; set; }
    }

}
