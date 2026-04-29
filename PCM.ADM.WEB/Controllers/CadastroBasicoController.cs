using System;
using System.Configuration;
using System.Web.Mvc;
using PCM.WEB.MODELS;
using PCM.WEB.DAL;
using Microsoft.AspNet.Identity;
using PCM.ADM.WEB.Class;
using System.Web;
using System.IO;
using System.Web.UI.WebControls;

namespace PCM.ADM.WEB.Controllers
{
    public class CadastroBasicoController : Controller
    {
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnectionAdm"].ConnectionString);
        private SIM oSIM = new SIM(ConfigurationManager.ConnectionStrings["DefaultConnectionAdm"].ConnectionString);
        private Administracao oAdministracao = new Administracao(ConfigurationManager.ConnectionStrings["DefaultConnectionAdm"].ConnectionString);
        private CadastroBasico oCadastroBasico = new CadastroBasico(ConfigurationManager.ConnectionStrings["DefaultConnectionAdm"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnectionAdm"].ConnectionString);
        
        #region ::: JSON :::

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

            //JSON: /VALIDA USUARIO
            public JsonResult ValidaUsuario(string email, int codigo = 0)
            {
                if (codigo == 0)
                {
                    return Json(oAdministracao.ValidaUsuario(codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                             email: email));
                }
                else
                {
                    return Json(oAdministracao.ValidaUsuario(codigoUsuario: codigo,
                                                             email: email));
                }
            }

        #endregion

        #region ::: EMPRESA :::

            // GET: INDEX
            public ActionResult EmpresaIndex()
                {
                    if (Session["empresa"] == null)
                    {
                        return RedirectToAction("Login", "Account");
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
                                            sFormulario: "cad_empresa",
                                            bInserir: ref inserir,
                                            bEditar: ref editar,
                                            bExcluir: ref excluir,
                                            bAdministrador: ref administrador);

                        ViewBag.inserir = inserir;
                        ViewBag.editar = editar;
                        ViewBag.excluir = excluir;

                        return View(oSIM.IndexEmpresa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())));
                    }
                }

            // GET: /INSERT
            public ActionResult EmpresaInsert()
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ViewBag.uf = new SelectList(oCombo.UF(), "codigo", "descricao", null);
                    ViewBag.tipo_empresa = new SelectList(oCombo.TipoEmpresa(), "codigo", "descricao", null);

                    return View();
                }
            }

            // POST: INSERT
            [HttpPost]
            public ActionResult EmpresaInsert(int tipo_empresa, 
                                              string nome_fantasia, 
                                              string cnpj, 
                                              string inscricao_estadual, 
                                              string inscricao_municipal, 
                                              string cep, 
                                              string uf, 
                                              string municipio, 
                                              string logradouro, 
                                              string numero, 
                                              string bairro, 
                                              string complemento, 
                                              string telefone, 
                                              string valor, 
                                              string data_inicio, 
                                              string url, 
                                              string email, 
                                              string senha,
                                              HttpPostedFileBase logoMin,
                                              HttpPostedFileBase logoMax,
                                              bool ativo = false,
                                              bool auditoria = false,
                                              bool aeb = false,
                                              bool estoque = false,
                                              bool financas = false,
                                              bool governanca = false,
                                              bool green_planet = false,
                                              bool log_book = false,
                                              bool laudo = false,
                                              bool ordem_servico = false,
                                              bool preventiva = false,
                                              bool rotina = false,
                                              bool pmoc = false,
                                              bool uh = false)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account");
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

                //Insere Registro no Banco de Dados
                oSIM.InsertEmpresa(iCodigoTipoEmpresa: tipo_empresa,
                                       sNomeFantasia: nome_fantasia.ToUpper(),
                                       sCNPJ: cnpj,
                                       sInscricaoEstadual: inscricao_estadual,
                                       sInscricaoMunicipal: inscricao_municipal,
                                       sCEP: cep,
                                       sUF: uf,
                                       sMunicipio: municipio.ToUpper(),
                                       sLogradouro: logradouro.ToUpper(),
                                       sNumero: numero.ToUpper(),
                                       sBairro: bairro.ToUpper(),
                                       sComplemento: complemento.ToUpper(),
                                       sTelefone: telefone,
                                       dValor: Convert.ToDouble(valor.ToString().Replace("R$ ", "")),
                                       sDataInicio: data_inicio,
                                       sURL: url,
                                       sEmail: email.ToLower(),
                                       sSenha: senha,
                                       bAtivo: ativo,
                                       sLogoMin: sLogoMinPath,
                                       sLogoMax: sLogoMaxPath,
                                       bAuditoria:auditoria,
                                       bAEB:aeb,
                                       bEstoque: estoque,
                                       bFinancas:financas,
                                       bGovernanca:governanca,
                                       bGreenPlanet:green_planet,
                                       bLogBook:log_book,
                                       bLaudo:laudo,
                                       bOrdemServico:ordem_servico,
                                       bPreventiva:preventiva,
                                       bRotina:rotina,
                                       bPMOC:pmoc,
                                       bUH: uh);

                    IISHelper.CreateSite(siteName: "PCM", path: "C:\\SIM\\PCM\\SITE\\APP");
                    IISHelper.CreateApplication(siteName: "PCM", applicationName: url, path: "C:\\SIM\\PCM\\SITE\\APP");
                    IISHelper.SetApplicationApplicationPool(siteName: url, applicationPoolName: "DefaultAppPool");

                    return RedirectToAction("EmpresaInsert");
                }
            }

            // GET: /EDIT
            public ActionResult EmpresaEdit(int codigo)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    Empresa empresa = null;

                    oSIM.InfoEmpresa(iCodigo: codigo,
                                     oEmpresa: ref empresa);

                    if (empresa == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.uf = new SelectList(oCombo.UF(), "codigo", "descricao", empresa.uf);
                    ViewBag.tipo_empresa = new SelectList(oCombo.TipoEmpresa(), "codigo", "descricao", empresa.codigo_tipo_empresa);

                    return View(empresa);
                }
            }

            // POST: /EDIT
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult EmpresaEdit(int tipo_empresa,
                                            string nome_fantasia,
                                            string cnpj,
                                            string inscricao_estadual,
                                            string inscricao_municipal,
                                            string cep,
                                            string uf,
                                            string municipio,
                                            string logradouro,
                                            string numero,
                                            string bairro,
                                            string complemento,
                                            string telefone,
                                            string valor,
                                            string data_inicio, 
                                            int codigo,
                                            HttpPostedFileBase logoMin,
                                            HttpPostedFileBase logoMax,
                                            bool ativo = false)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account");
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

                //Atualiza Registro no Banco de Dados
                oSIM.UpdateEmpresa(iCodigoTipoEmpresa: tipo_empresa,
                                   sNomeFantasia: nome_fantasia.ToUpper(),
                                   sCNPJ: cnpj,
                                   sInscricaoEstadual: inscricao_estadual,
                                   sInscricaoMunicipal: inscricao_municipal,
                                   sCEP: cep,
                                   sUF: uf,
                                   sMunicipio: municipio.ToUpper(),
                                   sLogradouro: logradouro.ToUpper(),
                                   sNumero: numero.ToUpper(),
                                   sBairro: bairro.ToUpper(),
                                   sComplemento: complemento.ToUpper(),
                                   sTelefone: telefone,
                                   dValor: Convert.ToDouble(valor.ToString().Replace("R$ ", "")),
                                   sDataInicio: data_inicio,
                                   bAtivo: ativo,
                                   sLogoMin: sLogoMinPath,
                                   sLogoMax: sLogoMaxPath,
                                   iCodigo: codigo);
                
                    //Redireciona para Index
                    return RedirectToAction("EmpresaIndex");
                }
            }

            //JSON: /VALIDA
            public JsonResult ValidaEmpresa(string cnpj, int codigo)
            {

                return Json(oSIM.ValidaEmpresa(iCodigo: codigo,
                                               sCNPJ: cnpj));

            }

            // GET: /DELETE
            public ActionResult EmpresaDelete(int codigo, string url, string erro = "")
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    Empresa empresa = null;

                    oSIM.InfoEmpresa(iCodigo: codigo,
                                     oEmpresa: ref empresa);

                    if (empresa == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.erro = erro;
                    return View(empresa);
                }
            }

            // POST: /DELETE
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult EmpresaDelete(int codigo, string url)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {

                    try
                    {
                        //Insere Registro no Banco de Dados
                        oSIM.DeleteEmpresa(iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                           iCodigo: codigo);

                        //IISHelper.DropApplication(siteName: "PCM", applicationName:url);

                        //Redireciona para Index
                        return RedirectToAction("EmpresaIndex");
                    }
                    catch
                    {
                        return EmpresaDelete(codigo: codigo,
                                             url: url,
                                             erro: PCM.ADM.WEB.Properties.Resources.valida_excluir); ;
                    }

                }

            }

        #endregion

    }
}