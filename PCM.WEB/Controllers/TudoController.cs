using Microsoft.AspNet.Identity;
using PCM.WEB.DAL;
using PCM.WEB.MODELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PCM.WEB.Controllers
{
    public class TudoController : Controller
    {
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.Tudo oTudo = new DAL.Tudo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.Picture oPicture = new DAL.Picture(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: TUDO EM DIA - CHECKLIST :::

        // GET: LISTA / PAINEL
        public ActionResult ChecklistTudo(int unidade = 0, int setor = -1, string status = "", string statusSelected = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                int empresa = Convert.ToInt32(Session["empresa"].ToString());
                int usuario = Convert.ToInt32(User.Identity.GetUserName());

                if (unidade == 0)
                    unidade = Convert.ToInt32(Session["codigo_unidade"].ToString());

                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                // Reusa a permissão do UH em Dia por ora (mesmo público). Uma permissão
                // dedicada "tudo_checklist" pode ser adicionada depois (coluna de perfil + login).
                oAccount.LoadPerfil(iCodigoEmpresa: empresa,
                                    iCodigoUsuario: usuario,
                                    sFormulario: "uh_checklist",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                // toggle do filtro de status (mesma lógica do UH em Dia)
                var filterStatus = statusSelected.Split(',').Where(s => !string.IsNullOrEmpty(s)).ToList();

                if (!string.IsNullOrEmpty(status))
                {
                    if (filterStatus.Contains(status))
                        filterStatus.Remove(status);
                    else
                        filterStatus.Add(status);
                }

                ViewBag.statusSelected = string.Join(",", filterStatus);
                ViewBag.unidadeSel = unidade;
                ViewBag.setorSel = setor;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: empresa, iCodigoUsuario: usuario, bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: empresa, iCodigoUnidade: unidade), "codigo", "descricao", setor);

                ViewBag.status = oTudo.LoadTudoChecklistStatus(codigoEmpresa: empresa,
                                                               codigoUnidade: unidade,
                                                               codigoSetor: setor);

                return View(oTudo.LoadTudoChecklist(codigoEmpresa: empresa,
                                                    codigoUnidade: unidade,
                                                    codigoSetor: setor,
                                                    status: string.Join(",", filterStatus)));
            }
        }

        // GET: EXECUÇÃO DO CHECKLIST
        public ActionResult ChecklistTudoApontamento(int codigo_unidade, int codigo_apartamento, long codigo_apontamento = 0, int status = 0, bool visualizar = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                int empresa = Convert.ToInt32(Session["empresa"].ToString());
                int usuario = Convert.ToInt32(User.Identity.GetUserName());

                bool editar = false, inserir = false, excluir = false, administrador = false;
                oAccount.LoadPerfil(iCodigoEmpresa: empresa, iCodigoUsuario: usuario, sFormulario: "uh_checklist",
                                    bInserir: ref inserir, bEditar: ref editar, bExcluir: ref excluir, bAdministrador: ref administrador);

                bool somenteLeitura = visualizar || status == 5;

                long codigoApontamento = codigo_apontamento;
                long codigoItens;

                if (somenteLeitura)
                {
                    // visualização de um apontamento já concluído
                    codigoItens = codigoApontamento > 0 ? codigoApontamento : -1;
                }
                else
                {
                    // nova execução: cria (ou reaproveita) o cabeçalho para permitir anexar fotos
                    long codigoNovo = 0, checklistNovo = 0;
                    oTudo.StartTudoApontamento(codigoEmpresa: empresa,
                                               codigoUsuario: usuario,
                                               codigoUnidade: codigo_unidade,
                                               codigoApartamento: codigo_apartamento,
                                               codigo: ref codigoNovo,
                                               codigoChecklist: ref checklistNovo);
                    codigoApontamento = codigoNovo;
                    codigoItens = -1;
                }

                var apontamento = oTudo.LoadTudoApontamento(codigoEmpresa: empresa,
                                                            codigoUnidade: codigo_unidade,
                                                            codigoApartamento: codigo_apartamento,
                                                            codigo: codigoApontamento);

                ViewBag.apontamento = apontamento;
                ViewBag.status = status;
                ViewBag.somenteLeitura = somenteLeitura;
                ViewBag.codigo_unidade = codigo_unidade;
                ViewBag.codigo_apontamento = codigoApontamento;
                ViewBag.dataHoje = DateTime.Now.ToString("dd/MM/yyyy");
                ViewBag.horaAgora = DateTime.Now.ToString("HH:mm");
                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: empresa,
                                                                        iCodigoUnidade: codigo_unidade,
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])),
                                                     "codigo", "descricao", apontamento.codigo_funcionario_responsavel);

                return View(oTudo.LoadTudoApontamentoChecklist(codigoEmpresa: empresa,
                                                               codigoUnidade: codigo_unidade,
                                                               codigoApartamento: codigo_apartamento,
                                                               codigo: codigoItens));
            }
        }

        // POST: GRAVA A EXECUÇÃO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChecklistTudoApontamento(int codigo_unidade, int codigo_apartamento, long codigo_apontamento, int funcionario_responsavel,
                                                     string data_inicio, string data_termino, List<TudoApontamentoChecklist> checklist,
                                                     string hora_inicio = "", string hora_termino = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                int empresa = Convert.ToInt32(Session["empresa"].ToString());
                int usuario = Convert.ToInt32(User.Identity.GetUserName());
                var ptBr = new System.Globalization.CultureInfo("pt-BR");

                long codigo = codigo_apontamento;
                long codigo_checklist = 0;

                oTudo.FinalizaTudoApontamento(codigoEmpresa: empresa,
                                              codigoUsuario: usuario,
                                              codigoUnidade: codigo_unidade,
                                              codigoApartamento: codigo_apartamento,
                                              codigo: codigo,
                                              codigoFuncionario: funcionario_responsavel,
                                              dataInicio: Convert.ToDateTime(data_inicio, ptBr),
                                              dataTermino: Convert.ToDateTime(data_termino, ptBr),
                                              horaInicio: hora_inicio,
                                              horaTermino: hora_termino,
                                              codigoChecklist: ref codigo_checklist);

                if (checklist != null)
                {
                    foreach (var item in checklist)
                    {
                        oTudo.InsertTudoApontamentoChecklist(codigoEmpresa: empresa,
                                                             codigoUnidade: codigo_unidade,
                                                             codigoTudoApontamento: codigo,
                                                             codigoChecklist: codigo_checklist,
                                                             codigoChecklistItem: item.codigo,
                                                             codigoTipoItem: item.codigo_tipo_item_checklist,
                                                             descricaoChecklist: item.descricao,
                                                             opcao: item.opcao,
                                                             resposta: item.resposta,
                                                             observacao: item.observacao,
                                                             abreOs: item.abre_os,
                                                             novaVistoria: item.nova_vistoria);
                    }
                }

                oTudo.UpdateTudoStatus(codigoEmpresa: empresa,
                                       codigoUnidade: codigo_unidade,
                                       codigoTudoApontamento: codigo);

                return RedirectToAction("ChecklistTudo");
            }
        }

        // POST: GRID DO HISTÓRICO (loadGridMain)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult LoadTudoHistorico(int unidade = -1, int apartamento = -1, string data_inicio = null, string data_termino = null)
        {
            int empresa = Convert.ToInt32(Session["empresa"].ToString());

            if (unidade <= 0)
                unidade = Convert.ToInt32(Session["codigo_unidade"].ToString());

            if (string.IsNullOrEmpty(data_inicio))
                data_inicio = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy");
            if (string.IsNullOrEmpty(data_termino))
                data_termino = DateTime.Now.ToString("dd/MM/yyyy");

            var data = oTudo.LoadTudoHistoricoGrid(codigoEmpresa: empresa,
                                                   codigoUnidade: unidade,
                                                   codigoApartamento: apartamento,
                                                   dataInicio: data_inicio,
                                                   dataTermino: data_termino);

            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue, RecursionLimit = 100 };
        }

        // POST: EXCLUI UM APONTAMENTO (recalcula a próxima execução)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirTudoApontamento(int codigo_unidade, long codigo)
        {
            try
            {
                int empresa = Convert.ToInt32(Session["empresa"].ToString());

                oTudo.DeleteTudoApontamento(codigoEmpresa: empresa,
                                            codigoUnidade: codigo_unidade,
                                            codigo: codigo);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: UPLOAD DE FOTO DE UM ITEM
        public JsonResult UploadFoto(long codigo_apontamento, int codigo_unidade, int codigo_item_checklist)
        {
            try
            {
                int empresa = Convert.ToInt32(Session["empresa"].ToString());

                string sPath = Path.Combine("C:\\SIM\\PCM\\SITE\\IMAGE\\TUDO", empresa.ToString(), codigo_unidade.ToString(), codigo_apontamento.ToString());

                if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase oHttpPostedFileBase = Request.Files[i];
                    string sFileName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(oHttpPostedFileBase.FileName);
                    if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));

                    ResizeAndSaveImage(oHttpPostedFileBase.InputStream, Path.Combine(sPath, sFileName));

                    oPicture.InsertPicture(iCodigoEmpresa: empresa,
                                           iCodigoUnidade: codigo_unidade,
                                           sTipo: "TUDO",
                                           lCodigo: codigo_apontamento,
                                           iCodigoItemChecklist: codigo_item_checklist,
                                           sImagePath: Path.Combine(sPath, sFileName));
                }

                return Json("true");
            }
            catch
            {
                return Json("false");
            }
        }

        // POST: LISTA FOTOS DE UM ITEM
        public JsonResult LoadFoto(long codigo_apontamento, int codigo_unidade, int codigo_item_checklist)
        {
            int empresa = Convert.ToInt32(Session["empresa"].ToString());

            return Json(oPicture.PictureList(iCodigoEmpresa: empresa,
                                             iCodigoUnidade: codigo_unidade,
                                             lCodigo: codigo_apontamento,
                                             sTipo: "TUDO",
                                             iCodigoItemChecklist: codigo_item_checklist));
        }

        // POST: EXCLUI FOTO DE UM ITEM
        public JsonResult ExcluirFoto(int codigo_unidade, long codigo_apontamento, int codigo_item_checklist, int codigo)
        {
            try
            {
                int empresa = Convert.ToInt32(Session["empresa"].ToString());

                oPicture.DeletePicture(iCodigoEmpresa: empresa,
                                       iCodigoUnidade: codigo_unidade,
                                       sTipo: "TUDO",
                                       lCodigoDocumento: codigo_apontamento,
                                       iCodigoItemChecklist: codigo_item_checklist,
                                       iCodigo: codigo);

                return Json(true);
            }
            catch
            {
                return Json(false);
            }
        }

        private void ResizeAndSaveImage(Stream imageStream, string outputFilePath)
        {
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

                    resizedImage.Save(outputFilePath, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }

        // GET: HISTÓRICO
        public ActionResult ChecklistTudoHistorico(int unidade = 0, int apartamento = -1, string data_inicio = null, string data_termino = null)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                int empresa = Convert.ToInt32(Session["empresa"].ToString());
                int usuario = Convert.ToInt32(User.Identity.GetUserName());

                if (unidade == 0)
                    unidade = Convert.ToInt32(Session["codigo_unidade"].ToString());

                if (string.IsNullOrEmpty(data_inicio))
                    data_inicio = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy");
                if (string.IsNullOrEmpty(data_termino))
                    data_termino = DateTime.Now.ToString("dd/MM/yyyy");

                bool editar = false, inserir = false, excluir = false, administrador = false;
                oAccount.LoadPerfil(iCodigoEmpresa: empresa, iCodigoUsuario: usuario, sFormulario: "uh_checklist",
                                    bInserir: ref inserir, bEditar: ref editar, bExcluir: ref excluir, bAdministrador: ref administrador);

                ViewBag.unidadeSel = unidade;
                ViewBag.apartamentoSel = apartamento;
                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;
                ViewBag.excluir = excluir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: empresa, iCodigoUsuario: usuario, bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.apartamento = new SelectList(oCombo.Apartamento(iCodigoEmpresa: empresa, iCodigoUnidade: unidade), "codigo", "descricao", apartamento);

                // O grid é carregado via loadGridMain (LoadTudoHistorico)
                return View();
            }
        }

        #endregion
    }
}
