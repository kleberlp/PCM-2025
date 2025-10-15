using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

public class BasicAuthenticationHandler : AuthorizationFilterAttribute
{
    public override void OnAuthorization(HttpActionContext actionContext)
    {
        var authHeader = actionContext.Request.Headers.Authorization;
        if (authHeader != null && authHeader.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
        {
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');
            var username = credentials[0];
            var password = credentials[1];

            // Verifique as credenciais
            if (IsAuthorizedUser(username, password))
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);
                return;
            }
        }

        HandleUnauthorized(actionContext);
    }

    private bool IsAuthorizedUser(string username, string password)
    {
        // Substitua pela lógica de validação real (ex: buscar no banco de dados)
        return username == "intercity" && password == "PCMbyS1M_ICG2024";
    }

    private void HandleUnauthorized(HttpActionContext actionContext)
    {
        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
        actionContext.Response.Headers.Add("WWW-Authenticate", "Basic realm=\"MyAPI\"");
    }
}
