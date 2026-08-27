using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using TAG.SERVICE.Models;

namespace TAG.SERVICE.DAL
{
    /// <summary>
    /// Mesmas stored procedures do PCM.SERVICE.TAG legado — nada muda no banco.
    /// </summary>
    public class TagRepository : ITagRepository
    {
        private readonly string _connectionString;

        public TagRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default")!;
        }

        public async Task<IEnumerable<TagPendente>> GetEquipamentosPendentesAsync()
        {
            await using var con = new SqlConnection(_connectionString);

            return await con.QueryAsync<TagPendente>(
                "sp_select_cadastro_basico_equipamento_tag",
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateEquipamentoTagAsync(TagPendente item, byte[] imagem)
        {
            await using var con = new SqlConnection(_connectionString);

            await con.ExecuteAsync(
                "sp_update_cadastro_basico_equipamento_tag",
                new
                {
                    codigo_empresa = item.CodigoEmpresa,
                    codigo_unidade = item.CodigoUnidade,
                    codigo = item.Codigo,
                    tag_imagem = imagem
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TagPendente>> GetRotinasPendentesAsync()
        {
            await using var con = new SqlConnection(_connectionString);

            return await con.QueryAsync<TagPendente>(
                "sp_select_pcm_programada_tag",
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateRotinaTagAsync(TagPendente item, byte[] imagem)
        {
            await using var con = new SqlConnection(_connectionString);

            await con.ExecuteAsync(
                "sp_update_pcm_programada_tag",
                new
                {
                    codigo_empresa = item.CodigoEmpresa,
                    codigo_unidade = item.CodigoUnidade,
                    codigo = item.Codigo,
                    tag_imagem = imagem
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TagApartamentoPendente>> GetApartamentosPendentesAsync()
        {
            await using var con = new SqlConnection(_connectionString);

            return await con.QueryAsync<TagApartamentoPendente>(
                "sp_select_pcm_os_hospede_apartamento_tag",
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateApartamentoTagAsync(string uniqueId, byte[] imagem)
        {
            await using var con = new SqlConnection(_connectionString);

            await con.ExecuteAsync(
                "sp_update_pcm_os_hospede_apartamento_tag",
                new
                {
                    uniqueId,
                    tag = imagem
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}
