using Dapper;
using MESSAGE.SERVICE.Models;
using MySqlConnector;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MESSAGE.SERVICE.DAL
{
    public class MessageRepository : IMessageRepository
    {
        private readonly string _connectionString;

        public MessageRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default")!;
        }

        public async Task<IEnumerable<MessageQueue>> GetPendingMessagesAsync()
        {
            await using var con = new MySqlConnection(_connectionString);

            return await con.QueryAsync<MessageQueue>(
                "sp_select_pending_messages",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task MarkAsSentAsync(long id)
        {
            await using var con = new MySqlConnection(_connectionString);

            await con.ExecuteAsync(
                "sp_update_message_sent",
                new { p_id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task MarkAsErrorAsync(long id, string error)
        {
            await using var con = new MySqlConnection(_connectionString);

            await con.ExecuteAsync(
                "sp_update_message_error",
                new { p_id = id, p_error =  error },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
