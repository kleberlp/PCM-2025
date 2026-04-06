using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.AspNet.Identity;
using PCM.WEB.DAL;
using PCM.WEB.MODELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using static NPOI.HSSF.Util.HSSFColor;

namespace PCM.WEB.Controllers
{
    public class PCMController : Controller
    {
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private CadastroBasico oCadastroBasico = new CadastroBasico(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.PCM oPCM = new DAL.PCM(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.Relatorio oRelatorio = new DAL.Relatorio(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.OrdemServico oOrdemServico = new DAL.OrdemServico(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.Estoque oEstoque = new DAL.Estoque(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.Picture oPicture = new DAL.Picture(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: JSON :::

        //JSON: /CATEGORIA/
        public JsonResult LoadCategoria(int unidade)
        {
            return Json(oCombo.Categoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: unidade));
        }

        //JSON: /FUNCIONÁRIOManuenta
        public JsonResult LoadFuncionario(int unidade)
        {
            return Json(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: unidade,
                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])));
        }

        //JSON: /JUSTIFICATIVA - FALTA/
        public JsonResult LoadJustificativaFalta(int unidade)
        {
            return Json(oCombo.JustificativaFalta(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: unidade));
        }

        //JSON: /SETOR/
        public JsonResult LoadSetor(int unidade)
        {
            return Json(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUnidade: unidade));
        }

        //JSON: /PREVENTIVA/
        public JsonResult LoadPreventiva(int unidade)
        {
            return Json(oCombo.Preventiva(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                          iCodigoUnidade: unidade));
        }

        //JSON: /ROTINA/
        public JsonResult LoadRotina(int unidade)
        {
            return Json(oCombo.Rotina(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                      iCodigoUnidade: unidade));
        }

        //JSON: /DEPARTAMENTO/
        public JsonResult LoadDepartamento()
        {
            return Json(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())));
        }

        //JSON: /EQUIPAMENTO/
        public JsonResult LoadEquipamento(int unidade)
        {
            return Json(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: unidade));
        }

        //JSON: /EQUIPAMENTO/
        public JsonResult LoadEquipamentoSetor(int unidade, int setor)
        {
            return Json(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: unidade,
                                            iCodigoSetor: setor));
        }

        //JSON: /EQUIPAMENTO/
        public int LoadSetorEquipamento(long codigo)
        {
            Equipamento equipamento = new Equipamento();
            oCadastroBasico.InfoEquipamento(iCodigoEmpresa: Convert.ToInt32( Session["empresa"].ToString()),
                                            lCodigo: codigo,
                                            oEquipamento: ref equipamento);
            return equipamento.codigo_setor;
        }

        //JSON: /PRIORIDADE/
        public JsonResult LoadPrioridade(int unidade)
        {
            return Json(oCombo.Prioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: unidade));
        }

        //JSON: /UNIDADE/
        public JsonResult LoadUnidade()
        {
            return Json(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        bCadastro: false));
        }

        //JSON: /PRODUTO LOTE/
        public JsonResult LoadProdutoLote(int unidade, long produto)
        {
            return Json(oCombo.ProdutoLote(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: unidade,
                                            lCodigoProduto: produto));
        }

        //JSON: /VALIDA OS/
        public JsonResult ValidateOrdemServico(int unidade, long equipamento)
        {
            return Json(oOrdemServico.ValidateOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            lCodigoEquipamento: equipamento));
        }

        public void ResizeAndSaveImage(Stream imageStream, string outputFilePath)
        {
            using (Bitmap originalImage = new Bitmap(imageStream))
            {
                double scaleFactor = (originalImage.Width > originalImage.Height)
                    ? 400.0 / originalImage.Width
                    : 400.0 / originalImage.Height;

                int newWidth = (int)Math.Ceiling(originalImage.Width * scaleFactor);
                int newHeight = (int)Math.Ceiling(originalImage.Height * scaleFactor);

                using (Bitmap resizedImage = new Bitmap(originalImage, newWidth, newHeight))
                {
                    resizedImage.Save(outputFilePath, System.Drawing.Imaging.ImageFormat.Png);
                }
            }

        }

        // POST: /LOAD FOTO
        public JsonResult LoadFoto(long codigo, int codigo_unidade, int codigo_item_checklist, string tipo)
        {

            return Json(oPicture.PictureList(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: codigo_unidade,
                                            lCodigo: codigo,
                                            sTipo: tipo,
                                            iCodigoItemChecklist: codigo_item_checklist));
        }

        // POST: /UPLOAD FOTO
        public JsonResult UploadFoto(long codigo, int codigo_unidade, int codigo_item_checklist, string tipo)
        {

            try
            {
                string filename = "";
                string path = Path.Combine("C:\\", "SIM", "PCM", "SITE", "IMAGE", tipo, Session["empresa"].ToString(), codigo_unidade.ToString());

                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase oHttpPostedFileBase = Request.Files[i];
                    filename = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(oHttpPostedFileBase.FileName);
                    if (System.IO.File.Exists(Path.Combine(path, filename))) System.IO.File.Delete(Path.Combine(path, filename));

                    ResizeAndSaveImage(oHttpPostedFileBase.InputStream, Path.Combine(path, filename));

                    oPicture.InsertPicture(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: codigo_unidade,
                                            sTipo: tipo,
                                            lCodigo: codigo,
                                            iCodigoItemChecklist: codigo_item_checklist,
                                            sImagePath: Path.Combine(path, filename));

                }

                return Json("true");
            }
            catch (Exception ex)
            {
                return Json("false");
            }

        }

        #endregion

        #region ::: APONTAMENTO - OS :::

        // GET: /APONTAMENTO
        public ActionResult ApontamentoOS(long codigo_pcm_ordem_servico, int codigo_unidade, string page = "AgendaOS", string model = "PCM")
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    Apontamento apontamento = null;

                    oPCM.LoadApontamentoOS(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                           iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                           iCodigoUnidade: codigo_unidade,
                                           lCodigoPCMOrdemServico: codigo_pcm_ordem_servico,
                                           oApontamento: ref apontamento);

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
                                        sFormulario: "pcm_apontamento_os",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.funcionario_vinculado = apontamento.codigo_funcionario;
                    ViewBag.page = page;
                    ViewBag.model = model;
                    ViewBag.inserir = inserir;
                    ViewBag.administrador = administrador;
                    ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                    ViewBag.apontamento = apontamento;
                    ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: codigo_unidade,
                                                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", null);
                    ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                          iCodigoUnidade: codigo_unidade), "codigo", "descricao", apontamento.codigo_fornecedor);
                    ViewBag.categoria = new SelectList(oCombo.Categoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: codigo_unidade), "codigo", "descricao", apontamento.codigo_categoria);
                    ViewBag.tipo_servico = new SelectList(oCombo.TipoServico(), "codigo", "descricao", apontamento.codigo_tipo_servico);
                    ViewBag.tipo_ordem_servico = new SelectList(oCombo.TipoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                        iOrdemServico: 1), "codigo", "descricao", apontamento.codigo_tipo_ordem_servico);
                    ViewBag.justificativa_apontamento = new SelectList(oCombo.JustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                                                       iCodigoUnidade: codigo_unidade), "codigo", "descricao", null);
                    ViewBag.produto_combo = new SelectList(oCombo.Produto(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                          iCodigoUnidade: codigo_unidade), "codigo", "descricao", null);
                    ViewBag.lote = new SelectList(oCombo.ProdutoLote(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                     iCodigoUnidade: codigo_unidade,
                                                                     lCodigoProduto: -1), "codigo", "descricao", null);

                    List<SaidaEstoque> saidas = new List<SaidaEstoque>();
                    SaidaEstoque saida = new SaidaEstoque();
                    saida.excluido = 1;
                    saidas.Add(saida);

                    return View(saidas);

                }

            }

        // POST: /APONTAMENTO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApontamentoOS(string descricao_solucao, string data_inicio, string data_termino, int codigo_unidade, long codigo_pcm_ordem_servico, HttpPostedFileBase arquivo, int[] funcionario, List<SaidaEstoque> saidas, int fornecedor = -1, int categoria = -1, string hora_inicio = "00:00", string hora_termino = "00:00", int tipo_servico = -1, int tipo_ordem_servico = -1, bool concluido = false, bool desativar_equipamento = false, int justificativa_apontamento = -1, string observacao = "", string page = "AgendaOS", string model = "PCM")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                long codigo = 0;

                if (funcionario == null)
                {
                    oPCM.InsertApontamentoOS(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                iCodigoUnidade: codigo_unidade,
                                                lCodigoPCMOrdemServico: codigo_pcm_ordem_servico,
                                                iCodigoTipoServico: tipo_servico,
                                                iCodigoTipoOrdemServico: tipo_ordem_servico,
                                                iCodigoFornecedor: fornecedor,
                                                iCodigoFuncionario: -1,
                                                iCodigoCategoria: categoria,
                                                sDataInicio: data_inicio,
                                                sDataTermino: data_termino,
                                                sHoraInicio: hora_inicio,
                                                sHoraTermino: hora_termino,
                                                dValor: 0,
                                                sDescricaoSolucao: descricao_solucao,
                                                sArquivo: "",
                                                bConcluido: concluido,
                                                iCodigoJustificativaApontamento: (concluido) ? -1 : justificativa_apontamento,
                                                sObservacaoApontamento: (concluido) ? "" : observacao,
                                                bDesativarApontamento: desativar_equipamento,
                                                lCodigo: ref codigo);

                    if (arquivo != null)
                    {

                        string filename = "";
                        string path = Path.Combine("C:\\", "SIM", "PCM", "SITE", "IMAGE", "OS_APONTAMENTO", Session["empresa"].ToString(), codigo_unidade.ToString());

                        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            filename = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(arquivo.FileName);
                            if (System.IO.File.Exists(Path.Combine(path, filename))) System.IO.File.Delete(Path.Combine(path, filename));

                            ResizeAndSaveImage(arquivo.InputStream, Path.Combine(path, filename));

                            oPicture.InsertPicture(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    sTipo: "OS_APONTAMENTO",
                                                    lCodigo: codigo,
                                                    iCodigoItemChecklist: -1,
                                                    sImagePath: Path.Combine(path, filename));

                        }

                    }

                }
                else
                {

                    for (int i = 0; i < funcionario.Length; i++)
                    {
                        oPCM.InsertApontamentoOS(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    lCodigoPCMOrdemServico: codigo_pcm_ordem_servico,
                                                    iCodigoTipoServico: tipo_servico,
                                                    iCodigoTipoOrdemServico: tipo_ordem_servico,
                                                    iCodigoFornecedor: fornecedor,
                                                    iCodigoFuncionario: funcionario[i],
                                                    iCodigoCategoria: categoria,
                                                    sDataInicio: data_inicio,
                                                    sDataTermino: data_termino,
                                                    sHoraInicio: hora_inicio,
                                                    sHoraTermino: hora_termino,
                                                    dValor: 0,
                                                    sDescricaoSolucao: descricao_solucao,
                                                    sArquivo: "",
                                                    bConcluido: concluido,
                                                    iCodigoJustificativaApontamento: (concluido)? -1 : justificativa_apontamento,
                                                    sObservacaoApontamento: (concluido) ? "" : observacao,
                                                    bDesativarApontamento: desativar_equipamento,
                                                    lCodigo: ref codigo);

                        if (arquivo != null)
                        {
                            string filename = "";
                            string path = Path.Combine("C:\\", "SIM", "PCM", "SITE", "IMAGE", "OS_APONTAMENTO", Session["empresa"].ToString(), codigo_unidade.ToString());

                            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                                filename = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(arquivo.FileName);
                                if (System.IO.File.Exists(Path.Combine(path, filename))) System.IO.File.Delete(Path.Combine(path, filename));

                                ResizeAndSaveImage(arquivo.InputStream, Path.Combine(path, filename));

                                oPicture.InsertPicture(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUnidade: codigo_unidade,
                                                        sTipo: "OS_APONTAMENTO",
                                                        lCodigo: codigo,
                                                        iCodigoItemChecklist: -1,
                                                        sImagePath: Path.Combine(path, filename));


                        }

                    }
            }

                return RedirectToAction(page, model);
            }
        }

        // GET: /APONTAMENTO EDITAR
        public ActionResult ApontamentoOSEdit(long codigo, int codigo_unidade, string return_page = "AgendaOS", string data_inicio = "", string data_termino = "", int mes = -1, int ano = -1, int codigo_funcionario = -1, long codigo_equipamento = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Apontamento apontamento = null;

                oPCM.LoadApontamentoOSInfo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            lCodigoPCMApontamento: codigo,
                                            iCodigoUnidade: codigo_unidade,
                                            oApontamento: ref apontamento);


                ViewBag.codigo_unidade = codigo_unidade;
                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;
                ViewBag.mes = mes;
                ViewBag.ano = ano;
                ViewBag.codigo_funcionario = codigo_funcionario;
                ViewBag.codigo_equipamento = codigo_equipamento;

                ViewBag.return_page = return_page;
                ViewBag.tipo = "programada";
                ViewBag.apontamento = apontamento;
                ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: codigo_unidade,
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", apontamento.codigo_funcionario);
                ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: codigo_unidade), "codigo", "descricao", apontamento.codigo_fornecedor);
                ViewBag.categoria = new SelectList(oCombo.Categoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: codigo_unidade), "codigo", "descricao", apontamento.codigo_categoria);
                ViewBag.tipo_servico = new SelectList(oCombo.TipoServico(), "codigo", "descricao", apontamento.codigo_tipo_servico);
                ViewBag.tipo_ordem_servico = new SelectList(oCombo.TipoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                                    iOrdemServico: 1), "codigo", "descricao", apontamento.codigo_tipo_ordem_servico);
                ViewBag.justificativa_apontamento = new SelectList(oCombo.JustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                                                    iCodigoUnidade: codigo_unidade), "codigo", "descricao", null);
                ViewBag.apontamento = apontamento;
                return View(oPCM.LoadApontamentoCheckListExcluir(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: codigo_unidade,
                                                                    lCodigoPCMApontamento: codigo));
            }
        }

        // POST: /APONTAMENTO EDITAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApontamentoOSEdit(string descricao_solucao, string data_inicio, string data_termino, int codigo_unidade, List<ApontamentoChecklist> checklist, HttpPostedFileBase arquivo, long codigo_apontamento, int funcionario = -1, int fornecedor = -1, int categoria = -1, string hora_inicio = "00:00", string hora_termino = "00:00", int tipo_servico = -1, int tipo_ordem_servico = -1, string return_page = "AgendaOS", string data_inicio_return = "", string data_termino_return = "", int mes_return = -1, int ano_return = -1, int codigo_funcionario_return = -1, long codigo_equipamento_return = -1)
    {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                if (arquivo != null)
                {
                    string filename = "";
                    string path = Path.Combine("C:\\", "SIM", "PCM", "SITE", "IMAGE", "OS_APONTAMENTO", Session["empresa"].ToString(), codigo_unidade.ToString());

                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                    filename = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(arquivo.FileName);
                    if (System.IO.File.Exists(Path.Combine(path, filename))) System.IO.File.Delete(Path.Combine(path, filename));

                    ResizeAndSaveImage(arquivo.InputStream, Path.Combine(path, filename));

                    oPicture.InsertPicture(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: codigo_unidade,
                                            sTipo: "OS_APONTAMENTO",
                                            lCodigo: codigo_apontamento,
                                            iCodigoItemChecklist: -1,
                                            sImagePath: Path.Combine(path, filename));


                }

                oPCM.UpdateApontamentoOS(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                            iCodigoCategoria: categoria,
                                            iCodigoFornecedor: fornecedor,
                                            iCodigoFuncionario: funcionario,
                                            sDataInicio: data_inicio,
                                            sDataTermino: data_termino,
                                            sHoraInicio: hora_inicio,
                                            sHoraTermino: hora_termino,
                                            iCodigoTipoServico: tipo_servico,
                                            iCodigoTipoOrdemServico: tipo_ordem_servico,
                                            dValor: 0,
                                            sDescricaoSolucao: descricao_solucao,
                                            lCodigoApontamento: codigo_apontamento,
                                            iCodigoUnidade: codigo_unidade);

                return RedirectToAction(return_page, "PCM", new { codigo_unidade = codigo_unidade, data_inicio = data_inicio_return, data_termino = data_termino_return, mes = mes_return, ano = ano_return, codigo_funcionario = codigo_funcionario_return, codigo_equipamento = codigo_equipamento_return});
            }
        }

        // GET: /APONTAMENTO EXCLUIR
        public ActionResult ApontamentoOSExcluir(long codigo, int codigo_unidade)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Apontamento apontamento = null;

                oPCM.LoadApontamentoOSInfo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            lCodigoPCMApontamento: codigo,
                                            iCodigoUnidade: codigo_unidade,
                                            oApontamento: ref apontamento);

                if (apontamento == null)
                {
                    return HttpNotFound();
                }

                ViewBag.apontamento = apontamento;
                return View();
            }
        }

        // POST: /APONTAMENTO EXCLUIR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApontamentoOSExcluir([Bind(Include = "codigo_apontamento, codigo_unidade")] Apontamento apontamento)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {

                    oPCM.DeleteApontamentoOS(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                             iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                             iCodigoUnidade: apontamento.codigo_unidade,
                                             lCodigo: apontamento.codigo_apontamento);

                    return RedirectToAction("AgendaOS", "PCM");
                }
            }

        #endregion

        #region ::: AGENDA - OS :::

        // GET: INDEX
        public ActionResult AgendaOS()
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
                                    sFormulario: "pcm_apontamento_os",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.administrador = administrador;
                ViewBag.data_inicio = System.TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddMonths(-1).ToShortDateString();
                ViewBag.data_termino = System.DateTime.Now.Date.ToShortDateString();
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.equipamento = new SelectList(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.status = new SelectList(oCombo.StatusManutencao(), "codigo", "descricao", null);
                ViewBag.prioridade = new SelectList(oCombo.Prioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.filtro = 1;
                ViewBag.codigo_status = "[1,3,4]";

                List<AgendaOS> agenda = oPCM.LoadAgendaOS(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                            sOrdemServico: "",
                                                            iCodigoSetor: -1,
                                                            lCodigoEquipamento: -1,
                                                            sStatus: "1,3,4",
                                                            iCodigoPrioridade: -1,
                                                            sDataInicio: System.TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddMonths(-1).ToShortDateString(),
                                                            sDataTermino: System.DateTime.Now.Date.ToShortDateString());

                ViewBag.total = agenda.Count;

                return View(agenda);
            }
        }

        // POST: INDEX
        [HttpPost]
        public ActionResult AgendaOS(int[] status, int unidade = -1, string ordem_servico = "", int setor = -1, long equipamento = -1, int prioridade = -1, string data_inicio = "", string data_termino = "")
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
                                    sFormulario: "pcm_apontamento_os",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                if (editar && excluir)
                    ViewBag.columnDefs = "columnDefs: [{ orderable: false, targets: [0, 1] }], order: [[2]]";
                else if (editar || excluir)
                    ViewBag.columnDefs = "columnDefs: [{ orderable: false, targets: [0] }], order: [[1]]";
                else
                    ViewBag.columnDefs = "order: [[0]]";

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.administrador = administrador;
                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;
                ViewBag.ordem_servico = ordem_servico;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade), "codigo", "descricao", setor);
                ViewBag.equipamento = new SelectList(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: unidade), "codigo", "descricao", equipamento);
                ViewBag.prioridade = new SelectList(oCombo.Prioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: unidade), "codigo", "descricao", prioridade);
                ViewBag.status = new SelectList(oCombo.StatusManutencao(), "codigo", "descricao");
                ViewBag.filtro = 0;

                string codigo_status = "";

                if (status != null)
                {
                    for (int i = 0; i < status.Length; i++)
                    {
                        codigo_status = String.Concat(codigo_status, String.Concat((codigo_status == "") ? "" : ",", status[i].ToString()));
                    }
                }

                ViewBag.codigo_status = String.Concat("[", codigo_status, "]");

                List<AgendaOS> agenda = oPCM.LoadAgendaOS(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            iCodigoUnidade: unidade,
                                                            sOrdemServico: ordem_servico,
                                                            iCodigoSetor: setor,
                                                            lCodigoEquipamento: equipamento,
                                                            sStatus: codigo_status,
                                                            iCodigoPrioridade: prioridade,
                                                            sDataInicio: data_inicio,
                                                            sDataTermino: data_termino);

                ViewBag.total = agenda.Count;

                return View(agenda);
            }
        }

        // GET: Concluir OS
        public ActionResult ConcluirOS(long codigo_pcm_ordem_servico, int unidade, string data_inicio = "", string data_termino = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                oPCM.UpdateStatusOS(lCodigoPCMOrdemServico: codigo_pcm_ordem_servico,
                                    iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    iCodigoUnidade: unidade,
                                    iStatus: 2);

                return RedirectToAction("AgendaOS", "PCM");
            }
        }

        // GET: Reabrir OS
        public ActionResult ReabrirOS(long codigo_pcm_ordem_servico, int unidade)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {

                    oPCM.UpdateStatusOS(lCodigoPCMOrdemServico: codigo_pcm_ordem_servico,
                                        iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        iCodigoUnidade: unidade,
                                        iStatus: 4);

                    return RedirectToAction("AgendaOS", "PCM");
                }
            }

        #endregion

        #region ::: FALTA :::

        // GET: INDEX
        public ActionResult FaltaIndex()
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
                                    sFormulario: "pcm_falta",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                string data_inicio = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddMonths(-1).ToShortDateString();
                string data_termino = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();

                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", null);
                ViewBag.justificativa = new SelectList(oCombo.JustificativaFalta(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);

                return View(oPCM.IndexFalta(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"]),
                                            sDataInicio: data_inicio,
                                            sDataTermino: data_termino,
                                            iCodigoFuncionario: -1,
                                            iCodigoJustificativaFalta: -1));
            }
        }

        // POST: INDEX
        [HttpPost]
        public ActionResult FaltaIndex(int unidade = -1, string data_inicio = "", string data_termino = "", int funcionario = -1, int justificativa = -1)
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
                                    sFormulario: "pcm_falta",
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
                ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: unidade,
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", funcionario);
                ViewBag.justificativa = new SelectList(oCombo.JustificativaFalta(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                    iCodigoUnidade: unidade), "codigo", "descricao", justificativa);

                return View(oPCM.IndexFalta(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"]),
                                            sDataInicio: data_inicio,
                                            sDataTermino: data_termino,
                                            iCodigoFuncionario: funcionario,
                                            iCodigoJustificativaFalta: justificativa));
            }
        }

        // GET: INSERT
        public ActionResult FaltaInsert()
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
                ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", null);
                ViewBag.justificativa = new SelectList(oCombo.JustificativaFalta(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);

                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult FaltaInsert(int unidade, int funcionario, int justificativa, string data_inicio, string hora_inicio, string data_termino, string hora_termino, HttpPostedFileBase arquivo, string observacao = "")
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
                    sPath = Server.MapPath(Path.Combine("~/Content/arq/Falta", Session["empresa"].ToString()));
                    sFileName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(arquivo.FileName);
                    if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                    if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                    arquivo.SaveAs(Path.Combine(sPath, sFileName));
                }

                //Insere Registro no Banco de Dados
                oPCM.InsertFalta(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUnidade: unidade,
                                    iCodigoFuncionario: funcionario,
                                    iCodigoJustificativaFalta: justificativa,
                                    sDataInicio: data_inicio,
                                    sHoraInicio: hora_inicio,
                                    sDataTermino: data_termino,
                                    sHoraTermino: hora_termino,
                                    sObservacao: observacao,
                                    sPathArquivo: (sFileName == "") ? "" : Path.Combine("~/Content/arq/Falta", Session["empresa"].ToString(), sFileName),
                                    sArquivo: (arquivo != null) ? arquivo.FileName : "",
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()));


                return RedirectToAction("FaltaInsert");
            }
        }

        // GET: /DELETE
        public ActionResult FaltaDelete(int unidade, long codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                PCMFalta pcm_falta = null;

                oPCM.InfoFalta(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                iCodigoUnidade: unidade,
                                lCodigo: codigo,
                                oPCMFalta: ref pcm_falta);

                if (pcm_falta == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(pcm_falta);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FaltaDelete(int unidade, long codigo)
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
                        oPCM.DeleteFalta(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                         iCodigoUnidade: unidade,
                                         lCodigo: codigo,
                                         iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()));

                        //Redireciona para Index
                        return RedirectToAction("FaltaIndex");
                    }
                    catch
                    {
                        
                        return FaltaDelete(unidade: unidade,
                                           codigo: codigo,
                                           erro: PCM.WEB.Properties.Resources.valida_excluir);
                    }
                }
            }

        #endregion

        #region ::: PROGRAMADA :::

        // GET: ORDEM DE SERVIÇO - PROGRAMADA
        public ActionResult OrdemServicoProgramada(int codigo_unidade, long codigo_pcm_programada_ordem_servico, string formulario, string tipo = "geral")
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
                                    sFormulario: formulario,
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.inserir = inserir;
                ViewBag.administrador = administrador;
                ViewBag.formulario = formulario;
                ViewBag.tipo = tipo;

                if (formulario == "pcm_apontamento_preventiva")
                {
                    @ViewBag.resource_apontamento = PCM.WEB.Properties.Resources.apontamento_preventiva;
                    @ViewBag.resource_name = PCM.WEB.Properties.Resources.preventiva;
                    @ViewBag.view = (tipo == "historico")? "HistoricoPreventiva": "ManutencaoPreventiva";
                    @ViewBag.pcm_tipo = "PREVENTIVA";
                }
                else if (formulario == "pcm_apontamento_rotina")
                {
                    @ViewBag.resource_apontamento = PCM.WEB.Properties.Resources.apontamento_rotina;
                    @ViewBag.resource_name = PCM.WEB.Properties.Resources.rotina;
                    @ViewBag.view = (tipo == "historico") ? "HistoricoRotina" : "ManutencaoRotina";
                    @ViewBag.pcm_tipo = "ROTINA";
                }
                else if (formulario == "pcm_apontamento_laudo")
                {
                    @ViewBag.resource_apontamento = PCM.WEB.Properties.Resources.apontamento_laudo;
                    @ViewBag.resource_name = PCM.WEB.Properties.Resources.laudo;
                    @ViewBag.view = (tipo == "historico") ? "HistoricoLaudo" : "ManutencaoLaudo";
                    @ViewBag.pcm_tipo = "LAUDO";
                }

                PCMProgramadaOrdemServico ordemServico = oPCM.OrdemServicoPreventiva(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                     iCodigoUnidade: codigo_unidade,
                                                                                     lCodigoPCMProgramadaOrdemServico: codigo_pcm_programada_ordem_servico);

                ViewBag.ordem_servico = ordemServico;

                return View(ordemServico.checklist);
            }
        }

        // POST: ORDEM DE SERVIÇO - PREVENTIVA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrdemServicoProgramada(string descricao_solucao, string data_inicio, string data_termino, int codigo_unidade, long codigo_pcm_programada, long codigo_pcm_programada_ordem_servico, List<PCMProgramadaOrdemServicoChecklist> checklist, string formulario, string hora_inicio = "00:00", string hora_termino = "00:00", bool concluido = false, string valor_manutencao = "0", int quantidade_equipamento = 0, string tipo = "geral")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Ordem de Serviço
                oPCM.UpdateOrdemServicoProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    lCodigoPCMProgramada: codigo_pcm_programada,
                                                    dValor: Convert.ToDouble(valor_manutencao.Replace(".", "").Replace("R$ ", "")),
                                                    iQuantidadeEquipamento: quantidade_equipamento,
                                                    sDescricaoSolucao: descricao_solucao,
                                                    bConcluido: concluido,
                                                    lCodigoPCMProgramadaOrdemServico: codigo_pcm_programada_ordem_servico);

                //Insere Checklist
                if (checklist != null)
                {

                    foreach (PCMProgramadaOrdemServicoChecklist item in checklist)
                    {

                        //Insere Registro no Banco de Dados
                        oPCM.InsertChecklistProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigoUnidade: codigo_unidade,
                                                        lCodigoPCMProgramadaOrdemServico: codigo_pcm_programada_ordem_servico,
                                                        iCodigoChecklistItem: item.codigo,
                                                        sResultado: item.resultado,
                                                        sObservacao: item.observacao);

                    }

                }

                return RedirectToAction("OrdemServicoProgramada", "PCM", new { codigo_unidade = codigo_unidade, codigo_pcm_programada_ordem_servico = codigo_pcm_programada_ordem_servico, formulario = formulario, tipo = tipo });
            }
        }

        // GET: /APONTAMENTO
        public ActionResult ApontamentoProgramada(long codigo_pcm_programada, int codigo_unidade, string formulario, string data = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Apontamento apontamento = null;

                oPCM.LoadApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        iCodigoUnidade: codigo_unidade,
                                        lCodigoPCMProgramada: codigo_pcm_programada,
                                        oApontamento: ref apontamento);

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
                                    sFormulario: formulario,
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.administrador = administrador;
                ViewBag.inserir = inserir;
                ViewBag.data = (data == "")? TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString(): data;
                ViewBag.apontamento = apontamento;
                ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: codigo_unidade,
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", apontamento.codigo_funcionario);
                ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: codigo_unidade), "codigo", "descricao", apontamento.codigo_fornecedor);
                ViewBag.categoria = new SelectList(oCombo.Categoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: codigo_unidade), "codigo", "descricao", apontamento.codigo_categoria);
                ViewBag.tipo_servico = new SelectList(oCombo.TipoServico(), "codigo", "descricao", apontamento.codigo_tipo_servico);

                if (formulario == "pcm_apontamento_preventiva")
                {
                    @ViewBag.resource_apontamento = PCM.WEB.Properties.Resources.apontamento_preventiva;
                    @ViewBag.resource_name = PCM.WEB.Properties.Resources.preventiva;
                    @ViewBag.view = "ManutencaoPreventiva";
                }
                else if (formulario == "pcm_apontamento_rotina")
                {
                    @ViewBag.resource_apontamento = PCM.WEB.Properties.Resources.apontamento_rotina;
                    @ViewBag.resource_name = PCM.WEB.Properties.Resources.rotina;
                    @ViewBag.view = "ManutencaoRotina";
                }
                else if (formulario == "pcm_apontamento_laudo")
                {
                    @ViewBag.resource_apontamento = PCM.WEB.Properties.Resources.apontamento_laudo;
                    @ViewBag.resource_name = PCM.WEB.Properties.Resources.laudo;
                    @ViewBag.view = "ManutencaoLaudo";
                }

                return View(oPCM.LoadPCMProgramadaCheckList(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            lCodigoPCMProgramada: codigo_pcm_programada,
                                                            iCodigoUnidade: codigo_unidade));
            }
        }

        // POST: /APONTAMENTO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApontamentoProgramada(string descricao_solucao, string data_inicio, string data_termino, int codigo_unidade, long codigo_pcm_programada, long codigo_pcm_programada_ordem_servico, List<PCMProgramadaOrdemServicoChecklist> checklist, string view, int[] funcionario, int fornecedor = -1, string hora_inicio = "00:00", string hora_termino = "00:00", bool concluido = true, string valor_manutencao = "0", int quantidade_equipamento = 0)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                valor_manutencao = (valor_manutencao == "") ? "0" : valor_manutencao;

                //Insere Ordem de Serviço
                oPCM.InsertOrdemServicoProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    lCodigoPCMProgramada: codigo_pcm_programada,
                                                    dValor: Convert.ToDouble(valor_manutencao.Replace(".", "").Replace("R$ ", "")),
                                                    iQuantidadeEquipamento: quantidade_equipamento,
                                                    sDescricaoSolucao: descricao_solucao,
                                                    bConcluido: concluido,
                                                    sData: data_inicio,
                                                    sDataTermino: data_termino,
                                                    lCodigoPCMProgramadaOrdemServico: ref codigo_pcm_programada_ordem_servico);

                //Insere Apontamento
                if (funcionario == null)
                {

                    oPCM.InsertApontamentoProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigoUnidade: codigo_unidade,
                                                        lCodigoPCMProgramadaOrdemServico: codigo_pcm_programada_ordem_servico,
                                                        iCodigoFornecedor: fornecedor,
                                                        iCodigoFuncionario: -1,
                                                        sDataInicio: data_inicio,
                                                        sDataTermino: data_termino,
                                                        sHoraInicio: hora_inicio,
                                                        sHoraTermino: hora_termino);
                }
                else
                {

                    for (int i = 0; i < funcionario.Length; i++)
                    {

                        oPCM.InsertApontamentoProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                         iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                         iCodigoUnidade: codigo_unidade,
                                                         lCodigoPCMProgramadaOrdemServico: codigo_pcm_programada_ordem_servico,
                                                         iCodigoFornecedor: fornecedor,
                                                         iCodigoFuncionario: funcionario[i],
                                                         sDataInicio: data_inicio,
                                                         sDataTermino: data_termino,
                                                         sHoraInicio: hora_inicio,
                                                         sHoraTermino: hora_termino);
                    }
                }

                //Insere Checklist
                if (checklist != null)
                {

                    foreach (PCMProgramadaOrdemServicoChecklist item in checklist)
                    {

                        //Insere Registro no Banco de Dados
                        oPCM.InsertChecklistProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigoUnidade: codigo_unidade,
                                                        lCodigoPCMProgramadaOrdemServico: codigo_pcm_programada_ordem_servico,
                                                        iCodigoChecklistItem: item.codigo,
                                                        sResultado: item.resultado,
                                                        sObservacao: item.observacao);

                    }

                }

                return RedirectToAction(view, "PCM");
            }
        }

        // GET: /APONTAMENTO EDITAR
        public ActionResult ApontamentoProgramadaEdit(long codigo_apontamento, int codigo_unidade, string formulario, string tipo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Apontamento apontamento = null;

                oPCM.LoadApontamentoProgramadaInfo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    lCodigoPCMApontamento: codigo_apontamento,
                                                    iCodigoUnidade: codigo_unidade,
                                                    oApontamento: ref apontamento);

                if (apontamento == null)
                {
                    return HttpNotFound();
                }

                ViewBag.tipo = tipo;
                ViewBag.apontamento = apontamento;
                ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: codigo_unidade,
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", apontamento.codigo_funcionario);
                ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: codigo_unidade), "codigo", "descricao", apontamento.codigo_fornecedor);
                ViewBag.formulario = formulario;

                if (formulario == "pcm_apontamento_preventiva")
                {
                    @ViewBag.resource_apontamento = PCM.WEB.Properties.Resources.apontamento_preventiva;
                    @ViewBag.resource_name = PCM.WEB.Properties.Resources.preventiva;
                    @ViewBag.view = "ManutencaoPreventiva";
                }
                else if (formulario == "pcm_apontamento_rotina")
                {
                    @ViewBag.resource_apontamento = PCM.WEB.Properties.Resources.apontamento_rotina;
                    @ViewBag.resource_name = PCM.WEB.Properties.Resources.rotina;
                    @ViewBag.view = "ManutencaoRotina";
                }
                else if (formulario == "pcm_apontamento_laudo")
                {
                    @ViewBag.resource_apontamento = PCM.WEB.Properties.Resources.apontamento_laudo;
                    @ViewBag.resource_name = PCM.WEB.Properties.Resources.laudo;
                    @ViewBag.view = "ManutencaoLaudo";
                }

                return View();
            }
        }

        // POST: /APONTAMENTO EDITAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApontamentoProgramadaEdit(string data_inicio, string data_termino, int codigo_unidade, long codigo_pcm_programada_ordem_servico, long codigo_apontamento, string formulario, int funcionario = -1, int fornecedor = -1, string hora_inicio = "00:00", string hora_termino = "00:00", string tipo = "geral")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                oPCM.UpdateApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        iCodigoUnidade: codigo_unidade,
                                        lCodigoApontamento: codigo_apontamento,
                                        iCodigoFornecedor: fornecedor,
                                        iCodigoFuncionario: funcionario,
                                        sDataInicio: data_inicio,
                                        sDataTermino: data_termino,
                                        sHoraInicio: hora_inicio,
                                        sHoraTermino: hora_termino);

                return RedirectToAction("OrdemServicoProgramada", "PCM", new { codigo_unidade = codigo_unidade, codigo_pcm_programada_ordem_servico = codigo_pcm_programada_ordem_servico, formulario = formulario, tipo = tipo });
            }
        }

        // GET: /APONTAMENTO INSERT
        public ActionResult ApontamentoProgramadaInsert(long codigo_pcm_programada_ordem_servico, int codigo_unidade, string formulario, string tipo = "geral")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: codigo_unidade,
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", null);
                ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: codigo_unidade), "codigo", "descricao", null);
                ViewBag.ordem_servico = oPCM.OrdemServicoPreventiva(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: codigo_unidade,
                                                                    lCodigoPCMProgramadaOrdemServico: codigo_pcm_programada_ordem_servico);
                ViewBag.formulario = formulario;
                ViewBag.tipo = tipo;

                if (formulario == "pcm_apontamento_preventiva")
                {
                    @ViewBag.resource_apontamento = PCM.WEB.Properties.Resources.apontamento_preventiva;
                    @ViewBag.resource_name = PCM.WEB.Properties.Resources.preventiva;
                    @ViewBag.view = "ManutencaoPreventiva";
                }
                else if (formulario == "pcm_apontamento_rotina")
                {
                    @ViewBag.resource_apontamento = PCM.WEB.Properties.Resources.apontamento_rotina;
                    @ViewBag.resource_name = PCM.WEB.Properties.Resources.rotina;
                    @ViewBag.view = "ManutencaoRotina";
                }
                else if (formulario == "pcm_apontamento_laudo")
                {
                    @ViewBag.resource_apontamento = PCM.WEB.Properties.Resources.apontamento_laudo;
                    @ViewBag.resource_name = PCM.WEB.Properties.Resources.laudo;
                    @ViewBag.view = "ManutencaoLaudo";
                }

                return View();
            }
        }

        // POST: /APONTAMENTO INSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApontamentoProgramadaInsert(string data_inicio, string data_termino, int codigo_unidade, long codigo_pcm_programada_ordem_servico, string formulario, int funcionario = -1, int fornecedor = -1, string hora_inicio = "00:00", string hora_termino = "00:00", string tipo = "geral")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                oPCM.InsertApontamentoProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    lCodigoPCMProgramadaOrdemServico: codigo_pcm_programada_ordem_servico,
                                                    iCodigoFornecedor: fornecedor,
                                                    iCodigoFuncionario: funcionario,
                                                    sDataInicio: data_inicio,
                                                    sDataTermino: data_termino,
                                                    sHoraInicio: hora_inicio,
                                                    sHoraTermino: hora_termino);

                return RedirectToAction("OrdemServicoProgramada", "PCM", new { codigo_unidade = codigo_unidade, codigo_pcm_programada_ordem_servico = codigo_pcm_programada_ordem_servico, formulario = formulario, tipo = tipo });
            }
        }

        // GET: /APONTAMENTO EXCLUIR
        public bool ApontamentoProgramadaExcluir(long codigo_apontamento, int codigo_unidade)
        {

            try
            {
                oPCM.DeleteApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        iCodigoUnidade: codigo_unidade,
                                        lCodigo: codigo_apontamento);

                return true;
            }
            catch
            {
                return false;
            }

        }

        // GET: Concluir 
        public ActionResult ConcluirProgramada(long codigo_pcm_programada_ordem_servico, int codigo_unidade, string formulario, string tipo = "geral")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                oPCM.UpdateStatus(lCodigoPCMProgramadaOrdemServico: codigo_pcm_programada_ordem_servico,
                                    iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    iCodigoUnidade: codigo_unidade,
                                    iStatus: 2);

                return RedirectToAction("OrdemServicoProgramada", "PCM", new { codigo_unidade = codigo_unidade, codigo_pcm_programada_ordem_servico = codigo_pcm_programada_ordem_servico, formulario = formulario, tipo = tipo });
            }
        }

        // GET: Reabrir 
        public ActionResult ReabrirProgramada(long codigo_pcm_programada_ordem_servico, int codigo_unidade, string formulario, string tipo = "geral")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                oPCM.UpdateStatus(lCodigoPCMProgramadaOrdemServico: codigo_pcm_programada_ordem_servico,
                                    iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    iCodigoUnidade: codigo_unidade,
                                    iStatus: 4);

                return RedirectToAction("OrdemServicoProgramada", "PCM", new { codigo_unidade = codigo_unidade, codigo_pcm_programada_ordem_servico = codigo_pcm_programada_ordem_servico, formulario = formulario, tipo = tipo });
            }
        }

        // GET: INDEX
        public ActionResult DeleteOrdemServicoProgramada(int codigo_unidade, long codigo_pcm_programada_ordem_servico, string view, string control)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {

                    oPCM.DeleteOrdemServicoProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                      iCodigoUnidade: codigo_unidade,
                                                      lCodigoPCMProgramadaOrdemServico: codigo_pcm_programada_ordem_servico,
                                                      iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()));

                    return RedirectToAction(view, control);
                }
            }

        #endregion

        #region ::: HISTÓRICO :::

        // GET: PROGRAMADA - HISTÓRICO - LAUDO
        public ActionResult HistoricoLaudo(string formulario = "pcm_apontamento_laudo")
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
                bool imprimir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: formulario,
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bImprimir: ref imprimir,
                                    bAdministrador: ref administrador);

                ViewBag.formulario = formulario;
                ViewBag.view = "HistoricoLaudo";
                ViewBag.editar = editar;
                ViewBag.imprimir = imprimir;
                ViewBag.data_inicio = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddMonths(-1).ToShortDateString();
                ViewBag.data_termino = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", Convert.ToInt32(Session["codigo_modulo"].ToString()));

                return View(oPCM.LoadPCMProgramadaHistorico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                            lCodigoProgramada: -1,
                                                            sDataInicio: TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddMonths(-1).ToShortDateString(),
                                                            sDataTermino: TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString(),
                                                            sFormulario: formulario));

            }
        }

        // GET: CHECKLIST HISTÓRICO - LAUDO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HistoricoLaudo(string data_inicio, string data_termino, string formulario, int unidade = -1, int modulo = -1)
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
                bool imprimir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: formulario,
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                ViewBag.formulario = formulario;
                ViewBag.view = "HistoricoLaudo";
                ViewBag.editar = editar;
                ViewBag.imprimir = imprimir;
                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", unidade);
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", modulo);

                return View(oPCM.LoadPCMProgramadaHistorico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            iCodigoModulo: modulo,
                                                            lCodigoProgramada: -1,
                                                            sDataInicio: data_inicio,
                                                            sDataTermino: data_termino,
                                                            sFormulario: formulario));

            }
        }

        // GET: PROGRAMADA - HISTÓRICO - PREVENTIVA
        public ActionResult HistoricoPreventiva(string formulario = "pcm_apontamento_preventiva")
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
                bool imprimir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: formulario,
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bImprimir: ref imprimir,
                                    bAdministrador: ref administrador);

                ViewBag.formulario = formulario;
                ViewBag.view = "HistoricoPreventiva";
                ViewBag.editar = editar;
                ViewBag.imprimir = imprimir;
                ViewBag.data_inicio = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddMonths(-1).ToShortDateString();
                ViewBag.data_termino = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", Convert.ToInt32(Session["codigo_modulo"].ToString()));
                ViewBag.programada = new SelectList(oCombo.Preventiva(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                      iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);

                return View(oPCM.LoadPCMProgramadaHistorico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                            lCodigoProgramada: -1,
                                                            sDataInicio: TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddMonths(-1).ToShortDateString(),
                                                            sDataTermino: TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString(),
                                                            sFormulario: formulario));

            }
        }

        // GET: CHECKLIST HISTÓRICO - PREVENTIVA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HistoricoPreventiva(string data_inicio, string data_termino, string formulario, int unidade = -1, int modulo = -1, int programada = -1)
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
                bool imprimir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: formulario,
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                ViewBag.formulario = formulario;
                ViewBag.view = "HistoricoPreventiva";
                ViewBag.editar = editar;
                ViewBag.imprimir = imprimir;
                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", unidade);

                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", modulo);
                ViewBag.programada = new SelectList(oCombo.Preventiva(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                      iCodigoUnidade: unidade), "codigo", "descricao", programada);

                return View(oPCM.LoadPCMProgramadaHistorico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            iCodigoModulo: modulo,
                                                            lCodigoProgramada: programada,
                                                            sDataInicio: data_inicio,
                                                            sDataTermino: data_termino,
                                                            sFormulario: formulario));

            }
        }

        // GET: PROGRAMADA - HISTÓRICO - ROTINA
        public ActionResult HistoricoRotina(string formulario = "pcm_apontamento_rotina")
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
                bool imprimir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: formulario,
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bImprimir: ref imprimir,
                                    bAdministrador: ref administrador);

                ViewBag.formulario = formulario;
                ViewBag.view = "HistoricoRotina";
                ViewBag.editar = editar;
                ViewBag.imprimir = imprimir;
                ViewBag.data_inicio = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddMonths(-1).ToShortDateString();
                ViewBag.data_termino = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", Convert.ToInt32(Session["codigo_modulo"].ToString()));
                ViewBag.programada = new SelectList(oCombo.Rotina(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                  iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);


                return View(oPCM.LoadPCMProgramadaHistorico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                            lCodigoProgramada: -1,
                                                            sDataInicio: TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddMonths(-1).ToShortDateString(),
                                                            sDataTermino: TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString(),
                                                            sFormulario: formulario));

            }
        }

        // GET: CHECKLIST HISTÓRICO - ROTINA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HistoricoRotina(string data_inicio, string data_termino, string formulario, int unidade = -1, int modulo = -1, long programada = -1)
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
                bool imprimir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: formulario,
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                ViewBag.formulario = formulario;
                ViewBag.view = "HistoricoRotina";
                ViewBag.editar = editar;
                ViewBag.imprimir = imprimir;
                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", unidade);
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", modulo);
                ViewBag.programada = new SelectList(oCombo.Rotina(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                  iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);

                return View(oPCM.LoadPCMProgramadaHistorico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            iCodigoModulo: modulo,
                                                            lCodigoProgramada: programada,
                                                            sDataInicio: data_inicio,
                                                            sDataTermino: data_termino,
                                                            sFormulario: formulario));

            }
        }

        // GET: /PRINT
        public ActionResult PrintReportProgramada(long codigo_pcm_programada_ordem_servico, int unidade)
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

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), "RPT000000035.rpt"));

            oCrDatabase = oReportDocument.Database;
            oCrTables = oCrDatabase.Tables;

            foreach (Table crTable in oCrTables)
            {
                oTableLogOnInfo = crTable.LogOnInfo;
                oTableLogOnInfo.ConnectionInfo = oConnectionInfo;
                crTable.ApplyLogOnInfo(oTableLogOnInfo);
            }

            oReportDocument.SetParameterValue("@codigo_unidade", unidade);
            oReportDocument.SetParameterValue("@codigo_empresa", Convert.ToInt32(Session["empresa"].ToString()));
            oReportDocument.SetParameterValue("@codigo_pcm_programada_ordem_servico", codigo_pcm_programada_ordem_servico);

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=RPT000000035.pdf");
            return File(stream, "application/pdf;");
        }

        #endregion

        #region ::: MANUTENÇÃO LAUDO :::

        // GET: INDEX
        public ActionResult ManutencaoLaudo(bool rotina = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool imprimir = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "pcm_manutencao_preventiva",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());                    
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", Convert.ToInt32(Session["codigo_modulo"].ToString()));

                ViewBag.data_filtro = String.Concat(System.DateTime.Now.Date.Month.ToString("00"), '/', System.DateTime.Now.Date.Year);

                int month = System.DateTime.Now.Date.Month;
                string[] mes = new string[12];

                for (int i = 0; i < 12; i++)
                {
                    mes[i] = String.Concat(CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(((month + i) > 12) ? month + i - 12 : month + i), '/', Convert.ToString(((month + i) > 12) ? System.DateTime.Now.Date.Year + 1 : System.DateTime.Now.Date.Year));
                }

                ViewBag.data_atual = System.DateTime.Now.Date.AddHours(-5);
                ViewBag.data = mes;

                return View(oPCM.ManutencaoLaudo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                    iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                    sData: System.DateTime.Now.Date.ToShortDateString(),
                                                    iCodigoTipoOrdemServico: 6,
                                                    bRotina: rotina));
            }
        }

        // POST: /INDEX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManutencaoLaudo(string data, int unidade = -1, int modulo = -1, bool rotina = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool imprimir = false;

                int month = Convert.ToInt32(data.Split('/')[0]);
                string[] mes = new string[12];

                for (int i = 0; i < 12; i++)
                {
                    mes[i] = String.Concat(CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(((month + i) > 12) ? month + i - 12 : month + i), '/', Convert.ToString(((month + i) > 12) ? Convert.ToInt32(data.Split('/')[1]) + 1 : Convert.ToInt32(data.Split('/')[1])));
                }

                ViewBag.data = mes;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "pcm_manutencao_programada",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                  iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", modulo);

                ViewBag.data_filtro = data;

                return View(oPCM.ManutencaoLaudo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: unidade,
                                                    iCodigoModulo: modulo,
                                                    sData: data,
                                                    iCodigoTipoOrdemServico: 6,
                                                    bRotina: rotina));
            }
        }

        #endregion

        #region ::: MANUTENÇÃO PREVENTIVA :::

        // GET: INDEX
        public ActionResult ManutencaoPreventiva(int status = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool imprimir = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "pcm_manutencao_preventiva",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;

                ManutencaoPreventivaStatus statusPreventiva = new ManutencaoPreventivaStatus();

                statusPreventiva.pendente = 0;
                statusPreventiva.atrasado = 0;
                statusPreventiva.concluido = 0;
                statusPreventiva.emAndamento = 0;

                List<ManutencaoPreventiva> preventiva = new List<ManutencaoPreventiva>();
                preventiva = oPCM.ManutencaoPreventiva(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                       iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                       iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                       iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                       iStatus: status,
                                                       oManutencaoPreventivaStatus: ref statusPreventiva);

                ViewBag.status = statusPreventiva;

                return View(preventiva);
            }
        }

        // POST: /PRINT
        public ActionResult PrintPreventiva(int unidade, long codigo)
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

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), "RPT000000021.rpt"));

            oCrDatabase = oReportDocument.Database;
            oCrTables = oCrDatabase.Tables;

            foreach (Table crTable in oCrTables)
            {
                oTableLogOnInfo = crTable.LogOnInfo;
                oTableLogOnInfo.ConnectionInfo = oConnectionInfo;
                crTable.ApplyLogOnInfo(oTableLogOnInfo);
            }

            oReportDocument.SetParameterValue("usuario", Session["nome"].ToString());
            oReportDocument.SetParameterValue("@codigo_unidade", unidade);
            oReportDocument.SetParameterValue("@codigo_empresa", Convert.ToInt32(Session["empresa"].ToString()));
            oReportDocument.SetParameterValue("@codigo_pcm_programada_ordem_servico", codigo);

            oReportDocument.SetDatabaseLogon(ConfigurationManager.AppSettings.GetValues("user_id")[0],
                                             ConfigurationManager.AppSettings.GetValues("password")[0],
                                             ConfigurationManager.AppSettings.GetValues("data_source")[0],
                                             ConfigurationManager.AppSettings.GetValues("initial_catalog")[0]);

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=RPT000000021.pdf");
            return File(stream, "application/pdf;");
        }

        #endregion

        #region ::: CRONOGRAMA MANUTENÇÃO PREVENTIVA :::

        // GET: INDEX
        public ActionResult CronogramaManutencaoPreventiva(bool rotina = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool imprimir = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "pcm_manutencao_preventiva",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);
                
                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", Convert.ToInt32(Session["codigo_modulo"].ToString()));
                ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.status = new SelectList(oCombo.StatusPreventiva(), "codigo", "descricao", null);
                ViewBag.data_filtro = String.Concat(System.DateTime.Now.Date.Month.ToString("00"), '/', System.DateTime.Now.Date.Year);
    
                int month = System.DateTime.Now.Date.Month;
                string[] mes = new string[12];

                for (int i = 0; i < 12; i++)
                {
                    mes[i] = String.Concat(CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(((month + i) > 12) ? month + i - 12 : month + i), '/', Convert.ToString(((month + i) > 12) ? System.DateTime.Now.Date.Year + 1 : System.DateTime.Now.Date.Year));
                }

                ViewBag.data = mes;

                return View(oPCM.CronogramaManutencaoPreventiva(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                iCodigoSetor: -1,
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                sData: System.DateTime.Now.Date.ToShortDateString(),
                                                                iCodigoTipoOrdemServico: 3,
                                                                bRotina: rotina,
                                                                iStatus: 0));
            }
        }

        // POST: /INDEX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CronogramaManutencaoPreventiva(string data, int unidade = -1, int modulo = -1, bool rotina = false, int status = 0, int setor = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool imprimir = false;
                
                int month = Convert.ToInt32(data.Split('/')[0]);                
                string[] mes = new string[12];

                for(int i = 0; i< 12; i++)
                {
                    mes[i] = String.Concat(CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(((month + i) > 12)? month + i - 12: month + i), '/', Convert.ToString(((month + i) > 12)? Convert.ToInt32(data.Split('/')[1]) + 1: Convert.ToInt32(data.Split('/')[1])));
                }

                ViewBag.data = mes;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "pcm_manutencao_programada",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);
                
                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", modulo);
                ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade), "codigo", "descricao", setor);
                ViewBag.status = new SelectList(oCombo.StatusPreventiva(), "codigo", "descricao", status);
                ViewBag.data_filtro = data;

                return View(oPCM.CronogramaManutencaoPreventiva(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: unidade,
                                                                iCodigoModulo: modulo,
                                                                iCodigoSetor: setor,
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                sData: data,
                                                                iCodigoTipoOrdemServico: 3,
                                                                bRotina: rotina,
                                                                iStatus: status));
            }
        }

        #endregion

        #region ::: MANUTENÇÃO ROTINA :::

        // GET: INDEX
        public ActionResult ManutencaoRotina2()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool imprimir = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "pcm_manutencao_rotina",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", Convert.ToInt32(Session["codigo_modulo"].ToString()));

                ViewBag.data_filtro = String.Concat(System.DateTime.Now.Date.Month.ToString("00"), '/', System.DateTime.Now.Date.Year);
                ViewBag.dia_atual = DateTime.Now.Day;
                ViewBag.maior_dia = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

                return View(oPCM.ManutencaoRotina(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                    iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                    iMes: DateTime.Now.Month,
                                                    iAno: DateTime.Now.Year));
            }
        }

        // POST: /INDEX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManutencaoRotina2(string data, int unidade = -1, int modulo = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool imprimir = false;

                int month = Convert.ToInt32(data.Split('/')[0]);
                int year = Convert.ToInt32(data.Split('/')[1]);

                ViewBag.data = data;
                ViewBag.dia_atual = (month == DateTime.Now.Month && year == DateTime.Now.Year)? DateTime.Now.Day: -1;
                ViewBag.maior_dia = DateTime.DaysInMonth(year, month);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "pcm_manutencao_rotina",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", modulo);

                ViewBag.data_filtro = data;

                return View(oPCM.ManutencaoRotina(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: unidade,
                                                    iCodigoModulo: modulo,
                                                    iMes: month,
                                                    iAno: year));
            }
        }

        // GET: INDEX
        public ActionResult ManutencaoRotina(int status = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool imprimir = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "pcm_manutencao_rotina",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.status = oPCM.LoadManutencaoRotinaStatus(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                    iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()));

                return View(oPCM.LoadManutencaoRotina(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                        iStatus:status));
            }
        }

        #endregion

        #region ::: MANUTENÇÃO APONTAMENTO :::

        // GET: INDEX
        public ActionResult ManutencaoApontamento(int codigo_unidade, string data_inicio, string data_termino, int mes, int ano, int codigo_funcionario = -1, long codigo_equipamento = -1)
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
                string total = "";

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "pcm_apontamento_os_edit",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                ViewBag.administrador = administrador;

                ViewBag.codigo_unidade = codigo_unidade;
                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;
                ViewBag.mes = mes;
                ViewBag.ano = ano;
                ViewBag.codigo_funcionario = codigo_funcionario;
                ViewBag.codigo_equipamento = codigo_equipamento;

                List<ManutencaoApontamento> manutencaoApontamentos = oPCM.ManutencaoApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                iCodigoUnidade: codigo_unidade,
                                                                                                iCodigoFuncionario: codigo_funcionario,
                                                                                                lCodigoEquipamento: codigo_equipamento,
                                                                                                sDataInicio: data_inicio,
                                                                                                sDataTermino: data_termino,
                                                                                                iMes: mes,
                                                                                                iAno: ano,
                                                                                                sTotal: ref total);

                ViewBag.total = total;

                return View(manutencaoApontamentos);
            }
        }
       
        // GET: /INDEX
        public ActionResult ManutencaoApontamento2()
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
                string total = "";

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "pcm_apontamento_os_edit",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                ViewBag.administrador = administrador;
                ViewBag.data_inicio = System.DateTime.Now.Date.AddMonths(-1).ToShortDateString();
                ViewBag.data_termino = System.DateTime.Now.Date.ToShortDateString();

                ViewBag.codigo_unidade = Convert.ToInt32(Session["codigo_unidade"]);
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Convert.ToInt32(Session["codigo_unidade"]));
                ViewBag.equipamento = new SelectList(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"]),
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", null);

                List<ManutencaoApontamento> manutencaoApontamentos = oPCM.ManutencaoApontamento2(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                                    iCodigoFuncionario: -1,
                                                                                                    lCodigoEquipamento: -1,
                                                                                                    sDataInicio: "",
                                                                                                    sDataTermino: "",
                                                                                                    sTotal: ref total);

                ViewBag.total = total;

                return View(manutencaoApontamentos);
            }
        }

        // POST: /INDEX
        [HttpPost]
        public ActionResult ManutencaoApontamento2(int unidade, string data_inicio, string data_termino, int funcionario = -1, long equipamento = -1)
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
                    string total = "";

                    oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        sFormulario: "pcm_apontamento_os_edit",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador,
                                        bImprimir: ref imprimir);

                    ViewBag.administrador = administrador;

                    ViewBag.codigo_unidade = unidade;
                    ViewBag.data_inicio = data_inicio;
                    ViewBag.data_termino = data_termino;
                    ViewBag.codigo_funcionario = funcionario;
                    ViewBag.codigo_equipamento = equipamento;

                    ViewBag.codigo_unidade = Convert.ToInt32(Session["codigo_unidade"]);
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: false), "codigo", "descricao", unidade);
                    ViewBag.equipamento = new SelectList(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: unidade), "codigo", "descricao", equipamento);
                    ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: unidade,
                                                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", funcionario);

                    List<ManutencaoApontamento> manutencaoApontamentos = oPCM.ManutencaoApontamento2(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                     iCodigoUnidade: unidade,
                                                                                                     iCodigoFuncionario: funcionario,
                                                                                                     lCodigoEquipamento: equipamento,
                                                                                                     sDataInicio: data_inicio,
                                                                                                     sDataTermino: data_termino,
                                                                                                     sTotal: ref total);

                    ViewBag.total = total;

                    return View(manutencaoApontamentos);
                }
            }

        #endregion

        #region ::: MANUTENÇÃO APONTAMENTO - CUSTO :::

        // GET: INDEX
        public ActionResult ManutencaoApontamentoCusto(int codigo_unidade, string data_inicio, string data_termino, int mes, int ano, int codigo_funcionario = -1, long codigo_equipamento = -1)
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
                    string total = "";

                    oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        sFormulario: "pcm_apontamento_os_edit",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador,
                                        bImprimir: ref imprimir);

                    ViewBag.administrador = administrador;

                    List<ManutencaoApontamento> manutencaoApontamentos = oPCM.ManutencaoApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                    iCodigoUnidade: codigo_unidade,
                                                                                                    iCodigoFuncionario: codigo_funcionario,
                                                                                                    lCodigoEquipamento: codigo_equipamento,
                                                                                                    sDataInicio: data_inicio,
                                                                                                    sDataTermino: data_termino,
                                                                                                    iMes: mes,
                                                                                                    iAno: ano,
                                                                                                    sTotal: ref total);

                    ViewBag.total = total;

                    return View(manutencaoApontamentos);
                }
            }

        #endregion

        #region ::: REQUISIÇÃO :::

        // GET: INSERT
        public ActionResult Requisicao()
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
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"])), "codigo", "descricao", null);
                    ViewBag.equipamento = new SelectList(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                    ViewBag.apartamento = new SelectList(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                    ViewBag.prioridade = new SelectList(oCombo.Prioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                    ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();

                    return View();
                }
            }

        //JSON: /INSERT - REQUISIÇÃO/
        public string InsertRequisicao(int unidade, string data, int prioridade, string descricao, int setor = -1, int apartamento = -1, long equipamento = -1)
        {
            if (Session["empresa"] == null)
            {
                return "";
            }
            else
            {
                string numero_requisicao = "";
                string body = "";
                string to = "";

                oPCM.InsertRequisicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        iCodigoUnidade: unidade,
                                        iCodigoSetor: setor,
                                        iCodigoApartamento: apartamento,
                                        lCodigoEquipamento: equipamento,
                                        sData: data,
                                        iCodigoPrioridade: prioridade,
                                        sDescricao: descricao,
                                        sImagem: "",
                                        sArquivo: "",
                                        sNumeroRequisicao: ref numero_requisicao,
                                        sTo: ref to,
                                        sBody: ref body);

                if (body != "")
                {
                    string remetente = "pcm@simservices.com.br"; //O e-mail do remetente
                    MailMessage mail = new MailMessage();
                    foreach (string email in to.Split(new char[] { ';' }))
                    {
                        mail.To.Add(email);
                    }

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    mail.From = new MailAddress(remetente, "PCM by SIM", System.Text.Encoding.UTF8);
                    mail.Subject = String.Concat("Nº Requisição: ", numero_requisicao, " - ", DateTime.Now.ToShortTimeString());
                    mail.SubjectEncoding = System.Text.Encoding.UTF8;
                    mail.Body = body;
                    mail.BodyEncoding = System.Text.Encoding.UTF8;
                    mail.IsBodyHtml = true;
                    mail.Priority = MailPriority.High;
                    SmtpClient client = new SmtpClient();
                    client.Credentials = new System.Net.NetworkCredential(remetente, "p@ssw0rd013459");
                    client.Port = 587;
                    client.Host = "smtp.office365.com";
                    client.EnableSsl = true;

                    try
                    {
                        client.Send(mail);
                    }
                    catch (Exception ex)
                    {
                    }
                }

                //Retorna Ordem de Serviço
                return numero_requisicao;
            }
        }

        // GET: INDEX
        public ActionResult RequisicaoAprovarReprovarIndex(int status = 1)
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
                                    sFormulario: "requisicao_aprovar_reprovar",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                string data_inicio = (status == -1) ? TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString() : "";
                string data_termino = (status == -1) ? TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString() : "";

                ViewBag.administrador = administrador;
                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.data_inicio = "";
                ViewBag.data_termino = "";
                ViewBag.numero_requisicao = "";
                ViewBag.codigo_unidade = Convert.ToInt32(Session["codigo_unidade"]);
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Convert.ToInt32(Session["codigo_unidade"]));
                ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"])), "codigo", "descricao", null);
                ViewBag.apartamento = new SelectList(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"]), iCodigoSetor: -1), "codigo", "descricao", null);
                ViewBag.equipamento = new SelectList(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.prioridade = new SelectList(oCombo.Prioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"])), "codigo", "descricao", null);
                ViewBag.solicitante = new SelectList(oCombo.Solicitante(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"])), "codigo", "descricao", null);
                ViewBag.status = new SelectList(oCombo.StatusRequisicao(), "codigo", "descricao", status);

                return View(oPCM.RequisicaoAprovarReprovarIndex(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                sDataInicio: data_inicio,
                                                                sDataTermino: data_termino,
                                                                iStatus: status));
            }
        }

        // POST: INDEX
        [HttpPost]
        public ActionResult RequisicaoAprovarReprovarIndex(int unidade = -1, int codigo_unidade = -1, string data_inicio = "", string data_termino = "", string numero_requisicao = "", int setor = -1, int prioridade = -1, long equipamento = -1, int status = -1, int solicitante = -1, int apartamento = -1)
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
                                    sFormulario: "requisicao_aprovar_reprovar",
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
                ViewBag.codigo_unidade = unidade;
                ViewBag.numero_requisicao = numero_requisicao;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", (unidade == -1) ? codigo_unidade : unidade);
                ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade), "codigo", "descricao", setor);
                ViewBag.apartamento = new SelectList(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: unidade, iCodigoSetor: setor), "codigo", "descricao", apartamento);
                ViewBag.equipamento = new SelectList(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: unidade), "codigo", "descricao", equipamento);
                ViewBag.prioridade = new SelectList(oCombo.Prioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: unidade), "codigo", "descricao", prioridade);
                ViewBag.solicitante = new SelectList(oCombo.Solicitante(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                        iCodigoUnidade: unidade), "codigo", "descricao", solicitante);
                ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"]),
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", null);
                ViewBag.status = new SelectList(oCombo.StatusManutencao(), "codigo", "descricao", status);

                return View(oPCM.RequisicaoAprovarReprovarIndex(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                iCodigoUnidade: (unidade == -1) ? codigo_unidade : unidade,
                                                                sNumeroRequisicao: numero_requisicao,
                                                                sDataInicio: data_inicio,
                                                                sDataTermino: data_termino,
                                                                iCodigoSetor: setor,
                                                                iCodigoPrioridade: prioridade,
                                                                lCodigoEquipamento: equipamento,
                                                                iCodigoApartamento: apartamento,
                                                                iCodigoSolicitante: solicitante,
                                                                iStatus: status));
            }
        }

        // POST: APROVAR
        [HttpPost]
        public ActionResult RequisicaoAprovarReprovar(string codigo, int status, string justificativa)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    foreach (string codigos in codigo.Split(new char[] { ',' }))
                    {
                        if (codigos != "0")
                        {

                            oPCM.RequisicaoAprovarReprovar(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                           iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                           iCodigoUnidade: Convert.ToInt32(codigos.Split(new char[] { '|' })[1]),
                                                           lCodigo: Convert.ToInt64(codigos.Split(new char[] { '|' })[0]),
                                                           iStatus: status,
                                                           sJustificativa: justificativa);
                        }
                    }

                    return RedirectToAction("RequisicaoAprovarReprovarIndex", "PCM");
                }
            }

        #endregion

        #region ::: CRONOGRAMA :::

        // GET: INSERT
        public ActionResult CronogramaSemana(int unidade = -1, int tipo_programada = -1, int other = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                unidade = (unidade == -1) ? Convert.ToInt32(Session["codigo_unidade"].ToString()) : unidade;
                tipo_programada = (tipo_programada == -1) ? 3 : tipo_programada;

                DateTimeFormatInfo oDateTimeFormatInfo = DateTimeFormatInfo.CurrentInfo;
                Calendar oCalendar = oDateTimeFormatInfo.Calendar;
                ViewBag.numero_semanas = oCalendar.GetWeekOfYear(new System.DateTime(DateTime.Now.Year, 12, 31), CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday);
                ViewBag.semana_atual = oCalendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday);
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.semana = new SelectList(oCombo.Semana(), "codigo", "descricao", oCalendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday));
                ViewBag.tipo_programada = new SelectList(oCombo.TipoProgramada(), "codigo", "descricao", tipo_programada);

                return View(oPCM.CronogramaSemana(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: unidade,
                                                    iTipo: tipo_programada));
            }
        }

        // GET: INSERT
        [HttpPost]
        public ActionResult CronogramaSemana(int unidade, int tipo_programada)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                DateTimeFormatInfo oDateTimeFormatInfo = DateTimeFormatInfo.CurrentInfo;
                Calendar oCalendar = oDateTimeFormatInfo.Calendar;
                ViewBag.numero_semanas = oCalendar.GetWeekOfYear(new System.DateTime(DateTime.Now.Year, 12, 31), CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday);
                ViewBag.semana_atual = oCalendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday);
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.semana = new SelectList(oCombo.Semana(), "codigo", "descricao", oCalendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday));
                ViewBag.tipo_programada = new SelectList(oCombo.TipoProgramada(), "codigo", "descricao", tipo_programada);

                return View(oPCM.CronogramaSemana(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: unidade,
                                                    iTipo: tipo_programada));
            }
        }

        // GET: INSERT
        [HttpPost]
        public ActionResult CronogramaSemanaData(int semana, long codigo_pcm_programada, int codigo_unidade, int codigo_tipo_programada)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {

                    //Insere Data de Início
                    oPCM.CronogramaSemanaData(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                              iCodigoUnidade: codigo_unidade,
                                              lCodigoPCMProgramada: codigo_pcm_programada,
                                              iSemana: semana);

                    return RedirectToAction("CronogramaSemana", "PCM", new { unidade = codigo_unidade, tipo_programada = codigo_tipo_programada });
                }
            }

        #endregion

        #region "::: PLANO DE AÇÃO :::

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
                    ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"])), "codigo", "descricao", null);
                    ViewBag.equipamento = new SelectList(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                    ViewBag.apartamento = new SelectList(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                    ViewBag.prioridade = new SelectList(oCombo.Prioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                          iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                    ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();

                    return View();
                }
            }

        #endregion

    }
}