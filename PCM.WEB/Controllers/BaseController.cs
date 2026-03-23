using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;

namespace PCM.WEB.Controllers
{
    public class BaseController : Controller
    {
        protected bool IsSessionValid()
        {
            return Session["empresa"] != null && User.Identity.GetUserName() != "";
        }

        protected int codigoEmpresa
        {
            get { return Convert.ToInt32(Session["empresa"]); }
        }

        protected int codigoUsuario
        {
            get { return Convert.ToInt32(User.Identity.GetUserName()); }
        }

        protected int codigoUnidade
        {
            get { return Convert.ToInt32(Session["codigo_unidade"]); }
        }
    }
}