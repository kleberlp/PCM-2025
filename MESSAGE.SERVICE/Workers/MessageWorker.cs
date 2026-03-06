using MESSAGE.SERVICE.DAL;
using MESSAGE.SERVICE.Enums;
using MESSAGE.SERVICE.Services;

namespace MESSAGE.SERVICE.Workers
{
    public class MessageWorker : BackgroundService
    {
        private readonly IMessageRepository _repo;
        private readonly WhatsAppService _whatsApp;
        private readonly EmailService _email;
        private readonly IConfiguration _config;

        public MessageWorker(
            IMessageRepository repo,
            WhatsAppService whatsApp,
            EmailService email,
            IConfiguration config)
        {
            _repo = repo;
            _whatsApp = whatsApp;
            _email = email;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var delay = TimeSpan.FromSeconds(
                _config.GetValue<int>("Worker:IntervalSeconds")
            );

            while (!stoppingToken.IsCancellationRequested)
            {
                var messages = await _repo.GetPendingMessagesAsync();

                foreach (var msg in messages)
                {
                    try
                    {
                        switch (msg.Type)
                        {
                            case "whatsapp":
                                await _whatsApp.SendAsync(msg.Phone!, msg.Body);
                                await _repo.MarkAsSentAsync(msg.Id);
                                break;

                            case "email":
                                _email.Send(msg.Email!, msg.Subject!, msg.Body);
                                await _repo.MarkAsSentAsync(msg.Id);
                                break;
                        }

                    }
                    catch (Exception ex)
                    {
                        await _repo.MarkAsErrorAsync(msg.Id, ex.Message);
                    }
                }

                await Task.Delay(delay, stoppingToken);
            }
        }
    }
}
