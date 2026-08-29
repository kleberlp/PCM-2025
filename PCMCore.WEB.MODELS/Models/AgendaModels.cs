using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCM.WEB.MODELS
{
    public class AreaComum
    {
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public int codigo { get; set; }
        public string descricao { get; set; }
        public int quantidade_eventos_concorrentes { get; set; }
        public bool ativo { get; set; }
    }

}
