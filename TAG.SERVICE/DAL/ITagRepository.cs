using TAG.SERVICE.Models;

namespace TAG.SERVICE.DAL
{
    public interface ITagRepository
    {
        Task<IEnumerable<TagPendente>> GetEquipamentosPendentesAsync();
        Task UpdateEquipamentoTagAsync(TagPendente item, byte[] imagem);

        Task<IEnumerable<TagPendente>> GetRotinasPendentesAsync();
        Task UpdateRotinaTagAsync(TagPendente item, byte[] imagem);

        Task<IEnumerable<TagApartamentoPendente>> GetApartamentosPendentesAsync();
        Task UpdateApartamentoTagAsync(string uniqueId, byte[] imagem);
    }
}
