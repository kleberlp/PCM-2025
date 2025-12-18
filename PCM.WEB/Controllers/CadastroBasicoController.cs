using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using NPOI.Util;
using PCM.WEB.DAL;
using PCM.WEB.MODELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static NPOI.HSSF.Util.HSSFColor;
using Table = CrystalDecisions.CrystalReports.Engine.Table;

namespace PCM.WEB.Controllers
{
    public class CadastroBasicoController : Controller
    {
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private CadastroBasico oCadastroBasico = new CadastroBasico(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Excel oExcel = new Excel(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        private enum eTipoChecklist { Preventiva = 1, Rotina = 2, PMOC = 3, UH = 4, Auditoria = 5, Qualidade = 6, Tarefa = 8, Governanca=10 };

        #region ::: JSON :::

        //JSON: /ANDAR/
        public JsonResult LoadAndar(int unidade)
        {
            return Json(oCombo.Andar(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                     iCodigoUnidade: unidade));
        }

        //JSON: /ANDAR/
        public JsonResult LoadAndarApartamento(int unidade, string bloco = "")
        {
            return Json(oCombo.AndarApartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: unidade,
                                                sBloco: bloco));
        }

        //JSON: /APARTAMENTO/
        public JsonResult LoadApartamento(int unidade, int setor = -1)
        {
            return Json(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                           iCodigoUnidade: unidade,
                                           iCodigoSetor: setor));
        }

        //JSON: /AR CONDICIONADO - PMOC/
        public JsonResult LoadArCondicionadoPMOC(int unidade)
        {
            return Json(oCombo.ArCondicionadoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                  iCodigoUnidade: unidade));
        }

        //JSON: /BLOCO/
        public JsonResult LoadBloco(int unidade)
        {
            return Json(oCombo.Bloco(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                     iCodigoUnidade:unidade));
        }

        //JSON: /ANDAR/
        public JsonResult LoadBlocoApartamento(int unidade)
        {
            return Json(oCombo.BlocoApartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: unidade));
        }

        //JSON: /CATEGORIA/
        public JsonResult LoadCategoria(int unidade)
        {
            return Json(oCombo.Categoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                         iCodigoUnidade: unidade));
        }

        //JSON: /CHECKLIST/
        public JsonResult LoadChecklist(int unidade, int codigo_tipo_checklist)
        {
            return Json(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                         iCodigoUnidade: unidade,
                                         iCodigoTipoChecklist: codigo_tipo_checklist));
        }

        //JSON: /DEPARTAMENTO/
        public JsonResult LoadDepartamento()
        {
            return Json(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())));
        }

        //JSON: /EQUIPAMENTO - EQUIPAMENTO/
        public JsonResult LoadEquipamentoSetor(int unidade, int setor)
        {
            return Json(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                           iCodigoUnidade: unidade,
                                           iCodigoSetor: setor));
        }

        //JSON: /EQUIPAMENTO - EQUIPAMENTO/
        public JsonResult LoadEquipamentoFamiliaSetor(int unidade, int setor, int familia)
        {
            return Json(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                           iCodigoUnidade: unidade,
                                           iCodigoSetor: setor,
                                           iCodigoFamiliaEquipamento: familia));
        }

        //JSON: /EQUIPAMENTO - EQUIPAMENTO/
        public JsonResult LoadEquipamento(int unidade)
        {
            return Json(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                           iCodigoUnidade: unidade));
        }

        //JSON: /EQUIPAMENTO - ITENS GERAIS/
        public JsonResult LoadItensGerais(int unidade)
        {
            return Json(oCombo.ItensGerais(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                           iCodigoUnidade: unidade));
        }

        //JSON: /FAMILIA EQUIPAMENTO/
        public JsonResult LoadFamiliaEquipamento(int unidade)
        {
            return Json(oCombo.FamiliaEquipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                  iCodigoUnidade: unidade));
        }

        //JSON: /FUNÇÃO/
        public JsonResult LoadFuncao(int unidade)
        {
            return Json(oCombo.Funcao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                      iCodigoUnidade: unidade));
        }

        //JSON: /GRUPO - ITEM DE MEDIÇÃO
        public JsonResult LoadGrupoItemMedicao(int unidade)
        {
            return Json(oCombo.GrupoItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: unidade));
        }

        //JSON: /GRUPO - CHECKLIST/
        public JsonResult LoadGrupoChecklist(int unidade)
        {
            return Json(oCombo.GrupoChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                              iCodigoUnidade: unidade));
        }

        //JSON: /GRUPO - PRODUTO
        public JsonResult LoadComboGrupoProduto(int unidade)
        {
            return Json(oCombo.GrupoProduto(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: unidade));
        }

        //JSON: /GRUPO - Programada
        public JsonResult LoadProgramada(int unidade, int tipo_ordem_servico)
        {
            return Json(oCombo.Programada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                          iCodigoUnidade: unidade,
                                          iCodigoTipoOrdemServico: tipo_ordem_servico));
        }

        //JSON: /SETOR/
        public JsonResult LoadSetor(int unidade)
        {
            return Json(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                     iCodigoUnidade: unidade));
        }

        //JSON: /TIPO AR CONDICIONADO/
        public JsonResult LoadTipoArCondicionado(int unidade)
        {
            return Json(oCombo.TipoArCondicionado(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                  iCodigoUnidade: unidade));
        }

        //JSON: /TIPO APARTAMENTO/
        public JsonResult LoadTipoApartamento(int unidade)
        {
            return Json(oCombo.TipoApartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                               iCodigoUnidade: unidade));
        }

        //JSON: /TIPO CAMA/
        public JsonResult LoadTipoCama(int unidade)
        {
            return Json(oCombo.TipoCama(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUnidade: unidade));
        }

        //JSON: /TIPO CHECKLIST/
        public JsonResult LoadTipoChecklist(int unidade)
        {
            return Json(oCombo.TipoChecklist2(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                             iCodigoUnidade: unidade));
        }

        //JSON: /USUÁRIO/
        public JsonResult LoadUsuario(int unidade)
        {
            return Json(oCombo.Usuario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                       iCodigoUnidade: unidade));
        }

        //JSON: /USUÁRIO/
        public JsonResult LoadUsuarioDepartamento(int unidade, int departamento)
        {
            return Json(oCombo.UsuarioDepartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                   iCodigoUnidade: unidade,
                                                   iCodigoDepartamento: departamento));
        }

        //JSON: /UNIDADE/
        public JsonResult LoadUnidade()
        {
            return Json(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                       iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                       bCadastro: true));
        }

        //JSON: /TIPO DE UNIDADE
        public JsonResult LoadTipoUnidade(int unidade)
        {
            return Json(oCadastroBasico.LoadTipoUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUnidade: unidade));
        }

        //JSON: /UNIDADE/
        public void SetSessionUnidade(int unidade)
        {
            Session["codigo_unidade"] = unidade;
        }

        //JSON: /CLIENTE
        public JsonResult LoadComboCliente(int unidade)
        {
            return Json(oCombo.Cliente(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                       codigoUnidade: unidade));
        }

        //JSON: /ENXOVAL
        public JsonResult LoadComboEnxoval(int unidade)
        {
            return Json(oCombo.Enxoval(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                       codigoUnidade: unidade));
        }

        #endregion

        #region ::: CEP :::

        //JSON: /CEP/
        public JsonResult CEPInfo(string cep)
        {
            return Json(oCadastroBasico.CEPInternet(cep));
        }

        //JSON: /MUNICIPIO/
        public JsonResult LoadMunicipio(string uf)
        {
            return Json(oCombo.Municipio(uf));
        }

        #endregion

        #region ::: APARTAMENTO :::

        // GET: INDEX
        public ActionResult ApartamentoIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;
                bool imprimir = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_apartamento",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.responsavelApartamento = new SelectList(oCombo.ResponsavelApartamento(), "codigo", "descricao", null);
                ViewBag.bloco = new SelectList(oCombo.Bloco(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.andar = new SelectList(oCombo.Andar(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", null);


                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.imprimir = imprimir;

                return View();
            }
        }

        // GET: INSERT
        [HttpPost]
        public JsonResult LoadApartamentoTable(int unidade, int responsavelApartamento, string bloco, string andar, int ativo)
        {
            return Json(oCadastroBasico.LoadApartamento(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        codigoUnidade: unidade,
                                                        codigoResponsavelApartamento: responsavelApartamento,
                                                        bloco: bloco,
                                                        andar: andar,
                                                        ativo: ativo));
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult ApartamentoInsert(int unidade, int setor, string apartamento, int tipo_apartamento, string bloco, int tipo_cama, int quantidade_cama, int responsavel_apartamento = 2, string descritivo = "", string metragem = "0", string carga_termica = "0", string descricao_atividade = "", int numero_pessoas_fixas = 0, int numero_pessoas_volantes = 0, bool ativo = false, string data_ultima_manutencao = "", int andar = 0)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertApartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: unidade,
                                                    iCodigoResponsavelApartamento: responsavel_apartamento,
                                                    iCodigoSetor: setor,
                                                    iCodigoTipoApartamento: tipo_apartamento,
                                                    sApartamento: apartamento,
                                                    sBloco: bloco,
                                                    iAndar: andar,
                                                    sDescritivo: descritivo,
                                                    iCodigoTipoCama: tipo_cama,
                                                    iQuantidadeCama: quantidade_cama,
                                                    dMetragem: Convert.ToDouble((metragem == "") ? "0" : metragem),
                                                    dCargaTermica: Convert.ToDouble((carga_termica == "") ? "0" : carga_termica),
                                                    sDescricaoAtividade: descricao_atividade.ToUpper(),
                                                    iNumeroPessoasFixas: numero_pessoas_fixas,
                                                    iNumeroPessoasVolantes: numero_pessoas_volantes,
                                                    bAtivo: ativo,
                                                    sDataUltimaManutencao: data_ultima_manutencao);

                return RedirectToAction("ApartamentoInsert");
            }
        }

        // GET: /EDIT
        public ActionResult ApartamentoEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Apartamento apartamento = null;

                oCadastroBasico.InfoApartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigo: codigo,
                                                oApartamento: ref apartamento);

                if (apartamento == null)
                {
                    return HttpNotFound();
                }

                ViewBag.bloco = new SelectList(oCombo.Bloco(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                            iCodigoUnidade: apartamento.codigo_unidade), "codigo", "descricao", apartamento.bloco);
                ViewBag.responsavel_apartamento = new SelectList(oCombo.ResponsavelApartamento(), "codigo", "descricao", apartamento.codigo_responsavel_apartamento);
                ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: apartamento.codigo_unidade), "codigo", "descricao", apartamento.codigo_setor);
                ViewBag.andar = new SelectList(oCombo.Andar(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                            iCodigoUnidade: apartamento.codigo_unidade), "codigo", "descricao", apartamento.andar);
                ViewBag.tipo_apartamento = new SelectList(oCombo.TipoApartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                                    iCodigoUnidade: apartamento.codigo_unidade), "codigo", "descricao", apartamento.codigo_tipo_apartamento);
                ViewBag.tipo_cama = new SelectList(oCombo.TipoCama(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                    iCodigoUnidade: apartamento.codigo_unidade), "codigo", "descricao", apartamento.codigo_tipo_cama);
                ViewBag.codigo_tipo_unidade = apartamento.codigo_tipo_unidade;
                ViewBag.data_ultima_manutencao = apartamento.data_ultima_manutencao;

                return View(apartamento);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApartamentoEdit(int codigo_unidade, int setor, string apartamento, int tipo_apartamento, string bloco, int tipo_cama, int quantidade_cama, int codigo, int responsavel_apartamento = 2, string descritivo = "", string metragem = "0", string carga_termica = "0", string descricao_atividade = "", int numero_pessoas_fixas = 0, int numero_pessoas_volantes = 0, bool ativo = false, string data_ultima_manutencao = "", int andar = 0)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateApartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    iCodigoResponsavelApartamento: responsavel_apartamento,
                                                    iCodigoSetor: setor,
                                                    iCodigoTipoApartamento: tipo_apartamento,
                                                    sApartamento: apartamento,
                                                    sBloco: bloco,
                                                    iAndar: andar,
                                                    sDescritivo: descritivo,
                                                    iCodigoTipoCama: tipo_cama,
                                                    iQuantidadeCama: quantidade_cama,
                                                    dMetragem: Convert.ToDouble((metragem == "") ? "0" : metragem),
                                                    dCargaTermica: Convert.ToDouble((carga_termica == "") ? "0" : carga_termica),
                                                    sDescricaoAtividade: descricao_atividade.ToUpper(),
                                                    iNumeroPessoasFixas: numero_pessoas_fixas,
                                                    iNumeroPessoasVolantes: numero_pessoas_volantes,
                                                    bAtivo: ativo,
                                                    sDataUltimaManutencao: data_ultima_manutencao,
                                                    iCodigo: codigo);

                //Redireciona para Index
                return RedirectToAction("ApartamentoIndex");
            }
        }

        // GET: /DELETE
        public ActionResult ApartamentoDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Apartamento apartamento = null;

                oCadastroBasico.InfoApartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigo: codigo,
                                                oApartamento: ref apartamento);

                if (apartamento == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(apartamento);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApartamentoDelete([Bind(Include = "codigo")] Apartamento apartamento)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteApartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigo: apartamento.codigo);
                    //Redireciona para Index
                    return RedirectToAction("ApartamentoIndex");
                }
                catch
                {
                    return ApartamentoDelete(codigo: apartamento.codigo,
                                                erro: PCM.WEB.Properties.Resources.valida_excluir);
                }

            }

        }

        //JSON: /VALIDA FUNÇÃO
        public JsonResult ValidaApartamento(int unidade, string apartamento, int codigo)
        {

            return Json(oCadastroBasico.ValidaApartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            sApartamento: apartamento,
                                                            iCodigo: codigo));

        }

        // POST: /PRINT TAG EQUIPAMENTO
        public ActionResult PrintEtiquetaApartamento(string codigo, string report)
        {

            ReportDocument oReportDocument = new ReportDocument();
            TableLogOnInfo oTableLogOnInfo = new TableLogOnInfo();
            ConnectionInfo oConnectionInfo = new ConnectionInfo();
            Database oCrDatabase;
            Tables oCrTables;

            oConnectionInfo.ServerName = ConfigurationManager.AppSettings["data_source"].ToString();
            oConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["initial_catalog"].ToString();
            oConnectionInfo.UserID = ConfigurationManager.AppSettings["user_id"].ToString();
            oConnectionInfo.Password = ConfigurationManager.AppSettings["password"].ToString();
            oConnectionInfo.Type = ConnectionInfoType.SQL;
            oConnectionInfo.IntegratedSecurity = false;

            if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Reports"), report + Session["empresa"].ToString() + ".rpt")))
            {
                report = report + Session["empresa"].ToString();
            }

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), report + ".rpt"));

            oCrDatabase = oReportDocument.Database;
            oCrTables = oCrDatabase.Tables;

            foreach (Table crTable in oCrTables)
            {
                oTableLogOnInfo = crTable.LogOnInfo;
                oTableLogOnInfo.ConnectionInfo = oConnectionInfo;
                crTable.ApplyLogOnInfo(oTableLogOnInfo);
            }

            oReportDocument.SetParameterValue("@codigo", codigo);
            oReportDocument.SetParameterValue("@codigo_empresa", Convert.ToInt32(Session["empresa"].ToString()));

            oReportDocument.SetDatabaseLogon(ConfigurationManager.AppSettings.GetValues("user_id")[0],
                                                ConfigurationManager.AppSettings.GetValues("password")[0],
                                                ConfigurationManager.AppSettings.GetValues("data_source")[0],
                                                ConfigurationManager.AppSettings.GetValues("initial_catalog")[0]);

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=" + report + ".pdf");
            return File(stream, "application/pdf;");
        }

        #region ::: OPERA :::

        //JSON: /VALIDA FUNÇÃO
        [AllowAnonymous]
        public async Task<JsonResult> AuthenticationOpera()
        {
            using (var client = new HttpClient())
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var userName = "ACCORBUP_Client";
                var userPassword = "DUXANO8I-fwTVgNv-m8qad5x";

                var authenticationString = $"{userName}:{userPassword}";
                var base64String = Convert.ToBase64String(
                   System.Text.Encoding.ASCII.GetBytes(authenticationString));

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://acc2-uat-oc.hospitality-api.us-ashburn-1.ocs.oc-test.com/oauth/v1/tokens");                                                                              
                requestMessage.Headers.Authorization =
                   new AuthenticationHeaderValue("Basic", base64String);

                // Define os dados que serão enviados no corpo da solicitação
                var dados = new Dictionary<string, string>
                {
                    { "username", "UAT-SIM" },
                    { "password", "IFCPCS#Opera2023" },
                    { "grant_type", "password" }
                };
                var conteudo = new FormUrlEncodedContent(dados);
                requestMessage.Headers.Add("x-app-key", "88310536-d938-4068-b7ee-15ae183c4840");
                requestMessage.Content = conteudo;
                var result = await client.SendAsync(requestMessage);

                if (result.IsSuccessStatusCode)
                {
                    // Lida com a resposta da API de acordo com o conteúdo retornado
                    string content = await result.Content.ReadAsStringAsync();
                    AuthenticationOpera resultado = JsonConvert.DeserializeObject<AuthenticationOpera>(content);
                    Session["token_opera"] = resultado.access_token;
                    Session["token_type"] = resultado.token_type;
                    return Json(true);
                } else
                {
                    return Json(false);
                }
            }

        }

        //JSON: /VALIDA FUNÇÃO
        public async Task<JsonResult> GetHotels()
        {
            using (var client = new HttpClient())
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                int page = 1;
                int total_page = 1;

                for (page = 1; page <= total_page; page++)
                {
                    HttpRequestMessage requestMessage;
                    
                    if (page == 1)
                    {
                        requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://acc2-uat-oc.hospitality-api.us-ashburn-1.ocs.oc-test.com/fof/v1/hotels/" + Session["hotel_opera"].ToString() + "/rooms?limit=500");
                    } else
                    {
                        requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://acc2-uat-oc.hospitality-api.us-ashburn-1.ocs.oc-test.com/fof/v1/hotels/\" + Session[\"hotel_opera\"].ToString() + \"/rooms?page=" + page.ToString());
                    }
                    
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue(Session["token_type"].ToString(), Session["token_opera"].ToString());
                    requestMessage.Headers.Add("x-hotelid", Session["hotel_opera"].ToString());
                    requestMessage.Headers.Add("x-app-key", "88310536-d938-4068-b7ee-15ae183c4840");
                    var result = await client.SendAsync(requestMessage);

                    if (result.IsSuccessStatusCode)
                    {
                        // Lida com a resposta da API de acordo com o conteúdo retornado
                        string content = await result.Content.ReadAsStringAsync();
                        HotelRoomsOpera resultado = JsonConvert.DeserializeObject<HotelRoomsOpera>(content);

                        oCadastroBasico.InterfaceHotelRoomsOpera(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                 iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                 oHotelRoomsOpera: resultado);

                        total_page = resultado.totalPages;

                    }
                }

                return Json(true);

            }

        }

        #endregion

        #endregion

        #region ::: AR CONDICIONADO :::

        // GET: INDEX
        public ActionResult ArCondicionadoIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;
                bool imprimir = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_ar_condicionado",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.imprimir = imprimir;
                

                ViewBag.tag = "";
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.tipo_ar_condicionado = new SelectList(oCombo.TipoArCondicionado(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.apartamento = new SelectList(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", null);
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", 1);


                return View();
            }
        }

        // POST: INDEX
        [HttpPost]
        public ActionResult ArCondicionadoIndex(string tag, int tipo_ar_condicionado = -1, int unidade = -1, int setor = -1, int apartamento = -1, int departamento = -1, int ativo = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;
                bool imprimir = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUnidade: unidade,
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_ar_condicionado",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.imprimir = imprimir;

                ViewBag.tag = tag;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: true), "codigo", "descricao", unidade);
                ViewBag.tipo_ar_condicionado = new SelectList(oCombo.TipoArCondicionado(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", tipo_ar_condicionado);
                ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: unidade), "codigo", "descricao", setor);
                ViewBag.apartamento = new SelectList(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: unidade), "codigo", "descricao", apartamento);
                ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", departamento);
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", ativo);

                return View();
            }
        }

        public JsonResult LoadArCondicionado(string tag, int tipo_ar_condicionado = -1, int unidade = -1, int setor = -1, int apartamento = -1, int departamento = -1, int ativo = -1)
        {
            List<MODELS.ArCondicionado> result = new List<MODELS.ArCondicionado>();

            result = oCadastroBasico.IndexArCondicionado(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                         iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                         iCodigoUnidade: unidade,
                                                         sTAG: tag,
                                                         iCodigoTipoArCondicionado: tipo_ar_condicionado,
                                                         iCodigoDepartamento: departamento,
                                                         iCodigoSetor: setor,
                                                         iCodigoApartamento: apartamento,
                                                         iAtivo: ativo);

            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        // GET: INSERT
        public ActionResult ArCondicionadoInsert()
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                    ViewBag.tipo_ar_condicionado = new SelectList(oCombo.TipoArCondicionado(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                    ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", null);
                    ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                    ViewBag.apartamento = new SelectList(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                    ViewBag.potencia_ar_condicionado = new SelectList(oCombo.PotenciaArCondicionado(), "codigo", "descricao", null);
                    ViewBag.ar_condicionado_pmoc = new SelectList(oCombo.ArCondicionado(iCodigoEmpresa: Convert.ToInt32(Session["codigo_empresa_pmoc"].ToString()),
                                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade_pmoc"].ToString())), "codigo", "descricao", null);

                    return View();
                }
            }

        // POST: INSERT
        [HttpPost]
        public ActionResult ArCondicionadoInsert(int unidade, int tipo_ar_condicionado, int departamento, string tag, string descricao, string fabricante, string endereco_fabricante, string contato_fabricante, string modelo, string numero_fabricacao, string ano_fabricacao, string caracteristicas, int potencia_ar_condicionado, string data_proxima_manutencao, string arquivo, string potencia = "0", int setor = -1, int apartamento = -1, bool ativo = false, long ar_condicionado_pmoc = 0, int empresa_pmoc = 0, int andar = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertArCondicionado(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigoUnidade: unidade,
                                                        iCodigoTipoArCondicionado: tipo_ar_condicionado,
                                                        iCodigoDepartamento: departamento,
                                                        iCodigoEmpresaPMOC: empresa_pmoc,
                                                        lCodigoArCondicionadoPMOC: ar_condicionado_pmoc,
                                                        sTag: tag,
                                                        sDescricao: descricao,
                                                        iCodigoSetor: setor,
                                                        iCodigoApartamento: apartamento,
                                                        sFabricante: fabricante,
                                                        sEnderecoFabricante: endereco_fabricante,
                                                        sContatoFabricante: contato_fabricante,
                                                        sModelo: modelo,
                                                        sNumeroFabricacao: numero_fabricacao,
                                                        sAnoFabricacao: ano_fabricacao,
                                                        sCaracteristicas: caracteristicas,
                                                        dPotencia: Convert.ToDouble(potencia) / 100.0,
                                                        iCodigoPotenciaArCondicionado: potencia_ar_condicionado,
                                                        iAndar: andar,
                                                        sDataUltimaManutencao: data_proxima_manutencao,
                                                        sArquivo: arquivo,
                                                        bAtivo: ativo);

                return RedirectToAction("ArCondicionadoInsert");
            }
        }

        // GET: /EDIT
        public ActionResult ArCondicionadoEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ArCondicionado ar_condicionado = null;

                oCadastroBasico.InfoArCondicionado(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    lCodigo: codigo,
                                                    oArCondicionado: ref ar_condicionado);

                if (ar_condicionado == null)
                {
                    return HttpNotFound();
                }

                ViewBag.tipo_ar_condicionado = new SelectList(oCombo.TipoArCondicionado(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                        iCodigoUnidade: ar_condicionado.codigo_unidade), "codigo", "descricao", ar_condicionado.codigo_tipo_ar_condicionado);
                ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", ar_condicionado.codigo_departamento);
                ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: ar_condicionado.codigo_unidade), "codigo", "descricao", ar_condicionado.codigo_setor);
                ViewBag.apartamento = new SelectList(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: ar_condicionado.codigo_unidade,
                                                                        iCodigoSetor: ar_condicionado.codigo_setor), "codigo", "descricao", ar_condicionado.codigo_apartamento);
                ViewBag.potencia_ar_condicionado = new SelectList(oCombo.PotenciaArCondicionado(), "codigo", "descricao", ar_condicionado.codigo_potencia_ar_condicionado);
                ViewBag.data_proxima_manutencao = ar_condicionado.data_proxima_manutencao;
                ViewBag.ar_condicionado_pmoc = new SelectList(oCombo.ArCondicionado(iCodigoEmpresa: ar_condicionado.codigo_empresa_pmoc,
                                                                                    iCodigoUnidade: ar_condicionado.codigo_unidade_pmoc), "codigo", "descricao", ar_condicionado.codigo_ar_condicionado_pmoc);

                return View(ar_condicionado);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ArCondicionadoEdit(int codigo_unidade, int tipo_ar_condicionado, int departamento, string tag, string descricao, string fabricante, string endereco_fabricante, string contato_fabricante, string modelo, string numero_fabricacao, string ano_fabricacao, string caracteristicas, int potencia_ar_condicionado, string data_proxima_manutencao, string arquivo, long codigo, int codigo_empresa_pmoc, string potencia = "0", int setor = -1, int apartamento = -1, bool ativo = false, long ar_condicionado_pmoc = 0, int andar = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Atualiza Registro no Banco de Dados
                oCadastroBasico.UpdateArCondicionado(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigoUnidade: codigo_unidade,
                                                        iCodigoTipoArCondicionado: tipo_ar_condicionado,
                                                        iCodigoDepartamento: departamento,
                                                        iCodigoEmpresaPMOC: codigo_empresa_pmoc,
                                                        lCodigoArCondicionadoPMOC: ar_condicionado_pmoc,
                                                        sTag: tag,
                                                        sDescricao: descricao,
                                                        iCodigoSetor: setor,
                                                        iCodigoApartamento: apartamento,
                                                        sFabricante: fabricante,
                                                        sEnderecoFabricante: endereco_fabricante,
                                                        sContatoFabricante: contato_fabricante,
                                                        sModelo: modelo,
                                                        sNumeroFabricacao: numero_fabricacao,
                                                        sAnoFabricacao: ano_fabricacao,
                                                        sCaracteristicas: caracteristicas,
                                                        dPotencia: Convert.ToDouble(potencia)/100.0,
                                                        iCodigoPotenciaArCondicionado: potencia_ar_condicionado,
                                                        iAndar: andar,
                                                        sDataUltimaManutencao: data_proxima_manutencao,
                                                        sArquivo: arquivo,
                                                        bAtivo: ativo,
                                                        lCodigo: codigo);


                //Redireciona para Index
                return RedirectToAction("ArCondicionadoIndex");
            }
        }

        // GET: /DELETE
        public ActionResult ArCondicionadoDelete(long codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ArCondicionado ar_condicionado = null;

                oCadastroBasico.InfoArCondicionado(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    lCodigo: codigo,
                                                    oArCondicionado: ref ar_condicionado);

                if (ar_condicionado == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(ar_condicionado);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ArCondicionadoDelete([Bind(Include = "codigo")] ArCondicionado ar_condicionado)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Exclui Registro no Banco de Dados
                    oCadastroBasico.DeleteArCondicionado(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            lCodigo: ar_condicionado.codigo);

                    //Redireciona para Index
                    return RedirectToAction("ArCondicionadoIndex");
                }
                catch
                {
                    return ArCondicionadoDelete(codigo: ar_condicionado.codigo,
                                                erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA EQUIPAMENTO
        public JsonResult ValidaArCondicionado(int unidade, string tag, long codigo)
        {

            return Json(oCadastroBasico.ValidaArCondicionado(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: unidade,
                                                                sTag: tag,
                                                                lCodigo: codigo));

        }

        //JSON: /Ar Condicionado PMOC
        public JsonResult ArCondicionadoPMOC(int unidade)
        {

            return Json(oCadastroBasico.VerificaArCondicionadoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: unidade));

        }

        //JSON: /VALIDA EQUIPAMENTO
        public JsonResult ValidaArCondicionadoPMOC(int unidade, long ar_condicionado_pmoc, long codigo)
        {

            return Json(oCadastroBasico.ValidaArCondicionadoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: unidade,
                                                                    lCodigoArCondicionadoPMOC: ar_condicionado_pmoc,
                                                                    lCodigo: codigo));

        }

        #endregion

        #region ::: ATIVIDADE :::

        // GET: INDEX
        public ActionResult AtividadeIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_atividade",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                return View(oCadastroBasico.IndexAtividade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()), 
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())));
            }
        }

        // GET: INSERT
        public ActionResult AtividadeInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.categoria = new SelectList(oCombo.Categoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.equipamento = new SelectList(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.tipo_servico = new SelectList(oCombo.TipoServico(), "codigo", "descricao", null);
                ViewBag.bloco = new SelectList(oCombo.BlocoApartamento(Convert.ToInt32(Session["empresa"].ToString()), Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.andar = new SelectList(oCombo.AndarApartamento(Convert.ToInt32(Session["empresa"].ToString()), Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                    
                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult AtividadeInsert(int unidade, int categoria, string titulo, string descricao, int tipo_servico, string data_previsao_termino, string[] bloco, string[] andar, int equipamento = -1, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertAtividade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                iCodigoUnidade: unidade,
                                                iCodigoCategoria: categoria,
                                                lCodigoEquipamento: equipamento,
                                                sTitulo: titulo,
                                                sDescricao: descricao,
                                                iCodigoTipoServico: tipo_servico,
                                                sDataPrevisaoTermino: data_previsao_termino,
                                                sBloco: (bloco == null)? "": string.Join(",", bloco),
                                                sAndar: (andar == null) ? "" : string.Join(",", andar),
                                                bAtivo: ativo);

                return RedirectToAction("AtividadeInsert");
            }
        }

        // GET: /EDIT
        public ActionResult AtividadeEdit(int codigo, int codigo_unidade)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Atividade atividade = null;

                oCadastroBasico.InfoAtividade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                lCodigo: codigo,
                                                iCodigoUnidade: codigo_unidade,
                                                oAtividade: ref atividade);

                if (atividade == null)
                {
                    return HttpNotFound();
                }

                ViewBag.categoria = new SelectList(oCombo.Categoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: atividade.codigo_unidade), "codigo", "descricao", atividade.codigo_categoria);
                ViewBag.equipamento = new SelectList(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: atividade.codigo_unidade), "codigo", "descricao", atividade.codigo_equipamento);
                ViewBag.tipo_servico = new SelectList(oCombo.TipoServico(), "codigo", "descricao", atividade.codigo_tipo_servico);
                ViewBag.data_previsao_termino = atividade.data_previsao_termino;
                ViewBag.bloco = new SelectList(oCombo.BlocoApartamento(Convert.ToInt32(Session["empresa"].ToString()), atividade.codigo_unidade), "codigo", "descricao", null);
                ViewBag.andar = new SelectList(oCombo.AndarApartamento(Convert.ToInt32(Session["empresa"].ToString()), atividade.codigo_unidade, atividade.bloco.Replace("[", "").Replace("]", "")), "codigo", "descricao", null);
                ViewBag.registro_bloco = atividade.bloco;
                ViewBag.registro_andar = atividade.andar;

                return View(atividade);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AtividadeEdit(int codigo_unidade, int codigo_unidade_old, int categoria, string titulo, string descricao, int tipo_servico, string data_previsao_termino, long codigo, string[] bloco, string[] andar, int equipamento = -1, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateAtividade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                iCodigoUnidade: codigo_unidade,
                                                iCodigoCategoria: categoria,
                                                lCodigoEquipamento: equipamento,
                                                sTitulo: titulo,
                                                sDescricao: descricao,
                                                iCodigoTipoServico: tipo_servico,
                                                sDataPrevisaoTermino: data_previsao_termino,
                                                sBloco: (bloco == null) ? "" : string.Join(",", bloco),
                                                sAndar: (andar == null) ? "" : string.Join(",", andar),
                                                bAtivo: ativo,
                                                lCodigo: codigo,
                                                iCodigoUnidadeOld: codigo_unidade_old);

                //Redireciona para Index
                return RedirectToAction("AtividadeIndex");
            }
        }

        // GET: /DELETE
        public ActionResult AtividadeDelete(long codigo, int codigo_unidade, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Atividade atividade = null;

                oCadastroBasico.InfoAtividade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                lCodigo: codigo,
                                                iCodigoUnidade: codigo_unidade,
                                                oAtividade: ref atividade);

                if (atividade == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(atividade);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AtividadeDelete([Bind(Include = "codigo, codigo_unidade")] Atividade atividade)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteAtividade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    lCodigo: atividade.codigo,
                                                    iCodigoUnidade: atividade.codigo_unidade);

                    //Redireciona para Index
                    return RedirectToAction("AtividadeIndex");
                }
                catch
                {
                    return AtividadeDelete(codigo: atividade.codigo,
                                            codigo_unidade: atividade.codigo_unidade,
                                            erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        #endregion

        #region ::: AUDITORIA QUALIDADE :::

        // GET: INDEX
        public ActionResult AuditoriaQualidadeIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_auditoria_qualidade",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;


                return View(oCadastroBasico.IndexAuditoriaQualidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"]),
                                                                    iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])));
            }
        }

        // GET: INSERT
        public ActionResult AuditoriaQualidadeInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.checklist = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                    iCodigoTipoChecklist: 6), "codigo", "descricao", null);
                ViewBag.periodicidade = new SelectList(oCombo.Periodicidade(bChecklist: false), "codigo", "descricao", null);

                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult AuditoriaQualidadeInsert(int unidade, string descricao, int checklist, int periodicidade = -1, int intervalo = 0, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertAuditoriaQualidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUnidade: unidade,
                                                        sDescricao: descricao,
                                                        iCodigoChecklist: checklist,
                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                        iCodigoPeriodicidade: periodicidade,
                                                        iIntervalo: intervalo,
                                                        bAtivo: ativo,
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()));


                return RedirectToAction("AuditoriaQualidadeInsert");
            }
        }

        // GET: EDIT
        public ActionResult AuditoriaQualidadeEdit(int codigo, int unidade)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                AuditoriaQualidade Auditoria = null;

                oCadastroBasico.InfoAuditoriaQualidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUnidade: unidade,
                                                        iCodigo: codigo,
                                                        oAuditoriaQualidade: ref Auditoria);

                if (Auditoria == null)
                {
                    return HttpNotFound();
                }

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: true), "codigo", "descricao", Auditoria.codigo_unidade);
                ViewBag.checklist = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                    iCodigoTipoChecklist: 6), "codigo", "descricao", Auditoria.codigo_checklist);
                ViewBag.periodicidade = new SelectList(oCombo.Periodicidade(bChecklist: false), "codigo", "descricao", Auditoria.codigo_periodicidade);

                ViewBag.ativo = (Auditoria.ativo) ? "checked" : "";

                return View(Auditoria);
            }
        }

        // POST: EDIT
        [HttpPost]
        public ActionResult AuditoriaQualidadeEdit(int unidade, string descricao, int checklist, int codigo, int periodicidade = -1, int intervalo = 0, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateAuditoriaQualidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUnidade: unidade,
                                                        sDescricao: descricao,
                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                        iCodigoChecklist: checklist,
                                                        iCodigoPeriodicidade: periodicidade,
                                                        iIntervalo: intervalo,
                                                        bAtivo: ativo,
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigo: codigo);


                return RedirectToAction("AuditoriaQualidadeIndex");
            }
        }

        // GET: /DELETE
        public ActionResult AuditoriaQualidadeDelete(int unidade, int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                AuditoriaQualidade Auditoria = null;

                oCadastroBasico.InfoAuditoriaQualidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUnidade: unidade,
                                                        iCodigo: codigo,
                                                        oAuditoriaQualidade: ref Auditoria);

                if (Auditoria == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(Auditoria);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AuditoriaQualidadeDelete(int unidade, int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteAuditoriaQualidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            iCodigoUnidade: unidade,
                                                            iCodigo: codigo);

                    //Redireciona para Index
                    return RedirectToAction("AuditoriaQualidadeIndex");
                }
                catch
                {
                    return AuditoriaQualidadeDelete(unidade: unidade,
                                                    codigo: codigo,
                                                    erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        #endregion

        #region ::: AUDITORIA CORPORATIVA :::

        // GET: INDEX
        public ActionResult AuditoriaCorporativoIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_auditoria_corporativo",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;


                return View(oCadastroBasico.IndexAuditoriaComporativo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"]),
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])));
            }
        }

        // GET: INSERT
        public ActionResult AuditoriaCorporativoInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.checklist = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                    iCodigoTipoChecklist: 5), "codigo", "descricao", null);

                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult AuditoriaCorporativoInsert(int unidade, string descricao, int checklist, bool ativo = false, bool gerar_plano_acao = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertAuditoriaCorporativo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            sDescricao: descricao,
                                                            iCodigoChecklist: checklist,
                                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                            bAtivo: ativo,
                                                            bGerarPlanoAcao: gerar_plano_acao,
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()));


                return RedirectToAction("AuditoriaCorporativoInsert");
            }
        }

        // GET: EDIT
        public ActionResult AuditoriaCorporativoEdit(int codigo, int unidade)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                AuditoriaCorporativo Auditoria = null;

                oCadastroBasico.InfoAuditoriaCorporativo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            iCodigo: codigo,
                                                            oAuditoriaCorporativo: ref Auditoria);

                if (Auditoria == null)
                {
                    return HttpNotFound();
                }

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Auditoria.codigo_unidade);
                ViewBag.checklist = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Auditoria.codigo_unidade,
                                                                    iCodigoTipoChecklist: 5), "codigo", "descricao", Auditoria.codigo_checklist);
                ViewBag.ativo = (Auditoria.ativo) ? "checked" : "";
                ViewBag.gerar_plano_acao = (Auditoria.gerar_plano_acao) ? "checked" : "";

                return View(Auditoria);
            }
        }

        // POST: EDIT
        [HttpPost]
        public ActionResult AuditoriaCorporativoEdit(int unidade, string descricao, int checklist, int codigo, bool ativo = false, bool gerar_plano_acao = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateAuditoriaCorporativo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            sDescricao: descricao,
                                                            iCodigoChecklist: checklist,
                                                            bAtivo: ativo,
                                                            bGerarPlanoAcao: gerar_plano_acao,
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            iCodigo: codigo);


                return RedirectToAction("AuditoriaCorporativoIndex");
            }
        }

        // GET: /DELETE
        public ActionResult AuditoriaCorporativoDelete(int unidade, int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                AuditoriaCorporativo Auditoria = null;

                oCadastroBasico.InfoAuditoriaCorporativo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            iCodigo: codigo,
                                                            oAuditoriaCorporativo: ref Auditoria);

                if (Auditoria == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(Auditoria);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AuditoriaCorporativoDelete(int unidade, int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteAuditoriaCorporativo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                iCodigoUnidade: unidade,
                                                                iCodigo: codigo);

                    //Redireciona para Index
                    return RedirectToAction("AuditoriaCorporativoIndex");
                }
                catch
                {
                    return AuditoriaCorporativoDelete(unidade: unidade,
                                                        codigo: codigo,
                                                        erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        #endregion

        #region ::: CATEGORIA :::

        // GET: INDEX
        public ActionResult CategoriaIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_categoria",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                return View(oCadastroBasico.IndexCategoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())));
        }
        }

        // GET: INSERT
        public ActionResult CategoriaInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult CategoriaInsert(int unidade, string descricao, string observacao, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertCategoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                iCodigoUnidade: unidade,
                                                sDescricao: descricao,
                                                bAtivo: ativo);

                return RedirectToAction("CategoriaInsert");
            }
        }

        // GET: /EDIT
        public ActionResult CategoriaEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Categoria categoria = null;

                oCadastroBasico.InfoCategoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigo: codigo,
                                            oCategoria: ref categoria);

                if (categoria == null)
                {
                    return HttpNotFound();
                }
                    
                return View(categoria);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CategoriaEdit(int codigo_unidade, string descricao, string observacao, int codigo, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateCategoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                iCodigoUnidade: codigo_unidade,
                                                sDescricao: descricao,
                                                bAtivo: ativo,
                                                iCodigo: codigo);

                //Redireciona para Index
                return RedirectToAction("CategoriaIndex");
            }
        }

        // GET: /DELETE
        public ActionResult CategoriaDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Categoria categoria = null;

                oCadastroBasico.InfoCategoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigo: codigo,
                                            oCategoria: ref categoria);

                if (categoria == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(categoria);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CategoriaDelete([Bind(Include = "codigo")] Categoria categoria)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteCategoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigo: categoria.codigo);

                    //Redireciona para Index
                    return RedirectToAction("CategoriaIndex");
                }
                catch
                {
                    return CategoriaDelete(codigo: categoria.codigo,
                                            erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA FUNÇÃO
        public JsonResult ValidaCategoria(int unidade, string descricao, int codigo)
        {

            return Json(oCadastroBasico.ValidaCategoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUnidade: unidade,
                                                        sDescricao: descricao,
                                                        iCodigo: codigo));

        }

        #endregion

        #region ::: CHECKLIST :::

        // GET: INDEX
        public ActionResult ChecklistIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_checklist",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.codigo_unidade = Session["codigo_unidade"].ToString();
                ViewBag.tipo_checklist = new SelectList(oCombo.TipoChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", null);
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Convert.ToInt32(Session["codigo_unidade"].ToString()));

                return View(oCadastroBasico.IndexChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                            iCodigoTipoChecklist: -1,
                                                            sDescricao: "",
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())));
            }
        }

        // POST: INDEX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChecklistIndex(int unidade = -1, int tipo_checklist = -1, string descricao = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_checklist",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.descricao = descricao;
                ViewBag.codigo_unidade = Session["codigo_unidade"].ToString();
                ViewBag.tipo_checklist = new SelectList(oCombo.TipoChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", tipo_checklist);
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);

                return View(oCadastroBasico.IndexChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                            iCodigoTipoChecklist: tipo_checklist,
                                                            sDescricao: descricao,
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())));
            }
        }

        // GET: INSERT
        public ActionResult ChecklistInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.unidade_input = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                        bCadastro: false), "codigo", "descricao", null);
                ViewBag.tipo_checklist_input = new SelectList(oCombo.TipoChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", null);
                ViewBag.tipo_checklist_item_input = new SelectList(oCombo.TipoItemChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", null);
                ViewBag.periodicidade_input = new SelectList(oCombo.Periodicidade(bChecklist: false), "codigo", "descricao", null);
                ViewBag.auditado_input = new SelectList(oCombo.SimNao(), "codigo", "descricao", null);
                ViewBag.ordem_servico = new SelectList(oCombo.SimNao(), "codigo", "descricao", null);
                ViewBag.foto_input = new SelectList(oCombo.SimNao(), "codigo", "descricao", null);
                ViewBag.ordem_servico = new SelectList(oCombo.SimNao(), "codigo", "descricao", null);
                ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", null);

                List<ChecklistItem> checklistItems = new List<ChecklistItem>();
                ChecklistItem checklistItem = new ChecklistItem();
                checklistItem.codigo = 0;
                checklistItem.excluido = 1;
                checklistItems.Add(checklistItem);

                return View(checklistItems);
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult ChecklistInsert(int unidade_input, int tipo_checklist_input, string descricao_header_input, List<ChecklistItem> checklistItems)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                long codigo = 0;

                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                iCodigoUnidade: unidade_input,
                                                iCodigoTipoChecklist: tipo_checklist_input,
                                                sDescricao: descricao_header_input.ToUpper(),
                                                lCodigo: ref codigo);

                if (codigo > 0)
                {
                    if (checklistItems != null)
                    {
                        foreach (ChecklistItem item in checklistItems)
                        {

                            if (item.excluido == 0)
                            {
                                //Insere Registro no Banco de Dados
                                oCadastroBasico.InsertChecklistItem(lCodigoChecklist: codigo,
                                                                    iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    sChecklist: item.checklist,
                                                                    iCodigoTipoItemChecklist: item.codigo_tipo_item_checklist,
                                                                    sGrupo: item.grupo,
                                                                    iPesoGrupo: item.peso_grupo,
                                                                    sSubgrupo: item.subgrupo,
                                                                    iPesoSubgrupo: item.peso_subgrupo,
                                                                    sDescricao: item.descricao,
                                                                    bAllowPicture: item.allow_picture,
                                                                    dValorMinimo: item.valor_minimo,
                                                                    dValorMaximo: item.valor_maximo,
                                                                    sUnidadeMedida: item.unidade_medida,
                                                                    iTempoEstimado: item.tempo_estimado,
                                                                    bAuditado: item.auditado,
                                                                    bOrdemServico: item.ordem_servico,
                                                                    iCodigoPeriodicidade: item.codigo_periodicidade,
                                                                    iIntervalo: item.intervalo,
                                                                    iPeso: item.peso,
                                                                    iCodigoDepartamento: item.codigo_departamento);
                            }

                        }

                    }
                }

                return RedirectToAction("ChecklistInsert");
            }
        }

        // GET: /EDIT
        public ActionResult ChecklistEdit(long codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ChecklistHeader checklistHeader = null;

                oCadastroBasico.InfoChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                lCodigo: codigo,
                                                oChecklistHeader: ref checklistHeader);

                if (checklistHeader == null)
                {
                    return HttpNotFound();
                }

                ViewBag.unidade_input = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                        bCadastro: false), "codigo", "descricao", checklistHeader.codigo_unidade);
                ViewBag.tipo_checklist_input = new SelectList(oCombo.TipoChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", checklistHeader.codigo_tipo_checklist);
                ViewBag.tipo_checklist_item_input = new SelectList(oCombo.TipoItemChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", null);
                ViewBag.periodicidade_input = new SelectList(oCombo.Periodicidade(bChecklist: false), "codigo", "descricao", null);
                ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", null);
                ViewBag.ordem_servico = new SelectList(oCombo.SimNao(), "codigo", "descricao", null);
                ViewBag.auditado_input = new SelectList(oCombo.SimNao(), "codigo", "descricao", null);
                ViewBag.foto_input = new SelectList(oCombo.SimNao(), "codigo", "descricao", null);
                ViewBag.checklist = checklistHeader;

                return View(oCadastroBasico.IndexChecklistItem(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                lCodigoChecklist: codigo));
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChecklistEdit(int tipo_checklist_input, string descricao_header_input, long codigo, List<ChecklistItem> checklistItems)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                iCodigoTipoChecklist: tipo_checklist_input,
                                                sDescricao: descricao_header_input.ToUpper(),
                                                lCodigo: codigo);

                if (codigo > 0)
                {
                    if (checklistItems != null)
                    {
                        foreach (ChecklistItem item in checklistItems)
                        {

                            if (item.excluido == 0)
                            {
                                //Insere Registro no Banco de Dados
                                oCadastroBasico.InsertChecklistItem(lCodigoChecklist: codigo,
                                                                    iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    sChecklist: item.checklist,
                                                                    iCodigoTipoItemChecklist: item.codigo_tipo_item_checklist,
                                                                    sGrupo: item.grupo,
                                                                    iPesoGrupo: item.peso_grupo,
                                                                    sSubgrupo: item.subgrupo,
                                                                    iPesoSubgrupo: item.peso_subgrupo,
                                                                    sDescricao: item.descricao,
                                                                    bAllowPicture: item.allow_picture,
                                                                    dValorMinimo: item.valor_minimo,
                                                                    dValorMaximo: item.valor_maximo,
                                                                    sUnidadeMedida: item.unidade_medida,
                                                                    iTempoEstimado: item.tempo_estimado,
                                                                    bAuditado: item.auditado,
                                                                    bOrdemServico: item.ordem_servico,
                                                                    iCodigoPeriodicidade: item.codigo_periodicidade,
                                                                    iIntervalo: item.intervalo,
                                                                    iPeso:item.peso,
                                                                    item.codigo_departamento);
                            }

                        }

                    }
                }

            //Redireciona para Index
            return RedirectToAction("ChecklistIndex");
            }
        }

        // GET: /DELETE
        public ActionResult ChecklistDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ChecklistHeader checklistHeader = null;

                oCadastroBasico.InfoChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                lCodigo: codigo,
                                                oChecklistHeader: ref checklistHeader);

                if (checklistHeader == null)
                {
                    return HttpNotFound();
                }

                ViewBag.checklist = checklistHeader;
                ViewBag.erro = erro;

                return View(oCadastroBasico.IndexChecklistItem(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                lCodigoChecklist: codigo));
                
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChecklistDelete(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigo: codigo);
                    //Redireciona para Index
                    return RedirectToAction("ChecklistIndex");
                }
                catch
                {
                    return ChecklistDelete(codigo: codigo,
                                            erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA FUNÇÃO
        public JsonResult ValidaChecklist(string descricao, int codigo)
        {

            return Json(oCadastroBasico.ValidaChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        sDescricao: descricao,
                                                        iCodigo: codigo));

        }

        // GET: /Download Excel
        [HttpGet]
        public virtual ActionResult DownloadExcel(int codigo_tipo_checklist, long codigo = -1)
        {

            string nome_relatorio = "";
            string path = Server.MapPath("~/Files");

            switch (codigo_tipo_checklist)
            {
                case 1: nome_relatorio = "PREVENTIVA.xlsx"; break;
                case 2: nome_relatorio = "ROTINA.xlsx"; break;
                case 3: nome_relatorio = "PMOC.xlsx"; break;
                case 4: nome_relatorio = "UH.xlsx"; break;
                case 5: nome_relatorio = "AUDITORIA.xlsx"; break;
                case 6: nome_relatorio = "QUALIDADE.xlsx"; break;
                case 8: nome_relatorio = "TAREFA.xlsx"; break;
                case 10: nome_relatorio = "GOVERNANCA.xlsx"; break;
            }

            string arquivo = System.IO.Path.Combine(path, nome_relatorio);
            string arquivoNew = System.IO.Path.Combine(path, String.Concat("CHECKLIST_PREENCHIDO_", DateTime.Now.ToString("ddMMyy_hhmmsss"), ".xlsx"));

            System.IO.File.Copy(arquivo, arquivoNew, true);

            oExcel.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                             lCodigoChecklist: codigo,
                             iCodigoTipoChecklist: codigo_tipo_checklist,
                             sFile: arquivo,
                             sPath: path,
                             sFileName: arquivoNew);

            return File(arquivoNew, "application/vnd.ms-excel", arquivoNew);
        }

        [HttpPost]
        public JsonResult UploadChecklist(int tipo_checklist)
        {
            string sFileName = "";
            string sPath = Server.MapPath(Path.Combine("~/Files/Upload", Session["empresa"].ToString()));
            if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);

            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase oHttpPostedFileBase = Request.Files[i];
                sFileName = oHttpPostedFileBase.FileName;
                if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                oHttpPostedFileBase.SaveAs(Path.Combine(sPath, sFileName));
            }
            try
            {
                return Json(oExcel.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoTipoChecklist: tipo_checklist,
                                                sFile: Path.Combine(sPath, sFileName)));
            }
            catch(Exception ex)
            {
                return Json(ex.Message);
            }                
        }

        #endregion

        #region ::: DEDETIZAÇÃO :::

        // GET: INDEX
        public ActionResult Dedetizacao()
        {
            if (Session["language"] == null || User.Identity.GetUserName() == "")
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_dedetizacao",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.codigo_unidade = Session["codigo_unidade"].ToString();
                ViewBag.username = User.Identity.GetUserName();

                Dedetizacao dedetizacao = new Dedetizacao();
                dedetizacao = oCadastroBasico.InfoDedetizacao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                              iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()));


                ViewBag.dedetizacao = dedetizacao;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                               iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                               bCadastro: false), "codigo", "descricao", dedetizacao.codigo_unidade);
                ViewBag.tipo_servico = new SelectList(oCombo.TipoServico(), "codigo", "descricao", dedetizacao.codigo_tipo_servico);

                return View(oCadastroBasico.DedetizacaoApartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                   iCodigoUnidade: dedetizacao.codigo_unidade));
            }
        }

        // GET: POST
        [HttpPost]
        public ActionResult DedetizacaoIndex(int unidade = -1)
        {
            if (Session["language"] == null || User.Identity.GetUserName() == "")
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_dedetizacao",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.codigo_unidade = Session["codigo_unidade"].ToString();
                ViewBag.username = User.Identity.GetUserName();
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                               iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                               bCadastro: false), "codigo", "descricao", unidade);

                return View();
            }
        }

        //JSON: /DEDETIZAÇÃO
        public JsonResult LoadDedetizacaoApartamento(int unidade = -1)
        {
            return Json(oCadastroBasico.DedetizacaoApartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                               iCodigoUnidade: unidade));

        }

        //JSON: /DEDETIZAÇÃO INFO
        public JsonResult LoadDedetizacaoInfo(int unidade = -1)
        {
            return Json(oCadastroBasico.InfoDedetizacao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUnidade: unidade));

        }

        // POST
        [HttpPost]
        public ActionResult Dedetizacao(int unidade, int periodicidade, int alerta, int tipo_servico, string data_inicio, List<DedetizacaoApartamento> apartamentos)
        {
            if (Session["language"] == null || User.Identity.GetUserName() == "")
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                int codigo = 0;

                oCadastroBasico.InsertDedetizacao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                  iCodigoUnidade: unidade,
                                                  iPeriodicidade: periodicidade,
                                                  iAlerta: alerta,
                                                  iCodigoTipoServico: tipo_servico,
                                                  sDataInicio: data_inicio,
                                                  iCodigo: ref codigo);

                foreach(DedetizacaoApartamento apartamento in apartamentos)
                {
                    if (apartamento.selected)
                    {
                        oCadastroBasico.InsertDedetizacaoApartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                     iCodigoUnidade: unidade,
                                                                     iCodigo: codigo,
                                                                     lCodigoApartamento: apartamento.codigo_apartamento);
                    }

                }

                return RedirectToAction("Dedetizacao");
            }
        }

        #endregion

        #region ::: DEPARTAMENTO :::

        // GET: INDEX
        public ActionResult DepartamentoIndex()
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {

                    //Váriaveis
                    bool editar = false;
                    bool inserir = false;
                    bool excluir = false;
                    bool administrador = false;

                    oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        sFormulario: "cad_departamento",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;

                    return View(oCadastroBasico.IndexDepartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())));
            }
            }

        // GET: INSERT
        public ActionResult DepartamentoInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult DepartamentoInsert(string descricao, string observacao, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertDepartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    sDescricao: descricao,
                                                    bAtivo: ativo);

                //Redireciona para Insert
                return RedirectToAction("DepartamentoInsert");
            }
        }

        // GET: /EDIT
        public ActionResult DepartamentoEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Departamento departamento = null;

                oCadastroBasico.InfoDepartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigo: codigo,
                                            oDepartamento: ref departamento);

                if (departamento == null)
                {
                    return HttpNotFound();
                }
                    
                return View(departamento);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DepartamentoEdit(string descricao, string observacao, int codigo, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateDepartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    sDescricao: descricao,
                                                    bAtivo: ativo,
                                                    iCodigo: codigo);

                //Redireciona para Index
                return RedirectToAction("DepartamentoIndex");
            }
        }

        // GET: /DELETE
        public ActionResult DepartamentoDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Departamento departamento = null;

                oCadastroBasico.InfoDepartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigo: codigo,
                                            oDepartamento: ref departamento);

                if (departamento == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(departamento);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DepartamentoDelete([Bind(Include = "codigo")] Departamento departamento)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteDepartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigo: departamento.codigo);

                    //Redireciona para Index
                    return RedirectToAction("DepartamentoIndex");
                }
                catch
                {
                    return DepartamentoDelete(codigo: departamento.codigo,
                                                erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA FUNÇÃO
        public JsonResult ValidaDepartamento(string descricao, int codigo)
        {

            return Json(oCadastroBasico.ValidaDepartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        sDescricao: descricao,
                                                        iCodigo: codigo));

        }

        #endregion

        #region ::: DEPARTAMENTO GESTOR :::

        // GET: INDEX
        public ActionResult DepartamentoGestorIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl }); // System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action") });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_departamento_gestor",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                return View(oCadastroBasico.IndexDepartamentoGestor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())));
            }
        }

        // GET: INSERT
        public ActionResult DepartamentoGestorInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", null);
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.usuario = new SelectList(oCombo.Usuario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult DepartamentoGestorInsert(int departamento, int unidade, int usuario)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertDepartamentoGestor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoDepartamento: departamento,
                                                            iCodigoUnidade: unidade,
                                                            iCodigoUsuario: usuario);

                return RedirectToAction("DepartamentoGestorInsert");
            }
        }

        // GET: /EDIT
        public ActionResult DepartamentoGestorEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                DepartamentoGestor DepartamentoGestor = null;

                oCadastroBasico.InfoDepartamentoGestor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigo: codigo,
                                                        oDepartamentoGestor: ref DepartamentoGestor);

                if (DepartamentoGestor == null)
                {
                    return HttpNotFound();
                }

                ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", DepartamentoGestor.codigo_departamento);
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", DepartamentoGestor.codigo_unidade);
                ViewBag.usuario = new SelectList(oCombo.Usuario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: DepartamentoGestor.codigo_unidade), "codigo", "descricao", DepartamentoGestor.codigo_usuario);

            return View(DepartamentoGestor);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DepartamentoGestorEdit(int departamento, int unidade, int usuario, int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateDepartamentoGestor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoDepartamento: departamento,
                                                            iCodigoUnidade: unidade,
                                                            iCodigoUsuario: usuario,
                                                            iCodigo: codigo);

                //Redireciona para Index
                return RedirectToAction("DepartamentoGestorIndex");
            }
        }

        // GET: /DELETE
        public ActionResult DepartamentoGestorDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                DepartamentoGestor DepartamentoGestor = null;

                oCadastroBasico.InfoDepartamentoGestor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigo: codigo,
                                                        oDepartamentoGestor: ref DepartamentoGestor);

                if (DepartamentoGestor == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(DepartamentoGestor);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DepartamentoGestorDelete([Bind(Include = "codigo")] DepartamentoGestor DepartamentoGestor)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteDepartamentoGestor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigo: DepartamentoGestor.codigo);

                    //Redireciona para Index
                    return RedirectToAction("DepartamentoGestorIndex");
                }
                catch
                {
                    return DepartamentoGestorDelete(codigo: DepartamentoGestor.codigo,
                                            erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA FUNÇÃO
        public JsonResult ValidaDepartamentoGestor(int departamento, int unidade, int usuario, int codigo)
        {

            return Json(oCadastroBasico.ValidaDepartamentoGestor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoDepartamento: departamento,
                                                                    iCodigoUsuario: usuario,
                                                                    iCodigo: codigo));

        }

        #endregion

        #region ::: EQUIPAMENTO :::

        // GET: INDEX
        public ActionResult EquipamentoIndex()
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    
                    //Váriaveis
                    bool editar = false;
                    bool inserir = false;
                    bool excluir = false;
                    bool administrador = false;
                    bool imprimir = false;

                    oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        sFormulario: "cad_equipamento",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador,
                                        bImprimir: ref imprimir);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;
                    ViewBag.imprimir = imprimir;


                    ViewBag.tag = "";
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                    ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                    ViewBag.apartamento = new SelectList(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                    ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", null);
                    ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", 1);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;

                    return View(oCadastroBasico.IndexEquipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                    sTAG: "",
                                                                    iCodigoSetor: 0,
                                                                    lCodigoApartamento:0,
                                                                    iCodigoDepartamento:0,
                                                                    iAtivo:1));
                }
            }  

        // POST: INDEX
        [HttpPost]
        public ActionResult EquipamentoIndex(string tag, int unidade = -1, int setor = -1, int apartamento = -1, int departamento = -1, int ativo = -1)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {

                    //Váriaveis
                    bool editar = false;
                    bool inserir = false;
                    bool excluir = false;
                    bool administrador = false;
                    bool imprimir = false;

                    oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        sFormulario: "cad_equipamento",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador,
                                        bImprimir: ref imprimir);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;
                    ViewBag.imprimir = imprimir;


                    ViewBag.tag = tag;
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: true), "codigo", "descricao", unidade);
                    ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: unidade), "codigo", "descricao", setor);
                    ViewBag.apartamento = new SelectList(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: unidade), "codigo", "descricao", apartamento);
                    ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", departamento);
                    ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", ativo);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;

                    return View(oCadastroBasico.IndexEquipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    iCodigoUnidade: unidade,
                                                                    sTAG: tag,
                                                                    iCodigoSetor: setor,
                                                                    lCodigoApartamento: apartamento,
                                                                    iCodigoDepartamento: departamento,
                                                                    iAtivo: ativo));
                }
            }
        
        // GET: INSERT
        public ActionResult EquipamentoInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.apartamento = new SelectList(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", null);
                ViewBag.familia = new SelectList(oCombo.FamiliaEquipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.laudo = new SelectList(oCombo.Programada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                    iCodigoTipoOrdemServico: 6), "codigo", "descricao", null);
                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult EquipamentoInsert(int unidade, string descricao, int departamento, int setor, string fabricante, string endereco_fabricante, string contato_fabricante, string modelo, string numero_fabricacao, string ano_fabricacao, string caracteristicas, string descricao_operacao, string instrucao_utilizacao, string procedimento_emergencia, string treinamento_operador, string condicao_seguranca, string indicacao_conclusiva, HttpPostedFileBase arquivo, string[] laudo, string tag = "", bool ativo = false, bool aeb = false, int apartamento = -1, int familia = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                string sPath = "";
                string sFileName = "";

                if (arquivo != null)
                {
                    sPath = Server.MapPath(Path.Combine("~/Content/arq/Equipamentos", Session["empresa"].ToString()));
                    sFileName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(arquivo.FileName);
                    if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                    if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                    arquivo.SaveAs(Path.Combine(sPath, sFileName));
                }

                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertEquipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: unidade,
                                                    iCodigoFamiliaEquipamento: familia,
                                                    iCodigoDepartamento: departamento,
                                                    sTag: tag,
                                                    sDescricao: descricao,
                                                    iCodigoSetor: setor,
                                                    iCodigoApartamento: apartamento,
                                                    sFabricante: fabricante,
                                                    sEnderecoFabricante: endereco_fabricante,
                                                    sContatoFabricante: contato_fabricante,
                                                    sModelo: modelo,
                                                    sNumeroFabricacao: numero_fabricacao,
                                                    sAnoFabricacao: ano_fabricacao,
                                                    sCaracteristicas: caracteristicas,
                                                    sProgramada: (laudo == null)? "": string.Join(",", laudo),
                                                    sDescricaoOperacao: descricao_operacao,
                                                    sInstrucaoUtilizacao: instrucao_utilizacao,
                                                    sProcedimentoEmergencia: procedimento_emergencia,
                                                    sTreinamentoOperador: treinamento_operador,
                                                    sCondicaoSeguranca: condicao_seguranca,
                                                    sIndicacaoConclusiva: indicacao_conclusiva,
                                                    sPathArquivo: (arquivo != null)? Path.Combine(sPath, sFileName): "",
                                                    sArquivo: (arquivo != null) ? arquivo.FileName : "",
                                                    bAtivo: ativo);

                return RedirectToAction("EquipamentoInsert");
            }
        }

        // GET: /EDIT
        public ActionResult EquipamentoEdit(long codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Equipamento equipamento = null;

                oCadastroBasico.InfoEquipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                lCodigo: codigo,
                                                oEquipamento: ref equipamento);

                if (equipamento == null)
                {
                    return HttpNotFound();
                }

                ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: equipamento.codigo_unidade), "codigo", "descricao", equipamento.codigo_setor);
                ViewBag.apartamento = new SelectList(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: equipamento.codigo_unidade,
                                                                        iCodigoSetor: equipamento.codigo_setor), "codigo", "descricao", equipamento.codigo_apartamento);
                ViewBag.laudo = new SelectList(oCombo.Programada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: equipamento.codigo_unidade,
                                                                    iCodigoTipoOrdemServico: 6), "codigo", "descricao", null);
                ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", equipamento.codigo_departamento);
                ViewBag.familia = new SelectList(oCombo.FamiliaEquipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: equipamento.codigo_unidade), "codigo", "descricao", equipamento.codigo_familia_equipamento);
                ViewBag.programada = string.Concat("[", equipamento.programada, "]");

                return View(equipamento);
            }
        }   

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EquipamentoEdit(int codigo_unidade, string descricao, int setor, string fabricante, string endereco_fabricante, string contato_fabricante, string modelo, string numero_fabricacao, string ano_fabricacao, string caracteristicas, long codigo, HttpPostedFileBase arquivo, string change_arquivo, string descricao_operacao, string instrucao_utilizacao, string procedimento_emergencia, string treinamento_operador, string condicao_seguranca, string indicacao_conclusiva, string[] laudo, string tag = "", bool ativo = false, bool aeb = false, int apartamento = -1, int familia = -1, int departamento = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
               
                string sPath = "";
                string sFileName = "";

                if (change_arquivo == "change")
                {
                    if (arquivo != null)
                    {
                        sPath = Server.MapPath(Path.Combine("~/Content/arq/Equipamentos", Session["empresa"].ToString()));
                        sFileName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(arquivo.FileName);
                        if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                        if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                        arquivo.SaveAs(Path.Combine(sPath, sFileName));
                    }

                }

                //Atualiza Registro no Banco de Dados
                oCadastroBasico.UpdateEquipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    iCodigoFamiliaEquipamento: familia,
                                                    iCodigoDepartamento: departamento,
                                                    sTag: tag,
                                                    sDescricao: descricao,
                                                    iCodigoSetor: setor,
                                                    iCodigoApartamento: apartamento,
                                                    sFabricante: fabricante,
                                                    sEnderecoFabricante: endereco_fabricante,
                                                    sContatoFabricante: contato_fabricante,
                                                    sModelo: modelo,
                                                    sNumeroFabricacao: numero_fabricacao,
                                                    sAnoFabricacao: ano_fabricacao,
                                                    sCaracteristicas: caracteristicas,
                                                    sProgramada: (laudo == null) ? "" : string.Join(",", laudo),
                                                    sDescricaoOperacao: descricao_operacao,
                                                    sInstrucaoUtilizacao: instrucao_utilizacao,
                                                    sProcedimentoEmergencia: procedimento_emergencia,
                                                    sTreinamentoOperador: treinamento_operador,
                                                    sCondicaoSeguranca: condicao_seguranca,
                                                    sIndicacaoConclusiva: indicacao_conclusiva,
                                                    sPathArquivo: (arquivo != null) ? Path.Combine(sPath, sFileName) : "",
                                                    sArquivo: (arquivo != null) ? arquivo.FileName : "",
                                                    bAtivo: ativo,
                                                    lCodigo: codigo);


                //Redireciona para Index
                return RedirectToAction("EquipamentoIndex");
            }
        }

        // GET: /DELETE
        public ActionResult EquipamentoDelete(long codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Equipamento equipamento = null;

                oCadastroBasico.InfoEquipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            lCodigo: codigo,
                                            oEquipamento: ref equipamento);

                if (equipamento == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(equipamento);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EquipamentoDelete([Bind(Include = "codigo")] Equipamento equipamento)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Exclui Registro no Banco de Dados
                    oCadastroBasico.DeleteEquipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        lCodigo: equipamento.codigo);

                    //Redireciona para Index
                    return RedirectToAction("EquipamentoIndex");
                }
                catch
                {
                    return EquipamentoDelete(codigo: equipamento.codigo,
                                                erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA EQUIPAMENTO
        public JsonResult ValidaEquipamento(int unidade, string tag, int codigo)
        {

            return Json(oCadastroBasico.ValidaEquipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            sTag: tag,
                                                            iCodigo: codigo));

        }

        // POST: /PRINT TAG EQUIPAMENTO
        public ActionResult PrintEtiquetaEquipamento(string codigo, string report)
        {

            ReportDocument oReportDocument = new ReportDocument();
            TableLogOnInfo oTableLogOnInfo = new TableLogOnInfo();
            ConnectionInfo oConnectionInfo = new ConnectionInfo();
            Database oCrDatabase;
            Tables oCrTables;

            oConnectionInfo.ServerName = ConfigurationManager.AppSettings["data_source"].ToString();
            oConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["initial_catalog"].ToString();
            oConnectionInfo.UserID = ConfigurationManager.AppSettings["user_id"].ToString();
            oConnectionInfo.Password = ConfigurationManager.AppSettings["password"].ToString();
            oConnectionInfo.Type = ConnectionInfoType.SQL;
            oConnectionInfo.IntegratedSecurity = false;

            if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Reports"), report + Session["empresa"].ToString() + ".rpt")))
            {
                report = report + Session["empresa"].ToString();
            }
                    
            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), report + ".rpt"));

            oCrDatabase = oReportDocument.Database;
            oCrTables = oCrDatabase.Tables;

            foreach (Table crTable in oCrTables)
            {
                oTableLogOnInfo = crTable.LogOnInfo;
                oTableLogOnInfo.ConnectionInfo = oConnectionInfo;
                crTable.ApplyLogOnInfo(oTableLogOnInfo);
            }

            oReportDocument.SetParameterValue("@codigo", codigo);
            oReportDocument.SetParameterValue("@codigo_empresa", Convert.ToInt32(Session["empresa"].ToString()));

            oReportDocument.SetDatabaseLogon(ConfigurationManager.AppSettings.GetValues("user_id")[0],
                                                ConfigurationManager.AppSettings.GetValues("password")[0],
                                                ConfigurationManager.AppSettings.GetValues("data_source")[0],
                                                ConfigurationManager.AppSettings.GetValues("initial_catalog")[0]);

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=ETIQUETA.pdf");
            return File(stream, "application/pdf;");
        }

        // POST: /PRINT TAG EQUIPAMENTO
        public ActionResult PrintProntuarioEquipamento(string codigo, int unidade, string report)
        {

            ReportDocument oReportDocument = new ReportDocument();
            TableLogOnInfo oTableLogOnInfo = new TableLogOnInfo();
            ConnectionInfo oConnectionInfo = new ConnectionInfo();
            Database oCrDatabase;
            Tables oCrTables;

            oConnectionInfo.ServerName = ConfigurationManager.AppSettings["data_source"].ToString();
            oConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["initial_catalog"].ToString();
            oConnectionInfo.UserID = ConfigurationManager.AppSettings["user_id"].ToString();
            oConnectionInfo.Password = ConfigurationManager.AppSettings["password"].ToString();
            oConnectionInfo.Type = ConnectionInfoType.SQL;
            oConnectionInfo.IntegratedSecurity = false;

            if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Reports"), report + Session["empresa"].ToString() + ".rpt")))
            {
                report = report + Session["empresa"].ToString();
            }

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), report + ".rpt"));

            oCrDatabase = oReportDocument.Database;
            oCrTables = oCrDatabase.Tables;

            foreach (Table crTable in oCrTables)
            {
                oTableLogOnInfo = crTable.LogOnInfo;
                oTableLogOnInfo.ConnectionInfo = oConnectionInfo;
                crTable.ApplyLogOnInfo(oTableLogOnInfo);
            }

            oReportDocument.SetParameterValue("@codigo", codigo);
            oReportDocument.SetParameterValue("@codigo_empresa", Convert.ToInt32(Session["empresa"].ToString()));
            oReportDocument.SetParameterValue("unidade", Session["unidade"].ToString());
            oReportDocument.SetParameterValue("usuario", Session["nome"].ToString());
            oReportDocument.SetParameterValue("@codigo_unidade", unidade);

            oReportDocument.SetDatabaseLogon(ConfigurationManager.AppSettings.GetValues("user_id")[0],
                                                ConfigurationManager.AppSettings.GetValues("password")[0],
                                                ConfigurationManager.AppSettings.GetValues("data_source")[0],
                                                ConfigurationManager.AppSettings.GetValues("initial_catalog")[0]);

        Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
        stream.Seek(0, SeekOrigin.Begin);
        Response.AppendHeader("Content-Length", stream.Length.ToString());
        Response.AppendHeader("Content-Disposition", "inline; filename=" + report + ".pdf");
        return File(stream, "application/pdf;");
    }

        #endregion

        #region ::: ENXOVAL :::

        // GET: INDEX
        public ActionResult EnxovalIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_enxoval",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                               iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                               bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", 1);

                return View();
            }
        }

        // POST: INDEX
        [HttpPost]
        public ActionResult EnxovalIndex(int unidade, string descricao, int ativo = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_enxoval",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                               iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                               bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", ativo);
                ViewBag.descricao = descricao;

                return View();
            }
        }

        public JsonResult LoadEnxoval(int unidade, string descricao, int ativo = -1)
        {
            return Json(oCadastroBasico.IndexEnxoval(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                     iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                     iCodigoUnidade: unidade,
                                                     sDescricao: descricao,
                                                     iAtivo: ativo));
        }

        // GET: INSERT
        public ActionResult EnxovalInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult EnxovalInsert(int unidade, string descricao, string observacao, float peso = 0, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertEnxoval(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                iCodigoUnidade: unidade,
                                                sDescricao: descricao,
                                                dPeso: peso,
                                                bAtivo: ativo);

                return RedirectToAction("EnxovalInsert");
            }
        }

        // GET: /EDIT
        public ActionResult EnxovalEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Enxoval categoria = null;

                oCadastroBasico.InfoEnxoval(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigo: codigo,
                                            oEnxoval: ref categoria);

                if (categoria == null)
                {
                    return HttpNotFound();
                }

                return View(categoria);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EnxovalEdit(int codigo_unidade, string descricao, string observacao, int codigo, float peso = 0, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateEnxoval(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                iCodigoUnidade: codigo_unidade,
                                                sDescricao: descricao,
                                                dPeso: peso,
                                                bAtivo: ativo,
                                                iCodigo: codigo);

                //Redireciona para Index
                return RedirectToAction("EnxovalIndex");
            }
        }

        // GET: /DELETE
        public ActionResult EnxovalDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Enxoval categoria = null;

                oCadastroBasico.InfoEnxoval(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigo: codigo,
                                            oEnxoval: ref categoria);

                if (categoria == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(categoria);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EnxovalDelete([Bind(Include = "codigo")] Enxoval enxoval)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteEnxoval(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigo: enxoval.codigo);

                    //Redireciona para Index
                    return RedirectToAction("EnxovalIndex");
                }
                catch
                {
                    return EnxovalDelete(codigo: enxoval.codigo,
                                            erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA
        public JsonResult ValidaEnxoval(int unidade, string descricao, int codigo)
        {

            return Json(oCadastroBasico.ValidaEnxoval(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUnidade: unidade,
                                                        sDescricao: descricao,
                                                        iCodigo: codigo));

        }

        #endregion

        #region ::: FAMÍLIA EQUIPAMENTO :::

        // GET: INDEX
        public ActionResult FamiliaEquipamentoIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_familia_equipamento",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                return View(oCadastroBasico.IndexFamiliaEquipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())));
            }
        }

        // GET: INSERT
        public ActionResult FamiliaEquipamentoInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult FamiliaEquipamentoInsert(int unidade, string descricao, string observacao, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertFamiliaEquipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                iCodigoUnidade: unidade,
                                                sDescricao: descricao,
                                                bAtivo: ativo);

                return RedirectToAction("FamiliaEquipamentoInsert");
            }
        }

        // GET: /EDIT
        public ActionResult FamiliaEquipamentoEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                FamiliaEquipamento familia_equipamento = null;

                oCadastroBasico.InfoFamiliaEquipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigo: codigo,
                                            oFamiliaEquipamento: ref familia_equipamento);

                if (familia_equipamento == null)
                {
                    return HttpNotFound();
                }

                return View(familia_equipamento);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FamiliaEquipamentoEdit(int codigo_unidade, string descricao, string observacao, int codigo, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateFamiliaEquipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                iCodigoUnidade: codigo_unidade,
                                                sDescricao: descricao,
                                                bAtivo: ativo,
                                                iCodigo: codigo);

                //Redireciona para Index
                return RedirectToAction("FamiliaEquipamentoIndex");
            }
        }

        // GET: /DELETE
        public ActionResult FamiliaEquipamentoDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                FamiliaEquipamento familia_equipamento = null;

                oCadastroBasico.InfoFamiliaEquipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigo: codigo,
                                            oFamiliaEquipamento: ref familia_equipamento);

                if (familia_equipamento == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(familia_equipamento);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FamiliaEquipamentoDelete([Bind(Include = "codigo")] FamiliaEquipamento familia_equipamento)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteFamiliaEquipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigo: familia_equipamento.codigo);

                    //Redireciona para Index
                    return RedirectToAction("FamiliaEquipamentoIndex");
                }
                catch
                {
                    return FamiliaEquipamentoDelete(codigo: familia_equipamento.codigo,
                                        erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA FUNÇÃO
        public JsonResult ValidaFamiliaEquipamento(int unidade, string descricao, int codigo)
        {

            return Json(oCadastroBasico.ValidaFamiliaEquipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUnidade: unidade,
                                                        sDescricao: descricao,
                                                        iCodigo: codigo));

        }

        #endregion

        #region ::: FORNECEDOR :::

        // GET: INDEX
        public ActionResult FornecedorIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_fornecedor",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                        bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.categoria = new SelectList(oCombo.Categoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);

                ViewBag.uf = new SelectList(oCombo.UF(), "codigo", "descricao", null);
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", 1);

                return View(oCadastroBasico.IndexFornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())));
            }
        }

        // GET: INDEX
        [HttpPost]
        public ActionResult FornecedorIndex(string cnpj, string nome_fantasia, string uf, int ativo = -1, int categoria = -1, int unidade = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_fornecedor",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.cnpj = cnpj;
                ViewBag.nome_fantasia = nome_fantasia;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
            ViewBag.categoria = new SelectList(oCombo.Categoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", categoria);

            ViewBag.uf = new SelectList(oCombo.UF(), "codigo", "descricao", uf);
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", ativo);
                    

                return View(oCadastroBasico.IndexFornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                            sCNPJ: cnpj,
                                                            sNomeFantasia: nome_fantasia,
                                                            iCodigoCategoria: categoria,
                                                            sUF: uf,
                                                            iAtivo: ativo));
            }
        }

        // GET: /INSERT
        public ActionResult FornecedorInsert()
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                    ViewBag.uf = new SelectList(oCombo.UF(), "codigo", "descricao", null);
                    ViewBag.municipio = new SelectList(oCombo.Municipio(""), "codigo", "descricao", null);
                    ViewBag.categoria = new SelectList(oCombo.Categoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);

                return View();
                }
            }

        // POST: INSERT
        [HttpPost]
        public ActionResult FornecedorInsert(int unidade, string nome_fantasia, string razao_social, string cnpj, string inscricao_estadual, string inscricao_municipal, string cep, string uf, string municipio, string logradouro, string numero, string bairro, string complemento, string telefone, string email, bool ativo = false, int categoria = -1)
        {

            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertFornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: unidade,
                                                    sNomeFantasia: nome_fantasia,
                                                    sRazaoSocial: razao_social,
                                                    sCNPJ: cnpj,
                                                    sInscricaoEstadual: inscricao_estadual,
                                                    sInscricaoMunicipal: inscricao_municipal,
                                                    sCEP: cep,
                                                    sUF: uf,
                                                    sMunicipio: municipio,
                                                    sLogradouro: logradouro,
                                                    sNumero: numero,
                                                    sBairro: bairro,
                                                    sComplemento: complemento,
                                                    sTelefone: telefone,
                                                    sEmail: email,
                                                    iCodigoCategoria: categoria,
                                                    bAtivo: ativo);

                return RedirectToAction("FornecedorInsert");
            }
        }

        // GET: /EDIT
        public ActionResult FornecedorEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Fornecedor fornecedor = null;

                oCadastroBasico.InfoFornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigo: codigo,
                                                oFornecedor: ref fornecedor);

                if (fornecedor == null)
                {
                    return HttpNotFound();
                }

                ViewBag.uf = new SelectList(oCombo.UF(), "codigo", "descricao", fornecedor.uf);
                ViewBag.municipio = new SelectList(oCombo.Municipio(fornecedor.uf), "codigo", "descricao", fornecedor.municipio);
                ViewBag.categoria = new SelectList(oCombo.Categoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: fornecedor.codigo_unidade), "codigo", "descricao", fornecedor.codigo_categoria);

            return View(fornecedor);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FornecedorEdit(int codigo_unidade, string nome_fantasia, string razao_social, string cnpj, string inscricao_estadual, string inscricao_municipal, string cep, string uf, string municipio, string logradouro, string numero, string bairro, string complemento, string telefone, string email, int codigo, bool ativo = false, int categoria = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Altera Registro no Banco de Dados
                oCadastroBasico.UpdateFornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    sNomeFantasia: nome_fantasia,
                                                    sRazaoSocial: razao_social,
                                                    sCNPJ: cnpj,
                                                    sInscricaoEstadual: inscricao_estadual,
                                                    sInscricaoMunicipal: inscricao_municipal,
                                                    sCEP: cep,
                                                    sUF: uf,
                                                    sMunicipio: municipio,
                                                    sLogradouro: logradouro,
                                                    sNumero: numero,
                                                    sBairro: bairro,
                                                    sComplemento: complemento,
                                                    sTelefone: telefone,
                                                    sEmail: email,
                                                    iCodigoCategoria: categoria,
                                                    bAtivo: ativo,
                                                    iCodigo: codigo);

                //Redireciona para Index
                return RedirectToAction("FornecedorIndex");
            }
        }

        // GET: /DELETE
        public ActionResult FornecedorDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Fornecedor fornecedor = null;

                oCadastroBasico.InfoFornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigo: codigo,
                                                oFornecedor: ref fornecedor);

                if (fornecedor == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(fornecedor);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FornecedorDelete([Bind(Include = "codigo")] Fornecedor fornecedor)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteFornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigo: fornecedor.codigo);

                    //Redireciona para Index
                    return RedirectToAction("FornecedorIndex");
                }
                catch
                {
                    return FornecedorDelete(codigo: fornecedor.codigo,
                                            erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA
        public JsonResult ValidaFornecedor(string cnpj, int codigo, int unidade)
        {

            return Json(oCadastroBasico.ValidaFornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            iCodigo: codigo,
                                                            sCNPJ: cnpj));

        }

        #endregion

        #region ::: FUNÇÃO :::

        // GET: INDEX
        public ActionResult FuncaoIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_funcao",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                return View(oCadastroBasico.IndexFuncao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())));
        }
        }

        // GET: INSERT
        public ActionResult FuncaoInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult FuncaoInsert(int unidade, string descricao, string observacao, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertFuncao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                iCodigoUnidade: unidade,
                                                sDescricao: descricao,
                                                bAtivo: ativo);

                return RedirectToAction("FuncaoInsert");
            }
        }

        // GET: /EDIT
        public ActionResult FuncaoEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Funcao funcao = null;

                oCadastroBasico.InfoFuncao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigo: codigo,
                                            oFuncao: ref funcao);

                if (funcao == null)
                {
                    return HttpNotFound();
                }
                    
                return View(funcao);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FuncaoEdit(int codigo_unidade, string descricao, string observacao, int codigo, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateFuncao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                iCodigoUnidade: codigo_unidade,
                                                sDescricao: descricao,
                                                bAtivo: ativo,
                                                iCodigo: codigo);

                //Redireciona para Index
                return RedirectToAction("FuncaoIndex");
            }
        }

        // GET: /DELETE
        public ActionResult FuncaoDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Funcao funcao = null;

                oCadastroBasico.InfoFuncao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigo: codigo,
                                            oFuncao: ref funcao);

                if (funcao == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(funcao);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FuncaoDelete([Bind(Include = "codigo")] Funcao funcao)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteFuncao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigo: funcao.codigo);

                    //Redireciona para Index
                    return RedirectToAction("FuncaoIndex");
                }
                catch
                {
                    return FuncaoDelete(codigo: funcao.codigo,
                                        erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA FUNÇÃO
        public JsonResult ValidaFuncao(int unidade, string descricao, int codigo)
        {

            return Json(oCadastroBasico.ValidaFuncao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUnidade: unidade,
                                                        sDescricao: descricao,
                                                        iCodigo: codigo));

        }

        #endregion

        #region ::: FUNCIONÁRIO :::

        // GET: INDEX
        public ActionResult FuncionarioIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_funcionario",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                               iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                               bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.funcao = new SelectList(oCombo.Funcao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", null);
                ViewBag.tipo_funcionario = new SelectList(oCombo.TipoFuncionario(), "codigo", "descricao", null);
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", 1);

                return View();

            }
        }

        // GET: INDEX
        [HttpPost]
        public ActionResult FuncionarioIndex(string nome, int funcao = -1, int tipo_funcionario = -1, int ativo = -1, int unidade = -1, int modulo = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_funcionario",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                               iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                               bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.funcao = new SelectList(oCombo.Funcao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: unidade), "codigo", "descricao", funcao);
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", modulo);
                ViewBag.tipo_funcionario = new SelectList(oCombo.TipoFuncionario(), "codigo", "descricao", tipo_funcionario);
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", ativo);

                return View();

            }
        }

        // GET: INDEX
        [HttpPost]
        public JsonResult LoadFuncionario(string nome, int funcao = -1, int tipo_funcionario = -1, int ativo = -1, int unidade = -1, int modulo = -1)
        {
            return Json(oCadastroBasico.IndexFuncionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                         iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                         iCodigoUnidade: unidade,
                                                         iCodigoModulo: modulo,
                                                         sNome: nome,
                                                         iCodigoFuncao: funcao,
                                                         iCodigoTipoFuncionario: tipo_funcionario,
                                                         iAtivo: ativo));
        
        }

        // GET: /INSERT
        public ActionResult FuncionarioInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.funcao = new SelectList(oCombo.Funcao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.usuario = new SelectList(oCombo.Usuario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", null);
                ViewBag.tipo_funcionario = new SelectList(oCombo.TipoFuncionario(), "codigo", "descricao", null);
                    
                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult FuncionarioInsert(int unidade, string nome, string cpf, int modulo, string telefone, int tipo_funcionario, string valor_hora = "0", bool ativo = false, int usuario = -1, int funcao = -1, bool contabiliza_hora = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertFuncionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: unidade,
                                                    sNome: nome,
                                                    sCPF: cpf,
                                                    iCodigoFuncao: funcao,
                                                    iCodigoModulo: modulo,
                                                    sTelefone: telefone,
                                                    iCodigoUsuarioVinculado: usuario,
                                                    iCodigoTipoFuncionario: tipo_funcionario,
                                                    dValorHora: float.Parse(valor_hora == ""? "0": valor_hora),
                                                    bAtivo: ativo,
                                                    bContabilizaHora: contabiliza_hora);

                return RedirectToAction("FuncionarioInsert");
            }
        }

        // GET: /EDIT
        public ActionResult FuncionarioEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Funcionario funcionario = null;

                oCadastroBasico.InfoFuncionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigo: codigo,
                                                oFuncionario: ref funcionario);

                if (funcionario == null)
                {
                    return HttpNotFound();
                }

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", funcionario.codigo_unidade);
                ViewBag.funcao = new SelectList(oCombo.Funcao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                iCodigoUnidade: funcionario.codigo_unidade), "codigo", "descricao", funcionario.codigo_funcao);
                ViewBag.usuario = new SelectList(oCombo.Usuario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: funcionario.codigo_unidade), "codigo", "descricao", funcionario.codigo_usuario);
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", funcionario.codigo_modulo);
                ViewBag.tipo_funcionario = new SelectList(oCombo.TipoFuncionario(), "codigo", "descricao", funcionario.codigo_tipo_funcionario);

                return View(funcionario);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FuncionarioEdit(int unidade, string nome, string cpf, int modulo, string telefone, int tipo_funcionario, int codigo, string valor_hora = "0", bool ativo = false, int usuario = -1, int funcao = -1, bool contabiliza_hora = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateFuncionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: unidade,
                                                    sNome: nome,
                                                    sCPF: cpf,
                                                    iCodigoFuncao: funcao,
                                                    iCodigoModulo: modulo,
                                                    sTelefone: telefone,
                                                    iCodigoUsuarioVinculado: usuario,
                                                    iCodigoTipoFuncionario: tipo_funcionario,
                                                    bAtivo: ativo,
                                                    dValorHora: float.Parse(valor_hora),
                                                    bContabilizaHora: contabiliza_hora,
                                                    iCodigo: codigo);

                //Redireciona para Index
                return RedirectToAction("FuncionarioIndex");
            }
        }

        // GET: /DELETE
        public ActionResult FuncionarioDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Funcionario funcionario = null;

                oCadastroBasico.InfoFuncionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigo: codigo,
                                                oFuncionario: ref funcionario);

                if (funcionario == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(funcionario);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FuncionarioDelete([Bind(Include = "codigo")] Funcionario funcionario)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteFuncionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigo: funcionario.codigo);

                    //Redireciona para Index
                    return RedirectToAction("FuncionarioIndex");
                }
                catch
                {
                    return FuncionarioDelete(codigo: funcionario.codigo,
                                                erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA
        public JsonResult ValidaFuncionario(string cpf, int codigo, int unidade)
        {

            return Json(oCadastroBasico.ValidaFuncionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            iCodigo: codigo,
                                                            sCPF: cpf));

        }

        #endregion

        #region ::: GRUPO - CHECKLIST :::

        // GET: INDEX
        public ActionResult UHGrupoChecklistIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_grupo_checklist",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                return View(oCadastroBasico.IndexGrupoChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())));
        }
        }

        // GET: INSERT
        public ActionResult UHGrupoChecklistInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult UHGrupoChecklistInsert(int unidade, string codigo_grupo, string descricao, string observacao, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertGrupoChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigoUnidade: unidade,
                                                        sCodigoGrupo: codigo_grupo,
                                                        sDescricao: descricao,
                                                        bAtivo: ativo);

                return RedirectToAction("UHGrupoChecklistInsert");
            }
        }

        // GET: /EDIT
        public ActionResult UHGrupoChecklistEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                GrupoChecklist grupo = null;

                oCadastroBasico.InfoGrupoChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigo: codigo,
                                                    oGrupoChecklist: ref grupo);

                if (grupo == null)
                {
                    return HttpNotFound();
                }

                return View(grupo);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UHGrupoChecklistEdit(int codigo_unidade, string codigo_grupo, string descricao, string observacao, int codigo, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateGrupoChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigoUnidade: codigo_unidade,
                                                        sCodigoGrupo: codigo_grupo,
                                                        sDescricao: descricao,
                                                        bAtivo: ativo,
                                                        iCodigo: codigo);

                //Redireciona para Index
                return RedirectToAction("UHGrupoChecklistIndex");
            }
        }

        // GET: /DELETE
        public ActionResult UHGrupoChecklistDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                GrupoChecklist grupo = null;

                oCadastroBasico.InfoGrupoChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigo: codigo,
                                                    oGrupoChecklist: ref grupo);

                if (grupo == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(grupo);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UHGrupoChecklistDelete([Bind(Include = "codigo")] GrupoChecklist grupo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteGrupoChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            iCodigo: grupo.codigo);

                    //Redireciona para Index
                    return RedirectToAction("UHGrupoChecklistIndex");
                }
                catch
                {
                    return UHGrupoChecklistDelete(codigo: grupo.codigo,
                                                    erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA FUNÇÃO
        public JsonResult ValidaGrupoChecklist(int unidade, string codigo_grupo, int codigo)
        {

            return Json(oCadastroBasico.ValidaGrupoChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: unidade,
                                                                sCodigoGrupo: codigo_grupo,
                                                                iCodigo: codigo));

        }

        #endregion

        #region ::: GRUPO - ITEM MEDIÇÃO :::

        // GET: INDEX
        public ActionResult GrupoItemMedicaoIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_grupo_item_medicao",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                return View(oCadastroBasico.IndexGrupoItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())));
        }
        }

        // GET: INSERT
        public ActionResult GrupoItemMedicaoInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult GrupoItemMedicaoInsert(int unidade, string descricao, string observacao, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertGrupoItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigoUnidade: unidade,
                                                        sDescricao: descricao,
                                                        bAtivo: ativo);

                return RedirectToAction("GrupoItemMedicaoInsert");
            }
        }

        // GET: /EDIT
        public ActionResult GrupoItemMedicaoEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                GrupoItemMedicao grupo = null;

                oCadastroBasico.InfoGrupoItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigo: codigo,
                                                        oGrupoItemMedicao: ref grupo);

                if (grupo == null)
                {
                    return HttpNotFound();
                }

                return View(grupo);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GrupoItemMedicaoEdit(int codigo_unidade, string descricao, string observacao, int codigo, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateGrupoItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigoUnidade: codigo_unidade,
                                                        sDescricao: descricao,
                                                        bAtivo: ativo,
                                                        iCodigo: codigo);

                //Redireciona para Index
                return RedirectToAction("GrupoItemMedicaoIndex");
            }
        }

        // GET: /DELETE
        public ActionResult GrupoItemMedicaoDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                GrupoItemMedicao grupo = null;

                oCadastroBasico.InfoGrupoItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigo: codigo,
                                                        oGrupoItemMedicao: ref grupo);

                if (grupo == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(grupo);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GrupoItemMedicaoDelete([Bind(Include = "codigo")] GrupoItemMedicao grupo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteGrupoItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            iCodigo: grupo.codigo);

                    //Redireciona para Index
                    return RedirectToAction("GrupoItemMedicaoIndex");
                }
                catch
                {
                    return GrupoItemMedicaoDelete(codigo: grupo.codigo,
                                                    erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA FUNÇÃO
        public JsonResult ValidaGrupoItemMedicao(int unidade, string descricao, int codigo)
        {

            return Json(oCadastroBasico.ValidaGrupoItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: unidade,
                                                                sDescricao: descricao,
                                                                iCodigo: codigo));

        }

        #endregion

        //#region ::: GRUPO - PRODUTO :::

        //// GET: INDEX
        //public ActionResult GrupoProdutoIndex()
        //{
        //    if (Session["empresa"] == null)
        //    {
        //        return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
        //    }
        //    else
        //    {

        //        //Váriaveis
        //        bool editar = false;
        //        bool inserir = false;
        //        bool excluir = false;
        //        bool administrador = false;

        //        oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
        //                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
        //                            sFormulario: "cad_grupo_produto",
        //                            bInserir: ref inserir,
        //                            bEditar: ref editar,
        //                            bExcluir: ref excluir,
        //                            bAdministrador: ref administrador);

        //        ViewBag.inserir = inserir;
        //        ViewBag.editar = editar;
        //        ViewBag.excluir = excluir;

        //        return View(oCadastroBasico.IndexGrupoProduto(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
        //                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
        //                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())));
        //    }
        //}

        //// GET: INSERT
        //public ActionResult GrupoProdutoInsert()
        //{
        //    if (Session["empresa"] == null)
        //    {
        //        return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
        //    }
        //    else
        //    {
        //        ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
        //                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
        //                                                        bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
        //        return View();
        //    }
        //}

        //// POST: INSERT
        //[HttpPost]
        //public ActionResult GrupoProdutoInsert(int unidade, string descricao, string observacao, bool ativo = false)
        //{
        //    if (Session["empresa"] == null)
        //    {
        //        return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
        //    }
        //    else
        //    {
        //        //Insere Registro no Banco de Dados
        //        oCadastroBasico.InsertGrupoProduto(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
        //                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
        //                                            iCodigoUnidade: unidade,
        //                                            sDescricao: descricao,
        //                                            bAtivo: ativo);

        //        return RedirectToAction("GrupoProdutoInsert");
        //    }
        //}

        //// GET: /EDIT
        //public ActionResult GrupoProdutoEdit(int codigo)
        //{
        //    if (Session["empresa"] == null)
        //    {
        //        return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
        //    }
        //    else
        //    {
        //        GrupoProduto grupo = null;

        //        oCadastroBasico.InfoGrupoProduto(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
        //                                            iCodigo: codigo,
        //                                            oGrupoProduto: ref grupo);

        //        if (grupo == null)
        //        {
        //            return HttpNotFound();
        //        }

        //        return View(grupo);
        //    }
        //}

        //// POST: /EDIT
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult GrupoProdutoEdit(int codigo_unidade, string descricao, string observacao, int codigo, bool ativo = false)
        //{
        //    if (Session["empresa"] == null)
        //    {
        //        return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
        //    }
        //    else
        //    {
        //        //Insere Registro no Banco de Dados
        //        oCadastroBasico.UpdateGrupoProduto(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
        //                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
        //                                            iCodigoUnidade: codigo_unidade,
        //                                            sDescricao: descricao,
        //                                            bAtivo: ativo,
        //                                            iCodigo: codigo);

        //        //Redireciona para Index
        //        return RedirectToAction("GrupoProdutoIndex");
        //    }
        //}

        //// GET: /DELETE
        //public ActionResult GrupoProdutoDelete(int codigo, string erro = "")
        //{
        //    if (Session["empresa"] == null)
        //    {
        //        return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
        //    }
        //    else
        //    {
        //        GrupoProduto grupo = null;

        //        oCadastroBasico.InfoGrupoProduto(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
        //                                            iCodigo: codigo,
        //                                            oGrupoProduto: ref grupo);

        //        if (grupo == null)
        //        {
        //            return HttpNotFound();
        //        }

        //        ViewBag.erro = erro;
        //        return View(grupo);
        //    }
        //}

        //// POST: /DELETE
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult GrupoProdutoDelete([Bind(Include = "codigo")] GrupoProduto grupo)
        //{
        //    if (Session["empresa"] == null)
        //    {
        //        return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
        //    }
        //    else
        //    {

        //        try
        //        {
        //            //Insere Registro no Banco de Dados
        //            oCadastroBasico.DeleteGrupoProduto(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
        //                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
        //                                                iCodigo: grupo.codigo);

        //            //Redireciona para Index
        //            return RedirectToAction("GrupoProdutoIndex");
        //        }
        //        catch
        //        {
        //            return GrupoProdutoDelete(codigo: grupo.codigo,
        //                                        erro: PCM.WEB.Properties.Resources.valida_excluir);
        //        }
        //    }
        //}

        ////JSON: /VALIDA FUNÇÃO
        //public JsonResult ValidaGrupoProduto(int unidade, string descricao, int codigo)
        //{

        //    return Json(oCadastroBasico.ValidaGrupoProduto(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
        //                                                    iCodigoUnidade: unidade,
        //                                                    sDescricao: descricao,
        //                                                    iCodigo: codigo));

        //}

        //#endregion

        #region ::: ITEM - MEDIÇÃO :::

        // GET: INDEX
        public ActionResult ItemMedicaoIndex()
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {

                    //Váriaveis
                    bool editar = false;
                    bool inserir = false;
                    bool excluir = false;
                    bool administrador = false;

                    oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        sFormulario: "cad_item_medicao",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;

                    return View(oCadastroBasico.IndexItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())));
            }
            }

        // GET: INSERT
        public ActionResult ItemMedicaoInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.forma_leitura = new SelectList(oCombo.FormaLeitura(), "codigo", "descricao", null);
                ViewBag.grupo_item_medicao = new SelectList(oCombo.GrupoItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult ItemMedicaoInsert(int unidade, int grupo_item_medicao, string descricao, string observacao, string meta_consumo, int forma_leitura, string unidade_medida, int numero_digitos = 0, int numero_casas_decimais = 0, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: unidade,
                                                    iCodigoGrupoItemMedicao: grupo_item_medicao,
                                                    sDescricao: descricao,
                                                    sMetaConsumo: meta_consumo,
                                                    iCodigoFormaLeitura: forma_leitura,
                                                    iNumeroDigitos: numero_digitos,
                                                    iNumeroCasasDecimais: numero_casas_decimais,
                                                    sUnidadeMedida: unidade_medida,
                                                    bAtivo: ativo);

                return RedirectToAction("ItemMedicaoInsert");
            }
        }

        // GET: /EDIT
        public ActionResult ItemMedicaoEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ItemMedicao item_medicao = null;

                oCadastroBasico.InfoItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigo: codigo,
                                                oItemMedicao: ref item_medicao);

                if (item_medicao == null)
                {
                    return HttpNotFound();
                }

                ViewBag.forma_leitura = new SelectList(oCombo.FormaLeitura(), "codigo", "descricao", item_medicao.codigo_forma_leitura);
                ViewBag.grupo_item_medicao = new SelectList(oCombo.GrupoItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                    iCodigoUnidade: item_medicao.codigo_unidade), "codigo", "descricao", item_medicao.codigo_grupo_item_medicao);
                    
                return View(item_medicao);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ItemMedicaoEdit(int codigo_unidade, int grupo_item_medicao, string descricao, string observacao, string meta_consumo, int forma_leitura, string unidade_medida, int codigo, int numero_digitos = 0, int numero_casas_decimais = 0, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    iCodigoGrupoItemMedicao: grupo_item_medicao,
                                                    sDescricao: descricao,
                                                    sMetaConsumo: meta_consumo,
                                                    iCodigoFormaLeitura: forma_leitura,
                                                    iNumeroDigitos: numero_digitos,
                                                    iNumeroCasasDecimais: numero_casas_decimais,
                                                    sUnidadeMedida: unidade_medida,
                                                    bAtivo: ativo,
                                                    iCodigo: codigo);

                //Redireciona para Index
                return RedirectToAction("ItemMedicaoIndex");
            }
        }

        // GET: /DELETE
        public ActionResult ItemMedicaoDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ItemMedicao item_medicao = null;

                oCadastroBasico.InfoItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigo: codigo,
                                                oItemMedicao: ref item_medicao);

                if (item_medicao == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(item_medicao);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ItemMedicaoDelete([Bind(Include = "codigo")] ItemMedicao item_medicao)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigo: item_medicao.codigo);

                    //Redireciona para Index
                    return RedirectToAction("ItemMedicaoIndex");
                }
                catch
                {
                    return ItemMedicaoDelete(codigo: item_medicao.codigo,
                                                erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA FUNÇÃO
        public JsonResult ValidaItemMedicao(int unidade, string descricao, int codigo)
        {

            return Json(oCadastroBasico.ValidaItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            sDescricao: descricao,
                                                            iCodigo: codigo));

        }

        #endregion

        #region ::: ITENS GERAIS :::

        // GET: INDEX
        public ActionResult ItensGeraisIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_itens_gerais",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                return View(oCadastroBasico.IndexItensGerais(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())));
        }
        }

        // GET: INSERT
        public ActionResult ItensGeraisInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult ItensGeraisInsert(int unidade, string descricao, string observacao, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertItensGerais(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: unidade,
                                                    sDescricao: descricao,
                                                    bAtivo: ativo);

                return RedirectToAction("ItensGeraisInsert");
            }
        }

        // GET: /EDIT
        public ActionResult ItensGeraisEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ItensGerais itens_gerais = null;

                oCadastroBasico.InfoItensGerais(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigo: codigo,
                                                oItensGerais: ref itens_gerais);

                if (itens_gerais == null)
                {
                    return HttpNotFound();
                }

                return View(itens_gerais);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ItensGeraisEdit(int codigo_unidade, string descricao, string observacao, int codigo, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateItensGerais(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    sDescricao: descricao,
                                                    bAtivo: ativo,
                                                    iCodigo: codigo);

                //Redireciona para Index
                return RedirectToAction("ItensGeraisIndex");
            }
        }

        // GET: /DELETE
        public ActionResult ItensGeraisDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ItensGerais itens_gerais = null;

                oCadastroBasico.InfoItensGerais(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigo: codigo,
                                                oItensGerais: ref itens_gerais);

                if (itens_gerais == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(itens_gerais);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ItensGeraisDelete([Bind(Include = "codigo")] ItensGerais item)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteItensGerais(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigo: item.codigo);

                    //Redireciona para Index
                    return RedirectToAction("ItensGeraisIndex");
                }
                catch
                {
                    return ItensGeraisDelete(codigo: item.codigo,
                                                erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA FUNÇÃO
        public JsonResult ValidaItensGerais(int unidade, string descricao, int codigo)
        {

            return Json(oCadastroBasico.ValidaItensGerais(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            sDescricao: descricao,
                                                            iCodigo: codigo));

        }

        #endregion

        #region ::: JUSTIFICATIVA - APONTAMENTO :::

        // GET: INDEX
        public ActionResult JustificativaApontamentoIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_justificativa_apontamento",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                return View(oCadastroBasico.IndexJustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())));
            }
        }

        // GET: INSERT
        public ActionResult JustificativaApontamentoInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult JustificativaApontamentoInsert(string descricao, string observacao, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertJustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                sDescricao: descricao,
                                                                bAtivo: ativo);

                return RedirectToAction("JustificativaApontamentoInsert");
            }
        }

        // GET: /EDIT
        public ActionResult JustificativaApontamentoEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                JustificativaApontamento justificativa_apontamento = null;

                oCadastroBasico.InfoJustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigo: codigo,
                                                                oJustificativaApontamento: ref justificativa_apontamento);

                if (justificativa_apontamento == null)
                {
                    return HttpNotFound();
                }

                return View(justificativa_apontamento);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JustificativaApontamentoEdit(string descricao, string observacao, int codigo, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateJustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                sDescricao: descricao,
                                                                bAtivo: ativo,
                                                                iCodigo: codigo);

                //Redireciona para Index
                return RedirectToAction("JustificativaApontamentoIndex");
            }
        }

        // GET: /DELETE
        public ActionResult JustificativaApontamentoDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                JustificativaApontamento justificativa_apontamento = null;

                oCadastroBasico.InfoJustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigo: codigo,
                                            oJustificativaApontamento: ref justificativa_apontamento);

                if (justificativa_apontamento == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(justificativa_apontamento);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JustificativaApontamentoDelete([Bind(Include = "codigo")] JustificativaApontamento justificativa)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteJustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigo: justificativa.codigo);

                    //Redireciona para Index
                    return RedirectToAction("JustificativaApontamentoIndex");
                }
                catch
                {
                    return JustificativaApontamentoDelete(codigo: justificativa.codigo,
                                                            erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA FUNÇÃO
        public JsonResult ValidaJustificativaApontamento(string descricao, int codigo)
        {

            return Json(oCadastroBasico.ValidaJustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        sDescricao: descricao,
                                                                        iCodigo: codigo));

        }

        #endregion

        #region ::: JUSTIFICATIVA - FALTA :::

        // GET: INDEX
        public ActionResult JustificativaFaltaIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_justificativa_apontamento",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                return View(oCadastroBasico.IndexJustificativaFalta(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()), 
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())));
            }
        }

        // GET: INSERT
        public ActionResult JustificativaFaltaInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult JustificativaFaltaInsert(int unidade, string descricao, string observacao, bool justificada = false, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertJustificativaFalta(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            iCodigoUnidade: unidade,
                                                            sDescricao: descricao,
                                                            bJustificada: justificada,
                                                            bAtivo: ativo);

                return RedirectToAction("JustificativaFaltaInsert");
            }
        }

        // GET: /EDIT
        public ActionResult JustificativaFaltaEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                JustificativaFalta justificativa_apontamento = null;

                oCadastroBasico.InfoJustificativaFalta(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigo: codigo,
                                            oJustificativaFalta: ref justificativa_apontamento);

                if (justificativa_apontamento == null)
                {
                    return HttpNotFound();
                }

                return View(justificativa_apontamento);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JustificativaFaltaEdit(int codigo_unidade, string descricao, string observacao, int codigo, bool justificada = false, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateJustificativaFalta(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            iCodigoUnidade: codigo_unidade,
                                                            sDescricao: descricao,
                                                            bJustificada: justificada,
                                                            bAtivo: ativo,
                                                            iCodigo: codigo);

                //Redireciona para Index
                return RedirectToAction("JustificativaFaltaIndex");
            }
        }

        // GET: /DELETE
        public ActionResult JustificativaFaltaDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                JustificativaFalta justificativa_apontamento = null;

                oCadastroBasico.InfoJustificativaFalta(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigo: codigo,
                                            oJustificativaFalta: ref justificativa_apontamento);

                if (justificativa_apontamento == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(justificativa_apontamento);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JustificativaFaltaDelete([Bind(Include = "codigo")] JustificativaFalta justificativa)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteJustificativaFalta(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigo: justificativa.codigo);

                    //Redireciona para Index
                    return RedirectToAction("JustificativaFaltaIndex");
                }
                catch
                {
                    return JustificativaFaltaDelete(codigo: justificativa.codigo,
                                                            erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA FUNÇÃO
        public JsonResult ValidaJustificativaFalta(int unidade, string descricao, int codigo)
        {

            return Json(oCadastroBasico.ValidaJustificativaFalta(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUnidade: unidade,
                                                        sDescricao: descricao,
                                                        iCodigo: codigo));

        }

        #endregion

        #region ::: JUSTIFICATIVA - CANCELAMENTO - ORDEM SERVICO :::

        // GET: INDEX
        public ActionResult JustificativaCancelamentoOrdemServicoIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_justificativa_cancelar_ordem_servico",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                return View(oCadastroBasico.IndexJustificativaCancelamentoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                       iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())));
            }
        }

        // GET: INSERT
        public ActionResult JustificativaCancelamentoOrdemServicoInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult JustificativaCancelamentoOrdemServicoInsert(string descricao, string observacao, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertJustificativaCancelamentoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                            sDescricao: descricao,
                                                                            bAtivo: ativo);

                return RedirectToAction("JustificativaCancelamentoOrdemServicoInsert");
            }
        }

        // GET: /EDIT
        public ActionResult JustificativaCancelamentoOrdemServicoEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                JustificativaCancelamentoOrdemServico justificativa_apontamento = null;

                oCadastroBasico.InfoJustificativaCancelamentoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                          iCodigo: codigo,
                                                                          oJustificativaCancelamentoOrdemServico: ref justificativa_apontamento);

                if (justificativa_apontamento == null)
                {
                    return HttpNotFound();
                }

                return View(justificativa_apontamento);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JustificativaCancelamentoOrdemServicoEdit(string descricao, string observacao, int codigo, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateJustificativaCancelamentoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                            sDescricao: descricao,
                                                                            bAtivo: ativo,
                                                                            iCodigo: codigo);

                //Redireciona para Index
                return RedirectToAction("JustificativaCancelamentoOrdemServicoIndex");
            }
        }

        // GET: /DELETE
        public ActionResult JustificativaCancelamentoOrdemServicoDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                JustificativaCancelamentoOrdemServico justificativa_apontamento = null;

                oCadastroBasico.InfoJustificativaCancelamentoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                          iCodigo: codigo,
                                                                          oJustificativaCancelamentoOrdemServico: ref justificativa_apontamento);

                if (justificativa_apontamento == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(justificativa_apontamento);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JustificativaCancelamentoOrdemServicoDelete([Bind(Include = "codigo")] JustificativaCancelamentoOrdemServico justificativa)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteJustificativaCancelamentoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigo: justificativa.codigo);

                    //Redireciona para Index
                    return RedirectToAction("JustificativaCancelamentoOrdemServicoIndex");
                }
                catch
                {
                    return JustificativaCancelamentoOrdemServicoDelete(codigo: justificativa.codigo,
                                                            erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA FUNÇÃO
        public JsonResult ValidaJustificativaCancelamentoOrdemServico(string descricao, int codigo)
        {

            return Json(oCadastroBasico.ValidaJustificativaCancelamentoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                    sDescricao: descricao,
                                                                                    iCodigo: codigo));

        }

        #endregion

        #region ::: LAUDO :::

        // GET: INDEX
        public ActionResult LaudoIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_laudo",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                return View(oCadastroBasico.IndexProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                            iCodigoTipoOrdemServico: 6,
                                                            bRotina: false));
            }
        }

        // GET: INSERT
        public ActionResult LaudoInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_programada",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", null);
                ViewBag.categoria = new SelectList(oCombo.Categoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.equipamento = new SelectList(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.periodicidade = new SelectList(oCombo.Periodicidade(bChecklist: false), "codigo", "descricao", null);
                ViewBag.tipo_servico = new SelectList(oCombo.TipoServico(), "codigo", "descricao", null);
                    
                return View();
            }
        }

        // POST: INSERT
        [HttpPost, ValidateInput(false)]
        public ActionResult LaudoInsert(int unidade, int categoria, string setor, string descricao, int periodicidade, int intervalo, int tipo_servico, int tipo_ordem_servico, int modulo, string emailLaudo, int dias_alerta = 0, bool ativo = false, bool envia_email = false, bool exige_laudo = false, string valor_previsto = "", int quantidade_equipamento = 0)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                 iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                 iCodigoUnidade: unidade,
                                                 iCodigoModulo: modulo,
                                                 iCodigoCategoria: categoria,
                                                 iCodigoSetor: (setor == "") ? -1 : Convert.ToInt32(setor),
                                                 sDescricao: descricao,
                                                 dValorPrevisto: valor_previsto,
                                                 iQuantidadeEquipamento: quantidade_equipamento,
                                                 iCodigoPeriodicidade: periodicidade,
                                                 iIntervalo: intervalo,
                                                 iCodigoTipoServico: tipo_servico,
                                                 iCodigoTipoOrdemServico: tipo_ordem_servico,
                                                 bAtivo: ativo,
                                                 bExigeLaudo: exige_laudo,                                                     
                                                 bRotina: false,
                                                 bEnviaEmail: envia_email,
                                                 sEmail: emailLaudo,
                                                 iDiasAlerta: dias_alerta);

                return RedirectToAction("LaudoInsert");
            }
        }

        // GET: /EDIT
        public ActionResult LaudoEdit(int codigo, int codigo_unidade)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Programada programada = null;

                oCadastroBasico.InfoProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: codigo_unidade,
                                                lCodigo: codigo,
                                                oProgramada: ref programada);

                if (programada == null)
                {
                    return HttpNotFound();
                }

                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", programada.codigo_modulo);
                ViewBag.categoria = new SelectList(oCombo.Categoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: programada.codigo_unidade), "codigo", "descricao", programada.codigo_categoria);
                ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: programada.codigo_unidade), "codigo", "descricao", programada.codigo_setor);
                ViewBag.equipamento = new SelectList(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: programada.codigo_unidade), "codigo", "descricao", programada.codigo_equipamento);
                ViewBag.periodicidade = new SelectList(oCombo.Periodicidade(bChecklist: false), "codigo", "descricao", programada.codigo_periodicidade);
                ViewBag.tipo_servico = new SelectList(oCombo.TipoServico(), "codigo", "descricao", programada.codigo_tipo_servico);

                return View(programada);
            }
        }

        // POST: /EDIT
        [HttpPost, ValidateInput(false)]
        public ActionResult LaudoEdit(int codigo_unidade, int categoria, string setor, string equipamento, string descricao, int periodicidade, int intervalo, int tipo_servico, long codigo, int unidade_old, int modulo, string emailLaudo, int tipo_ordem_servico, long checklist = -1, int dias_alerta = 0, bool ativo = false, bool envia_email = false, bool exige_laudo = false, string valor_previsto = "", int quantidade_equipamento = 0)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    iCodigoCategoria: categoria,
                                                    iCodigoSetor: (setor == "") ? -1 : Convert.ToInt32(setor),
                                                    lCodigoEquipamento: (equipamento == "") ? -1 : Convert.ToInt64(equipamento),
                                                    sDescricao: descricao,
                                                    dValorPrevisto: valor_previsto,
                                                    iQuantidadeEquipamento: quantidade_equipamento,
                                                    iCodigoPeriodicidade: periodicidade,
                                                    iIntervalo: intervalo,
                                                    lCodigoChecklist: checklist,
                                                    iCodigoTipoServico: tipo_servico,
                                                    iCodigoTipoOrdemServico: tipo_ordem_servico,
                                                    bExigeLaudo: exige_laudo,
                                                    bAtivo: ativo,
                                                    iCodigoModulo: modulo,
                                                    lCodigo: codigo,
                                                    iCodigoUnidadeOld: unidade_old,
                                                    bEnviaEmail: envia_email,
                                                    sEmail: emailLaudo,
                                                    iDiasAlerta: dias_alerta);

            //Redireciona para Index
            return RedirectToAction("LaudoIndex");
            }
        }

        // GET: /DELETE
        public ActionResult LaudoDelete(long codigo, int codigo_unidade, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Programada programada = null;

                oCadastroBasico.InfoProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: codigo_unidade,
                                                lCodigo: codigo,
                                                oProgramada: ref programada);

                if (programada == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(programada);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LaudoDelete([Bind(Include = "codigo, codigo_unidade")] Programada programada)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        lCodigo: programada.codigo,
                                                        iCodigoUnidade: programada.codigo_unidade);

                    //Redireciona para Index
                    return RedirectToAction("LaudoIndex");
                }
                catch
                {
                    return LaudoDelete(codigo: programada.codigo,
                                            codigo_unidade: programada.codigo_unidade,
                                            erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        #endregion

        #region ::: PRIORIDADE :::

        // GET: INDEX
        public ActionResult PrioridadeIndex()
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {

                    //Váriaveis
                    bool editar = false;
                    bool inserir = false;
                    bool excluir = false;
                    bool administrador = false;

                    oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        sFormulario: "cad_categoria",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;

                    return View(oCadastroBasico.IndexPrioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())));
            }
            }

        // GET: INSERT
        public ActionResult PrioridadeInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                return View();
            }
        }

        // POST: INSERT
        [HttpPost, ValidateInput(false)]
        public ActionResult PrioridadeInsert(int unidade, string descricao, bool envia_email = false, string email = "", bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertPrioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: unidade,
                                                    sDescricao: descricao,
                                                    bEnviaEmail: envia_email,
                                                    sEmail: email,
                                                    bAtivo: ativo);

                return RedirectToAction("PrioridadeInsert");
            }
        }

        // GET: /EDIT
        public ActionResult PrioridadeEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Prioridade prioridade = null;

                oCadastroBasico.InfoPrioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigo: codigo,
                                                oPrioridade: ref prioridade);

                if (prioridade == null)
                {
                    return HttpNotFound();
                }
                    
                return View(prioridade);
            }
        }

        // POST: /EDIT
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult PrioridadeEdit(int codigo_unidade, string descricao, int codigo, bool envia_email = false, string email = "", bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdatePrioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    sDescricao: descricao,
                                                    bEnviaEmail: envia_email,
                                                    sEmail: email,
                                                    bAtivo: ativo,
                                                    iCodigo: codigo);

                //Redireciona para Index
                return RedirectToAction("PrioridadeIndex");
            }
        }

        // GET: /DELETE
        public ActionResult PrioridadeDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Prioridade prioridade = null;

                oCadastroBasico.InfoPrioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigo: codigo,
                                                oPrioridade: ref prioridade);

                if (prioridade == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(prioridade);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrioridadeDelete([Bind(Include = "codigo")] Prioridade prioridade)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeletePrioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigo: prioridade.codigo);

                    //Redireciona para Index
                    return RedirectToAction("PrioridadeIndex");
                }
                catch
                {
                    return PrioridadeDelete(codigo: prioridade.codigo,
                                            erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA FUNÇÃO
        public JsonResult ValidaPrioridade(int unidade, string descricao, int codigo)
        {

            return Json(oCadastroBasico.ValidaPrioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            sDescricao: descricao,
                                                            iCodigo: codigo));

        }

        #endregion

        #region ::: PREVENTIVA :::
                        
        // GET: INDEX
        public ActionResult PreventivaIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_preventiva",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.periodicidade = new SelectList(oCombo.Periodicidade(bChecklist: false), "codigo", "descricao", null);
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", 1);


                return View(oCadastroBasico.IndexProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                            iCodigoTipoOrdemServico: 5,
                                                            bRotina: false,
                                                            iAtivo: 1));
            }
        }

        // POST: INDEX
        [HttpPost]
        public ActionResult PreventivaIndex(int unidade, string descricao = "", string equipamento = "", int periodicidade = -1, int ativo = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_preventiva",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.periodicidade = new SelectList(oCombo.Periodicidade(bChecklist: false), "codigo", "descricao", periodicidade);
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", ativo);
                ViewBag.descricao = descricao;
                ViewBag.equipamento = equipamento;

                return View(oCadastroBasico.IndexProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                            iCodigoTipoOrdemServico: 5,
                                                            bRotina: false,
                                                            sDescricao: descricao,
                                                            sEquipamento: equipamento,
                                                            iCodigoPeriodicidade: periodicidade,
                                                            iAtivo: ativo));
            }
        }

        // GET: INSERT
        public ActionResult PreventivaInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_programada",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", null);
                ViewBag.categoria = new SelectList(oCombo.Categoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.familia = new SelectList(oCombo.FamiliaEquipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.equipamento = new SelectList(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.periodicidade = new SelectList(oCombo.Periodicidade(bChecklist:false), "codigo", "descricao", null);
                ViewBag.forma_lancamento = new SelectList(oCombo.FormaLancamentoPreventiva(), "codigo", "descricao", null);
                ViewBag.tipo_servico = new SelectList(oCombo.TipoServico(), "codigo", "descricao", null);
                ViewBag.tipo_ordem_servico = new SelectList(oCombo.TipoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                                    iProgramada: 1), "codigo", "descricao", null);
                ViewBag.checklist = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                    iCodigoTipoChecklist: Convert.ToInt32(eTipoChecklist.Preventiva)), "codigo", "descricao", null);
                    
                return View();
            }
        }

        // POST: INSERT
        [HttpPost, ValidateInput(false)]
        public ActionResult PreventivaInsert(int unidade, int categoria, string setor, string equipamento, string descricao, string valor_previsto, int quantidade_equipamento, int periodicidade, int intervalo, int tipo_servico, int tipo_ordem_servico, int modulo, long checklist = -1, bool ativo = false, bool exige_laudo = false, int familia = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {                 
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: unidade,
                                                    iCodigoCategoria: categoria,
                                                    iCodigoModulo: modulo,
                                                    iCodigoSetor: (setor == "") ? -1 : Convert.ToInt32(setor),
                                                    iCodigoFamiliaEquipamento: familia,
                                                    lCodigoEquipamento: (equipamento == null || equipamento == "") ? -1 : Convert.ToInt64(equipamento),
                                                    sDescricao: descricao,
                                                    dValorPrevisto: valor_previsto,
                                                    iQuantidadeEquipamento: quantidade_equipamento,
                                                    iCodigoPeriodicidade: periodicidade,
                                                    iIntervalo: intervalo,
                                                    lCodigoChecklist: checklist,
                                                    iCodigoTipoServico: tipo_servico,
                                                    iCodigoTipoOrdemServico: tipo_ordem_servico,
                                                    bAtivo: ativo,
                                                    bExigeLaudo: exige_laudo,
                                                    bRotina: false);

                return RedirectToAction("PreventivaInsert");
            }
        }

        // GET: /EDIT
        public ActionResult PreventivaEdit(int codigo, int codigo_unidade)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Programada programada = null;

                oCadastroBasico.InfoProgramada(iCodigoEmpresa: Convert.ToInt32( Session["empresa"].ToString()),
                                                iCodigoUnidade: codigo_unidade,
                                                lCodigo: codigo,
                                                oProgramada: ref programada);

                if (programada == null)
                {
                    return HttpNotFound();
                }


                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", programada.codigo_modulo);
                ViewBag.categoria = new SelectList(oCombo.Categoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                    iCodigoUnidade: programada.codigo_unidade), "codigo", "descricao", programada.codigo_categoria);
                ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: programada.codigo_unidade), "codigo", "descricao", programada.codigo_setor);
                ViewBag.equipamento = new SelectList(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: programada.codigo_unidade), "codigo", "descricao", programada.codigo_equipamento);
                ViewBag.periodicidade = new SelectList(oCombo.Periodicidade(bChecklist:false), "codigo", "descricao", programada.codigo_periodicidade);
                ViewBag.tipo_servico = new SelectList(oCombo.TipoServico(), "codigo", "descricao", programada.codigo_tipo_servico);
                ViewBag.tipo_ordem_servico = new SelectList(oCombo.TipoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                                    iProgramada: 1), "codigo", "descricao", programada.codigo_tipo_ordem_servico);
                ViewBag.checklist = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                    iCodigoTipoChecklist: Convert.ToInt32(eTipoChecklist.Preventiva)), "codigo", "descricao", programada.codigo_checklist);
                    
                return View(programada);
            }
        }

        // POST: /EDIT
        [HttpPost, ValidateInput(false)]
        public ActionResult PreventivaEdit(int codigo_unidade, int categoria, string setor, string equipamento, string descricao, string valor_previsto, int quantidade_equipamento, int periodicidade, int intervalo, int tipo_servico, int tipo_ordem_servico, long codigo, int unidade_old, int modulo, long checklist = -1, bool ativo = false, bool exige_laudo = false, int familia = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    iCodigoCategoria: categoria,
                                                    iCodigoSetor: (setor == "") ? -1 : Convert.ToInt32(setor),
                                                    lCodigoEquipamento: (equipamento == "") ? -1 : Convert.ToInt64(equipamento),
                                                    sDescricao: descricao,
                                                    dValorPrevisto: valor_previsto,
                                                    iQuantidadeEquipamento: quantidade_equipamento,
                                                    iCodigoPeriodicidade: periodicidade,
                                                    iIntervalo: intervalo,
                                                    lCodigoChecklist: checklist,
                                                    iCodigoTipoServico: tipo_servico,
                                                    iCodigoTipoOrdemServico: tipo_ordem_servico,
                                                    bExigeLaudo: exige_laudo,
                                                    bAtivo: ativo,
                                                    iCodigoModulo: modulo,
                                                    lCodigo: codigo,
                                                    iCodigoUnidadeOld: unidade_old);

                //Redireciona para Index
                return RedirectToAction("PreventivaIndex");
            }
        }

        // GET: /DELETE
        public ActionResult PreventivaDelete(long codigo, int codigo_unidade, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Programada programada = null;

                oCadastroBasico.InfoProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: codigo_unidade,
                                                lCodigo: codigo,
                                                oProgramada: ref programada);

                if (programada == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(programada);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PreventivaDelete([Bind(Include = "codigo, codigo_unidade")] Programada programada)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        lCodigo: programada.codigo,
                                                        iCodigoUnidade: programada.codigo_unidade);

                    //Redireciona para Index
                    return RedirectToAction("PreventivaIndex");
                }
                catch
                {
                    return PreventivaDelete(codigo: programada.codigo,
                                            codigo_unidade: programada.codigo_unidade,
                                            erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        #endregion

        #region ::: RELATÓRIO - ITENS AUDITÁVEIS :::

        // GET: INDEX
        public ActionResult RelatorioItensAuditaveisIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;
                bool imprimir = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_relatorio_itens_auditaveis",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.imprimir = imprimir;


                ViewBag.descricao = "";
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", 1);


                return View();
            }
        }

        // POST: INDEX
        [HttpPost]
        public ActionResult RelatorioItensAuditaveisIndex(string descricao, int unidade = -1, int ativo = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;
                bool imprimir = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_relatorio_itens_auditaveis",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.imprimir = imprimir;

                ViewBag.descricao = descricao;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: true), "codigo", "descricao", unidade);
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", ativo);

                return View();
            }
        }

        public JsonResult LoadRelatorioItensAuditaveis(string descricao, int unidade = -1, int ativo = -1)
        {
            List<MODELS.RelatorioItensAuditaveis> result = new List<MODELS.RelatorioItensAuditaveis>();

            result = oCadastroBasico.IndexRelatorioItensAuditaveis(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                   iCodigoUnidade: unidade,
                                                                   iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                   sDescricao: descricao,
                                                                   iAtivo: ativo);

            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        
        public JsonResult LoadRelatorioItensAuditaveisChecklist(long codigo)
        {
            List<MODELS.RelatorioItensAuditaveisDetais> result = new List<MODELS.RelatorioItensAuditaveisDetais>();

            result = oCadastroBasico.IndexRelatorioItensAuditaveisChecklist(lCodigoRelatorioItensAuditaveis: codigo);

            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        // GET: INSERT
        public ActionResult RelatorioItensAuditaveisInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());

                return View(oCadastroBasico.IndexRelatorioItensAuditaveisChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                   iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                   lCodigoRelatorioItensAuditaveis: -1));
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult RelatorioItensAuditaveisInsert(int unidade, string descricao, List<RelatorioItensAuditaveisDetais> details, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                long codigo = 0;

                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertRelatorioItensAuditaveis(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                               iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                               iCodigoUnidade: unidade,
                                                               iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                               sDescricao: descricao,
                                                               bAtivo: ativo,
                                                               lCodigo: ref codigo);

                foreach (RelatorioItensAuditaveisDetais item in details)
                {

                    if (item.selecionado && codigo > 0)
                    {

                        //Insere Registro no Banco de Dados
                        oCadastroBasico.InsertRelatorioItensAuditaveisChecklist(lCodigoRelatorioItensAuditaveis: codigo,
                                                                                iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                lCodigoChecklist: item.codigo_checklist,
                                                                                iCodigoChecklistItem: item.codigo);

                    }

                }

                return RedirectToAction("RelatorioItensAuditaveisInsert");
            }
        }

        // GET: /EDIT
        public ActionResult RelatorioItensAuditaveisEdit(long codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                RelatorioItensAuditaveis relatorio_itens_auditaveis = null;

                oCadastroBasico.InfoRelatorioItensAuditaveis(lCodigo: codigo,
                                                             oRelatorioItensAuditaveis: ref relatorio_itens_auditaveis);

                if (relatorio_itens_auditaveis == null)
                {
                    return HttpNotFound();
                }

                ViewBag.dados = relatorio_itens_auditaveis;

                return View(oCadastroBasico.IndexRelatorioItensAuditaveisChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                   iCodigoUnidade: relatorio_itens_auditaveis.codigo_unidade,
                                                                                   lCodigoRelatorioItensAuditaveis: relatorio_itens_auditaveis.codigo));
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RelatorioItensAuditaveisEdit(int codigo_unidade, string descricao, long codigo, List<RelatorioItensAuditaveisDetais> details, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Atualiza Registro no Banco de Dados
                oCadastroBasico.UpdateRelatorioItensAuditaveis(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                               iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                               iCodigoUnidade: codigo_unidade,
                                                               sDescricao: descricao,
                                                               bAtivo: ativo,
                                                               lCodigo: codigo);

                foreach (RelatorioItensAuditaveisDetais item in details)
                {

                    if (item.selecionado && codigo > 0)
                    {

                        //Insere Registro no Banco de Dados
                        oCadastroBasico.InsertRelatorioItensAuditaveisChecklist(lCodigoRelatorioItensAuditaveis: codigo,
                                                                                iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                lCodigoChecklist: item.codigo_checklist,
                                                                                iCodigoChecklistItem: item.codigo);

                    }

                }

                //Redireciona para Index
                return RedirectToAction("RelatorioItensAuditaveisIndex");
            }
        }

        // GET: /DELETE
        public JsonResult RelatorioItensAuditaveisDelete(long codigo)
        {
           
            try
            {
                oCadastroBasico.DeleteRelatorioItensAuditaveis(lCodigo: codigo,
                                                               iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()));

                return Json(PCM.WEB.Properties.Resources.register_deleted);
            } catch(Exception ex)
            {
                return Json(PCM.WEB.Properties.Resources.valida_excluir);
            }

        }

        //JSON: /VALIDA EQUIPAMENTO
        public JsonResult ValidaRelatorioItensAuditaveis(int unidade, string descricao, long codigo)
        {

            return Json(oCadastroBasico.ValidaRelatorioItensAuditaveis(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                       iCodigoUnidade: unidade,
                                                                       sDescricao: descricao,
                                                                       lCodigo: codigo));

        }

        #endregion

        #region ::: ROTINA :::

        // GET: INDEX
        public ActionResult RotinaIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_rotina",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                return View(oCadastroBasico.IndexProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                            iCodigoTipoOrdemServico: 7,
                                                            bRotina: true));
            }
        }

        // GET: INSERT
        public ActionResult RotinaInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.categoria = new SelectList(oCombo.Categoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.equipamento = new SelectList(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", null);
                ViewBag.periodicidade = new SelectList(oCombo.Periodicidade(bChecklist: false), "codigo", "descricao", null);
                ViewBag.prioridade = new SelectList(oCombo.Prioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.tipo_servico = new SelectList(oCombo.TipoServico(), "codigo", "descricao", null);
                ViewBag.checklist = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                    iCodigoTipoChecklist: Convert.ToInt32(eTipoChecklist.Rotina)), "codigo", "descricao", null);

                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RotinaInsert(int unidade, int categoria, string equipamento, string descricao, int periodicidade, int intervalo, int tipo_servico, int modulo, long checklist = -1, string setor = "", bool ativo = false, bool exige_laudo = false, bool segunda = true, bool terca = true, bool quarta = true, bool quinta = true, bool sexta = true, bool sabado = true, bool domingo = true, int prioridade = -1, string tempo_estimado = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: unidade,
                                                    iCodigoCategoria: categoria,
                                                    iCodigoModulo: modulo,
                                                    iCodigoSetor: (setor == "") ? -1 : Convert.ToInt32(setor),
                                                    lCodigoEquipamento: (equipamento == "") ? -1 : Convert.ToInt64(equipamento),
                                                    sDescricao: descricao,
                                                    dValorPrevisto: "0",
                                                    iQuantidadeEquipamento: 0,
                                                    iCodigoPeriodicidade: periodicidade,
                                                    iIntervalo: intervalo,
                                                    lCodigoChecklist: checklist,
                                                    iCodigoTipoServico: tipo_servico,
                                                    iCodigoTipoOrdemServico: 7,
                                                    bAtivo: ativo,
                                                    bEnviaEmail: false,
                                                    sEmail: "",
                                                    iDiasAlerta: 0,
                                                    bExigeLaudo: exige_laudo,
                                                    bRotina: true,
                                                    iCodigoPrioridade: prioridade,
                                                    dTempoEstimado: (tempo_estimado == "") ? 0 : Convert.ToDouble(tempo_estimado),
                                                    bSegunda: segunda,
                                                    bTerca: terca,
                                                    bQuarta: quarta,
                                                    bQuinta: quinta,
                                                    bSexta: sexta,
                                                    bSabado: sabado,
                                                    bDomingo: domingo);

                return RedirectToAction("RotinaInsert");
            }
        }

        // GET: /EDIT
        public ActionResult RotinaEdit(int codigo, int codigo_unidade)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Programada programada = null;

                oCadastroBasico.InfoProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: codigo_unidade,
                                                lCodigo: codigo,
                                                oProgramada: ref programada);

                if (programada == null)
                {
                    return HttpNotFound();
                }

                ViewBag.categoria = new SelectList(oCombo.Categoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: programada.codigo_unidade), "codigo", "descricao", programada.codigo_categoria);
                ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: programada.codigo_unidade), "codigo", "descricao", programada.codigo_setor);
                ViewBag.equipamento = new SelectList(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: programada.codigo_unidade), "codigo", "descricao", programada.codigo_equipamento);
                ViewBag.periodicidade = new SelectList(oCombo.Periodicidade(bChecklist: false), "codigo", "descricao", programada.codigo_periodicidade);
                ViewBag.prioridade = new SelectList(oCombo.Prioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", programada.codigo_prioridade);
                ViewBag.tipo_servico = new SelectList(oCombo.TipoServico(), "codigo", "descricao", programada.codigo_tipo_servico);
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", programada.codigo_modulo);
                ViewBag.checklist = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: programada.codigo_unidade,
                                                                    iCodigoTipoChecklist: Convert.ToInt32(eTipoChecklist.Rotina)), "codigo", "descricao", programada.codigo_checklist);

                return View(programada);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RotinaEdit(int codigo_unidade, int categoria, string equipamento, string descricao, int periodicidade, int intervalo, int tipo_servico, int modulo, long codigo, int unidade_old, long checklist = -1, string setor = "", bool ativo = false, bool exige_laudo = false, int prioridade = -1, bool segunda = true, bool terca = true, bool quarta = true, bool quinta = true, bool sexta = true, bool sabado = true, bool domingo = true, string tempo_estimado = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    iCodigoCategoria: categoria,
                                                    iCodigoSetor: (setor == "") ? -1 : Convert.ToInt32(setor),
                                                    lCodigoEquipamento: (equipamento == "") ? -1 : Convert.ToInt64(equipamento),
                                                    sDescricao: descricao,
                                                    dValorPrevisto: "0",
                                                    iQuantidadeEquipamento: 0,
                                                    iCodigoPeriodicidade: periodicidade,
                                                    iIntervalo: intervalo,
                                                    lCodigoChecklist: checklist,
                                                    iCodigoTipoServico: tipo_servico,
                                                    iCodigoTipoOrdemServico: 7,
                                                    bExigeLaudo: exige_laudo,
                                                    bEnviaEmail: false,
                                                    sEmail: "",
                                                    iDiasAlerta: 0,
                                                    bAtivo: ativo,
                                                    iCodigoModulo : modulo,
                                                    lCodigo: codigo,
                                                    iCodigoUnidadeOld: unidade_old,
                                                    iCodigoPrioridade: prioridade,
                                                    dTempoEstimado: (tempo_estimado=="")?0: Convert.ToDouble(tempo_estimado),
                                                    bSegunda: segunda,
                                                    bTerca: terca,
                                                    bQuarta: quarta,
                                                    bQuinta: quinta,
                                                    bSexta: sexta,
                                                    bSabado: sabado,
                                                    bDomingo: domingo);

                //Redireciona para Index
                return RedirectToAction("RotinaIndex");
            }
        }

        // GET: /DELETE
        public ActionResult RotinaDelete(long codigo, int codigo_unidade, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Programada programada = null;

                oCadastroBasico.InfoProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: codigo_unidade,
                                                lCodigo: codigo,
                                                oProgramada: ref programada);

                if (programada == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(programada);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RotinaDelete([Bind(Include = "codigo, codigo_unidade")] Rotina programada)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        lCodigo: programada.codigo,
                                                        iCodigoUnidade: programada.codigo_unidade);

                    //Redireciona para Index
                    return RedirectToAction("RotinaIndex");
                }
                catch
                {
                    return RotinaDelete(codigo: programada.codigo,
                                        codigo_unidade: programada.codigo_unidade,
                                        erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        // POST: /PRINT TAG EQUIPAMENTO
        public ActionResult PrintEtiquetaRotina(int unidade, string codigo, string report)
        {

            ReportDocument oReportDocument = new ReportDocument();
            TableLogOnInfo oTableLogOnInfo = new TableLogOnInfo();
            ConnectionInfo oConnectionInfo = new ConnectionInfo();
            Database oCrDatabase;
            Tables oCrTables;

            oConnectionInfo.ServerName = ConfigurationManager.AppSettings["data_source"].ToString();
            oConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["initial_catalog"].ToString();
            oConnectionInfo.UserID = ConfigurationManager.AppSettings["user_id"].ToString();
            oConnectionInfo.Password = ConfigurationManager.AppSettings["password"].ToString();
            oConnectionInfo.Type = ConnectionInfoType.SQL;
            oConnectionInfo.IntegratedSecurity = false;

            if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Reports"), report + Session["empresa"].ToString() + ".rpt")))
            {
                report = report + Session["empresa"].ToString();
            }

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), report + ".rpt"));

            oCrDatabase = oReportDocument.Database;
            oCrTables = oCrDatabase.Tables;

            foreach (Table crTable in oCrTables)
            {
                oTableLogOnInfo = crTable.LogOnInfo;
                oTableLogOnInfo.ConnectionInfo = oConnectionInfo;
                crTable.ApplyLogOnInfo(oTableLogOnInfo);
            }

            oReportDocument.SetParameterValue("@codigo", codigo);
            oReportDocument.SetParameterValue("@codigo_empresa", Convert.ToInt32(Session["empresa"].ToString()));
            oReportDocument.SetParameterValue("@codigo_unidade", unidade);

            oReportDocument.SetDatabaseLogon(ConfigurationManager.AppSettings.GetValues("user_id")[0],
                                                ConfigurationManager.AppSettings.GetValues("password")[0],
                                                ConfigurationManager.AppSettings.GetValues("data_source")[0],
                                                ConfigurationManager.AppSettings.GetValues("initial_catalog")[0]);

        Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
        stream.Seek(0, SeekOrigin.Begin);
        Response.AppendHeader("Content-Length", stream.Length.ToString());
        Response.AppendHeader("Content-Disposition", "inline; filename=" + report + ".pdf");
        return File(stream, "application/pdf;");
    }

        #endregion

        #region ::: SETOR :::

        // GET: INDEX
        public ActionResult SetorIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_setor",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                return View(oCadastroBasico.IndexSetor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())));
        }
        }

        // GET: INSERT
        public ActionResult SetorInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());


                List<SetorLocal> registros = new List<SetorLocal>();

                SetorLocal info = new SetorLocal();

                info.codigo = 0;
                info.local = "";
                info.excluido = 0;

                registros.Add(info);

                return View(registros);
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult SetorInsert(int unidade, string descricao, string observacao, List<SetorLocal> registros, bool ativo = false, string metragem = "0", string carga_termica = "0", string descricao_atividade = "", int numero_pessoas_fixas = 0, int numero_pessoas_volantes = 0)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                int codigo = 0;
                
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertSetor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                            iCodigoUnidade: unidade,
                                            sDescricao: descricao,
                                            sObservacao: observacao,
                                            dMetragem: Convert.ToDouble((metragem == "") ? "0" : metragem),
                                            dCargaTermica: Convert.ToDouble((carga_termica == "") ? "0" : carga_termica),
                                            sDescricaoAtividade: descricao_atividade.ToUpper(),
                                            iNumeroPessoasFixas: numero_pessoas_fixas,
                                            iNumeroPessoasVolantes: numero_pessoas_volantes,
                                            bAtivo: ativo,
                                            iCodigo: ref codigo);

                foreach (SetorLocal reg in registros)
                {
                    if (codigo > 0 && reg.excluido == 0 && reg.local != null)
                    {
                        //Insere Registro no Banco de Dados
                        oCadastroBasico.InsertSetorLocal(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            iCodigoUnidade: unidade,
                                                            iCodigoSetor: codigo,
                                                            sLocal: reg.local);
                    }
                }

                return RedirectToAction("SetorInsert");

            }
        }

        // GET: /EDIT
        public ActionResult SetorEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Setor setor = null;

                oCadastroBasico.InfoSetor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigo: codigo,
                                            oSetor: ref setor);

                if (setor == null)
                {
                    return HttpNotFound();
                }

                ViewBag.ativo = (setor.ativo) ? "checked" : "";
                ViewBag.setor = setor;

                return View(setor.local);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetorEdit(int codigo_unidade, string descricao, string observacao, int codigo, List<SetorLocal> registros, bool ativo = false, string metragem = "0", string carga_termica = "0", string descricao_atividade = "", string numero_pessoas_fixas = "0", string numero_pessoas_volantes = "0")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateSetor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                            iCodigoUnidade: codigo_unidade,
                                            sDescricao: descricao,
                                            sObservacao: observacao,
                                            dMetragem: metragem,
                                            dCargaTermica: carga_termica,
                                            sDescricaoAtividade: descricao_atividade.ToUpper(),
                                            iNumeroPessoasFixas: numero_pessoas_fixas,
                                            iNumeroPessoasVolantes: numero_pessoas_volantes,
                                            bAtivo: ativo,
                                            iCodigo: codigo);

                foreach (SetorLocal reg in registros)
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.UpdateSetorLocal(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigoUnidade: codigo_unidade,
                                                        iCodigoSetor: codigo,
                                                        sLocal: reg.local,
                                                        iExcluido: reg.excluido,
                                                        iCodigo: reg.codigo);
                }

                //Redireciona para Index
                return RedirectToAction("SetorIndex");
            }
        }

        // GET: /DELETE
        public ActionResult SetorDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Setor setor = null;

                oCadastroBasico.InfoSetor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigo: codigo,
                                            oSetor: ref setor);

                if (setor == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(setor);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetorDelete([Bind(Include = "codigo")] Setor setor)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteSetor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                iCodigo: setor.codigo);

                    //Redireciona para Index
                    return RedirectToAction("SetorIndex");
                }
                catch
                {
                    return SetorDelete(codigo: setor.codigo,
                                        erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA SETOR
        public JsonResult ValidaSetor(int unidade, string descricao, int codigo)
        {

            return Json(oCadastroBasico.ValidaSetor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: unidade,
                                                    sDescricao: descricao,
                                                    iCodigo: codigo));

        }

        #endregion

        #region ::: TAREFA :::

        // GET: INDEX
        public ActionResult TarefaIndex()
        {
            if (Session["language"] == null || User.Identity.GetUserName() == "")
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_tarefa",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.empresa = Session["empresa"].ToString();
                ViewBag.usuario = User.Identity.GetUserName();

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()), bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());                

                return View();
            }
        }

        // POST: INDEX
        [HttpPost]
        public ActionResult TarefaIndex(string unidade, string descricao)
        {
            if (Session["language"] == null || User.Identity.GetUserName() == "")
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_tarefa",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.descricao = descricao;
                ViewBag.empresa = Session["empresa"].ToString();
                ViewBag.usuario = User.Identity.GetUserName();

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()), bCadastro: false), "codigo", "descricao", unidade);

                return View();
            }
        }

        //JSON: /TAREFA/
        public JsonResult LoadTarefa(int unidade, string descricao)
        {

            return Json(oCadastroBasico.IndexTarefa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: unidade,
                                                    iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                    sDescricao: descricao));

        }

        // GET: INSERT
        public ActionResult TarefaInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.periodicidade = new SelectList(oCombo.Periodicidade(bChecklist: false), "codigo", "descricao", null);
                ViewBag.checklist = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                    iCodigoTipoChecklist: Convert.ToInt32(eTipoChecklist.Tarefa)), "codigo", "descricao", null);

                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TarefaInsert(int unidade, string descricao, int periodicidade, int intervalo, long checklist = -1, bool ativo = false, bool segunda = true, bool terca = true, bool quarta = true, bool quinta = true, bool sexta = true, bool sabado = true, bool domingo = true)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertTarefa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                             iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                             iCodigoUnidade: unidade,
                                             iCodigoModulo: 2,
                                             sDescricao: descricao,
                                             iCodigoPeriodicidade: periodicidade,
                                             iIntervalo: intervalo,
                                             lCodigoChecklist: checklist,
                                             bAtivo: ativo,
                                             bSegunda: segunda,
                                             bTerca: terca,
                                             bQuarta: quarta,
                                             bQuinta: quinta,
                                             bSexta: sexta,
                                             bSabado: sabado,
                                             bDomingo: domingo);

                return RedirectToAction("TarefaInsert");
            }
        }

        // GET: /EDIT
        public ActionResult TarefaEdit(int codigo, int codigo_unidade)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Tarefa tarefa = null;

                oCadastroBasico.InfoTarefa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                           iCodigoUnidade: codigo_unidade,
                                           lCodigo: codigo,
                                           oTarefa: ref tarefa);

                if (tarefa == null)
                {
                    return HttpNotFound();
                }

                ViewBag.periodicidade = new SelectList(oCombo.Periodicidade(bChecklist: false), "codigo", "descricao", tarefa.codigo_periodicidade);
                ViewBag.checklist = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: tarefa.codigo_unidade,
                                                                    iCodigoTipoChecklist: Convert.ToInt32(eTipoChecklist.Tarefa)), "codigo", "descricao", tarefa.codigo_checklist);

                return View(tarefa);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TarefaEdit(int codigo_unidade, string descricao, int periodicidade, int intervalo, long codigo, int unidade_old, long checklist = -1, bool ativo = false, bool segunda = true, bool terca = true, bool quarta = true, bool quinta = true, bool sexta = true, bool sabado = true, bool domingo = true)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateTarefa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                             iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                             iCodigoUnidade: codigo_unidade,
                                             sDescricao: descricao,
                                             iCodigoPeriodicidade: periodicidade,
                                             iIntervalo: intervalo,
                                             lCodigoChecklist: checklist,
                                             bAtivo: ativo,
                                             iCodigoModulo: 2,
                                             lCodigo: codigo,
                                             iCodigoUnidadeOld: unidade_old,
                                             bSegunda: segunda,
                                             bTerca: terca,
                                             bQuarta: quarta,
                                             bQuinta: quinta,
                                             bSexta: sexta,
                                             bSabado: sabado,
                                             bDomingo: domingo);

                //Redireciona para Index
                return RedirectToAction("TarefaIndex");
            }
        }

        //JSON: /DELETA TAREFA
        public JsonResult TarefaDelete(int empresa, int unidade, int usuario, long codigo)
        {
            try
            {
                oCadastroBasico.DeleteTarefa(iCodigoEmpresa: empresa,
                                             iCodigoUsuario: usuario,
                                             lCodigo: codigo,
                                             iCodigoUnidade: unidade);

                return Json(PCM.WEB.Properties.Resources.register_deleted);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        // POST: /PRINT TAG EQUIPAMENTO
        public ActionResult PrintEtiquetaTarefa(int unidade, string codigo, string report)
        {

            ReportDocument oReportDocument = new ReportDocument();
            TableLogOnInfo oTableLogOnInfo = new TableLogOnInfo();
            ConnectionInfo oConnectionInfo = new ConnectionInfo();
            Database oCrDatabase;
            Tables oCrTables;

            oConnectionInfo.ServerName = ConfigurationManager.AppSettings["data_source"].ToString();
            oConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["initial_catalog"].ToString();
            oConnectionInfo.UserID = ConfigurationManager.AppSettings["user_id"].ToString();
            oConnectionInfo.Password = ConfigurationManager.AppSettings["password"].ToString();
            oConnectionInfo.Type = ConnectionInfoType.SQL;
            oConnectionInfo.IntegratedSecurity = false;

            if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Reports"), report + Session["empresa"].ToString() + ".rpt")))
            {
                report = report + Session["empresa"].ToString();
            }

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), report + ".rpt"));

            oCrDatabase = oReportDocument.Database;
            oCrTables = oCrDatabase.Tables;

            foreach (Table crTable in oCrTables)
            {
                oTableLogOnInfo = crTable.LogOnInfo;
                oTableLogOnInfo.ConnectionInfo = oConnectionInfo;
                crTable.ApplyLogOnInfo(oTableLogOnInfo);
            }

            oReportDocument.SetParameterValue("@codigo", codigo);
            oReportDocument.SetParameterValue("@codigo_empresa", Convert.ToInt32(Session["empresa"].ToString()));
            oReportDocument.SetParameterValue("@codigo_unidade", unidade);

            oReportDocument.SetDatabaseLogon(ConfigurationManager.AppSettings.GetValues("user_id")[0],
                                                ConfigurationManager.AppSettings.GetValues("password")[0],
                                                ConfigurationManager.AppSettings.GetValues("data_source")[0],
                                                ConfigurationManager.AppSettings.GetValues("initial_catalog")[0]);

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=" + report + ".pdf");
            return File(stream, "application/pdf;");
        }

        #endregion

        #region ::: TIPO APARTAMENTO :::

        // GET: INDEX
        public ActionResult TipoApartamentoIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_tipo_apartamento",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                               iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                               bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", 1);

                return View();
            }
        }

        // POST: INDEX
        [HttpPost]
        public ActionResult TipoApartamentoIndex(int unidade, string descricao, int ativo = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_tipo_apartamento",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                               iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                               bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", ativo);
                ViewBag.descricao = descricao;

                return View();
            }
        }

        public JsonResult LoadTipoApartamentoIndex(int unidade, string descricao, int ativo = -1)
        {
            return Json(oCadastroBasico.IndexTipoApartamento(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                             codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                             codigoUnidade: unidade,
                                                             descricao: descricao,
                                                             ativo: ativo));
        }

        // GET: INSERT
        public ActionResult TipoApartamentoInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.checklist_uh = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                        iCodigoTipoChecklist: Convert.ToInt32(eTipoChecklist.UH)), "codigo", "descricao", null);
                ViewBag.periodicidade_uh = new SelectList(oCombo.Periodicidade(bChecklist: false), "codigo", "descricao", null);
                ViewBag.checklist_governanca_permanencia = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                           iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                           iCodigoTipoChecklist: Convert.ToInt32(eTipoChecklist.Governanca)), "codigo", "descricao",
                                                                                           null);
                ViewBag.checklist_governanca_saida = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                     iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                     iCodigoTipoChecklist: Convert.ToInt32(eTipoChecklist.Governanca)), "codigo", "descricao",
                                                                                     null);
                ViewBag.checklist_governanca_manutencao = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                          iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                          iCodigoTipoChecklist: Convert.ToInt32(eTipoChecklist.Governanca)), "codigo", "descricao",
                                                                                          null);
                ViewBag.checklist_governanca_permanencia_vistoria = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                                    iCodigoTipoChecklist: Convert.ToInt32(eTipoChecklist.Governanca)), "codigo", "descricao",
                                                                                                    null);
                ViewBag.checklist_governanca_saida_vistoria = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                              iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                              iCodigoTipoChecklist: Convert.ToInt32(eTipoChecklist.Governanca)), "codigo", "descricao",
                                                                                              null);
                ViewBag.checklist_governanca_manutencao_vistoria = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                   iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                                   iCodigoTipoChecklist: Convert.ToInt32(eTipoChecklist.Governanca)), "codigo", "descricao",
                                                                                                   null);
                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult TipoApartamentoInsert(int unidade, string descricao, long checklist_uh = -1, long checklist_governanca_permanencia = -1, long checklist_governanca_saida = -1, long checklist_governanca_manutencao = -1, long checklist_governanca_permanencia_vistoria = -1, long checklist_governanca_saida_vistoria = -1, long checklist_governanca_manutencao_vistoria = -1, int periodicidade_uh = -1, int intervalo_uh = -1, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertTipoApartamento(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                      codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                      codigoUnidade: unidade,
                                                      descricao: descricao,
                                                      codigoChecklistUH: checklist_uh,
                                                      codigoPeriodicidadeUH: periodicidade_uh,
                                                      intervaloUH: intervalo_uh,
                                                      codigoChecklistGovernancaPermanencia: checklist_governanca_permanencia,
                                                      codigoChecklistGovernancaSaida: checklist_governanca_saida,
                                                      codigoChecklistGovernancaManutencao: checklist_governanca_manutencao,
                                                      codigoChecklistGovernancaPermanenciaVistoria: checklist_governanca_permanencia_vistoria,
                                                      codigoChecklistGovernancaSaidaVistoria: checklist_governanca_saida_vistoria,
                                                      codigoChecklistGovernancaManutencaoVistoria: checklist_governanca_manutencao_vistoria,
                                                      ativo: ativo);

                return RedirectToAction("TipoApartamentoInsert");
            }
        }

        // GET: /EDIT
        public ActionResult TipoApartamentoEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                TipoApartamento tipo_apartamento = null;

                oCadastroBasico.InfoTipoApartamento(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    codigo: codigo,
                                                    oTipoApartamento: ref tipo_apartamento);

                if (tipo_apartamento == null)
                {
                    return HttpNotFound();
                }


                ViewBag.checklist_uh = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                        iCodigoTipoChecklist: Convert.ToInt32(eTipoChecklist.UH)), "codigo", "descricao", tipo_apartamento.codigo_checklist_uh);
                ViewBag.periodicidade_uh = new SelectList(oCombo.Periodicidade(bChecklist: false), "codigo", "descricao", tipo_apartamento.codigo_periodicidade_uh);
                ViewBag.checklist_governanca_permanencia = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                           iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                           iCodigoTipoChecklist: Convert.ToInt32(eTipoChecklist.Governanca)), "codigo", "descricao", 
                                                                                           tipo_apartamento.codigo_checklist_governanca_permanencia);
                ViewBag.checklist_governanca_saida = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                     iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                     iCodigoTipoChecklist: Convert.ToInt32(eTipoChecklist.Governanca)), "codigo", "descricao",
                                                                                     tipo_apartamento.codigo_checklist_governanca_saida);
                ViewBag.checklist_governanca_manutencao = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                          iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                          iCodigoTipoChecklist: Convert.ToInt32(eTipoChecklist.Governanca)), "codigo", "descricao",
                                                                                          tipo_apartamento.codigo_checklist_governanca_manutencao);
                ViewBag.checklist_governanca_permanencia_vistoria = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                                    iCodigoTipoChecklist: Convert.ToInt32(eTipoChecklist.Governanca)), "codigo", "descricao",
                                                                                                    tipo_apartamento.codigo_checklist_governanca_permanencia_vistoria);
                ViewBag.checklist_governanca_saida_vistoria = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                              iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                              iCodigoTipoChecklist: Convert.ToInt32(eTipoChecklist.Governanca)), "codigo", "descricao",
                                                                                              tipo_apartamento.codigo_checklist_governanca_saida_vistoria);
                ViewBag.checklist_governanca_manutencao_vistoria = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                   iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                                   iCodigoTipoChecklist: Convert.ToInt32(eTipoChecklist.Governanca)), "codigo", "descricao",
                                                                                                   tipo_apartamento.codigo_checklist_governanca_manutencao_vistoria);

                return View(tipo_apartamento);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TipoApartamentoEdit(int codigo_unidade, string descricao, int codigo, long checklist_uh = -1, long checklist_governanca_permanencia = -1, long checklist_governanca_saida = -1, long checklist_governanca_manutencao = -1, long checklist_governanca_permanencia_vistoria = -1, long checklist_governanca_saida_vistoria = -1, long checklist_governanca_manutencao_vistoria = -1, int periodicidade_uh = -1, int intervalo_uh = -1, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateTipoApartamento(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                      codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                      codigoUnidade: codigo_unidade,
                                                      descricao: descricao,
                                                      codigoChecklistUH: checklist_uh,
                                                      codigoPeriodicidadeUH: periodicidade_uh,
                                                      intervaloUH: intervalo_uh,
                                                      codigoChecklistGovernancaPermanencia: checklist_governanca_permanencia,
                                                      codigoChecklistGovernancaSaida: checklist_governanca_saida,
                                                      codigoChecklistGovernancaManutencao: checklist_governanca_manutencao,
                                                      codigoChecklistGovernancaPermanenciaVistoria: checklist_governanca_permanencia_vistoria,
                                                      codigoChecklistGovernancaSaidaVistoria: checklist_governanca_saida_vistoria,
                                                      codigoChecklistGovernancaManutencaoVistoria: checklist_governanca_manutencao_vistoria,
                                                      ativo: ativo,
                                                      codigo: codigo);

                //Redireciona para Index
                return RedirectToAction("TipoApartamentoIndex");
            }
        }

        // GET: /DELETE
        public ActionResult TipoApartamentoDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                TipoApartamento tipo_apartamento = null;

                oCadastroBasico.InfoTipoApartamento(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    codigo: codigo,
                                                    oTipoApartamento: ref tipo_apartamento);

                if (tipo_apartamento == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(tipo_apartamento);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TipoApartamentoDelete([Bind(Include = "codigo")] TipoApartamento tipo_apartamento)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteTipoApartamento(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                          codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                          codigo: tipo_apartamento.codigo);

                    //Redireciona para Index
                    return RedirectToAction("TipoApartamentoIndex");
                }
                catch
                {
                    return TipoApartamentoDelete(codigo: tipo_apartamento.codigo,
                                                    erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA FUNÇÃO
        public JsonResult ValidaTipoApartamento(int unidade, string descricao, int codigo)
        {

            return Json(oCadastroBasico.ValidaTipoApartamento(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                              codigoUnidade: unidade,
                                                              descricao: descricao,
                                                              codigo: codigo));

        }

        #endregion

        #region ::: TIPO DE AR CONDICIONADO :::

        // GET: INDEX
        public ActionResult TipoArCondicionadoIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_tipo_ar_condicionado",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                return View(oCadastroBasico.IndexTipoArCondicionado(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())));
            }
        }

        // GET: INSERT
        public ActionResult TipoArCondicionadoInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.periodicidade = new SelectList(oCombo.Periodicidade(bChecklist: true), "codigo", "descricao", null);
                ViewBag.checklist = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                    iCodigoTipoChecklist: Convert.ToInt32(eTipoChecklist.PMOC)), "codigo", "descricao", null);

                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult TipoArCondicionadoInsert(string tipo, string descricao, int periodicidade, int intervalo = -1, long checklist = -1, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertTipoArCondicionado(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            sTipo: tipo,
                                                            sDescricao: descricao,
                                                            iCodigoPeriodicidade: periodicidade,
                                                            iIntervalo: intervalo,
                                                            lCodigoChecklist: checklist,
                                                            bAtivo: ativo);

                return RedirectToAction("TipoArCondicionadoInsert");
            }
        }

        // GET: /EDIT
        public ActionResult TipoArCondicionadoEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                TipoArCondicionado tipo_ar_condicionado = null;

                oCadastroBasico.InfoTipoArCondicionado(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigo: codigo,
                                                        oTipoArCondicionado: ref tipo_ar_condicionado);

                if (tipo_ar_condicionado == null)
                {
                    return HttpNotFound();
                }

                ViewBag.periodicidade = new SelectList(oCombo.Periodicidade(bChecklist: true), "codigo", "descricao", tipo_ar_condicionado.codigo_periodicidade);
                ViewBag.checklist = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                    iCodigoTipoChecklist: Convert.ToInt32(eTipoChecklist.PMOC)), "codigo", "descricao", tipo_ar_condicionado.codigo_checklist);

                ViewBag.ativo = (tipo_ar_condicionado.ativo) ? "checked" : "";

                return View(tipo_ar_condicionado);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TipoArCondicionadoEdit(string tipo, string descricao, int periodicidade, int intervalo, int codigo, long checklist = -1, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateTipoArCondicionado(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            sTipo: tipo,
                                                            sDescricao: descricao,
                                                            iCodigoPeriodicidade: periodicidade,
                                                            iIntervalo: intervalo,
                                                            lCodigoChecklist:checklist,
                                                            bAtivo: ativo,
                                                            iCodigo: codigo); 
                //Redireciona para Index
                return RedirectToAction("TipoArCondicionadoIndex");
            }
        }

        // GET: /DELETE
        public ActionResult TipoArCondicionadoDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                TipoArCondicionado tipo_ar_condicionado = null;

                oCadastroBasico.InfoTipoArCondicionado(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigo: codigo,
                                                        oTipoArCondicionado: ref tipo_ar_condicionado);

                if (tipo_ar_condicionado == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(tipo_ar_condicionado);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TipoArCondicionadoDelete([Bind(Include = "codigo")] TipoArCondicionado tipo_ar_condicionado)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteTipoArCondicionado(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                iCodigo: tipo_ar_condicionado.codigo);

                    //Redireciona para Index
                    return RedirectToAction("TipoArCondicionadoIndex");
                }
                catch
                {
                    return TipoArCondicionadoDelete(codigo: tipo_ar_condicionado.codigo,
                                                    erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA FUNÇÃO
        public JsonResult ValidaTipoArCondicionado(string tipo, int codigo)
        {

            return Json(oCadastroBasico.ValidaTipoArCondicionado(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    sTipo: tipo,
                                                                    iCodigo: codigo));

        }

        #endregion

        #region ::: TIPO CAMA :::

        // GET: INDEX
        public ActionResult TipoCamaIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_tipo_cama",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                return View(oCadastroBasico.IndexTipoCama(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())));
        }
        }

        // GET: INSERT
        public ActionResult TipoCamaInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult TipoCamaInsert(int unidade, string descricao, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertTipoCama(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                iCodigoUnidade: unidade,
                                                sDescricao: descricao,
                                                bAtivo: ativo);

                return RedirectToAction("TipoCamaInsert");
            }
        }

        // GET: /EDIT
        public ActionResult TipoCamaEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                TipoCama tipo_cama = null;

                oCadastroBasico.InfoTipoCama(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigo: codigo,
                                                oTipoCama: ref tipo_cama);

                if (tipo_cama == null)
                {
                    return HttpNotFound();
                }

                return View(tipo_cama);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TipoCamaEdit(int codigo_unidade, string descricao, int codigo, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateTipoCama(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                iCodigoUnidade: codigo_unidade,
                                                sDescricao: descricao,
                                                bAtivo: ativo,
                                                iCodigo: codigo);

                //Redireciona para Index
                return RedirectToAction("TipoCamaIndex");
            }
        }

        // GET: /DELETE
        public ActionResult TipoCamaDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                TipoCama tipo_cama = null;

                oCadastroBasico.InfoTipoCama(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigo: codigo,
                                                oTipoCama: ref tipo_cama);

                if (tipo_cama == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(tipo_cama);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TipoCamaDelete([Bind(Include = "codigo")] TipoCama tipo_cama)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteTipoCama(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigo: tipo_cama.codigo);

                    //Redireciona para Index
                    return RedirectToAction("TipoCamaIndex");
                }
                catch
                {
                    return TipoCamaDelete(codigo: tipo_cama.codigo,
                                            erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA FUNÇÃO
        public JsonResult ValidaTipoCama(int unidade, string descricao, int codigo)
        {

            return Json(oCadastroBasico.ValidaTipoCama(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUnidade: unidade,
                                                        sDescricao: descricao,
                                                        iCodigo: codigo));

        }

        #endregion

        #region ::: TIPO CHECKLIST :::

        // GET: INDEX
        public ActionResult TipoChecklistIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_tipo_checklist",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                return View(oCadastroBasico.IndexTipoChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())));
        }
        }

        // GET: INSERT
        public ActionResult TipoChecklistInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                return View(); 
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult TipoChecklistInsert(int unidade, string descricao, string observacao, List<TipoChecklistItem> checklist, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                int codigo = 0;

                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertTipoChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: unidade,
                                                    sDescricao: descricao,
                                                    bAtivo: ativo,
                                                    iCodigo: ref codigo);

                foreach (TipoChecklistItem item in checklist)
                {

                    if (item.selecionado) {

                        //Insere Registro no Banco de Dados
                        oCadastroBasico.InsertTipoChecklistItem(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoTipoChecklist: codigo,
                                                                iCodigoUnidade: unidade,
                                                                iCodigoChecklist: item.codigo);

                    }

                }

                return RedirectToAction("TipoChecklistInsert");
            }
        }

        // GET: /EDIT
        public ActionResult TipoChecklistEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                TipoChecklist tipo_checklist = null;

                oCadastroBasico.InfoTipoChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigo: codigo,
                                                    oTipoChecklist: ref tipo_checklist);

                if (tipo_checklist == null)
                {
                    return HttpNotFound();
                }

                ViewBag.tipo_checklist = tipo_checklist;

                return View(oCadastroBasico.IndexTipoChecklistItem(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoTipoChecklist: tipo_checklist.codigo,
                                                                    iCodigoUnidade: tipo_checklist.codigo_unidade));
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TipoChecklistEdit(int unidade, string descricao, int codigo, List<TipoChecklistItem> checklist, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateTipoChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: unidade,
                                                    sDescricao: descricao,
                                                    bAtivo: ativo,
                                                    iCodigo: codigo);

                foreach (TipoChecklistItem item in checklist)
                {

                    if (item.selecionado)
                    {

                        //Insere Registro no Banco de Dados
                        oCadastroBasico.InsertTipoChecklistItem(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoTipoChecklist: codigo,
                                                                iCodigoUnidade: unidade,
                                                                iCodigoChecklist: item.codigo);

                    }

                }

                //Redireciona para Index
                return RedirectToAction("TipoChecklistIndex");
            }
        }

        // GET: /DELETE
        public ActionResult TipoChecklistDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                TipoChecklist tipo_checklist = null;

                oCadastroBasico.InfoTipoChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigo: codigo,
                                                    oTipoChecklist: ref tipo_checklist);

                if (tipo_checklist == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(tipo_checklist);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TipoChecklistDelete([Bind(Include = "codigo")] TipoChecklist tipo_checklist)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteTipoChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigo: tipo_checklist.codigo);

                    //Redireciona para Index
                    return RedirectToAction("TipoChecklistIndex");
                }
                catch
                {
                    return TipoChecklistDelete(codigo: tipo_checklist.codigo,
                                                erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA FUNÇÃO
        public JsonResult ValidaTipoChecklist(int unidade, string descricao, int codigo)
        {

            return Json(oCadastroBasico.ValidaTipoChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            sDescricao: descricao,
                                                            iCodigo: codigo));

        }
        
        //JSON: /VALIDA FUNÇÃO
        //public JsonResult LoadChecklist(int unidade)
        //{

        //    return Json(oCadastroBasico.IndexTipoChecklistItem(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
        //                                                       iCodigoTipoChecklist: -1, 
        //                                                       iCodigoUnidade: unidade));

        //}
        
        #endregion

        #region ::: TIPO DE DESPESA :::

        // GET: INDEX
        public ActionResult TipoDespesaIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_tipo_despesa",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                return View(oCadastroBasico.IndexTipoDespesa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()), 
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())));
            }
        }

        // GET: INSERT
        public ActionResult TipoDespesaInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult TipoDespesaInsert(int unidade, string codigo_tipo_despesa, string descricao, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertTipoDespesa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: unidade,
                                                    sCodigoTipoDespesa: codigo_tipo_despesa,
                                                    sDescricao: descricao,
                                                    bAtivo: ativo);

                return RedirectToAction("TipoDespesaInsert");
            }
        }

        // GET: /EDIT
        public ActionResult TipoDespesaEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                TipoDespesa grupo = null;

                oCadastroBasico.InfoTipoDespesa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigo: codigo,
                                                oTipoDespesa: ref grupo);

                if (grupo == null)
                {
                    return HttpNotFound();
                }

                return View(grupo);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TipoDespesaEdit(int codigo_unidade, string codigo_tipo_despesa, string descricao, int codigo, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateTipoDespesa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    sCodigoTipoDespesa: codigo_tipo_despesa,
                                                    sDescricao: descricao,
                                                    bAtivo: ativo,
                                                    iCodigo: codigo);

                //Redireciona para Index
                return RedirectToAction("TipoDespesaIndex");
            }
        }

        // GET: /DELETE
        public ActionResult TipoDespesaDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                TipoDespesa grupo = null;

                oCadastroBasico.InfoTipoDespesa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigo: codigo,
                                                oTipoDespesa: ref grupo);

                if (grupo == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(grupo);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TipoDespesaDelete([Bind(Include = "codigo")] TipoDespesa tipo_despesa)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteTipoDespesa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigo: tipo_despesa.codigo);

                    //Redireciona para Index
                    return RedirectToAction("TipoDespesaIndex");
                }
                catch
                {
                    return TipoDespesaDelete(codigo: tipo_despesa.codigo,
                                                erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA FUNÇÃO
        public JsonResult ValidaTipoDespesa(int unidade, string codigo_tipo_despesa, int codigo)
        {

            return Json(oCadastroBasico.ValidaTipoDespesa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            sCodigoTipoDespesa: codigo_tipo_despesa,
                                                            iCodigo: codigo));

        }

        #endregion

        #region ::: TREINAMENTO :::

        // GET: INDEX
        public ActionResult TreinamentoIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_treinamento",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                if (editar || excluir)
                    if (Session["unidade"].ToString() == "")
                        ViewBag.columnDefs = "columnDefs: [{ orderable: false, targets: [3] }], order: [[0]]";
                    else
                        ViewBag.columnDefs = "columnDefs: [{ orderable: false, targets: [2] }], order: [[0]]";
                else
                    ViewBag.columnDefs = "order: [[0]]";

                return View(oCadastroBasico.IndexTreinamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"]),
                                                                iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])));
            }
        }

        // GET: INSERT
        public ActionResult TreinamentoInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", Session["codigo_modulo"].ToString());

                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult TreinamentoInsert(string descricao, string comentario, HttpPostedFileBase arquivo, bool ativo = false, int unidade = -1, int modulo = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                string sPath = "";
                string sFileName = "";

                if (arquivo != null)
                {
                    sPath = Server.MapPath(Path.Combine("~/Content/arq/Treinamento", Session["empresa"].ToString()));
                    sFileName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(arquivo.FileName);
                    if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                    if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                    arquivo.SaveAs(Path.Combine(sPath, sFileName));
                }


                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertTreinamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: unidade,
                                                    iCodigoModulo: modulo,
                                                    sDescricao: descricao,
                                                    sComentario: comentario,
                                                    sPathArquivo: (sFileName == "") ? "" : Path.Combine("~/Content/arq/Treinamento", Session["empresa"].ToString(), sFileName),
                                                    sArquivo: (arquivo != null) ? arquivo.FileName : "",
                                                    bAtivo: ativo);

                return RedirectToAction("TreinamentoInsert");
            }
        }

        // GET: /EDIT
        public ActionResult TreinamentoEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Treinamento treinamento = null;

                oCadastroBasico.InfoTreinamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigo: codigo,
                                                oTreinamento: ref treinamento);

                if (treinamento == null)
                {
                    return HttpNotFound();
                }

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", treinamento.codigo_unidade);
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), treinamento.codigo_modulo);
                ViewBag.arquivo = treinamento.arquivo;

                return View(treinamento);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TreinamentoEdit(string descricao, string comentario, int codigo, HttpPostedFileBase arquivo, string change_arquivo, bool ativo = false, int unidade = -1, int modulo = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                string sPath = "";
                string sFileName = "";

                if (change_arquivo == "change")
                {
                    if (arquivo != null)
                    {
                        sPath = Server.MapPath(Path.Combine("~/Content/arq/Treinamento", Session["empresa"].ToString()));
                        sFileName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(arquivo.FileName);
                        if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                        if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                        arquivo.SaveAs(Path.Combine(sPath, sFileName));
                    }

                }

                //Insere Registro no Banco de Dados
                oCadastroBasico.UpdateTreinamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: unidade,
                                                    iCodigoModulo: modulo,
                                                    sDescricao: descricao,
                                                    sComentario: comentario,
                                                    sArquivo: (arquivo != null) ? arquivo.FileName : "",
                                                    sPathArquivo: (sFileName == "") ? "": Path.Combine("~/Content/arq/Treinamento", Session["empresa"].ToString(), sFileName),
                                                    bAtivo: ativo,
                                                    iCodigo: codigo);

                //Redireciona para Index
                return RedirectToAction("TreinamentoIndex");
            }
        }

        // GET: /DELETE
        public ActionResult TreinamentoDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Treinamento treinamento = null;

                oCadastroBasico.InfoTreinamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigo: codigo,
                                                oTreinamento: ref treinamento);

                if (treinamento == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(treinamento);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TreinamentoDelete([Bind(Include = "codigo")] Treinamento treinamento)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Deleta Registro no Banco de Dados
                    oCadastroBasico.DeleteTreinamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigo: treinamento.codigo);

                    //Redireciona para Index
                    return RedirectToAction("TreinamentoIndex");
                }
                catch
                {
                    return TreinamentoDelete(codigo: treinamento.codigo,
                                                erro: PCM.WEB.Properties.Resources.valida_excluir);
                }

            }
        }

        //JSON: /VALIDA FUNÇÃO
        public JsonResult ValidaTreinamento(int unidade, int modulo, string descricao, int codigo)
        {

            return Json(oCadastroBasico.ValidaTreinamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            iCodigoModulo: modulo,
                                                            sDescricao: descricao,
                                                            iCodigo: codigo));

        }

        #endregion

        #region ::: UNIDADE :::

        // GET: INDEX
        public ActionResult UnidadeIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_unidade",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                return View(oCadastroBasico.IndexUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()), 
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())));
            }
        }

        // GET: /INSERT
        public ActionResult UnidadeInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.uf = new SelectList(oCombo.UF(), "codigo", "descricao", null);
                ViewBag.tipo_unidade = new SelectList(oCombo.TipoUnidade(), "codigo", "descricao", null);

                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult UnidadeInsert(string nome_fantasia, string razao_social, string cnpj, string inscricao_estadual, string inscricao_municipal, string cep, string uf, string municipio, string logradouro, string numero, string bairro, string complemento, string telefone, int quantidade_bloco, int quantidade_andar, int tipo_unidade, string hotel_opera, HttpPostedFileBase logoMin, HttpPostedFileBase logoMax, bool aponta_horas = false, bool aponta_horas_qualidade = false, string area_total = "0", string area_total_construida = "0", int quantidae_maxima_horas_apontamento = 0, bool ativo = false, string action = "UnidadeIndex")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                string sPath = "";
                string sFileName = "";
                string sLogoMinPath = "";
                string sLogoMaxPath = "";

                if (logoMin != null)
                {
                    FileInfo fileLogoMin = new FileInfo(logoMin.FileName);
                    sPath = Server.MapPath(Path.Combine("~/Content/img/Cliente/Icons", Session["empresa"].ToString()));
                    sFileName = "min_" + cnpj.Replace(".", "").Replace("/", "").Replace("-", "") + fileLogoMin.Extension;
                    if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                    if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                    sLogoMinPath = Path.Combine(sPath, sFileName);
                    logoMin.SaveAs(Path.Combine(sPath, sFileName));
                }

                if (logoMax != null)
                {
                    FileInfo fileLogoMax = new FileInfo(logoMax.FileName);
                    sPath = Server.MapPath(Path.Combine("~/Content/img/Cliente/Icons", Session["empresa"].ToString()));
                    sFileName = "max_" + cnpj.Replace(".", "").Replace("/", "").Replace("-", "") + fileLogoMax.Extension;
                    if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                    if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                    sLogoMaxPath = Path.Combine(sPath, sFileName);
                    logoMin.SaveAs(Path.Combine(sPath, sFileName));
                }

                int codigo_unidade = 0;

                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                sNomeFantasia: nome_fantasia,
                                                sRazaoSocial: razao_social,
                                                sCNPJ: cnpj,
                                                sInscricaoEstadual: inscricao_estadual,
                                                sInscricaoMunicipal: inscricao_municipal,
                                                sCEP: cep,
                                                sUF: uf,
                                                sMunicipio: municipio,
                                                sLogradouro: logradouro,
                                                sNumero: numero,
                                                sBairro: bairro,
                                                sComplemento: complemento,
                                                sTelefone: telefone,
                                                iQuantidadeBloco: quantidade_bloco,
                                                iQuantidadeAndar: quantidade_andar,
                                                sLogoMin: sLogoMinPath,
                                                sLogoMax: sLogoMaxPath,
                                                bApontaHoras: aponta_horas,
                                                bApontaHorasQualidade: aponta_horas_qualidade,
                                                iQuantidadeMaximaHorasApontamento: quantidae_maxima_horas_apontamento,
                                                dAreaTotal: Convert.ToDouble((area_total == "")? "0" : area_total),
                                                dAreaTotalConstruida: Convert.ToDouble((area_total_construida == "") ? "0" : area_total_construida),
                                                iCodigoTipoUnidade: tipo_unidade,
                                                bAtivo: ativo,
                                                sHotelOpera: hotel_opera,
                                                iCodigoUnidade: ref codigo_unidade);

                if(action == "SetorInsert")
                {
                    Session["codigo_unidade"] = codigo_unidade;
                    Session["unidade"] = nome_fantasia.ToUpper();
                    Session["unidade_descricao"] = nome_fantasia.ToUpper();
                }

                return RedirectToAction(action);
            }
        }

        // GET: /EDIT
        public ActionResult UnidadeEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Unidade unidade = null;

                oCadastroBasico.InfoUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigo: codigo,
                                            oUnidade: ref unidade);

                if (unidade == null)
                {
                    return HttpNotFound();
                }

                ViewBag.uf = new SelectList(oCombo.UF(), "codigo", "descricao", unidade.uf);
                ViewBag.tipo_unidade = new SelectList(oCombo.TipoUnidade(), "codigo", "descricao", unidade.codigo_tipo_unidade);
                ViewBag.arquivo = unidade.arquivo;

                return View(unidade);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnidadeEdit(string nome_fantasia, string razao_social, string cnpj, string inscricao_estadual, string inscricao_municipal, string cep, string uf, string municipio, string logradouro, string numero, string bairro, string complemento, string telefone, int quantidade_bloco, int quantidade_andar, int tipo_unidade, string hotel_opera, HttpPostedFileBase logoMin, HttpPostedFileBase logoMax, int codigo, string change_imagem, bool aponta_horas = false, bool aponta_horas_qualidade = false, string area_total = "0", string area_total_construida = "0", int quantidae_maxima_horas_apontamento = 0, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                string sPath = "";
                string sFileName = "";
                string sLogoMinPath = "";
                string sLogoMaxPath = "";

                if (logoMin != null)
                {
                    FileInfo fileLogoMin = new FileInfo(logoMin.FileName);
                    sPath = Server.MapPath(Path.Combine("~/Content/img/Cliente/Icons", Session["empresa"].ToString()));
                    sFileName = "min_" + cnpj.Replace(".", "").Replace("/", "").Replace("-", "") + fileLogoMin.Extension;
                    if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                    if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                    sLogoMinPath = Path.Combine(sPath, sFileName);
                    logoMin.SaveAs(Path.Combine(sPath, sFileName));
                }

                if (logoMax != null)
                {
                    FileInfo fileLogoMax = new FileInfo(logoMax.FileName);
                    sPath = Server.MapPath(Path.Combine("~/Content/img/Cliente/Icons", Session["empresa"].ToString()));
                    sFileName = "max_" + cnpj.Replace(".", "").Replace("/", "").Replace("-", "") + fileLogoMax.Extension;
                    if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                    if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                    sLogoMaxPath = Path.Combine(sPath, sFileName);
                    logoMin.SaveAs(Path.Combine(sPath, sFileName));
                }

                //Altera Registro no Banco de Dados
                oCadastroBasico.UpdateUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                sNomeFantasia: nome_fantasia,
                                                sRazaoSocial: razao_social,
                                                sCNPJ: cnpj,
                                                sInscricaoEstadual: inscricao_estadual,
                                                sInscricaoMunicipal: inscricao_municipal,
                                                sCEP: cep,
                                                sUF: uf,
                                                sMunicipio: municipio,
                                                sLogradouro: logradouro,
                                                sNumero: numero,
                                                sBairro: bairro,
                                                sComplemento: complemento,
                                                sTelefone: telefone,
                                                iQuantidadeBloco: quantidade_bloco,
                                                iQuantidadeAndar: quantidade_andar,
                                                sLogoMin: sLogoMinPath,
                                                sLogoMax: sLogoMaxPath,
                                                bApontaHoras: aponta_horas,
                                                bApontaHorasQualidade: aponta_horas_qualidade,
                                                iQuantidadeMaximaHorasApontamento: quantidae_maxima_horas_apontamento,
                                                dAreaTotal: Convert.ToDouble((area_total == "") ? "0" : area_total),
                                                dAreaTotalConstruida: Convert.ToDouble((area_total_construida == "") ? "0" : area_total_construida),
                                                iCodigoTipoUnidade: tipo_unidade,
                                                bAtivo: ativo,
                                                sHotelOpera: hotel_opera,
                                                iCodigo: codigo);

                //Redireciona para Index
                return RedirectToAction("UnidadeIndex");
            }
        }

        // GET: /DELETE
        public ActionResult UnidadeDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Unidade unidade = null;

                oCadastroBasico.InfoUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigo: codigo,
                                            oUnidade: ref unidade);

                if (unidade == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(unidade);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnidadeDelete([Bind(Include = "codigo")] Unidade unidade)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oCadastroBasico.DeleteUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigo: unidade.codigo);

                    //Redireciona para Index
                    return RedirectToAction("UnidadeIndex");
                }
                catch
                {
                    return UnidadeDelete(codigo: unidade.codigo,
                                            erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA
        public JsonResult ValidaUnidade(string cnpj, int codigo)
        {

            return Json(oCadastroBasico.ValidaUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigo: codigo,
                                                        sCNPJ: cnpj));

        }

        #endregion


        #region ::: PRODUCT - GROUP :::

        // GET: INDEX
        public ActionResult GrupoProdutoIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_grupo_produto",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
               
                
                return View();
            }
        }

        //JSON: /COMPANY
        public JsonResult LoadGrupoProduto(int unidade, string descricao)
        {
            return Json(oCadastroBasico.IndexGrupoProduto(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                          codigoUnidade: unidade,
                                                          descricao: descricao));

        }

        // GET: INSERT
        public ActionResult GrupoProdutoInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());


                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult GrupoProdutoInsert(int unidade, string descricao, bool ativo = false)
        {
            if (Session["language"] == null || User.Identity.GetUserName() == "")
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertGrupoProduto(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                   codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                   codigoUnidade: unidade,
                                                   descricao: descricao,
                                                   ativo: ativo);

                return RedirectToAction("GrupoProdutoInsert");
            }
        }

        // GET: /EDIT
        public ActionResult GrupoProdutoEdit(int codigo)
        {
            if (Session["language"] == null || User.Identity.GetUserName() == "")
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                GrupoProduto grupoProduto = oCadastroBasico.InfoGrupoProduto(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                             codigo: codigo);

                if (grupoProduto == null)
                {
                    return HttpNotFound();
                }

                return View(grupoProduto);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GrupoProdutoEdit(int unidade, string descricao, int codigo, bool ativo = false)
        {
            if (Session["language"] == null || User.Identity.GetUserName() == "")
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Altera Registro no Banco de Dados
                oCadastroBasico.UpdateGrupoProduto(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                   codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                   codigoUnidade: unidade,
                                                   descricao: descricao,
                                                   ativo: ativo,
                                                   codigo: codigo);

                //Redireciona para Index
                return RedirectToAction("GrupoProdutoIndex");
            }
        }

        //JSON: /DELETE
        public JsonResult GrupoProdutoDelete(int codigo)
        {
            try
            {
                oCadastroBasico.DeleteGrupoProduto(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                   codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()), 
                                                   codigo: codigo);

                return Json(PCM.WEB.Properties.Resources.register_deleted);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        //JSON: /VALIDATE
        public JsonResult ValidaGrupoProduto(int unidade, string descricao, int codigo)
        {

            return Json(oCadastroBasico.ValidaGrupoProduto(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                           codigoUnidade: unidade,
                                                           descricao: descricao,
                                                           codigo: codigo)); 

        }

        #endregion

        #region ::: PRODUTO :::

        // GET: INDEX
        public ActionResult ProdutoIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_produto",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.grupoProduto = new SelectList(oCombo.GrupoProduto(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                          iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", -1);

                return View();
            }
        }

        //JSON: /COMPANY
        public JsonResult LoadProduto(int unidade, int grupoProduto, string descricao)
        {
            return Json(oCadastroBasico.IndexProduto(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                     codigoUnidade: unidade,
                                                     codigoGrupoProduto: grupoProduto,
                                                     descricao: descricao));

        }

        // GET: INSERT
        public ActionResult ProdutoInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.grupoProduto = new SelectList(oCombo.GrupoProduto(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                          iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", -1);


                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult ProdutoInsert(int unidade, int grupoProduto, string codigoProduto, string descricao, string unidadeMedida, int pontoReposicao, bool controlaLote = false, bool controlaDataValidade = false, bool ativo = false)
        { 
            if (Session["language"] == null || User.Identity.GetUserName() == "")
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertProduto(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                              codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                              codigoUnidade: unidade,
                                              codigoGrupoProduto: grupoProduto,
                                              codigoProduto: codigoProduto,
                                              descricao: descricao,
                                              unidadeMedida: unidadeMedida,
                                              pontoReposicao: pontoReposicao,
                                              controlaLote: controlaLote,
                                              controlaDataValidade: controlaDataValidade,
                                              ativo: ativo);

                return RedirectToAction("ProdutoInsert");

            }
        }

        // GET: /EDIT
        public ActionResult ProdutoEdit(int codigo)
        {
            if (Session["language"] == null || User.Identity.GetUserName() == "")
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Produto produto = oCadastroBasico.InfoProduto(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                             codigo: codigo);
                ViewBag.grupoProduto = new SelectList(oCombo.GrupoProduto(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                          iCodigoUnidade: produto.codigoUnidade), "codigo", "descricao", produto.codigoGrupoProduto);

                if (produto == null)
                {
                    return HttpNotFound();
                }

                return View(produto);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProdutoEdit(int unidade, int grupoProduto, string codigoProduto, string descricao, string unidadeMedida, int pontoReposicao, int codigo, bool controlaLote = false, bool controlaDataValidade = false, bool ativo = false)
        {
            if (Session["language"] == null || User.Identity.GetUserName() == "")
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Altera Registro no Banco de Dados
                oCadastroBasico.UpdateProduto(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                              codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                              codigoUnidade: unidade,
                                              codigoGrupoProduto: grupoProduto,
                                              codigoProduto: codigoProduto,
                                              descricao: descricao,
                                              unidadeMedida: unidadeMedida,
                                              pontoReposicao: pontoReposicao,
                                              controlaLote: controlaLote,
                                              controlaDataValidade: controlaDataValidade,
                                              ativo: ativo,
                                              codigo: codigo);

                //Redireciona para Index
                return RedirectToAction("ProdutoIndex");
            }
        }

        //JSON: /DELETE
        public JsonResult ProdutoDelete(int codigo)
        {
            try
            {
                oCadastroBasico.DeleteProduto(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                              codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                              codigo: codigo);

                return Json(PCM.WEB.Properties.Resources.register_deleted);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        //JSON: /VALIDATE
        public JsonResult ValidaProduto(int unidade, string codigoProduto, int codigo)
        {

            return Json(oCadastroBasico.ValidaProduto(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                      codigoUnidade: unidade,
                                                      codigoProduto: codigoProduto,
                                                      codigo: codigo));

        }

        #endregion

        #region ::: CLIENTE :::

        // GET: INDEX
        public ActionResult ClienteIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_cliente",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());

                return View();
            }
        }

        //JSON: /COMPANY
        public JsonResult LoadCliente(int unidade, string nomeFantasia)
        {
            return Json(oCadastroBasico.IndexCliente(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                     codigoUnidade: unidade,                                                   
                                                     nomeFantasia: nomeFantasia));

        }

        // GET: INSERT
        public ActionResult ClienteInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.uf = new SelectList(oCombo.UF(), "codigo", "descricao", null);
                ViewBag.municipio = new SelectList(oCombo.Municipio(""), "codigo", "descricao", null);

                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult ClienteInsert(int unidade, string nomeFantasia, string razaoSocial, string cnpj, string inscricaoEstadual, string inscricaoMunicipal, string cep, string uf, string municipio, string logradouro, string numero, string bairro, string complemento, string telefone, string email, bool ativo = false)
        {
            if (Session["language"] == null || User.Identity.GetUserName() == "")
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertCliente(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                              codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                              codigoUnidade: unidade,
                                              nomeFantasia: nomeFantasia,
                                              razaoSocial: razaoSocial,
                                              cnpj: cnpj,
                                              inscricaoEstadual: inscricaoEstadual,
                                              inscricaoMunicipal: inscricaoMunicipal,
                                              cep: cep,
                                              uf: uf,
                                              municipio: municipio,
                                              logradouro: logradouro,
                                              numero: numero,
                                              bairro: bairro,
                                              complemento: complemento,
                                              telefone: telefone,
                                              email: email,
                                              ativo: ativo);

                return RedirectToAction("ClienteInsert");

            }
        }

        // GET: /EDIT
        public ActionResult ClienteEdit(int codigo)
        {
            if (Session["language"] == null || User.Identity.GetUserName() == "")
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Cliente cliente = oCadastroBasico.InfoCliente(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                             codigo: codigo);


                ViewBag.uf = new SelectList(oCombo.UF(), "codigo", "descricao", cliente.uf);
                ViewBag.municipio = new SelectList(oCombo.Municipio(""), "codigo", "descricao", cliente.municipio);

                if (cliente == null)
                {
                    return HttpNotFound();
                }

                return View(cliente);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClienteEdit(int unidade, string nomeFantasia, string razaoSocial, string cnpj, string inscricaoEstadual, string inscricaoMunicipal, string cep, string uf, string municipio, string logradouro, string numero, string bairro, string complemento, string telefone, string email, int codigo, bool ativo = false)
        {
            if (Session["language"] == null || User.Identity.GetUserName() == "")
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Altera Registro no Banco de Dados
                oCadastroBasico.UpdateCliente(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                              codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                              codigoUnidade: unidade,
                                              nomeFantasia: nomeFantasia,
                                              razaoSocial: razaoSocial,
                                              cnpj: cnpj,
                                              inscricaoEstadual: inscricaoEstadual,
                                              inscricaoMunicipal: inscricaoMunicipal,
                                              cep: cep,
                                              uf: uf,
                                              municipio: municipio,
                                              logradouro: logradouro,
                                              numero: numero,
                                              bairro: bairro,
                                              complemento: complemento,
                                              telefone: telefone,
                                              email: email,
                                              ativo: ativo,
                                              codigo: codigo);

                //Redireciona para Index
                return RedirectToAction("ClienteIndex");
            }
        }

        //JSON: /DELETE
        public JsonResult ClienteDelete(int codigo)
        {
            try
            {
                oCadastroBasico.DeleteCliente(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                              codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                              codigo: codigo);

                return Json(PCM.WEB.Properties.Resources.register_deleted);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        //JSON: /VALIDATE
        public JsonResult ValidaCliente(int unidade, string cnpj, int codigo)
        {

            return Json(oCadastroBasico.ValidaCliente(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                      codigoUnidade: unidade,
                                                      cnpj: cnpj,
                                                      codigo: codigo));

        }

        #endregion

        #region ::: CLIENTE - ACORDO COMERCIAL :::

        // GET: INDEX
        public ActionResult ClienteEnxoval()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_cliente_acordo_comercial",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.cliente = new SelectList(oCombo.Cliente(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                codigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.enxoval = new SelectList(oCombo.Enxoval(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                codigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);

                return View();
            }
        }

        [HttpPost]
        public JsonResult LoadClienteEnxoval(int unidade, int cliente)
        {
            return Json(oCadastroBasico.LoadClienteEnxoval(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                           codigoUnidade: unidade,
                                                           codigoCliente: cliente));
        }

        [HttpPost]
        public JsonResult InsertClienteEnxoval(int unidade, int cliente, int enxoval, int quantidade, string valorUnitario)
        {
            pwaDefaultResponse response = new pwaDefaultResponse();

            try
            {
                oCadastroBasico.InsertClienteEnxoval(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                     codigoUnidade: unidade,
                                                     codigoCliente: cliente,
                                                     codigoEnxoval: enxoval,
                                                     quantidade: quantidade,
                                                     valorUnitario: Convert.ToDouble(valorUnitario.Replace("R$ ", "").Replace(".", "").Replace(",", ".")));

                response.success = true;
                response.message = "";

            } catch (Exception ex) {

                response.success = false;
                response.message = ex.Message.ToString();
            }

            return Json(response);

        }

        [HttpPost]
        public JsonResult DeleteClienteEnxoval(int unidade, int cliente, int enxoval)
        {
            pwaDefaultResponse response = new pwaDefaultResponse();

            try
            {
                oCadastroBasico.DeleteClienteEnxoval(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                     codigoUnidade: unidade,
                                                     codigoCliente: cliente,
                                                     codigoEnxoval: enxoval);

                response.success = true;
                response.message = "";

            }
            catch (Exception ex)
            {

                response.success = false;
                response.message = ex.Message.ToString();
            }

            return Json(response);

        }

        #endregion

        //#region ::: PRODUCT - STOCK :::

        //// GET: INDEX
        //public ActionResult ProductStockIndex()
        //{
        //    if (Session["language"] == null || User.Identity.GetUserName() == "")
        //    {
        //        return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
        //    }
        //    else
        //    {

        //        //Váriaveis
        //        bool update = false;
        //        bool insert = false;
        //        bool delete = false;
        //        bool administrator = false;
        //        bool print = false;

        //        oAccount.LoadProfile(sUsername: User.Identity.GetUserName(),
        //                             sForm: "reg_product_stock",
        //                             bInsert: ref insert,
        //                             bUpdate: ref update,
        //                             bDelete: ref delete,
        //                             bAdministrator: ref administrator,
        //                             bPrint: ref print);

        //        ViewBag.branch = new SelectList(oBasicInput.Combo(sStoredProcedure: "sp_select_combo_register_branch", sCode: Session["branch"].ToString()), "code", "description", Session["branch"].ToString());
        //        ViewBag.product_group = new SelectList(oBasicInput.Combo(sStoredProcedure: "sp_select_combo_register_product_group", sCode: Session["branch"].ToString()), "code", "description", null);

        //        ViewBag.insert = insert;
        //        ViewBag.update = update;
        //        ViewBag.delete = delete;
        //        ViewBag.username = User.Identity.GetUserName();

        //        return View();
        //    }
        //}

        //// POST: INDEX
        //[HttpPost]
        //public ActionResult ProductStockIndex(string branch, string product_group, string product, string description)
        //{
        //    if (Session["language"] == null || User.Identity.GetUserName() == "")
        //    {
        //        return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
        //    }
        //    else
        //    {

        //        //Váriaveis
        //        bool update = false;
        //        bool insert = false;
        //        bool delete = false;
        //        bool administrator = false;
        //        bool print = false;

        //        oAccount.LoadProfile(sUsername: User.Identity.GetUserName(),
        //                             sForm: "reg_product_stock",
        //                             bInsert: ref insert,
        //                             bUpdate: ref update,
        //                             bDelete: ref delete,
        //                             bAdministrator: ref administrator,
        //                             bPrint: ref print);

        //        ViewBag.branch = new SelectList(oBasicInput.Combo(sStoredProcedure: "sp_select_combo_register_branch", sCode: Session["branch"].ToString()), "code", "description", branch);
        //        ViewBag.product_group = new SelectList(oBasicInput.Combo(sStoredProcedure: "sp_select_combo_register_product_group", sCode: branch), "code", "description", product_group);

        //        ViewBag.insert = insert;
        //        ViewBag.update = update;
        //        ViewBag.delete = delete;
        //        ViewBag.product = product;
        //        ViewBag.description = description;
        //        ViewBag.username = User.Identity.GetUserName();

        //        return View();
        //    }
        //}

        ////JSON: /COMPANY
        //public JsonResult LoadProductStock(string branch, string product, string description, int product_group = -1)
        //{
        //    return Json(oBasicInput.IndexProductStock(sBranch: branch,
        //                                              iProductGroupID: product_group,
        //                                              sProduct: product,
        //                                              sDescription: description));
        //}

        //// GET: INSERT
        //public ActionResult ProductStockInsert()
        //{
        //    if (Session["language"] == null || User.Identity.GetUserName() == "")
        //    {
        //        return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
        //    }
        //    else
        //    {
        //        ViewBag.branch = new SelectList(oBasicInput.Combo(sStoredProcedure: "sp_select_combo_register_branch", sCode: Session["branch"].ToString()), "code", "description", Session["branch"].ToString());
        //        ViewBag.product_group = new SelectList(oBasicInput.Combo(sStoredProcedure: "sp_select_combo_register_product_group", sCode: Session["branch"].ToString()), "code", "description", null);

        //        return View();
        //    }
        //}

        //public JsonResult LoadProductUOMList(string branch)
        //{

        //    return Json(oBasicInput.IndexProductUOMStock(sBranch: branch,
        //                                                lProductID: -1));

        //}

        //// POST: INSERT
        //[HttpPost]
        //public ActionResult ProductStockInsert(string branch, int product_group, string product, string description, int spare_point = 0, bool batch_control = false, bool expiration_date_control = false, bool quality_control = false, bool active = false, bool receive_more = false, string percentage = "0")
        //{
        //    if (Session["language"] == null || User.Identity.GetUserName() == "")
        //    {
        //        return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
        //    }
        //    else
        //    {

        //        long id = 0;

        //        //Insere Registro no Banco de Dados
        //        oBasicInput.InsertProductStock(sBranch: branch,
        //                                       iProductGroupID: product_group,
        //                                       sProduct: product,
        //                                       sDescription: description,
        //                                       iSparePoint: spare_point,
        //                                       bBatchControl: batch_control,
        //                                       bExpirationDateControl: expiration_date_control,
        //                                       bQualityControl: quality_control,
        //                                       bReceiveMore: receive_more,
        //                                       dPercentage: (percentage == "") ? 0 : Convert.ToDouble(percentage),
        //                                       bActive: active,
        //                                       sCurrentUser: User.Identity.GetUserName(),
        //                                       lID: ref id);

        //        return RedirectToAction("ProductStockInsert");
        //    }
        //}

        //// GET: /EDIT
        //public ActionResult ProductStockEdit(long id)
        //{
        //    if (Session["language"] == null || User.Identity.GetUserName() == "")
        //    {
        //        return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
        //    }
        //    else
        //    {
        //        reg_product product = oBasicInput.FindProductStock(lID: id);

        //        if (product == null)
        //        {
        //            return HttpNotFound();
        //        }

        //        ViewBag.product_group = new SelectList(oBasicInput.Combo(sStoredProcedure: "sp_select_combo_register_product_group", sCode: product.branch), "code", "description", product.product_group_id);
        //        ViewBag.product = product;

        //        return View();
        //    }
        //}

        //// POST: /EDIT
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ProductStockEdit(string branch, int product_group, string product, string description, long id, int spare_point = 0, bool batch_control = false, bool expiration_date_control = false, bool quality_control = false, bool active = false, bool receive_more = false, string percentage = "")
        //{
        //    if (Session["language"] == null || User.Identity.GetUserName() == "")
        //    {
        //        return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
        //    }
        //    else
        //    {

        //        //Altera Registro no Banco de Dados
        //        oBasicInput.UpdateProductStock(iProductGroupID: product_group,
        //                                       sProduct: product,
        //                                       sDescription: description,
        //                                       iSparePoint: spare_point,
        //                                       bBatchControl: batch_control,
        //                                       bExpirationDateControl: expiration_date_control,
        //                                       bQualityControl: quality_control,
        //                                       bReceiveMore: receive_more,
        //                                       dPercentage: (percentage == "") ? 0 : Convert.ToDouble(percentage),
        //                                       bActive: active,
        //                                       sCurrentUser: User.Identity.GetUserName(),
        //                                       lID: id);

        //        //Redireciona para Index
        //        return RedirectToAction("ProductStockIndex");
        //    }
        //}

        ////JSON: /DELETE
        //public JsonResult ProductStockDelete(long id, string username)
        //{
        //    try
        //    {
        //        oBasicInput.DeleteProductStock(lID: id,
        //                                       sCurrentUser: username);

        //        return Json(SYSPACK.WEB.Properties.Resources.register_deleted);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(ex.Message);
        //    }
        //}

        ////JSON: /VALIDATE
        //public JsonResult ValidateProductStock(string branch, string product, long id)
        //{

        //    return Json(oBasicInput.ValidateProductStock(sBranch: branch,
        //                                                 sProduct: product,
        //                                                 lID: id));

        //}

        ////JSON: /COMPANY
        //public JsonResult LoadProductUOMStock(long id)
        //{
        //    return Json(oBasicInput.IndexProductUOMStock(lProductID: id));
        //}

        //#endregion

        //#region ::: PRODUCT - STOCK - SUPPLIER :::

        ////JSON: /COMPANY
        //public JsonResult LoadProductStockSupplier(int supplier)
        //{
        //    return Json(oBasicInput.IndexProductStockSupplier(sBranch: Session["branch"].ToString(),
        //                                                      iSupplierID: supplier));
        //}

        //// GET: INSERT
        //public ActionResult ProductStockSupplierInsert(string branch = "", int supplier = -1)
        //{
        //    if (Session["language"] == null || User.Identity.GetUserName() == "")
        //    {
        //        return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
        //    }
        //    else
        //    {
        //        //Váriaveis
        //        bool update = false;
        //        bool insert = false;
        //        bool delete = false;
        //        bool administrator = false;
        //        bool print = false;

        //        oAccount.LoadProfile(sUsername: User.Identity.GetUserName(),
        //                             sForm: "reg_product_stock_supplier",
        //                             bInsert: ref insert,
        //                             bUpdate: ref update,
        //                             bDelete: ref delete,
        //                             bAdministrator: ref administrator,
        //                             bPrint: ref print);

        //        ViewBag.insert = insert;
        //        ViewBag.update = update;
        //        ViewBag.delete = delete;
        //        ViewBag.username = User.Identity.GetUserName();

        //        branch = (branch == "") ? Session["branch"].ToString() : branch;

        //        ViewBag.branch = new SelectList(oBasicInput.Combo(sStoredProcedure: "sp_select_combo_register_branch", sCode: branch), "code", "description", Session["branch"].ToString());
        //        ViewBag.supplier = new SelectList(oBasicInput.Combo(sStoredProcedure: "sp_select_combo_register_supplier", sCode: branch), "code", "description", supplier);
        //        ViewBag.product = new SelectList(oBasicInput.Combo(sStoredProcedure: "sp_select_combo_register_product_stock", sCode: branch), "code", "description", null);
        //        ViewBag.uom = new SelectList(oBasicInput.Combo(sStoredProcedure: "sp_select_combo_register_uom", sCode: branch), "code", "description", null);

        //        return View();
        //    }
        //}

        ////JSON: /DELETE
        //public JsonResult InsertProductStockSupplier(string branch, int supplier, long product, string supplier_code_product, string gtin, string username, string uom = "", string economic_batch = "0", string lead_time = "0")
        //{
        //    try
        //    {
        //        economic_batch = economic_batch.Replace(".", "").Replace(",", ".");
        //        lead_time = lead_time.Replace(".", "").Replace(",", ".");

        //        economic_batch = economic_batch.All(Char.IsNumber) && economic_batch != "" ? economic_batch : "0";
        //        lead_time = lead_time.All(Char.IsNumber) && lead_time != "" ? lead_time : "0";

        //        oBasicInput.InsertProductStockSupplier(sBranch: branch,
        //                                               iSupplierID: supplier,
        //                                               lProductID: product,
        //                                               sSupplierCodeProduct: supplier_code_product,
        //                                               sUom: uom,
        //                                               sGTIN: gtin,
        //                                               dEconomicBatch: Convert.ToDouble(economic_batch.Replace(".", "").Replace(",", ".")),
        //                                               iLeadTime: Convert.ToInt32(lead_time),
        //                                               sCurrentUser: username);

        //        return Json(1);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(ex.Message);
        //    }
        //}

        //#endregion

        //#region ::: UOM :::

        //// GET: INDEX
        //public ActionResult UOMIndex()
        //{
        //    if (Session["language"] == null || User.Identity.GetUserName() == "")
        //    {
        //        return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
        //    }
        //    else
        //    {

        //        //Váriaveis
        //        bool update = false;
        //        bool insert = false;
        //        bool delete = false;
        //        bool administrator = false;
        //        bool print = false;

        //        oAccount.LoadProfile(sUsername: User.Identity.GetUserName(),
        //                             sForm: "reg_uom",
        //                             bInsert: ref insert,
        //                             bUpdate: ref update,
        //                             bDelete: ref delete,
        //                             bAdministrator: ref administrator,
        //                             bPrint: ref print);

        //        ViewBag.branch = new SelectList(oBasicInput.Combo(sStoredProcedure: "sp_select_combo_register_branch", sCode: Session["branch"].ToString()), "code", "description", Session["branch"].ToString());

        //        ViewBag.insert = insert;
        //        ViewBag.update = update;
        //        ViewBag.delete = delete;

        //        return View();
        //    }
        //}

        //// POST: INDEX
        //[HttpPost]
        //public ActionResult UOMIndex(string branch, string uom, string description)
        //{
        //    if (Session["language"] == null || User.Identity.GetUserName() == "")
        //    {
        //        return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
        //    }
        //    else
        //    {

        //        //Váriaveis
        //        bool update = false;
        //        bool insert = false;
        //        bool delete = false;
        //        bool administrator = false;
        //        bool print = false;

        //        oAccount.LoadProfile(sUsername: User.Identity.GetUserName(),
        //                             sForm: "reg_uom",
        //                             bInsert: ref insert,
        //                             bUpdate: ref update,
        //                             bDelete: ref delete,
        //                             bAdministrator: ref administrator,
        //                             bPrint: ref print);

        //        ViewBag.branch = new SelectList(oBasicInput.Combo(sStoredProcedure: "sp_select_combo_register_branch", sCode: Session["branch"].ToString()), "code", "description", branch);

        //        ViewBag.insert = insert;
        //        ViewBag.update = update;
        //        ViewBag.delete = delete;
        //        ViewBag.uom = uom;
        //        ViewBag.description = description;

        //        return View();
        //    }
        //}

        ////JSON: /INDEX
        //public JsonResult LoadUOM(string branch, string uom, string description)
        //{
        //    return Json(oBasicInput.IndexUOM(sBranch: branch,
        //                                     sUOM: uom,
        //                                     sDescription: description));

        //}

        //// GET: INSERT
        //public ActionResult UOMInsert()
        //{
        //    if (Session["language"] == null || User.Identity.GetUserName() == "")
        //    {
        //        return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
        //    }
        //    else
        //    {
        //        ViewBag.branch = new SelectList(oBasicInput.Combo(sStoredProcedure: "sp_select_combo_register_branch", sCode: Session["branch"].ToString()), "code", "description", Session["branch"].ToString());

        //        return View();
        //    }
        //}

        //// POST: INSERT
        //[HttpPost]
        //public ActionResult UOMInsert(string branch, string uom, string description, int decimal_places = 0, bool active = false)
        //{
        //    if (Session["language"] == null || User.Identity.GetUserName() == "")
        //    {
        //        return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
        //    }
        //    else
        //    {

        //        //Insere Registro no Banco de Dados
        //        oBasicInput.InsertUOM(sBranch: branch,
        //                              sUOM: uom,
        //                              sDescription: description,
        //                              iDecimalPlaces: decimal_places,
        //                              bActive: active,
        //                              sCurrentUser: User.Identity.GetUserName());

        //        return RedirectToAction("UOMInsert");
        //    }
        //}

        //// GET: /EDIT
        //public ActionResult UOMEdit(int id)
        //{
        //    if (Session["language"] == null || User.Identity.GetUserName() == "")
        //    {
        //        return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
        //    }
        //    else
        //    {

        //        reg_uom info = oBasicInput.FindUOM(iID: id);

        //        if (info == null)
        //        {
        //            return HttpNotFound();
        //        }

        //        return View(info);
        //    }
        //}

        //// POST: /EDIT
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult UOMEdit(string branch, string uom, string description, int id, int decimal_places = 0, bool active = false)
        //{
        //    if (Session["language"] == null || User.Identity.GetUserName() == "")
        //    {
        //        return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
        //    }
        //    else
        //    {

        //        //Altera Registro no Banco de Dados
        //        oBasicInput.UpdateUOM(sBranch: branch,
        //                              sUOM: uom,
        //                              sDescription: description,
        //                              iDecimalPlaces: decimal_places,
        //                              bActive: active,
        //                              iID: id,
        //                              sCurrentUser: User.Identity.GetUserName());

        //        //Redireciona para Index
        //        return RedirectToAction("UOMIndex");
        //    }
        //}

        ////JSON: /DELETE
        //public JsonResult UOMDelete(int id, string username)
        //{
        //    try
        //    {
        //        oBasicInput.DeleteUOM(iID: id,
        //                              sCurrentUser: username);

        //        return Json(SYSPACK.WEB.Properties.Resources.register_deleted);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(ex.Message);
        //    }
        //}

        ////JSON: /VALIDATE
        //public JsonResult ValidateUOM(string branch, string uom, int id)
        //{

        //    return Json(oBasicInput.ValidateUOM(sBranch: branch,
        //                                        sUOM: uom,
        //                                        iID: id));

        //}

        //#endregion


        #region ::: ITEM OS HOSPEDE :::

        // GET: INDEX
        public ActionResult ItemOSHospedeIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cad_item_os_hospede",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());


                return View();
            }
        }

        //JSON: /COMPANY
        public JsonResult LoadItemOSHospede(int unidade, string descricao)
        {
            return Json(oCadastroBasico.IndexItemOSHospede(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                          codigoUnidade: unidade,
                                                          descricao: descricao));

        }

        // GET: INSERT
        public ActionResult ItemOSHospedeInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());


                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult ItemOSHospedeInsert(int unidade, string descricao, bool ativo = false)
        {
            if (Session["language"] == null || User.Identity.GetUserName() == "")
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertItemOSHospede(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    codigoUnidade: unidade,
                                                    descricao: descricao,
                                                    ativo: ativo);

                return RedirectToAction("ItemOSHospedeInsert");
            }
        }

        // GET: /EDIT
        public ActionResult ItemOSHospedeEdit(int codigo)
        {
            if (Session["language"] == null || User.Identity.GetUserName() == "")
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ItemOSHospede grupoProduto = oCadastroBasico.InfoItemOSHospede(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                               codigo: codigo);

                if (grupoProduto == null)
                {
                    return HttpNotFound();
                }

                return View(grupoProduto);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ItemOSHospedeEdit(int unidade, string descricao, int codigo, bool ativo = false)
        {
            if (Session["language"] == null || User.Identity.GetUserName() == "")
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Altera Registro no Banco de Dados
                oCadastroBasico.UpdateItemOSHospede(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    codigoUnidade: unidade,
                                                    descricao: descricao,
                                                    ativo: ativo,
                                                    codigo: codigo);

                //Redireciona para Index
                return RedirectToAction("ItemOSHospedeIndex");
            }
        }

        //JSON: /DELETE
        public JsonResult ItemOSHospedeDelete(int codigo)
        {
            try
            {
                oCadastroBasico.DeleteItemOSHospede(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    codigo: codigo);

                return Json(PCM.WEB.Properties.Resources.register_deleted);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        //JSON: /VALIDATE
        public JsonResult ValidaItemOSHospede(int unidade, string descricao, int codigo)
        {

            return Json(oCadastroBasico.ValidaItemOSHospede(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            codigoUnidade: unidade,
                                                            descricao: descricao,
                                                            codigo: codigo));

        }

        #endregion

    }
}