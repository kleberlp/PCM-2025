using Microsoft.AspNet.Identity;
using PCM.WEB.DAL;
using PCM.WEB.MODELS;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace PCM.WEB.Controllers
{
    public class AgendaController : Controller
    {
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Agenda oAgenda = new Agenda(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: JSON :::

        #endregion

        #region ::: CADASTRO DE ÁREA COMUM :::

            // GET: INDEX
            public ActionResult AreaComumIndex()
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
                                        sFormulario: "cad_area_comum",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;

                    return View(oAgenda.IndexAreaComum(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                       iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()), 
                                                       iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())));
                }
            }

            // GET: INSERT
            public ActionResult AreaComumInsert()
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
            public ActionResult AreaComumInsert(int unidade, string descricao, int quantidade_enventos_concorrentes, bool ativo = false)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {

                    //Insere Registro no Banco de Dados
                    oAgenda.InsertAreaComum(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                            iCodigoUnidade: unidade,
                                            sDescricao: descricao,
                                            iQuantidadeEventosConcorrentes: quantidade_enventos_concorrentes,
                                            bAtivo: ativo);

                    return RedirectToAction("AreaComumInsert");
                }
            }

            // GET: /EDIT
            public ActionResult AreaComumEdit(int codigo)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    AreaComum areaComum = oAgenda.InfoAreaComum(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigo: codigo);

                    if (areaComum == null)
                    {
                        return HttpNotFound();
                    }

                    return View(areaComum);
                }
            }

            // POST: /EDIT
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult AreaComumEdit(string descricao, int quantidade_eventos_concorrentes, int codigo, bool ativo = false)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    //Insere Registro no Banco de Dados
                    oAgenda.UpdateAreaComum(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                            sDescricao: descricao,
                                            iQuantidadeEventosConcorrentes: quantidade_eventos_concorrentes,
                                            bAtivo: ativo,
                                            iCodigo: codigo);

                    //Redireciona para Index
                    return RedirectToAction("AreaComumIndex");
                }
            }

            // GET: /DELETE
            public ActionResult AreaComumDelete(int codigo, string erro = "")
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    AreaComum areaComum = oAgenda.InfoAreaComum(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigo: codigo);
                    
                    if (areaComum == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.erro = erro;
                    return View(areaComum);
                }
            }

            // POST: /DELETE
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult AreaComumDelete([Bind(Include = "codigo")] AreaComum areaComum)
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
                        oAgenda.DeleteAreaComum(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                iCodigo: areaComum.codigo);

                        //Redireciona para Index
                        return RedirectToAction("AreaComumIndex");
                    }
                    catch
                    {
                        return AreaComumDelete(codigo: areaComum.codigo,
                                               erro: PCM.WEB.Properties.Resources.valida_excluir);
                    }
                }
            }

        #endregion

        #region ::: AGENDA :::

            // GET: INDEX
            public ActionResult Agenda()
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
                                        sFormulario: "agenda",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;

                    return View();
                }
            }

        #endregion

    }
}