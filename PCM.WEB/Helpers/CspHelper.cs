using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace PCM.WEB
{
    /// <summary>
    /// CSP degrau 1 (docs/MANUAL-MIGRACAO-DOTNET10-SEGURANCA.md): nonce por request
    /// aplicado aos scripts inline das views + emissão do cabeçalho.
    ///
    /// O modo é controlado pelo appSetting "csp:enforce":
    ///   false (padrão) -> Content-Security-Policy-Report-Only: nada é bloqueado,
    ///                     as violações aparecem no console do navegador
    ///   true           -> Content-Security-Policy: política passa a valer
    ///
    /// IMPORTANTE: só ligar o enforce depois do degrau 2 do manual (remoção dos
    /// handlers inline onclick/onchange) — com nonce ativo o navegador ignora o
    /// unsafe-inline e os handlers inline param de funcionar.
    /// </summary>
    public static class Csp
    {
        private const string ItemKey = "csp:nonce";

        /// <summary>Nonce único por request (gerado sob demanda e reutilizado).</summary>
        public static string Nonce
        {
            get
            {
                var context = HttpContext.Current;
                if (context == null) return string.Empty;

                var nonce = context.Items[ItemKey] as string;

                if (nonce == null)
                {
                    var bytes = new byte[16];
                    using (var rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(bytes);
                    }
                    nonce = Convert.ToBase64String(bytes);
                    context.Items[ItemKey] = nonce;
                }

                return nonce;
            }
        }

        /// <summary>Uso nas views: &lt;script @Csp.NonceAttr&gt;</summary>
        public static IHtmlString NonceAttr
        {
            get { return new HtmlString("nonce=\"" + Nonce + "\""); }
        }

        public static bool Enforce
        {
            get
            {
                return string.Equals(ConfigurationManager.AppSettings["csp:enforce"],
                                     "true", StringComparison.OrdinalIgnoreCase);
            }
        }

        public static string HeaderName
        {
            get { return Enforce ? "Content-Security-Policy" : "Content-Security-Policy-Report-Only"; }
        }

        /// <summary>
        /// Política: scripts próprios + nonce + os dois CDNs que as views usam;
        /// style-src mantém unsafe-inline até o degrau 3 (2.737 style= inline).
        /// </summary>
        public static string HeaderValue()
        {
            return "default-src 'self'; " +
                   "script-src 'self' 'nonce-" + Nonce + "' https://cdn.datatables.net https://cdnjs.cloudflare.com; " +
                   "style-src 'self' 'unsafe-inline' https://cdn.datatables.net https://cdnjs.cloudflare.com https://fonts.googleapis.com; " +
                   "font-src 'self' data: https://fonts.gstatic.com https://cdnjs.cloudflare.com; " +
                   "img-src 'self' data: blob:; " +
                   "connect-src 'self'; " +
                   "object-src 'none'; " +
                   "frame-ancestors 'self'; " +
                   "base-uri 'self'; " +
                   "form-action 'self'";
        }
    }

    /// <summary>Emite o cabeçalho CSP uma vez por request (registrado no FilterConfig).</summary>
    public class ContentSecurityPolicyFilter : ActionFilterAttribute
    {
        private const string EmittedKey = "csp:header-emitted";

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            // Child actions (Html.Action) reaproveitam o request: o cabeçalho já foi emitido
            if (!filterContext.IsChildAction)
            {
                var context = filterContext.HttpContext;

                if (!context.Items.Contains(EmittedKey))
                {
                    context.Items[EmittedKey] = true;
                    context.Response.AppendHeader(Csp.HeaderName, Csp.HeaderValue());
                }
            }

            base.OnResultExecuting(filterContext);
        }
    }
}
