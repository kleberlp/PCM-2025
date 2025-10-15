using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCM.WEB.MODELS
{

    public class ControleGasto
    {
        public int ano { get; set; }
        public int mes { get; set; }
        public float previsao_gasto_janeiro { get; set; }
        public float gasto_janeiro { get; set; }
        public float saldo_janeiro { get; set; }
        public float previsao_gasto_fevereiro { get; set; }
        public float gasto_fevereiro { get; set; }
        public float saldo_fevereiro { get; set; }
        public float previsao_gasto_marco { get; set; }
        public float gasto_marco { get; set; }
        public float saldo_marco { get; set; }
        public float previsao_gasto_abril { get; set; }
        public float gasto_abril { get; set; }
        public float saldo_abril { get; set; }
        public float previsao_gasto_maio { get; set; }
        public float gasto_maio { get; set; }
        public float saldo_maio { get; set; }
        public float previsao_gasto_junho { get; set; }
        public float gasto_junho { get; set; }
        public float saldo_junho { get; set; }
        public float previsao_gasto_julho { get; set; }
        public float gasto_julho { get; set; }
        public float saldo_julho { get; set; }
        public float previsao_gasto_agosto { get; set; }
        public float gasto_agosto { get; set; }
        public float saldo_agosto { get; set; }
        public float previsao_gasto_setembro { get; set; }
        public float gasto_setembro { get; set; }
        public float saldo_setembro { get; set; }
        public float previsao_gasto_outubro { get; set; }
        public float gasto_outubro { get; set; }
        public float saldo_outubro { get; set; }
        public float previsao_gasto_novembro { get; set; }
        public float gasto_novembro { get; set; }
        public float saldo_novembro { get; set; }
        public float previsao_gasto_dezembro { get; set; }
        public float gasto_dezembro { get; set; }
        public float saldo_dezembro { get; set; }
    }

    public class Contrato
    {
        public int codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string descricao { get; set; }
        public string comentario { get; set; }  
        public string data_inicio { get; set; }
        public string data_termino { get; set; }
        public bool ativo { get; set; }
        public string arquivo { get; set; }
        public string path_arquivo { get; set; }
        public bool envia_email { get; set; }
        public int dias_alerta { get; set; }
        public string email { get; set; }
    }

    public class Despesa
    {
        public int codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string numero_documento { get; set; }
        public int codigo_fornecedor { get; set; }
        public string fornecedor { get; set; }
        public int codigo_tipo_despesa { get; set; }
        public string tipo_despesa { get; set; }
        public string descricao { get; set; }
        public float valor { get; set; }
        public string data_competencia { get; set; }
        public string data_pagamento { get; set; }
        public string arquivo { get; set; }
        public string path_arquivo { get; set; }
    }

}
