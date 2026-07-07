using Microsoft.AspNet.Identity;
using PCM.WEB.DAL;
using PCM.WEB.MODELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace PCM.WEB.Controllers
{
    public class TudoController : Controller
    {
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.Tudo oTudo = new DAL.Tudo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

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
        public ActionResult ChecklistTudoApontamento(int codigo_unidade, int codigo_apartamento, long codigo_apontamento = 0, int status = 0)
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

                var apontamento = oTudo.LoadTudoApontamento(codigoEmpresa: empresa,
                                                            codigoUnidade: codigo_unidade,
                                                            codigoApartamento: codigo_apartamento,
                                                            codigo: codigo_apontamento);

                ViewBag.apontamento = apontamento;
                ViewBag.status = status;
                ViewBag.somenteLeitura = (status == 5);
                ViewBag.codigo_unidade = codigo_unidade;
                ViewBag.dataHoje = DateTime.Now.ToString("dd/MM/yyyy");
                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: empresa,
                                                                        iCodigoUnidade: codigo_unidade,
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])),
                                                     "codigo", "descricao", apontamento.codigo_funcionario_responsavel);

                // itens: respondidos quando concluído (status 5); template caso contrário
                long codigoItens = (status == 5 && codigo_apontamento > 0) ? codigo_apontamento : -1;

                return View(oTudo.LoadTudoApontamentoChecklist(codigoEmpresa: empresa,
                                                               codigoUnidade: codigo_unidade,
                                                               codigoApartamento: codigo_apartamento,
                                                               codigo: codigoItens));
            }
        }

        // POST: GRAVA A EXECUÇÃO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChecklistTudoApontamento(int codigo_unidade, int codigo_apartamento, int funcionario_responsavel,
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

                long codigo = 0;
                long codigo_checklist = 0;

                oTudo.InsertTudoApontamento(codigoEmpresa: empresa,
                                            codigoUsuario: usuario,
                                            codigoUnidade: codigo_unidade,
                                            codigoApartamento: codigo_apartamento,
                                            codigoFuncionario: funcionario_responsavel,
                                            dataInicio: Convert.ToDateTime(data_inicio, ptBr),
                                            dataTermino: Convert.ToDateTime(data_termino, ptBr),
                                            horaInicio: hora_inicio,
                                            horaTermino: hora_termino,
                                            codigo: ref codigo,
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
                                                             descricaoChecklist: item.descricao,
                                                             opcao: item.opcao,
                                                             observacao: item.observacao,
                                                             novaVistoria: item.nova_vistoria);
                    }
                }

                oTudo.UpdateTudoStatus(codigoEmpresa: empresa,
                                       codigoUnidade: codigo_unidade,
                                       codigoTudoApontamento: codigo);

                return RedirectToAction("ChecklistTudo");
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
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: empresa, iCodigoUsuario: usuario, bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.apartamento = new SelectList(oCombo.Apartamento(iCodigoEmpresa: empresa, iCodigoUnidade: unidade), "codigo", "descricao", apartamento);

                return View(oTudo.LoadTudoChecklistHistorico(codigoEmpresa: empresa,
                                                             codigoUnidade: unidade,
                                                             dataInicio: data_inicio,
                                                             dataTermino: data_termino,
                                                             codigoApartamento: apartamento));
            }
        }

        #endregion
    }
}
