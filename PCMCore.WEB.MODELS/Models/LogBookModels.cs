using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCM.WEB.MODELS
{
    public class LogBook
    {
        public int codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string data_input { get; set; }
        public string informacao { get; set; }
        public string data_visualizacao { get; set; }
        public string usuario { get; set; }
        public string html { get; set; }
    }

}
