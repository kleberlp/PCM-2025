using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCM.WEB.MODELS
{

    public class ChartResumoServicos
    {
        public List<string> label { get; set; }
        public List<string> os { get; set; }
        public List<string> programada { get; set; }
        public List<string> pmoc { get; set; }
        public List<string> uh { get; set; }
    }

    public class InfoOrdemServico
    {
        public string unidade { get; set; }
        public int quantidade_pendente_pool { get; set; }
        public int quantidade_concluida_pool { get; set; }
        public int quantidade_andamento_pool { get; set; }
        public int quantidade_vinculada_pool { get; set; }
        public int quantidade_pendente_condo { get; set; }
        public int quantidade_concluida_condo { get; set; }
        public int quantidade_andamento_condo { get; set; }
        public int quantidade_vinculada_condo { get; set; }
        public float nota_auditoria_externa { get; set; }
        public float nota_avaliacao_mensal { get; set; }
        public string ranking { get; set; }
        public string arquivo_auditoria_externa { get; set; }
        public string log_book { get; set; }
    }

    public class InfoOrdemServicoNew
    {
        public string unidade { get; set; }
        public int quantidade_atrasada_pool { get; set; }
        public int quantidade_pendente_pool { get; set; }
        public int quantidade_concluida_pool { get; set; }
        public int quantidade_andamento_pool { get; set; }
        public int quantidade_vinculada_pool { get; set; }
        public int quantidade_atrasada_condo { get; set; }
        public int quantidade_pendente_condo { get; set; }
        public int quantidade_concluida_condo { get; set; }
        public int quantidade_andamento_condo { get; set; }
        public int quantidade_vinculada_condo { get; set; }
        public float nota_auditoria_externa { get; set; }
        public float nota_avaliacao_mensal { get; set; }
        public string ranking { get; set; }
        public string arquivo_auditoria_externa { get; set; }
        public string log_book { get; set; }
    }

    public class ChartGauge
    {
        public float laudo { get; set; }
        public float preventiva { get; set; }
        public float uh_dia { get; set; }
        public float pmoc { get; set; }
        public float rotina { get; set; }
        public float green_planet { get; set; }
        public string cor_laudo { get; set; }
        public string cor_preventiva { get; set; }
        public string cor_uh_dia { get; set; }
        public string cor_pmoc { get; set; }
        public string cor_rotina { get; set; }
        public string cor_green_planet { get; set; }
    }

    public class DashboardInfoMain
    {
        public string unidade { get; set; }
        public int quantidadeOSGerada { get; set; }
        public int quantidadeOSAtendida { get; set; }
        public int quantidadeOSPendente { get; set; }
        public float laudo { get; set; }
        public float preventiva { get; set; }
        public float rotina { get; set; }
        public float pmoc { get; set; }
        public float uhDia { get; set; }
        public float greenPlanet { get; set; }
        public string corLaudo { get; set; }
        public string corPreventiva { get; set; }
        public string corRotina { get; set; }
        public string corPMOC { get; set; }
        public string corUHDia { get; set; }
        public string corGreenPlanet { get; set; }
    }

    public class PrincipaisOcorrencias
    {
        public int item { get; set; }
        public string local { get; set; }
        public long quantidade { get; set; }
        public string horas { get; set; }
    }

    public class ResumoEmpresa
    {
        public string empresa { get; set; }
        public string unidade { get; set; }
        public int quantidade_unidade { get; set; }
        public string numero_os_aberto { get; set; }
        public string numero_os_concluido { get; set; }
        public string saldo { get; set; }
        public string horas_corretivas { get; set; }
        public string horas_preventivas { get; set; }
        public string horas_pmoc { get; set; }
        public string horas_uh { get; set; }
    }

    public class Chart
    {
        public string descricao { get; set; }
        public long valor { get; set; }
    }

    public class Notificacao
    {
        public long codigo { get; set; }
        public int codigo_empresa { get; set; }
        public string modulo { get; set; }
        public string descricao { get; set; }
        public string data_input { get; set; }
        public string data_necessidade { get; set; }
        public string css { get; set; }
        public string link { get; set; }
        public bool visto { get; set; }
    }

    public class NotificacaoOSHoespede
    {
        public string message { get; set; }
        public string type { get; set; }
        public int delay { get; set; }
        public int timer { get; set; }
        public string url { get; set; }
    }

}
