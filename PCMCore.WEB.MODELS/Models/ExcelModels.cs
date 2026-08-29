using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PCM.WEB.MODELS
{
    public class OrdemServicoExcel
    {
        [DisplayName("Unidade")]
        public string unidade { get; set; }
        [DisplayName("Nº Ordem de Serviço")]
        public string numero_documento { get; set; }
        [DisplayName("Nº Ordem de Serviço Cliente")]
        public string ordem_servico_cliente { get; set; }
        [DisplayName("Data")]
        public string data { get; set; }
        [DisplayName("Categoria")]
        public string categoria { get; set; }
        [DisplayName("Descrição")]
        public string descricao { get; set; }
        [DisplayName("Setor")]
        public string setor { get; set; }
        [DisplayName("Apartamento")]
        public string apartamento { get; set; }
        [DisplayName("Equipamento")]
        public string equipamento { get; set; }
        [DisplayName("Solicitante")]
        public string solicitante { get; set; }
        [DisplayName("Departamento")]
        public string departamento { get; set; }
        [DisplayName("Executor")]
        public string executor { get; set; }
        [DisplayName("Prioridade")]
        public string prioridade { get; set; }
        [DisplayName("Tipo de Serviço")]
        public string tipo_servico { get; set; }
        [DisplayName("Tipo de Ordem de Serviço")]
        public string tipo_ordem_servico { get; set; }
        [DisplayName("Status")]
        public string status { get; set; }
        [DisplayName("Data Execução")]
        public string data_execucao { get; set; }
        [DisplayName("Horas")]
        public string horas { get; set; }
        [DisplayName("Justificativa")]
        public string justificativa_apontamento { get; set; }
        [DisplayName("Ação")]
        public string acao { get; set; }
    }

    public class PlanoAcaoQA
    {

        [DisplayName("Categoria")]
        public string categoria { get; set; }

        [DisplayName("Unidade")] 
        public string unidade { get; set; }
        [DisplayName("Setor")]
        public string setor { get; set; }
        [DisplayName("Equipamento")]
        public string equipamento { get; set; }
        [DisplayName("Departamento")]
        public string departamento { get; set; }
        [DisplayName("Local")]
        public string local { get; set; }
        [DisplayName("Solicitante")]
        public string solicitante { get; set; }
        [DisplayName("Executor")]
        public string executor { get; set; }
        [DisplayName("Data Execução")]
        public string data_execucao { get; set; }
        [DisplayName("Apartamento")]
        public string apartamento { get; set; }
        [DisplayName("Nº Documento")]
        public string numero_documento { get; set; }
        [DisplayName("Data")]
        public string data { get; set; }
        [DisplayName("Data Necessidade")]
        public string data_necessidade { get; set; }
        [DisplayName("Dias")]
        public string dias { get; set; }
        [DisplayName("Descrição")]
        public string descricao { get; set; }
        [DisplayName("Prioridade")]
        public string prioridade { get; set; }
        [DisplayName("Tipo de Serviço")]
        public string tipo_servico { get; set; }
        [DisplayName("Tipo Ordem de Serviço")]
        public string tipo_ordem_servico { get; set; }
        [DisplayName("Status")]
        public string status_descricao { get; set; }
        [DisplayName("Justificativa")]
        public string justificativa_apontamento { get; set; }
        [DisplayName("Auditoria")]
        public string auditoria { get; set; }
    }

    public class InterfaceExcelColumn
    {
        public string ColunaExcel { get; set; }
        public string DataMember { get; set; }
        public bool Obrigatorio { get; set; }
        public bool Visivel { get; set; }
        public int Linha { get; set; }
        public int Coluna { get; set; }
        public string Largura { get; set; }
        public string TipoValidacao { get; set; }
        public string FonteLista { get; set; }
    }
}
