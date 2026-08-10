using Microsoft.AspNet.Identity;
using PCM.WEB.DAL;
using PCM.WEB.MODELS;
using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;


namespace PCM.WEB.Controllers
{
    public class AEBController : Controller
    {
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private AEB oAEB = new AEB(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: JSON :::

            //JSON: /UNIDADE/
            public JsonResult LoadUnidade()
            {
                return Json(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                           iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                           bCadastro: false));
            }

            //JSON: /PROGRAMADA/
            public JsonResult LoadProgramada(int unidade, int tipo_ordem_servico)
            {
                return Json(oCombo.Programada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                              iCodigoUnidade: unidade,
                                              iCodigoTipoOrdemServico: tipo_ordem_servico));
            }

            //JSON: /FORNECEDOR/
            public JsonResult LoadFornecedor(int unidade)
            {
                return Json(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                              iCodigoUnidade: unidade));
            }

            //JSON: /EQUIPAMENTO/
            public JsonResult LoadEquipamentoLaudo(int unidade, long programada)
            {
                return Json(oCombo.EquipamentoAEB(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                  iCodigoUnidade: unidade,
                                                  lCodigoProgramada: programada));
            }

        #endregion

        #region ::: AUDITORIA EXTERNA :::

            // GET: INDEX
            public ActionResult AuditoriaExternaIndex()
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
                                        sFormulario: "aeb_auditoria_externa",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;

                    string data_inicio = DateTime.Now.AddYears(-1).ToShortDateString();
                    string data_termino = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();

                    ViewBag.data_inicio = data_inicio;
                    ViewBag.data_termino = data_termino;
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                    ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                          iCodigoUnidade: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", null);

                    return View(oAEB.IndexAuditoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"]),
                                                    sDataInicio: data_inicio,
                                                    sDataTermino: data_termino,
                                                    iCodigoFornecedor: -1));
                }
            }

            // POST: INDEX
            [HttpPost]
            public ActionResult AuditoriaExternaIndex(int unidade = -1, int fornecedor = -1, string data_inicio = "", string data_termino = "")
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
                                        sFormulario: "aeb_auditoria_externa",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;

                    ViewBag.data_inicio = data_inicio;
                    ViewBag.data_termino = data_termino;
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: false), "codigo", "descricao", unidade);
                    ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                          iCodigoUnidade: unidade), "codigo", "descricao", fornecedor);

                    return View(oAEB.IndexAuditoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: unidade,
                                                    sDataInicio: data_inicio,
                                                    sDataTermino: data_termino,
                                                    iCodigoFornecedor: fornecedor));
                }
            }

            // GET: INSERT
            public ActionResult AuditoriaExternaInsert()
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
                    ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                          iCodigoUnidade: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", null);

                    return View();
                }
            }

            // POST: INSERT
            [HttpPost]
            public ActionResult AuditoriaExternaInsert(int unidade, int fornecedor, string descricao, string data, string data_validade, HttpPostedFileBase arquivo, string valor = "0")
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
                        sPath = Server.MapPath(Path.Combine("~/Content/arq/AEB/AuditoriaExterna", Session["empresa"].ToString()));
                        sFileName = DateTime.Now.ToString("yyMMdd_HHmmss_") + arquivo.FileName.Replace("+", "").Replace("-", "").Replace(" ", "");
                        if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                        if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                        arquivo.SaveAs(Path.Combine(sPath, sFileName));
                    }

                    //Insere Registro no Banco de Dados
                    oAEB.InsertAuditoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                         iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                         iCodigoUnidade: unidade,
                                         iCodigoFornecedor: fornecedor,
                                         sDescricao: descricao,
                                         sData: data,
                                         sDataValidade: data_validade,
                                         sValor: valor,
                                         sPathArquivo: Path.Combine("~/Content/arq/AEB/AuditoriaExterna", Session["empresa"].ToString(), sFileName),
                                         sArquivo: (arquivo != null) ? arquivo.FileName : "");


                    return RedirectToAction("AuditoriaExternaIndex");
                }
            }

            // GET: /DELETE
            public ActionResult AuditoriaExternaDelete(int unidade, long codigo, string erro = "")
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    AuditoriaExterna Auditoria = null;

                    oAEB.InfoAuditoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                       iCodigoUnidade: unidade,
                                       lCodigo: codigo,
                                       oAuditoria: ref Auditoria);

                    if (Auditoria == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.erro = erro;
                    return View(Auditoria);
                }
            }

        #endregion

        #region ::: LAUDOS :::

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
                                        sFormulario: "aeb_laudo",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;

                    string data_inicio = DateTime.Now.AddYears(-1).ToShortDateString();
                    string data_termino = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();

                    ViewBag.data_inicio = data_inicio;
                    ViewBag.data_termino = data_termino;
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                    ViewBag.programada = new SelectList(oCombo.Programada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                            iCodigoTipoOrdemServico: 6), "codigo", "descricao", null);
                    ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                    ViewBag.equipamento = new SelectList(oCombo.EquipamentoAEB(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                               iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                               lCodigoProgramada: 0), "codigo", "descricao", null);

                    return View(oAEB.IndexLaudo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"]),
                                                sDataInicio: data_inicio,
                                                sDataTermino: data_termino,
                                                lCodigoPCMProgramada: -1,
                                                iCodigoFornecedor: -1,
                                                lCodigoEquipamento: -1));
                }
            }
        
            // POST: INDEX
            [HttpPost]
            public ActionResult LaudoIndex(int unidade = -1, string data_inicio = "", string data_termino = "", long programada = -1, int fornecedor = -1, long equipamento = -1)
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
                                        sFormulario: "aeb_laudo",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;

                    ViewBag.data_inicio = data_inicio;
                    ViewBag.data_termino = data_termino;
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()), 
                                                                    bCadastro: false), "codigo", "descricao", unidade);
                    ViewBag.programada = new SelectList(oCombo.Programada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                          iCodigoUnidade: unidade,
                                                                          iCodigoTipoOrdemServico: 6), "codigo", "descricao", programada);
                    ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                          iCodigoUnidade: unidade), "codigo", "descricao", fornecedor);
                    ViewBag.equipamento = new SelectList(oCombo.EquipamentoAEB(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                               iCodigoUnidade: unidade,
                                                                               lCodigoProgramada: programada), "codigo", "descricao", equipamento);

                    return View(oAEB.IndexLaudo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: unidade,
                                                sDataInicio: data_inicio,
                                                sDataTermino: data_termino,
                                                lCodigoPCMProgramada: programada,
                                                iCodigoFornecedor:fornecedor,
                                                lCodigoEquipamento: equipamento));
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
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                    ViewBag.programada = new SelectList(oCombo.Programada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                          iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                          iCodigoTipoOrdemServico: 6), "codigo", "descricao", null);
                    ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                          iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                    ViewBag.equipamento = new SelectList(oCombo.EquipamentoAEB(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                               iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                               lCodigoProgramada: 0), "codigo", "descricao", null);

                    return View();
                }
            }

            // POST: INSERT
            [HttpPost]
            public ActionResult LaudoInsert(int unidade, long programada, int fornecedor, string data, string data_validade, HttpPostedFileBase arquivo, string[] equipamento, string valor = "0")
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
                        sPath = Server.MapPath(Path.Combine("~/Content/arq/AEB/Laudo", Session["empresa"].ToString()));
                        sFileName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(arquivo.FileName);
                        if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                        if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                        arquivo.SaveAs(Path.Combine(sPath, sFileName));
                    }

                    //Insere Registro no Banco de Dados
                    oAEB.InsertLaudo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                     iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                     iCodigoUnidade: unidade,
                                     lCodigoPCMProgramada: programada,
                                     iCodigoFornecedor: fornecedor,
                                     sData: data,
                                     sDataValidade: data_validade,
                                     sValor: valor,
                                     sEquipamento: (equipamento == null) ? "" : string.Join(",", equipamento),
                                     sPathArquivo: Path.Combine("~/Content/arq/AEB/Laudo", Session["empresa"].ToString(), sFileName),
                                     sArquivo: (arquivo != null) ? arquivo.FileName : "");


                    return RedirectToAction("LaudoInsert");
                }
            }

            // GET: /DELETE
            public ActionResult LaudoDelete(int unidade, long codigo, string erro = "")
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    Laudo laudo = null;

                    oAEB.InfoLaudo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                   iCodigoUnidade: unidade,
                                   lCodigo: codigo,
                                   oLaudo: ref laudo);

                    if (laudo == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.erro = erro;
                    return View(laudo);
                }
            }

            // POST: /DELETE
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult LaudoDelete([Bind(Include = "codigo, codigo_unidade")] Laudo laudo)
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
                        oAEB.DeleteLaudo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                         iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                         iCodigoUnidade: laudo.codigo_unidade,
                                         lCodigo: laudo.codigo);

                        //Redireciona para Index
                        return RedirectToAction("LaudoIndex");
                    }
                    catch
                    {
                        return LaudoDelete(unidade: laudo.codigo_unidade,
                                           codigo: laudo.codigo,
                                           erro: PCM.WEB.Properties.Resources.valida_excluir);
                    }
                }
            }

        #endregion

        #region ::: NORMAS E PROCEDIMENTOS :::

            // GET: INDEX
            public ActionResult NormasProcedimentosIndex()
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
                                        sFormulario: "aeb_normas_procedimentos",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;

                    return View(oAEB.IndexNormasProcedimentos(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                              iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                              iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"])));
                }
            }

            // GET: INSERT
            public ActionResult NormasProcedimentosInsert()
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

                    return View();
                }
            }

            // POST: INSERT
            [HttpPost]
            public ActionResult NormasProcedimentosInsert(string descricao, string comentario, HttpPostedFileBase arquivo, bool ativo = false, int unidade = -1)
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
                        sPath = Server.MapPath(Path.Combine("~/Content/arq/AEB/NormasProcedimentos", Session["empresa"].ToString()));
                        sFileName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(arquivo.FileName);
                        if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                        if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                        arquivo.SaveAs(Path.Combine(sPath, sFileName));
                    }
    
                    //Insere Registro no Banco de Dados
                    oAEB.InsertNormasProcedimentos(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                   iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                   iCodigoUnidade: unidade,
                                                   sDescricao: descricao,
                                                   sComentario: comentario,                                                         
                                                   sPathArquivo: Path.Combine("~/Content/arq/AEB/NormasProcedimentos", Session["empresa"].ToString(), sFileName),
                                                   sArquivo: (arquivo != null) ? arquivo.FileName : "",
                                                   bAtivo: ativo);

                    return RedirectToAction("NormasProcedimentosInsert");
                }
            }

            // GET: /EDIT
            public ActionResult NormasProcedimentosEdit(int codigo)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    NormasProcedimentos normas_procedimentos = null;

                    oAEB.InfoNormasProcedimentos(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                 iCodigo: codigo,
                                                 oNormasProcedimentos: ref normas_procedimentos);

                    if (normas_procedimentos == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: false), "codigo", "descricao", normas_procedimentos.codigo_unidade);
                    ViewBag.arquivo = normas_procedimentos.arquivo;

                    return View(normas_procedimentos);
                }
            }

            // POST: /EDIT
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult NormasProcedimentosEdit(string descricao, string comentario, int codigo, HttpPostedFileBase arquivo, string change_arquivo, bool ativo = false, int unidade = -1)
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
                            sPath = Server.MapPath(Path.Combine("~/Content/arq/AEB/NormasProcedimentos", Session["empresa"].ToString()));
                            sFileName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(arquivo.FileName);
                            if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                            if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                            arquivo.SaveAs(Path.Combine(sPath, sFileName));
                        }

                    }

                    //Insere Registro no Banco de Dados
                    oAEB.UpdateNormasProcedimentos(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                       iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                       iCodigoUnidade: unidade,
                                                       sDescricao: descricao,
                                                       sComentario: comentario,
                                                       sArquivo: (arquivo != null) ? arquivo.FileName : "",
                                                       sPathArquivo: Path.Combine("~/Content/arq/AEB/NormasProcedimentos", Session["empresa"].ToString(), sFileName),
                                                       bAtivo: ativo,
                                                       iCodigo: codigo);

                    //Redireciona para Index
                    return RedirectToAction("NormasProcedimentosIndex");
                }
            }

            // GET: /DELETE
            public ActionResult NormasProcedimentosDelete(int codigo, string erro = "")
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    NormasProcedimentos normas_procedimentos = null;

                    oAEB.InfoNormasProcedimentos(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                 iCodigo: codigo,
                                                 oNormasProcedimentos: ref normas_procedimentos);

                    if (normas_procedimentos == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.erro = erro;
                    return View(normas_procedimentos);
                }
            }

            // POST: /DELETE
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult NormasProcedimentosDelete([Bind(Include = "codigo")] NormasProcedimentos normas_procedimentos)
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
                        oAEB.DeleteNormasProcedimentos(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                       iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                       iCodigo: normas_procedimentos.codigo);

                        //Redireciona para Index
                        return RedirectToAction("NormasProcedimentosIndex");
                    }
                    catch
                    {
                        return NormasProcedimentosDelete(codigo: normas_procedimentos.codigo,
                                                         erro: PCM.WEB.Properties.Resources.valida_excluir);
                    }

                }
            }

            //JSON: /VALIDA FUNÇÃO
            public JsonResult ValidaNormasProcedimentos(int unidade, string descricao, int codigo)
            {

                return Json(oAEB.ValidaNormasProcedimentos(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                           iCodigoUnidade: unidade,
                                                           sDescricao: descricao,
                                                           iCodigo: codigo));

            }

        #endregion


        #region ::: CONTRATOS :::

            // GET: INDEX
            public ActionResult ContratoIndex()
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
                                        sFormulario: "aeb_contrato",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;

                    return View(oAEB.IndexContrato(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                   iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                   iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"])));
                }
            }

            // GET: INSERT
            public ActionResult ContratoInsert()
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
            public ActionResult ContratoInsert(string descricao, string comentario, HttpPostedFileBase arquivo, bool ativo = true, int unidade = -1, string data_inicio = "", string data_termino = "", bool envia_email = false, int dias_alerta = 0, string email = "")
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
                        sPath = Server.MapPath(Path.Combine("~/Content/arq/AEB/Contrato", Session["empresa"].ToString()));
                        sFileName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(arquivo.FileName);
                        if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                        if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                        arquivo.SaveAs(Path.Combine(sPath, sFileName));
                    }

                    //Insere Registro no Banco de Dados
                    oAEB.InsertContrato(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        iCodigoUnidade: unidade,
                                        sDescricao: descricao,
                                        sComentario: comentario,
                                        sPathArquivo: Path.Combine("~/Content/arq/AEB/Contrato", Session["empresa"].ToString(), sFileName),
                                        sArquivo: (arquivo != null) ? arquivo.FileName : "",
                                        sDataInicio: data_inicio,
                                        sDataTermino: data_termino,
                                        bEnviaEmail: envia_email,
                                        iDiasAlerta: dias_alerta,
                                        sEmail: email,
                                        bAtivo: ativo);

                    return RedirectToAction("ContratoInsert");
                }
            }

            // GET: /EDIT
            public ActionResult ContratoEdit(int codigo)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    Contrato contrato = null;

                    oAEB.InfoContrato(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                      iCodigo: codigo,
                                      oContrato: ref contrato);

                    if (contrato == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: true), "codigo", "descricao", contrato.codigo_unidade);
                    ViewBag.arquivo = contrato.arquivo;

                    return View(contrato);
                }
            }

            // POST: /EDIT
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult ContratoEdit(string descricao, string comentario, int codigo, HttpPostedFileBase arquivo, string change_arquivo, bool ativo = true, int codigo_unidade = -1, string data_inicio = "", string data_termino = "", bool envia_email = false, int dias_alerta = 0, string email = "")
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
                            sPath = Server.MapPath(Path.Combine("~/Content/arq/AEB/Contrato", Session["empresa"].ToString()));
                            sFileName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(arquivo.FileName);
                            if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                            if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                            arquivo.SaveAs(Path.Combine(sPath, sFileName));
                        }

                    }

                    //Insere Registro no Banco de Dados
                    oAEB.UpdateContrato(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        iCodigoUnidade: codigo_unidade,
                                        sDescricao: descricao,
                                        sComentario: comentario,
                                        sArquivo: (arquivo != null) ? arquivo.FileName : "",
                                        sPathArquivo: Path.Combine("~/Content/arq/AEB/Contrato", Session["empresa"].ToString(), sFileName),
                                        bAtivo: ativo,
                                        sDataInicio: data_inicio,
                                        sDataTermino: data_termino,
                                        bEnviaEmail: envia_email,
                                        iDiasAlerta: dias_alerta,
                                        sEmail: email,
                                        iCodigo: codigo);

                    //Redireciona para Index
                    return RedirectToAction("ContratoIndex");
                }
            }

            // GET: /DELETE
            public ActionResult ContratoDelete(int codigo, string erro = "")
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    Contrato contrato = null;

                    oAEB.InfoContrato(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                      iCodigo: codigo,
                                      oContrato: ref contrato);

                    if (contrato == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.erro = erro;
                    return View(contrato);
                }
            }

            // POST: /DELETE
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult ContratoDelete([Bind(Include = "codigo")] Contrato contrato)
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
                        oAEB.DeleteContrato(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                            iCodigo: contrato.codigo);

                        //Redireciona para Index
                        return RedirectToAction("ContratoIndex");
                    }
                    catch
                    {
                        return ContratoDelete(codigo: contrato.codigo,
                                              erro: PCM.WEB.Properties.Resources.valida_excluir);
                    }

                }
            }

        #endregion

    }
}