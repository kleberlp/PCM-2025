using System;
using System.Collections.Generic;

namespace PCM.WEB.MODELS
{
    public class CustoHorasTrabalhadas
    {
        public string funcionario { get; set; }
        public int codigo_funcionario { get; set; }
        public int codigo_unidade { get; set; }
        public List<CustoHorasTrabalhadasMesValor> valores { get; set; }
        public string total { get; set; }
        public string falta { get; set; }
    }

    public class CustoHorasTrabalhadasMesValor
    {
        public int mes { get; set; }
        public int ano { get; set; }
        public string valor { get; set; }
        public string falta { get; set; }
    }

    public class chartCustoHorasTrabalhadas
    {
        public string nome { get; set; }
        public List<string> valor { get; set; }
    }

    public class FuncionarioHorasTrabalhadas
    {
        public string unidade { get; set; }
        public string funcionario { get; set; }
        public int codigo_funcionario { get; set; }
        public int codigo_unidade { get; set; }
        public List<FuncionarioHorasTrabalhadasMesValor> valores { get; set; }
        public string total { get; set; }
        public string falta { get; set; }
        public long minutos { get; set; }
        public string total_horas { get; set; }
    }

    public class FuncionarioHorasTrabalhadasMesValor
    {
        public int mes { get; set; }
        public int ano { get; set; }
        public long minutos { get; set; }
        public long falta_minutos { get; set; }
        public string valor { get; set; }
        public string falta { get; set; }
    }

    public class FuncionarioHoras
    {
        public int sequence { get; set; }
        public int codigoUnidade { get; set; }
        public int codigoFuncionario { get; set; }
        public string funcionario { get; set; }
        public string janeiroHoras { get; set; } = "00:00";
        public string fevereiroHoras { get; set; } = "00:00";
        public string marcoHoras { get; set; } = "00:00";
        public string abrilHoras { get; set; } = "00:00";
        public string maioHoras { get; set; } = "00:00";
        public string junhoHoras { get; set; } = "00:00";
        public string julhoHoras { get; set; } = "00:00";
        public string agostoHoras { get; set; } = "00:00";
        public string setembroHoras { get; set; } = "00:00";
        public string outubroHoras { get; set; } = "00:00";
        public string novembroHoras { get; set; } = "00:00";
        public string dezembroHoras { get; set; } = "00:00";
        public string totalHoras { get; set; } = "00:00";
        public string janeiroFaltas { get; set; } = "00:00";
        public string fevereiroFaltas { get; set; } = "00:00";
        public string marcoFaltas { get; set; } = "00:00";
        public string abrilFaltas { get; set; } = "00:00";
        public string maioFaltas { get; set; } = "00:00";
        public string junhoFaltas { get; set; } = "00:00";
        public string julhoFaltas { get; set; } = "00:00";
        public string agostoFaltas { get; set; } = "00:00";
        public string setembroFaltas { get; set; } = "00:00";
        public string outubroFaltas { get; set; } = "00:00";
        public string novembroFaltas { get; set; } = "00:00";
        public string dezembroFaltas { get; set; } = "00:00";
        public string totalFaltas { get; set; } = "00:00";
    }

    public class chartFuncionarioHorasTrabalhadas
    {
        public string nome { get; set; }
        public List<string> valor { get; set; }
    }

    public class FuncionarioOciosidade
    {
        public string funcionario { get; set; }
        public string tipo_funcionario { get; set; }
        public int codigo_funcionario { get; set; }
        public int codigo_unidade { get; set; }
        public List<FuncionarioOciosidadeMesValor> valores { get; set; }
        public string total { get; set; }
        public string falta { get; set; }
    }

    public class FuncionarioOciosidadeMesValor
    {
        public int mes { get; set; }
        public int ano { get; set; }
        public string valor { get; set; }
        public string horas { get; set; }
        public string falta { get; set; }
    }

    public class chartFuncionarioOciosidade
    {
        public string nome { get; set; }
        public List<string> valor { get; set; }
    }

    public class GreenPlanet
    {
        public int codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string data_inicio { get; set; }
        public string data_termino { get; set; }
        public int codigo_fomula_calculo { get; set; }
        public string descricao { get; set; }
        public string data { get; set; }
        public string quantidade_hospedes { get; set; }
        public string quartos_ocupados { get; set; }
        public float valor { get; set; }
        public string valor_texto { get; set; }
        public bool erro_quantidade_hospede { get; set; }
        public bool erro_quartos_ocupados { get; set; }
        public bool erro_valor { get; set; }
        public string unidade_medida { get; set; }
        public string medicao { get; set; }
        public int foto { get; set; }
    }

    public class GreenPlanetLancamento
    {
        public int codigo { get; set; }
        public string item_medicao { get; set; }
        public string data { get; set; }
        public string quantidade_hospedes { get; set; }
        public string quartos_ocupados { get; set; }
        public string valor { get; set; }
        public string valor_anterior { get; set; }
        public string consumo { get; set; }
        public string unidade_medida { get; set; }
    }

    public class chartGreenPlanet
    {
        public string item_medicao { get; set; }
        public List<string> valor { get; set; }
    }

    public class ManutencaoAbertoConcluido
    {
        public string dia { get; set; }
        public int aberto { get; set; }
        public int concluido { get; set; }
        public int saldo { get; set; }
    }

    public class RelatorioLogBook
    {
        public DateTime data { get; set; }
        public string usuario { get; set; }
        public string descricao { get; set; }
    }

    public class chartManutencaoAbertoConcluido
    {
        public string data { get; set; }
        public long aberto { get; set; }
        public long concluido { get; set; }
        public long saldo { get; set; }
    }

    public class ManutencaoCategoria
    {
        public long item { get; set; }
        public string categoria { get; set; }
        public List<ManutencaoCategoriaMesValor> valores { get; set; }
        public string total { get; set; }
    }

    public class ManutencaoCategoriaMesValor
    {
        public string valor { get; set; }
    }

    public class chartManutencaoCategoria
    {
        public string categoria { get; set; }
        public List<string> valor { get; set; }
    }

    public class ManutencaoEquipamento
    {
        public int codigo_unidade { get; set; }
        public long codigo_equipamento { get; set; }
        public long item { get; set; }
        public string equipamento { get; set; }
        public List<ManutencaoEquipamentoMesValor> valores { get; set; }
        public string total { get; set; }
    }

    public class ManutencaoEquipamentoMesValor
    {
        public int mes { get; set; }
        public int ano { get; set; }
        public string valor { get; set; }
    }

    public class chartManutencaoEquipamento
    {
        public string equipamento { get; set; }
        public List<string> valor { get; set; }
    }

    public class ManutencaoExecutor
    {
        public string executor { get; set; }
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
        public int total { get; set; }
    }

    public class chartManutencaoExecutor
    {
        public string executor { get; set; }
        public List<string> valor { get; set; }
    }

    public class ManutencaoSetor
    {
        public int item { get; set; }
        public string setor { get; set; }
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
        public int total { get; set; }
    }

    public class chartManutencaoSetor
    {
        public string setor { get; set; }
        public List<string> valor { get; set; }
    }

    public class ManutencaoSolicitante
    {
        public string solicitante { get; set; }
        public string departamento { get; set; }
        public int janeiro { get; set; } = 0;
        public int fevereiro { get; set; } = 0;
        public int marco { get; set; } = 0;
        public int abril { get; set; } = 0;
        public int maio { get; set; } = 0;
        public int junho { get; set; } = 0;
        public int julho { get; set; } = 0;
        public int agosto { get; set; } = 0;
        public int setembro { get; set; } = 0;
        public int outubro { get; set; } = 0;
        public int novembro { get; set; } = 0;
        public int dezembro { get; set; } = 0;
        public int total { get; set; } = 0;
    }

    public class Manutencao
    {
        public string descricao { get; set; }
        public string unidade { get; set; }
        public int janeiro { get; set; } = 0;
        public int fevereiro { get; set; } = 0;
        public int marco { get; set; } = 0;
        public int abril { get; set; } = 0;
        public int maio { get; set; } = 0;
        public int junho { get; set; } = 0;
        public int julho { get; set; } = 0;
        public int agosto { get; set; } = 0;
        public int setembro { get; set; } = 0;
        public int outubro { get; set; } = 0;
        public int novembro { get; set; } = 0;
        public int dezembro { get; set; } = 0;
        public int total { get; set; } = 0;
    }

    public class chartManutencaoSolicitante
    {
        public string solicitante { get; set; }
        public List<string> valor { get; set; }
    }

    public class ManutencaoTipoOrdemServico
    {
        public string mes { get; set; }
        public int corretiva { get; set; }
        public int melhoria { get; set; }
        public int preventiva { get; set; }
        public int preditiva { get; set; }
        public int checklist { get; set; }
        public int total { get; set; }
    }

    public class chartManutencaoTipoOrdemServico
    {
        public string tipo { get; set; }
        public string valor { get; set; }
    }

    public class ManutencaoTempoMedioAtendimento
    {
        public string mes { get; set; }
        public string no_dia { get; set; }
        public string um_dia { get; set; }
        public string tres_dias { get; set; }
        public string cinco_dias { get; set; }
        public string acima_cinco_dias { get; set; }
    }

    public class chartManutencaoTempoMedioAtendimento
    {
        public string tipo { get; set; }
        public float valor { get; set; }
    }

    public class PMOCAno
    {
        public int ano { get; set; }
        public List<PMOCAnoUnidade> unidade { get; set; }
    }

    public class PMOCAnoUnidade
    {
        public string unidade { get; set; }
        public int codigo_unidade { get; set; }
        public List<PMOCAnoUnidadeMes> mes { get; set; }
        public List<PMOCAnoUnidadeBimestral> bimestre { get; set; }
    }

    public class PMOCAnoUnidadeMes
    {
        public int mes { get; set; }
        public int ano { get; set; }
        public string descricao { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }

    public class PMOCAnoUnidadeBimestral
    {
        public int bimestre { get; set; }
        public string descricao { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }

    public class PMOCAnoBimestre
    {
        public int ano { get; set; }
        public List<PMOCAnoUnidadeBimestre> unidade { get; set; }
    }

    public class PMOCAnoUnidadeBimestre
    {
        public string unidade { get; set; }
        public int codigo_unidade { get; set; }
        public List<PMOCAnoUnidadeMesBimestre> mes { get; set; }
    }

    public class PMOCAnoUnidadeMesBimestre
    {
        public int mes { get; set; }
        public string descricao { get; set; }
    }

    #region ::: PREVENTIVA MÊS :::

    public class PreventivaMes
    {
        public int ano { get; set; }
        public int mes { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string descricao { get; set; }
        public int linha { get; set; }
    }

    #endregion

    public class NaoConformidade    
    {
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string tipo { get; set; }
        public long codigo_pcm_programada { get; set; }
        public string manutencao_programada { get; set; }
        public string descricao { get; set; }
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
    }

    public class RelatorioDinamicoDia
    {
        public string checklist { get; set; }
        public string descricao { get; set; }
        public string grupo { get; set; }
        public string subgrupo { get; set; }
        public string valorMinimo { get; set; }
        public string valorMaximo { get; set; }
        public int codigoTipoItemChecklist { get; set; }
        public string dia1 { get; set; }
        public string dia2 { get; set; }
        public string dia3 { get; set; }
        public string dia4 { get; set; }
        public string dia5 { get; set; }
        public string dia6 { get; set; }
        public string dia7 { get; set; }
        public string dia8 { get; set; }
        public string dia9 { get; set; }
        public string dia10 { get; set; }
        public string dia11 { get; set; }
        public string dia12 { get; set; }
        public string dia13 { get; set; }
        public string dia14 { get; set; }
        public string dia15 { get; set; }
        public string dia16 { get; set; }
        public string dia17 { get; set; }
        public string dia18 { get; set; }
        public string dia19 { get; set; }
        public string dia20 { get; set; }
        public string dia21 { get; set; }
        public string dia22 { get; set; }
        public string dia23 { get; set; }
        public string dia24 { get; set; }
        public string dia25 { get; set; }
        public string dia26 { get; set; }
        public string dia27 { get; set; }
        public string dia28 { get; set; }
        public string dia29 { get; set; }
        public string dia30 { get; set; }
        public string dia31 { get; set; }
    }

    public class RelatorioConsumoEnxoval
    {
        public string enxoval { get; set; }
        public string peso { get; set; } 
        public string total { get; set; }
        public int dia1 { get; set; } = 0;
        public int dia2 { get; set; } = 0;
        public int dia3 { get; set; } = 0;
        public int dia4 { get; set; } = 0;
        public int dia5 { get; set; } = 0;
        public int dia6 { get; set; } = 0;
        public int dia7 { get; set; } = 0;
        public int dia8 { get; set; } = 0;
        public int dia9 { get; set; } = 0;
        public int dia10 { get; set; } = 0;
        public int dia11 { get; set; } = 0;
        public int dia12 { get; set; } = 0;
        public int dia13 { get; set; } = 0;
        public int dia14 { get; set; } = 0;
        public int dia15 { get; set; } = 0;
        public int dia16 { get; set; } = 0;
        public int dia17 { get; set; } = 0;
        public int dia18 { get; set; } = 0;
        public int dia19 { get; set; } = 0;
        public int dia20 { get; set; } = 0;
        public int dia21 { get; set; } = 0;
        public int dia22 { get; set; } = 0;
        public int dia23 { get; set; } = 0;
        public int dia24 { get; set; } = 0;
        public int dia25 { get; set; } = 0;
        public int dia26 { get; set; } = 0;
        public int dia27 { get; set; } = 0;
        public int dia28 { get; set; } = 0;
        public int dia29 { get; set; } = 0;
        public int dia30 { get; set; } = 0;
        public int dia31 { get; set; } = 0;
    }
    
    public class RelatorioCamareiraUH
    {
        public string camareira { get; set; }
        public string total { get; set; }
        public int dia1 { get; set; } = 0;
        public int dia2 { get; set; } = 0;
        public int dia3 { get; set; } = 0;
        public int dia4 { get; set; } = 0;
        public int dia5 { get; set; } = 0;
        public int dia6 { get; set; } = 0;
        public int dia7 { get; set; } = 0;
        public int dia8 { get; set; } = 0;
        public int dia9 { get; set; } = 0;
        public int dia10 { get; set; } = 0;
        public int dia11 { get; set; } = 0;
        public int dia12 { get; set; } = 0;
        public int dia13 { get; set; } = 0;
        public int dia14 { get; set; } = 0;
        public int dia15 { get; set; } = 0;
        public int dia16 { get; set; } = 0;
        public int dia17 { get; set; } = 0;
        public int dia18 { get; set; } = 0;
        public int dia19 { get; set; } = 0;
        public int dia20 { get; set; } = 0;
        public int dia21 { get; set; } = 0;
        public int dia22 { get; set; } = 0;
        public int dia23 { get; set; } = 0;
        public int dia24 { get; set; } = 0;
        public int dia25 { get; set; } = 0;
        public int dia26 { get; set; } = 0;
        public int dia27 { get; set; } = 0;
        public int dia28 { get; set; } = 0;
        public int dia29 { get; set; } = 0;
        public int dia30 { get; set; } = 0;
        public int dia31 { get; set; } = 0;
    }

    public class RelatorioResponsavelVistoriaUH
    {
        public string responsavelVistoria { get; set; }
        public string total { get; set; }
        public int dia1 { get; set; } = 0;
        public int dia2 { get; set; } = 0;
        public int dia3 { get; set; } = 0;
        public int dia4 { get; set; } = 0;
        public int dia5 { get; set; } = 0;
        public int dia6 { get; set; } = 0;
        public int dia7 { get; set; } = 0;
        public int dia8 { get; set; } = 0;
        public int dia9 { get; set; } = 0;
        public int dia10 { get; set; } = 0;
        public int dia11 { get; set; } = 0;
        public int dia12 { get; set; } = 0;
        public int dia13 { get; set; } = 0;
        public int dia14 { get; set; } = 0;
        public int dia15 { get; set; } = 0;
        public int dia16 { get; set; } = 0;
        public int dia17 { get; set; } = 0;
        public int dia18 { get; set; } = 0;
        public int dia19 { get; set; } = 0;
        public int dia20 { get; set; } = 0;
        public int dia21 { get; set; } = 0;
        public int dia22 { get; set; } = 0;
        public int dia23 { get; set; } = 0;
        public int dia24 { get; set; } = 0;
        public int dia25 { get; set; } = 0;
        public int dia26 { get; set; } = 0;
        public int dia27 { get; set; } = 0;
        public int dia28 { get; set; } = 0;
        public int dia29 { get; set; } = 0;
        public int dia30 { get; set; } = 0;
        public int dia31 { get; set; } = 0;
    }

    public class RelatorioCamareiraNC
    {
        public string camareira { get; set; }
        public string naoConformidade { get; set; }
        public string total { get; set; }
        public string dia1 { get; set; } = "";
        public string dia2 { get; set; } = "";
        public string dia3 { get; set; } = "";
        public string dia4 { get; set; } = "";
        public string dia5 { get; set; } = "";
        public string dia6 { get; set; } = "";
        public string dia7 { get; set; } = "";
        public string dia8 { get; set; } = "";
        public string dia9 { get; set; } = "";
        public string dia10 { get; set; } = "";
        public string dia11 { get; set; } = "";
        public string dia12 { get; set; } = "";
        public string dia13 { get; set; } = "";
        public string dia14 { get; set; } = "";
        public string dia15 { get; set; } = "";
        public string dia16 { get; set; } = "";
        public string dia17 { get; set; } = "";
        public string dia18 { get; set; } = "";
        public string dia19 { get; set; } = "";
        public string dia20 { get; set; } = "";
        public string dia21 { get; set; } = "";
        public string dia22 { get; set; } = "";
        public string dia23 { get; set; } = "";
        public string dia24 { get; set; } = "";
        public string dia25 { get; set; } = "";
        public string dia26 { get; set; } = "";
        public string dia27 { get; set; } = "";
        public string dia28 { get; set; } = "";
        public string dia29 { get; set; } = "";
        public string dia30 { get; set; } = "";
        public string dia31 { get; set; } = "";
    }

}
