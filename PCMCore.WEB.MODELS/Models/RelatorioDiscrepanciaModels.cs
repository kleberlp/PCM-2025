namespace PCM.WEB.MODELS
{
    // Linha do Relatório de Discrepâncias (RE18) — Governança
    public class RelatorioDiscrepancia
    {
        public long codigoApartamento { get; set; }
        public int codigoUnidade { get; set; }
        public string local { get; set; }            // Local / U.H.
        public string data { get; set; }             // dd/MM/yyyy
        public string planejadoPara { get; set; }    // camareira planejada
        public string executadoPor { get; set; }     // camareira que executou
        public string vistoriadoPor { get; set; }    // supervisora
        public string horaTermino { get; set; }
        public string statusUh { get; set; }           // status UH da discrepância
        public string statusGov { get; set; }         // LIMPO / NÃO PERTURBE / N.Q.A / etc.
        public string divergencia { get; set; }       // SIM / NÃO
        public string ocupacao { get; set; }          // AD / Cr1 / Cr2  (ex.: "2 / 1 / 0")
        public string bagagem { get; set; }           // M / P / G
        public string observacao { get; set; }
        public bool semVistoria { get; set; }
        public bool semExecucao { get; set; }
    }

    // KPIs do Relatório de Discrepâncias
    public class RelatorioDiscrepanciaKpi
    {
        // Contexto do período
        public int totalPlanejado { get; set; }
        public int totalArrumado { get; set; }
        public int totalPermanencia { get; set; }
        public int totalSaida { get; set; }
        // Discrepâncias
        public int divergencias { get; set; }
        public int planejadoSemExecucao { get; set; }
        public int executadoSemVistoria { get; set; }
        public int quartosNqa { get; set; }
        public int quartosNaoPerturbe { get; set; }
    }
}
