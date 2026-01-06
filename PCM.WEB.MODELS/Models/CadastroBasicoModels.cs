using System.Collections.Generic;

namespace PCM.WEB.MODELS
{
    public class ListCombo
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
    }

    public class ListComboString
    {
        public string codigo { get; set; }
        public string descricao { get; set; }
    }

    public class CEP
    {
        public string tipo_logradouro { get; set; }
        public string logradouro { get; set; }
        public string bairro { get; set; }
        public string uf { get; set; }        
        public string municipio { get; set; }
    }

    public class Apartamento
    {
        public int codigo { get; set; }
        public string apartamento { get; set; }
        public string bloco { get; set; }
        public int andar { get; set; }
        public string descritivo { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo_responsavel_apartamento { get; set; }
        public string responsavel_apartamento { get; set; }
        public int codigo_tipo_unidade { get; set; }
        public string unidade { get; set; }
        public int codigo_setor { get; set; }
        public string setor { get; set; }
        public int codigo_tipo_apartamento { get; set; }
        public string tipo_apartamento { get; set; }
        public int codigo_tipo_cama { get; set; }
        public string tipo_cama { get; set; }
        public int quantidade_cama { get; set; }
        public string room_status { get; set; }
        public string front_office_status { get; set; }        
        public float metragem { get; set; }
        public float carga_termica { get; set; }
        public string descricao_atividade { get; set; }
        public int numero_pessoas_fixas { get; set; }
        public int numero_pessoas_volantes { get; set; }
        public bool ativo { get; set; }
        public string ativoTexto { get; set; }
        public string data_ultima_manutencao { get; set; }
    }

    public class ArCondicionado
    {
        public long codigo { get; set; }
        public string tag { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public int codigo_tipo_ar_condicionado { get; set; }
        public int codigo_departamento { get; set; }
        public string tipo_ar_condicionado { get; set; }
        public string descricao { get; set; }
        public int codigo_setor { get; set; }
        public string setor { get; set; }
        public int codigo_apartamento { get; set; }
        public string apartamento { get; set; }
        public string fabricante { get; set; }
        public string endereco_fabricante { get; set; }
        public string contato_fabricante { get; set; }
        public string modelo { get; set; }
        public string numero_fabricacao { get; set; }
        public int ano_fabricacao { get; set; }
        public string caracteristicas { get; set; }
        public float potencia { get; set; }
        public int codigo_potencia_ar_condicionado { get; set; }
        public string potencia_ar_condicionado { get; set; }
        public string data_proxima_manutencao { get; set; }
        public bool ativo { get; set; }
        public string texto_ativo { get; set; }
        public string qrcode { get; set; }
        public int codigo_empresa_pmoc { get; set; }
        public int codigo_unidade_pmoc { get; set; }
        public string andar { get; set; }
        public long codigo_ar_condicionado_pmoc { get; set; }
    }

    public class Atividade
    {
        public long codigo { get; set; }
        public int codigo_categoria { get; set; }
        public string categoria { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public long codigo_equipamento { get; set; }
        public string equipamento { get; set; }
        public string titulo { get; set; }
        public string descricao { get; set; }
        public int codigo_tipo_servico { get; set; }
        public string tipo_servico { get; set; }
        public string data_previsao_termino { get; set; }
        public string data_inicio { get; set; }
        public string data_termino { get; set; }
        public string bloco { get; set; }
        public string andar { get; set; }
        public bool ativo { get; set; }
    }

    public class AuditoriaQualidade
    {
        public long codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string descricao { get; set; }
        public int codigo_checklist { get; set; }
        public string checklist { get; set; }
        public int codigo_periodicidade { get; set; }
        public string periodicidade { get; set; }
        public int intervalo { get; set; }
        public bool ativo { get; set; }
        public int codigo_modulo { get; set; }
        public string modulo { get; set; }
    }

    public class AuditoriaCorporativo
    {
        public long codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string descricao { get; set; }
        public int codigo_checklist { get; set; }
        public string checklist { get; set; }
        public bool gerar_plano_acao { get; set; }
        public int codigo_periodicidade { get; set; }
        public string periodicidade { get; set; }
        public int intervalo { get; set; }
        public bool ativo { get; set; }
        public int codigo_modulo { get; set; }
        public string modulo { get; set; }
    }

    public class Categoria
    {
        public int codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string descricao { get; set; }
        public bool ativo { get; set; }
    }

    public class ChecklistHeader
    {
        public int codigoUnidade { get; set; }
        public string unidade { get; set; }
        public int codigo { get; set; }
        public int codigoTipoChecklist { get; set; }
        public string tipoChecklist { get; set; }
        public string descricao { get; set; }
    }

    public class ChecklistItem
    {
        public long codigo_checklist { get; set; }
        public int codigo { get; set; }
        public string checklist { get; set; }
        public string tipo_item_checklist { get; set; }
        public int codigo_tipo_item_checklist { get; set; }
        public string grupo { get; set; }
        public int peso_grupo { get; set; }
        public string subgrupo { get; set; }
        public int peso_subgrupo { get; set; }
        public string descricao { get; set; }
        public int numero_digitos { get; set; }
        public bool allow_picture { get; set; }
        public float valor_minimo { get; set; }
        public float valor_maximo { get; set; }
        public string unidade_medida { get; set; }
        public int tempo_estimado { get; set; }
        public bool auditado { get; set; }
        public bool ordem_servico { get; set; } = false;
        public int codigo_periodicidade { get; set; }
        public string periodicidade { get; set; }
        public int intervalo { get; set; }
        public int excluido { get; set; }
        public int peso { get; set; }
        public int codigo_departamento { get; set; }
    }

    public class Cliente
    {
        public int codigo { get; set; }
        public int codigoUnidade { get; set; }
        public string unidade { get; set; }
        public string nomeFantasia { get; set; }
        public string razaoSocial { get; set; }
        public string cnpj { get; set; }
        public string inscricaoEstadual { get; set; }
        public string inscricaoMunicipal { get; set; }
        public string cep { get; set; }
        public string uf { get; set; }
        public string municipio { get; set; }
        public string logradouro { get; set; }
        public string numero { get; set; }
        public string bairro { get; set; }
        public string complemento { get; set; }
        public string telefone { get; set; }
        public string email { get; set; }
        public bool ativo { get; set; }
    }

    public class ClienteAcordoComercial
    {
        public int codigoEnxoval { get; set; }
        public string enxoval { get; set; }
        public int quantidade { get; set; }
        public string valorUnitario { get; set; }
    }

    public class Departamento
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
        public bool ativo { get; set; }
        public List<DepartamentoGestor> gestor { get; set; }
    }

    public class DepartamentoGestor
    {
        public int codigo_departamento { get; set; }
        public int codigo_unidade { get; set; }
        public int codigo_usuario { get; set; }
        public string departamento { get; set; }
        public string unidade { get; set; }
        public string nome { get; set; }
        public int codigo { get; set; }
    }

    public class Dedetizacao
    {
        public int codigo { get; set; }
        public int codigo_unidade { get; set; }
        public int periodicidade { get; set; }
        public int alerta { get; set; }
        public int codigo_tipo_servico { get; set; }
        public string data_inicio { get; set; }
    }

    public class DedetizacaoApartamento
    {
        public long codigo_apartamento { get; set; }
        public string setor { get; set; }
        public string apartamento { get; set; }
        public string tipo_apartamento { get; set; }
        public string bloco { get; set; }
        public string andar { get; set; }
        public bool selected { get; set; }
    }

    public class Equipamento
    {
        public long codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string tag { get; set; }
        public string unidade { get; set; }
        public int codigo_familia_equipamento { get; set; }
        public string familia { get; set; }
        public string descricao { get; set; }
        public int codigo_departamento { get; set; }
        public int codigo_setor { get; set; }
        public string setor { get; set; }
        public int codigo_apartamento { get; set; }
        public string apartamento { get; set; }
        public string fabricante { get; set; }
        public string endereco_fabricante { get; set; }
        public string contato_fabricante { get; set; }
        public string modelo { get; set; }
        public string numero_fabricacao { get; set; }
        public int ano_fabricacao { get; set; }
        public string caracteristicas { get; set; }
        public string programada { get; set; }
        public string descricao_operacao { get; set; }
        public string instrucao_utilizacao { get; set; }
        public string procedimento_emergencia { get; set; }
        public string treinamento_operador { get; set; }
        public string condicao_seguranca { get; set; }
        public string indicacao_conclusiva { get; set; }
        public string path_arquivo { get; set; }
        public string arquivo { get; set; }
        public bool ativo { get; set; }
    }

    public class Enxoval
    {
        public int codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string descricao { get; set; }
        public float peso { get; set; }
        public bool ativo { get; set; }
        public string texto_ativo { get; set; }
    }

    public class FamiliaEquipamento
    {
        public int codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string descricao { get; set; }
        public bool ativo { get; set; }
    }

    public class Fornecedor
    {
        public int codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string nome_fantasia { get; set; }
        public string razao_social { get; set; }
        public string cnpj { get; set; }
        public string inscricao_estadual { get; set; }
        public string inscricao_municipal { get; set; }
        public string cep { get; set; }
        public string uf { get; set; }
        public string municipio { get; set; }
        public string logradouro { get; set; }
        public string numero { get; set; }
        public string bairro { get; set; }
        public string complemento { get; set; }
        public string telefone { get; set; }
        public string email { get; set; }
        public int codigo_categoria { get; set; }
        public string categoria { get; set; }
        public bool ativo { get; set; }
    }

    public class Funcao
    {
        public int codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string descricao { get; set; }
        public bool ativo { get; set; }
    }

    public class Funcionario
    {
        public int codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string nome { get; set; }
        public string cpf { get; set; }
        public int codigo_funcao { get; set; }
        public int codigo_usuario { get; set; }
        public string funcao { get; set; }
        public string telefone { get; set; }
        public string tipo_funcionario { get; set; }
        public int codigo_tipo_funcionario { get; set; }
        public string modulo { get; set; }
        public int codigo_modulo { get; set; }
        public float valor_hora { get; set; }
        public bool ativo { get; set; }
        public string texto_ativo { get; set; }
        public bool contabiliza_hora { get; set; }
    }

    public class GrupoChecklist
    {
        public int codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string codigo_grupo { get; set; }
        public string descricao { get; set; }
        public bool ativo { get; set; }
    }

    public class GrupoItemMedicao
    {
        public int codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string descricao { get; set; }
        public bool ativo { get; set; }
    }

    public class GrupoProduto
    {
        public int codigo { get; set; }
        public int codigoUnidade { get; set; }
        public string unidade { get; set; }
        public string codigoGrupo { get; set; }
        public string descricao { get; set; }
        public bool ativo { get; set; }
    }

    public class ItemMedicao
    {
        public int codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public int codigo_grupo_item_medicao { get; set; }
        public string grupo_item_medicao { get; set; }
        public string descricao { get; set; }
        public float meta_consumo { get; set; }
        public int codigo_forma_leitura { get; set; }
        public string forma_leitura { get; set; }
        public int numero_digitos { get; set; }
        public int numero_casas_decimais { get; set; }
        public string unidade_medida { get; set; }
        public bool ativo { get; set; }
    }

    public class ItensGerais
    {
        public int codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string descricao { get; set; }
        public bool ativo { get; set; }
    }

    public class JustificativaApontamento
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
        public bool ativo { get; set; }
    }

    public class JustificativaFalta
    {
        public int codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string descricao { get; set; }
        public bool justificada { get; set; }
        public bool ativo { get; set; }
    }

    public class JustificativaCancelamentoOrdemServico
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
        public bool justificada { get; set; }
        public bool ativo { get; set; }
    }

    public class Prioridade
    {
        public int codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string descricao { get; set; }
        public bool envia_email { get; set; }
        public string email { get; set; }
        public bool ativo { get; set; }
    }

    public class Produto
    {
        public int codigo { get; set; }
        public int codigoUnidade { get; set; }
        public string unidade { get; set; }
        public int codigoGrupoProduto { get; set; }
        public string grupoProduto { get; set; }
        public string codigoProduto { get; set; }
        public string descricao { get; set; }
        public string unidadeMedida { get; set; }
        public int pontoReposicao { get; set; }
        public float quantidade { get; set; }
        public bool controlaLote { get; set; }
        public bool controlaDataValidade { get; set; }
        public bool ativo { get; set; }
        public float quantidadePendente { get; set; }
        public float saldo { get; set; }
    }

    public class Programada
    {
        public long codigo { get; set; }
        public int codigo_categoria { get; set; }
        public string categoria { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public int codigo_setor { get; set; }
        public string setor { get; set; }
        public long codigo_equipamento { get; set; }
        public string equipamento { get; set; }
        public string descricao { get; set; }
        public int codigo_prioridade { get; set; }
        public string prioridade { get; set; }
        public int codigo_periodicidade { get; set; }
        public string periodicidade { get; set; }
        public int codigo_tipo_servico { get; set; }
        public string tipo_servico { get; set; }
        public int codigo_tipo_ordem_servico { get; set; }
        public string tipo_ordem_servico { get; set; }
        public float valor_previsto { get; set; }
        public int quantidade_equipamento { get; set; }
        public string intervalo { get; set; }
        public long codigo_checklist { get; set; }
        public string checklist { get; set; }
        public bool exige_laudo { get; set; }
        public bool ativo { get; set; }
        public bool envia_email { get; set; }
        public string email { get; set; }
        public int dias_alerta { get; set; }
        public int codigo_modulo { get; set; }
        public string tempo_estimado { get; set; }
        public string modulo { get; set; }
        public bool segunda { get; set; }
        public bool terca { get; set; }
        public bool quarta { get; set; }
        public bool quinta { get; set; }
        public bool sexta { get; set; }
        public bool sabado { get; set; }
        public bool domingo { get; set; }
    }

    public class Relatorio
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
        public bool ativo { get; set; }
    }

    public class Rotina
    {
        public long codigo { get; set; }
        public int codigo_categoria { get; set; }
        public string categoria { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public long codigo_equipamento { get; set; }
        public string equipamento { get; set; }
        public string descricao { get; set; }
        public int codigo_periodicidade { get; set; }
        public string periodicidade { get; set; }
        public int codigo_tipo_servico { get; set; }
        public string tipo_servico { get; set; }
        public int codigo_tipo_ordem_servico { get; set; }
        public string tipo_ordem_servico { get; set; }
        public string intervalo { get; set; }
        public string data_inicio { get; set; }
        public string checklist { get; set; }
        public bool ativo { get; set; }
    }

    public class Setor
    {
        public int codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string descricao { get; set; }
        public string observacao { get; set; }
        public float metragem { get; set; }
        public float carga_termica { get; set; }
        public string descricao_atividade { get; set; }
        public int numero_pessoas_fixas { get; set; }
        public int numero_pessoas_volantes { get; set; }
        public bool ativo { get; set; }
        public List <SetorLocal> local { get; set; }
    }

    public class SetorLocal
    {
        public int codigo { get; set; }
        public string local { get; set; }
        public int excluido { get; set; }
    }

    public class Tarefa
    {
        public long codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string descricao { get; set; }
        public int codigo_periodicidade { get; set; }
        public string periodicidade { get; set; }
        public string intervalo { get; set; }
        public long codigo_checklist { get; set; }
        public string checklist { get; set; }
        public bool ativo { get; set; }
        public int codigo_modulo { get; set; }
        public string modulo { get; set; }
        public bool segunda { get; set; }
        public bool terca { get; set; }
        public bool quarta { get; set; }
        public bool quinta { get; set; }
        public bool sexta { get; set; }
        public bool sabado { get; set; }
        public bool domingo { get; set; }
    }

    public class TipoApartamento
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
        public bool ativo { get; set; }
        public string texto_ativo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public long codigo_checklist_uh { get; set; }
        public string checklist_uh { get; set; }
        public int codigo_periodicidade_uh { get; set; }
        public int intervalo_uh { get; set; }
        public long codigo_checklist_governanca_permanencia { get; set; }
        public long codigo_checklist_governanca_saida { get; set; }
        public long codigo_checklist_governanca_manutencao { get; set; }
        public long codigo_checklist_governanca_permanencia_vistoria { get; set; }
        public long codigo_checklist_governanca_saida_vistoria { get; set; }
        public long codigo_checklist_governanca_manutencao_vistoria { get; set; }
        public string checklist_governanca_permanencia { get; set; }
        public string checklist_governanca_saida { get; set; }
        public string checklist_governanca_manutencao { get; set; }        
    }

    public class TipoArCondicionado
    {
        public int codigo { get; set; }
        public string tipo { get; set; }
        public string descricao { get; set; }
        public int codigo_periodicidade { get; set; }
        public string periodicidade { get; set; }
        public int intervalo { get; set; }
        public int codigo_checklist { get; set; }
        public string checklist { get; set; }
        public bool ativo { get; set; }
    }
    public class TipoCama
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
        public bool ativo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
    }

    public class TipoChecklist
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
        public bool ativo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
    }

    public class TipoChecklistItem
    {
        public int codigo_tipo_checklist { get; set; }
        public int codigo { get; set; }
        public string codigo_checklist { get; set; }
        public string checklist { get; set; }
        public string grupo { get; set; }
        public bool selecionado { get; set; }
    }

    public class TipoDespesa
    {
        public int codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public string codigo_tipo_despesa { get; set; }
        public string descricao { get; set; }
        public bool ativo { get; set; }
    }

    public class Treinamento
    {
        public int codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string unidade { get; set; }
        public int codigo_modulo { get; set; }
        public string modulo { get; set; }
        public string descricao { get; set; }
        public string comentario { get; set; }
        public string data_cadastro { get; set; }
        public string arquivo { get; set; }
        public string path_arquivo { get; set; }
        public bool ativo { get; set; }
    }

    public class Unidade
    {
        public int codigo { get; set; }
        public string nome_fantasia { get; set; }
        public string razao_social { get; set; }
        public string cnpj { get; set; }
        public string inscricao_estadual { get; set; }
        public string inscricao_municipal { get; set; }
        public string cep { get; set; }
        public string uf { get; set; }
        public string municipio { get; set; }
        public string logradouro { get; set; }
        public string numero { get; set; }
        public string bairro { get; set; }
        public string complemento { get; set; }
        public string telefone { get; set; }
        public int quantidade_andar { get; set; }
        public int quantidade_bloco { get; set; }
        public string imagem { get; set; }
        public string arquivo { get; set; }
        public bool aponta_horas { get; set; }
        public bool aponta_horas_qualidade { get; set; }
        public int quantidae_maxima_horas_apontamento { get; set; }
        public float area_total { get; set; }
        public float area_total_construida { get; set; }
        public int codigo_tipo_unidade { get; set; }
        public bool ativo { get; set; }
        public string hotel_opera { get; set; }
    }

    public class RelatorioItensAuditaveis
    {
        public long codigo { get; set; }
        public int codigo_unidade { get; set; }
        public string descricao { get; set; }
        public string unidade { get; set; }
        public bool ativo { get; set; }
    }

    public class RelatorioItensAuditaveisDetais
    {
        public long codigo_checklist { get; set; }
        public int codigo { get; set; }
        public string descricao { get; set; }
        public bool selecionado { get; set; }
    }

    public class UnidadeMedida
    {
        public int codigo { get; set; }
        public string sigla { get; set; }
        public string descricao { get; set; }
        public bool ativo { get; set; }
    }

    public class ItemOSHospede
    {
        public int codigo { get; set; }
        public int codigoUnidade { get; set; }
        public string unidade { get; set; }
        public string codigoGrupo { get; set; }
        public string descricao { get; set; }
        public bool ativo { get; set; }
    }

}
