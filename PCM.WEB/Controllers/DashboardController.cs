using PCM.WEB.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.Mvc;

namespace PCM.WEB.Controllers
{
    public class DashboardController : Controller
    {
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.Dashboard oDashboard = new DAL.Dashboard(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: CHART :::

            //JSON: /DATA
            public JsonResult DataMonth(int unidade, string data)
            {
                return Json(oDashboard.Mes(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                           iCodigoUnidade: unidade,
                                           sData: data));
            }

            //JSON: /DATA DAY
            public JsonResult DataDay(string data)
            {
                List<String> day = new List<string>();

                for (int i = 1; i <= DateTime.DaysInMonth(Convert.ToDateTime(data).Year, Convert.ToDateTime(data).Month); i++)
                {
                    DateTime currentDate = new DateTime(Convert.ToDateTime(data).Year, Convert.ToDateTime(data).Month, i);
                    day.Add(currentDate.ToString("dd/MM/yy", CultureInfo.InvariantCulture));
                }

                return Json(day);
            }

            //JSON: /EVOLUÇÃO MENSAL
            public JsonResult ChartEvolucaoMensal(string data)
            {
                return Json(oDashboard.ChartEvolucaoMensal(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                           iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                           sData: data));
            }

            //JSON: /EVOLUÇÃO MENSAL - ATENDIMENTO OS
            public JsonResult ChartEvolucaoMensalAtendimentoOS(string data)
            {
                return Json(oDashboard.ChartEvolucaoMensalAtendimentoOS(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                        sData: data));
            }

            //JSON: /EVOLUÇÃO MENSAL - ATENDIMENTO OS
            public JsonResult ChartEvolucaoHH(string data)
            {
                return Json(oDashboard.ChartEvolucaoHH(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                       iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                       sData: data));
            }

            //JSON: /EVOLUÇÃO MENSAL - ATENDIMENTO OS
            public JsonResult ChartEvolucaoPMOC(string data)
            {
                return Json(oDashboard.ChartEvolucaoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                         iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                         sData: data));
            }

        #endregion

        #region ::: UNIDADES :::

            //JSON: /UNIDADES
            public JsonResult Unidades(string data)
            {
                return Json(oDashboard.Unidades(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())));
            }

            //JSON: /CHART GERAL
            public JsonResult ChartGeral(string data)
            {
                return Json(oDashboard.ChartGeral(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                  sData: data));
            }

            //JSON: /CHART MÉTRICA
            public JsonResult ChartMetricas(String field, String descricao, string data)
            {
                return Json(oDashboard.ChartMetrica(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    sData: data,
                                                    sField: field,
                                                    sDescricao: descricao));
            }

            //JSON: /CHART NÚMERO OS
            public JsonResult ChartNumeroOS(string data)
            {
                return Json(oDashboard.ChartNumeroOS(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                     sData: data));
            }

            //JSON: /CHART TEMPO MEDIO ATENDIMENTO
            public JsonResult ChartTempoMedioAtendimento(string data)
            {
                return Json(oDashboard.ChartTempoMedioAtendimento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                  sData: data));
            }

            //JSON: /CHART MEDIA OS COLABORADOR
            public JsonResult ChartMediaOSColaborador(string data)
            {
                return Json(oDashboard.ChartMediaOSColaborador(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                               sData: data));
            }

        #endregion

        #region ::: QUALIDADE :::

            //JSON: /DATA
            public JsonResult QualidadeDataMonth(int unidade, string data)
            {
                return Json(oDashboard.MesQualidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: unidade,
                                                    sData: data));
            }

            //JSON: /Qualidade - Notas - Unidade
            public JsonResult QualidadeNotasUnidade(int unidade,
                                                    string data)
            {
                return Json(oDashboard.ChartQualidadeNotasUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                  iCodigoUnidade: unidade,
                                                                  sData: data));
            }

            //JSON: /Qualidade - Notas
            public JsonResult QualidadeNotas(string data)
            {
                return Json(oDashboard.ChartQualidadeNotas(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                           sData: data));
            }

        #endregion

    }
}