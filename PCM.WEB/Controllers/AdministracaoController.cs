
using System;
using System.Web;
using System.Configuration;
using System.Web.Mvc;
using PCM.WEB.MODELS;
using PCM.WEB.DAL;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.IO;
using System.Web.UI.WebControls;
using Antlr.Runtime.Misc;

namespace PCM.WEB.Controllers
{
    public class AdministracaoController : Controller
    {
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Administracao oAdministracao = new Administracao(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private CadastroBasico oCadastroBasico = new CadastroBasico(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: JSON :::

        //JSON: /UsuarioUpdate/
        public JsonResult UsuarioUpdate(string nome, string email, string senha)
        {

            //Insere Registro no Banco de Dados
            oAdministracao.UpdateUsuario(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                         codigoUnidade: Convert.ToInt32(Session["codigo_unidade"]),
                                         senha: senha,
                                         nome: nome,
                                         email: email,
                                         codigo: Convert.ToInt32(User.Identity.GetUserName()));

            return Json(true);
        }

        //JSON: /UNIDADE/
        public JsonResult LoadUnidade()
        {
            return Json(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        bCadastro: false));
        }

        //JSON: /PERFIL/
        public JsonResult UpdatePerfil(int hierarquia, string opcao)
        {
            return Json(oAdministracao.UpdatePerfilHierarquia(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iHierarquia: hierarquia,
                                                                sOpcao: opcao));
        }

        #endregion

        #region ::: USUÁRIO :::

        // GET: INDEX
        public ActionResult UsuarioIndex()
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
                                    sFormulario: "adm_usuario",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.administrador = administrador;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", null);
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", 1);

                return View();

            }
        }

        // POST: INDEX
        [HttpPost]
        public JsonResult LoadUsuario(int unidade, string nome = "", int departamento = -1, int ativo = -1)
        {
            return Json(oAdministracao.IndexUsuario(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    codigoUnidade: unidade,
                                                    nome: nome,
                                                    codigoDepartamento: departamento,
                                                    codigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                    ativo: ativo));
        }

        // GET: INSERT
        public ActionResult UsuarioInsert()
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
                ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", null);
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: -1), "codigo", "descricao", null);
                ViewBag.modulo_default = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUsuario: -1), "codigo", "descricao", null);
                ViewBag.perfil = new SelectList(oCombo.Perfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", null);
                ViewBag.funcao = new SelectList(oCombo.Funcao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                               iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", null);
                ViewBag.tipo_funcionario = new SelectList(oCombo.TipoFuncionario(), "codigo", "descricao", null);
                return View(oAdministracao.IndexUsuarioUnidade(codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                               codigoUsuarioUpdate: -1,
                                                               codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())));
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult UsuarioInsert(string nome, string email, string email_senha, int perfil, string senha, string telefone, int[] modulo, int modulo_default, List<UsuarioUnidade> usuario_unidade, string apelido = "", bool ativo = false, int unidade = -1, int departamento = -1, bool aplicativo = false, bool website = false, bool funcionario = false, bool contabiliza_hora = false, string valor_hora = "0", int tipo_funcionario = -1, int funcao = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                int codigo = 0;

                //Insere Registro no Banco de Dados
                oAdministracao.InsertUsuario(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                             codigoUnidade: unidade,
                                             codigoPerfil: perfil,
                                             codigoDepartamento: departamento,
                                             senha: senha,
                                             nome: nome,
                                             apelido: apelido,
                                             telefone: telefone,
                                             email: email,
                                             emailSenha: email_senha,
                                             aplicativo: aplicativo,
                                             website:website,
                                             ativo: ativo,
                                             colaborador: funcionario,
                                             contabilizaHora: contabiliza_hora,
                                             valorHora: Convert.ToDouble(valor_hora),
                                             codigoTipoFuncionario: tipo_funcionario,
                                             codigoFuncao: funcao,
                                             modulo: string.Join(",", modulo),
                                             codigoModuloDefault: modulo_default,
                                             codigo: ref codigo);


                foreach (UsuarioUnidade item in usuario_unidade)
                {

                    if (item.selecionado && codigo > 0)
                    {

                        //Insere Registro no Banco de Dados
                        oAdministracao.InsertUsuarioUnidade(codigoUsuario: codigo,
                                                            codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            codigoUnidade: item.codigoUnidade);

                    }

                }


                return RedirectToAction("UsuarioInsert");
            }
        }

        // GET: /EDIT
        public ActionResult UsuarioEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Usuario usuario = null;

                oAdministracao.InfoUsuario(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                           codigo: codigo,
                                           codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                           oUsuario: ref usuario);

                if (usuario == null)
                {
                    return HttpNotFound();
                }

                ViewBag.modulo_associado = "" + usuario.modulo + "";
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", usuario.codigoUnidade);
                ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", usuario.codigoDepartamento);
                ViewBag.perfil = new SelectList(oCombo.Perfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", usuario.codigoPerfil);
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: -1), "codigo", "descricao", null);
                ViewBag.modulo_default = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUsuario: -1), "codigo", "descricao", usuario.codigoModuloDefault);
                ViewBag.funcao = new SelectList(oCombo.Funcao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                               iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", usuario.codigoFuncao);
                ViewBag.tipo_funcionario = new SelectList(oCombo.TipoFuncionario(), "codigo", "descricao", usuario.codigoTipoFuncionario);

                return View(usuario);

            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UsuarioEdit(string nome, string email, string email_senha, int perfil, string telefone, int codigo, int[] modulo, int modulo_default, List<UsuarioUnidade> unidades, string apelido = "", bool ativo = false, int unidade = -1, int departamento = -1, bool aplicativo = false, bool website = false, bool colaborador = false, bool contabilizaHoras = false, string valor_hora = "0", int tipo_funcionario = -1, int funcao = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oAdministracao.UpdateUsuario(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                             codigoUnidade: unidade,
                                             codigoPerfil: perfil,
                                             codigoDepartamento: departamento,
                                             nome: nome,
                                             apelido: apelido,
                                             telefone: telefone,
                                             email: email,
                                             emailSenha: email_senha,
                                             aplicativo: aplicativo,
                                             website: website,
                                             ativo: ativo,
                                             colaborador: colaborador,
                                             contabilizaHora: contabilizaHoras,
                                             valorHora: Convert.ToDouble(valor_hora),
                                             codigoTipoFuncionario: tipo_funcionario,
                                             codigoFuncao: funcao,
                                             modulo: string.Join(",", modulo),
                                             codigoModuloDefault: modulo_default,
                                             codigo: codigo);

                foreach (UsuarioUnidade item in unidades)
                {

                    if (item.selecionado && codigo > 0)
                    {

                        //Insere Registro no Banco de Dados
                        oAdministracao.InsertUsuarioUnidade(codigoUsuario: codigo,
                                                            codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            codigoUnidade: item.codigoUnidade);

                    }

                }

                FormularioVisualizar formulario_visualizar = new FormularioVisualizar();

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    oFormularioVisualizar: ref formulario_visualizar);

                Session["modulo"] = string.Join(",", modulo);
                Session["codigo_modulo"] = modulo_default;
                Session["adm_empresa"] = formulario_visualizar.adm_empresa;
                Session["adm_perfil"] = formulario_visualizar.adm_perfil;
                Session["adm_usuario"] = formulario_visualizar.adm_usuario;
                Session["adm_perfil"] = formulario_visualizar.adm_perfil;
                Session["adm_perfil_hierarquia"] = formulario_visualizar.adm_perfil_hierarquia;
                Session["audit_externa"] = formulario_visualizar.audit_externa;
                Session["audit_corporativo"] = formulario_visualizar.audit_corporativo;
                Session["audit_laudo"] = formulario_visualizar.audit_laudo;
                Session["audit_normas_procedimentos"] = formulario_visualizar.audit_normas_procedimentos;
                Session["audit_relatorio"] = formulario_visualizar.audit_relatorio;
                Session["audit_relatorio_mensal_pcm"] = formulario_visualizar.audit_relatorio_mensal_pcm;
                Session["audit_corporativo_historico"] = formulario_visualizar.audit_corporativo_historico;
                Session["cad_apartamento"] = formulario_visualizar.cad_apartamento;
                Session["cad_ar_condicionado"] = formulario_visualizar.cad_ar_condicionado;
                Session["cad_atividade"] = formulario_visualizar.cad_atividade;
                Session["cad_auditoria_qualidade"] = formulario_visualizar.cad_auditoria_qualidade;
                Session["cad_auditoria_corporativo"] = formulario_visualizar.cad_auditoria_corporativo;
                Session["cad_categoria"] = formulario_visualizar.cad_categoria;
                Session["adm_cliente"] = formulario_visualizar.adm_cliente;
                Session["cad_checklist"] = formulario_visualizar.cad_checklist;
                Session["cad_cliente"] = formulario_visualizar.cad_cliente;
                Session["cad_cliente_acordo_comercial"] = formulario_visualizar.cad_cliente_acordo_comercial;
                Session["cad_departamento"] = formulario_visualizar.cad_departamento;
                Session["cad_departamento_gestor"] = formulario_visualizar.cad_departamento_gestor;
                Session["cad_equipamento"] = formulario_visualizar.cad_equipamento;
                Session["cad_enxoval"] = formulario_visualizar.cad_enxoval;
                Session["cad_familia_equipamento"] = formulario_visualizar.cad_familia_equipamento;
                Session["cad_fornecedor"] = formulario_visualizar.cad_fornecedor;
                Session["cad_funcao"] = formulario_visualizar.cad_funcao;
                Session["cad_funcionario"] = formulario_visualizar.cad_funcionario;
                Session["cad_dedetizacao"] = formulario_visualizar.cad_dedetizacao;
                Session["cad_grupo_item_medicao"] = formulario_visualizar.cad_grupo_item_medicao;
                Session["cad_grupo_produto"] = formulario_visualizar.cad_grupo_produto;
                Session["cad_item_medicao"] = formulario_visualizar.cad_item_medicao;
                Session["cad_itens_gerais"] = formulario_visualizar.cad_itens_gerais;
                Session["cad_justificativa_apontamento"] = formulario_visualizar.cad_justificativa_apontamento;
                Session["cad_justificativa_falta"] = formulario_visualizar.cad_justificativa_falta;
                Session["cad_justificativa_cancelar_ordem_servico"] = formulario_visualizar.cad_justificativa_cancelar_ordem_servico;
                Session["cad_prioridade"] = formulario_visualizar.cad_prioridade;
                Session["cad_produto"] = formulario_visualizar.cad_produto;
                Session["cad_laudo"] = formulario_visualizar.cad_laudo;
                Session["adm_configuracao_desempenho_unidade"] = formulario_visualizar.adm_configuracao_desempenho_unidade;
                Session["cad_preventiva"] = formulario_visualizar.cad_preventiva;
                Session["cad_rotina"] = formulario_visualizar.cad_rotina;
                Session["cad_setor"] = formulario_visualizar.cad_setor;
                Session["cad_tarefa"] = formulario_visualizar.cad_tarefa;
                Session["cad_tipo_apartamento"] = formulario_visualizar.cad_tipo_apartamento;
                Session["cad_tipo_ar_condicionado"] = formulario_visualizar.cad_tipo_ar_condicionado;
                Session["cad_tipo_cama"] = formulario_visualizar.cad_tipo_cama;
                Session["cad_tipo_despesa"] = formulario_visualizar.cad_tipo_despesa;
                Session["cad_treinamento"] = formulario_visualizar.cad_treinamento;
                Session["cad_unidade"] = formulario_visualizar.cad_unidade;
                Session["cad_relatorio_itens_auditaveis"] = formulario_visualizar.cad_relatorio_itens_auditaveis;
                Session["cad_item_os_hospede"] = formulario_visualizar.cad_item_os_hospede;
                Session["cfg_opera"] = formulario_visualizar.cfg_opera;
                Session["fin_contrato"] = formulario_visualizar.fin_contrato;
                Session["fin_controle_gasto"] = formulario_visualizar.fin_controle_gasto;
                Session["fin_input_despesa"] = formulario_visualizar.fin_input_despesa;

                Session["governanca"] = formulario_visualizar.governanca;
                Session["gov_funcionario"] = formulario_visualizar.gov_funcionario;
                Session["gov_planejamento"] = formulario_visualizar.gov_planejamento;
                Session["gov_planejamento_historico"] = formulario_visualizar.gov_planejamento_historico;
                Session["gov_historico"] = formulario_visualizar.gov_historico;
                Session["gov_dashboard"] = formulario_visualizar.gov_dashboard;
                Session["gov_apontamento"] = formulario_visualizar.gov_apontamento;
                Session["gov_status_uh"] = formulario_visualizar.gov_status_uh;
                Session["gov_lavanderia"] = formulario_visualizar.gov_lavanderia;
                Session["gov_log_alteracao_status_gov"] = formulario_visualizar.gov_log_alteracao_status_gov;
                Session["gov_inventario_enxoval"] = formulario_visualizar.gov_inventario_enxoval;
                Session["gov_movimentacao_enxoval"] = formulario_visualizar.gov_movimentacao_enxoval;
                Session["gov_cad_tipo_perda"] = formulario_visualizar.gov_cad_tipo_perda;

                Session["green_lancamento"] = formulario_visualizar.green_lancamento;
                Session["log_book"] = formulario_visualizar.log_book;
                Session["ordem_servico"] = formulario_visualizar.ordem_servico;
                Session["ordem_servico_atribuir"] = formulario_visualizar.ordem_servico_atribuir;
                Session["uh_atividade"] = formulario_visualizar.uh_atividade;
                Session["pcm_apontamento_os_edit"] = formulario_visualizar.pcm_apontamento_os_edit;
                Session["pcm_apontamento_laudo"] = formulario_visualizar.pcm_apontamento_laudo;
                Session["pcm_apontamento_preventiva"] = formulario_visualizar.pcm_apontamento_preventiva;
                Session["pcm_apontamento_os"] = formulario_visualizar.pcm_apontamento_os;
                Session["pcm_apontamento_rotina"] = formulario_visualizar.pcm_apontamento_rotina;
                Session["pcm_cronograma_semanal"] = formulario_visualizar.pcm_cronograma_semanal;
                Session["pcm_falta"] = formulario_visualizar.pcm_falta;
                Session["pcm_manutencao_laudo"] = formulario_visualizar.pcm_manutencao_laudo;
                Session["pcm_manutencao_preventiva"] = formulario_visualizar.pcm_manutencao_preventiva;
                Session["pcm_manutencao_rotina"] = formulario_visualizar.pcm_manutencao_rotina;
                Session["pcm_apontamento_laudo"] = formulario_visualizar.pcm_apontamento_laudo;
                Session["pcm_apontamento_preventiva"] = formulario_visualizar.pcm_apontamento_preventiva;
                Session["pcm_historico_programada"] = formulario_visualizar.pcm_historico_programada;
                Session["pcm_plano_acao"] = formulario_visualizar.pcm_plano_acao;
                Session["pcm_requisicao"] = formulario_visualizar.pcm_requisicao;
                Session["pcm_requisicao_aprovar_reprovar"] = formulario_visualizar.pcm_requisicao_aprovar_reprovar;
                Session["pmoc_apontamento"] = formulario_visualizar.pmoc_apontamento;
                Session["pmoc_historico"] = formulario_visualizar.pmoc_historico;
                Session["pmoc"] = formulario_visualizar.pmoc;
                Session["pmoc_cronograma"] = formulario_visualizar.pmoc_cronograma;
                Session["pmoc_andar"] = formulario_visualizar.pmoc_andar;
                Session["pmoc_bup"] = formulario_visualizar.pmoc_bup;
                Session["pmoc_cronograma_bup"] = formulario_visualizar.pmoc_cronograma_bup;
                Session["rel_custo_horas_trabalhadas"] = formulario_visualizar.rel_custo_horas_trabalhadas;
                Session["rel_funcionario_horas_trabalhadas"] = formulario_visualizar.rel_funcionario_horas_trabalhadas;
                Session["rel_funcionario_ociosidade"] = formulario_visualizar.rel_funcionario_ociosidade;
                Session["rel_green_planet"] = formulario_visualizar.rel_green_planet;
                Session["rel_manutencao_aberto_concluido"] = formulario_visualizar.rel_manutencao_aberto_concluido;
                Session["rel_manutencao"] = formulario_visualizar.rel_manutencao;
                Session["rel_manutencao_categoria"] = formulario_visualizar.rel_manutencao_categoria;
                Session["rel_manutencao_equipamento"] = formulario_visualizar.rel_manutencao_equipamento;
                Session["rel_manutencao_executor"] = formulario_visualizar.rel_manutencao_executor;
                Session["rel_manutencao_setor"] = formulario_visualizar.rel_manutencao_setor;
                Session["rel_manutencao_solicitante"] = formulario_visualizar.rel_manutencao_solicitante;
                Session["rel_manutencao_tempo_medio_atendimento"] = formulario_visualizar.rel_manutencao_tempo_medio_atendimento;
                Session["rel_manutencao_tipo_ordem_servico"] = formulario_visualizar.rel_manutencao_tipo_ordem_servico;
                Session["rel_pmoc_mes"] = formulario_visualizar.rel_pmoc_mes;
                Session["rel_pmoc_bimestral"] = formulario_visualizar.rel_pmoc_bimestral;
                Session["rel_log_book"] = formulario_visualizar.rel_log_book;
                Session["rel_nao_conformidade"] = formulario_visualizar.rel_nao_conformidade;
                Session["rel_preventiva_mes"] = formulario_visualizar.rel_preventiva_mes;
                Session["rel_dinamico"] = formulario_visualizar.rel_dinamico;
                Session["rel_consumo_enxoval"] = formulario_visualizar.rel_consumo_enxoval;
                Session["rel_horas_trabalhadas_governanca"] = formulario_visualizar.rel_horas_trabalhadas_governanca;
                Session["rel_camareira_uh"] = formulario_visualizar.rel_camareira_uh;
                Session["rel_responsavel_vistoria_uh"] = formulario_visualizar.rel_responsavel_vistoria_uh;
                Session["rel_camareira_nc"] = formulario_visualizar.rel_camareira_nc;
                
                Session["uh_checklist"] = formulario_visualizar.uh_checklist;
                Session["uh_checklist_historico"] = formulario_visualizar.uh_checklist_historico;
                Session["uh_dedetizacao"] = formulario_visualizar.uh_dedetizacao;
                Session["uhDedetizacaoHistorico"] = formulario_visualizar.uhDedetizacaoHistorico;
                Session["uhMapaManutencaoHistorico"] = formulario_visualizar.uhMapaManutencaoHistorico;

                Session["excel_ordem_servico"] = formulario_visualizar.excel_ordem_servico;
                Session["excel_plano_acao_qa"] = formulario_visualizar.excel_plano_acao_qa;

                Session["est_listagem"] = formulario_visualizar.est_listagem;
                Session["est_requisicao_compra"] = formulario_visualizar.est_requisicao_compra;
                Session["est_ordem_compra"] = formulario_visualizar.est_ordem_compra;
                Session["est_entrada"] = formulario_visualizar.est_entrada;
                Session["est_saida"] = formulario_visualizar.est_saida;
                Session["est_inventario"] = formulario_visualizar.est_inventario;
                Session["est_entrada"] = formulario_visualizar.est_entrada;

                Session["aeb_contrato"] = formulario_visualizar.aeb_contrato;
                Session["aeb_laudo"] = formulario_visualizar.aeb_laudo;
                Session["aeb_normas_procedimentos"] = formulario_visualizar.aeb_normas_procedimentos;
                Session["aeb_auditoria_externa"] = formulario_visualizar.aeb_auditoria_externa;
                Session["qa_auditoria"] = formulario_visualizar.qa_auditoria;
                Session["qa_auditoria_historico"] = formulario_visualizar.qa_auditoria_historico;
                Session["qa_auditoria_cronograma"] = formulario_visualizar.qa_auditoria_cronograma;
                Session["qa_plano_acao"] = formulario_visualizar.qa_plano_acao;
                Session["qa_tarefa"] = formulario_visualizar.qa_tarefa;
                Session["qa_tarefa_historico"] = formulario_visualizar.qa_tarefa_historico;
                Session["dash_desempenho"] = formulario_visualizar.dash_desempenho;
                Session["dash_desempenho_qa"] = formulario_visualizar.dash_desempenho_qa;
                Session["cad_area_comum"] = formulario_visualizar.cad_area_comum;
                Session["agenda_area_comum"] = formulario_visualizar.agenda_area_comum;
                Session["agenda_entrada"] = formulario_visualizar.agenda_entrada;
                Session["agenda_saida"] = formulario_visualizar.agenda_saida;
                Session["upload_excel"] = formulario_visualizar.upload_excel;
                Session["upload_pmoc"] = formulario_visualizar.upload_pmoc;
                Session["lav_apontamento"] = formulario_visualizar.lav_apontamento;
                Session["lav_historico"] = formulario_visualizar.lav_historico;
                Session["rel_lav_controle"] = formulario_visualizar.rel_lav_controle;

                //Redireciona para Index
                return RedirectToAction("UsuarioIndex");
            }
        }

        // GET: /DELETE
        public ActionResult UsuarioDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Usuario usuario = new Usuario();

                oAdministracao.InfoUsuario(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                           codigo: codigo,
                                           codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                           oUsuario: ref usuario);

                if (usuario == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(usuario);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UsuarioDelete([Bind(Include = "codigo")] Usuario usuario)
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
                    oAdministracao.DeleteUsuario(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                 codigo: usuario.codigo);

                    //Redireciona para Index
                    return RedirectToAction("UsuarioIndex");

                }
                catch
                {

                    //Redireciona para Index
                    return UsuarioDelete(codigo: usuario.codigo,
                                            erro: PCM.WEB.Properties.Resources.valida_excluir);
                }

            }
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

        // GET: /EDIT
        public ActionResult UsuarioPassword(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Usuario usuario = null;

                oAdministracao.InfoUsuario(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                           codigo:codigo,
                                           codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                           oUsuario: ref usuario);

                if (usuario == null)
                {
                    return HttpNotFound();
                }

                return View(usuario);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UsuarioPassword(int codigo, string senha)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oAdministracao.UpdateUsuario(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                             senha: senha,
                                             codigo: codigo);

                //Redireciona para Index
                return RedirectToAction("UsuarioIndex");
            }
        }

        #endregion

        #region ::: PERFIL :::

        // GET: INDEX
        public ActionResult PerfilIndex()
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
                                    sFormulario: "adm_perfil",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                return View(oAdministracao.IndexPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())));
            }
        }

        // GET: HIERARQUIA
        public ActionResult PerfilHierarquia()
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
                                    sFormulario: "adm_perfil_hierarquia",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                if (editar || excluir)
                    ViewBag.columnDefs = "columnDefs: [{ orderable: false, targets: [1, 2] }]";
                else
                    ViewBag.columnDefs = "";

                return View(oAdministracao.IndexPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())));
            }
        }

        // GET: INSERT
        public ActionResult PerfilInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                return View(oAdministracao.IndexPerfilDireito(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), iCodigoPerfil: -1));
            }
        }

        // POST: INSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PerfilInsert(string descricao, List<PerfilDireito> direito, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                int codigo = 0;

                //Insere Registro no Banco de Dados
                oAdministracao.InsertPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                            sDescricao: descricao,
                                            bAtivo: ativo,
                                            iCodigo: ref codigo);

                foreach (PerfilDireito item in direito)
                {

                    //Insere Registro no Banco de Dados
                    oAdministracao.InsertPerfilDireito(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigoPerfil: codigo,
                                                        sCodigoFormulario: item.codigo_formulario,
                                                        bVisualizar: item.visualizar,
                                                        bInserir: item.inserir,
                                                        bEditar: item.editar,
                                                        bExcluir: item.excluir,
                                                        bImprimir: item.imprimir,
                                                        bAdministrador: item.administrador);

                }

                return RedirectToAction("PerfilInsert");
            }
        }

        // GET: /EDIT
        public ActionResult PerfilEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Perfil perfil = null;

                oAdministracao.InfoPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigo: codigo,
                                            oPerfil: ref perfil);

                if (perfil == null)
                {
                    return HttpNotFound();
                }

                ViewBag.codigo = perfil.codigo;
                ViewBag.descricao = perfil.descricao;
                ViewBag.ativo = perfil.ativo;

                return View(oAdministracao.IndexPerfilDireito(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), iCodigoPerfil: codigo));
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PerfilEdit(string descricao, int codigo, List<PerfilDireito> direito, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Atualiza Registro no Banco de Dados
                oAdministracao.UpdatePerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                            sDescricao: descricao,
                                            bAtivo: ativo,
                                            iCodigo: codigo);

                foreach (PerfilDireito item in direito)
                {

                    //Insere Registro no Banco de Dados
                    oAdministracao.InsertPerfilDireito(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigoPerfil: codigo,
                                                        sCodigoFormulario: item.codigo_formulario,
                                                        bVisualizar: item.visualizar,
                                                        bInserir: item.inserir,
                                                        bEditar: item.editar,
                                                        bExcluir: item.excluir,
                                                        bImprimir: item.imprimir,
                                                        bAdministrador: item.administrador);

                }

                FormularioVisualizar formulario_visualizar = new FormularioVisualizar();

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    oFormularioVisualizar: ref formulario_visualizar);

                Session["adm_empresa"] = formulario_visualizar.adm_empresa;
                Session["adm_perfil"] = formulario_visualizar.adm_perfil;
                Session["adm_usuario"] = formulario_visualizar.adm_usuario;
                Session["adm_perfil"] = formulario_visualizar.adm_perfil;
                Session["adm_perfil_hierarquia"] = formulario_visualizar.adm_perfil_hierarquia;
                Session["audit_externa"] = formulario_visualizar.audit_externa;
                Session["audit_corporativo"] = formulario_visualizar.audit_corporativo;
                Session["audit_laudo"] = formulario_visualizar.audit_laudo;
                Session["audit_normas_procedimentos"] = formulario_visualizar.audit_normas_procedimentos;
                Session["audit_relatorio"] = formulario_visualizar.audit_relatorio;
                Session["audit_relatorio_mensal_pcm"] = formulario_visualizar.audit_relatorio_mensal_pcm;
                Session["audit_corporativo_historico"] = formulario_visualizar.audit_corporativo_historico;
                Session["cad_apartamento"] = formulario_visualizar.cad_apartamento;
                Session["cad_ar_condicionado"] = formulario_visualizar.cad_ar_condicionado;
                Session["cad_atividade"] = formulario_visualizar.cad_atividade;
                Session["cad_auditoria_qualidade"] = formulario_visualizar.cad_auditoria_qualidade;
                Session["cad_auditoria_corporativo"] = formulario_visualizar.cad_auditoria_corporativo;
                Session["cad_categoria"] = formulario_visualizar.cad_categoria;
                Session["cad_checklist"] = formulario_visualizar.cad_checklist;
                Session["cad_cliente"] = formulario_visualizar.cad_cliente;
                Session["cad_cliente_acordo_comercial"] = formulario_visualizar.cad_cliente_acordo_comercial;
                Session["adm_cliente"] = formulario_visualizar.adm_cliente;
                Session["cad_departamento"] = formulario_visualizar.cad_departamento;
                Session["cad_departamento_gestor"] = formulario_visualizar.cad_departamento_gestor;
                Session["cad_equipamento"] = formulario_visualizar.cad_equipamento;
                Session["cad_enxoval"] = formulario_visualizar.cad_enxoval;
                Session["cad_familia_equipamento"] = formulario_visualizar.cad_familia_equipamento;
                Session["cad_fornecedor"] = formulario_visualizar.cad_fornecedor;
                Session["cad_funcao"] = formulario_visualizar.cad_funcao;
                Session["cad_funcionario"] = formulario_visualizar.cad_funcionario;
                Session["cad_dedetizacao"] = formulario_visualizar.cad_dedetizacao;
                Session["cad_grupo_item_medicao"] = formulario_visualizar.cad_grupo_item_medicao;
                Session["cad_grupo_produto"] = formulario_visualizar.cad_grupo_produto;
                Session["cad_item_medicao"] = formulario_visualizar.cad_item_medicao;
                Session["cad_itens_gerais"] = formulario_visualizar.cad_itens_gerais;
                Session["cad_justificativa_apontamento"] = formulario_visualizar.cad_justificativa_apontamento;
                Session["cad_justificativa_cancelar_ordem_servico"] = formulario_visualizar.cad_justificativa_cancelar_ordem_servico;
                Session["cad_justificativa_falta"] = formulario_visualizar.cad_justificativa_falta;
                Session["cad_prioridade"] = formulario_visualizar.cad_prioridade;
                Session["cad_produto"] = formulario_visualizar.cad_produto;
                Session["cad_laudo"] = formulario_visualizar.cad_laudo;
                Session["adm_configuracao_desempenho_unidade"] = formulario_visualizar.adm_configuracao_desempenho_unidade;
                Session["cad_preventiva"] = formulario_visualizar.cad_preventiva;
                Session["cad_rotina"] = formulario_visualizar.cad_rotina;
                Session["cad_setor"] = formulario_visualizar.cad_setor;
                Session["cad_tarefa"] = formulario_visualizar.cad_tarefa;
                Session["cad_tipo_apartamento"] = formulario_visualizar.cad_tipo_apartamento;
                Session["cad_tipo_ar_condicionado"] = formulario_visualizar.cad_tipo_ar_condicionado;
                Session["cad_tipo_cama"] = formulario_visualizar.cad_tipo_cama;
                Session["cad_tipo_despesa"] = formulario_visualizar.cad_tipo_despesa;
                Session["cad_treinamento"] = formulario_visualizar.cad_treinamento;
                Session["cad_unidade"] = formulario_visualizar.cad_unidade;
                Session["cad_relatorio_itens_auditaveis"] = formulario_visualizar.cad_relatorio_itens_auditaveis;
                Session["cad_item_os_hospede"] = formulario_visualizar.cad_item_os_hospede;
                Session["cfg_opera"] = formulario_visualizar.cfg_opera;
                Session["fin_contrato"] = formulario_visualizar.fin_contrato;
                Session["fin_controle_gasto"] = formulario_visualizar.fin_controle_gasto;
                Session["fin_input_despesa"] = formulario_visualizar.fin_input_despesa;

                Session["governanca"] = formulario_visualizar.governanca;
                Session["gov_funcionario"] = formulario_visualizar.gov_funcionario;
                Session["gov_planejamento"] = formulario_visualizar.gov_planejamento;
                Session["gov_planejamento_historico"] = formulario_visualizar.gov_planejamento_historico;
                Session["green_lancamento"] = formulario_visualizar.green_lancamento;
                Session["gov_historico"] = formulario_visualizar.gov_historico;
                Session["gov_dashboard"] = formulario_visualizar.gov_dashboard;
                Session["gov_apontamento"] = formulario_visualizar.gov_apontamento;
                Session["gov_status_uh"] = formulario_visualizar.gov_status_uh;
                Session["gov_lavanderia"] = formulario_visualizar.gov_lavanderia;
                Session["gov_log_alteracao_status_gov"] = formulario_visualizar.gov_log_alteracao_status_gov;
                Session["gov_inventario_enxoval"] = formulario_visualizar.gov_inventario_enxoval;
                Session["gov_movimentacao_enxoval"] = formulario_visualizar.gov_movimentacao_enxoval;
                Session["gov_cad_tipo_perda"] = formulario_visualizar.gov_cad_tipo_perda;

                Session["log_book"] = formulario_visualizar.log_book;
                Session["ordem_servico"] = formulario_visualizar.ordem_servico;
                Session["ordem_servico_atribuir"] = formulario_visualizar.ordem_servico_atribuir;
                Session["uh_atividade"] = formulario_visualizar.uh_atividade;
                Session["pcm_apontamento_os_edit"] = formulario_visualizar.pcm_apontamento_os_edit;
                Session["pcm_apontamento_laudo"] = formulario_visualizar.pcm_apontamento_laudo;
                Session["pcm_apontamento_preventiva"] = formulario_visualizar.pcm_apontamento_preventiva;
                Session["pcm_apontamento_os"] = formulario_visualizar.pcm_apontamento_os;
                Session["pcm_apontamento_rotina"] = formulario_visualizar.pcm_apontamento_rotina;
                Session["pcm_cronograma_semanal"] = formulario_visualizar.pcm_cronograma_semanal;
                Session["pcm_falta"] = formulario_visualizar.pcm_falta;
                Session["pcm_manutencao_laudo"] = formulario_visualizar.pcm_manutencao_laudo;
                Session["pcm_manutencao_preventiva"] = formulario_visualizar.pcm_manutencao_preventiva;
                Session["pcm_manutencao_rotina"] = formulario_visualizar.pcm_manutencao_rotina;
                Session["pcm_historico_programada"] = formulario_visualizar.pcm_historico_programada;
                Session["pcm_plano_acao"] = formulario_visualizar.pcm_plano_acao;
                Session["pcm_requisicao"] = formulario_visualizar.pcm_requisicao;
                Session["pcm_requisicao_aprovar_reprovar"] = formulario_visualizar.pcm_requisicao_aprovar_reprovar;
                Session["pmoc_apontamento"] = formulario_visualizar.pmoc_apontamento;
                Session["pmoc_historico"] = formulario_visualizar.pmoc_historico;
                Session["pmoc"] = formulario_visualizar.pmoc;
                Session["pmoc_cronograma"] = formulario_visualizar.pmoc_cronograma;
                Session["pmoc_bup"] = formulario_visualizar.pmoc_bup;
                Session["pmoc_andar"] = formulario_visualizar.pmoc_andar;
                Session["pmoc_cronograma_bup"] = formulario_visualizar.pmoc_cronograma_bup;
                Session["rel_custo_horas_trabalhadas"] = formulario_visualizar.rel_custo_horas_trabalhadas;
                Session["rel_funcionario_horas_trabalhadas"] = formulario_visualizar.rel_funcionario_horas_trabalhadas;
                Session["rel_funcionario_ociosidade"] = formulario_visualizar.rel_funcionario_ociosidade;
                Session["rel_green_planet"] = formulario_visualizar.rel_green_planet;
                Session["rel_manutencao_aberto_concluido"] = formulario_visualizar.rel_manutencao_aberto_concluido;
                Session["rel_manutencao"] = formulario_visualizar.rel_manutencao;
                Session["rel_manutencao_categoria"] = formulario_visualizar.rel_manutencao_categoria;
                Session["rel_manutencao_equipamento"] = formulario_visualizar.rel_manutencao_equipamento;
                Session["rel_manutencao_executor"] = formulario_visualizar.rel_manutencao_executor;
                Session["rel_manutencao_setor"] = formulario_visualizar.rel_manutencao_setor;
                Session["rel_manutencao_solicitante"] = formulario_visualizar.rel_manutencao_solicitante;
                Session["rel_manutencao_tempo_medio_atendimento"] = formulario_visualizar.rel_manutencao_tempo_medio_atendimento;
                Session["rel_manutencao_tipo_ordem_servico"] = formulario_visualizar.rel_manutencao_tipo_ordem_servico;
                Session["rel_log_book"] = formulario_visualizar.rel_log_book;
                Session["rel_pmoc_mes"] = formulario_visualizar.rel_pmoc_mes;
                Session["rel_pmoc_bimestral"] = formulario_visualizar.rel_pmoc_bimestral;
                Session["rel_nao_conformidade"] = formulario_visualizar.rel_nao_conformidade;
                Session["rel_preventiva_mes"] = formulario_visualizar.rel_preventiva_mes;
                Session["rel_dinamico"] = formulario_visualizar.rel_dinamico;
                Session["rel_consumo_enxoval"] = formulario_visualizar.rel_consumo_enxoval;
                Session["rel_horas_trabalhadas_governanca"] = formulario_visualizar.rel_horas_trabalhadas_governanca;
                Session["rel_camareira_uh"] = formulario_visualizar.rel_camareira_uh;
                Session["rel_responsavel_vistoria_uh"] = formulario_visualizar.rel_responsavel_vistoria_uh;
                Session["rel_camareira_nc"] = formulario_visualizar.rel_camareira_nc;
                
                Session["uh_checklist"] = formulario_visualizar.uh_checklist;
                Session["uh_checklist_historico"] = formulario_visualizar.uh_checklist_historico;
                Session["uh_dedetizacao"] = formulario_visualizar.uh_dedetizacao;
                Session["uhDedetizacaoHistorico"] = formulario_visualizar.uhDedetizacaoHistorico;
                Session["uhMapaManutencaoHistorico"] = formulario_visualizar.uhMapaManutencaoHistorico;

                Session["excel_ordem_servico"] = formulario_visualizar.excel_ordem_servico;
                Session["excel_plano_acao_qa"] = formulario_visualizar.excel_plano_acao_qa;

                Session["est_listagem"] = formulario_visualizar.est_listagem;
                Session["est_requisicao_compra"] = formulario_visualizar.est_requisicao_compra;
                Session["est_ordem_compra"] = formulario_visualizar.est_ordem_compra;
                Session["est_entrada"] = formulario_visualizar.est_entrada;
                Session["est_saida"] = formulario_visualizar.est_saida;
                Session["est_inventario"] = formulario_visualizar.est_inventario;
                Session["est_entrada"] = formulario_visualizar.est_entrada;

                Session["aeb_contrato"] = formulario_visualizar.aeb_contrato;
                Session["aeb_laudo"] = formulario_visualizar.aeb_laudo;
                Session["aeb_normas_procedimentos"] = formulario_visualizar.aeb_normas_procedimentos;
                Session["aeb_auditoria_externa"] = formulario_visualizar.aeb_auditoria_externa;

                Session["qa_auditoria"] = formulario_visualizar.qa_auditoria;
                Session["qa_auditoria_historico"] = formulario_visualizar.qa_auditoria_historico;
                Session["qa_auditoria_cronograma"] = formulario_visualizar.qa_auditoria_cronograma;
                Session["qa_plano_acao"] = formulario_visualizar.qa_plano_acao;
                Session["qa_tarefa"] = formulario_visualizar.qa_tarefa;
                Session["qa_tarefa_historico"] = formulario_visualizar.qa_tarefa_historico;
                Session["dash_desempenho"] = formulario_visualizar.dash_desempenho;
                Session["dash_desempenho_qa"] = formulario_visualizar.dash_desempenho_qa;

                Session["cad_area_comum"] = formulario_visualizar.cad_area_comum;
                Session["agenda_area_comum"] = formulario_visualizar.agenda_area_comum;
                Session["agenda_entrada"] = formulario_visualizar.agenda_entrada;
                Session["agenda_saida"] = formulario_visualizar.agenda_saida;

                Session["upload_excel"] = formulario_visualizar.upload_excel;
                Session["upload_pmoc"] = formulario_visualizar.upload_pmoc;

                Session["lav_apontamento"] = formulario_visualizar.lav_apontamento;
                Session["lav_historico"] = formulario_visualizar.lav_historico;
                Session["rel_lav_controle"] = formulario_visualizar.rel_lav_controle;

                //Redireciona para Index
                return RedirectToAction("PerfilIndex");
            }
        }

        // GET: /DELETE
        public ActionResult PerfilDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Perfil unidade = null;

                oAdministracao.InfoPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigo: codigo,
                                            oPerfil: ref unidade);

                if (unidade == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(unidade);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PerfilDelete([Bind(Include = "codigo")] Perfil perfil)
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
                    oAdministracao.DeletePerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                iCodigo: perfil.codigo);
                    //Redireciona para Index
                    return RedirectToAction("PerfilIndex");
                }
                catch
                {

                    //Redireciona para Index
                    return PerfilDelete(codigo: perfil.codigo,
                                        erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA PERFIL
        public JsonResult ValidaPerfil(string descricao, int codigo)
        {
            return Json((oAdministracao.ValidaPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoPerfil: codigo,
                                                        sDescricao: descricao)));
        }

        #endregion

        #region ::: EMPRESA :::

        // GET: INDEX
        public ActionResult Empresa()
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
                                    sFormulario: "adm_empresa",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                return View(oAdministracao.IndexEmpresa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())));
            }
        }

        // GET: INDEX
        public ActionResult EmpresaAtivarInativar(int codigo, bool ativar)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Atualiza Registro no Banco de Dados
                oAdministracao.UpdateEmpresa(iCodigo: codigo,
                                                bAtivo: ativar);
            }

            return RedirectToAction("Empresa");

        }

        #endregion

        #region ::: CLIENTE :::

        // GET: INDEX
        public ActionResult ClienteIndex()
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
                                    sFormulario: "adm_cliente",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                return View(oCadastroBasico.IndexUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())));
            }
        }

        // GET: /INSERT
        public ActionResult ClienteInsert()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.uf = new SelectList(oCombo.UF(), "codigo", "descricao", null);
                ViewBag.tipo_unidade = new SelectList(oCombo.TipoUnidade(), "codigo", "descricao", null);

                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult ClienteInsert(string nome_fantasia, string razao_social, string cnpj, string inscricao_estadual, string inscricao_municipal, string cep, string uf, string municipio, string logradouro, string numero, string bairro, string complemento, string telefone, HttpPostedFileBase logoMin, HttpPostedFileBase logoMax, int tipo_unidade = -1, int quantidade_bloco = 1, int quantidade_andar = 1, bool aponta_horas = false, bool aponta_horas_qualidade = false, string area_total = "0", string area_total_construida = "0", int quantidae_maxima_horas_apontamento = 0, bool ativo = false, string hotel_opera = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
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

                int codigo_unidade = 0;

                //Insere Registro no Banco de Dados
                oCadastroBasico.InsertUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                              iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                              sNomeFantasia: nome_fantasia,
                                              sRazaoSocial: razao_social,
                                              sCNPJ: cnpj,
                                              sInscricaoEstadual: inscricao_estadual,
                                              sInscricaoMunicipal: inscricao_municipal,
                                              sCEP: cep,
                                              sUF: uf,
                                              sMunicipio: municipio,
                                              sLogradouro: logradouro,
                                              sNumero: numero,
                                              sBairro: bairro,
                                              sComplemento: complemento,
                                              sTelefone: telefone,
                                              iQuantidadeBloco: quantidade_bloco,
                                              iQuantidadeAndar: quantidade_andar,
                                              sLogoMin: sLogoMinPath,
                                              sLogoMax: sLogoMaxPath,
                                              bApontaHoras: aponta_horas,
                                              bApontaHorasQualidade: aponta_horas_qualidade,
                                              iQuantidadeMaximaHorasApontamento: quantidae_maxima_horas_apontamento,
                                              dAreaTotal: Convert.ToDouble((area_total == "") ? "0" : area_total),
                                              dAreaTotalConstruida: Convert.ToDouble((area_total_construida == "") ? "0" : area_total_construida),
                                              iCodigoTipoUnidade: tipo_unidade,
                                              bAtivo: ativo,
                                              sHotelOpera: hotel_opera,
                                              iCodigoUnidade: ref codigo_unidade);

                return RedirectToAction("ClienteInsert");
            }
        }

        // GET: /EDIT
        public ActionResult ClienteEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Unidade unidade = null;

                oCadastroBasico.InfoUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigo: codigo,
                                            oUnidade: ref unidade);

                if (unidade == null)
                {
                    return HttpNotFound();
                }

                ViewBag.uf = new SelectList(oCombo.UF(), "codigo", "descricao", unidade.uf);
                ViewBag.tipo_unidade = new SelectList(oCombo.TipoUnidade(), "codigo", "descricao", unidade.codigo_tipo_unidade);
                ViewBag.arquivo = unidade.arquivo;

                return View(unidade);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClienteEdit(string nome_fantasia, string razao_social, string cnpj, string inscricao_estadual, string inscricao_municipal, string cep, string uf, string municipio, string logradouro, string numero, string bairro, string complemento, string telefone, int quantidade_bloco, int quantidade_andar, int tipo_unidade, HttpPostedFileBase logoMin, HttpPostedFileBase logoMax, int codigo, string change_imagem, bool aponta_horas = false, bool aponta_horas_qualidade = false, string area_total = "0", string area_total_construida = "0", int quantidae_maxima_horas_apontamento = 0, bool ativo = false, string hotel_opera = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
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

                //Altera Registro no Banco de Dados
                oCadastroBasico.UpdateUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                            sNomeFantasia: nome_fantasia,
                                            sRazaoSocial: razao_social,
                                            sCNPJ: cnpj,
                                            sInscricaoEstadual: inscricao_estadual,
                                            sInscricaoMunicipal: inscricao_municipal,
                                            sCEP: cep,
                                            sUF: uf,
                                            sMunicipio: municipio,
                                            sLogradouro: logradouro,
                                            sNumero: numero,
                                            sBairro: bairro,
                                            sComplemento: complemento,
                                            sTelefone: telefone,
                                            iQuantidadeBloco: quantidade_bloco,
                                            iQuantidadeAndar: quantidade_andar,
                                            sLogoMin: sLogoMinPath,
                                            sLogoMax: sLogoMaxPath,
                                            bApontaHoras: aponta_horas,
                                            bApontaHorasQualidade: aponta_horas_qualidade,
                                            iQuantidadeMaximaHorasApontamento: quantidae_maxima_horas_apontamento,
                                            dAreaTotal: Convert.ToDouble((area_total == "") ? "0" : area_total),
                                            dAreaTotalConstruida: Convert.ToDouble((area_total_construida == "") ? "0" : area_total_construida),
                                            iCodigoTipoUnidade: tipo_unidade,
                                            bAtivo: ativo,
                                            sHotelOpera: hotel_opera,
                                            iCodigo: codigo);

                if (codigo == (int)Session["codigo_unidade"])
                {
                    Session["unidade"] = nome_fantasia;
                    if (Path.Combine(sPath, sFileName) != "")
                    {
                        Session["imagem"] = Path.Combine(sPath, sFileName);
                    }

                }

                //Redireciona para Index
                return RedirectToAction("ClienteIndex");
            }
        }

        // GET: /DELETE
        public ActionResult ClienteDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Unidade unidade = null;

                oCadastroBasico.InfoUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigo: codigo,
                                            oUnidade: ref unidade);

                if (unidade == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(unidade);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClienteDelete([Bind(Include = "codigo")] Unidade unidade)
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
                    oCadastroBasico.DeleteUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigo: unidade.codigo);

                    //Redireciona para Index
                    return RedirectToAction("ClienteIndex");
                }
                catch
                {
                    return ClienteDelete(codigo: unidade.codigo,
                                            erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA
        public JsonResult ValidaCliente(string cnpj, int codigo)
        {

            return Json(oCadastroBasico.ValidaUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigo: codigo,
                                                        sCNPJ: cnpj));

        }

        #endregion

        #region ::: CONFIGURAÇÃO - DESEMPENHO DAS UNIDADES :::

        // POST: /CONFIGURAÇÃO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfiguracaoDesempenhoUnidade(string laudo, string preventiva, string rotina, string pmoc, string uh_dia, string os_atendimento_dia, string hh_nao_apontado, string os_pendente, string hh_extra, string preventiva_corretiva, string green_planet)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Altera Registro no Banco de Dados
                oAdministracao.UpdateConfiguracaoDesempenhoUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    sLaudo: laudo,
                                                                    sPreventiva: preventiva,
                                                                    sRotina: rotina,
                                                                    sPMOC: pmoc,
                                                                    sUHDia: uh_dia,
                                                                    sOSAtendimentoDia: os_atendimento_dia,
                                                                    sHHNaoApontado: hh_nao_apontado,
                                                                    sOSPendente: os_pendente,
                                                                    sHHExtra: hh_extra,
                                                                    sPreventivaCorretiva: preventiva_corretiva,
                                                                    sGreenPlanet: green_planet);

                //Redireciona para Index
                return RedirectToAction("ConfiguracaoDesempenhoUnidade");
            }
        }

        // GET: /CONFIGURAÇÃO
        public ActionResult ConfiguracaoDesempenhoUnidade()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                return View(oAdministracao.ConfiguracaoDesempenhoUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())));
            }
        }

        #endregion

    }
}