using Dapper;
using MESSAGE.SERVICE.Models;
using Microsoft.Data.SqlClient;
using System.Data;

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
            await using var con = new SqlConnection(_connectionString);

            return await con.QueryAsync<MessageQueue>(
                "sp_msg_select_pending_messages",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task MarkAsSentAsync(long id)
        {
            await using var con = new SqlConnection(_connectionString);

            await con.ExecuteAsync(
                "sp_msg_update_message_sent",
                new { id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task MarkAsErrorAsync(long id, string error)
        {
            await using var con = new SqlConnection(_connectionString);

            await con.ExecuteAsync(
                "sp_msg_update_message_error",
                new { id = id, error = error },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}