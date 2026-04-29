using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCM.WEB.MODELS
{
    public class AssetModel
    {
        public long codigo { get; set; }
        public short codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public string assetCode { get; set; }
        public string descricao { get; set; }
        public string numeroSerie { get; set; }
        public string tag { get; set; }
        public string contaContabil { get; set; }
        public DateTime? dataCompra { get; set; }
        public decimal? valorCompra { get; set; }
        public int? tempoDepreciacaoMes { get; set; }
        public string notaFiscal { get; set; }
        public decimal? valorAtual { get; set; }
        public int? codigoStatus { get; set; }
        public int? codigoSetor { get; set; }
        public int? codigoApartamento { get; set; }
        public int? codigoUsuarioResponsavel { get; set; }
        public string responsavel { get; set; }
        public long? codigoEquipamento { get; set; }
        public bool ativo { get; set; }
        public int? codigoUsuarioInput { get; set; }
        public int? codigoUsuarioUpdate { get; set; }
    }

    public class AssetInventory
    {
        public string asset { get; set; } = "";
        public string descricao { get; set; } = "";
        public bool ativoCadastrado { get; set; } = false;
        public string cssClass { get; set; } = "";
    }

    public class AssetInventoryDetails
    {
        public string assetCode { get; set; } = "";
        public string descricao { get; set; } = "";
        public string setor { get; set; } = "";
        public string apartamento { get; set; } = "";
        public string setorAnterior { get; set; } = "";
        public string apartamentoAnterior { get; set; } = "";
        public string usuario { get; set; } = "";
        public string data { get; set; } = "";
        public string ativoCadastrado { get; set; } = "";
    }

    public class AssetInventoryInfo
    {
        public string descricao { get; set; } = "";
        public long codigoInventario { get; set; } = 0;
    }

    public class AssetTipoMovimentacaoConfig
    {
        public bool success { get; set; } = true;
        public string message { get; set; } = "";
        public bool documento { get; set; } = false;
        public bool valor { get; set; } = false;
        public bool setor { get; set; } = false;
        public bool apartamento { get; set; } = false;
        public bool fornecedor { get; set; } = false;

    }
}
