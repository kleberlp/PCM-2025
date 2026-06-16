using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PCM.INTERFACE.ATRIO.Configuration;
using System.Net;
using System.Net.Mail;

namespace PCM.INTERFACE.ATRIO.Infrastructure;

public interface IEmailNotificationService
{
    Task SendErrorAsync(string subject, string body, Exception? ex = null);
    Task SendInfoAsync(string subject, string body);
}

public class EmailNotificationService : IEmailNotificationService
{
    private readonly EmailSettings _settings;
    private readonly ILogger<EmailNotificationService> _logger;

    public EmailNotificationService(
        IOptions<EmailSettings> options,
        ILogger<EmailNotificationService> logger)
    {
        _settings = options.Value;
        _logger   = logger;
    }

    public Task SendErrorAsync(string subject, string body, Exception? ex = null)
    {
        var fullBody = ex is null
            ? body
            : $"{body}\n\n--- Detalhes do Erro ---\n{ex}";

        return SendAsync($"[ERRO] PCM Interface ATRIO — {subject}", fullBody);
    }

    public Task SendInfoAsync(string subject, string body) =>
        SendAsync($"[INFO] PCM Interface ATRIO — {subject}", body);

    private async Task SendAsync(string subject, string body)
    {
        if (!_settings.Enabled)
        {
            _logger.LogDebug("Notificação por e-mail desabilitada. Assunto: {Subject}", subject);
            return;
        }

        if (_settings.Recipients.Count == 0)
        {
            _logger.LogWarning("Nenhum destinatário configurado para envio de e-mail.");
            return;
        }

        try
        {
            using var smtp = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort)
            {
                EnableSsl   = _settings.UseSsl,
                Credentials = new NetworkCredential(_settings.Username, _settings.Password)
            };

            using var message = new MailMessage
            {
                From       = new MailAddress(_settings.FromAddress, _settings.FromName),
                Subject    = subject,
                Body       = body,
                IsBodyHtml = false
            };

            foreach (var recipient in _settings.Recipients)
                message.To.Add(recipient);

            await smtp.SendMailAsync(message);

            _logger.LogInformation("E-mail enviado: {Subject} → {Recipients}",
                subject, string.Join(", ", _settings.Recipients));
        }
        catch (Exception ex)
        {
            // Nunca deixa falha de e-mail derrubar o serviço
            _logger.LogError(ex, "Falha ao enviar e-mail. Assunto: {Subject}", subject);
        }
    }
}
