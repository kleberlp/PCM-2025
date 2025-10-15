using System;
using System.Web;
using System.Configuration;
using System.Web.Mvc;
using PCM.WEB.DAL;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using PCM.WEB.MODELS;

namespace PCM.WEB.Controllers
{
    public class PlanoAcaoController : Controller
    {
        private const int V = 1;
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.PlanoAcao oPlanoAcao = new DAL.PlanoAcao(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.Picture oPicture = new DAL.Picture(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: JSON :::

            //JSON: /SETOR/
            public JsonResult LoadSetor(int unidade)
            {
                return Json(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                         iCodigoUnidade: unidade));
            }

            //JSON: /JUSTIFICATIVA APONTAMENTO/
            public JsonResult LoadJustificativaApontamento(int unidade)
            {
                return Json(oCombo.JustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade));
            }

            //JSON: /PRIORIDADE/
            public JsonResult LoadPrioridade(int unidade)
            {
                return Json(oCombo.Prioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                              iCodigoUnidade: unidade));
            }

            //JSON: /DEPARTAMENTO/
            public JsonResult LoadDepartamento()
            {
                return Json(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())));
            }

            //JSON: /RESPONSÁVEL - DEPARTAMENTO/
            public JsonResult LoadResponsavelDepartamento(int unidade, int departamento)
            {
                return Json(oCombo.ResponsavelDepartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                           iCodigoUnidade: unidade,
                                                           iCodigoDepartamento: departamento));
            }

            //JSON: /SOLICITANTE/
            public JsonResult LoadSolicitante(int unidade)
                {
                    return Json(oCombo.Solicitante(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: unidade));
                }
       
            //JSON: /INSERT - ORDEM DE SERVIÇO/
            public string InsertPlanoAcao(int unidade, int solicitante, int departamento, int responsavel, string data_necessidade, string descricao, int prioridade)
            {
                if (Session["empresa"] == null)
                {
                    return "";
                }
                else
                {
                    long codigo = 0;
                    string plano_acao = "";
                    string body = "";
                    string to = "";

                    oPlanoAcao.InsertPlanoAcao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                     iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                     iCodigoUnidade: unidade,
                                                     iCodigoUsuarioSolicitante: solicitante,
                                                     iCodigoDepartamento: departamento,
                                                     iCodigoResponsavel: responsavel,
                                                     sDataNecessidade: data_necessidade,
                                                     sDescricao: descricao.ToUpper(),
                                                     iCodigoPrioridade: prioridade,
                                                     lCodigo: ref codigo,
                                                     sPlanoAcao: ref plano_acao,
                                                     sTo: ref to,
                                                     sBody: ref body);

                    string filename = "";
                    string path = Path.Combine("C:\\", "SIM", "PCM", "SITE", "IMAGE", "PA", Session["empresa"].ToString(), unidade.ToString(), codigo.ToString());

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase oHttpPostedFileBase = Request.Files[i];
                        if (oHttpPostedFileBase.FileName != "")
                        {
                            filename = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(oHttpPostedFileBase.FileName);
                            if (System.IO.File.Exists(Path.Combine(path, filename))) { System.IO.File.Delete(Path.Combine(path, filename)); }
                            ResizeAndSaveImage(oHttpPostedFileBase.InputStream, Path.Combine(path, filename));

                            oPicture.InsertPicture(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: unidade,
                                                    lCodigo: codigo,
                                                    sTipo: "PA",
                                                    iCodigoItemChecklist: -1,
                                                    sImagePath: Path.Combine(path, filename));

                        }
                    }

                    //Retorna Plano de Ação
                    return plano_acao;
                }
            }

            //JSON: /UNIDADE/
            public JsonResult LoadUnidade()
            {
                return Json(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                           iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                           bCadastro: false));
            }

            //JSON: //UPDATE STATUS/
            public JsonResult UpdateStatusOS(long codigo, int unidade, int status)
            {

                //Atualiza Status
                oPlanoAcao.UpdatePlanoAcaoStatus(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                 iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                 lCodigo: codigo,
                                                 iCodigoUnidade: unidade,
                                                 iStatus: status);

                return Json(true);

            }

            //JSON: /ATUALIZA DATA DA ORDEM DE SERVIÇO/
            public bool UpdateDataPlanoAcao(long codigo, int unidade, string data = "")
            {
                return oPlanoAcao.UpdatePlanoAcaoDataNecessidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                 iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                 lCodigo: codigo,
                                                                 iCodigoUnidade: unidade,
                                                                 sDataNecessidade: data);
            }

            public void ResizeAndSaveImage(Stream imageStream, string outputFilePath)
            {

            // Ensure the directory exists
            string directoryPath = Path.GetDirectoryName(outputFilePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Load and resize the image
            using (Bitmap originalImage = new Bitmap(imageStream))
            {
                double scaleFactor = (originalImage.Width > originalImage.Height) ? 400.0 / originalImage.Width : 400.0 / originalImage.Height;
                int newWidth = (int)(originalImage.Width * scaleFactor);
                int newHeight = (int)(originalImage.Height * scaleFactor);

                using (Bitmap resizedImage = new Bitmap(newWidth, newHeight))
                {
                    using (Graphics graphics = Graphics.FromImage(resizedImage))
                    {
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                    }

                    // Save the image safely
                    resizedImage.Save(outputFilePath, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }

        #endregion

        #region ::: PLANO DE AÇÃO :::

        // GET: INDEX
        public ActionResult PlanoAcaoIndex(int status = 1)
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
                                        sFormulario: "pcm_plano_acao",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    if (Session["pa_plano_acao"] == null || status > 1)
                    {
                        string data_inicio = (status == -1) ? TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString() : "";
                        string data_termino = (status == -1) ? TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString() : "";

                        ViewBag.administrador = administrador;
                        ViewBag.inserir = inserir;
                        ViewBag.editar = editar;
                        ViewBag.excluir = excluir;
                        ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                        ViewBag.data_inicio = data_inicio;
                        ViewBag.data_termino = data_termino;
                        ViewBag.plano_acao = "";
                        ViewBag.executor = "";

                        ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                        bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                        ViewBag.solicitante = new SelectList(oCombo.Solicitante(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                        ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                  iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", null);
                        ViewBag.prioridade = new SelectList(oCombo.Prioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                              iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                        ViewBag.justificativa_apontamento = new SelectList(oCombo.JustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                        ViewBag.status = new SelectList(oCombo.StatusManutencao(), "codigo", "descricao", status);

                        ViewBag.plano_acao_status = oPlanoAcao.PlanoAcaoStatus(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                               iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                               iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()));

                        return View(oPlanoAcao.IndexPlanoAcao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                              iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                              iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                              iCodigoDepartamento: -1,
                                                              sDataInicio: data_inicio,
                                                              sDataTermino: data_termino,
                                                              sPlanoAcao: "",
                                                              iCodigoPrioridade: -1,
                                                              iCodigoUsuarioSolicitante: -1,
                                                              sExecutor: "",
                                                              iStatus: status,
                                                              iCodigoJustificativaApontamento: -1));

                    }
                    else
                    {

                        ViewBag.administrador = administrador;
                        ViewBag.inserir = inserir;
                        ViewBag.editar = editar;
                        ViewBag.excluir = excluir;
                        ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                        ViewBag.data_inicio = Session["pa_data_inicio"].ToString();
                        ViewBag.data_termino = Session["pa_data_termino"].ToString();
                        ViewBag.plano_acao = Session["pa_plano_acao"].ToString();
                        ViewBag.executor = Session["pa_executor"].ToString();

                        ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                        bCadastro: false), "codigo", "descricao", Session["pa_unidade"].ToString());
                        ViewBag.solicitante = new SelectList(oCombo.Solicitante(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                                iCodigoUnidade: Convert.ToInt32(Session["pa_unidade"].ToString())), "codigo", "descricao", Session["pa_solicitante"].ToString());
                        ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                  iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", Session["pa_departamento"].ToString());
                        ViewBag.prioridade = new SelectList(oCombo.Prioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                              iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", Session["pa_prioridade"].ToString());
                        ViewBag.justificativa_apontamento = new SelectList(oCombo.JustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", Session["pa_justificativa_apontamento"].ToString());
                        ViewBag.status = new SelectList(oCombo.StatusManutencao(), "codigo", "descricao", status);

                        ViewBag.plano_acao_status = oPlanoAcao.PlanoAcaoStatus(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                               iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                               iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()));

                        return View(oPlanoAcao.IndexPlanoAcao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                              iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                              iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                              iCodigoDepartamento: -1,
                                                              sDataInicio: Session["pa_data_inicio"].ToString(),
                                                              sDataTermino: Session["pa_data_termino"].ToString(),
                                                              sPlanoAcao: Session["pa_plano_acao"].ToString(),
                                                              iCodigoPrioridade: Convert.ToInt32(Session["pa_prioridade"].ToString()),
                                                              iCodigoUsuarioSolicitante: Convert.ToInt32(Session["pa_solicitante"].ToString()),
                                                              sExecutor: Session["pa_executor"].ToString(),
                                                              iStatus: Convert.ToInt32(Session["pa_status"].ToString()),
                                                              iCodigoJustificativaApontamento: Convert.ToInt32(Session["pa_justificativa_apontamento"].ToString())));
                    }
                }
            }

        // POST: INDEX
        [HttpPost]
        public ActionResult PlanoAcaoIndex(int unidade = -1, int departamento = -1, string data_inicio = "", string data_termino = "", string plano_acao = "", int prioridade = -1, int status = -1, string executor = "", int solicitante = -1, int justificativa_apontamento = -1)
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

                Session["pa_unidade"] = unidade;
                Session["pa_data_inicio"] = data_inicio;
                Session["pa_data_termino"] = data_termino;
                Session["pa_plano_acao"] = plano_acao;
                Session["pa_prioridade"] = prioridade;
                Session["pa_status"] = status;
                Session["pa_executor"] = executor;
                Session["pa_solicitante"] = solicitante;
                Session["pa_departamento"] = departamento;
                Session["pa_justificativa_apontamento"] = justificativa_apontamento;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "pcm_plano_acao",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.administrador = administrador;
                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;
                ViewBag.plano_acao = plano_acao;
                ViewBag.executor = executor;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao",  unidade);
                ViewBag.solicitante = new SelectList(oCombo.Solicitante(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                        iCodigoUnidade: unidade), "codigo", "descricao", solicitante);
                ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", departamento);
                ViewBag.prioridade = new SelectList(oCombo.Prioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: unidade), "codigo", "descricao", prioridade);
                ViewBag.justificativa_apontamento = new SelectList(oCombo.JustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                    iCodigoUnidade: unidade), "codigo", "descricao", justificativa_apontamento);
                ViewBag.status = new SelectList(oCombo.StatusManutencao(), "codigo", "descricao", status);

                ViewBag.plano_acao_status = oPlanoAcao.PlanoAcaoStatus(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: unidade,
                                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()));

                return View(oPlanoAcao.IndexPlanoAcao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigoUnidade:  unidade,
                                                        iCodigoDepartamento: departamento,
                                                        sDataInicio: data_inicio,
                                                        sDataTermino: data_termino,
                                                        sPlanoAcao: plano_acao,
                                                        iCodigoPrioridade: prioridade,
                                                        iCodigoUsuarioSolicitante: solicitante,
                                                        sExecutor: executor,
                                                        iStatus: status,
                                                        iCodigoJustificativaApontamento: justificativa_apontamento));
            }
        }

        // GET: Concluir Plano Ação
        public ActionResult ConcluirPlanoAcao(long codigo_pcm_plano_acao, int unidade)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {


                oPlanoAcao.UpdatePlanoAcaoStatus(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    lCodigo: codigo_pcm_plano_acao,
                                    iCodigoUnidade: unidade,
                                    iStatus: 2);

                return RedirectToAction("PlanoAcaoIndex");
            }
        }

        // GET: Reabrir Plano Ação
        public ActionResult ReabrirPlanoAcao(long codigo_pcm_plano_acao, int unidade)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                oPlanoAcao.UpdatePlanoAcaoStatus(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    lCodigo: codigo_pcm_plano_acao,
                                                    iCodigoUnidade: unidade,
                                                    iStatus: 4);

                return RedirectToAction("PlanoAcaoIndex", "PlanoAcao");
            }
        }

        // GET: INSERT
        public ActionResult PlanoAcaoInsert()
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
                ViewBag.solicitante = new SelectList(oCombo.Solicitante(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: -1), "codigo", "descricao", null);
                ViewBag.responsavel = new SelectList(oCombo.ResponsavelDepartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                    iCodigoDepartamento: -1), "codigo", "descricao", null);
                ViewBag.prioridade = new SelectList(oCombo.Prioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();

                return View();
            }
        }

        // GET: /EDIT
        public ActionResult PlanoAcaoEdit(long codigo, int unidade)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                MODELS.PlanoAcao plano_acao = null;

                oPlanoAcao.InfoPlanoAcao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            lCodigo: codigo,
                                            iCodigoUnidade: unidade,
                                            oPlanoAcao: ref plano_acao);

                if (plano_acao == null)
                {
                    return HttpNotFound();
                }

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", plano_acao.codigo_unidade);
                ViewBag.solicitante = new SelectList(oCombo.Solicitante(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                        iCodigoUnidade: plano_acao.codigo_unidade), "codigo", "descricao", plano_acao.codigo_usuario_solicitante);
                ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: -1), "codigo", "descricao", plano_acao.codigo_departamento);
                ViewBag.responsavel = new SelectList(oCombo.ResponsavelDepartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                    iCodigoDepartamento: plano_acao.codigo_departamento), "codigo", "descricao", plano_acao.codigo_responsavel);
                ViewBag.prioridade = new SelectList(oCombo.Prioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: plano_acao.codigo_unidade), "codigo", "descricao", plano_acao.codigo_prioridade);
                ViewBag.data_necessidade = plano_acao.data_necessidade;

                return View(plano_acao);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlanoAcaoEdit(int unidade, int solicitante, int departamento, string data_necessidade, string descricao, int prioridade, HttpPostedFileBase arquivo, long codigo, int unidade_old, string change_imagem, int responsavel = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                string sPath = "";
                string sFileName = "";

                if (change_imagem == "change")
                {
                    if (arquivo != null)
                    {
                        sPath = Server.MapPath(Path.Combine("~/Content/img/Cliente/PA", Session["empresa"].ToString()));
                        sFileName = DateTime.Now.ToString("yyMMdd_HHmmss_") + arquivo.FileName.Replace("+", "").Replace("-", "").Replace(" ", "");
                        if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                        if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                        arquivo.SaveAs(Path.Combine(sPath, sFileName));
                    }
                }

                //Atualiza Plano de Ação
                oPlanoAcao.UpdatePlanoAcao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                            iCodigoUnidade: unidade,
                                            iCodigoUsuarioSolicitante: solicitante,
                                            iCodigoDepartamento: departamento,
                                            iCodigoResponsavel: responsavel,
                                            sDataNecessidade: data_necessidade,
                                            sDescricao: descricao.ToUpper(),
                                            iCodigoPrioridade: prioridade,
                                            sImagem: Path.Combine("~/Content/img/Cliente/OS", Session["empresa"].ToString(), sFileName),
                                            sArquivo: (arquivo != null) ? arquivo.FileName : "",
                                            lCodigo: codigo,
                                            iCodigoUnidadeOld: unidade_old);

                //Redireciona para Index
                return RedirectToAction("PlanoAcaoIndex");
            }
        }

        // GET: /DELETE
        public ActionResult PlanoAcaoDelete(long codigo, int unidade)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                MODELS.PlanoAcao ordem_servico = null;

                oPlanoAcao.InfoPlanoAcao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                lCodigo: codigo,
                                                iCodigoUnidade: unidade,
                                                oPlanoAcao: ref ordem_servico);

                if (ordem_servico == null)
                {
                    return HttpNotFound();
                }

                return View(ordem_servico);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlanoAcaoDelete([Bind(Include = "codigo, codigo_unidade")] MODELS.PlanoAcao ordem_servico)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oPlanoAcao.DeletePlanoAcao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        lCodigo: ordem_servico.codigo,
                                        iCodigoUnidade: ordem_servico.codigo_unidade);

                //Redireciona para Index
                return RedirectToAction("PlanoAcaoIndex");
            }
        }

        // GET: /VIEW
        public ActionResult PlanoAcaoView(long codigo, int unidade)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                MODELS.PlanoAcao plano_acao = null;

                oPlanoAcao.InfoPlanoAcao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                lCodigo: codigo,
                                                iCodigoUnidade: unidade,
                                                oPlanoAcao: ref plano_acao);

                ViewBag.plano_acao = plano_acao;

                return View(oPlanoAcao.PlanoAcaoApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            lCodigoPlanoAcao: codigo));
            }
        }

        // GET: /VIEW
        [AllowAnonymous]
        public ActionResult PlanoAcaoViewEmail(int empresa, long codigo, int unidade)
        {
            MODELS.PlanoAcao ordem_servico = null;

            oPlanoAcao.InfoPlanoAcao(iCodigoEmpresa: empresa,
                                            lCodigo: codigo,
                                            iCodigoUnidade: unidade,
                                            oPlanoAcao: ref ordem_servico);

            ViewBag.ordem_servico = ordem_servico;

            return View(oPlanoAcao.PlanoAcaoApontamento(iCodigoEmpresa: empresa,
                                                                iCodigoUnidade: unidade,
                                                                lCodigoPlanoAcao: codigo));
        }

        // POST: /PRINT
        public ActionResult PlanoAcaoPrintPDF(int codigo_empresa, int codigo_unidade, long codigo_ordem_servico)
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

        oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), "RPT000000033.rpt"));

        oCrDatabase = oReportDocument.Database;
        oCrTables = oCrDatabase.Tables;

        foreach (Table crTable in oCrTables)
        {
            oTableLogOnInfo = crTable.LogOnInfo;
            oTableLogOnInfo.ConnectionInfo = oConnectionInfo;
            crTable.ApplyLogOnInfo(oTableLogOnInfo);
        }

        oReportDocument.SetParameterValue("@codigo_unidade", codigo_unidade);
        oReportDocument.SetParameterValue("@codigo_empresa", codigo_empresa);
        oReportDocument.SetParameterValue("@codigo_ordem_servico", codigo_ordem_servico);

        Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
        stream.Seek(0, SeekOrigin.Begin);
        Response.AppendHeader("Content-Length", stream.Length.ToString());
        Response.AppendHeader("Content-Disposition", "inline; filename=RPT000000033.pdf");
        return File(stream, "application/pdf;");
    }


        // GET: /APONTAMENTO
        public ActionResult ApontamentoPlanoAcao(long codigo_pcm_plano_acao, int codigo_unidade)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                MODELS.PlanoAcaoApontamento apontamento = null;

                oPlanoAcao.LoadApontamentoPlanoAcao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    lCodigoPCMPlanoAcao: codigo_pcm_plano_acao,
                                                    oPlanoAcaoApontamento: ref apontamento);

                if (apontamento == null)
                {
                    return HttpNotFound();
                }

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "pcm_plano_acao",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.administrador = administrador;
                ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.apontamento = apontamento;
                ViewBag.justificativa_apontamento = new SelectList(oCombo.JustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                    iCodigoUnidade: codigo_unidade), "codigo", "descricao", null);

                return View();

            }

        }

        // POST: /APONTAMENTO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApontamentoPlanoAcao(string descricao, string data, int codigo_unidade, long codigo_pcm_plano_acao, HttpPostedFileBase arquivo, bool concluido = false, int justificativa_apontamento = -1, string observacao = "", string valor = "0", string percentual = "0")
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {

                    long codigo = 0;
                    string path = Path.Combine("C:\\", "SIM", "PCM", "SITE", "IMAGE", "PA", Session["empresa"].ToString(), codigo_unidade.ToString(), codigo.ToString());
                    string filename = "";

                    oPlanoAcao.InsertApontamentoPlanoAcao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                          iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                          lCodigoPCMPlanoAcao: codigo_pcm_plano_acao,
                                                          iCodigoUnidade: codigo_unidade,
                                                          sData: data,
                                                          sDescricao: descricao,
                                                          dValor: Convert.ToDouble(valor),
                                                          bConcluido: concluido,
                                                          iCodigoJustificativaApontamento: justificativa_apontamento,
                                                          dPercentual: Convert.ToDouble(percentual.Replace(".", ",")),
                                                          sObservacao: observacao,
                                                          lCodigo: ref codigo);

                    if (arquivo != null)
                    {
                        filename = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(arquivo.FileName);
                        if (System.IO.File.Exists(Path.Combine(path, filename))) { System.IO.File.Delete(Path.Combine(path, filename)); }
                        ResizeAndSaveImage(arquivo.InputStream, Path.Combine(path, filename));

                        oPicture.InsertPicture(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: codigo_unidade,
                                                lCodigo: codigo,
                                                sTipo: "PA_APONTAMENTO",
                                                iCodigoItemChecklist: -1,
                                                sImagePath: Path.Combine(path, filename));

                    }

                    return RedirectToAction("PlanoAcaoIndex", "PlanoAcao");
                }
            }

        #endregion

    }
}