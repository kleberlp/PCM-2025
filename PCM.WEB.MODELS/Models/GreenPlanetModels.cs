using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCM.WEB.MODELS
{

    public class LancamentoMedicao
    {
        public int codigo_item_medicao { get; set; }
        public string item_medicao { get; set; }
        public long acumulado_mes { get; set; }
        public long acumulado_ano { get; set; }
        public long valor { get; set; }
        public int quantidade_hospede { get; set; }
        public int ocupacao_quartos { get; set; }
        public int numero_digitos { get; set; }
    }

}
