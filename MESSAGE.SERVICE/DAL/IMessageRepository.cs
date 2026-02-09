using MESSAGE.SERVICE.Models;

namespace MESSAGE.SERVICE.DAL
{
    public interface IMessageRepository
    {
        Task<IEnumerable<MessageQueue>> GetPendingMessagesAsync();
        Task MarkAsSentAsync(long id);
        Task MarkAsErrorAsync(long id, string error);
    }
}
