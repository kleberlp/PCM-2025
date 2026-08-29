using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCM.WEB.MODELS
{
    
    public class NormasProcedimentos
    {
        public int codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string descricao { get; set; }
        public string comentario { get; set; }
        public string tipo { get; set; }
        public string arquivo { get; set; }
        public string path_arquivo { get; set; }
        public bool ativo { get; set; }
    }

    public class Auditoria
    {
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public int codigo_auditoria_interna { get; set; }
        public long codigo_auditoria { get; set; }
        public long codigo_checklist { get; set; }
        public string descricao { get; set; }
        public int status { get; set; }
        public string descricao_status { get; set; }
        public string css_class { get; set; }
        public string data { get; set; }
        public string usuario { get; set; }
        public string pontos_possiveis { get; set; }
        public string pontos_realizados { get; set; }
        public string conforme { get; set; }
        public string nao_conforme { get; set; }
        public string nao_respondido { get; set; }
        public string nao_aplicavel { get; set; }
        public string nota { get; set; }
    }

    public class AuditoriaApontamentoArquivo
    {
        public long codigo_auditoria { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo_item_checklist { get; set; }
        public List<string> arquivo { get; set; }
    }

    public class AuditoriaExterna
    {
        public long codigo { get; set; }
        public string descricao { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string data { get; set; }
        public string data_validade { get; set; }
        public string fornecedor { get; set; }
        public string path_arquivo { get; set; }
        public float valor { get; set; }
        public string valor_texto { get; set; }
    }

    public class AuditoriaQualidadeIndex
    {
        public string unidade { get; set; }
        public string auditoria_interna { get; set; }
        public string numero_documento { get; set; }
        public string data { get; set; }
        public string auditor { get; set; }
        public int codigo_empresa { get; set; }
        public int codigo_unidade { get; set; }
        public long codigo { get; set; }
        public int grupo { get; set; }
    }

    public class Laudo
    {
        public long codigo { get; set; }
        public long codigo_pcm_programada { get; set; }
        public string descricao { get; set; }
        public int codigo_unidade { get; set; }
        public string modulo { get; set; }
        public int codigo_modulo { get; set; }
        public string unidade { get; set; }
        public string data { get; set; }
        public string data_validade { get; set; }
        public string fornecedor { get; set; }
        public string path_arquivo { get; set; }
        public float valor { get; set; }
        public string valor_texto { get; set; }
        public string data_input { get; set; }
        public List<LaudoEquipamento> equipamento { get; set; }
    }

    public class LaudoEquipamento
    {
        public string tag { get; set; }
        public string descricao { get; set; }
    }

    public class RelatorioAuditoria
    {
        public string checklist { get; set; }
        public int codigo_tipo_item_checklist { get; set; }
        public string tipo_item_checklist { get; set; }
        public string unidade_medida { get; set; }
        public string valor_minimo { get; set; }
        public string valor_maximo { get; set; }
    }

    public class RelatorioAuditoriaDataValor
    {
        public int codigo_tipo_item { get; set; }
        public string data { get; set; }
        public string resultado { get; set; }
        public string unidade_medida { get; set; }
        public string valor_minimo { get; set; }
        public string valor_maximo { get; set; }
        public string observacao { get; set; }
        public string css_class { get; set; }
    }
}
