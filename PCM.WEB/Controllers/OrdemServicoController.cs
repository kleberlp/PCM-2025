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
using System.Linq;
using System.Collections;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using Microsoft.Owin.Mapping;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.ComponentModel.DataAnnotations;
using NPOI.SS.Formula.Functions;
using OfficeOpenXml;
using System.ComponentModel;

namespace PCM.WEB.Controllers
{
    public class OrdemServicoController : Controller
    {
        private const int V = 1;
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.OrdemServico oOrdemServico = new DAL.OrdemServico(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.PCM oPCM = new DAL.PCM(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Picture oPicture = new DAL.Picture(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
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

        //JSON: /DEPARTAMENTO/
        public JsonResult LoadDepartamento()
        {
            return Json(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())));
        }

        //JSON: /FUNCIONARIO/
        public JsonResult LoadFuncionario(int unidade)
        {
            return Json(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                           iCodigoUnidade: unidade,
                                           iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])));
        }

        //JSON: /SOLICITANTE/   
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
                                            iCodigoUnidade: unidade, 
                                            iCodigoApartamento: apartamento));
        }

        //JSON: /INSERT - ORDEM DE SERVIÇO/
        public string InsertOrdemServico(int unidade, string data, int prioridade, string descricao, int setor = -1, int apartamento = -1, long equipamento = -1)
        {
            if (Session["empresa"] == null)
            {
                return "";
            }
            else
            {
                long codigo = 0;
                string ordem_servico = "";
                string body = "";
                string token = "";
                string to = "";
                        
                oOrdemServico.InsertOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
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
                                                    lCodigo: ref codigo,
                                                    sOrdemServico: ref ordem_servico,
                                                    sTo: ref to,
                                                    sToken: ref token,
                                                    sBody: ref body);

                string filename = "";
                string path = Path.Combine("C:\\", "SIM", "PCM", "SITE", "IMAGE", "OS", Session["empresa"].ToString(), unidade.ToString(), codigo.ToString());

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
                                                sTipo: "ORDEM SERVIÇO",
                                                iCodigoItemChecklist: -1,
                                                sImagePath: Path.Combine(path, filename));

                    }
                }

                //Retorna Ordem de Serviço
                return ordem_servico;
            }
        }

        public void ResizeAndSaveImage(Stream imageStream, string outputFilePath)
        {

            // Carrega a imagem a partir do Stream
            using (Bitmap originalImage = new Bitmap(imageStream))
            {

                double scaleFactor = (originalImage.Width > originalImage.Height) ? 400.0 / originalImage.Width : 400.0 / originalImage.Height;

                // Calcula a nova largura e altura com base no fator de escala
                int newWidth = (int)(originalImage.Width * scaleFactor);
                int newHeight = (int)(originalImage.Height * scaleFactor);

                // Cria uma nova imagem redimensionada
                using (Bitmap resizedImage = new Bitmap(newWidth, newHeight))
                {
                    // Configura o objeto Graphics para redimensionar a imagem com alta qualidade
                    using (Graphics graphics = Graphics.FromImage(resizedImage))
                    {
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                    }

                    // Salva a imagem redimensionada no caminho de destino
                    resizedImage.Save(outputFilePath, System.Drawing.Imaging.ImageFormat.Png); // ou outro formato conforme necessário
                }
            }
        }

        public void SendMessage(String ordem_servico, String descricao, string to)
        {

            List<string> deviceTokens = to.Split(new char[] { (char)13 }).ToList();
            deviceTokens.Remove("");

            if (deviceTokens.Count > 0)
            {

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
            oOrdemServico.UpdateOrdemServicoStatus(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    lCodigo: codigo,
                                                    iCodigoUnidade: unidade,
                                                    iStatus: status);

            return Json(true);

        }

        //JSON: /VINCULA FUNCIONARIO/
        public bool VincularFuncionarioOS(long codigo, int unidade, int status, string funcionario = "", string funcionario_nome = "")
        {
            return oOrdemServico.VincularFuncionarioOS(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        lCodigo: codigo,
                                                        iCodigoUnidade: unidade,
                                                        iStatus: status,
                                                        sCodigoFuncionario: funcionario,
                                                        sNomeFuncionario: funcionario_nome);
        }

        //JSON: /VALIDA OS/
        public JsonResult ValidateOrdemServico(int unidade, long equipamento)
        {
            return Json(oOrdemServico.ValidateOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            lCodigoEquipamento: equipamento));
        }

        #endregion

        #region ::: ORDEM DE SERVIÇO :::

        // GET: INDEX
        public ActionResult OrdemServicoIndex2(int status = 1, int responsavel_apartamento = -1, int hospede = -1)
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
                bool apontamento = false;

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

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "pcm_apontamento_os",
                                    sDireito: "inserir",
                                    bReturn: ref apontamento);

                ViewBag.hospede = hospede;

                if (Session["os_ordem_servico"] == null || status > 1)
                {

                    string data_inicio = DateTime.Now.AddMonths(-1).ToShortDateString();
                    string data_termino = DateTime.Now.ToShortDateString();
                    
                    ViewBag.administrador_vincular = administrador_vincular;
                    ViewBag.administrador = administrador;
                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;
                    ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                    ViewBag.data_inicio = data_inicio;
                    ViewBag.data_termino = data_termino;
                    ViewBag.ordem_servico = "";
                    ViewBag.apontamento = apontamento;
                    ViewBag.empresa = Session["empresa"].ToString();
                    ViewBag.usuario = User.Identity.GetUserName();                    
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
                    ViewBag.justificativa_apontamento = new SelectList(oCombo.JustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"])), "codigo", "descricao", null);
                    ViewBag.justificativa_cancelamento = new SelectList(oCombo.JustificativaCancelamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", null);
                    ViewBag.status = new SelectList(oCombo.StatusManutencao(), "codigo", "descricao", status);
                    ViewBag.responsavel_apartamento = new SelectList(oCombo.ResponsavelApartamento(), "codigo", "descricao", responsavel_apartamento);
                    ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", Convert.ToInt32(Session["codigo_departamento"]));
                    ViewBag.origem = new SelectList(oCombo.LoadCombo("sp_select_combo_static_origem_ordem_servico", codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", null);

                    return View();
                }
                else
                {

                    ViewBag.administrador = administrador;
                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;
                    ViewBag.administrador_vincular = administrador_vincular;
                    ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                    ViewBag.data_inicio = Session["os_data_inicio"].ToString();
                    ViewBag.data_termino = Session["os_data_termino"].ToString();
                    ViewBag.ordem_servico = Session["os_ordem_servico"].ToString();
                    ViewBag.apontamento = apontamento;
                    ViewBag.usuario = User.Identity.GetUserName();
                    ViewBag.codigo_unidade = Convert.ToInt32(Session["os_unidade"].ToString());
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: false), "codigo", "descricao", Convert.ToInt32(Session["os_unidade"]));
                    ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["os_unidade"])), "codigo", "descricao", Convert.ToInt32(Session["os_setor"]));
                    ViewBag.apartamento = new SelectList(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["os_unidade"]), iCodigoSetor: -1), "codigo", "descricao", Convert.ToInt32(Session["os_apartamento"]));
                    ViewBag.equipamento = new SelectList(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["os_unidade"].ToString())), "codigo", "descricao", Convert.ToInt32(Session["os_equipamento"]));
                    ViewBag.prioridade = new SelectList(oCombo.Prioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["os_unidade"])), "codigo", "descricao", Convert.ToInt32(Session["os_prioridade"]));
                    ViewBag.solicitante = new SelectList(oCombo.Solicitante(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["os_unidade"])), "codigo", "descricao", Convert.ToInt32(Session["os_solicitante"]));
                    ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["os_unidade"]),
                                                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", Convert.ToInt32(Session["os_setor"]));
                    ViewBag.justificativa_apontamento = new SelectList(oCombo.JustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                        iCodigoUnidade: Convert.ToInt32(Session["os_unidade"])), "codigo", "descricao", Convert.ToInt32(Session["os_justificativa_apontamento"].ToString()));
                    ViewBag.justificativa_cancelamento = new SelectList(oCombo.JustificativaCancelamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", null);
                    ViewBag.status = new SelectList(oCombo.StatusManutencao(), "codigo", "descricao", Convert.ToInt32(Session["os_status"].ToString()));
                    ViewBag.responsavel_apartamento = new SelectList(oCombo.ResponsavelApartamento(), "codigo", "descricao", Convert.ToInt32(Session["os_responsavel_apartamento"].ToString()));
                    ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", Convert.ToInt32(Session["codigo_departamento"]));
                    ViewBag.origem = new SelectList(oCombo.LoadCombo("sp_select_combo_static_origem_ordem_servico", codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", null);

                    return View();
                }
            }
        }

        public JsonResult LoadOrdemServicoIndex(int empresa, int usuario, int unidade, int departamento, string ordem_servico, string ordem_servico_cliente, string data_inicio, string data_termino, int setor = -1, int prioridade = -1, int equipamento = -1, int solicitante = -1, int responsavel_apartamento = -1, int apartamento = -1, string executor = "", int status = -1, int justificativa_apontamento = -1, string data_execucao_inicio = "", string data_execucao_termino = "", int hospede = -1, int origem = -1)
        {
            List<MODELS.OrdemServico> result = new List<MODELS.OrdemServico>();

            result = oOrdemServico.IndexOrdemServico(iCodigoEmpresa: empresa,
                                                     iCodigoUsuario: usuario,
                                                     iCodigoUnidade: unidade,
                                                     iCodigoDepartamento: departamento,
                                                     sOrdemServico: ordem_servico,
                                                     sOrdemServicoCliente: ordem_servico_cliente,
                                                     sDataInicio: data_inicio,
                                                     sDataTermino: data_termino,
                                                     iCodigoSetor: setor,
                                                     iCodigoPrioridade: prioridade,
                                                     lCodigoEquipamento: equipamento,
                                                     iCodigoSolicitante: solicitante,
                                                     iCodigoResponsavelApartamento: responsavel_apartamento,
                                                     iCodigoApartamento: apartamento,
                                                     sExecutor: executor,
                                                     iStatus: status,
                                                     iCodigoJustificativaApontamento: justificativa_apontamento,
                                                     iHospede: hospede,
                                                     codigoOrigem: origem);


            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public void OrdemServicoExcel(int empresa, int usuario, int unidade, int departamento, string ordem_servico, string ordem_servico_cliente, string data_inicio, string data_termino, int setor = -1, int prioridade = -1, int equipamento = -1, int solicitante = -1, int responsavel_apartamento = -1, int apartamento = -1, string executor = "", int status = -1, int justificativa_apontamento = -1)
        {

            ExcelPackage.License.SetNonCommercialOrganization("<ACTI>");

            ExcelPackage excel = oOrdemServico.ExcelOrdemServico(iCodigoEmpresa: empresa,
                                                                 iCodigoUsuario: usuario,
                                                                 iCodigoUnidade: unidade,
                                                                 iCodigoDepartamento: departamento,
                                                                 sOrdemServico: ordem_servico,
                                                                 sOrdemServicoCliente: ordem_servico_cliente,
                                                                 sDataInicio: data_inicio,
                                                                 sDataTermino: data_termino,
                                                                 iCodigoSetor: setor,
                                                                 iCodigoPrioridade: prioridade,
                                                                 lCodigoEquipamento: equipamento,
                                                                 iCodigoSolicitante: solicitante,
                                                                 iCodigoResponsavelApartamento: responsavel_apartamento,
                                                                 iCodigoApartamento: apartamento,
                                                                 sExecutor: executor,
                                                                 iStatus: status,
                                                                 iCodigoJustificativaApontamento: justificativa_apontamento);

            using (var memoryStream = new MemoryStream())
            {
                HttpResponse Response = System.Web.HttpContext.Current.Response;
                Response.ClearContent();
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=OrdemServicoList.xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }

        public JsonResult LoadOrdemServicoApontamento(int empresa, int unidade, long codigo_ordem_servico)
        {
            List<MODELS.OrdemServicoApontamento> result = new List<MODELS.OrdemServicoApontamento>();

            result = oOrdemServico.OrdemServicoApontamento(iCodigoEmpresa: empresa,
                                                           iCodigoUnidade: unidade,
                                                           lCodigoOrdemServico: codigo_ordem_servico);


            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        // GET: INDEX
        public ActionResult OrdemServicoIndex(int status = 1, int responsavel_apartamento = -1)
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
                bool apontamento = false;

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

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "pcm_apontamento_os",
                                    sDireito: "inserir",
                                    bReturn: ref apontamento);


            if (Session["os_ordem_servico"] == null || status > 1)
            {

                string data_inicio = (status == -1) ? TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString() : "";
                string data_termino = (status == -1) ? TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString() : "";

                ViewBag.administrador_vincular = administrador_vincular;
                ViewBag.administrador = administrador;
                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.data_inicio = "";
                ViewBag.data_termino = "";
                ViewBag.ordem_servico = "";
                ViewBag.apontamento = apontamento;
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
                ViewBag.justificativa_apontamento = new SelectList(oCombo.JustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"])), "codigo", "descricao", null);
                ViewBag.status = new SelectList(oCombo.StatusManutencao(), "codigo", "descricao", status);
                ViewBag.responsavel_apartamento = new SelectList(oCombo.ResponsavelApartamento(), "codigo", "descricao", responsavel_apartamento);
                ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", Convert.ToInt32(Session["codigo_departamento"]));

                return View(oOrdemServico.IndexOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                            iCodigoDepartamento: Convert.ToInt32(Session["codigo_departamento"].ToString()),
                                                            sOrdemServico: "",
                                                            sOrdemServicoCliente: "",
                                                            sDataInicio: data_inicio,
                                                            sDataTermino: data_termino,
                                                            iCodigoSetor: -1,
                                                            iCodigoPrioridade: -1,
                                                            lCodigoEquipamento: -1,
                                                            iCodigoSolicitante: -1,
                                                            iCodigoResponsavelApartamento: responsavel_apartamento,
                                                            iCodigoApartamento: -1,
                                                            sExecutor: "",
                                                            iStatus: status));
            } else
            {

                ViewBag.administrador = administrador;
                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.administrador_vincular = administrador_vincular;
                ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.data_inicio = Session["os_data_inicio"].ToString();
                ViewBag.data_termino = Session["os_data_termino"].ToString();
                ViewBag.ordem_servico = Session["os_ordem_servico"].ToString();
                ViewBag.apontamento = apontamento;
                ViewBag.codigo_unidade = Convert.ToInt32(Session["os_unidade"].ToString());
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Convert.ToInt32(Session["os_unidade"]));
                ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["os_unidade"])), "codigo", "descricao", Convert.ToInt32(Session["os_setor"]));
                ViewBag.apartamento = new SelectList(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["os_unidade"]), iCodigoSetor: -1), "codigo", "descricao", Convert.ToInt32(Session["os_apartamento"]));
                ViewBag.equipamento = new SelectList(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["os_unidade"].ToString())), "codigo", "descricao", Convert.ToInt32(Session["os_equipamento"]));
                ViewBag.prioridade = new SelectList(oCombo.Prioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["os_unidade"])), "codigo", "descricao", Convert.ToInt32(Session["os_prioridade"]));
                ViewBag.solicitante = new SelectList(oCombo.Solicitante(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["os_unidade"])), "codigo", "descricao", Convert.ToInt32(Session["os_solicitante"]));
                ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["os_unidade"]),
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", Convert.ToInt32(Session["os_setor"]));
                ViewBag.justificativa_apontamento = new SelectList(oCombo.JustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                    iCodigoUnidade: Convert.ToInt32(Session["os_unidade"])), "codigo", "descricao", Convert.ToInt32(Session["os_justificativa_apontamento"].ToString()));
                ViewBag.status = new SelectList(oCombo.StatusManutencao(), "codigo", "descricao", Convert.ToInt32(Session["os_status"].ToString()));
                ViewBag.responsavel_apartamento = new SelectList(oCombo.ResponsavelApartamento(), "codigo", "descricao", Convert.ToInt32(Session["os_responsavel_apartamento"].ToString()));
                ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", Convert.ToInt32(Session["codigo_departamento"]));

                return View(oOrdemServico.IndexOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["os_unidade"].ToString()),
                                                            iCodigoDepartamento: Convert.ToInt32(Session["os_departamento"].ToString()),
                                                            sOrdemServico: Session["os_ordem_servico"].ToString(),
                                                            sOrdemServicoCliente: Session["os_ordem_servico_cliente"].ToString(),
                                                            sDataInicio: Session["os_data_inicio"].ToString(),
                                                            sDataTermino: Session["os_data_termino"].ToString(),
                                                            iCodigoSetor: Convert.ToInt32(Session["os_setor"].ToString()),
                                                            iCodigoPrioridade: Convert.ToInt32(Session["os_prioridade"].ToString()),
                                                            lCodigoEquipamento: Convert.ToInt32(Session["os_equipamento"].ToString()),
                                                            iCodigoSolicitante: Convert.ToInt32(Session["os_solicitante"].ToString()),
                                                            iCodigoResponsavelApartamento: Convert.ToInt32(Session["os_responsavel_apartamento"].ToString()),
                                                            iCodigoApartamento: Convert.ToInt32(Session["os_apartamento"].ToString()),
                                                            sExecutor: Session["os_executor"].ToString(),
                                                            iStatus: Convert.ToInt32(Session["os_status"].ToString())));
            }
        }
        }

        // POST: INDEX
        [HttpPost]
        public ActionResult OrdemServicoIndex(int unidade = -1, int departamento = -1, int codigo_unidade = -1, string data_inicio = "", string data_termino = "", string ordem_servico = "", string ordem_servico_cliente = "", int setor = -1, int prioridade = -1, long equipamento = -1, int status = -1, int responsavel_apartamento = -1, string executor = "", int solicitante = -1, int apartamento = -1, int justificativa_apontamento = -1)
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
                bool apontamento = false;

                Session["os_unidade"] = unidade;
                Session["os_departamento"] = departamento;
                Session["os_codigo_unidade"] = codigo_unidade;
                Session["os_data_inicio"] = data_inicio;
                Session["os_data_termino"] = data_termino;
                Session["os_ordem_servico"] = ordem_servico;
                Session["os_ordem_servico_cliente"] = ordem_servico_cliente;
                Session["os_setor"] = setor;
                Session["os_prioridade"] = prioridade;
                Session["os_equipamento"] = equipamento;
                Session["os_status"] = status;
                Session["os_responsavel_apartamento"] = responsavel_apartamento;
                Session["os_executor"] = executor;
                Session["os_solicitante"] = solicitante;
                Session["os_apartamento"] = apartamento;
                Session["os_justificativa_apontamento"] = justificativa_apontamento;

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

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "pcm_apontamento_os",
                                    sDireito: "inserir",
                                    bReturn: ref apontamento);

                ViewBag.administrador_vincular = administrador_vincular;
                ViewBag.apontamento = apontamento;
                ViewBag.administrador = administrador;
                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;
                ViewBag.ordem_servico = ordem_servico;
                ViewBag.ordem_servico_cliente = ordem_servico_cliente;
                ViewBag.codigo_unidade = unidade;
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
                ViewBag.justificativa_apontamento = new SelectList(oCombo.JustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                    iCodigoUnidade: codigo_unidade), "codigo", "descricao", justificativa_apontamento);
                ViewBag.responsavel_apartamento = new SelectList(oCombo.ResponsavelApartamento(), "codigo", "descricao", responsavel_apartamento);
                ViewBag.executor = executor;
                ViewBag.status = new SelectList(oCombo.StatusManutencao(), "codigo", "descricao", status);
                ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", departamento);

                return View(oOrdemServico.IndexOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            iCodigoUnidade: (unidade == -1)? codigo_unidade: unidade,
                                                            iCodigoDepartamento: departamento,
                                                            sOrdemServico: ordem_servico,
                                                            sOrdemServicoCliente: ordem_servico_cliente,
                                                            sDataInicio: data_inicio,
                                                            sDataTermino: data_termino,
                                                            iCodigoSetor: setor,
                                                            iCodigoPrioridade: prioridade,
                                                            lCodigoEquipamento: equipamento,
                                                            iCodigoApartamento: apartamento,
                                                            sExecutor: executor,
                                                            iCodigoSolicitante: solicitante,
                                                            iCodigoResponsavelApartamento: responsavel_apartamento,
                                                            iStatus: status,
                                                            iCodigoJustificativaApontamento: justificativa_apontamento));
            }
        }

        // GET: Concluir OS
        public ActionResult ConcluirOS(long codigo_pcm_ordem_servico, int unidade)
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

                return RedirectToAction("OrdemServicoIndex2");
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

                return RedirectToAction("OrdemServicoIndex2", "OrdemServico");
            }
        }

        // GET: INDEX
        public ActionResult OrdemServicoAtribuir()
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
                                    sFormulario: "ordem_servico_atribuir",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.data_inicio = "";
                ViewBag.data_termino = "";
                ViewBag.ordem_servico = "";
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
                    
                return View(oOrdemServico.OrdemServicoAtribuir(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                sOrdemServico: "",
                                                                sDataInicio: "",
                                                                sDataTermino: "",
                                                                iCodigoSetor: -1,
                                                                iCodigoPrioridade: -1,
                                                                lCodigoEquipamento: -1,
                                                                iCodigoSolicitante: -1,
                                                                sExecutor: ""));
            }
        }

        // POST: INDEX
        [HttpPost]
        public ActionResult OrdemServicoAtribuir(int unidade = -1, string data_inicio = "", string data_termino = "", string ordem_servico = "", int setor = -1, int prioridade = -1, long equipamento = -1, string executor = "", int solicitante = -1, int apartamento = -1)
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
                                    sFormulario: "ordem_servico_atribuir",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;
                ViewBag.ordem_servico = ordem_servico;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
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
                ViewBag.executor = executor;

                return View(oOrdemServico.OrdemServicoAtribuir(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                iCodigoUnidade: unidade,
                                                                sOrdemServico: ordem_servico,
                                                                sDataInicio: data_inicio,
                                                                sDataTermino: data_termino,
                                                                iCodigoSetor: setor,
                                                                iCodigoPrioridade: prioridade,
                                                                lCodigoEquipamento: equipamento,
                                                                sExecutor: executor,
                                                                iCodigoSolicitante: solicitante));
            }
        }

        // GET: INSERT
        public ActionResult OrdemServicoInsert()
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

        // GET: /EDIT
        public ActionResult OrdemServicoEdit(long codigo, int unidade)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                MODELS.OrdemServico ordem_servico = null;

                oOrdemServico.InfoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                lCodigo: codigo,
                                                iCodigoUnidade: unidade,
                                                sTipo: "OS",
                                                oOrdemServico: ref ordem_servico);

                if (ordem_servico == null)
                {
                    return HttpNotFound();
                }

                ViewBag.setor = new SelectList(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: ordem_servico.codigo_unidade), "codigo", "descricao", ordem_servico.codigo_setor);
                ViewBag.apartamento = new SelectList(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                        iCodigoUnidade: ordem_servico.codigo_unidade, 
                                                                        iCodigoSetor: ordem_servico.codigo_setor), "codigo", "descricao", ordem_servico.codigo_apartamento);
                ViewBag.prioridade = new SelectList(oCombo.Prioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                        iCodigoUnidade: ordem_servico.codigo_unidade), "codigo", "descricao", ordem_servico.codigo_prioridade);
                ViewBag.tipo_servico = new SelectList(oCombo.TipoServico(), "codigo", "descricao", ordem_servico.codigo_tipo_servico);
                ViewBag.tipo_ordem_servico = new SelectList(oCombo.TipoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                                    iOrdemServico: 1), "codigo", "descricao", ordem_servico.codigo_tipo_ordem_servico);
                ViewBag.justificativaApontamento = new SelectList(oCombo.JustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                  iCodigoUnidade: ordem_servico.codigo_unidade), "codigo", "descricao", null);
                ViewBag.equipamento = new SelectList(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: ordem_servico.codigo_unidade,
                                                                        iCodigoSetor: ordem_servico.codigo_setor,
                                                                        iCodigoApartamento: ordem_servico.codigo_apartamento), "codigo", "descricao", ordem_servico.codigo_equipamento);

                ViewBag.data = ordem_servico.data;
                ViewBag.codigo_unidade = ordem_servico.codigo_unidade;
                ViewBag.arquivo = ordem_servico.arquivo;

                return View(ordem_servico);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrdemServicoEdit(int unidade, string data, int prioridade, string descricao, HttpPostedFileBase arquivo, long codigo, int unidade_old, string change_imagem, int setor = -1, int apartamento = -1, long equipamento = -1, int justificativaApontamento = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                string filename = "";
                string path = Path.Combine("C:\\", "SIM", "PCM", "SITE", "IMAGE", "OS", Session["empresa"].ToString(), unidade.ToString(), codigo.ToString());

                if (change_imagem == "change")
                {
                    if (arquivo != null)
                    {
                        filename = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(arquivo.FileName);
                        if (System.IO.File.Exists(Path.Combine(path, filename))) { System.IO.File.Delete(Path.Combine(path, filename)); }
                        ResizeAndSaveImage(arquivo.InputStream, Path.Combine(path, filename));

                        oPicture.InsertPicture(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: unidade,
                                                lCodigo: codigo,
                                                sTipo: "ORDEM SERVIÇO",
                                                iCodigoItemChecklist: -1,
                                                sImagePath: Path.Combine(path, filename));
                    }
                }

                oOrdemServico.UpdateOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                iCodigoUnidade: unidade,
                                                iCodigoSetor: setor,
                                                iCodigoApartamento: apartamento,
                                                lCodigoEquipamento: equipamento,
                                                sData: data,
                                                iCodigoJustificativaAlteracaoData: justificativaApontamento,
                                                iCodigoPrioridade: prioridade,
                                                sDescricao: descricao,
                                                sImagem: "",
                                                sArquivo: "",
                                                lCodigo: codigo,
                                                iCodigoUnidadeOld: unidade_old);

                //Redireciona para Index
                return RedirectToAction("OrdemServicoIndex2");
            }
        }

        // GET: /DELETE
        public ActionResult OrdemServicoDelete(long codigo, int unidade)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                MODELS.OrdemServico ordem_servico = null;

                oOrdemServico.InfoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                lCodigo: codigo,
                                                iCodigoUnidade: unidade,
                                                sTipo: "OS",
                                                oOrdemServico: ref ordem_servico);

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
        public ActionResult OrdemServicoDelete([Bind(Include = "codigo, codigo_unidade")] MODELS.OrdemServico ordem_servico)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oOrdemServico.DeleteOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        lCodigo: ordem_servico.codigo,
                                        iCodigoUnidade: ordem_servico.codigo_unidade);

                //Redireciona para Index
                return RedirectToAction("OrdemServicoIndex2");
            }
        }

        // POST: /DELETE
        [HttpPost]
        public JsonResult OrdemServicoCancelar(long codigo, int codigo_unidade, int justificativa)
        {
                //Insere Registro no Banco de Dados
                oOrdemServico.CancelartOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    lCodigo: codigo,
                                                    iCodigoUnidade: codigo_unidade,
                                                    iCodigoJustificativaCancelamento: justificativa);

                //Redireciona para Index
                return Json("True");
        }

        // GET: /VIEW
        public ActionResult OrdemServicoView(long codigo, int unidade)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                MODELS.OrdemServico ordem_servico = null;

                oOrdemServico.InfoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                lCodigo: codigo,
                                                iCodigoUnidade: unidade,
                                                sTipo: "OS",
                                                oOrdemServico: ref ordem_servico);

                ViewBag.ordem_servico = ordem_servico;

                return View(oOrdemServico.OrdemServicoApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: unidade,
                                                                    lCodigoOrdemServico: codigo));
            }
        }

        // GET: /VIEW
        [AllowAnonymous]
        public ActionResult OrdemServicoViewEmail(int empresa, long codigo, int unidade)
        {
            MODELS.OrdemServico ordem_servico = null;

            oOrdemServico.InfoOrdemServico(iCodigoEmpresa: empresa,
                                            lCodigo: codigo,
                                            iCodigoUnidade: unidade,
                                            sTipo: "OS",
                                            oOrdemServico: ref ordem_servico);

            ViewBag.ordem_servico = ordem_servico;

            return View(oOrdemServico.OrdemServicoApontamento(iCodigoEmpresa: empresa,
                                                                iCodigoUnidade: unidade,
                                                                lCodigoOrdemServico: codigo));
        }


        // POST: /PRINT
        public ActionResult OrdemServicoPrintPDF(int codigo_empresa, int codigo_unidade, long codigo_ordem_servico)
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

        // POST: /BACKLOG
        [HttpPost]
        public JsonResult SendBacklog(string codigos)
        {
            defaultResponse response = new defaultResponse();

            try
            {

                foreach (string row in codigos.Split(','))
                {
                    oOrdemServico.UpdateOrdemServicoStatus(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                           iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                           lCodigo: Convert.ToInt64(row.Split('|')[0]),
                                                           iCodigoUnidade: Convert.ToInt32(row.Split('|')[1]),
                                                           iStatus: 0);

                }

                response.success = true;
                response.message = "Ordens de serviço enviadas para backlog com sucesso.";

            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = "Ocorreu um erro ao enviar as ordens de serviço para backlog: " + ex.Message;                
            }

            return Json(response);

        }

        // POST: /BACKLOG
        [HttpPost]
        public JsonResult ReturnBacklog(string codigos)
        {
            defaultResponse response = new defaultResponse();

            try
            {

                foreach (string row in codigos.Split(','))
                {
                    oOrdemServico.UpdateOrdemServicoStatus(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                           iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                           lCodigo: Convert.ToInt64(row.Split('|')[0]),
                                                           iCodigoUnidade: Convert.ToInt32(row.Split('|')[1]),
                                                           iStatus: 1);

                }

                response.success = true;
                response.message = "Ordens de serviço retornadas do backlog com sucesso.";

            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = "Ocorreu um erro ao retornar as ordens de serviço para backlog: " + ex.Message;
            }

            return Json(response);

        }

        #endregion

    }
}