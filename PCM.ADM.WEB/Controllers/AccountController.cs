
using System.Web.Mvc;
using PCM.WEB.MODELS;
using PCM.WEB.DAL;
using System.Configuration;
using System;
using System.Linq;
using System.Net.Mail;
using System.Net.Configuration;

namespace PCM.ADM.WEB.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnectionAdm"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnectionAdm"].ConnectionString);

        #region ::: LOGIN :::

            // GET: /LOGIN/
            [AllowAnonymous]
            public ActionResult Login(string returnUrl)
            {
                ViewBag.ReturnUrl = returnUrl;

                imagemLogin imagemLogin = oAccount.LoadImageLogin(sURL: this.Request.RawUrl);
                ViewBag.imagem = imagemLogin;

                return View();
            }
        
            // POST: /LOGIN/
            [HttpPost]
            [AllowAnonymous]
            public ActionResult Login(string email, string senha, string returnUrl)
            {
                //Váriaveis
                int iCodigoEmpresa = 0;
                int iCodigoUsuario = 0;
                string sImagemSidebar = "";
                string sImagemSidebarMin = "";
                string sImagemHeader = "";

                login login = new login();
                FormularioVisualizar formulario_visualizar = new FormularioVisualizar();
           
                if (!oAccount.Login(sEmail: email, 
                                    sSenha: senha,
                                    iCodigoEmpresa: ref iCodigoEmpresa,
                                    iCodigoUsuario: ref iCodigoUsuario,
                                    sImagemSidebar: ref sImagemSidebar,
                                    sImagemSidebarMin: ref sImagemSidebarMin,
                                    sImagemHeader: ref sImagemHeader))
                {
                    ModelState.AddModelError("", "Usuário ou Senha Inválido.");
                }
                else
                {
                    
                    System.Web.Security.FormsAuthentication.SetAuthCookie(iCodigoUsuario.ToString(), false);
                    Session["empresa"] = iCodigoEmpresa;
                    Session["codigo_usuario"] = iCodigoUsuario;
                    Session["language"] = "PT";
                    Session["imagem_sidebar"] = sImagemSidebar;
                    Session["imagem_sidebar_min"] = sImagemSidebarMin;
                    Session["imagem_header"] = sImagemHeader;

                    return RedirectToAction("Index", "Home");
                }

                login.email = email;
                login.senha = senha;

                return View(login);
            }

        #endregion

        #region ::: PASSWORD REMINDER :::

            // GET: /PASSWORD REMINDER/
            [AllowAnonymous]
            public ActionResult PasswordReminder()
            {
                imagemLogin imagemLogin = oAccount.LoadImageLogin(sURL: this.Request.RawUrl);
                ViewBag.imagem = imagemLogin;

                return View();
            }

            // POST: /PASSWORD REMINDER/
            [HttpPost]
            [AllowAnonymous]
            public ActionResult PasswordReminder(string email)
            {
                string password = getPassword(8);

                string remetente = "pcm@simservices.com.br"; //O e-mail do remetente
                MailMessage mail = new MailMessage();
                mail.To.Add(email);
                mail.From = new MailAddress(remetente, "PCM by SIM", System.Text.Encoding.UTF8);
                mail.Subject = String.Concat("Solicitação de nova senha. - ", DateTime.Now.ToShortDateString(), " ", DateTime.Now.ToShortTimeString());
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Body = String.Concat("Olá, <br /> <br />Conforme sua solicitação, segue abaixo sua nova senha.<br /><br />Nova Senha: <b>", password, "</b><br /><br /><b><i> Obs.: Esta senha é provisória e deve ser alterada assim que acessar o PCM by SIM. </i></b><br /><br />Atenciosamente, <br />PCM by SIM");
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient(); 
                client.Credentials = new System.Net.NetworkCredential(remetente, "p@ssw0rd013459");
                client.Port = 80;
                client.Host = "smtpout.secureserver.net";
                client.EnableSsl = false;

                try
                {
                    //Envia E-mail
                    client.Send(mail);

                    //Atualiza Senha
                    oAccount.UpdatePassword(sEmail: email,
                                            sPassword: password);

                    return RedirectToAction("Login");

                }
                catch (Exception ex)
                {
                }

                return View();
            }

            public static string getPassword(int tamanho)
            {
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var random = new Random();
                var result = new string(
                    Enumerable.Repeat(chars, tamanho)
                              .Select(s => s[random.Next(s.Length)])
                              .ToArray());
                return result;
            }

        #endregion

        #region ::: LOGOFF :::

            // POST: /LOGOFF/
        public ActionResult LogOff()
        {
            Session["empresa"] = null;
            Session["nome"] = null;
            Session["email"] = null;
            Session["codigo_funcionario"] = null;
            Session["fornecedor_pmoc"] = null;
            Session["funcionario"] = null;
            Session["language"] = null;
            Session["codigo_unidade"] = null;
            Session["unidade"] = null;
            Session["unidade_descricao"] = null;
            Session["imagem"] = null;
            Session["codigo_empresa_pmoc"] = null;
            Session["codigo_unidade_pmoc"] = null;
            Session["adm_empresa"] = null;
            Session["adm_perfil"] = null;
            Session["adm_usuario"] = null;
            Session["adm_perfil"] = null;
            Session["adm_perfil_hierarquia"] = null;
            Session["audit_externa"] = null;
            Session["audit_interna"] = null;
            Session["audit_laudo"] = null;
            Session["audit_normas_procedimentos"] = null;
            Session["audit_relatorio"] = null;
            Session["audit_relatorio_mensal_pcm"] = null;
            Session["cad_apartamento"] = null;
            Session["cad_ar_condicionado"] = null;
            Session["cad_atividade"] = null;
            Session["cad_auditoria_qualidade"] = null;
            Session["cad_auditoria_corportativa"] = null;
            Session["cad_categoria"] = null;
            Session["cad_checklist"] = null;
            Session["cad_cliente"] = null;
            Session["cad_cliente_acordo_comercial"] = null;
            Session["adm_cliente"] = null;
            Session["cad_departamento"] = null;
            Session["cad_equipamento"] = null;
            Session["cad_enxoval"] = null;
            Session["cad_fornecedor"] = null;
            Session["cad_funcao"] = null;
            Session["cad_funcionario"] = null;
            Session["cad_dedetizacao"] = null;
            Session["cad_grupo_checklist"] = null;
            Session["cad_grupo_item_medicao"] = null;
            Session["cad_grupo_produto"] = null;
            Session["cad_itens_gerais"] = null;
            Session["cad_item_medicao"] = null;
            Session["cad_justificativa_apontamento"] = null;
            Session["cad_justificativa_cancelar_ordem_servico"] = null;
            Session["cad_justificativa_falta"] = null;
            Session["cad_prioridade"] = null;
            Session["cad_programada"] = null;
            Session["cad_rotina"] = null;
            Session["cad_setor"] = null;
            Session["cad_tipo_apartamento"] = null;
            Session["cad_tipo_ar_condicionado"] = null;
            Session["cad_tipo_cama"] = null;
            Session["cad_tipo_checklist"] = null;
            Session["cad_tipo_despesa"] = null;
            Session["cad_unidade"] = null;
            Session["cad_item_os_hospede"] = null;
            Session["fin_contrato"] = null;
            Session["fin_controle_gasto"] = null;
            Session["fin_input_despesa"] = null;
            Session["green_lancamento"] = null;
            Session["governanca"] = null;
            Session["log_book"] = null;
            Session["ordem_servico"] = null;
            Session["ordem_servico_atribuir"] = null;
            Session["uh_atividade"] = null;
            Session["pcm_apontamento_laudo"] = null;
            Session["pcm_apontamento_preventiva"] = null;
            Session["pcm_apontamento_os"] = null;
            Session["pcm_apontamento_rotina"] = null;
            Session["pcm_manutencao_laudo"] = null;
            Session["pcm_manutencao_preventiva"] = null;                        
            Session["pcm_manutencao_rotina"] = null;
            Session["pcm_historico_programada"] = null;
            Session["pcm_requisicao"] = null;
            Session["pcm_requisicao_aprovar_reprovar"] = null;
            Session["pmoc_apontamento"] = null;
            Session["pmoc_cronograma"] = null;
            Session["pmoc_historico"] = null;
            Session["pmoc"] = null;
            Session["pmoc_andar"] = null;
            Session["pmoc_bup"] = null;
            Session["pmoc_cronograma_bup"] = null;
            Session["rel_custo_horas_trabalhadas"] = null;
            Session["rel_funcionario_horas_trabalhadas"] = null;
            Session["rel_funcionario_ociosidade"] = null;
            Session["rel_green_planet"] = null;
            Session["rel_manutencao_aberto_concluido"] = null;
            Session["rel_manutencao_categoria"] = null;
            Session["rel_manutencao"] = null;
            Session["rel_manutencao_equipamento"] = null;
            Session["rel_manutencao_executor"] = null;
            Session["rel_manutencao_setor"] = null;
            Session["rel_manutencao_solicitante"] = null;
            Session["rel_manutencao_tempo_medio_atendimento"] = null;
            Session["rel_manutencao_tipo_ordem_servico"] = null;
            Session["rel_pmoc_mes"] = null;
            Session["rel_pmoc_bimestral"] = null;
            Session["rel_log_book"] = null;
            Session["rel_preventiva_mes"] = null;
            Session["rel_dinamico"] = null;
            Session["rel_consumo_enxoval"] = null;
            Session["rel_horas_trabalhadas_governanca"] = null;
            Session["rel_camareira_uh"] = null;
            Session["rel_responsavel_vistoria_uh"] = null;
            Session["rel_camareira_nc"] = null;
            Session["rel_uh_nc"] = null;
            Session["uh_checklist"] = null;
            Session["uh_checklist_historico"] = null;
            Session["uh_dedetizacao"] = null;
            Session["uhDedetizacaoHistorico"] = null;
            Session["uhMapaManutencaoHistorico"] = null;
            Session["excel_ordem_servico"] = null;
            Session["checklist"] = null;
            Session["est_listagem"] = null;
            Session["est_requisicao_compra"] = null;
            Session["est_ordem_compra"] = null;
            Session["est_entrada"] = null;
            Session["est_saida"] = null;
            Session["est_inventario"] = null;
            Session["aeb_contrato"] = null;
            Session["aeb_laudo"] = null;
            Session["aeb_normas_procedimentos"] = null;
            Session["aeb_auditoria_externa"] = null;

            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region ::: DISPOSE :::

        protected override void Dispose(bool disposing)
        {           
            base.Dispose(disposing);
        }

        #endregion

    }
}