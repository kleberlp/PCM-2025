using System;
using System.Web;
using System.Configuration;
using System.Web.Mvc;
using PCM.WEB.DAL;
using Microsoft.AspNet.Identity;
using System.IO;
using OfficeOpenXml;

namespace PCM.ADM.WEB.Controllers
{
    public class ExcelController : Controller
    {
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnectionAdm"].ConnectionString);
        private Excel oExcel = new Excel(ConfigurationManager.ConnectionStrings["DefaultConnectionAdm"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnectionAdm"].ConnectionString);

    }
}