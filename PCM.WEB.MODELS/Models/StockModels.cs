using System.Collections.Generic;
using System.Web;

namespace SYSPACK.WEB.MODELS
{
    public class stockInfo
    {
        public string product { get; set; }
        public string description { get; set; }
        public string batch { get; set; }
        public string quantity { get; set; }
        public string spare_point { get; set; }
        public string unitary_value { get; set; }
        public string total_value { get; set; }
        public string status { get; set; }
    }

    public class stock_movement
    {
        public string document { get; set; }
        public string date { get; set; }
        public string quantity { get; set; }
        public string type { get; set; }
        public string username { get; set; }
    }

    public class stock_movement_report
    {
        public string product { get; set; }
        public string description { get; set; }
        public string batch { get; set; }
        public string quantity { get; set; }
        public string unitary_value { get; set; }
        public string total_value { get; set; }
        public string document { get; set; }
        public string date { get; set; }
        public string type { get; set; }
        public string username { get; set; }
        public string file { get; set; }
        public string css_class { get; set; }
    }

    public class stock_picking
    {
        public long product_id { get; set; }
        public string product { get; set; }
        public int uom_id { get; set; }
        public string description { get; set; }
        public string batch { get; set; }
        public string bin_position { get; set; }
        public string quantity { get; set; }
        public int deleted { get; set; }
    }

    public class stock_replenishment
    {
        public long product_id { get; set; }
        public string product { get; set; }
        public int uom_id { get; set; }
        public string uom { get; set; }
        public string description { get; set; }
        public string batch { get; set; }
        public string expiration_date { get; set; }
        public string quantity { get; set; }        
        public string unitary_value { get; set; }
        public string total_value { get; set; }
        public string arquivo { get; set; }
        public int deleted { get; set; }
    }

    public class product_stock_info
    {
        public string description { get; set; }
        public string uom { get; set; }
        public bool batch_control { get; set; }
        public string economic_batch { get; set; }
        public bool expiration_date_control { get; set; }
        public bool quality_control { get; set; }
        public bool receive_more { get; set; }
        public int quantity { get; set; }
        public int quantity_pending { get; set; }
        public float percentual { get; set; }
    }

    public class product_info
    {
        public string product { get; set; }
        public string description { get; set; }
        public string uom { get; set; }
        public bool batch_control { get; set; }
    }

    public class uom_info
    {
        public int decimal_places { get; set; }
    }

    public class estoqueEntrada
    {
        public long codigo { get; set; }
        public long codigoProduto { get; set; }
        public string numeroDocumento { get; set; }
        public string ordemCompra { get; set; }
        public string dataDocumento { get; set; }
        public string fornecedor { get; set; }
        public string statusDescricao { get; set; }
        public int status { get; set; }
        public string cssClass { get; set; }
        public string produto { get; set; }
        public string descricaoProduto { get; set; }
        public string quantidade { get; set; }
        public string quantidadePendente { get; set; }
        public string quantidadeRecebida { get; set; }
        public string usuario { get; set; }
    }

    public class estoqueOrdemCompra
    {
        public long codigoOrdemCompra { get; set; }
        public int codigoUnidade { get; set; }
        public string ordemCompra { get; set; }
        public string dataCriacao { get; set; }
        public string fornecedor { get; set; }
        public int codigoFornecedor { get; set; }
        public int status { get; set; }
        public string statusDescricao { get; set; }
        public string quantidade { get; set; }
        public string quantidadePendente { get; set; }
        public string quantidadeRecebida { get; set; }
        public string usuario { get; set; }
        public string cssClass { get; set; }
        public string path { get; set; }
    }

    public class estoqueOrdemCompraProduto
    {
        public long codigoProduto { get; set; }
        public string produto { get; set; }
        public int codigoUnidadeMedida { get; set; }
        public string uom { get; set; }
        public string descricao { get; set; }
        public string quantidade { get; set; }
        public string quantidadeRecebida { get; set; }
        public string quantidadePendente { get; set; }
    }

    public class estoqueRequisicaoCompra
    {
        public long codigoRequisicaoCompra { get; set; }
        public int codigoUnidade { get; set; }
        public string numeroRequisicao { get; set; }
        public string ordemCompra { get; set; }
        public string dataCriacao { get; set; }
        public string fornecedor { get; set; }
        public int codigoFornecedor { get; set; }
        public string statusDescricao { get; set; }
        public int status { get; set; }
        public string usuario { get; set; }
        public string cssClass { get; set; }
    }

    public class estoqueRequisicaoCompraProduto
    {
        public long codigoProduto { get; set; }
        public string produto { get; set; }
        public int codigoUnidadeMedida { get; set; }
        public string descricao { get; set; }
        public string uom { get; set; }
        public string quantidade { get; set; }
        public string estoque { get; set; }
        public string transito { get; set; }
        public string consumo { get; set; }
        public string tempoReposicao { get; set; }
        public string loteEconomico { get; set; }
        public string pontoReposicao { get; set; }
    }

    public class estoqueListagemInventario
    {
        public long codigoProduto { get; set; }
        public string produto { get; set; }
        public string descricaoProduto { get; set; }
        public string lote { get; set; }
        public int quantidadeEntrada { get; set; }
        public int quantidadeSaida { get; set; }
        public int saldo { get; set; }
        public string unidadeMedida { get; set; }
        public string localizacao { get; set; }
    }

}
