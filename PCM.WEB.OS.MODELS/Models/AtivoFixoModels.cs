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

}