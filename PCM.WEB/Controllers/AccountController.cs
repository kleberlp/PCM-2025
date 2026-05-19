using PCM.WEB.DAL;
using PCM.WEB.MODELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PCM.WEB.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: MESSAGE :::

        public void SendMessage2(String ordem_servico, String descricao, string to)
        {

            List<string> deviceTokens = to.Split(new char[] { (char)13 }).ToList();
            deviceTokens.Remove("");

            if (deviceTokens.Count > 0)
            {

                // Define the JSON payload
                //var data = new JObject
                //{
                //    ["registration_ids"] = new JArray(deviceTokens),
                //    ["priority"] = "high",
                //    ["content_available"] = true,
                //    ["data"] = new JObject
                //    {
                //        ["title"] = "Nº OS: " + ordem_servico,
                //        ["body"] = descricao.ToUpper(),
                //    }
                //};

                String timeStamp = DateTime.Now.ToFileTime().ToString();

                var data = new
                {
                    registration_ids = deviceTokens.ToArray(),
                    priority = "high",
                    content_available = true,
                    data = new
                    {
                        message = descricao.ToUpper(),
                        title = "Nº OS: " + ordem_servico,
                        is_background = true,
                        timestamp = timeStamp
                    }
                };

                SendNotification(data);
            }

        }

        public void SendNotification(object data)
        {
            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(data);
            Byte[] byteArray = Encoding.UTF8.GetBytes(json);

            SendNotification(byteArray);
        }

        public void SendNotification(Byte[] byteArray)
        {
            try
            {
                string server_api_key = "AAAAhu64c6k:APA91bG9TSbnmR9nKnlR5qcrl6J5QcML-gVnur-D2YYvdua5WRxqwLtOVS6CmNJN0qQaLPGjqmePxj7Azg7e5AYqvqMmjM-Fa3mZseqFXoVCB1MzA3t9mqJs9psxZ0JBJ8OUbW2yNnpg";
                string sender_id = "579530683305";

                WebRequest webRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                webRequest.Method = "post";
                webRequest.ContentType = "application/json";
                webRequest.Headers.Add($"Authorization: key={server_api_key}");
                webRequest.Headers.Add($"Sender: id={sender_id}");

                webRequest.ContentLength = byteArray.Length;
                Stream dataStream = webRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse webResponse = webRequest.GetResponse();
                dataStream = webResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(dataStream);

                string sResponseFromServer = streamReader.ReadToEnd();

                streamReader.Close();
                dataStream.Close();
                webResponse.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //JSON: /TIPO DE UNIDADE
        public JsonResult ValidaLogin(string email)
        {
            return Json(oAccount.ValidaLogin(sEmail: email));
        }

        #endregion

        #region ::: LOGIN :::

        // GET: /LOGIN/
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            //So that the user can be referred back to where they were when they click logon
            if (string.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null)
                returnUrl = Server.UrlEncode(Request.UrlReferrer.PathAndQuery);

            if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.ReturnURL = returnUrl;
            }

            var chars = new[] { '/' };

            imagemLogin imagemLogin = oAccount.LoadImageLogin(sURL: this.Request.RawUrl.Split(chars, 3, StringSplitOptions.RemoveEmptyEntries)[0]);
            ViewBag.imagem = imagemLogin;

            return View();
        }

        // POST: /LOGIN/
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string email, string senha, string returnUrl)
        {
            //Váriaveis
            Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            bool bAtivo = false;
            string sCodigoFuncionario = "";
            bool bFornecedorPMOC = false;
            string sFuncionario = "";
            string sNome = "";
            string sUnidade = "";
            string sImagemSidebar = "";
            string sImagemSidebarMin = "";
            string sImagemHeader = "";
            int iCodigoUsuario = 0;
            int iCodigoUnidade = 0;
            int iCodigoEmpresa = 0;
            int iCodigoDepartamento = -1;
            int iCodigoEmpresaPMOC = 0;
            int iCodigoUnidadePMOC = 0;
            int iCodigoModuloDefault = 0;
            string sDashboardPage = "Index";
            string sModulo = "";
            string sModuloDescricao = "";
            string sHotelOpera = "";
            bool bQualidade = false;
            bool bManutencao = false;
            bool bAeB = false;
            bool bRotinaPrioridade = false;
            bool bApontaHoras = false;
            login login = new login();
            FormularioVisualizar formulario_visualizar = new FormularioVisualizar();

            if (!oAccount.Login(sEmail: email,
                                sSenha: senha,
                                bAtivo: ref bAtivo,
                                iCodigoUsuario: ref iCodigoUsuario,
                                sCodigoFuncionario: ref sCodigoFuncionario,
                                sFuncionario: ref sFuncionario,
                                iCodigoDepartamento: ref iCodigoDepartamento,
                                sNome: ref sNome,
                                iCodigoUnidade: ref iCodigoUnidade,
                                sUnidade: ref sUnidade,
                                iCodigoEmpresa: ref iCodigoEmpresa,
                                iCodigoEmpresaPMOC: ref iCodigoEmpresaPMOC,
                                iCodigoUnidadePMOC: ref iCodigoUnidadePMOC,
                                bFornecedorPMOC: ref bFornecedorPMOC,
                                sImagemSidebar: ref sImagemSidebar,
                                sImagemSidebarMin: ref sImagemSidebarMin,
                                sImagemHeader: ref sImagemHeader,
                                sDashboardPage: ref sDashboardPage,
                                bManutencao: ref bManutencao,
                                bQualidade: ref bQualidade,
                                bAeB: ref bAeB,
                                bRotinaPrioridade: ref bRotinaPrioridade,
                                iCodigoModuloDefault: ref iCodigoModuloDefault,
                                bApontaHoras: ref bApontaHoras,
                                sModulo: ref sModulo,
                                sModuloDescricao: ref sModuloDescricao,
                                sHotelOpera: ref sHotelOpera))
            {
                ViewBag.errorLogin = "Usuário ou Senha Inválido.";
            }
            else
            {
                if (bAtivo)
                {
                    oAccount.LoadPerfil(iCodigoEmpresa: iCodigoEmpresa,
                                        iCodigoUsuario: iCodigoUsuario,
                                        oFormularioVisualizar: ref formulario_visualizar);

                    System.Web.Security.FormsAuthentication.SetAuthCookie(iCodigoUsuario.ToString(), false);
                    Session["manutencao"] = bManutencao;
                    Session["qualidade"] = bQualidade;
                    Session["aeb"] = bAeB;
                    Session["codigo_modulo"] = iCodigoModuloDefault;
                    Session["modulo"] = sModulo;
                    Session["dashboard"] = sDashboardPage;
                    Session["codigo_unidade"] = iCodigoUnidade;
                    Session["unidade"] = sUnidade;
                    Session["unidade_descricao"] = (sUnidade == "") ? "TODAS AS UNIDADES" : sUnidade.Replace("WISH ", "").Replace("PRODIGY ", "").Replace("LINX ", "");
                    Session["empresa"] = iCodigoEmpresa;
                    Session["codigo_funcionario"] = sCodigoFuncionario;
                    Session["fornecedor_pmoc"] = bFornecedorPMOC;
                    Session["funcionario"] = sFuncionario;
                    Session["nome"] = sNome;
                    Session["email"] = email;
                    Session["language"] = "PT";
                    Session["aponta_horas"] = bApontaHoras;
                    Session["codigo_departamento"] = iCodigoDepartamento;
                    Session["hotel_opera"] = sHotelOpera;
                    Session["imagem_sidebar"] = Url.Content(sImagemSidebar);
                    Session["imagem_sidebar_min"] = Url.Content(sImagemSidebarMin);
                    Session["imagem_header"] = Url.Content(sImagemHeader);
                    Session["codigo_empresa_pmoc"] = iCodigoEmpresaPMOC;
                    Session["codigo_unidade_pmoc"] = iCodigoUnidadePMOC;
                    Session["rotina_prioridade"] = bRotinaPrioridade;
                    Session["adm_perfil"] = formulario_visualizar.adm_perfil;
                    Session["adm_empresa"] = formulario_visualizar.adm_empresa;
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
                    Session["adm_configuracao_desempenho_unidade"] = formulario_visualizar.adm_configuracao_desempenho_unidade;
                    Session["cad_prioridade"] = formulario_visualizar.cad_prioridade;
                    Session["cad_produto"] = formulario_visualizar.cad_produto;
                    Session["cad_laudo"] = formulario_visualizar.cad_laudo;
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
                    Session["green_lancamento"] = formulario_visualizar.green_lancamento;
                    
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
                    Session["pmoc_cronograma"] = formulario_visualizar.pmoc_cronograma;
                    Session["pmoc_historico"] = formulario_visualizar.pmoc_historico;
                    Session["pmoc"] = formulario_visualizar.pmoc;
                    Session["pmoc_andar"] = formulario_visualizar.pmoc_andar;
                    Session["pmoc_bup"] = formulario_visualizar.pmoc_bup;
                    Session["pmoc_cronograma_bup"] = formulario_visualizar.pmoc_cronograma_bup;
                    Session["rel_custo_horas_trabalhadas"] = formulario_visualizar.rel_custo_horas_trabalhadas;
                    Session["rel_funcionario_horas_trabalhadas"] = formulario_visualizar.rel_funcionario_horas_trabalhadas;
                    Session["rel_funcionario_ociosidade"] = formulario_visualizar.rel_funcionario_ociosidade;
                    Session["rel_green_planet"] = formulario_visualizar.rel_green_planet;
                    Session["rel_manutencao_aberto_concluido"] = formulario_visualizar.rel_manutencao_aberto_concluido;
                    Session["rel_manutencao_categoria"] = formulario_visualizar.rel_manutencao_categoria;
                    Session["rel_manutencao"] = formulario_visualizar.rel_manutencao;
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
                    Session["rel_uh_nc"] = formulario_visualizar.rel_uh_nc;

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
                    Session["dash_desempenho"] = formulario_visualizar.dash_desempenho;
                    Session["dash_desempenho_qa"] = formulario_visualizar.dash_desempenho_qa;
                    Session["qa_tarefa"] = formulario_visualizar.qa_tarefa;
                    Session["qa_tarefa_historico"] = formulario_visualizar.qa_tarefa_historico;

                    Session["cad_area_comum"] = formulario_visualizar.cad_area_comum;
                    Session["agenda_area_comum"] = formulario_visualizar.agenda_area_comum;
                    Session["agenda_entrada"] = formulario_visualizar.agenda_entrada;
                    Session["agenda_saida"] = formulario_visualizar.agenda_saida;

                    Session["upload_excel"] = formulario_visualizar.upload_excel;
                    Session["upload_pmoc"] = formulario_visualizar.upload_pmoc;

                    Session["lav_apontamento"] = formulario_visualizar.lav_apontamento;
                    Session["lav_historico"] = formulario_visualizar.lav_historico;
                    Session["rel_lav_controle"] = formulario_visualizar.rel_lav_controle;

                    Session["cad_asset"] = formulario_visualizar.cad_asset;
                    Session["assetMovement"] = formulario_visualizar.assetMovement;
                    Session["assetInventory"] = formulario_visualizar.assetInventory;
                    Session["assetInventoryMng"] = formulario_visualizar.assetInventoryMng;

                    Session["app_ios"] = IsRequestFromIOS();

                    Session["modulo"] = sModuloDescricao;

                    switch (iCodigoModuloDefault)
                    {
                        case 1: @Session["modulo_css"] = "text-primary"; break;
                        case 2: @Session["modulo_css"] = "text-danger"; break;
                        case 3: @Session["modulo_css"] = "text-success"; break;
                        case 4: @Session["modulo_css"] = "text-earth"; break;
                    }



                    string decodedUrl = "";
                    if (!string.IsNullOrEmpty(returnUrl))
                        decodedUrl = Server.UrlDecode(returnUrl);

                    if (Url.IsLocalUrl(decodedUrl) && (decodedUrl.Contains("/Account") || 
                        decodedUrl.Contains("/Administracao") || 
                        decodedUrl.Contains("/AEB") || 
                        decodedUrl.Contains("/Agenda") || 
                        decodedUrl.Contains("/Auditoria") ||
                        decodedUrl.Contains("/CadastroBasico") ||
                        decodedUrl.Contains("/Configuracao") ||
                        decodedUrl.Contains("/Dashboard") ||
                        decodedUrl.Contains("/Estoque") ||
                        decodedUrl.Contains("/Excel") ||
                        decodedUrl.Contains("/Financas") ||
                        decodedUrl.Contains("/Governanca") ||
                        decodedUrl.Contains("/GreenPlanet") ||
                        decodedUrl.Contains("/Home") ||
                        decodedUrl.Contains("/Lavanderia") ||
                        decodedUrl.Contains("/LogBook") ||
                        decodedUrl.Contains("/OrdemServico") ||
                        decodedUrl.Contains("/PCM") ||
                        decodedUrl.Contains("/PlanoAcao") ||
                        decodedUrl.Contains("/PMOC") ||
                        decodedUrl.Contains("/Qualidade") ||
                        decodedUrl.Contains("/Relatorio") ||
                        decodedUrl.Contains("/Treinamento") ||
                        decodedUrl.Contains("/UH") ||
                        decodedUrl.Contains("/Upload")))
                    {
                        if (decodedUrl.Contains("/Governanca") || decodedUrl.Contains("IndexGovernanca"))
                        {
                            Session["codigo_modulo"] = 2;
                            Session["modulo"] = "GOVERNANÇA";
                        }


                        return Redirect(decodedUrl);
                    }
                    else
                    {
                        return RedirectToAction(sDashboardPage, "Home");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Usuário Inativo.");
                }
            }

            login.email = email;
            login.senha = senha;

            imagemLogin imagemLogin = oAccount.LoadImageLogin(sURL: this.Request.RawUrl);
            ViewBag.imagem = imagemLogin;

            return View(login);
        }

        private bool IsRequestFromIOS()
        {
            string userAgent = Request.UserAgent;
            return userAgent != null && (userAgent.Contains("iPad") || userAgent.Contains("iPhone") || userAgent.Contains("iPod"));
        }

        [AllowAnonymous]
        public FileResult Download(string aplicativo)
        {
            string nomeArquivo = aplicativo;
            string extensao = System.IO.Path.GetExtension(nomeArquivo);
            string nomeArquivoV = System.IO.Path.GetFileNameWithoutExtension(nomeArquivo);
            return File(nomeArquivo, "application/apk", nomeArquivoV + extensao);
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


            string emailSenha = oAccount.LoadEmail(sEmail: email);

            if (emailSenha != "")
            {

                string password = getPassword(8);

                string remetente = "no-reply@pcmbysim.com.br"; //O e-mail do remetente
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; 
                MailMessage mail = new MailMessage();
                mail.To.Add(email);
                mail.From = new MailAddress(remetente, "PCM by SIM", System.Text.Encoding.UTF8);
                mail.Subject = String.Concat("Solicitação de nova senha. - ", TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString(), " ", DateTime.Now.ToShortTimeString());
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Body = String.Concat("Olá, <br /> <br />Conforme sua solicitação, segue abaixo sua nova senha.<br /><br />Nova Senha: <b>", password, "</b><br /><br /><b><i> Obs.: Esta senha é provisória e deve ser alterada assim que acessar o PCM by SIM. </i></b><br /><br />Atenciosamente, <br />PCM by SIM");
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential(remetente, "$Noreply@2026$");
                client.Port = 465;
                client.Host = "smtpout.secureserver.net";
                client.EnableSsl = true;

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
            Session["codigo_dashboard"] = null;
            Session["empresa"] = null;
            Session["nome"] = null;
            Session["email"] = null;
            Session["codigo_funcionario"] = null;
            Session["fornecedor_pmoc"] = null;
            Session["codigo_departamento"] = null;
            Session["funcionario"] = null;
            Session["language"] = null;
            Session["aponta_horas"] = null;
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
            Session["audit_corporativo"] = null;
            Session["audit_laudo"] = null;
            Session["audit_normas_procedimentos"] = null;
            Session["audit_relatorio"] = null;
            Session["audit_relatorio_mensal_pcm"] = null;
            Session["cad_apartamento"] = null;
            Session["cad_ar_condicionado"] = null;
            Session["cad_atividade"] = null;
            Session["cad_auditoria_qualidade"] = null;
            Session["cad_categoria"] = null;
            Session["cad_checklist"] = null;
            Session["cad_cliente"] = null;
            Session["cad_cliente_acordo_comercial"] = null;
            Session["adm_cliente"] = null;
            Session["cad_departamento"] = null;
            Session["cad_departamento_gestor"] = null;
            Session["cad_equipamento"] = null;
            Session["cad_enxoval"] = null;
            Session["cad_familia_equipamento"] = null;
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
            Session["cad_justificativa_falta"] = null;
            Session["cad_justificativa_cancelar_ordem_servico"] = null;
            Session["adm_configuracao_desempenho_unidade"] = null;
            Session["cad_prioridade"] = null;
            Session["cad_programada"] = null;
            Session["cad_rotina"] = null;
            Session["cad_setor"] = null;
            Session["cad_tarefa"] = null;
            Session["cad_tipo_apartamento"] = null;
            Session["cad_tipo_ar_condicionado"] = null;
            Session["cad_tipo_cama"] = null;
            Session["cad_tipo_checklist"] = null;
            Session["cad_tipo_despesa"] = null;
            Session["cad_treinamento"] = null;
            Session["cad_unidade"] = null;
            Session["cad_relatorio_itens_auditaveis"] = null;
            Session["cad_item_os_hospede"] = null;
            Session["cfg_opera"] = null;
            Session["fin_contrato"] = null;
            Session["fin_controle_gasto"] = null;
            Session["fin_input_despesa"] = null;
            Session["green_lancamento"] = null;
            Session["governanca"] = null;
            Session["gov_funcionario"] = null;
            Session["gov_planejamento"] = null;
            Session["gov_planejamento_historico"] = null;
            Session["gov_historico"] = null;
            Session["gov_dashboard"] = null;
            Session["gov_apontamento"] = null;
            Session["gov_status_uh"] = null;
            Session["log_book"] = null;
            Session["ordem_servico"] = null;
            Session["ordem_servico_atribuir"] = null;
            Session["uh_atividade"] = null;
            Session["pcm_apontamento_os_edit"] = null;
            Session["pcm_apontamento_laudo"] = null;
            Session["pcm_apontamento_preventiva"] = null;
            Session["pcm_apontamento_os"] = null;
            Session["pcm_apontamento_rotina"] = null;
            Session["pcm_cronograma_semanal"] = null;
            Session["pcm_manutencao_laudo"] = null;
            Session["pcm_manutencao_preventiva"] = null;
            Session["pcm_manutencao_rotina"] = null;
            Session["pcm_historico_programada"] = null;
            Session["pcm_plano_acao"] = null;
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
            Session["rel_manutencao"] = null;
            Session["rel_manutencao_categoria"] = null;
            Session["rel_manutencao_equipamento"] = null;
            Session["rel_manutencao_executor"] = null;
            Session["rel_manutencao_setor"] = null;
            Session["rel_manutencao_solicitante"] = null;
            Session["rel_manutencao_tempo_medio_atendimento"] = null;
            Session["rel_manutencao_tipo_ordem_servico"] = null;
            Session["rel_pmoc_mes"] = null;
            Session["rel_pmoc_bimestral"] = null;
            Session["rel_log_book"] = null;
            Session["rel_nao_conformidade"] = null;
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
            Session["excel_plano_acao_qa"] = null;
            Session["checklist"] = null;


            Session["est_listagem"] = null;
            Session["est_requisicao_compra"] = null;
            Session["est_ordem_compra"] = null;
            Session["est_entrada"] = null;
            Session["est_saida"] = null;
            Session["est_inventario"] = null;
            Session["est_entrada"] = null;


            Session["aeb_contrato"] = null;
            Session["aeb_laudo"] = null;
            Session["aeb_normas_procedimentos"] = null;
            Session["aeb_auditoria_externa"] = null;

            Session["qa_cad_auditoria"] = null;
            Session["qa_auditoria"] = null;
            Session["qa_auditoria_historico"] = null;
            Session["qa_auditoria_cronograma"] = null;
            Session["qa_plano_acao"] = null;
            Session["qa_tarefa"] = null;
            Session["qa_tarefa_historico"] = null;

            Session["dash_desempenho"] = null;
            Session["dash_desempenho_qa"] = null;

            Session["cad_area_comum"] = null;
            Session["agenda_area_comum"] = null;
            Session["agenda_entrada"] = null;
            Session["agenda_saida"] = null;

            Session["upload_excel"] = null;
            Session["upload_pmoc"] = null;

            Session["lav_apontamento"] = null;
            Session["lav_historico"] = null;
            Session["rel_lav_controle"] = null;

            Session["cad_asset"] = null;
            Session["assetMovement"] = null;
            Session["assetInventory"] = null;
            Session["assetInventoryMng"] = null;

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