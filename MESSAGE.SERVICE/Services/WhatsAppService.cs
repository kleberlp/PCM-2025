using Microsoft.Extensions.Options;
using MESSAGE.SERVICE.Settings;
using RestSharp;

namespace MESSAGE.SERVICE.Services
{
    public class WhatsAppService
    {
        private readonly WhatsAppSettings _settings;

        public WhatsAppService(IOptions<WhatsAppSettings> options)
        {
            _settings = options.Value;
        }

        public async Task<bool> SendAsync(string phone, string message)
        {
            var client = new RestClient(
                $"https://api.z-api.io/instances/{_settings.InstanceId}/token/{_settings.Token}/send-text"
            );

            var request = new RestRequest("", Method.Post);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("client-token", _settings.ClientToken);
            request.AddJsonBody(new { phone, message });

            var response = await client.ExecuteAsync(request);
            return response.IsSuccessful;
        }
    }
}
