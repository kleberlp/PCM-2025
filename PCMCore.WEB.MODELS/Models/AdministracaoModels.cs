using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCM.WEB.MODELS
{
    public class Usuario
    {
        public int codigo { get; set; }
        public int codigoUnidade { get; set; }
        public string unidade { get; set; }
        public int codigoPerfil { get; set; }
        public string perfil { get; set; }
        public int codigoDepartamento { get; set; }
        public string departamento { get; set; }
        public string senha { get; set; }
        public string confirmarSenha { get; set; }
        public string nome { get; set; }
        public string apelido { get; set; }
        public string telefone { get; set; }
        public string email { get; set; }
        public string emailSenha { get; set; }
        public bool aplicativo { get; set; }
        public bool website { get; set; }
        public bool ativo { get; set; }
        public bool colaborador { get; set; }
        public bool contabilizaHoras { get; set; }
        public string valorHora { get; set; }
        public int codigoTipoFuncionario { get; set; }
        public int codigoFuncao { get; set; }
        public string modulo { get; set; }
        public int codigoModuloDefault { get; set; }
        public int codigoFuncionario { get; set; }
        public List<UsuarioUnidade> unidades { get; set; }
    }

    public class UsuarioUnidade
    {
        public int codigoUsuario { get; set; }
        public int codigoUnidade { get; set; }
        public string unidade { get; set; }
        public bool selecionado { get; set; }
    }

    public class Perfil
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
        public int hierarquia { get; set; }
        public bool ativo { get; set; }
    }

    public class PerfilDireito
    {
        public int codigo_perfil { get; set; }
        public string codigo_formulario { get; set; }
        public string formulario { get; set; }
        public bool visualizar { get; set; }
        public bool inserir { get; set; }
        public bool editar { get; set; }
        public bool excluir { get; set; }
        public bool imprimir { get; set; }
        public bool administrador { get; set; }
        public string direito { get; set; }
    }

    public class PerfilHierarquia
    {
        public string linha { get; set; }
        public List<string> coluna { get; set; }
    }

    public class Empresa
    {
        public int codigo { get; set; }
        public int codigo_tipo_empresa { get; set; }
        public string tipo { get; set; }
        public string cnpj { get; set; }
        public string nome_fantasia { get; set; }
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
        public string valor { get; set; }
        public string data_inicio{ get; set; }
        public bool ativo { get; set; }
        public string url { get; set; }
    }

    public class ConfiguracaoDesempenhoUnidades
    {
        public int codigo_empresa { get; set; }
        public string laudo { get; set; }
        public string preventiva { get; set; }
        public string rotina { get; set; }
        public string pmoc { get; set; }
        public string uh_dia { get; set; }
        public string os_atendimento_dia { get; set; }
        public string hh_nao_apontado { get; set; }
        public string os_pendente { get; set; }
        public string hh_extra { get; set; }
        public string preventiva_corretiva { get; set; }
        public string green_planet { get; set; }
    }
}
