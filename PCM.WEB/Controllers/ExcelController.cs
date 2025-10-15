using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using PCM.WEB.DAL;
using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;

namespace PCM.WEB.Controllers
{
    public class ExcelController : Controller
    {
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.Excel oExcel = new DAL.Excel(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Qualidade oQualidade = new Qualidade(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: JSON :::

            //JSON: /SETOR/
            public JsonResult LoadSetor(int unidade)
            {
                return Json(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                         iCodigoUnidade: unidade));
            }

            //JSON: /EQUIPAMENTO/
            public JsonResult LoadEquipamento(int unidade)
            {
                return Json(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                               iCodigoUnidade: unidade));
            }

            //JSON: /PRIORIDADE/
            public JsonResult LoadPrioridade(int unidade)
            {
                return Json(oCombo.Prioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                              iCodigoUnidade: unidade));
            }

            //JSON: /APARTAMENTO/
            public JsonResult LoadApartamento(int unidade, int setor = -1)
            {
                return Json(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                               iCodigoUnidade: unidade,
                                               iCodigoSetor: setor));
            }

            //JSON: /APARTAMENTO/
            public JsonResult LoadSolicitante(int unidade)
            {
                return Json(oCombo.Solicitante(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                iCodigoUnidade: unidade));
            }

            //JSON: /EQUIPAMENTO - SETOR/
            public JsonResult LoadEquipamentoSetor(int unidade, int setor)
            {
                return Json(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                               iCodigoUnidade: unidade,
                                               iCodigoSetor: setor));
            }

            //JSON: /APARTAMENTO - EQUIPAMENTO/
            public JsonResult LoadEquipamentoApartamento(int unidade, int apartamento)
            {
                return Json(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                               iCodigoUnidade: unidade, iCodigoApartamento: apartamento));
            }
        
        #endregion

        #region ::: ORDEM DE SERVIÇO :::

            // GET: INDEX
            public ActionResult OrdemServico(int status = 1)
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
                    bool administrador_vincular = false;

                    oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        sFormulario: "ordem_servico",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        sFormulario: "ordem_servico_atribuir",
                                        sDireito: "administrador",
                                        bReturn: ref administrador_vincular);

                    string data_inicio = (status == -1) ? TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString() : "";
                    string data_termino = (status == -1) ? TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString() : "";

                    ViewBag.administrador_vincular = administrador_vincular;
                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;
                    ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                    ViewBag.data_inicio = "";
                    ViewBag.data_termino = "";
                    ViewBag.ordem_servico = "";
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
                    ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"]),
                                                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", null);
                    ViewBag.status = new SelectList(oCombo.StatusManutencao(), "codigo", "descricao", status);

                    return View();
                }
            }

            // VOID: ORDEM DE SERVIÇO
            [HttpPost]
            public ActionResult OrdemServico(int unidade = -1, int codigo_unidade = -1, string data_inicio = "", string data_termino = "", string ordem_servico = "", string ordem_servico_cliente = "", int setor = -1, int prioridade = -1, long equipamento = -1, int status = -1, string executor = "", int solicitante = -1, int apartamento = -1)
            {

                using (var stream = oExcel.OrdemServico(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        codigoUnidade: (unidade == -1) ? codigo_unidade : unidade,
                                                        numeroOrdemServico: ordem_servico,
                                                        ordemServicoCliente: ordem_servico_cliente,
                                                        dataInicio: data_inicio,
                                                        dataTermino: data_termino,
                                                        codigoSetor: setor,
                                                        codigoPrioridade: prioridade,
                                                        codigoEquipamento: equipamento,
                                                        codigoApartamento: apartamento,
                                                        executor: executor,
                                                        codigoSolicitante: solicitante,
                                                        status: status))
            {
                var content = stream.ToArray();
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ORDEM SERVIÇO.xlsx");
            }

            }

        #endregion

        #region ::: PLANO DE AÇÃO - QA :::

            // GET: PLANO DE AÇÃO - QUALIDADE
            public ActionResult PlanoAcaoQA()
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    string data_inicio = DateTime.Now.AddDays(-7).ToShortDateString();
                    string data_termino = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();

                    ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                    ViewBag.data_inicio = data_inicio;
                    ViewBag.data_termino = data_termino;
                    ViewBag.ordem_servico = "";
                    ViewBag.codigo_unidade = Convert.ToInt32(Session["codigo_unidade"]);
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: false), "codigo", "descricao", Convert.ToInt32(Session["codigo_unidade"]));
                    ViewBag.auditoria = new SelectList(oCombo.AuditoriaQualidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                 iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                                 iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"])), "codigo", "descricao", null);
                    ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                              iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", null);
                    ViewBag.prioridade = new SelectList(oCombo.Prioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                          iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"])), "codigo", "descricao", null);
                    ViewBag.solicitante = new SelectList(oCombo.Solicitante(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"])), "codigo", "descricao", null);
                    ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"]),
                                                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", null);
                    ViewBag.status = new SelectList(oCombo.StatusManutencao(), "codigo", "descricao", null);

                    ViewBag.plano_acao_status = oQualidade.LoadPlanoAcaoStatus(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                               iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()));

                    return View();
                }
            }

            // VOID: ORDEM DE SERVIÇO
            [HttpPost]
            public void PlanoAcaoQA(int unidade = -1, int codigo_unidade = -1, string data_inicio = "", string data_termino = "", string ordem_servico = "", int auditoria_interna = -1, int prioridade = -1, int departamento = -1, int status = -1, string executor = "", int solicitante = -1)
            {

                var data = oExcel.PlanoAcaoQA(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                              iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                              iCodigoUnidade: (unidade == -1) ? codigo_unidade : unidade,
                                              sOrdemServico: ordem_servico,
                                              sDataInicio: data_inicio,
                                              sDataTermino: data_termino,
                                              iCodigoAuditoria: auditoria_interna,
                                              iCodigoPrioridade: prioridade,
                                              iCodigoDepartamento: departamento,
                                              iCodigoSolicitante: solicitante,
                                              sExecutor: executor,
                                              iStatus: status);

                ExcelPackage excel = new ExcelPackage();
                var workSheet = excel.Workbook.Worksheets.Add("Plan1");
                workSheet.Cells["A1"].LoadFromCollection(Collection: data, PrintHeaders: true);
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=PlanoAcao.xlsx");
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }

        #endregion

        #region ::: CHECKLIST :::

        #endregion

    }
}