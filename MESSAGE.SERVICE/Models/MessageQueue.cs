using MESSAGE.SERVICE.Enums;

namespace MESSAGE.SERVICE.Models
{
    public class MessageQueue
    {
        public long Id { get; set; }                 // id_mensageiro
        public string? Type { get; set; }        // email / whatsapp

        public string? Phone { get; set; }           // resolvido no SQL
        public string? Email { get; set; }

        public string? Subject { get; set; }
        public string Body { get; set; } = default!;
    }
}
