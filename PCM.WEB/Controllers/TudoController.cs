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

        #endregion
    }
}
