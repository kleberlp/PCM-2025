namespace PCM.WEB.OS.MODELS
{
    public class AssetInventoryItem
    {
        public string? asset { get; set; }
        public string? descricao { get; set; }
        public string? cssClass { get; set; }
        public bool? statusOk { get; set; }
        public string? observacao { get; set; }
    }

    public class AssetInventoryViewModel
    {
        public AssetInventory? inventory { get; set; }
        public List<AssetInventoryItem> items { get; set; } = new();
    }

    public class AssetInventory
    {
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
    }

    public class AssetInventoryCheck
    {
        public bool alreadyCounted { get; set; } = false;
        public bool sameLocation { get; set; } = false;
        public string localAtual { get; set; } = "";
    }

    public class AssetLastEvaluation
    {
        public bool possuiAvaliacao { get; set; } = false;
        public bool statusOk { get; set; } = true;
        public string observacao { get; set; } = "";
        public bool possuiFoto { get; set; } = false;
    }

}