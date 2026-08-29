using System;
using System.Collections.Generic;

namespace PCM.WEB.MODELS
{
    public class UHChecklistStatus
    {
        public long quantidade_atrasado { get; set; }
        public long quantidade_pendente { get; set; }
        public long quantidade_nova_vistoria { get; set; }
        public long quantidade_manutencao { get; set; }
        public long quantidade_realizada { get; set; }
    }
    
    public class UHChecklist
    {
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public List<UHBloco> bloco { get; set; }
    }
    
    public class UHBloco
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
        public List<UHAndar> andar { get; set; }            
    }

    public class UHAndar
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
        public List<UH> uh { get; set; }        
    }

    public class UH
    {
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string bloco { get; set; }
        public string andar { get; set; }
        public string apartamento { get; set; }        
        public int codigo_apartamento { get; set; }
        public string tipo_apartamento { get; set; }
        public int codigo_tipo_apartamento { get; set; }
        public int status { get; set; }
        public string descricao_status { get; set; }
        public string data_proxima { get; set; }
        public string css_class { get; set; }
        public long codigo_vistoria { get; set; }
        public string room_status { get; set; }
        public string front_office_status { get; set; }
    }

    public class UHApontamento
    {
        public string unidade { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo_apartamento { get; set; }
        public string apartamento { get; set; }
        public int codigo_funcionario_responsavel_vistoria { get; set; }
        public string funcionario_responsavel_vistoria { get; set; }
        public int codigo_funcionario_responsavel_unidade { get; set; }
        public string funcionario_responsavel_unidade { get; set; }
        public string data { get; set; }
        public string data_inicio { get; set; }
        public string data_termino { get; set; }
        public bool aponta_horas { get; set; }
        public string hora_inicio { get; set; }
        public string hora_termino { get; set; }
        public float valor { get; set; }
        public bool nova_vistoria { get; set; }
        public long codigo_apontamento { get; set; }
    }
    
    public class UHApontamentoChecklist
    {
        public string grupo { get; set; }
        public int codigo { get; set; }
        public string checklist { get; set; }
        public string descricao { get; set; }
        public string opcao { get; set; }        
        public string observacao { get; set; }
        public bool nova_vistoria { get; set; }
    }

    public class UHChecklistHistorico
    {
        public string unidade { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo_apartamento { get; set; }
        public string apartamento { get; set; }
        public DateTime data_inicio { get; set; }
        public DateTime data_termino { get; set; }
        public string responsavel_vistoria { get; set; }
        public string tempo_gasto { get; set; }
        public long codigo { get; set; }
        public int codigo_apontamento { get; set; }
    }

    public class UHAtividadeBloco
    {
        public string codigo { get; set; }
        public string descricao { get; set; }
        public List<UHAtividadeAndar> andar { get; set; }
    }

    public class UHAtividadeAndar
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
        public List<UHAtividade> uh { get; set; }
    }

    public class UHAtividade
    {
        public string data_inicio { get; set; }
        public string data_termino { get; set; }
        public string apartamento { get; set; }
        public int codigo_apartamento { get; set; }
        public int codigo_unidade { get; set; }
        public string tipo_apartamento { get; set; }
        public int codigo_tipo_apartamento { get; set; }
        public int status { get; set; }
        public string descricao_status { get; set; }
        public string room_status { get; set; }
        public string color { get; set; }
        public long codigo_atividade { get; set; }
    }

    public class UHAtividadeApontamento
    {
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public long codigo_atividade { get; set; }
        public string atividade { get; set; }
        public string descricao { get; set; }
        public int codigo_tipo_servico { get; set; }
        public string categoria { get; set; }
        public string itens_gerais { get; set; }
        public string apartamento { get; set; }
        public int codigo_apartamento { get; set; }
        public string data_inicio { get; set; }
        public string hora_inicio { get; set; }
        public string data_termino { get; set; }
        public string hora_termino { get; set; }
        public string observacao { get; set; }
        public string codigo_funcionario { get; set; }
        public int codigo_fornecedor { get; set; }
        public string status_uh { get; set; }
    }

    public class UHDedetizacaoStatus
    {
        public long quantidade_atrasado { get; set; }
        public long quantidade_pendente { get; set; }
        public long quantidade_realizada { get; set; }
    }

    public class UHDedetizacao
    {
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public List<UHBlocoDedetizacao> bloco { get; set; }
    }

    public class UHBlocoDedetizacao
    {
        public string codigo { get; set; }
        public string descricao { get; set; }
        public List<UHAndarDedetizacao> andar { get; set; }
    }

    public class UHAndarDedetizacao
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
        public List<UHDedetizacaoApartamento> uh { get; set; }
    }

    public class UHDedetizacaoApartamento
    {
        public int codigo_uh_atividade { get; set; }
        public int codigo_apartamento { get; set; }
        public string apartamento { get; set; }
        public string data { get; set; }
        public string color { get; set; }
        public string icon { get; set; }
        public long codigo { get; set; }
        public int status { get; set; }
        public string room_status { get; set; }
        public string front_office_status { get; set; }
    }

    public class UHDedetizacaoList
    {
        public int codigo_unidade { get; set; }
        public int codigo_apartamento { get; set; }
        public int codigo_uh_atividade { get; set; }
        public string unidade { get; set; }
        public string bloco { get; set; }
        public string andar { get; set; }
        public string apartamento { get; set; }
        public string data { get; set; }
        public string data_proxima { get; set; }
        public string css_class { get; set; }
        public string icon { get; set; }
        public long codigo { get; set; }
        public int status { get; set; }
        public string room_status { get; set; }
        public string front_office_status { get; set; }
    }

    public class UHDedetizacaoApontamento
    {
        public int codigo_uh_atividade { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo_tipo_servico { get; set; }
        public int codigo_apartamento { get; set; }
        public long codigo { get; set; }
        public string icon { get; set; }
        public string uh_local { get; set; }
        public string unidade { get; set; }
        public string descricao { get; set; }
        public int codigo_funcionario { get; set; }
        public int codigo_fornecedor { get; set; }
        public string data { get; set; }
        public string observacao { get; set; }
    }

    public class UHAtividadeInfo
    {
        public int codigo_uh_atividade { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo_tipo_servico { get; set; }
        public string icon { get; set; }
        public string unidade { get; set; }
        public string descricao { get; set; }
    }

    public class UHDedetizacaoHistorico
    {
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public int codigoUHAtividade { get; set; }
        public long codigo { get; set; }
        public string unidade { get; set; }
        public string colaborador { get; set; }
        public string apartamento { get; set; }
        public string data { get; set; }
        public string observacao { get; set; }
    }

    public class UHMapaManutencaoHistorico
    {
        public int codigoEmpresa { get; set; }
        public int codigoUnidade { get; set; }
        public int codigoApartamento { get; set; }
        public long codigo { get; set; }
        public string unidade { get; set; }
        public string colaborador { get; set; }
        public string apartamento { get; set; }
        public string descricao { get; set; }
        public string data { get; set; }
        public string dataPrevisaoTermino { get; set; }
        public string observacao { get; set; }
    }

}
