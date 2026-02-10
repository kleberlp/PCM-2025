using Microsoft.Extensions.Options;
using MESSAGE.SERVICE.Settings;
using System.Net;
using System.Net.Mail;

namespace MESSAGE.SERVICE.Services
{
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public void Send(string to, string subject, string body, string? cc = null)
        {
            string remetente = _settings.From;
            MailMessage mail = new MailMessage();

            if (to != "" && to != null)
            {
                foreach (string email in to.Split(','))
                {
                    if (to != "")
                    {
                        mail.To.Add(to);
                    }
                }
            }
            if (cc != "" && cc != null)
            {
                foreach (string emailCC in cc.Split(','))
                {
                    if (cc != "")
                    {
                        mail.CC.Add(emailCC.Replace(",", ""));
                    }
                }
            }
            System.Diagnostics.Debug.WriteLine("=== Entrou no SendEmailProtocolo ===");
            mail.From = new MailAddress(remetente, "Maneja - Protocolo de Entrega", System.Text.Encoding.UTF8);
            mail.Subject = subject;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = body;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(_settings.User, _settings.Password);
            client.Port = _settings.Port;
            client.Host = _settings.Host;
            client.EnableSsl = _settings.EnableSsl;

            try
            {
                //Envia E-mail                
                client.Send(mail);
                System.Diagnostics.Debug.WriteLine("=== Aeee!!! Funfou no SendEmailProtocolo ===");
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("=== Deu ruim no SendEmailProtocolo ===");
            }


        }
    }
}
