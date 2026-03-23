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
        public DateTime? dataCompra { get; set; }
        public decimal? valorCompra { get; set; }
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
}
