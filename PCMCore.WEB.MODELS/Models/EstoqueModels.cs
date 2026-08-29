using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCM.WEB.MODELS
{
    public class EntradaEstoque
    {
        public long codigo_produto { get; set; }
        public string produto { get; set; }
        public string descricao_produto { get; set; }
        public string lote { get; set; }
        public string data_validade { get; set; }
        public int quantidade { get; set; } 
        public string unidade_medida { get; set; }
        public int excluido { get; set; }
    }

    public class SaidaEstoque
    {
        public long codigo_produto { get; set; }
        public string produto { get; set; }
        public string descricao_produto { get; set; }
        public string lote { get; set; }
        public int quantidade { get; set; }
        public string unidade_medida { get; set; }
        public int excluido { get; set; }
    }

    public class EstoqueListagem
    {
        public long codigo_produto { get; set; }
        public string produto { get; set; }
        public string descricao_produto { get; set; }
        public string lote { get; set; }
        public int quantidade_entrada { get; set; }
        public int quantidade_saida { get; set; }
        public int saldo { get; set; }
        public string unidade_medida { get; set; }
        public string localizacao { get; set; }
    }

    public class EstoqueInventario
    {
        public bool success { get; set; }
        public string message { get; set; }
        public List<EstoqueInventarioDetalhe> result { get; set; }
        public List<EstoqueInventarioDetalheError> resultError { get; set; }
    }

    public class EstoqueInventarioDetalhe
    {
        public string produto { get; set; }
        public string descricao { get; set; }
        public string lote { get; set; }
        public string quantidade { get; set; }
        public string quantidadeInventario { get; set; }
        public string divergencia { get; set; }
    }

    public class EstoqueInventarioDetalheError
    {
        public string produto { get; set; }
        public string erro { get; set; }
    }

}
