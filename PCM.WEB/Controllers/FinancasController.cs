using System;
using System.Web;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using PCM.WEB.MODELS;
using PCM.WEB.DAL;
using Microsoft.AspNet.Identity;

namespace PCM.WEB.Controllers
{
    public class FinancasController : Controller
    {
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Financas oFinancas = new Financas(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: JSON :::

            //JSON: /UNIDADE/
            public JsonResult LoadUnidade()
            {
                return Json(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                           iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()), 
                                           bCadastro: false));
            }

            //JSON: /TIPO DE DESPESA/
            public JsonResult LoadTipoDespesa(int unidade)
            {
                return Json(oCombo.TipoDespesa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                               iCodigoUnidade: unidade));
            }

            //JSON: /FORNECEDOR/
            public JsonResult LoadFornecedor(int unidade)
            {
                return Json(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                              iCodigoUnidade: unidade));
            }

        #endregion

        #region ::: CONTROLE DE GASTOS :::

            // GET: INDEX
            public ActionResult ControleGastoIndex(int unidade = -1, int ano = -1, string a = "")
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
                                        sFormulario: "fin_controle_gasto",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;

                    if (editar && excluir)
                        ViewBag.columnDefs = "columnDefs: [{ orderable: false, targets: [0, 1] }], order: [[2]]";
                    else if (editar || excluir)
                        ViewBag.columnDefs = "columnDefs: [{ orderable: false, targets: [0] }], order: [[1]]";
                    else
                        ViewBag.columnDefs = "order: [[0]]";

                    if (unidade != -1 && ano != -1)
                    {
                        ControleGasto controle_gasto = null;

                        oFinancas.ControleGastoIndex(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                     iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                     iCodigoUnidade: unidade,
                                                     iAno: ano,
                                                     oControleGasto: ref controle_gasto);

                        if (controle_gasto == null)
                        {
                            return HttpNotFound();
                        }
                        ViewBag.mes = controle_gasto.mes;
                        ViewBag.ano = new SelectList(oCombo.Ano(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", controle_gasto.ano);
                        ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: true), "codigo", "descricao", unidade);
                        return View(controle_gasto);
                    }
                    else
                    {
                        ViewBag.mes = 13;
                        ViewBag.ano = new SelectList(oCombo.Ano(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao");
                        ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                        bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                        return View();
                    } 
                    
                    ViewBag.mes = 13;
                    ViewBag.ano = new SelectList(oCombo.Ano(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao");
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                    return View();
                }
            }

            // POST: INDEX
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult ControleGastoIndex(int unidade = 0, int ano = 0)
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
                                        sFormulario: "fin_controle_gasto",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;

                    if (editar && excluir)
                        ViewBag.columnDefs = "columnDefs: [{ orderable: false, targets: [0, 1] }], order: [[2]]";
                    else if (editar || excluir)
                        ViewBag.columnDefs = "columnDefs: [{ orderable: false, targets: [0] }], order: [[1]]";
                    else
                        ViewBag.columnDefs = "order: [[0]]";

                    ControleGasto controle_gasto = null;

                    oFinancas.ControleGastoIndex(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: unidade,
                                                    iAno: ano,
                                                    oControleGasto: ref controle_gasto);

                    if (controle_gasto == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.mes = controle_gasto.mes;
                    ViewBag.ano = new SelectList(oCombo.Ano(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", controle_gasto.ano);
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: true), "codigo", "descricao", unidade);
                    return View(controle_gasto);
                    
                }
            }

            // POST: INSERT
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult ControleGastoInsert(int unidade, int ano, float previsao_gasto_janeiro = 0, float previsao_gasto_fevereiro = 0, float previsao_gasto_marco = 0, float previsao_gasto_abril = 0, float previsao_gasto_maio = 0, float previsao_gasto_junho = 0, float previsao_gasto_julho = 0, float previsao_gasto_agosto = 0, float previsao_gasto_setembro = 0, float previsao_gasto_outubro = 0, float previsao_gasto_novembro = 0, float previsao_gasto_dezembro = 0)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                   
                    oFinancas.InsertControleGasto(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                  iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                  iCodigoUnidade: unidade,
                                                  iAno: ano,
                                                  dPrevisaoGastoJaneiro: previsao_gasto_janeiro,
                                                  dPrevisaoGastoFevereiro: previsao_gasto_fevereiro,
                                                  dPrevisaoGastoMarco: previsao_gasto_marco,
                                                  dPrevisaoGastoAbril: previsao_gasto_abril,
                                                  dPrevisaoGastoMaio: previsao_gasto_maio,
                                                  dPrevisaoGastoJunho: previsao_gasto_junho,
                                                  dPrevisaoGastoJulho: previsao_gasto_julho,
                                                  dPrevisaoGastoAgosto: previsao_gasto_agosto,
                                                  dPrevisaoGastoSetembro: previsao_gasto_setembro,
                                                  dPrevisaoGastoOutubro: previsao_gasto_outubro,
                                                  dPrevisaoGastoNovembro: previsao_gasto_novembro,
                                                  dPrevisaoGastoDezembro: previsao_gasto_dezembro);

                    return RedirectToAction("ControleGastoIndex", new { unidade = unidade, ano = ano });

                }
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
                                        sFormulario: "fin_contrato",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;

                    return View(oFinancas.IndexContrato(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
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
                        sPath = Server.MapPath(Path.Combine("~/Content/arq/Contrato", Session["empresa"].ToString()));
                        sFileName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(arquivo.FileName);
                        if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                        if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                        arquivo.SaveAs(Path.Combine(sPath, sFileName));
                    }

                    //Insere Registro no Banco de Dados
                    oFinancas.InsertContrato(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                             iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                             iCodigoUnidade: unidade,
                                             sDescricao: descricao,
                                             sComentario: comentario,
                                             sPathArquivo: Path.Combine("~/Content/arq/Contrato", Session["empresa"].ToString(), sFileName),
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

                    oFinancas.InfoContrato(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
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
                            sPath = Server.MapPath(Path.Combine("~/Content/arq/Contrato", Session["empresa"].ToString()));
                            sFileName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(arquivo.FileName);
                            if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                            if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                            arquivo.SaveAs(Path.Combine(sPath, sFileName));
                        }
                    }

                    //Insere Registro no Banco de Dados
                    oFinancas.UpdateContrato(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                             iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                             iCodigoUnidade: codigo_unidade,
                                             sDescricao: descricao,
                                             sComentario: comentario,
                                             sArquivo: (arquivo != null) ? arquivo.FileName : "",
                                             sPathArquivo: Path.Combine("~/Content/arq/Contrato", Session["empresa"].ToString(), sFileName),
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

                    oFinancas.InfoContrato(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
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
                        oFinancas.DeleteContrato(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
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

        #region ::: DESPESA :::

            // GET: INDEX
            public ActionResult DespesaIndex()
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
                                        sFormulario: "fin_input_despesa",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;

                    return View(oFinancas.IndexDespesa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                       iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                       iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"])));
                }
            }

            // GET: INSERT
            public ActionResult DespesaInsert()
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

                    ViewBag.tipo_despesa = new SelectList(oCombo.TipoDespesa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                             iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);

                    ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                          iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);

                    return View();
                }
            }

            // POST: INSERT
            [HttpPost]
            public ActionResult DespesaInsert(int unidade, string numero_documento, int fornecedor, int tipo_despesa, string descricao, string valor, HttpPostedFileBase arquivo, string data_competencia = "", string data_pagamento = "")
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
                        sPath = Server.MapPath(Path.Combine("~/Content/arq/Despesa", Session["empresa"].ToString()));
                        sFileName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(arquivo.FileName);
                        if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                        if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                        arquivo.SaveAs(Path.Combine(sPath, sFileName));
                    }

                    //Insere Registro no Banco de Dados
                    oFinancas.InsertDespesa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                            iCodigoUnidade: unidade,
                                            iCodigoTipoDespesa: tipo_despesa,
                                            iCodigoFornecedor: fornecedor,
                                            sNumeroDocumento: numero_documento,
                                            sDescricao: descricao,
                                            dValor: Convert.ToDouble(valor),
                                            sDataCompetencia: data_competencia,
                                            sDataPagamento: data_pagamento,
                                            sPathArquivo: Path.Combine("~/Content/arq/Despesa", Session["empresa"].ToString(), sFileName),
                                            sArquivo: (arquivo != null) ? arquivo.FileName : "");

                    return RedirectToAction("DespesaInsert");
                }
            }

            // GET: /EDIT
            public ActionResult DespesaEdit(int codigo)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    Despesa despesa = null;

                    oFinancas.InfoDespesa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                          lCodigo: codigo,
                                          oDespesa: ref despesa);

                    if (despesa == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: true), "codigo", "descricao", despesa.codigo_unidade);

                    ViewBag.tipo_despesa = new SelectList(oCombo.TipoDespesa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                             iCodigoUnidade: despesa.codigo_unidade), "codigo", "descricao", despesa.codigo_tipo_despesa);

                    ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                          iCodigoUnidade: despesa.codigo_unidade), "codigo", "descricao", despesa.codigo_fornecedor);
                    ViewBag.arquivo = despesa.arquivo;

                    return View(despesa);
                }
            }

            // POST: /EDIT
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult DespesaEdit(int codigo_unidade, int fornecedor, int tipo_despesa, string numero_documento, string descricao, long codigo, string valor, HttpPostedFileBase arquivo, string data_competencia = "", string data_pagamento = "")
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
                        sPath = Server.MapPath(Path.Combine("~/Content/arq/Despesa", Session["empresa"].ToString()));
                        sFileName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(arquivo.FileName);
                        if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                        if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                        arquivo.SaveAs(Path.Combine(sPath, sFileName));
                    }

                    //Atualiza Registro no Banco de Dados
                    oFinancas.UpdateDespesa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                            iCodigoUnidade: codigo_unidade,
                                            iCodigoTipoDespesa: tipo_despesa,
                                            iCodigoFornecedor: fornecedor,
                                            sNumeroDocumento: numero_documento,
                                            sDescricao: descricao,
                                            dValor: Convert.ToDouble(valor),
                                            sDataCompetencia: data_competencia,
                                            sDataPagamento: data_pagamento,
                                            sPathArquivo: Path.Combine("~/Content/arq/Despesa", Session["empresa"].ToString(), sFileName),
                                            sArquivo: (arquivo != null) ? arquivo.FileName : "",
                                            lCodigo: codigo);

                    //Redireciona para Index
                    return RedirectToAction("DespesaIndex");
                }
            }

            // GET: /DELETE
            public ActionResult DespesaDelete(long codigo, string erro = "")
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    Despesa despesa = null;

                    oFinancas.InfoDespesa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                          lCodigo: codigo,
                                          oDespesa: ref despesa);

                    if (despesa == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.erro = erro;
                    return View(despesa);
                }
            }

            // POST: /DELETE
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult DespesaDelete([Bind(Include = "codigo")] Despesa despesa)
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
                        oFinancas.DeleteDespesa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                 iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                 lCodigo: despesa.codigo);

                        //Redireciona para Index
                        return RedirectToAction("DespesaIndex");
                    }
                    catch
                    {
                        return DespesaDelete(codigo: despesa.codigo,
                                             erro: PCM.WEB.Properties.Resources.valida_excluir);
                    }

                }
            }

        #endregion

    }
}