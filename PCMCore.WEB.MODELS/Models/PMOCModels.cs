using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCM.WEB.MODELS
{
    public class PMOCOrdemServico
    {
        public long codigo_pmoc_ordem_servico { get; set; }
        public long codigo_equipamento { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string equipamento { get; set; }
        public string apartamento { get; set; }
        public string setor { get; set; }
        public string tipo_ar_condicionado { get; set; }
        public string data { get; set; }
        public int status { get; set; }
        public bool aponta_horas { get; set; }
        public List<PMOCOrdemServicoApontamento> apontamento { get; set; }
        public List<PMOCOrdemServicoChecklist> checklist { get; set; }
    }

    public class PMOCOrdemServicoApontamento
    {
        public long codigo_pmoc_ordem_servico { get; set; }
        public int codigo { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo_executor { get; set; }
        public string executor { get; set; }
        public string data_inicio { get; set; }
        public string data_termino { get; set; }
    }

    public class PMOCOrdemServicoChecklist
    {
        public long codigo_pmoc_ordem_servico { get; set; }
        public long codigo_equipamento { get; set; }
        public string data_manutencao { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo { get; set; }
        public int codigo_tipo_item_checklist { get; set; }
        public string tipo_checklist { get; set; }
        public string grupo { get; set; }
        public string subgrupo { get; set; }
        public string checklist { get; set; }
        public string descricao { get; set; }
        public bool allow_picture { get; set; }
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

    public class PMOCApontamento
    {
        public long codigo_pmoc_ordem_servico { get; set; }
        public long codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public long codigo_equipamento { get; set; }
        public string equipamento { get; set; }
        public string tipo_ar_condicionado { get; set; }
        public string setor { get; set; }
        public string apartamento { get; set; }
        public int codigo_funcionario { get; set; }
        public int codigo_fornecedor { get; set; }
        public string funcionario { get; set; }
        public string fornecedor { get; set; }
        public string data_inicio { get; set; }
        public string data_termino { get; set; }
        public string hora_inicio { get; set; }
        public string hora_termino { get; set; }
        public bool aponta_horas { get; set; }
        public bool concluido { get; set; }
    }

    public class PMOC2
    {
        public string unidade { get; set; }
        public int codigo_unidade { get; set; }
        public string tipo_ar_condicionado { get; set; }
        public int codigo_tipo_ar_condicionado { get; set; }
        public long codigo_pmoc_ordem_servico { get; set; }
        public int codigo_equipamento { get; set; }
        public long codigo { get; set; }
        public string data_ultima { get; set; }
        public string data_proxima { get; set; }
        public string data_proxima_formatada { get; set; }
        public string tag { get; set; }
        public string descricao { get; set; }
        public string cor { get; set; }
        public string periodicidade { get; set; }
        public int status { get; set; }
        public string descricao_status { get; set; }
        public string linha { get; set; }
    }

    public class PMOCMesFechado
    {
        public string unidade { get; set; }
        public int codigo_unidade { get; set; }
        public string tipo_ar_condicionado { get; set; }
        public int codigo_tipo_ar_condicionado { get; set; }
        public long codigo_pmoc_ordem_servico { get; set; }
        public string andar { get; set; }
        public int codigo_equipamento { get; set; }
        public int quantidade_executada { get; set; }
        public int quantidade_necessaria { get; set; }
        public long codigo { get; set; }
        public string data_ultima { get; set; }
        public string data_proxima { get; set; }
        public string data_proxima_formatada { get; set; }
        public string tag { get; set; }
        public string descricao { get; set; }
        public string cor { get; set; }
        public string periodicidade { get; set; }
        public int status { get; set; }
        public string descricao_status { get; set; }
        public string linha { get; set; }
    }

    public class PMOCUnidade
    {
        public string unidade { get; set; }
        public int codigo_unidade { get; set; }
        public bool registro { get; set; }
        public List<PMOCTipoArCondicionado> tipo_ar_condicionado { get; set; }
    }

    public class PMOCTipoArCondicionado
    {
        public string tipo_ar_condicionado { get; set; }
        public int codigo_tipo_ar_condicionado { get; set; }
        public bool registro { get; set; }
        public List<PMOCEquipamento> equipamento { get; set; }
    }

    public class PMOCEquipamento
    {
        public long codigo_pmoc_ordem_servico { get; set; }    
        public int codigo_equipamento { get; set; }
        public long codigo { get; set; }
        public string data { get; set; }
        public string tag { get; set; }
        public string descricao { get; set; }
        public string cor { get; set; }
        public string periodicidade { get; set; }
        public int status { get; set; }
        public string descricao_status { get; set; }
    }

    public class PMOCStatus
    {
        public int quantidade_pendente { get; set; }
        public int quantidade_concluido { get; set; }
        public int quantidade_atrasado { get; set; }
        public int quantidade_em_andamento { get; set; }
    }

    public class PMOCHistorico
    {
        public string unidade { get; set; }
        public string data { get; set; }
        public string equipamento { get; set; }
        public string tipo_ar_condicionado { get; set; }
        public string status { get; set; }
        public long codigo { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo_empresa { get; set; }
    }

    public class PMOCCadastro
    {
        public int codigo { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo_empresa { get; set; }
        public string unidade { get; set; }
        public string tipo_servico { get; set; }
        public int codigo_empresa_pmoc { get; set; }
        public int codigo_unidade_pmoc { get; set; }
        public int codigo_tipo_servico { get; set; }
        public int codigo_responsavel_legal { get; set; }
        public string responsavel_tecnico_pmoc { get; set; }
        public string numero_art_pmoc { get; set; }
        public string data_inicio_vigencia_art_pmoc { get; set; }
        public string data_termino_vigencia_art_pmoc { get; set; }
    }

    public class PMOCCronograma
    {
        public string unidade { get; set; }
        public string tag { get; set; }
        public string descricao { get; set; }
        public string mes_1 { get; set; }
        public string mes_2 { get; set; }
        public string mes_3 { get; set; }
        public string mes_4 { get; set; }
        public string mes_5 { get; set; }
        public string mes_6 { get; set; }
        public string mes_7 { get; set; }
        public string mes_8 { get; set; }
        public string mes_9 { get; set; }
        public string mes_10 { get; set; }
        public string mes_11 { get; set; }
        public string mes_12 { get; set; }
    }

    public class PMOCAndar
    {
        public string andar { get; set; }
        public List<PMOCAndarEquipamento> equipamentos { get; set; }
    }

    public class PMOCAndarEquipamento
    {
        public long id { get; set; }
        public string value { get; set; }
        public string type { get; set; }
    }
}
