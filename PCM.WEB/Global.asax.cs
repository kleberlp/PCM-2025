using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using PCM.WEB.MODELS;
using System.Globalization;

namespace PCM.WEB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Database.SetInitializer<SYSPACKContext>(null);
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
            {
            if (HttpContext.Current.Session != null && Session["language"] != null)
            {
                string langName = Session["language"].ToString();
                CultureInfo ci;
                ci = new CultureInfo(langName);
                System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
                System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
            }
        }
    }
}
