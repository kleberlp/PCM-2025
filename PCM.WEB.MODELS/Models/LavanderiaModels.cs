using System.Collections.Generic;

namespace PCM.WEB.MODELS
{

    public class HistoricoApontamentoLavanderia
    {

        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public long codigo { get; set; }
        public string data { get; set; }
        public string cliente { get; set; }
        public string equipamento { get; set; }
        public string funcionario { get; set; }
        public string horaInicio { get; set; }
        public string horaTermino { get; set; }
        public string tempoGasto { get; set; }
        public string peso { get; set; }
        public string pesoRelavagem { get; set; }
        public string pesoTotal { get; set; }
    }

    public class HistoricoApontamentoLavanderiaDetalhe
    {
        public List<HistoricoApontamentoLavanderiaEnxoval> enxoval { get; set; }
        public List<HistoricoApontamentoLavanderiaEquipamento> equipamento { get; set; }
        public List<HistoricoApontamentoLavanderiaTipoFuncionario> funcionario { get; set; }
    }

    public class HistoricoApontamentoLavanderiaEnxoval
    {
        public string enxoval { get; set; }
        public string peso { get; set; }
        public string pesoRelave { get; set; }
        public string pesoTotal { get; set; }
    }

    public class HistoricoApontamentoLavanderiaEquipamento
    {
        public string equipamento { get; set; }
        public string quantidade { get; set; }
    }

    public class HistoricoApontamentoLavanderiaTipoFuncionario
    {
        public string tipoFuncionario { get; set; }
        public string quantidade { get; set; }
    }

    public class RelatorioControleLavagem
    {
        public string descricao { get; set; }
        public string dia { get; set; } = "";
        public float quiloLavagem { get; set; } = 0;
        public float quiloRelave { get; set; } = 0;
        public string percentualRelave { get; set; } = "";
        public string cssClassPercentualRelave { get; set; } = "";
        public float quiloHH { get; set; } = 0;
        public string maquinadas { get; set; } = "";
        public float total { get; set; } = 0;
    }

    public class LavanderiaEquipamento
    {
        public string tag { get; set; }
        public string descricao { get; set; } = "";
        public long codigoEquipamento { get; set; } = 0;
        public int quantidade { get; set; } = 0;
    }

    public class LavanderiaEnxoval
    {
        public string descricao { get; set; } = "";
        public long codigoEnxoval { get; set; } = 0;
        public int quantidade { get; set; } = 0;
        public int quantidadeRelave { get; set; } = 0;
    }

    public class LavanderiaEnxovalInfo
    {
        public string peso { get; set; } = "0";
        public bool impresso { get; set; } = false;
    }

    public class LavanderiaFuncionario
    {
        public string descricao { get; set; } = "";
        public long codigo { get; set; } = 0;
        public int quantidade { get; set; } = 0;
    }

}
