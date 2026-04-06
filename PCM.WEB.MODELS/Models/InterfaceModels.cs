using System.Collections.Generic;

namespace PCM.WEB.MODELS
{
    public class UserCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class InterfaceUserInfo
    {
        public int codigoEmpresa { get; set; }
        public string name { get; set; }
        public bool success { get; set; }
    }

    public class InterfaceGreenPlanet
    {
        public long id { get; set; }
        public string unidade { get; set; }
        public string data { get; set; }
        public int codigoItemMedicao { get; set; }
        public string itemMedicao { get; set; }
        public string numeroHospedes { get; set; }
        public string UHOcupada { get; set; }
        public float medicao { get; set; }
        public float consumo { get; set; }
    }

    public class InterfaceStatusUH
    {
        public List<InterfaceStatusUHDetails> rows { get; set; }
    }

    public class InterfaceStatusUHDetails
    {
        public string IDHOTEL { get; set; }
        public string UH { get; set; }
        public string STATUSDAUH { get; set; }
        public string STATUSDAGOV { get; set; }
    }

    public class ReservasUH
    {
        public string UH { get; set; }
        public string dataChegada { get; set; }
        public string dataPartida { get; set; }
    }

    public class StatusUHInterface
    {
        public string hotelId { get; set; }
        public string uh { get; set; }
        public string status { get; set; }
    }

    public class statusUH
    {
        public List<statusUHInfo> rows { get; set; }
    }

    public class statusUHInfo
    {
        public int status { get; set; }
        public string description { get; set; }
    }


    public class resumoUnidades
    {
        public string unidade { get; set; }
        public int quantidadeOSGerada { get; set; }
        public int quantidadeOSAtendida { get; set; }
        public int quantidadeOSPendente { get; set; }
        public float laudo { get; set; }
        public float preventiva { get; set; }
        public float rotina { get; set; }
        public float pmoc { get; set; }
        public float uhDia { get; set; }
        public float greenPlanet { get; set; }
    }

    public class interfacePrincipaisOcorrencias
    {
        public int ranking { get; set; }
        public string descricao { get; set; }
        public long quantidade { get; set; }
    }

    public class interfaceOrdemServico
    {
        public int page { get; set; }
        public int totalPage { get; set; }
        public int totalRegistros { get; set; }
        public List<interfaceOrdemServicoDetails> os  { get; set; }
    }

    public class interfaceOrdemServicoDetails
    {
        public string unidade { get; set; }
        public string categoria { get; set; }
        public string numeroOrdemServico { get; set; }
        public string dataAbertura { get; set; }
        public string setor { get; set; }
        public string local { get; set; }
        public string equipamento { get; set; }
        public string prioridade { get; set; }
        public string tipoServico { get; set; }
        public string descricao { get; set; }
        public string status { get; set; }
        public string solicitante { get; set; }
        public string prazoExecucao { get; set; }
        public string executor { get; set; }
        public string departamento { get; set; }
        public string justificativaApontamento { get; set; }
        public string dataVinculo { get; set; }
        public string prazoExecucaoInicial { get; set; }
        public string justificativaAlteracaoPrazo { get; set; }
        public string usuarioAlteracao { get; set; }
        public string dataAlteracao { get; set; }
        public string justificativaCancelamento { get; set; }
        public string dataTermino { get; set; }
    }

    public class interfaceGovernanca
    {
        public int page { get; set; }
        public int totalPage { get; set; }
        public int totalRegistros { get; set; }
        public List<interfaceGovernancaDetails> apontamentos { get; set; }
    }

    public class interfaceGovernancaDetails
    {
        public string unidade { get; set; }
        public string dataInput { get; set; }
        public long codigoApontamento { get; set; }
        public string apartamento { get; set; }
        public string camareira { get; set; }
        public string vistoriador { get; set; }
        public string itemCheklist { get; set; }
        public string grupoChecklist { get; set; }
        public string subGrupoChecklist { get; set; }
        public string resposta { get; set; }
        public string observacao { get; set; }
    }

    public class interfaceRotina
    {
        public int page { get; set; }
        public int totalPage { get; set; }
        public int totalRegistros { get; set; }
        public List<interfaceRotinaDetails> rotina { get; set; }
    }

    public class interfaceRotinaDetails
    {
        public long codigo { get; set; }
        public string unidade { get; set; }
        public string setor { get; set; }
        public string rotina { get; set; }
        public string categoria { get; set; }
        public string tipoServico { get; set; }
        public List<interfaceRotinaDetailsExecutor> apontamentos { get; set; }
    }

    public class interfaceRotinaDetailsExecutor
    {
        public string nome { get; set; }
        public string inicio { get; set; }
        public string termino { get; set; }
        public string justificativa { get; set; }
    }

}